using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Template
{
    public class ProcessNamespaceDefine
    {
        #region Static variables
        public static readonly string m_namespace_prefix_bpmn = "bpmn";
        public static readonly string m_namespace_prefix_bpmndi = "bpmndi";
        public static readonly string m_namespace_prefix_di = "di";
        public static readonly string m_namespace_prefix_dc = "dc";
        public static readonly string m_namespace_prefix_sf = "sf";

        public static readonly string m_namespace_uri_bpmn = "http://www.omg.org/spec/BPMN/20100524/MODEL";
        public static readonly string m_namespace_uri_bpmndi = "http://www.omg.org/spec/BPMN/20100524/DI";
        public static readonly string m_namespace_uri_di = "http://www.omg.org/spec/DD/20100524/DI";
        public static readonly string m_namespace_uri_dc = "http://www.omg.org/spec/DD/20100524/DC";
        public static readonly string m_namespace_uri_sf = "http://www.slickflow.com/schema/sf";
        public static readonly string m_namespace_uri_target = "http://bpmn.io/schema/bpmn";
        public static readonly string m_namespace_uri_xsi = "http://www.w3.org/2001/XMLSchema-instance";

        public static readonly string m_bpmn_node_process = "bpmn:process";
        public static readonly string m_bpmndi_node_diagram = "bpmndi:BPMNDiagram";
        public static readonly string m_bpmndi_node_plane = "bpmndi:BPMNPlane";
        #endregion
    }
}
