using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Slickflow.Module.Essential.Common
{
    /// <summary>
    /// RabbitMQ client factory
    /// MQ 客户端创建类
    /// </summary>
    public class RabbitMQClientFactory
    {
        private static string ServerName = "localhost";
        private static ConnectionFactory MQConnectionFactory = null;
        private static IConnection MQPublishConnection = null;
        private static IModel MQPublishChannel = null;
        private static IConnection MQRecieveConnection = null;
        private static IModel MQRecieveChannel = null;

        static RabbitMQClientFactory()
        {
            CreateMQServer();
        }

        /// <summary>
        /// initialize server
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
        /// Create a publishing client channel
        /// 创建发布客户端通道
        /// </summary>
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
        /// Create a receiving channel
        /// 创建接收端通道
        /// </summary>
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
