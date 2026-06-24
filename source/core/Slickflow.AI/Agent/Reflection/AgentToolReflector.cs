using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Slickflow.AI.Agent.Attributes;
using Slickflow.AI.Agent.Core;
using Slickflow.AI.Agent.Tools;

namespace Slickflow.AI.Agent.Reflection
{
    /// <summary>
    /// Converts a class decorated with [AgentToolSet] / [AgentTool] into
    /// a list of IAgentTool instances ready for the ReAct loop.
    ///
    /// Handles:
    ///   - Async methods (Task / Task&lt;T&gt;) and synchronous methods
    ///   - Primitive parameters: string, int, long, double, float, decimal, bool
    ///   - Collection parameters: string[], List&lt;T&gt;
    ///   - Complex object parameters: deserialized via System.Text.Json
    ///   - CancellationToken parameters: injected automatically, not from LLM args
    ///   - Optional parameters (with default values): excluded from "required" in schema
    /// </summary>
    public static class AgentToolReflector
    {
        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Scans <typeparamref name="T"/> for [AgentTool]-decorated public methods
        /// and returns a ready-to-use tool list.
        /// </summary>
        public static IReadOnlyList<IAgentTool> BuildToolsFromType<T>() where T : class, new()
            => BuildToolsFromType(typeof(T));

        /// <summary>
        /// Same as <see cref="BuildToolsFromType{T}"/> but accepts a runtime Type.
        /// </summary>
        public static IReadOnlyList<IAgentTool> BuildToolsFromType(Type serviceType)
        {
            var instance = Activator.CreateInstance(serviceType)
                ?? throw new InvalidOperationException(
                    $"Cannot create instance of {serviceType.Name}. Ensure it has a public parameterless constructor.");

            var tools = new List<IAgentTool>();

            foreach (var method in serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var attr = method.GetCustomAttribute<AgentToolAttribute>();
                if (attr == null) continue;

                var toolName  = attr.Name ?? ToSnakeCase(method.Name);
                var schema    = BuildSchema(method);
                var captured  = method; // capture for lambda

                var tool = new FunctionCallingTool(
                    toolName,
                    attr.Description,
                    schema,
                    async (args, ct) => await InvokeAsync(instance, captured, args, ct));

                tools.Add(tool);
            }

            return tools.AsReadOnly();
        }

        // ── Schema generation ────────────────────────────────────────────────────

        private static JsonObject BuildSchema(MethodInfo method)
        {
            var properties = new JsonObject();
            var required   = new JsonArray();

            foreach (var param in method.GetParameters())
            {
                if (param.ParameterType == typeof(CancellationToken)) continue;

                properties[param.Name!] = BuildTypeSchema(param.ParameterType);

                if (!param.HasDefaultValue)
                    required.Add(JsonValue.Create(param.Name!));
            }

            return new JsonObject
            {
                ["type"]       = "object",
                ["properties"] = properties,
                ["required"]   = required
            };
        }

        private static JsonNode BuildTypeSchema(Type type)
        {
            if (type == typeof(string))  return new JsonObject { ["type"] = "string" };
            if (type == typeof(int)  || type == typeof(long))  return new JsonObject { ["type"] = "integer" };
            if (type == typeof(float) || type == typeof(double) || type == typeof(decimal))
                return new JsonObject { ["type"] = "number" };
            if (type == typeof(bool)) return new JsonObject { ["type"] = "boolean" };

            if (type.IsArray)
                return new JsonObject { ["type"] = "array", ["items"] = BuildTypeSchema(type.GetElementType()!) };

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return new JsonObject { ["type"] = "array", ["items"] = BuildTypeSchema(type.GetGenericArguments()[0]) };

            return new JsonObject { ["type"] = "object" };
        }

        // ── Method invocation ────────────────────────────────────────────────────

        private static async Task<AgentToolResult> InvokeAsync(
            object          instance,
            MethodInfo      method,
            JsonObject      args,
            CancellationToken ct)
        {
            var parameters = method.GetParameters();
            var argValues  = new object?[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];

                if (param.ParameterType == typeof(CancellationToken))
                {
                    argValues[i] = ct;
                    continue;
                }

                if (args.TryGetPropertyValue(param.Name!, out var node) && node != null)
                {
                    argValues[i] = BindValue(node, param.ParameterType);
                }
                else if (param.HasDefaultValue)
                {
                    argValues[i] = param.DefaultValue;
                }
                else
                {
                    argValues[i] = param.ParameterType.IsValueType
                        ? Activator.CreateInstance(param.ParameterType)
                        : null;
                }
            }

            object? rawResult;
            try
            {
                rawResult = method.Invoke(instance, argValues);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException ?? tie;
            }

            // Await async methods
            if (rawResult is Task task)
            {
                await task.ConfigureAwait(false);

                var taskType = task.GetType();
                if (taskType.IsGenericType)
                {
                    var resultProp = taskType.GetProperty("Result")!;
                    return ToToolResult(resultProp.GetValue(task));
                }
                return new AgentToolResult(true, "OK");
            }

            return ToToolResult(rawResult);
        }

        // ── Value binding (JsonNode → typed parameter) ───────────────────────────

        private static object? BindValue(JsonNode? node, Type targetType)
        {
            if (node == null)
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

            if (targetType == typeof(string))  return node.GetValue<string>();
            if (targetType == typeof(int))     return node.GetValue<int>();
            if (targetType == typeof(long))    return node.GetValue<long>();
            if (targetType == typeof(double))  return node.GetValue<double>();
            if (targetType == typeof(float))   return (float)node.GetValue<double>();
            if (targetType == typeof(decimal)) return (decimal)node.GetValue<double>();
            if (targetType == typeof(bool))    return node.GetValue<bool>();

            if (targetType.IsArray)
            {
                var elemType = targetType.GetElementType()!;
                var arr      = node.AsArray();
                var result   = Array.CreateInstance(elemType, arr.Count);
                for (int i = 0; i < arr.Count; i++)
                    result.SetValue(BindValue(arr[i], elemType), i);
                return result;
            }

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elemType = targetType.GetGenericArguments()[0];
                var arr      = node.AsArray();
                var list     = (IList)Activator.CreateInstance(targetType)!;
                foreach (var item in arr)
                    list.Add(BindValue(item, elemType));
                return list;
            }

            // Complex object: deserialize via System.Text.Json
            return JsonSerializer.Deserialize(node.ToJsonString(), targetType, _jsonOpts);
        }

        // ── Return value serialization ────────────────────────────────────────────

        private static AgentToolResult ToToolResult(object? value)
        {
            if (value == null)         return new AgentToolResult(true, "null");
            if (value is string s)     return new AgentToolResult(true, s);
            if (value is bool   b)     return new AgentToolResult(b, b.ToString());

            return new AgentToolResult(true, JsonSerializer.Serialize(value, _jsonOpts));
        }

        // ── Helper: PascalCase → snake_case ──────────────────────────────────────

        private static string ToSnakeCase(string name)
        {
            var sb = new StringBuilder(name.Length + 4);
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (i > 0 && char.IsUpper(c))
                    sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }
    }
}
