using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Utility;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 将历史流程 XML（wf_process.xml_content）中节点上的 <c>sf:variables/sf:variable</c> 迁移到 <c>wf_variable</c> 表。
    /// </summary>
    public static class ProcessXmlVariableMigrator
    {
        public sealed class MigrationResult
        {
            public int ProcessRowsScanned { get; set; }
            public int ProcessRowsWithVariableXml { get; set; }
            public int ActivitiesMigrated { get; set; }
            public int ActivitiesSkippedExistingDb { get; set; }
            public int ActivitiesSkippedNoVariables { get; set; }
            public IList<string> Errors { get; } = new List<string>();

            public override string ToString()
            {
                return
                    $"Scanned={ProcessRowsScanned}, WithVariableXml={ProcessRowsWithVariableXml}, " +
                    $"MigratedActivities={ActivitiesMigrated}, SkippedExisting={ActivitiesSkippedExistingDb}, " +
                    $"SkippedEmpty={ActivitiesSkippedNoVariables}, Errors={Errors.Count}";
            }
        }

        /// <summary>
        /// 遍历 <c>wf_process</c> 全表，把 XML 中的变量定义写入 <c>wf_variable</c>。
        /// </summary>
        /// <param name="skipActivityIfWfVariableExists">为 true 时，若该流程版本下某活动已有 wf_variable 记录则跳过（避免覆盖手工数据）。为 false 时按 XML 覆盖该活动变量。</param>
        public static MigrationResult MigrateFromDatabase(bool skipActivityIfWfVariableExists = true)
        {
            var result = new MigrationResult();
            var pm = new ProcessManager();
            List<ProcessEntity> all;
            try
            {
                all = pm.GetAll();
            }
            catch (Exception ex)
            {
                result.Errors.Add("ProcessManager.GetAll: " + ex.Message);
                return result;
            }

            foreach (var entity in all)
            {
                result.ProcessRowsScanned++;
                try
                {
                    if (string.IsNullOrWhiteSpace(entity.XmlContent))
                    {
                        continue;
                    }

                    var xml = entity.XmlContent;
                    if (xml.IndexOf("sf:variable", StringComparison.OrdinalIgnoreCase) < 0
                        && xml.IndexOf("sf:variables", StringComparison.OrdinalIgnoreCase) < 0)
                    {
                        continue;
                    }

                    result.ProcessRowsWithVariableXml++;
                    var version = string.IsNullOrWhiteSpace(entity.Version) ? "1" : entity.Version;
                    MigrateOneProcessEntity(entity, version, skipActivityIfWfVariableExists, result);
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"ProcessId={entity.ProcessId}, Version={entity.Version}: {ex.Message}");
                }
            }

            return result;
        }

        /// <summary>
        /// 迁移单条流程记录（可单元测试或单独调用）。
        /// </summary>
        public static void MigrateOneProcessEntity(
            ProcessEntity entity,
            string version,
            bool skipActivityIfWfVariableExists,
            MigrationResult result)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(entity.XmlContent);
            var nsmgr = XPDLHelper.GetSlickflowXmlNamespaceManager(xmlDoc, true);

            var processNodes = xmlDoc.SelectNodes("//bpmn:process", nsmgr);
            if (processNodes == null || processNodes.Count == 0)
            {
                return;
            }

            var vm = new VariableManager();
            var processIdForRow = entity.ProcessId;

            for (var pi = 0; pi < processNodes.Count; pi++)
            {
                var processNode = processNodes[pi];
                // 多泳道多 process 时，用 XML 中 process 的 id；单 process 时与 entity.ProcessId 一致即可
                var xmlProcessId = XMLHelper.GetXmlAttribute(processNode, "id");
                var effectiveProcessId = processNodes.Count == 1
                    ? processIdForRow
                    : (string.IsNullOrEmpty(xmlProcessId) ? processIdForRow : xmlProcessId);

                foreach (var activityNode in EnumerateActivityNodes(processNode, nsmgr))
                {
                    var activityId = XMLHelper.GetXmlAttribute(activityNode, "id");
                    if (string.IsNullOrEmpty(activityId))
                    {
                        continue;
                    }

                    var variablesNode = activityNode.SelectSingleNode(XPDLDefinition.Sf_StrXmlPath_Variables, nsmgr);
                    if (variablesNode == null)
                    {
                        continue;
                    }

                    var variableNodes = variablesNode.SelectNodes("sf:variable", nsmgr);
                    if (variableNodes == null || variableNodes.Count == 0)
                    {
                        result.ActivitiesSkippedNoVariables++;
                        continue;
                    }

                    if (skipActivityIfWfVariableExists)
                    {
                        var existing = vm.GetList(effectiveProcessId, version, activityId);
                        if (existing != null && existing.Count > 0)
                        {
                            result.ActivitiesSkippedExistingDb++;
                            continue;
                        }
                    }

                    var list = new List<VariableEntity>();
                    for (var i = 0; i < variableNodes.Count; i++)
                    {
                        var vn = variableNodes[i];
                        var ve = ParseVariableElement(vn, nsmgr, effectiveProcessId, version, activityId, i);
                        if (ve != null)
                        {
                            list.Add(ve);
                        }
                    }

                    if (list.Count == 0)
                    {
                        result.ActivitiesSkippedNoVariables++;
                        continue;
                    }

                    vm.SaveList(effectiveProcessId, version, activityId, list);
                    result.ActivitiesMigrated++;
                }
            }
        }

        private static IEnumerable<XmlNode> EnumerateActivityNodes(XmlNode processNode, XmlNamespaceManager nsmgr)
        {
            var types = new[]
            {
                "task", "userTask", "serviceTask", "scriptTask", "manualTask",
                "businessRuleTask", "sendTask", "receiveTask"
            };
            foreach (var t in types)
            {
                var path = ".//bpmn:" + t;
                var nodes = processNode.SelectNodes(path, nsmgr);
                if (nodes == null)
                {
                    continue;
                }

                foreach (XmlNode n in nodes)
                {
                    yield return n;
                }
            }
        }

        private static VariableEntity ParseVariableElement(
            XmlNode node,
            XmlNamespaceManager nsmgr,
            string processId,
            string version,
            string activityId,
            int sortOrder)
        {
            var name = XMLHelper.GetXmlAttribute(node, "name");
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var type = XMLHelper.GetXmlAttribute(node, "type");
            var defaultValue = XMLHelper.GetXmlAttribute(node, "defaultValue");
            var directionRaw = XMLHelper.GetXmlAttribute(node, "direction");
            var direction = string.Equals(directionRaw, "Output", StringComparison.OrdinalIgnoreCase)
                ? "Output"
                : "Input";

            var isRefAttr = XMLHelper.GetXmlAttribute(node, "isReferenced");
            short isReferenced = 0;
            if (!string.IsNullOrWhiteSpace(isRefAttr)
                && bool.TryParse(isRefAttr.Trim(), out var refVal)
                && refVal)
            {
                isReferenced = 1;
            }

            var isReqAttr = XMLHelper.GetXmlAttribute(node, "isRequired");
            short isRequired = 0;
            if (!string.IsNullOrWhiteSpace(isReqAttr)
                && bool.TryParse(isReqAttr.Trim(), out var reqVal)
                && reqVal)
            {
                isRequired = 1;
            }

            string sourceRef = null;
            string sourceVariableName = null;
            if (isReferenced == 1)
            {
                var refNode = node.SelectSingleNode("sf:varRefDetail", nsmgr)
                    ?? node.SelectSingleNode("varRefDetail", nsmgr);
                if (refNode != null)
                {
                    sourceRef = XMLHelper.GetXmlAttribute(refNode, "sourceRef");
                    sourceVariableName = XMLHelper.GetXmlAttribute(refNode, "variableName");
                }
            }

            return new VariableEntity
            {
                ProcessId = processId,
                Version = version,
                ActivityId = activityId,
                Name = name,
                Type = type,
                Direction = direction,
                DefaultValue = defaultValue,
                IsRequired = isRequired,
                IsReferenced = isReferenced,
                SourceRef = sourceRef,
                SourceVariableName = sourceVariableName,
                SortOrder = sortOrder
            };
        }
    }
}
