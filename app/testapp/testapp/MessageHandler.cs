using System;
using Newtonsoft.Json;
using System.Threading;

namespace testapp
{
    public struct Cloth
    {
        public string type;
    }

    public struct Status
    {
        public Int16 status;
    }

    public class MessageHandler
    {
        private MqttController m_mqttH;
        private ManualResetEvent m_finishedEvent;

        private static int s_timeOut = 5000;
        private static readonly string s_newItemInitTopic = "/wardrobe/new/init";
        private static readonly string s_newItemInsertedTopic = "/wardrobe/new/inserted";
        private static readonly string s_newItemStatusTopic = "/wardrobe/new/status";
        public MessageHandler()
        {
            m_finishedEvent = new ManualResetEvent(false);
            m_mqttH = new MqttController();
            m_mqttH.Init();
        }

        public void NewItemStatusCallback(byte[] payload)
        {
            m_finishedEvent.Set();
            Console.WriteLine("New Item Status Callback");
        }


        public void NewItemInit(string user_id, Cloth cloth)
        {
            string topic = user_id + s_newItemStatusTopic;
            m_mqttH.Subscribe(topic, NewItemStatusCallback);

            topic = user_id + s_newItemInitTopic;
            string payload = JsonConvert.SerializeObject(cloth);

            Console.WriteLine("Adding New Item");
            m_mqttH.Publish(topic, payload);
        }

        public bool NewItemFinish(string user_id, Status status)
        {
            string topic = user_id + s_newItemInsertedTopic;
            string payload = JsonConvert.SerializeObject(status);
            m_mqttH.Publish(topic, payload);

            bool res = m_finishedEvent.WaitOne(s_timeOut);
            m_finishedEvent.Reset();

            topic = user_id + s_newItemStatusTopic;
            m_mqttH.Unsubscribe(topic);

            return res;
        }
    }
}
