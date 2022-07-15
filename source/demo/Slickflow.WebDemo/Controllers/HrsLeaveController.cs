using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlickOne.WebUtility;
using Slickflow.HrsService.Entity;
using Slickflow.HrsService.Service;

namespace Slickflow.WebDemo.Controllers
{
    public class HrsLeaveController : Controller
    {
        [HttpGet]
        public ResponseResult<HrsLeaveEntity> GetLeave(int id)
        {
            var result = ResponseResult<HrsLeaveEntity>.Default();
            try
            {
                var hrsService = new HrsLeaveService();
                var entity = hrsService.GetByID(id);

                result = ResponseResult<HrsLeaveEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<HrsLeaveEntity>.Error(ex.Message);
            }
            return result;
        }

        [HttpPost]
        public ResponseResult<List<HrsLeaveEntity>> QueryLeave([FromBody] HrsLeaveQuery query)
        {
            var result = ResponseResult<List<HrsLeaveEntity>>.Default();
            try
            {
                var hrsService = new HrsLeaveService();
                var list = hrsService.QueryLeave(query);

                result = ResponseResult<List<HrsLeaveEntity>>.Success(list);
            }
            catch(System.Exception ex)
            {
                result = ResponseResult<List<HrsLeaveEntity>>.Error(ex.Message);
            }
            return result;
        }

        [HttpPost]
        public ResponseResult CreateNewLeave([FromBody] HrsLeaveRunner entityRunner)
        {
            var result = ResponseResult.Default();
            try
            {
                var hrsService = new HrsLeaveService();
                var entity = entityRunner.Entity;
                hrsService.RegisterWfAppRunner(entityRunner.Runner);
                hrsService.Submit(entity);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        [HttpPost]
        public ResponseResult UpdateLeave([FromBody] HrsLeaveEntity entity)
        {
            var result = ResponseResult.Default();
            try
            {
                var hrsService = new HrsLeaveService();
                var entityDB = hrsService.GetByID(entity.ID);
                entityDB.Opinions = entity.Opinions;
                hrsService.Update(entityDB);

                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }
    }
}
