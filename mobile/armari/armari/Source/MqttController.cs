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
        private static string s_host = "test.mosquitto.org";
        private static int s_port = 1883;

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
            m_config = new MqttConfiguration { Port = s_port };
            m_client = await MqttClient.CreateAsync(s_host, m_config);
            var clientId = "clientIdhGHvpYY9uM";

            try {
                await m_client.ConnectAsync(new MqttClientCredentials(clientId));
                m_initialized = true;
            }
            catch(MqttClientException ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }

        public async void Publish(string topic, string payload)
        {
            if (!m_initialized) return;
            var message = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes($"{payload}"));
            await m_client.PublishAsync(message, MqttQualityOfService.AtLeastOnce);
        }

        public async void Subscribe(string topic, OnMessage callback)
        {
            if (!m_initialized) return;
            Console.WriteLine("Subscribe to {0}", topic);
            await m_client.SubscribeAsync(topic, MqttQualityOfService.AtMostOnce);
            m_client
                .MessageStream
                .Where(msg => msg.Topic == topic)
                .Subscribe(msg => callback(msg.Payload));
        }

        public async void Unsubscribe(string topic)
        {
            if (!m_initialized) return;
            await m_client.UnsubscribeAsync(topic);
        }
    }
}
