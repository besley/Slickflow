using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Slickflow.Graph.Layout
{
    [JsonConverter(typeof(ProcessDataConverter))]
    public class ProcessData
    {
        public ProcessInfo Process { get; set; }
        public List<ProcessNode> ProcessNodes { get; set; }
        public List<SequenceFlow> SequenceFlows { get; set; }
        public Annotations Annotations { get; set; }
    }

    public class ProcessInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("startEvent")]
        public string StartEvent { get; set; }
        [JsonProperty("endEvent")]
        public string EndEvent { get; set; }
        //有些时候deepseek提供如下的数据格式，需要统一转换
        // 兼容嵌套在 Process 内部的 ProcessNodes 和 SequenceFlows
        [JsonProperty("ProcessNodes")]
        public List<ProcessNode> InnerProcessNodes { get; set; }

        [JsonProperty("SequenceFlows")]
        public List<SequenceFlow> InnerSequenceFlows { get; set; }
    }

    public class ProcessNode
    {
        public ProcessNode()
        {
            Position = new Position();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("from")]
        public string From { set { Source = value; } }
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("to")]
        public string To { set { Target = value; } }
        // 扩展属性
        public Dictionary<string, string> Properties { get; set; }
        public string Assignee { get; set; }
        public string Implementation { get; set; }
        public Position Position { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    // 基础几何结构定义
    public struct Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => !(a == b);

        public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
    }


    public class SequenceFlow
    {
        public string Id { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("from")]
        public string From { set { Source = value;  }}

        [JsonProperty("to")]
        public string To { set { Target = value; } }
        [JsonProperty("sourceRef")]
        public string SourceRef { set { Source = value; } }
        [JsonProperty("targetRef")]
        public string TargetRef { set { Target = value; } }
        public string ConditionExpression { get; set; }
        public List<Point> RoutePoints { get; set; } = new List<Point>();
    }

    public class ShapeSize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Annotations
    {
        public List<string> BusinessRules { get; set; }
        public List<DataObject> DataObjects { get; set; }
    }

    public class DataObject
    {
        public string Name { get; set; }
        public List<string> Properties { get; set; }
    }

    public class ProcessDataConverter : JsonConverter<ProcessData>
    {
        public override ProcessData ReadJson(
            JsonReader reader,
            Type objectType,
            ProcessData existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            JObject jsonObject = JObject.Load(reader);
            ProcessData data = new ProcessData();

            // 处理 Process 对象（不区分大小写）
            if (jsonObject.GetValueIgnoreCase("Process") is JObject processObj)
            {
                data.Process = new ProcessInfo();

                // 手动映射 ProcessInfo 属性（不区分大小写）
                data.Process.Id = processObj.GetValueIgnoreCase("id")?.Value<string>();
                data.Process.Name = processObj.GetValueIgnoreCase("name")?.Value<string>();
                data.Process.StartEvent = processObj.GetValueIgnoreCase("StartEvent")?.Value<string>();
                data.Process.EndEvent = processObj.GetValueIgnoreCase("EndEvent")?.Value<string>();

                // 处理嵌套的 ProcessNodes 和 SequenceFlows（不区分大小写）
                data.Process.InnerProcessNodes = processObj.GetValueIgnoreCase("ProcessNodes")?.ToObject<List<ProcessNode>>(serializer);
                data.Process.InnerSequenceFlows = processObj.GetValueIgnoreCase("SequenceFlows")?.ToObject<List<SequenceFlow>>(serializer);
            }

            // 处理根级别的 ProcessNodes（不区分大小写）
            data.ProcessNodes = jsonObject.GetValueIgnoreCase("ProcessNodes")?.ToObject<List<ProcessNode>>(serializer)
                ?? data.Process?.InnerProcessNodes;

            // 处理根级别的 SequenceFlows（不区分大小写）
            data.SequenceFlows = jsonObject.GetValueIgnoreCase("SequenceFlows")?.ToObject<List<SequenceFlow>>(serializer)
                ?? data.Process?.InnerSequenceFlows;

            // 处理 Annotations
            data.Annotations = jsonObject.GetValueIgnoreCase("Annotations")?.ToObject<Annotations>();

            return data;
        }

        public override void WriteJson(JsonWriter writer, ProcessData value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public static class JObjectExtensions
    {
        public static JToken GetValueIgnoreCase(this JObject obj, string key)
        {
            foreach (var property in obj.Properties())
            {
                if (property.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
                    return property.Value;
            }
            return null;
        }
    }


    public class BpmnDefine
    {
        public static ShapeSize GetShapeSize(string activityType)
        {
            var shapeSize = new ShapeSize();
            if (activityType.ToLower() == "startevent"
                || activityType.ToLower() == "intermediateevent"
                || activityType.ToLower() == "intermediatethrowevent"
                || activityType.ToLower() == "intermediatecatchevent"
                || activityType.ToLower() == "endevent"
                || activityType.ToLower() == "event")
            {
                shapeSize.Width = CanvasPositionDefine.EVENT_POSITION_WIDTH;
                shapeSize.Height = CanvasPositionDefine.EVENT_POSITION_HEIGHT;
            }
            else if (activityType.ToLower() == "gateway"
                || activityType.ToLower() == "exclusivegateway"
                || activityType.ToLower() == "parallelgateway"
                || activityType.ToLower() == "inclusivegateway"
                || activityType.ToLower() == "eventbasedgateway"
                || activityType.ToLower() == "complexgateway")
            {
                shapeSize.Width = CanvasPositionDefine.GATEWAY_POSITION_WIDTH;
                shapeSize.Height = CanvasPositionDefine.GATEWAY_POSITION_HEIGHT;
            }
            else if (activityType.ToLower() == "task"
                || activityType.ToLower() == "usertask"
                || activityType.ToLower() == "servicetask"
                || activityType.ToLower() == "sendtask"
                || activityType.ToLower() == "receivetask"
                || activityType.ToLower() == "manualtask"
                || activityType.ToLower() == "businessruletask"
                || activityType.ToLower() == "scripttask"
                || activityType.ToLower() == "externaltask"
                || activityType.ToLower() == "callactivity")
            {
                shapeSize.Width = CanvasPositionDefine.TASK_POSITION_WIDTH;
                shapeSize.Height = CanvasPositionDefine.TASK_POSITION_HEIGHT;
            }
            else if (activityType.ToLower() == "subprocess"
                || activityType.ToLower()   == "transaction")
            {
                shapeSize.Width = CanvasPositionDefine.TASK_POSITION_WIDTH;
                shapeSize.Height = CanvasPositionDefine.TASK_POSITION_HEIGHT;
            }
            else
            {
                throw new ApplicationException(string.Format("Unknown node type:{0}", activityType));
            }
            return shapeSize;
        }

        public static ShapeSize GetShapeSize(List<ProcessNode> processNodeList, string nodeId)
        {
            ShapeSize shapeSize = null;
            foreach (var node in processNodeList)
            {
                if (node.Id == nodeId)
                {
                    shapeSize = GetShapeSize(node.Type);
                    break;
                }
            }
            return shapeSize;
        }
    }
}