using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Slickflow.Graph.Roslyn
{
    /// <summary>
    /// Code Security Validator
    /// 代码安全验证器
    /// </summary>
    public static class CodeSecurityValidator
    {
        /// <summary>
        /// Allowed namespace prefixes
        /// 允许的命名空间前缀
        /// </summary>
        private static readonly HashSet<string> AllowedNamespacePrefixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Slickflow.Graph.Model",
            "Slickflow.Engine.Common"
        };

        /// <summary>
        /// Required namespaces (must be in first two non-empty lines)
        /// 必需的命名空间（必须在前两个非空行中）
        /// </summary>
        private static readonly HashSet<string> RequiredNamespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "using Slickflow.Graph.Model;",
            "using Slickflow.Engine.Common;"
        };

        /// <summary>
        /// Validate code namespace requirements
        /// 验证代码命名空间要求
        /// </summary>
        /// <param name="codeText">Code text to validate</param>
        /// <returns>Validation result</returns>
        public static ValidationResult Validate(string codeText)
        {
            if (string.IsNullOrWhiteSpace(codeText))
            {
                return ValidationResult.Error("Code text is empty or invalid.");
            }

            // Normalize line endings and split into lines
            // 标准化换行符并分割成行
            var lines = codeText.Replace("\r\n", "\n").Replace("\r", "\n")
                .Split('\n')
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrEmpty(line))
                .ToList();

            if (lines.Count < 2)
            {
                return ValidationResult.Error("Code must contain at least two non-empty lines with required namespace declarations.");
            }

            // Check first two lines for required namespaces
            // 检查前两行是否包含必需的命名空间
            var firstLine = lines[0];
            var secondLine = lines[1];

            var hasFirstNamespace = false;
            var hasSecondNamespace = false;

            if (RequiredNamespaces.Contains(firstLine))
            {
                if (firstLine.Equals("using Slickflow.Graph.Model;", StringComparison.OrdinalIgnoreCase))
                {
                    hasFirstNamespace = true;
                }
                else if (firstLine.Equals("using Slickflow.Engine.Common;", StringComparison.OrdinalIgnoreCase))
                {
                    hasSecondNamespace = true;
                }
            }

            if (RequiredNamespaces.Contains(secondLine))
            {
                if (secondLine.Equals("using Slickflow.Graph.Model;", StringComparison.OrdinalIgnoreCase))
                {
                    hasFirstNamespace = true;
                }
                else if (secondLine.Equals("using Slickflow.Engine.Common;", StringComparison.OrdinalIgnoreCase))
                {
                    hasSecondNamespace = true;
                }
            }

            if (!hasFirstNamespace || !hasSecondNamespace)
            {
                return ValidationResult.Error(
                    "Code must start with the following two namespace declarations:\n" +
                    "  using Slickflow.Graph.Model;\n" +
                    "  using Slickflow.Engine.Common;");
            }

            // Check all using statements for unauthorized namespaces
            // 检查所有 using 语句是否有未授权的命名空间
            var usingPattern = new Regex(@"^\s*using\s+([^;]+);", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var matches = usingPattern.Matches(codeText);

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    var namespaceName = match.Groups[1].Value.Trim();
                    if (!IsNamespaceAllowed(namespaceName))
                    {
                        return ValidationResult.Error(
                            $"Security violation: Unauthorized namespace \"{namespaceName}\" is not allowed. " +
                            "Only namespaces starting with \"Slickflow.Graph.Model\" or \"Slickflow.Engine.Common\" are permitted.");
                    }
                }
            }

            // Security check: Only Workflow methods are allowed
            // 安全检查：只允许 Workflow 方法调用
            var securityCheckResult = ValidateOnlyWorkflowMethods(codeText);
            if (!securityCheckResult.IsValid)
            {
                return securityCheckResult;
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validate that code only contains Workflow method calls
        /// 验证代码只包含 Workflow 方法调用
        /// </summary>
        /// <param name="codeText">Code text to validate</param>
        /// <returns>Validation result</returns>
        private static ValidationResult ValidateOnlyWorkflowMethods(string codeText)
        {
            // Remove using statements and comments for analysis
            // 移除 using 语句和注释以便分析
            var codeWithoutUsings = RemoveUsingStatementsAndComments(codeText);
            
            // Check for forbidden patterns
            // 检查禁止的模式
            
            // 1. Check for System. calls (except allowed namespaces)
            // 检查 System. 调用（除了允许的命名空间）
            var systemCallPattern = new Regex(@"\bSystem\.\w+", RegexOptions.IgnoreCase);
            var systemMatches = systemCallPattern.Matches(codeWithoutUsings);
            foreach (Match match in systemMatches)
            {
                var systemCall = match.Value;
                // Allow System in comments or string literals, but check actual code
                // 允许注释或字符串字面量中的 System，但检查实际代码
                if (!IsInCommentOrString(codeText, match.Index))
                {
                    return ValidationResult.Error(
                        $"Security violation: Unauthorized System call \"{systemCall}\" is not allowed. " +
                        "Only Workflow method calls are permitted.");
                }
            }

            // 2. Check for variable declarations other than 'var wf = new Workflow(...)' or 'var wf = Workflow.LoadProcess(...)'
            // 检查除了 'var wf = new Workflow(...)' 或 'var wf = Workflow.LoadProcess(...)' 之外的其他变量声明
            var variablePattern = new Regex(@"\b(string|int|bool|double|float|object|var)\s+\w+\s*=", RegexOptions.IgnoreCase);
            var variableMatches = variablePattern.Matches(codeWithoutUsings);
            foreach (Match match in variableMatches)
            {
                var variableDecl = match.Value;
                // Check if it's the allowed Workflow declaration
                // 检查是否是允许的 Workflow 声明
                var contextStart = Math.Max(0, match.Index - 100);
                var contextLength = Math.Min(200, codeWithoutUsings.Length - contextStart);
                var context = codeWithoutUsings.Substring(contextStart, contextLength);
                
                if (!context.Contains("new Workflow", StringComparison.OrdinalIgnoreCase) &&
                    !context.Contains("Workflow.LoadProcess", StringComparison.OrdinalIgnoreCase))
                {
                    return ValidationResult.Error(
                        $"Security violation: Unauthorized variable declaration \"{variableDecl.Trim()}\" is not allowed. " +
                        "Only 'var wf = new Workflow(...)' or 'var wf2 = Workflow.LoadProcess(...)' is permitted.");
                }
            }

            // 3. Check for method calls that are not Workflow methods
            // 检查不是 Workflow 方法的方法调用
            // Pattern: identifier followed by ( or .identifier(
            // 模式：标识符后跟 ( 或 .标识符(
            var methodCallPattern = new Regex(@"\b\w+\s*\(", RegexOptions.IgnoreCase);
            var methodMatches = methodCallPattern.Matches(codeWithoutUsings);
            
            var allowedMethodNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                // Workflow class name (for new Workflow(...) and Workflow.LoadProcess(...))
                "Workflow",
                // Workflow static methods
                "CreateProcess",
                "LoadProcess",
                "Create",
                // Workflow instance methods
                "Start",
                "Task",
                "End",
                "AndSplit",
                "AndJoin",
                "XOrSplit",
                "XOrJoin",
                "Parallels",
                "Branch",
                "Connect",  // Chain method for Workflow
                "Build",
                "Update",
                "Add",
                "Insert",
                "Set",
                "Replace",
                "Exchange",
                "Fork",
                "Remove",
                "GetBuilder",
                "SetUrl",
                "SetName",
                "Serialize",
                // NodeBuilder methods
                "NodeBuilder",
                "CreateTask",
                "CreateStart",
                "CreateEnd",
                "CreateGateway",
                // EdgeBuilder methods
                "EdgeBuilder",
                "CreateEdge",
                "AddCondition",
                // Variable names
                "wf",   // Variable name
                "wf2"   // Variable name (for multiple Workflow instances)
            };

            foreach (Match match in methodMatches)
            {
                var methodName = match.Value.TrimEnd('(').Trim();
                
                // Skip if it's in a comment or string
                // 如果在注释或字符串中则跳过
                if (IsInCommentOrString(codeText, match.Index))
                {
                    continue;
                }

                // Check if it's an allowed method or part of Workflow chain
                // 检查是否是允许的方法或 Workflow 链的一部分
                if (!allowedMethodNames.Contains(methodName))
                {
                    // Check if it's part of a Workflow chain (e.g., wf.Start)
                    // 检查是否是 Workflow 链的一部分（例如，wf.Start）
                    var beforeMatch = Math.Max(0, match.Index - 50);
                    var contextBefore = codeWithoutUsings.Substring(beforeMatch, match.Index - beforeMatch);
                    
                    // Check if it's part of a Workflow chain (e.g., wf.Start, new Workflow(...), Workflow.LoadProcess(...))
                    // 检查是否是 Workflow 链的一部分（例如，wf.Start, new Workflow(...), Workflow.LoadProcess(...)）
                    if (!contextBefore.Contains("wf", StringComparison.OrdinalIgnoreCase) &&
                           !contextBefore.Contains("wf2", StringComparison.OrdinalIgnoreCase) &&
                           !contextBefore.Contains("new Workflow", StringComparison.OrdinalIgnoreCase) &&
                           !contextBefore.Contains("Workflow.", StringComparison.OrdinalIgnoreCase) &&
                           !contextBefore.Contains("Workflow(", StringComparison.OrdinalIgnoreCase) &&
                           !contextBefore.Contains("NodeBuilder", StringComparison.OrdinalIgnoreCase) &&
                           !contextBefore.Contains("EdgeBuilder", StringComparison.OrdinalIgnoreCase))
                    {
                        return ValidationResult.Error(
                            $"Security violation: Unauthorized method call \"{methodName}\" is not allowed. " +
                            "Only Workflow method calls are permitted.");
                    }
                }
            }

            // 4. Check for foreach, for, while, if, etc. control structures
            // 检查 foreach, for, while, if 等控制结构
            // Note: Use negative lookahead/lookbehind to avoid matching "for" in "Fork" method name
            // 注意：使用负向前瞻/后顾来避免匹配 "Fork" 方法名中的 "for"
            var controlStructurePattern = new Regex(@"\b(foreach|(?<![a-zA-Z])for(?![a-zA-Z])|while|if|switch|try|catch|finally|using\s*\()", RegexOptions.IgnoreCase);
            var controlMatches = controlStructurePattern.Matches(codeWithoutUsings);
            foreach (Match match in controlMatches)
            {
                if (!IsInCommentOrString(codeText, match.Index))
                {
                    // Double-check: if matched "for", make sure it's not part of "Fork" method
                    // 双重检查：如果匹配到 "for"，确保它不是 "Fork" 方法的一部分
                    if (match.Value.Equals("for", StringComparison.OrdinalIgnoreCase))
                    {
                        var matchIndex = match.Index;
                        var matchLength = match.Length;
                        // Check if it's followed by 'k' (part of "Fork") or preceded/followed by other letters
                        // 检查它后面是否是 'k'（"Fork" 的一部分）或前后是否有其他字母
                        if (matchIndex + matchLength < codeWithoutUsings.Length)
                        {
                            var nextChar = codeWithoutUsings[matchIndex + matchLength];
                            if (char.IsLetter(nextChar) && char.ToLowerInvariant(nextChar) == 'k')
                            {
                                // This is part of "Fork", skip it
                                // 这是 "Fork" 的一部分，跳过它
                                continue;
                            }
                        }
                    }
                    
                    return ValidationResult.Error(
                        $"Security violation: Control structure \"{match.Value}\" is not allowed. " +
                        "Only Workflow method calls are permitted.");
                }
            }

            // 5. Check for class, struct, interface, enum declarations
            // 检查 class, struct, interface, enum 声明
            var typeDeclarationPattern = new Regex(@"\b(class|struct|interface|enum|namespace)\s+\w+", RegexOptions.IgnoreCase);
            var typeMatches = typeDeclarationPattern.Matches(codeWithoutUsings);
            foreach (Match match in typeMatches)
            {
                if (!IsInCommentOrString(codeText, match.Index))
                {
                    return ValidationResult.Error(
                        $"Security violation: Type declaration \"{match.Value}\" is not allowed. " +
                        "Only Workflow method calls are permitted.");
                }
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Remove using statements and comments from code
        /// 从代码中移除 using 语句和注释
        /// </summary>
        private static string RemoveUsingStatementsAndComments(string codeText)
        {
            var result = codeText;

            // Remove single-line comments
            // 移除单行注释
            result = Regex.Replace(result, @"//.*", string.Empty);

            // Remove multi-line comments
            // 移除多行注释
            result = Regex.Replace(result, @"/\*.*?\*/", string.Empty, RegexOptions.Singleline);

            // Remove using statements
            // 移除 using 语句
            result = Regex.Replace(result, @"^\s*using\s+[^;]+;\s*$", string.Empty, RegexOptions.Multiline);

            return result;
        }

        /// <summary>
        /// Check if position is inside a comment or string literal
        /// 检查位置是否在注释或字符串字面量中
        /// </summary>
        private static bool IsInCommentOrString(string codeText, int position)
        {
            // Simple check: look for // or /* before position on same line, or string quotes
            // 简单检查：在同一行查找 // 或 /*，或字符串引号
            var beforePosition = codeText.Substring(0, position);
            var lineStart = beforePosition.LastIndexOf('\n') + 1;
            var lineContent = beforePosition.Substring(lineStart);
            
            // Check for single-line comment
            // 检查单行注释
            if (lineContent.Contains("//"))
            {
                return true;
            }

            // Check for string literals (simplified)
            // 检查字符串字面量（简化版）
            var quoteCount = 0;
            var inString = false;
            for (int i = 0; i < position; i++)
            {
                if (codeText[i] == '"' && (i == 0 || codeText[i - 1] != '\\'))
                {
                    quoteCount++;
                    inString = !inString;
                }
            }
            
            return inString;
        }

        /// <summary>
        /// Check if namespace is allowed
        /// 检查命名空间是否允许
        /// </summary>
        /// <param name="namespaceName">Namespace name to check</param>
        /// <returns>True if allowed, false otherwise</returns>
        private static bool IsNamespaceAllowed(string namespaceName)
        {
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                return false;
            }

            // Check if namespace matches any allowed prefix
            // 检查命名空间是否匹配任何允许的前缀
            foreach (var allowedPrefix in AllowedNamespacePrefixes)
            {
                if (namespaceName.Equals(allowedPrefix, StringComparison.OrdinalIgnoreCase) ||
                    namespaceName.StartsWith(allowedPrefix + ".", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validation result
        /// 验证结果
        /// </summary>
        public class ValidationResult
        {
            public bool IsValid { get; private set; }
            public string Message { get; private set; }

            private ValidationResult(bool isValid, string message)
            {
                IsValid = isValid;
                Message = message;
            }

            public static ValidationResult Success()
            {
                return new ValidationResult(true, "Code validation passed.");
            }

            public static ValidationResult Error(string message)
            {
                return new ValidationResult(false, message);
            }
        }
    }
}
