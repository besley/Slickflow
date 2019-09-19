/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
*  
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;

namespace Slickflow.Graph
{
    /// <summary>
    /// 流程模型创建器
    /// </summary>
    public class ProcessModelBuilder
    {
        #region 基本属性和构造函数
        private Flow _flow = null;
        private Stack<Vertex> _sequenceStack = new Stack<Vertex>();
        private Stack<Vertex> _splitStack = new Stack<Vertex>();
        private Stack<Vertex> _joinStack = new Stack<Vertex>();
        private Stack<int> _splitTopPositionStack = new Stack<int>();

        public ProcessModelBuilder()
        {

        }
        #endregion

        #region 堆栈基本操作
        /// <summary>
        /// 节点进入堆栈
        /// </summary>
        /// <param name="stackType">堆栈类型</param>
        /// <param name="vertex">节点</param>
        private void AppendVertexIntoStack(StackTypeEnum stackType, Vertex vertex)
        {
            if (stackType == StackTypeEnum.Sequence)
            {
                _sequenceStack.Push(vertex);
            }
            else if (stackType == StackTypeEnum.Split)
            {
                _splitStack.Push(vertex);
            }
            else if (stackType == StackTypeEnum.Join)
            {
                _joinStack.Push(vertex);
            }
        }

        /// <summary>
        /// 添加新节点
        /// </summary>
        /// <param name="vertex">节点</param>
        private void AppendVertex(Vertex vertex)
        {
            _flow.Vertices.Add(vertex);
        }

        /// <summary>
        /// 获取最后一个节点
        /// </summary>
        /// <returns>节点</returns>
        private Vertex GetLastVertex()
        {
            var vertex = _flow.Vertices.Last();
            return vertex;
        }


        /// <summary>
        /// 添加新连线
        /// </summary>
        /// <param name="link">连线</param>
        private void AppendLink(Link link)
        {
            _flow.Links.Add(link);
        }
        /// <summary>
        /// 从堆栈读取节点
        /// </summary>
        /// <param name="stackType">堆栈类型</param>
        /// <returns>节点</returns>
        private Vertex GetPreviousVertexFromStack(StackTypeEnum stackType)
        {
            Vertex vertex = null;
            if (stackType == StackTypeEnum.Sequence)
            {
                vertex = _sequenceStack.Pop();
            }
            else if (stackType == StackTypeEnum.Split)
            {
                vertex = _splitStack.Pop();
            }
            else if (stackType == StackTypeEnum.Join)
            {
                vertex = _joinStack.Pop();
            }
            return vertex;
        }

        /// <summary>
        /// 获取堆栈中的元素个数
        /// </summary>
        /// <param name="stackType">堆栈类型</param>
        /// <returns>个数</returns>
        private int GetStackCount(StackTypeEnum stackType)
        {
            int count = 0;
            if (stackType == StackTypeEnum.Sequence)
            {
                count = _sequenceStack.Count();
            }
            else if(stackType == StackTypeEnum.Split)
            {
                count = _splitStack.Count();
            }
            else if(stackType == StackTypeEnum.Join)
            {
                count = _joinStack.Count();
            }
            return count;
        }
        #endregion

        #region 流程节点创建方法
        /// <summary>
        /// 创建开始节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder Start(string name = null, string code = null)
        {
            var pmb = Start(VertexBuilder.CreateStart(name, code));
            return pmb;
        }

        /// <summary>
        /// 创建开始节点
        /// </summary>
        /// <param name="vb">节点构造器</param>
        /// <returns>流程模型类</returns>
        public ProcessModelBuilder Start(VertexBuilder vb)
        {
            var vertex = vb._vertex;
            AppendVertex(vertex);

            AppendVertexIntoStack(StackTypeEnum.Sequence, vertex);

            return this;
        }

        /// <summary>
        /// 创建结束节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder End(string name = null, 
            string code= null,
            LinkBuilder lb = null)
        {
            var pmb = End(VertexBuilder.CreateEnd(name, code), lb);
            return pmb;
        }

        /// <summary>
        /// 创建结束节点
        /// </summary>
        /// <param name="nb">节点构造器</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>模型服务类</returns>
        public ProcessModelBuilder End(VertexBuilder nb, LinkBuilder lb = null)
        {
            var vertex = nb._vertex;
            AppendVertex(vertex);

            var previousVertex = GetPreviousVertexFromStack(StackTypeEnum.Sequence);

            var link = new Link();
            if (lb != null) link = lb._link;
                 
            link.Source = previousVertex;
            link.Target = vertex;

            SetLinkPosition(link);

            return this;
        }

        /// <summary>
        /// 创建任务节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder Task(string name = null, 
            string code = null, 
            LinkBuilder lb = null)
        {
            var pmb = Task(
                VertexBuilder.CreateTask(name, code), 
                lb
            );

            return pmb;
        }

        /// <summary>
        /// 创建任务节点
        /// </summary>
        /// <param name="vb">节点构造器</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>流程模型类</returns>
        public ProcessModelBuilder Task(VertexBuilder vb, LinkBuilder lb = null)
        {
            var vertex = vb._vertex;
            AppendVertex(vertex);

            var previousVertex = GetPreviousVertexFromStack(StackTypeEnum.Sequence);
            var link = new Link();
            if (lb != null) link = lb._link;

            link.Source = previousVertex;
            link.Target = vertex;

            SetLinkPosition(link);

            AppendVertexIntoStack(StackTypeEnum.Sequence, vertex);

            return this;
        }

        /// <summary>
        /// 创建分支节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder Split(string name = null, 
            string code = null,
            LinkBuilder lb = null)
        {
            var pmb = Split(VertexBuilder.CreateSplit(null, name, code), lb);
            return pmb;
        }

        /// <summary>
        /// 创建分支节点
        /// </summary>
        /// <param name="vb">节点构造器</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>流程模型构造器</returns>
        public ProcessModelBuilder Split(VertexBuilder vb, 
            LinkBuilder lb = null)
        {
            var vertex = vb._vertex;
            AppendVertex(vertex);

            var previousVertex = GetPreviousVertexFromStack(StackTypeEnum.Sequence);
            var link = new Link();
            if (lb != null) link = lb._link;

            link.Source = previousVertex;
            link.Target = vertex;

            SetLinkPosition(link);

            AppendVertexIntoStack(StackTypeEnum.Split, vertex);

            return this;
        }

        /// <summary>
        /// 创建并行分支节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder AndSplit(string name = null,
            string code = null,
            LinkBuilder lb = null)
        {
            var pmb = Split(VertexBuilder.CreateSplit(GatewayDirectionEnum.AndSplit, name, code), lb);
            return pmb;
        }

        /// <summary>
        /// 创建或分支节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder OrSplit(string name = null,
            string code = null,
            LinkBuilder lb = null)
        {
            var pmb = Split(VertexBuilder.CreateSplit(GatewayDirectionEnum.OrSplit, name, code), lb);
            return pmb;
        }

        /// <summary>
        /// 创建并行容器分支节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <param name="lb">连线构造器</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder AndSplitMI(string name = null,
            string code = null,
            LinkBuilder lb = null)
        {
            var pmb = Split(VertexBuilder.CreateSplit(GatewayDirectionEnum.AndSplitMI, name, code), lb);
            return pmb;
        }

        /// <summary>
        /// 创建合并节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <returns>流程模型构造器</returns>
        public ProcessModelBuilder Join(string name = null, 
            string code = null)
        {
            var pmb = Join(VertexBuilder.CreateJoin(null, name, code));
            return pmb;
        }

        /// <summary>
        /// 创建合并节点
        /// </summary>
        /// <param name="vb">节点构造器</param>
        /// <returns>流程模型构造器</returns>
        public ProcessModelBuilder Join(VertexBuilder vb)
        {
            var vertex = vb._vertex;
            AppendVertex(vertex);

            var count = GetStackCount(StackTypeEnum.Join);
            for (var i = 0; i < count; i++)
            {
                var previousVertex = GetPreviousVertexFromStack(StackTypeEnum.Join);
                var link = new Link();
                link.Source = previousVertex;
                link.Target = vertex;

                SetLinkPosition(link);
            }
            AppendVertexIntoStack(StackTypeEnum.Sequence, vertex);

            return this;
        }

        /// <summary>
        /// 创建与合并节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <returns>流程模型构造器</returns>
        public ProcessModelBuilder AndJoin(string name = null,
            string code = null)
        {
            var pmb = Join(VertexBuilder.CreateJoin(GatewayDirectionEnum.AndJoin, name, code));
            return pmb;
        }


        /// <summary>
        /// 创建或合并节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <returns>流程模型构造器</returns>
        public ProcessModelBuilder OrJoin(string name = null,
            string code = null)
        {
            var pmb = Join(VertexBuilder.CreateJoin(GatewayDirectionEnum.OrJoin, name, code));
            return pmb;
        }


        /// <summary>
        /// 创建并行容器合并节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <returns>流程模型构造器</returns>
        public ProcessModelBuilder AndJoinMI(string name = null,
            string code = null)
        {
            var pmb = Join(VertexBuilder.CreateJoin(GatewayDirectionEnum.AndJoinMI, name, code));
            return pmb;
        }


        /// <summary>
        /// 创建增强或合并节点
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="code">编码</param>
        /// <returns>流程模型构造器</returns>
        public ProcessModelBuilder EOrJoin(string name = null,
            string code = null)
        {
            var pmb = Join(VertexBuilder.CreateJoin(GatewayDirectionEnum.EOrJoin, name, code));
            return pmb;
        }

        /// <summary>
        /// 创建分支
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder Branch(params Func<ProcessModelBuilder>[] nodes)
        {
            ProcessModelBuilder b = null;
            var first = nodes.First();
            var last = nodes.Last();

            foreach (var func in nodes)
            {
                if (func.Equals(first))
                {
                    var gatewayNode = GetPreviousVertexFromStack(StackTypeEnum.Split);
                    AppendVertexIntoStack(StackTypeEnum.Sequence, gatewayNode);
                    
                    b = func();

                    //取出分支堆栈位置，并且给节点赋值，只限于第一个分支节点
                    var activityNode = GetLastVertex();
                    activityNode.Top = _splitTopPositionStack.Pop();

                    //分支只有一个节点的情况
                    if (func.Equals(last))
                    {
                        var firstLastBranchNode = GetPreviousVertexFromStack(StackTypeEnum.Sequence);
                        AppendVertexIntoStack(StackTypeEnum.Join, firstLastBranchNode);
                    }
                }
                else if (func.Equals(last))
                {
                    b = func();

                    var lastBranchNode = GetPreviousVertexFromStack(StackTypeEnum.Sequence);
                    AppendVertexIntoStack(StackTypeEnum.Join, lastBranchNode);
                }
                else
                {
                    b = func();
                }
            }
            return b;
        }

        /// <summary>
        /// 并行方法
        /// </summary>
        /// <param name="branches">分支数组</param>
        /// <returns>服务类</returns>
        public ProcessModelBuilder Parallels(params Func<ProcessModelBuilder>[] branches)
        {
            var branchCount = branches.Length;
            var gatewayNode = GetLastVertex();

            for (var i = 0; i < branchCount; i++)
            {
                //记录分支Top位置信息堆栈，便于Branch()执行
                //并行分支容器只有一个分支，所以Top跟Gateway节点齐平
                var top = branchCount == 1 ? gatewayNode.Top : gatewayNode.Top + (i - 1) * 100;

                _splitTopPositionStack.Push(top);

                AppendVertexIntoStack(StackTypeEnum.Split, gatewayNode);
            }

            ProcessModelBuilder b = null;
            foreach (var func in branches)
            {
                b = func();
            }
            return b;
        }

        /// <summary>
        /// 创建连线
        /// </summary>
        /// <param name="link">连线</param>
        /// <returns>连线</returns>
        private Link SetLinkPosition(Link link)
        {
            var source = link.Source;
            var target = link.Target;

            //连线时，根据节点类型，确定前后两个节点的位置
            target.Left = 50 + source.Level * 160;      //the first start node is the orgin position.
            if (source.Activity.ActivityType == ActivityTypeEnum.GatewayNode
                && source.Activity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Split)
            {
                if (source.Activity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
                {
                    target.Top = source.Top;
                }
                else
                {
                    target.Top = source.Top + 100;
                }
            }
            else if (target.Activity.ActivityType == ActivityTypeEnum.GatewayNode
                && target.Activity.GatewaySplitJoinType == GatewaySplitJoinTypeEnum.Join)
            {
                target.Top = source.Top;
            }
            else
            {
                target.Top = source.Top;
            }
            target.Level = source.Level + 1;

            //添加连线到集合
            AppendLink(link);
            
            return link;
        }
        #endregion

        #region 流程XML生成
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <returns>流程模型构造</returns>
        public string Serialize()
        {
            var pxb = new ProcessXmlBuilder(_flow);
            string xmlContent = pxb.Serialize();

            return xmlContent;
        }

        /// <summary>
        /// 存储流程图形XML到数据库
        /// </summary>
        /// <returns></returns>
        public ProcessEntity Store()
        {
            var pxb = new ProcessXmlBuilder(_flow);
            string xmlContent = pxb.Serialize();
            var entity = new ProcessEntity
            {
                ProcessGUID = Guid.NewGuid().ToString(),
                ProcessName = _flow.Name,
                ProcessCode = _flow.Code,
                Version = "1",
                IsUsing = 1,
                XmlContent = xmlContent,
                CreatedDateTime = System.DateTime.Now
            };

            var pm = new ProcessManager();
            var processID = pm.Insert(entity);
            entity.ID = processID;

            return entity;
        }

        /// <summary>
        /// 打印节点和连线信息
        /// </summary>
        /// <returns>流程模型构造</returns>
        public void Print()
        {
            foreach (var vertex in _flow.Vertices)
            {
                Console.WriteLine(vertex.Name);
            }

            foreach (var link in _flow.Links)
            {
                Console.WriteLine(string.Format("source:{0}, targer:{1}", link.Source.Name, link.Target.Name));
            }
        }
        #endregion

        #region 静态创建方法
        /// <summary>
        /// 创建流程
        /// </summary>
        /// <param name="name">流程名称</param>
        /// <param name="code">流程代码</param>
        /// <returns>流程</returns>
        public static ProcessModelBuilder CreateProcess(string name, string code = null)
        {
            var pmb = new ProcessModelBuilder();

            var flow = new Flow(name, code);
            pmb._flow = flow;

            return pmb;
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="name">流程名称</param>
        /// <param name="version">流程版本</param>
        /// <returns>流程</returns>
        public static ProcessModelBuilder GetProcess(string name, string version = null)
        {
            var pm = new ProcessManager();
            var processEntity = pm.GetByName(name, version);

            var pmb = new ProcessModelBuilder();
            return pmb;
        }
        #endregion
    }
}
