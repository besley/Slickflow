using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Definition of Node Position Variables
    /// 节点位置变量定义
    /// </summary>
    internal class CanvasPositionDefine
    {
        //画布起点位置
        //Starting point position of canvas
        internal const int CANVAS_POSITION_LEFT = 240;
        internal const int CANVAS_POSITION_TOP = 180;

        //任务节点位置
        //Task node location
        internal const int TASK_POSITION_WIDTH = 100;
        internal const int TASK_POSITION_HEIGHT = 80;

        //网关节点
        //Gateway node
        internal const int GATEWAY_POSITION_WIDTH = 36;
        internal const int GATEWAY_POSITION_HEIGHT = 36;

        //事件节点
        //Event Node
        internal const int EVENT_POSITION_WIDTH = 36;
        internal const int EVENT_POSITION_HEIGHT = 36;

        //连线位置
        //Connection location
        internal const int LINK_POSITION_WIDTH = 160;

        //分支位置
        //Branch Location
        internal const int BRANCH_POSITION_HEIGHT = 50;

        //偏移位置
        //Offset position
        internal const int SHIFT_POSITION_WIDTH = 80;

        //默认节点位置
        //default node size
        internal const int NODE_POSITION_WIDTH = 100;
        internal const int NODE_POSITION_HEIGHT = 80;

        //事件节点偏移位置
        //Event Node Offset position
        internal const int EVENT_NODE_OFFSET = 22;
    }
}
