using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Engine.Business.Entity;
using Slickflow.Scheduler.Entity;

namespace Slickflow.Scheduler.Service
{
    /// <summary>
    /// 消息服务接口
    /// </summary>
    public interface IEMailService
    {
        IList<UserEMailEntity> GetUserList();
        void SendTaskEMail(IList<ProcessEntity> processList, IList<UserEMailEntity> userList);
    }
}
