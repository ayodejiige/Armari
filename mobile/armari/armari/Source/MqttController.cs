using System;
using System.Net.Mqtt;
using System.Threading.Tasks;
using System.Text;
using System.Reactive.Linq;

namespace armari
{

    public delegate void OnMessage(byte [] payload);


    public class MqttController
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

        public bool IsInitialized()
        {
            return m_initialized;
        }
        public async void UnInit()
        {
            await this.m_client.DisconnectAsync();
        }

        public async Task Init()
        {
            m_config = new MqttConfiguration { Port = s_port };
            m_client = await MqttClient.CreateAsync(s_host, m_config);
            var clientId = GenerateId();

            try
            {
                await m_client.ConnectAsync(new MqttClientCredentials(clientId));
                m_initialized = true;
                Application.logger.Message(String.Format("Loaded MQTT for client {0}", clientId));
            }
            catch(MqttClientException ex)
            {
                Application.logger.Error("MQTT Error", "Failed to load mqtt");
                Application.logger.Error("MQTT Error", ex.ToString());
            }
        }

        public void Publish(string topic, string payload)
        {
            if (m_initialized == false) return;
            string txt = string.Format("Publish to {0} => {1}", topic, payload);
            m_logger.Message(txt);
            var message = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes($"{payload}"));

            try
            {
                m_client.PublishAsync(message, MqttQualityOfService.AtMostOnce).Wait();
            }
            catch (AggregateException agg)
            {
                HandleAgrregateException(agg);
                m_client.PublishAsync(message, MqttQualityOfService.AtMostOnce).Wait();
            }
        }

        private void HandleAgrregateException(AggregateException agg)
        {
            //bool foundDisposedEx = false;
            foreach (Exception ex in agg.InnerExceptions) // iterate over all
            {
                Application.logger.Message(string.Format("Exception {0} => {1}", ex.GetType(), ex.ToString()));
                //if(ex is System.ObjectDisposedException)
                //{
                //    Init().Wait();
                //    foundDisposedEx = true;
                //}
            }

            Init().Wait();

            //if (foundDisposedEx == false)
            //{
            //    Application.logger.Error("MQTT Error", "Failed to load mqtt");
            //    Application.logger.Error("MQTT Error", agg.ToString());
            //}
        }
        public void Subscribe(string topic, OnMessage callback)
        {
            if (m_initialized == false) return;
            string txt = string.Format("Subscribe to {0}", topic);
            m_logger.Message(txt);
            try
            {
                m_client.SubscribeAsync(topic, MqttQualityOfService.AtLeastOnce).Wait();
                m_client
                .MessageStream
                .Where(msg => msg.Topic == topic)
                .Subscribe(msg => callback(msg.Payload));
            }
            catch (AggregateException agg)
            {
                HandleAgrregateException(agg);
                m_client.SubscribeAsync(topic, MqttQualityOfService.AtLeastOnce).Wait();
                m_client
                .MessageStream
                .Where(msg => msg.Topic == topic)
                .Subscribe(msg => callback(msg.Payload));
            }
        }

        public void Unsubscribe(string topic)
        {
            if (!m_initialized) return;
            string txt = string.Format("Unsubscribe from {0}", topic);
            m_logger.Message(txt);
            try
            {
                m_client.UnsubscribeAsync(topic).Wait();
            }
            catch (AggregateException agg)
            {
                HandleAgrregateException(agg);
                m_client.UnsubscribeAsync(topic).Wait();
            }
        }

        private static string GenerateId()
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(10);

            for (int i = 0; i < 10; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }
    }
}
