using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Service;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Utility;
using System.Xml;
using System.IO;


namespace sfdapi.Models
{
    public class BpmnFileSampleDefine
    {
        private static readonly IDictionary<string, string> _bpmnFileDictionary = new Dictionary<string, string>();
        static BpmnFileSampleDefine()
        {
            //business tempaltes
            _bpmnFileDictionary.Add("askforleave", "samples\\bpmn\\business\\askforleave.bpmn");
            _bpmnFileDictionary.Add("contract", "samples\\bpmn\\business\\contract.bpmn");
            _bpmnFileDictionary.Add("eorder", "samples\\bpmn\\business\\eorder.bpmn");
            _bpmnFileDictionary.Add("officein", "samples\\bpmn\\business\\officein.bpmn");
            _bpmnFileDictionary.Add("reimbursement", "samples\\bpmn\\business\\reimbursement.bpmn");
            _bpmnFileDictionary.Add("warehousing", "samples\\bpmn\\business\\warehousing.bpmn");

            //logic templates
            _bpmnFileDictionary.Add("andsplitmi", "samples\\bpmn\\logic\\andsplitmi.bpmn");
            _bpmnFileDictionary.Add("complex", "samples\\bpmn\\logic\\complex.bpmn");
            _bpmnFileDictionary.Add("gateway", "samples\\bpmn\\logic\\gateway.bpmn");
            _bpmnFileDictionary.Add("multipleinstance", "samples\\bpmn\\logic\\multipleinstance.bpmn");
            _bpmnFileDictionary.Add("sequence", "samples\\bpmn\\logic\\sequence.bpmn");
            _bpmnFileDictionary.Add("subprocess", "samples\\bpmn\\logic\\subprocess.bpmn");
        }

        public static string RebuildBpmnPorcessXmlContentByTemplate(string templateName)
        {
            // 1. Get file path from dictionary
            if (!_bpmnFileDictionary.TryGetValue(templateName.ToLower(), out var relativeFilePath))
            {
                throw new ArgumentException($"Template '{templateName}' not found in dictionary.");
            }

            // 2. Get full file path
            var baseDirectory = AppContext.BaseDirectory;
            var fullFilePath = Path.Combine(baseDirectory, relativeFilePath);
            
            // Normalize path separators for current OS
            fullFilePath = Path.GetFullPath(fullFilePath);

            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException($"BPMN template file not found: {fullFilePath}");
            }

            // 3. Load XML file as XmlDocument
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fullFilePath);

            // 4. Generate two 4-digit random numbers
            var random1 = RandomSequenceGenerator.GetRandomInt4();
            var random2 = RandomSequenceGenerator.GetRandomInt4();

            // 5. Find process element and modify attributes
            var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("bpmn", "http://www.omg.org/spec/BPMN/20100524/MODEL");
            namespaceManager.AddNamespace("sf", "http://www.slickflow.com/schema/sf");

            // Find process element (with or without namespace)
            XmlNode processNode = null;
            
            // Try to find with namespace first
            processNode = xmlDoc.SelectSingleNode("//bpmn:process", namespaceManager);
            
            // If not found, try without namespace
            if (processNode == null)
            {
                processNode = xmlDoc.SelectSingleNode("//process");
            }

            if (processNode == null)
            {
                throw new InvalidOperationException("Process element not found in BPMN XML file.");
            }

            // 6. Modify process element attributes
            // id attribute: process_{0}_{1}
            if (processNode.Attributes["id"] != null)
            {
                processNode.Attributes["id"].Value = $"process_{random1}_{random2}";
            }
            else
            {
                var idAttr = xmlDoc.CreateAttribute("id");
                idAttr.Value = $"process_{random1}_{random2}";
                processNode.Attributes.Append(idAttr);
            }

            // name attribute: {template}_{0}_{1}
            if (processNode.Attributes["name"] != null)
            {
                processNode.Attributes["name"].Value = $"{templateName}_{random1}_{random2}";
            }
            else
            {
                var nameAttr = xmlDoc.CreateAttribute("name");
                nameAttr.Value = $"{templateName}_{random1}_{random2}";
                processNode.Attributes.Append(nameAttr);
            }

            // code attribute (sf:code): {template}_code_{0}_{1}
            var codeAttrName = "code";
            var codeAttr = processNode.Attributes[codeAttrName];
            
            // Check for sf:code namespace attribute
            if (codeAttr == null)
            {
                // Try to find sf:code with namespace
                foreach (XmlAttribute attr in processNode.Attributes)
                {
                    if (attr.LocalName == "code" && (attr.Prefix == "sf" || attr.NamespaceURI.Contains("slickflow")))
                    {
                        codeAttr = attr;
                        break;
                    }
                }
            }

            var codeValue = $"{templateName}_code_{random1}_{random2}";
            if (codeAttr != null)
            {
                codeAttr.Value = codeValue;
            }
            else
            {
                // Create sf:code attribute with namespace
                var sfCodeAttr = xmlDoc.CreateAttribute("sf", "code", "http://www.slickflow.com/schema/sf");
                sfCodeAttr.Value = codeValue;
                processNode.Attributes.Append(sfCodeAttr);
            }

            // 7. Return XML content as string
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlDoc.WriteTo(xmlWriter);
                }
                return stringWriter.ToString();
            }
        }
    }
}
