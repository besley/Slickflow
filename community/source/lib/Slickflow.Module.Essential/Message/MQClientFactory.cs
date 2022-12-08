using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Slickflow.Module.Essential.Message
{
    /// <summary>
    /// MQ 客户端创建类
    /// </summary>
    public class MQClientFactory
    {
        private static string ServerName = "localhost";
        private static ConnectionFactory MQConnectionFactory = null;
        private static IConnection MQPublishConnection = null;
        private static IModel MQPublishChannel = null;
        private static IConnection MQRecieveConnection = null;
        private static IModel MQRecieveChannel = null;

        static MQClientFactory()
        {
            CreateMQServer();
        }

        /// <summary>
        /// 初始化服务器
        /// </summary>
        private static void CreateMQServer()
        {
            if (MQConnectionFactory == null)
            {
                MQConnectionFactory = new ConnectionFactory() { HostName = ServerName };
            }
            
            CreatePublishChannel();
            CreateRecieveChannel();
        }

        /// <summary>
        /// 创建发布客户端通道
        /// </summary>
        /// <returns></returns>
        public static IModel CreatePublishChannel()
        {
            if (MQPublishConnection == null)
            {
                MQPublishConnection = MQConnectionFactory.CreateConnection();
            }

            if (MQPublishChannel == null)
            {
                MQPublishChannel = MQPublishConnection.CreateModel();
            }
            return MQPublishChannel;
        }

        /// <summary>
        /// 创建接收端通道
        /// </summary>
        /// <returns></returns>
        public static IModel CreateRecieveChannel()
        {
            if (MQRecieveConnection == null)
            {
                MQRecieveConnection = MQConnectionFactory.CreateConnection();
            }

            if (MQRecieveChannel == null)
            {
                MQRecieveChannel = MQRecieveConnection.CreateModel();
            }
            return MQRecieveChannel;
        }
    }
}
