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
    /// Production order service=
    /// After order synchronization, only valid orders can be dispatched, which starts the process and enters the formal production stage
    /// Example code for developers' reference
    /// 生产订单服务=
    /// 订单同步后，只有有效订单，才可以派单，即启动流程，进入正式生产环节
    /// 示例代码，供开发人员参考
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

        #region Sample Data
        //Testing Data
        private readonly string[] pruducttype = new string[] { "FairyTale-A", "Aircraft-B", "SmartToy-C", "LED-D", "AIVoice-E", "PirateShip-F", "MarsCar-G" };
        private readonly string[] shoptype = new string[] { "Store-A", "Store-B", "Store-C", "Store-J", "Store-F", "Store-V" };
        private readonly string[] customertype = new string[] { "HSBC", "CitiBank", "UBS", "PetrolShell", "PostUK", "Alilbaba", "MaiJIA", "HACK-News", "BBC", "Disney"};
        private readonly string[] addresstype = new string[] {"PepopleSquare" , "PuDongNewArea", "Wangfujing", "Yanshan", "FuxingGate", "WestLake", "Lingnan", "NewYork", "London", "Hongkong"};

        /// <summary>
        /// Get data paged
        /// </summary>
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
                    predicate = Predicates.Field<ProductOrderEntity>(f => f.Status, DapperExtensions.Operator.Eq, query.Status);
                }
                else if (!string.IsNullOrEmpty(query.RoleCode))
                {
                    var status = GetStatusByRoleCode(query.RoleCode);
                    if (status < 6)
                        predicate = Predicates.Field<ProductOrderEntity>(f => f.Status, DapperExtensions.Operator.Eq, status);
                    else
                        predicate = Predicates.Field<ProductOrderEntity>(f => f.Status, DapperExtensions.Operator.Ge, status);
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
        /// Get status by role code
        /// 根据角色获取数据权限
        /// </summary>
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
        /// Sync Orders
        /// 同步订单
        /// </summary>
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
        /// Dispatch Order
        /// 派单
        /// </summary>
        public WfAppResult Dispatch(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();

            try
            {
                var isRunning = wfas.CheckProcessInstanceRunning(WfAppRunner);
                if (isRunning == false)
                {
                    //启动流程
                    //Startup process
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

            //继续运行下一步运行
            //Continue running the next step
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //运行流程
                //Run process
                var runResult = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (runResult.Status == WfExecutedStatus.Success)
                {

                    //写步骤记录表
                    //Write a step record table
                    Write(session, WfAppRunner, "Disptach", entity.ID.ToString(), entity.OrderCode, "Dispatch Completed");

                    //业务数据处理部分，此处是简单示例...
                    //The business data processing part, here is a simple example
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
        /// Sample
        /// 打样
        /// </summary>
        public WfAppResult Sample(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                //Run Process
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    //Write a step record table
                    Write(session, WfAppRunner, "Sample", entity.ID.ToString(), entity.OrderCode, "Sample Completed");

                    //业务数据处理部分，此处是简单示例...
                    //The business data processing part, here is a simple example
                    short status = GetProductOrderStatusByActivityCode(WfAppRunner.ProcessGUID, WfAppRunner.Version,
                        WfAppRunner.NextActivityPerformers.Keys.ElementAt<string>(0));
                    UpdateStatus(entity.ID, status, session);
                    
                    session.Commit();
                    appResult = WfAppResult.Success();

                    try
                    {
                        //调用工厂作业流程节点：
                        //Call the factory workflow node
                        //Activity:OrderFactoryMessageCaught
                        //ProcessGUID:0f5829c7-17df-43eb-bfe5-1f2870fb2a0e Version:1
                        var invokeAppRunner = new WfAppRunner();
                        invokeAppRunner.UserID = WfAppRunner.UserID;
                        invokeAppRunner.UserName = WfAppRunner.UserName;
                        invokeAppRunner.ProcessGUID = "0f5829c7-17df-43eb-bfe5-1f2870fb2a0e";
                        invokeAppRunner.Version = "1";
                        invokeAppRunner.AppName = WfAppRunner.AppName;
                        invokeAppRunner.AppInstanceID = WfAppRunner.AppInstanceID;
                        invokeAppRunner.AppInstanceCode = WfAppRunner.AppInstanceCode;
                        //invokeAppRunner.DynamicVariables["ActivityCode"] = "OrderFactoryMessageCaught";

                        //var httpClient = HttpClientHelper.CreateHelper("http://localhost/sfsweb2/api/wfservice/invokejob");
                        //var invokeResult = httpClient.Post(invokeAppRunner);
                    }
                    catch (System.Exception ex)
                    {
                        //记录异常日志
                        //Record log data
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
        /// Manufacture
        /// 生产
        /// </summary>
        public WfAppResult Manufacture(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                //Run Process
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    //Write step log data
                    Write(session, WfAppRunner, "Manufacture", entity.ID.ToString(), entity.OrderCode, "Manufacture Completed");

                    //业务数据处理部分，此处是简单示例...
                    //The business data processing part, here is a simple example
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
        /// QC Check
        /// 质检
        /// </summary>
        public WfAppResult QCCheck(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                //Run process
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    //Write step log data
                    Write(session, WfAppRunner, "QC check", entity.ID.ToString(), entity.OrderCode, "QC check complted");

                    //业务数据处理部分，此处是简单示例...
                    //The business data processing part, here is a simple example
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
        /// Weight
        /// 称重
        /// </summary>
        public WfAppResult Weight(ProductOrderEntity entity)
        {
            var appResult = WfAppResult.Default();
            var wfas = new WfAppInteropService();
            var session = SessionFactory.CreateSession();
            try
            {
                session.BeginTrans();

                //流程运行
                //Run process
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    //Write step log data
                    Write(session, WfAppRunner, "Weight", entity.ID.ToString(), entity.OrderCode, "Weight Completed");

                    //业务数据处理部分，此处是简单示例...
                    //The business data processing part, here is a simple example
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
        /// Deliverty
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
                //Run Process
                var result = wfas.RunProcess(session, WfAppRunner, WfAppRunner.Conditions);
                if (result.Status == WfExecutedStatus.Success)
                {
                    //写步骤记录表
                    //Write step log data
                    Write(session, WfAppRunner, "Delivery", entity.ID.ToString(), entity.OrderCode, "Delivery Completed");

                    //业务数据处理部分，此处是简单示例...
                    //The business data processing part, here is a simple example
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
        /// Update Order Status
        /// 订单状态更新
        /// </summary>
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
        /// Write business process log information
        /// 写业务流程记录日志信息
        /// </summary>
        private void Write(IDbSession session, WfAppRunner runner, string activityName, 
            string appInstanceID, string appInstanceCode = null, string remark = null)
        {
            //"Dispatch", entity.ID.ToString(), entity.OrderCode, "Dispatch Completed")
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
        /// Obtain the corresponding product status based on the activity code
        /// 根据活动Code获取对应产品状态
        /// </summary>
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
