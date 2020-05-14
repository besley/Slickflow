using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DapperExtensions;
using SlickOne.WebUtility;
using Slickflow.BizAppService.Common;
using Slickflow.BizAppService.Entity;
using Slickflow.BizAppService.Interface;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Service;
using Slickflow.BizAppService.Utility;

namespace Slickflow.BizAppService.Service
{
    /// <summary>
    /// 生产订单服务
    /// 示例代码，请勿直接作为生产项目代码使用。
    /// 订单同步后，只有有效订单，才可以派单，即启动流程，进入正式生产环节。
    /// </summary>
    public partial class ProductOrderService : ServiceBase, IProductOrderService, IWfServiceRegister
    {
        #region IWorkflowRegister members
        public WfAppRunner WfAppRunner
        {
            get;
            set;
        }

        public void RegisterWfAppRunner(WfAppRunner runner)
        {
            WfAppRunner = runner;
        }
        #endregion

        #region IProductOrderService Members

        #region 基础数据准备
        //测试数据
        private readonly string[] pruducttype = new string[] { "童话玩具A型", "遥控飞机B型", "智能玩具C型", "遥控灯D型", "LED节能灯E型", "海盗船F型", "火星玩具车G型" };
        private readonly string[] shoptype = new string[] { "A店", "B店", "C店", "J店", "F店" , "V店"};
        private readonly string[] customertype = new string[] { "汇丰银行", "花旗银行", "瑞银信托", "中石油", "中国邮政", "阿里巴巴", "青田麦家", "HACK 新闻", "BBC", "迪斯尼乐园"};
        private readonly string[] addresstype = new string[] {"上海人民广场" , "上海浦东新区", "北京王府井", "北京燕山", "北京复兴门", "杭州西湖区", "福建岭南", "美国纽约", "英国伦敦", "香港乐趣"};

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="count">记录数</param>
        /// <returns></returns>
        public List<ProductOrderEntity> GetPaged(ProductOrderQuery query, out int count)
        {
            List<ProductOrderEntity> list = null;
            var conn = SessionFactory.CreateConnection();

            try
            {
                var sortList = new List<DapperExtensions.ISort>();
                sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });

                IFieldPredicate predicate = null;
                if (query.Status > 0){
                    predicate = Predicates.Field<ProductOrderEntity>(f => f.Status, Operator.Eq, query.Status);
                }
                else if (!string.IsNullOrEmpty(query.RoleCode))
                {
                    var status = GetStatusByRoleCode(query.RoleCode);
                    if (status < 6)
                        predicate = Predicates.Field<ProductOrderEntity>(f => f.Status, Operator.Eq, status);
                    else
                        predicate = Predicates.Field<ProductOrderEntity>(f => f.Status, Operator.Ge, status);
                }

                count = QuickRepository.Count<ProductOrderEntity>(conn, predicate);
                list = QuickRepository.GetPaged<ProductOrderEntity>(conn, query.PageIndex, query.PageSize,
                    predicate, sortList, false).ToList();

                return list;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 根据角色获取数据权限
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        private int GetStatusByRoleCode(string roleCode)
        {
            int status = 0;
            if (roleCode == RoleCodeEnum.salesmate.ToString())
            {
                status = 1;
            }
            else if (roleCode == RoleCodeEnum.techmate.ToString())
            {
                status = 3;
            }
            else if (roleCode == RoleCodeEnum.merchandiser.ToString())
            {
                status = 4;
            }
            else if (roleCode == RoleCodeEnum.qcmate.ToString())
            {
                status = 5;
            }
            else if (roleCode == RoleCodeEnum.expressmate.ToString())
            {
                status = 6;
            }
            
            return status;
        }

        /// <summary>
        /// 同步订单
        /// </summary>
        /// <param name="entity"></param>
        public ProductOrderEntity SyncOrder(IDbConnection conn, IDbTransaction trans)
        {
            var entity = new ProductOrderEntity();
            var rnd = new Random();
            entity.OrderCode = string.Format("TB{0}", rnd.Next(100000, 999999).ToString());
            entity.Status = (short)ProductOrderStatusEnum.Ready;
            entity.ProductName = pruducttype[rnd.Next(1, 7)-1];
            entity.Quantity = rnd.Next(1, 10);
            entity.UnitPrice = 1000;
            entity.TotalPrice = entity.Quantity * entity.UnitPrice;

            var a = rnd.Next(1, 10) - 1;
            entity.CustomerName = customertype[a];
            entity.Address = addresstype[a];

            entity.Mobile = rnd.Next(100000, 999999).ToString();
            entity.Remark = shoptype[rnd.Next(1, 6)-1];
            entity.CreatedTime = System.DateTime.Now;

            QuickRepository.Insert<ProductOrderEntity>(conn, entity, trans);

            return entity;
        }
        #endregion

        /// <summary>
        /// 派单
        /// </summary>
        /// <param name="entity"></param>
        public WfAppResult Dispatch(ProductOrderEntity entity)
        {
            //启动流程
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();

            try
            {
                //判断流程有没有已经启动
                var isRunning = wfas.CheckProcessInstanceRunning(WfAppRunner);
                if (isRunning == false)
                {
                    var startedResult = wfas.StartProcess(WfAppRunner);
                    if (startedResult.Status != WfExecutedStatus.Success)
                    {
                        appResult = WfAppResult.Error(startedResult.Message);
                        return appResult;
                    }
                }
            }
            catch (System.Exception ex)
            {
                appResult = WfAppResult.Error(ex.Message);
                return appResult;
            }

            //运行流程部分
            //继续下一步运行
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //运行流程
                var runResult = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (runResult.Status == WfExecutedStatus.Success)
                {

                    //写步骤记录表
                    Write(session, WfAppRunner, "派单", entity.ID.ToString(), entity.OrderCode, "完成派单");

                    //业务数据处理部分，此处是简单示例...
                    short status = GetProductOrderStatusByActivityCode(WfAppRunner.ProcessGUID, WfAppRunner.Version,
                        WfAppRunner.NextActivityPerformers.Keys.ElementAt<string>(0));
                    UpdateStatus(entity.ID, status, session);
                    session.Commit();
                    appResult = WfAppResult.Success();
                }
                else
                {
                    session.Rollback();
                    appResult = WfAppResult.Error(runResult.Message);
                }
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                appResult = WfAppResult.Error(ex.Message);
            }
            finally
            {
                session.Dispose();
            }
            return appResult;
        }

        /// <summary>
        /// 打样
        /// </summary>
        /// <param name="entity"></param>
        public WfAppResult Sample(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    Write(session, WfAppRunner, "打样", entity.ID.ToString(), entity.OrderCode, "完成打样");

                    //业务数据处理部分，此处是简单示例...
                    short status = GetProductOrderStatusByActivityCode(WfAppRunner.ProcessGUID, WfAppRunner.Version,
                        WfAppRunner.NextActivityPerformers.Keys.ElementAt<string>(0));
                    UpdateStatus(entity.ID, status, session);
                    
                    session.Commit();
                    appResult = WfAppResult.Success();

                    try
                    {
                        //调用工厂作业流程节点：
                        //流程节点:OrderFactoryMessageCaught
                        //流程ProcessGUID:0f5829c7-17df-43eb-bfe5-1f2870fb2a0e Version:1
                        var invokeAppRunner = new WfAppRunner();
                        invokeAppRunner.UserID = WfAppRunner.UserID;
                        invokeAppRunner.UserName = WfAppRunner.UserName;
                        invokeAppRunner.ProcessGUID = "0f5829c7-17df-43eb-bfe5-1f2870fb2a0e";
                        invokeAppRunner.Version = "1";
                        invokeAppRunner.AppName = WfAppRunner.AppName;
                        invokeAppRunner.AppInstanceID = WfAppRunner.AppInstanceID;
                        invokeAppRunner.AppInstanceCode = WfAppRunner.AppInstanceCode;
                        invokeAppRunner.DynamicVariables["ActivityCode"] = "OrderFactoryMessageCaught";

                        var httpClient = HttpClientHelper.CreateHelper("http://localhost/sfsweb2/api/wfservice/invokejob");
                        var invokeResult = httpClient.Post(invokeAppRunner);
                    }
                    catch (System.Exception ex)
                    {
                        //记录异常日志
                        ;
                    }
                }
                else
                {
                    session.Rollback();
                    appResult = WfAppResult.Error(result.Message);
                }
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                appResult = WfAppResult.Error(ex.Message);
            }
            finally
            {
                session.Dispose();
            }
            return appResult;
        }

        /// <summary>
        /// 生产
        /// </summary>
        /// <param name="entity"></param>
        public WfAppResult Manufacture(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    Write(session, WfAppRunner, "生产", entity.ID.ToString(), entity.OrderCode, "完成生产");

                    //业务数据处理部分，此处是简单示例...
                    short status = GetProductOrderStatusByActivityCode(WfAppRunner.ProcessGUID, WfAppRunner.Version,
                        WfAppRunner.NextActivityPerformers.Keys.ElementAt<string>(0));
                    UpdateStatus(entity.ID, status, session);
                    session.Commit();
                    appResult = WfAppResult.Success();
                }
                else
                {
                    session.Rollback();
                    appResult = WfAppResult.Error(result.Message);
                }
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                appResult = WfAppResult.Error(ex.Message);
            }
            finally
            {
                session.Dispose();
            }
            return appResult;
        }

        /// <summary>
        /// 质检
        /// </summary>
        /// <param name="entity"></param>
        public WfAppResult QCCheck(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    Write(session, WfAppRunner, "质检", entity.ID.ToString(), entity.OrderCode, "完成质检");

                    //业务数据处理部分，此处是简单示例...
                    short status = GetProductOrderStatusByActivityCode(WfAppRunner.ProcessGUID, WfAppRunner.Version,
                        WfAppRunner.NextActivityPerformers.Keys.ElementAt<string>(0));
                    UpdateStatus(entity.ID, status, session);
                    session.Commit();
                    appResult = WfAppResult.Success();
                }
                else
                {
                    session.Rollback();
                    appResult = WfAppResult.Error(result.Message);
                }
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                appResult = WfAppResult.Error(ex.Message);
            }
            finally
            {
                session.Dispose();
            }
            return appResult;
        }

        /// <summary>
        /// 称重
        /// </summary>
        /// <param name="entity"></param>
        public WfAppResult Weight(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    Write(session, WfAppRunner, "称重", entity.ID.ToString(), entity.OrderCode, "完成称重");

                    //业务数据处理部分，此处是简单示例...
                    short status = GetProductOrderStatusByActivityCode(WfAppRunner.ProcessGUID, WfAppRunner.Version,
                        WfAppRunner.NextActivityPerformers.Keys.ElementAt<string>(0));
                    UpdateStatus(entity.ID, status, session);
                    session.Commit();
                    appResult = WfAppResult.Success();
                }
                else
                {
                    session.Rollback();
                    appResult = WfAppResult.Error(result.Message);
                }
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                appResult = WfAppResult.Error(ex.Message);
            }
            finally
            {
                session.Dispose();
            }
            return appResult;
        }

        /// <summary>
        /// 快递发货
        /// </summary>
        /// <param name="entity"></param>
        public WfAppResult Delivery(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    Write(session, WfAppRunner, "发货", entity.ID.ToString(), entity.OrderCode, "完成发货");

                    //业务数据处理部分，此处是简单示例...
                    short status = GetProductOrderStatusByActivityCode(WfAppRunner.ProcessGUID, WfAppRunner.Version, 
                        WfAppRunner.NextActivityPerformers.Keys.ElementAt<string>(0));
                    UpdateStatus(entity.ID, status, session);
                    session.Commit();
                    appResult = WfAppResult.Success();
                }
                else
                {
                    session.Rollback();
                    appResult = WfAppResult.Error(result.Message);
                }
            }
            catch (System.Exception ex)
            {
                session.Rollback();
                appResult = WfAppResult.Error(ex.Message);
            }
            finally
            {
                session.Dispose();
            }
            return appResult;
        }

        /// <summary>
        /// 订单状态更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        private int UpdateStatus(int id, short status, IDbSession session)
        {
            var sql = @"UPDATE ManProductOrder
                        SET Status=@status,
                            LastUpdatedTime=@lastUpdatedTime
                        WHERE ID=@id";

            var rows = QuickRepository.Execute(session.Connection, sql,
                    new
                    {
                        status = status,
                        lastUpdatedTime = System.DateTime.Now,
                        id = id
                    },
                    session.Transaction);

            return rows;
        }

        /// <summary>
        /// 写业务流程记录日志信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="runner"></param>
        /// <param name="activityName"></param>
        /// <param name="appInstanceID"></param>
        /// <param name="appInstanceCode"></param>
        /// <param name="remark"></param>
        private void Write(IDbSession session, WfAppRunner runner, string activityName, 
            string appInstanceID, string appInstanceCode = null, string remark = null)
        {
            //"派单", entity.ID.ToString(), entity.OrderCode, "完成派单")
            var flow = new AppFlowEntity();

            flow.AppInstanceID = appInstanceID;
            flow.AppInstanceCode = appInstanceCode;
            flow.AppName = runner.AppName;
            flow.ActivityName = activityName;
            flow.ChangedTime = System.DateTime.Now;
            flow.ChangedUserID = runner.UserID;
            flow.ChangedUserName = runner.UserName;
            flow.Remark = remark;

            QuickRepository.Insert<AppFlowEntity>(session.Connection, flow, session.Transaction);
        }

        /// <summary>
        /// 根据活动Code获取对应产品状态
        /// </summary>
        /// <param name="processGUID"></param>
        /// <param name="verison"></param>
        /// <param name="activityGUID"></param>
        /// <returns></returns>
        private short GetProductOrderStatusByActivityCode(string processGUID, string verison, string activityGUID)
        {
            var wfas = new WfAppInteropService();
            var activityNode = wfas.GetActivity(processGUID, WfAppRunner.Version, activityGUID);
            if (activityNode.ActivityType == ActivityTypeEnum.EndNode)
            {
                return (short)ProductOrderStatusEnum.Completed;
            }
            else
            {
                return (short)EnumHelper.ParseEnum<ProductOrderStatusEnum>(activityNode.ActivityCode);
            }
        }

        #endregion
    }
}
