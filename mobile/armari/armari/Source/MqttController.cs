using System;
using System.Net.Mqtt;
using System.Threading.Tasks;
using System.Text;
using System.Reactive.Linq;

namespace armari
{
    public delegate void OnMessage(byte [] payload);
    public sealed class MqttController
    {
        //private static MqttController m_instance = new MqttController();
        private bool m_initialized;
        private MqttConfiguration m_config;
        private IMqttClient m_client;
        private static string s_host = "broker.hivemq.com";
        private static int s_port = 1883;
        private Logger m_logger = Logger.Instance;

        public MqttController()
        {
            m_initialized = false;
        }

        ~MqttController()
        {
            if(m_client != null)
            {
                UnInit();
            }
        }
        //public static MqttController Instance
        //{
        //    get
        //    {
        //        return m_instance;
        //    }
        //}
        public bool IsInitialized()
        {
            return m_initialized;
        }
        public async void UnInit()
        {
            await this.m_client.DisconnectAsync();
        }

        public async void Init(string id)
        {
            m_config = new MqttConfiguration { Port = s_port };
            m_client = await MqttClient.CreateAsync(s_host, m_config);
            var clientId = id;

            try {
                await m_client.ConnectAsync(new MqttClientCredentials(clientId));
                m_initialized = true;
                m_logger.Message("Loaded MQTT");
            }
            catch(MqttClientException ex)
            {
                m_logger.Error("Failed to load mqtt");
                m_logger.Error(ex.ToString());
            }
        }

        public void Publish(string topic, string payload)
        {
            if (m_initialized == false) return;
            string txt = string.Format("Publish to {0} => {1}", topic, payload);
            m_logger.Message(txt);
            var message = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes($"{payload}"));
            m_client.PublishAsync(message, MqttQualityOfService.AtMostOnce).Wait();
        }

        public void Subscribe(string topic, OnMessage callback)
        {
            if (m_initialized == false) return;
            string txt = string.Format("Subscribe to {0}", topic);
            m_logger.Message(txt);
            m_client.SubscribeAsync(topic, MqttQualityOfService.AtLeastOnce).Wait();
            m_client
                .MessageStream
                .Where(msg => msg.Topic == topic)
                .Subscribe(msg => callback(msg.Payload));
        }

        public void Unsubscribe(string topic)
        {
            if (!m_initialized) return;
            string txt = string.Format("Unsubscribe from {0}", topic);
            m_logger.Message(txt);
            m_client.UnsubscribeAsync(topic).Wait();
        }
    }
}
