﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Slickflow.Module.Essential.Entity;

namespace Slickflow.Module.Essential.Common
{
    /// <summary>
    /// 消息队列服务
    /// </summary>
    public class RabbitMQService : IMessageQueueService
    {
        public Func<MessageEntity, MessageConsumedResult> ConsumeMessageFunction;
        public Func<SignalEntity, List<SignalConsumedResult>> ConsumeSignalFunction;

        internal RabbitMQService()
        {

        }

        /// <summary>
        /// 消息发布
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="line">内容</param>
        public void Publish(string topic, string line)
        {
            var channel = RabbitMQClientFactory.CreatePublishChannel();
            channel.QueueDeclare(queue: topic,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            var body = Encoding.UTF8.GetBytes(line);
            channel.BasicPublish(exchange: "",
                                 routingKey: topic,
                                 basicProperties: null,
                                 body: body);
        }

        /// <summary>
        /// 消息发布
        /// </summary>
        /// <param name="message">消息实体</param>
        public void Publish(MessageEntity message)
        {
            Publish(message.Topic, message.Line);
        }

        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="jobName">作业名称</param>
        /// <param name="strTriggerType">触发类型</param>
        /// <returns></returns>
        public void Subscribe(string topic, string jobName, string strTriggerType)
        {
            var channel = RabbitMQClientFactory.CreateRecieveChannel();
            channel.QueueDeclare(queue: topic,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var line = Encoding.UTF8.GetString(body.ToArray());

                if (strTriggerType == "Message")
                {
                    var msgMediator = new MessageMediator();
                    msgMediator.InvokeFromMessage(ConsumeMessageFunction, topic, line, jobName);
                }
                else if (strTriggerType == "Signal")
                {
                    var signalMediator = new SignalMediator();
                    signalMediator.InvokeFromSignal(ConsumeSignalFunction, topic, line, jobName);
                }
                else
                {
                    throw new ApplicationException(
                        string.Format("There isnt a mediator to handle this trigger type:{0}", strTriggerType)
                    );
                }
            };
            channel.BasicConsume(queue: topic,
                                 autoAck: true,
                                 consumer: consumer);
        }

        /// <summary>
        /// 取消订阅主题
        /// </summary>
        /// <param name="topic">消息主题</param>
        public void Unsubscribe(string topic)
        {
            var channel = RabbitMQClientFactory.CreateRecieveChannel();
            channel.QueueDelete(topic);
        }
    }
}