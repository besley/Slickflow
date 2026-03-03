using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Event;
using Slickflow.Engine.Core.Event;
using Slickflow.Engine.Core.Runtime;

namespace Slickflow.Engine.Service
{
    /// <summary>
    /// Workflow Service - Process Execution
    /// 工作流服务 - 流程执行部分
    /// </summary>
    public partial class WorkflowService : IWorkflowService
    {
        /// <summary>
        /// Lock object
        /// </summary>
        private static readonly object startLock = new object();
        private static readonly object runLock = new object();

        #region Process Startup
        /// <summary>
        /// Process Startup
        /// Coding Example:
        /// var wfResult = wfService.CreateRunner(runner.UserId, runner.UserName)
        ///                 .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessId, runner.Version)
        ///                 .Start();
        /// </summary>
        public WfExecutedResult Start()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Start(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }            
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process Startup
        /// Coding Example:
        /// var wfResult = wfService.CreateRunner(runner.UserId, runner.UserName)
        ///                 .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessId, runner.Version)
        ///                 .Start(conn, trans);
        /// </summary>
        public WfExecutedResult Start(IDbConnection conn, IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult startedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //��������ִ��
            //Start method execution
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                lock (startLock)
                {
                    session = SessionFactory.CreateSession(conn, trans);
                    runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceStartup(runner, ref startedResult);

                    if (startedResult.Status == WfExecutedStatus.Exception)
                    {
                        return startedResult;
                    }

                    //���¼�
                    //Register event
                    WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                        runtimeInstance_OnWfProcessStarting,
                        runtimeInstance_OnWfProcessStarted);

                    bool isStarted = runtimeInstance.Execute(session);
                }
                //do some thing else here...
                //...
                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                startedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                startedResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.startprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_START_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessStarting, 
                   runtimeInstance_OnWfProcessStarted);
                waitHandler.Dispose();
            }
            return startedResult;

            void runtimeInstance_OnWfProcessStarting(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessStarting,
                        runner.EventSubscriptionList,
                        startedResult.ProcessInstanceIdStarted);
            }

            void runtimeInstance_OnWfProcessStarted(object sender, WfEventArgs args)
            {
                startedResult = args.WfExecutedResult;
                if (startedResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session, 
                        EventFireTypeEnum.OnProcessStarted,
                        runner.EventSubscriptionList,
                        startedResult.ProcessInstanceIdStarted);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Start process
        /// </summary>
        public WfExecutedResult StartProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Start();

            return result;
        }

        /// <summary>
        /// Start process async
        /// </summary>
        public async Task<WfExecutedResult> StartProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() => 
            {
                return StartProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Start process
        /// </summary>
        public WfExecutedResult StartProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Start(conn, trans);

            return result;
        }

        /// <summary>
        /// Start process async
        /// </summary>
        public async Task<WfExecutedResult> StartProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return StartProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Startup by Message
        /// <summary>
        /// Start by Message
        /// 消息启动流程
        /// </summary>
        public WfExecutedResult StartByMessage()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = StartByMessage(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                {
                    trans.Commit();
                }
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Start by Message
        /// 消息启动流程
        /// </summary>
        public WfExecutedResult StartByMessage(IDbConnection conn, IDbTransaction trans)
        {
            var pm = new ProcessManager();
            var entity = pm.GetByMessage(_wfAppRunner.MessageTopic);
            _wfAppRunner.ProcessId = entity.ProcessId;
            _wfAppRunner.ProcessCode = entity.ProcessCode;
            _wfAppRunner.Version = entity.Version;

            _wfAppRunner.UserId = WfDefine.SYSTEM_INTERNAL_USER_ID;
            _wfAppRunner.UserName = WfDefine.SYSTEM_INTERNAL_USER_NAME;

            var result = Start(conn, trans);
            return result;
        }

        /// <summary>
        /// Start by Message
        /// 流程流转
        /// </summary>
        public WfExecutedResult StartProcessByMessage(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = StartByMessage();
            return result;
        }

        /// <summary>
        /// Start by Message Async
        /// 异步消息启动流程
        /// </summary>
        public async Task<WfExecutedResult> StartProcessByMessageAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return StartProcessByMessage(runner);
            });
            return task;
        }

        /// <summary>
        /// Start by Message
        /// 流程流转
        /// </summary>
        public WfExecutedResult StartProcessByMessage(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = StartByMessage(conn, trans);

            return result;
        }

        /// <summary>
        /// Start by Message Async
        /// 异步启动流程
        /// </summary>
        public async Task<WfExecutedResult> StartProcessByMessageAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return StartProcessByMessage(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Run
        /// <summary>
        /// Running process
        /// Coding example:
        /// var wfResult = wfService.CreateRunner(runner.UserId, runner.UserName)
        ///                 .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessId, runner.Version)
        ///                 .Run();
        /// </summary>
        public WfExecutedResult Run()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Run(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Running process
        /// Coding example:
        /// var wfResult = wfService.CreateRunner(runner.UserId, runner.UserName)
        ///                 .UseApp(runner.AppInstanceId, runner.AppName, runner.AppInstanceCode)
        ///                 .UseProcess(runner.ProcessId, runner.Version)
        ///                 .Run(conn, trans);
        /// </summary>
        public WfExecutedResult Run(IDbConnection conn, 
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult runAppResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //���з�����ʼִ��
            //The running method starts executing
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                lock (runLock)
                {
                    session = SessionFactory.CreateSession(conn, trans);
                    runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceAppRunning(runner, session, ref runAppResult);
                    if (runAppResult.Status == WfExecutedStatus.Exception)
                    {
                        return runAppResult;
                    }

                    //ע���¼�������
                    //Register event
                    WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                        runtimeInstance_OnWfProcessRunning,
                        runtimeInstance_OnWfProcessContinued);
                    bool isRun = runtimeInstance.Execute(session);
                }
                //do some thing else here...
                //...
                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                runAppResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                runAppResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.runprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessRunning,
                    runtimeInstance_OnWfProcessContinued);
                waitHandler.Dispose();
            }
            return runAppResult;

            void runtimeInstance_OnWfProcessRunning(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessRunning,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId); 
            }

            void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
            {
                runAppResult = args.WfExecutedResult;
                if (runAppResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessContinued,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process Run
        /// Explanation: The new method uniformly calls RunProcess()
        /// 流程流转
        /// 说明：新方法统一调用 RunProcess()
        /// </summary>
        public WfExecutedResult RunProcessApp(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Run();

            return result;
        }

        /// <summary>
        /// Process Run
        /// Explanation: The new method uniformly calls RunProcess()
        /// 流程流转
        /// 说明：新方法统一调用 RunProcess()
        /// </summary>
        public WfExecutedResult RunProcessApp(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Run(conn, trans);

            return result;
        }

        /// <summary>
        /// Run Process
        /// 流程流转
        /// </summary>
        public WfExecutedResult RunProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Run();

            return result;
        }

        /// <summary>
        /// Run Process Async
        /// 异步执行流程
        /// </summary>
        public async Task<WfExecutedResult> RunProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RunProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Run Process
        /// 流程流转
        /// </summary>
        public WfExecutedResult RunProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Run(conn, trans);

            return result;
        }

        /// <summary>
        /// Run Process Async
        /// 异步流程流转
        /// </summary>
        public async Task<WfExecutedResult> RunProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RunProcessApp(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Run Auto
        /// <summary>
        /// Run Automatically
        /// 自动运行
        /// </summary>
        public WfExecutedResult RunAuto()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = RunAuto(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Run Automatically
        /// 自动运行
        /// </summary>
        public WfExecutedResult RunAuto(IDbConnection conn,
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult runAppResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //���з�����ʼִ��
            //The running method starts executing
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                lock (runLock)
                {
                    session = SessionFactory.CreateSession(conn, trans);
                    runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceRunAuto(runner, session, ref runAppResult);
                    if (runAppResult.Status == WfExecutedStatus.Exception)
                    {
                        return runAppResult;
                    }

                    //ע���¼�������
                    //Register event
                    WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                        runtimeInstance_OnWfProcessRunning,
                        runtimeInstance_OnWfProcessContinued);
                    bool isRun = runtimeInstance.Execute(session);
                }
                //do some thing else here...
                //...
                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                runAppResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                runAppResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.runprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_RUN_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance,
                    runtimeInstance_OnWfProcessRunning,
                    runtimeInstance_OnWfProcessContinued);
                waitHandler.Dispose();
            }
            return runAppResult;

            void runtimeInstance_OnWfProcessRunning(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessRunning,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessContinued(object sender, WfEventArgs args)
            {
                runAppResult = args.WfExecutedResult;
                if (runAppResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessContinued,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Run process auto
        /// 流程执行自动
        /// </summary>
        public WfExecutedResult RunProcessAuto(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = RunAuto();

            return result;
        }

        /// <summary>
        /// Run process auto async
        /// 异步执行自动
        /// </summary>
        public async Task<WfExecutedResult> RunProcessAutoAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RunProcessAuto(runner);
            });
            return task;
        }

        /// <summary>
        /// Run Process auto
        /// 流程执行自动
        /// </summary>
        public WfExecutedResult RunProcessAuto(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = RunAuto(conn, trans);

            return result;
        }

        /// <summary>
        /// Run process auto async
        /// 异步流程执行自动
        /// </summary>
        public async Task<WfExecutedResult> RunProcessAutoAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RunProcessAuto(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Withdraw
        /// <summary>
        /// Process withdraw
        /// 流程撤回
        /// </summary>
        public WfExecutedResult Withdraw()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Withdraw(conn, trans);

                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process withdraw
        /// 流程撤回
        /// </summary>
        public WfExecutedResult Withdraw(IDbConnection conn, IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult withdrawedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //����������ʼִ��
            //Withdraw method begins execution
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceWithdraw(runner, ref withdrawedResult);

                //�����㳷�������������쳣�����Ϣ
                //Not satisfied with withdraw operation, return exception
                if (withdrawedResult.Status == WfExecutedStatus.Exception)
                {
                    return withdrawedResult;
                }
                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                    runtimeInstance_OnWfProcessWithdrawing,
                    runtimeInstance_OnWfProcessWithdrawn);
                bool isWithdrawn = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                withdrawedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                withdrawedResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.withdrawprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_WITHDRAW_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessWithdrawing,
                    runtimeInstance_OnWfProcessWithdrawn);
                waitHandler.Dispose();
            }
            return withdrawedResult;

            void runtimeInstance_OnWfProcessWithdrawing(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessWithdrawing,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessWithdrawn(object sender, WfEventArgs args)
            {
                withdrawedResult = args.WfExecutedResult;
                if (withdrawedResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessWithdrawn,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process withdraw
        /// 流程撤回
        /// </summary>
        public WfExecutedResult WithdrawProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Withdraw();

            return result;
        }

        /// <summary>
        /// Process withdraw async
        /// 异步撤回流程
        /// </summary>
        public async Task<WfExecutedResult> WithdrawProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return WithdrawProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Process withdraw
        /// 流程流转
        /// </summary>
        public WfExecutedResult WithdrawProcess(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Withdraw(conn, trans);

            return result;
        }

        /// <summary>
        /// Process withdraw async
        /// 异步撤回流程
        /// </summary>
        public async Task<WfExecutedResult> WithdrawProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return WithdrawProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Sendback
        /// <summary>
        /// Process sendback
        /// 流程退回
        /// </summary>
        public WfExecutedResult SendBack()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = SendBack(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process sendback
        /// 流程退回
        /// </summary>
        public WfExecutedResult SendBack(IDbConnection conn, 
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult sendbackResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //�˻ؿ�ʼ
            //sendback operation
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceSendBack(runner, ref sendbackResult);

                if (sendbackResult.Status == WfExecutedStatus.Exception)
                {
                    return sendbackResult;
                }

                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessSendBacking,
                    runtimeInstance_OnWfProcessSendBacked);
                bool isSendBacked = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                sendbackResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                sendbackResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.sendbackprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_SENDBACK_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessSendBacking,
                    runtimeInstance_OnWfProcessSendBacked);
                waitHandler.Dispose();
            }
            return sendbackResult;


            void runtimeInstance_OnWfProcessSendBacking(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessSendBacking,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessSendBacked(object sender, WfEventArgs args)
            {
                sendbackResult = args.WfExecutedResult;
                if (sendbackResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessSendBacked,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process sendback
        /// 退回到下一步
        /// </summary>
        public WfExecutedResult SendBackProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = SendBack();

            return result;
        }

        /// <summary>
        /// Process sendback async
        /// 异步退回到下一步
        /// </summary>
        public async Task<WfExecutedResult> SendBackProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return SendBackProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Process sendback
        /// 退回到下一步
        /// </summary>
        public WfExecutedResult SendBackProcess(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = SendBack(conn, trans);

            return result;
        }

        /// <summary>
        /// Process sendback async
        /// 异步退回到下一步
        /// </summary>
        public async Task<WfExecutedResult> SendBackProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return SendBackProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Resend
        /// <summary>
        /// Process Resend
        /// 流程转发
        /// </summary>
        public WfExecutedResult Resend()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Resend(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process Resend
        /// 流程转发
        /// </summary>
        public WfExecutedResult Resend(IDbConnection conn, 
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult resendResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //���ͷ�����ʼִ��
            //Resend start to execute
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceResend(runner, ref resendResult);

                if (resendResult.Status == WfExecutedStatus.Exception)
                {
                    return resendResult;
                }

                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessResending,
                    runtimeInstance_OnWfProcessResent);
                bool isResent = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                resendResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                resendResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.resendprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_RESEND_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessResending,
                    runtimeInstance_OnWfProcessResent);
                waitHandler.Dispose();
            }
            return resendResult;

            void runtimeInstance_OnWfProcessResending(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessResending,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessResent(object sender, WfEventArgs args)
            {
                resendResult = args.WfExecutedResult;
                if (resendResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessResent,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process Resend
        /// 重新转发（退回到的节点）
        /// </summary>
        public WfExecutedResult ResendProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Resend();

            return result;
        }

        /// <summary>
        /// Process Resend Async
        /// 异步转发流程
        /// </summary>
        public async Task<WfExecutedResult> ResendProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ResendProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Process Resend
        /// 重新转发（退回到的节点）
        /// </summary>
        public WfExecutedResult ResendProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Resend(conn, trans);

            return result;
        }

        /// <summary>
        /// Process Resend Async
        /// 异步转发流程
        /// </summary>
        public async Task<WfExecutedResult> ResendProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ResendProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Revise
        /// <summary>
        /// Process revise
        /// (Reassignment of tasks to returned nodes)
        /// 流程修订
        /// (�˻غ�Ľڵ�����ָ������)
        /// </summary>
        public WfExecutedResult Revise()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Revise(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();
                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process revise
        /// 流程修订
        /// </summary>
        public WfExecutedResult Revise(IDbConnection conn,
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult reviseResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //�޶�������ʼִ��
            //Revise method start to execute
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceRevise(runner, ref reviseResult);

                if (reviseResult.Status == WfExecutedStatus.Success)
                {
                    return reviseResult;
                }

                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessRevising,
                    runtimeInstance_OnWfProcessRevised);
                bool isRevised = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch(System.Exception e)
            {
                reviseResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                reviseResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.reviseprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_REVISE_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ע���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessRevising,
                    runtimeInstance_OnWfProcessRevised);
                waitHandler.Dispose();
            }
            return reviseResult;

            void runtimeInstance_OnWfProcessRevising(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessRevising,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessRevised(object sendder, WfEventArgs args)
            {
                reviseResult = args.WfExecutedResult;
                if (reviseResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessRevised,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process revise
        /// 流程修订（退回后的节点重新指派任务）
        /// </summary>
        public WfExecutedResult ReviseProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Revise();

            return result;
        }

        /// <summary>
        /// Process revise async
        /// 异步修订流程
        /// </summary>
        public async Task<WfExecutedResult> ReviseProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ReviseProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Process revise
        /// 流程修订（退回后的节点重新指派任务）
        /// </summary>
        public WfExecutedResult ReviseProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Revise(conn, trans);

            return result;
        }

        /// <summary>
        /// Process revise async
        /// 异步修订流程
        /// </summary>
        public async Task<WfExecutedResult> ReviseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ReviseProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Reverse
        /// <summary>
        /// Process reverse
        /// (Completed processes can be reversed)
        /// 流程反签
        /// (�Ѿ����������̿��Ա����
        /// </summary>
        public WfExecutedResult Reverse()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Reverse(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process reverse
        /// 流程反签
        /// </summary>
        public WfExecutedResult Reverse(IDbConnection conn, IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult reversedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //��ǩ������ʼִ��
            //Reverse method start to execute
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceReverse(runner, ref reversedResult);

                if (reversedResult.Status == WfExecutedStatus.Exception)
                {
                    return reversedResult;
                }

                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessReversing,
                    runtimeInstance_OnWfProcessReversed);
                bool isReversed = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                reversedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                reversedResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.reverseprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_REVERSE_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessReversing,
                    runtimeInstance_OnWfProcessReversed);
                waitHandler.Dispose();
            }
            return reversedResult;

            void runtimeInstance_OnWfProcessReversing(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessReversing,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessReversed(object sender, WfEventArgs args)
            {
                reversedResult = args.WfExecutedResult;
                if (reversedResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessReversed,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process reverse
        /// 流程反签
        /// </summary>
        public WfExecutedResult ReverseProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Reverse();

            return result;
        }

        /// <summary>
        /// Process reverse async
        /// 异步反签流程
        /// </summary>
        public async Task<WfExecutedResult> ReverseProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ReverseProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Process reverse
        /// 流程反签
        /// </summary>
        public WfExecutedResult ReverseProcess(IDbConnection conn, 
            WfAppRunner runner, 
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Reverse(conn, trans);

            return result;
        }

        /// <summary>
        /// Process reverse async
        /// 异步反签流程
        /// </summary>
        public async Task<WfExecutedResult> ReverseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return ReverseProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Reject
        /// <summary>
        /// Process reject
        /// 流程驳回
        /// </summary>
        public WfExecutedResult Reject()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Reject(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();
                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process reject
        /// 流程驳回
        /// </summary>
        public WfExecutedResult Reject(IDbConnection conn, 
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            

            WfExecutedResult rejectedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //���ط�����ʼִ��
            //Reject method start to execute
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                runner.NextActivityPerformers = FillNextActivityPerforms(runner, JumpOptionEnum.Startup);
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceReject(runner, ref rejectedResult);
                if (rejectedResult.Status == WfExecutedStatus.Exception)
                {
                    return rejectedResult;
                }

                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                    runtimeInstance_OnWfProcessRejecting,
                    runtimeInstance_OnWfProcessRejected);
                bool isRejected = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                rejectedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                rejectedResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.rejectprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_REJECT_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance,
                    runtimeInstance_OnWfProcessRejecting,
                    runtimeInstance_OnWfProcessRejected);
                waitHandler.Dispose();
            }
            return rejectedResult;

            void runtimeInstance_OnWfProcessRejecting(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessRejecting,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessRejected(object sender, WfEventArgs args)
            {
                rejectedResult = args.WfExecutedResult;
                if (rejectedResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessRejected,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process reject
        /// 流程驳回
        /// </summary>
        public WfExecutedResult RejectProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Reject();

            return result;
        }


        /// <summary>
        /// Process reject async
        /// 异步驳回流程
        /// </summary>
        public async Task<WfExecutedResult> RejectProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RejectProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Process reject
        /// 流程驳回
        /// </summary>
        public WfExecutedResult RejectProcess(IDbConnection conn,
            WfAppRunner runner,
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Reject(conn, trans);

            return result;
        }

        /// <summary>
        /// Process reject async
        /// 异步驳回流程
        /// </summary>
        public async Task<WfExecutedResult> RejectProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return RejectProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Close
        /// <summary>
        /// Process Close
        /// 流程关闭
        /// </summary>
        public WfExecutedResult Close()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Close(conn, trans);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();
                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        ///  Process Close
        /// 流程关闭
        /// </summary>
        public WfExecutedResult Close(IDbConnection conn, 
            IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult closedResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //�رշ�����ʼִ��
            //Close method start to execute
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                runner.NextActivityPerformers = FillNextActivityPerforms(runner, JumpOptionEnum.End);
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceClose(runner, ref closedResult);
                if (closedResult.Status == WfExecutedStatus.Exception)
                {
                    return closedResult;
                }

                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance,
                    runtimeInstance_OnWfProcessClosing,
                    runtimeInstance_OnWfProcessClosed);
                bool isRejected = runtimeInstance.Execute(session);
            }
            catch (System.Exception e)
            {
                closedResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                closedResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.closeprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_CLOSE_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance,
                    runtimeInstance_OnWfProcessClosing,
                    runtimeInstance_OnWfProcessClosed);
                waitHandler.Dispose();
            }
            return closedResult;

            void runtimeInstance_OnWfProcessClosing(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessClosing,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessClosed(object sender, WfEventArgs args)
            {
                closedResult = args.WfExecutedResult;
                if (closedResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessClosed,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        ///  Process Close
        /// 流程关闭
        /// </summary>
        public WfExecutedResult CloseProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = Close();

            return result;
        }

        /// <summary>
        ///  Process Close Async
        /// 异步关闭流程
        /// </summary>
        public async Task<WfExecutedResult> CloseProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return CloseProcess(runner);
            });
            return task;
        }

        /// <summary>
        ///  Process Close
        /// 流程关闭
        /// </summary>
        public WfExecutedResult CloseProcess(IDbConnection conn,
            WfAppRunner runner,
            IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = Close(conn, trans);

            return result;
        }

        /// <summary>
        ///  Process Close Async
        /// 异步关闭流程
        /// </summary>
        public async Task<WfExecutedResult> CloseProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return CloseProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Jump
        /// <summary>
        /// Process Jump
        /// 流程跳转
        /// </summary>
        public WfExecutedResult Jump(JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                var result = Jump(conn, trans, jumpOption);
                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process Jump
        /// 流程跳转
        /// </summary>
        public WfExecutedResult Jump(IDbConnection conn,
            IDbTransaction trans,
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            var runner = _wfAppRunner;
            WfExecutedResult jumpResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //��ת������ʼִ��
            //Jump method start to execute
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                runner.NextActivityPerformers = FillNextActivityPerforms(runner, jumpOption);
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceJump(runner, ref jumpResult);

                if (jumpResult.Status == WfExecutedStatus.Exception)
                {
                    return jumpResult;
                }

                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessJumping,
                    runtimeInstance_OnWfProcessJumped);          
                bool isJumped = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                jumpResult.ExceptionType = WfExceptionType.Jump_OtherError;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                jumpResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.jumpprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_JUMP_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessJumping,
                    runtimeInstance_OnWfProcessJumped);
                waitHandler.Dispose();
            }
            return jumpResult;

            void runtimeInstance_OnWfProcessJumping(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessJumping,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessJumped(object sender, WfEventArgs args)
            {
                jumpResult = args.WfExecutedResult;
                if (jumpResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessJumped,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process Jump
        /// 流程跳转
        /// </summary>
        public WfExecutedResult JumpProcess(WfAppRunner runner, 
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            _wfAppRunner = runner;
            var result = Jump(jumpOption);

            return result;
        }

        /// <summary>
        /// Process Jump Async
        /// 异步跳转流程
        /// </summary>
        public async Task<WfExecutedResult> JumpProcessAsync(WfAppRunner runner, JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return JumpProcess(runner, jumpOption);
            });
            return task;
        }

        /// <summary>
        /// Process Jump
        /// 流程跳转
        /// </summary>
        public WfExecutedResult JumpProcess(IDbConnection conn,
            WfAppRunner runner,
            IDbTransaction trans,
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            _wfAppRunner = runner;
            var result = Jump(conn, trans, jumpOption);

            return result;
        }

        /// <summary>
        /// Process Jump Async
        /// 异步跳转流程
        /// </summary>
        public async Task<WfExecutedResult> JumpProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans,
            JumpOptionEnum jumpOption = JumpOptionEnum.Default)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return JumpProcess(conn, runner, trans, jumpOption);
            });
            return task;
        }

        /// <summary>
        /// Regenerate the list of performers for the jump activity
        /// 流程跳转动作执行人员列表
        /// </summary>
        private IDictionary<string, PerformerList> FillNextActivityPerforms(WfAppRunner runner,
            JumpOptionEnum jumpOption)
        {
            IDictionary<string, PerformerList> nextActivityPerformers = null;
            var pm = ProcessModelFactory.CreateByProcess(runner.ProcessId, runner.Version);
            if (jumpOption == JumpOptionEnum.Default)
            {
                nextActivityPerformers = runner.NextActivityPerformers;
            }
            else if (jumpOption == JumpOptionEnum.Startup)
            {
                var firstActivity = pm.GetFirstActivity();
                var aim = new ActivityInstanceManager();
                var firstActivityInstance = aim.GetActivityInstanceLatest(runner.AppInstanceId, runner.ProcessId, firstActivity.ActivityId);
                nextActivityPerformers = new Dictionary<string, PerformerList>();
                var performer = new Performer(firstActivityInstance.CreatedUserId, firstActivityInstance.CreatedUserName);
                var performerList = new PerformerList();
                performerList.Add(performer);
                nextActivityPerformers.Add(firstActivity.ActivityId, performerList);
            }
            else if (jumpOption == JumpOptionEnum.End)
            {
                var endActivity = pm.GetEndActivity();
                nextActivityPerformers = new Dictionary<string, PerformerList>();
                var performer = new Performer(runner.UserId, runner.UserName);
                var performerList = new PerformerList();
                performerList.Add(performer);
                nextActivityPerformers.Add(endActivity.ActivityId, performerList);
            }
            return nextActivityPerformers;
        }
        #endregion

        #region Process Signforward
        /// <summary>
        /// Process Signforward
        /// 流程会签
        /// </summary>
        public WfExecutedResult SignForward()
        {
            IDbConnection conn = SessionFactory.CreateConnection();
            IDbTransaction trans = null;

            try
            {
                trans = conn.BeginTransaction();
                var result = SignForward(conn, trans);

                if (result.Status == WfExecutedStatus.Success)
                    trans.Commit();
                else
                    trans.Rollback();

                return result;
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// Process Signforward
        /// 流程会签
        /// </summary>
        public WfExecutedResult SignForward(IDbConnection conn, IDbTransaction trans)
        {
            var runner = _wfAppRunner;
            WfExecutedResult signforwardResult = WfExecutedResult.Default();
            AutoResetEvent waitHandler = new AutoResetEvent(false);

            //��ǩ������ʼִ��
            //Signforward start to execute
            WfRuntimeManager runtimeInstance = null;
            IDbSession session = null;
            try
            {
                session = SessionFactory.CreateSession(conn, trans);
                runtimeInstance = WfRuntimeManagerFactory.CreateRuntimeInstanceSignForward(runner, ref signforwardResult);

                if (signforwardResult.Status == WfExecutedStatus.Exception)
                {
                    return signforwardResult;
                }
                //ע���¼�������
                //Register event
                WfRuntimeManagerFactory.RegisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessSignForwarding,
                    runtimeInstance_OnWfProcessSignForwarded);

                bool isSignForwarding = runtimeInstance.Execute(session);

                waitHandler.WaitOne();
            }
            catch (System.Exception e)
            {
                signforwardResult.Status = WfExecutedStatus.Failed;
                var error = e.InnerException != null ? e.InnerException.Message : e.Message;
                signforwardResult.Message = LocalizeHelper.GetEngineMessage("workflowservice.signforwardprocess.error", error);
                LogManager.RecordLog(WfDefine.WF_PROCESS_SIGN_FORWARD_ERROR, LogEventType.Error, LogPriority.High, runner, e);
            }
            finally
            {
                //ж���¼�
                //Unregister event
                WfRuntimeManagerFactory.UnregisterEvent(runtimeInstance, 
                    runtimeInstance_OnWfProcessSignForwarding,
                    runtimeInstance_OnWfProcessSignForwarded);
                waitHandler.Dispose();
            }
            return signforwardResult;

            void runtimeInstance_OnWfProcessSignForwarding(object sender, WfEventArgs args)
            {
                Event.EventExecutor.InvokeExternalEvent(session,
                    EventFireTypeEnum.OnProcessSignForwarding,
                    runner.EventSubscriptionList,
                    runtimeInstance.ProcessInstanceId);
            }

            void runtimeInstance_OnWfProcessSignForwarded(object sender, WfEventArgs args)
            {
                signforwardResult = args.WfExecutedResult;
                if (signforwardResult.Status == WfExecutedStatus.Success)
                {
                    Event.EventExecutor.InvokeExternalEvent(session,
                        EventFireTypeEnum.OnProcessSignForwarded,
                        runner.EventSubscriptionList,
                        runtimeInstance.ProcessInstanceId);
                }
                waitHandler.Set();
            }
        }

        /// <summary>
        /// Process Signforward
        /// 会签
        /// </summary>
        public WfExecutedResult SignForwardProcess(WfAppRunner runner)
        {
            _wfAppRunner = runner;
            var result = SignForward();

            return result;
        }

        /// <summary>
        /// Process Signforward Async
        /// 异步流程会签
        /// </summary>
        public async Task<WfExecutedResult> SignForwardProcessAsync(WfAppRunner runner)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return SignForwardProcess(runner);
            });
            return task;
        }

        /// <summary>
        /// Process Signforward
        /// 流程会签
        /// </summary>
        public WfExecutedResult SignForwardProcess(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            _wfAppRunner = runner;
            var result = SignForward(conn, trans);

            return result;
        }

        /// <summary>
        /// Process Signforward Async
        /// 异步流程会签
        /// </summary>
        public async Task<WfExecutedResult> SignForwardProcessAsync(IDbConnection conn, WfAppRunner runner, IDbTransaction trans)
        {
            var task = await Task.Run<WfExecutedResult>(() =>
            {
                return SignForwardProcess(conn, runner, trans);
            });
            return task;
        }
        #endregion

        #region Process Resume Suspend Cancel Discard Terminate
        /// <summary>
        /// Resume process instance (only applicable to suspended operations)
        /// Suspend (restore) processes, cancel (run) processes, discard processes in progress or completed
        /// 恢复流程实例(只针对已挂起)
        /// 挂起(恢复)流程、取消(运行)流程、撤回执行或执行中的流程
        /// </summary>
        public bool ResumeProcess(int processInstanceId, WfAppRunner runner)
        {
            bool result = true;
            try
            {
                var pim = new ProcessInstanceManager();
                pim.Resume(processInstanceId, runner, SessionFactory.CreateSession());
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Process Suspend
        /// 流程挂起实例
        /// </summary>
        public bool SuspendProcess(int taskId, WfAppRunner runner)
        {
            bool result = true;
            try
            {
                var pim = new ProcessInstanceManager();
                var taskMng = new TaskManager();
                TaskEntity taskentity = taskMng.GetTask(taskId);
                pim.Suspend(taskentity.ProcessInstanceId, runner, SessionFactory.CreateSession());
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Process Cancel
        /// 取消流程
        /// </summary>
        public bool CancelProcess(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Cancel(runner);
        }

        /// <summary>
        /// Process Discard
        /// 流程流转
        /// </summary>
        public bool DiscardProcess(WfAppRunner runner)
        {
            var pim = new ProcessInstanceManager();
            return pim.Discard(runner);
        }

        /// <summary>
        /// Process Terminate
        /// 终止流程
        /// </summary>
        public bool TerminateProcess(WfAppRunner terminator)
        {
            var pim = new ProcessInstanceManager();
            return pim.Terminate(terminator);
        }

        /// <summary>
        /// Process Terminate
        /// 终止流程实例
        /// </summary>
        public bool TerminateProcess(IDbConnection conn, ProcessInstanceEntity entity, string userId, string userName, IDbTransaction trans)
        {
            var pim = new ProcessInstanceManager();
            return pim.Terminate(conn, entity, userId, userName, trans);
        }
        #endregion

        #region Task Approval Agree Refuse
        /// <summary>
        /// Agree
        /// 同意
        /// </summary>
        public void AgreeTask(int taskId)
        {
            var aim = new ActivityInstanceManager();
            aim.Agree(taskId);
        }


        /// <summary>
        /// Refuse
        /// 拒绝
        /// </summary>
        public void RefuseTask(int taskId)
        {
            var aim = new ActivityInstanceManager();
            aim.Refuse(taskId);
        }
        #endregion
    }
}
