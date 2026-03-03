using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Graph.Layout
{
    /// <summary>
    /// Definition of Node Position Variables
    /// 节点位置变量定义
    /// </summary>
    public static class CanvasPositionDefine
    {
        //画布起点位置
        //Starting point position of canvas
        public const int CANVAS_POSITION_LEFT = 240;
        public const int CANVAS_POSITION_TOP = 180;

        //任务节点位置
        //Task node location
        public const int TASK_POSITION_WIDTH = 100;
        public const int TASK_POSITION_HEIGHT = 80;

        //网关节点
        //Gateway node
        public const int GATEWAY_POSITION_WIDTH = 36;
        public const int GATEWAY_POSITION_HEIGHT = 36;

        //事件节点
        //Event Node
        public const int EVENT_POSITION_WIDTH = 36;
        public const int EVENT_POSITION_HEIGHT = 36;

        //连线位置
        //Connection location
        public const int LINK_POSITION_WIDTH = 160;

        //分支位置
        //Branch Location
        public const int BRANCH_POSITION_HEIGHT = 50;

        //偏移位置
        //Offset position
        public const int SHIFT_POSITION_WIDTH = 80;

        //默认节点位置
        //default node size
        public const int NODE_POSITION_WIDTH = 100;
        public const int NODE_POSITION_HEIGHT = 80;

        //事件节点偏移位置
        //Event Node Offset position
        public const int EVENT_NODE_OFFSET = 22;
    }
}
