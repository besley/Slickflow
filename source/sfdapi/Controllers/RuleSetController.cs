using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Service;
using Slickflow.WebUtility;
using System;

namespace sfdapi.Controllers
{
    /// <summary>
    /// Rule set definition controller (wf_rule_set)
    /// </summary>
    public class RuleSetController : Controller
    {
        private readonly IWorkflowService _workflowService;

        public RuleSetController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpGet]
        public ResponseResult<IList<RuleSetEntity>> GetList()
        {
            try
            {
                var list = _workflowService.GetRuleSetList();
                return ResponseResult<IList<RuleSetEntity>>.Success(list);
            }
            catch (System.Exception ex)
            {
                return ResponseResult<IList<RuleSetEntity>>.Error(ex.Message);
            }
        }

        [HttpGet]
        public ResponseResult<RuleSetEntity> Get([FromQuery] string ruleSetCode)
        {
            try
            {
                var entity = _workflowService.GetRuleSet(ruleSetCode ?? "");
                return ResponseResult<RuleSetEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                return ResponseResult<RuleSetEntity>.Error(ex.Message);
            }
        }

        [HttpPost]
        public ResponseResult Save([FromBody] RuleSetEntity entity)
        {
            try
            {
                if (entity == null || string.IsNullOrEmpty(entity.RuleSetCode) || string.IsNullOrEmpty(entity.RuleSetName))
                {
                    return ResponseResult.Error("ruleSetCode and ruleSetName are required");
                }
                if (!RuleSetModeHelper.TryParseFromPersisted(entity.Mode, out var modeEnum))
                {
                    return ResponseResult.Error("mode must be \"" + RuleSetModeHelper.PersistedRuleTypes + "\" or \"" +
                                                RuleSetModeHelper.PersistedBindingsJson + "\"");
                }
                var mgr = new RuleSetExecutionManager();
                var modePersisted = RuleSetModeHelper.ToPersistedString(modeEnum);
                if (!mgr.ValidateRuleContent(modePersisted, entity.RuleContent ?? "", out var vmsg))
                    return ResponseResult.Error(vmsg ?? "rule_content validation failed");

                entity.Mode = modePersisted;
                _workflowService.SaveRuleSet(entity);
                return ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                return ResponseResult.Error(ex.Message);
            }
        }

        [HttpDelete]
        public ResponseResult Delete([FromQuery] string ruleSetCode)
        {
            try
            {
                _workflowService.DeleteRuleSet(ruleSetCode ?? "");
                return ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                return ResponseResult.Error(ex.Message);
            }
        }

        /// <summary>
        /// Execute one rule set using NRules.
        /// Request example:
        /// {
        ///   "RuleSetCode": "ORDER_APPROVAL",
        ///   "Variables": { "Amount": 1200, "IsVip": true }
        /// }
        /// </summary>
        [HttpPost]
        public ResponseResult<RuleExecutionResultEntity> Execute([FromBody] RuleExecutionEntity request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.RuleSetCode))
                {
                    return ResponseResult<RuleExecutionResultEntity>.Error("ruleSetCode is required");
                }

                var ruleSetManager = new RuleSetManager();
                var ruleSet = ruleSetManager.GetByCode(request.RuleSetCode);
                if (ruleSet == null)
                {
                    return ResponseResult<RuleExecutionResultEntity>.Error("rule set not found");
                }

                var executor = new RuleSetExecutionManager();
                var result = executor.Execute(ruleSet, request.Variables);
                return ResponseResult<RuleExecutionResultEntity>.Success(result);
            }
            catch (System.Exception ex)
            {
                return ResponseResult<RuleExecutionResultEntity>.Error(ex.Message);
            }
        }
    }
}

