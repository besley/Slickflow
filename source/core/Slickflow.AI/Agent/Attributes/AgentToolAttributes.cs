using System;

namespace Slickflow.AI.Agent.Attributes
{
    /// <summary>
    /// Marks a class as a named tool set for an Agent node.
    /// The Name must match the toolSetName attribute set in the BPMN designer.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AgentToolSetAttribute : Attribute
    {
        public string Name { get; }
        public AgentToolSetAttribute(string name) => Name = name;
    }

    /// <summary>
    /// Marks a public method as an agent tool exposed to the LLM for function calling.
    /// The tool name defaults to the method name converted to snake_case.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AgentToolAttribute : Attribute
    {
        public string  Description { get; }

        /// <summary>
        /// Optional override for the tool name sent to the LLM.
        /// Defaults to the method name in snake_case (e.g. SearchSuppliers → search_suppliers).
        /// </summary>
        public string? Name { get; init; }

        public AgentToolAttribute(string description) => Description = description;
    }
}
