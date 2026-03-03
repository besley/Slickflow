using System;
using System.Text.RegularExpressions;
using Slickflow.Engine.Service;
using Slickflow.Engine.Xpdl.Template;

namespace Slickflow.Graph.Roslyn
{
    /// <summary>
    /// Roslyn Builder Class
    /// </summary>
    public class RoslynBuilder
    {
        /// <summary> 
        /// Execute script code
        /// 执行脚本代码
        /// </summary>
        public RoslynExecuteResult Execute(ProcessGraph graph)
        {
            var result = RoslynExecuteResult.Default();
            var body = graph.Body;

            // Security validation: Check namespace requirements
            // 安全验证：检查命名空间要求
            var validationResult = CodeSecurityValidator.Validate(body);
            if (!validationResult.IsValid)
            {
                return RoslynExecuteResult.Error($"Security validation failed: {validationResult.Message}");
            }

            string methodPlaceholder = "method";
            string parametersPlaceholder = "parameters";
            // Match: new Workflow("name", "code") or Workflow.CreateProcess("name", "code") or CreateProcess("name", "code")
            // Support both new Workflow(...) and static method calls for backward compatibility
            string strCreatePattern = string.Format(@"(?:new\s+Workflow|Workflow\.CreateProcess|CreateProcess)\s*\((?<{0}>[^)]*)\)", parametersPlaceholder);
            // Match: Workflow.LoadProcess("code", "version") or LoadProcess("code", "version")
            string strLoadPattern = string.Format(@"(?:Workflow\.)?LoadProcess\s*\((?<{0}>[^)]*)\)", parametersPlaceholder);
            string strDeletePattern = string.Format(@"(?<{0}>DeleteProcess)(\((?<{1}>[^)]*)\))?", methodPlaceholder, parametersPlaceholder);

            ProcessGraph processGraph = null;
            var isCreateMatched = Regex.IsMatch(body, strCreatePattern, RegexOptions.IgnoreCase);
            if (isCreateMatched)
            {
                processGraph = GetProcessGraphToCreate(body, strCreatePattern, parametersPlaceholder);
                result = CreateProcess(processGraph, body); 
                return result;
            }

            var isLoadMatched = Regex.IsMatch(body, strLoadPattern, RegexOptions.IgnoreCase);
            if (isLoadMatched)
            {
                processGraph = GetProcessGraphToLoad(body, strLoadPattern, parametersPlaceholder);
                result = UpdateProcess(processGraph, body);
                return result;
            }

            var isDeleteMatched = Regex.IsMatch(body, strDeletePattern);
            if (isDeleteMatched)
            {
                //result = DeleteProcess(processCode, processVersion, body);
                return result;
            }
            //流程名称没有输入或者不能被识别
            //Process name not inputed or cannot be recognized
            result = new RoslynExecuteResult { Status = -1, Message = "Process name is not inputed or recognized!" };
            return result;
        }

        /// <summary>
        /// Generate graphic entity objects based on graphic scripts
        /// 根据图形脚本生成图形实体对象
        /// </summary>
        private ProcessGraph GetProcessGraphToCreate(string body, string strPattern, string parametersPlaceholder)
        {
            string processName = string.Empty;
            string processCode = string.Empty;
            string processVersion = string.Empty;
            string parameters = string.Empty;

            Regex regex = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = regex.Match(body);
            parameters = match.Groups[parametersPlaceholder].Value;
            parameters = Regex.Replace(parameters, @"\s+", "");
            string[] pts = parameters.Split(',');

            //缺少参数列表的方法,程序不能被执行!
            //Method without parameter list, program cannot be executed!
            if (pts.Count() == 0) throw new ApplicationException("The method is missing argument list, program can not be executed!");
            if (pts.Count() >= 1)
            {
                processName = pts[0];
                processName = processName.Remove(0, 1);
                processName = processName.Remove(processName.Length - 1, 1);
            }
            if (pts.Count() >= 2)
            {
                processCode = pts[1];
                processCode = processCode.Remove(0, 1);
                processCode = processCode.Remove(processCode.Length - 1, 1);
            }
            if (pts.Count() >= 3)
            {
                processVersion = pts[2];
                processVersion = processVersion.Remove(0, 1);
                processVersion = processVersion.Remove(processVersion.Length - 1, 1);
            }

            processVersion = "1";
            var entity = new ProcessGraph { Name = processName, Code = processCode, Version = processVersion };
            return entity;
        }

        /// <summary>
        /// Create Process by Executing C# Text Script Code
        /// </summary>
        /// <param name="graph">Graph Object</param>
        /// <returns>Roslyn Executed Result</returns>
        public RoslynExecuteResult CreateProcess(ProcessGraph graph, string body)
        {
            var result = RoslynExecuteResult.Default();
            var wfService = new WorkflowService();
            var process = wfService.GetProcessByName(graph.Name, graph.Version);

            if (process == null)
            {
                result = RoslynHotSpot.Execute(body).Result;
                if (result.Status == 1)
                {
                    process = wfService.GetProcessByName(graph.Name, graph.Version);
                    result.Process = process;
                }
            }
            else
            {
                result = new RoslynExecuteResult { Status = -1, Message = "The process with same name and version already exist!" };
            }
            return result;
        }

        /// <summary>
        /// Generate graphic entity objects based on graphic scripts
        /// 根据图形脚本生成图形实体对象
        /// </summary>
        private ProcessGraph GetProcessGraphToLoad(string body, string strPattern, string parametersPlaceholder)
        {
            string processName = string.Empty;
            string processCode = string.Empty;
            string processVersion = string.Empty;
            string parameters = string.Empty;

            Regex regex = new Regex(strPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var match = regex.Match(body);
            parameters = match.Groups[parametersPlaceholder].Value;
            parameters = Regex.Replace(parameters, @"\s+", "");
            string[] pts = parameters.Split(',');

            //缺少参数列表的方法,程序不能被执行!
            //Method without parameter list, program cannot be executed!
            if (pts.Count() == 0) throw new ApplicationException("The method is missing argument list, program can not be executed!");
            if (pts.Count() >= 1)
            {
                processCode = pts[0];
                processCode = processCode.Remove(0, 1);
                processCode = processCode.Remove(processCode.Length - 1, 1);
            }
            if (pts.Count() >= 2)
            {
                processVersion = pts[1];
                processVersion = processVersion.Remove(0, 1);
                processVersion = processVersion.Remove(processVersion.Length - 1, 1);
            }

            var entity = new ProcessGraph { Code = processCode, Version = processVersion };
            return entity;
        }

        /// <summary>
        /// Update Process by Executing C# Text Script Code
        /// </summary>
        /// <param name="graph">Graph Object</param>
        /// <returns>Roslyn Executed Result</returns>
        public RoslynExecuteResult UpdateProcess(ProcessGraph graph, string body)
        {
            var result = RoslynExecuteResult.Default();
            var wfService = new WorkflowService();
            var process = wfService.GetProcessByCode(graph.Code, graph.Version);

            if (process != null)
            {
                result = RoslynHotSpot.Execute(body).Result;
                if (result.Status == 1)
                {
                    process = wfService.GetProcessByCode(graph.Code, graph.Version);
                    result.Process = process;
                }
            }
            else
            {
                result = new RoslynExecuteResult { Status = -1, Message = "The process record is not exist!" };
            }
            return result;
        }
    }
}
