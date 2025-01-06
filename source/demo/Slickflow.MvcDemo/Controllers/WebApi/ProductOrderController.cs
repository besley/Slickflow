using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Slickflow.Data;
using Slickflow.Module.Resource;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using SlickOne.WebUtility;
using Slickflow.BizAppService.Entity;
using Slickflow.BizAppService.Interface;
using Slickflow.BizAppService.Service;

namespace Slickflow.MvcDemo.Controllers.WebApi
{
    /// <summary>
    /// Production order controller
    /// Example code for developers' reference
    /// 生产订单控制器
    /// 示例代码，供开发人员参考
    /// </summary>
    public partial class ProductOrderController : ApiControllerBase
    {
        #region Basic Interface
        private IProductOrderService _service;
        protected IProductOrderService ProductOrderService
        {
            get
            {
                if (_service == null)
                {
                    _service = new ProductOrderService();
                }
                return _service;
            }
        }

        /// <summary>
        /// Obtain production order data
        /// 获取生产订单数据
        /// </summary>
        [HttpGet]
        public ResponseResult<ProductOrderEntity> Get(int id)
        {
            return Get<ProductOrderEntity>(id);
        }

        /// <summary>
        /// Insert Product Data
        /// 插入ProductOrder数据
        /// </summary>
        [HttpPost]
        public ResponseResult<dynamic> Insert([FromBody] ProductOrderEntity entity)
        {
            return Insert<ProductOrderEntity>(entity);
        }

        /// <summary>
        /// Update Product Data
        /// 更新ProductOrder数据
        /// </summary>
        [HttpPut]
        public ResponseResult Update([FromBody] ProductOrderEntity entity)
        {
            return Update<ProductOrderEntity>(entity);
        }

        /// <summary>
        /// Delete Product Data
        /// 删除ProductOrder数据
        /// </summary>
        [HttpDelete]
        public ResponseResult Delete(int id)
        {
            return Delete<ProductOrderEntity>(id);
        }
        #endregion

        /// <summary>
        /// Query data paged
        /// 数据分页显示
        /// </summary>
        [HttpPost]
        public ResponseResult<List<ProductOrderEntity>> QueryPaged([FromBody] ProductOrderQuery query)
        {
            var result = ResponseResult<List<ProductOrderEntity>>.Default();
            try
            {
                var count = 0;
                var list = ProductOrderService.GetPaged(query, out count);
                result = ResponseResult<List<ProductOrderEntity>>.Success(list);
                result.TotalRowsCount = count;
                result.TotalPages = (count + query.PageSize - 1) / query.PageSize;
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<ProductOrderEntity>>.Error(
                    string.Format("Failed to retrieve paginated data, error:{0}",  ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Sync Order
        /// 同步订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResponseResult<ProductOrderEntity> SyncOrder()
        {
            var result = ResponseResult<ProductOrderEntity>.Default();
            try
            {
                using (var session = SessionFactory.CreateSession())
                {
                    try
                    {
                        session.BeginTrans();
                        var entity = ProductOrderService.SyncOrder(session.Connection, session.Transaction);
                        session.Commit();
                        result = ResponseResult<ProductOrderEntity>.Success(entity, "Synchronized order data successfully！");
                    }
                    catch (System.Exception ex)
                    {
                        session.Rollback();
                        result = ResponseResult<ProductOrderEntity>.Error(
                            string.Format("Synchronization order failed, error:{0}", ex.Message)
                        );
                    }
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProductOrderEntity>.Error(
                     string.Format("Synchronization order failed, error:{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Sync Order
        /// 同步订单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResponseResult<ProductOrderEntity>> SyncOrder2()
        {
            var result = ResponseResult<ProductOrderEntity>.Default();
            using (IDbSession session = SessionFactory.CreateSession())
            {
                try
                {
                    session.BeginTrans();
                    var entity = ProductOrderService.SyncOrder(session.Connection, session.Transaction);
                    session.Commit();

                    //发布消息主题
                    var wfResult = PublishProductOrderCreateMessage(entity);
                    if (wfResult.Status == 1)
                    {
                        result = ResponseResult<ProductOrderEntity>.Success(entity,
                            string.Format("Synchronize order data status:{0}", wfResult.Status));
                    }
                    else
                    {
                        session.Rollback();
                        result = ResponseResult<ProductOrderEntity>.Error(
                            string.Format("Order message initiation process failed, error:{0}", wfResult.Message)
                        );
                    }
                }
                catch (System.Exception ex)
                {
                    session.Rollback();
                    result = ResponseResult<ProductOrderEntity>.Error(
                        string.Format("Synchronization order failed, error:{0}",  ex.Message)
                    );
                }
            }
            return result;
        }

        private ResponseResult PublishProductOrderCreateMessage(ProductOrderEntity entity)
        {
            var topic = "Slickflow/ERP/OrderSystem/WorkflowService/ProductOrderSynced";
            var appRunner = new WfAppRunner();
            appRunner.MessageTopic = topic;
            appRunner.AppName = entity.ProductName;
            appRunner.AppInstanceID = entity.ID.ToString();
            appRunner.AppInstanceCode = entity.OrderCode;

            //var mqService = new MessageQueueService();
            //await mqService.Publish(topic, JsonConvert.SerializeObject(appRunner));
            var httpclient = HttpClientHelper.CreateHelper("http://localhost/sfa2/api/MessageQueue/InvokeProcess");
            var result = httpclient.Post(appRunner);
            return result;
        }

        /// <summary>
        /// Check if the order has been dispatched
        /// 检查是否已经被派单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ResponseResult CheckDispatched(int id)
        {
            var result = ResponseResult.Default();
            var orderEntity = QuickRepository.GetById<ProductOrderEntity>(id);
            if (orderEntity.Status == 1)
            {
                result = ResponseResult.Success();
            }
            else
            {
                result = ResponseResult.Error("The order has entered the process status and cannot be dispatched again！");
            }
            return result;
        }

        /// <summary>
        /// Assign Orders (Salesman)
        /// 分派订单(业务员)
        /// </summary>
        [HttpPost]
        public ResponseResult Dispatch([FromBody] dynamic entity)
        {
            var result = ResponseResult.Default();
            try
            {
                dynamic clientEntity = JsonConvert.DeserializeObject<dynamic>(entity.ToString());
                var productOrder = JsonConvert.DeserializeObject<ProductOrderEntity>(clientEntity.ProductOrderEntity.ToString());
                var runner = JsonConvert.DeserializeObject<WfAppRunner>(clientEntity.WfAppRunner.ToString());

                IWfServiceRegister s = ProductOrderService as IWfServiceRegister;
                s.RegisterWfAppRunner(runner);

                WfAppResult appResult = ProductOrderService.Dispatch(productOrder);
                if (appResult.Status == 1)
                    result = ResponseResult.Success("Successfully assigned order data！");
                else
                    result = ResponseResult.Error(string.Format("Order assignment failed:{0}", appResult.Message));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Order assignment failed, error:{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Sample(SampleMate)
        /// 打样(技术员)
        /// </summary>
        [HttpPost]
        public ResponseResult Sample([FromBody] dynamic entity)
        {
            var result = ResponseResult.Default();
            try
            {
                dynamic clientEntity = JsonConvert.DeserializeObject<dynamic>(entity.ToString());
                var productOrder = JsonConvert.DeserializeObject<ProductOrderEntity>(clientEntity.ProductOrderEntity.ToString());
                var runner = JsonConvert.DeserializeObject<WfAppRunner>(clientEntity.WfAppRunner.ToString());
                
                IWfServiceRegister s = ProductOrderService as IWfServiceRegister;
                s.RegisterWfAppRunner(runner);

                WfAppResult appResult = ProductOrderService.Sample(productOrder);
                if (appResult.Status == 1)
                    result = ResponseResult.Success("Successfully sample！");
                else
                    result = ResponseResult.Error(string.Format("Failed sample:{0}", appResult.Message));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Failed sample, error:{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Manufacture(Merchaandiser)
        /// 生产(跟单员)
        /// </summary>
        [HttpPost]
        public ResponseResult Manufacture([FromBody] dynamic entity)
        {
            var result = ResponseResult.Default();
            try
            {
                dynamic clientEntity = JsonConvert.DeserializeObject<dynamic>(entity.ToString());
                var productOrder = JsonConvert.DeserializeObject<ProductOrderEntity>(clientEntity.ProductOrderEntity.ToString());
                var runner = JsonConvert.DeserializeObject<WfAppRunner>(clientEntity.WfAppRunner.ToString());

                IWfServiceRegister s = ProductOrderService as IWfServiceRegister;
                s.RegisterWfAppRunner(runner);

                WfAppResult appResult = ProductOrderService.Manufacture(productOrder);
                if (appResult.Status == 1)
                    result = ResponseResult.Success("Successfully produced product！");
                else
                    result = ResponseResult.Error(string.Format("Failed produced product:{0}", appResult.Message));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Failed produced product:{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// QCCheck(QCMate)
        /// 质检(质检员)
        /// </summary>
        [HttpPost]
        public ResponseResult QCCheck([FromBody] dynamic entity)
        {
            var result = ResponseResult.Default();
            try
            {
                dynamic clientEntity = JsonConvert.DeserializeObject<dynamic>(entity.ToString());
                var productOrder = JsonConvert.DeserializeObject<ProductOrderEntity>(clientEntity.ProductOrderEntity.ToString());
                var runner = JsonConvert.DeserializeObject<WfAppRunner>(clientEntity.WfAppRunner.ToString());

                IWfServiceRegister s = ProductOrderService as IWfServiceRegister;
                s.RegisterWfAppRunner(runner);

                WfAppResult appResult = ProductOrderService.QCCheck(productOrder);
                if (appResult.Status == 1)
                    result = ResponseResult.Success("Product quality check successful！");
                else
                    result = ResponseResult.Error(string.Format("Failed product quality check, error:{0}", appResult.Message));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Failed product quality check, error:{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Weight(ExpressMate)
        /// 称重(包装员)
        /// </summary>
        [HttpPost]
        public ResponseResult Weight([FromBody] dynamic entity)
        {
            var result = ResponseResult.Default();
            try
            {
                dynamic clientEntity = JsonConvert.DeserializeObject<dynamic>(entity.ToString());
                var productOrder = JsonConvert.DeserializeObject<ProductOrderEntity>(clientEntity.ProductOrderEntity.ToString());
                var runner = JsonConvert.DeserializeObject<WfAppRunner>(clientEntity.WfAppRunner.ToString());

                IWfServiceRegister s = ProductOrderService as IWfServiceRegister;
                s.RegisterWfAppRunner(runner);

                WfAppResult appResult = ProductOrderService.Weight(productOrder);
                if (appResult.Status == 1)
                    result = ResponseResult.Success("Successful weight！");
                else
                    result = ResponseResult.Error(string.Format("Failed weight, error:{0}", appResult.Message));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Failed weight, error:{0}", ex.Message)
                );
            }
            return result;
        }

        /// <summary>
        /// Delivery(ExpressMate)
        /// 发货(包装员)
        /// </summary>
        [HttpPost]
        public ResponseResult Delivery([FromBody] dynamic entity)
        {
            var result = ResponseResult.Default();
            try
            {
                dynamic clientEntity = JsonConvert.DeserializeObject<dynamic>(entity.ToString());
                var productOrder = JsonConvert.DeserializeObject<ProductOrderEntity>(clientEntity.ProductOrderEntity.ToString());
                var runner = JsonConvert.DeserializeObject<WfAppRunner>(clientEntity.WfAppRunner.ToString());

                IWfServiceRegister s = ProductOrderService as IWfServiceRegister;
                s.RegisterWfAppRunner(runner);

                WfAppResult appResult = ProductOrderService.Delivery(productOrder);

                if (appResult.Status == 1)
                    result = ResponseResult.Success("Successful delivery！");
                else
                    result = ResponseResult.Error(string.Format("Failed deliverty, error:{0}", appResult.Message));
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(
                    string.Format("Failed deliverty, error:{0}", ex.Message)        
                );
            }
            return result;
        }
    }
}
