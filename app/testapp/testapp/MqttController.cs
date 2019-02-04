using System;
using System.Net.Mqtt;
using System.Threading.Tasks;
using System.Text;
using System.Reactive.Linq;

namespace testapp
{
    public delegate void OnMessage(byte [] payload);
    public sealed class MqttController
    {
        //private static MqttController m_instance = new MqttController();
        private bool m_initialized;
        private MqttConfiguration m_config;
        private IMqttClient m_client;

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

        private async void UnInit()
        {
            await this.m_client.DisconnectAsync();
        }

        public async void Init()
        {
            m_config = new MqttConfiguration { Port = 1883 };
            m_client = await MqttClient.CreateAsync("test.mosquitto.org", m_config);
            var clientId = "clientIdhGHvpYY9uM";
            await m_client.ConnectAsync(new MqttClientCredentials(clientId));
            m_initialized = true;
        }

        public async void Publish(string topic, string payload)
        {
            var message = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes($"{payload}"));
            await m_client.PublishAsync(message, MqttQualityOfService.AtLeastOnce);
        }

        public async void Subscribe(string topic, OnMessage callback)
        {
            Console.WriteLine("Subscribe to {0}", topic);
            await m_client.SubscribeAsync(topic, MqttQualityOfService.AtMostOnce);
            m_client
                .MessageStream
                .Where(msg => msg.Topic == topic)
                .Subscribe(msg => callback(msg.Payload));
        }

        public async void Unsubscribe(string topic)
        {
            await m_client.UnsubscribeAsync(topic);
        }
    }
}
