using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace armari
{
    public struct Cloth
    {
        public string type;
    }

    public struct Status
    {
        public Int16 status;
    }

    public struct NewItemInit
    {
        public string type;
    }

    public struct Response
    {
        public Int32 status;
        public string id;
    }

    public struct WardrobeReq
    {
        public string type;
    }

    public struct WardrobeResponse
    {
        public List<int> ids;
    }

    public struct Location
    {
        public Int32 x;
        public Int32 y;
    }

    public class MessageHandler
    {
        //public static readonly MessageHandler Instance = new MessageHandler();

        //private bool m_isInitialized = false;
        public MqttController m_mqttH;
        private ManualResetEvent m_locationEvent;
        private ManualResetEvent m_statusEvent;
        private ManualResetEvent m_wardrobeEvent;
        private Logger m_logger = Logger.Instance;

        private static readonly int s_timeOut = 100000;
        private static readonly string s_locationTopic = "/wardrobe/location";
        private static readonly string s_newItemInitTopic = "/wardrobe/new/init";
        private static readonly string s_newItemInsertedTopic = "/wardrobe/new/done";
        private static readonly string s_newItemStatusTopic = "/wardrobe/new/status";

        private Location m_locationResponse;
        private Response m_currentNewItemResponse;
        private WardrobeResponse m_wardrobeResponse;
        //private string m_currentService = "";

        public MessageHandler(string id)
        {
            m_mqttH = new MqttController();
            m_locationEvent = new ManualResetEvent(false);
            m_statusEvent = new ManualResetEvent(false);
            m_wardrobeEvent = new ManualResetEvent(false);
            m_mqttH.Init(id);
        }

        public void Init()
        {
            //while (m_mqttH.IsInitialized() == false) {
            //    Task.Delay(100).Wait();
            //}

        }
        public void LocationCallback(byte[] payload)
        {
            string result = System.Text.Encoding.UTF8.GetString(payload);
            m_locationResponse = JsonConvert.DeserializeObject<Location>(result);
            m_locationEvent.Set();
            m_logger.Message(string.Format("New Location Callback {0}", result));
        }

        public void StatusCallback(byte[] payload)
        {
            string result = System.Text.Encoding.UTF8.GetString(payload);
            m_currentNewItemResponse = JsonConvert.DeserializeObject<Response>(result);
            m_statusEvent.Set();
            m_logger.Message(string.Format("Status Callback {0}", result));
        }

        public void WardrobeCallback(byte[] payload)
        {
            string result = System.Text.Encoding.UTF8.GetString(payload);
            m_wardrobeResponse = JsonConvert.DeserializeObject<WardrobeResponse>(result);
            m_wardrobeEvent.Set();
            m_logger.Message(string.Format("Wardrobe Callback {0}", result));
        }

        public List<int> GetWardrobe(string type)
        {
            string recvTopic = Application.USERID + "/wardrobe/get/items";
            string sendTopic = Application.USERID + "/wardrobe/get";

            m_mqttH.Subscribe(recvTopic, WardrobeCallback);

            // Send Request
            WardrobeReq req;
            req.type = type; 
            string payload = JsonConvert.SerializeObject(req);
            m_mqttH.Publish(sendTopic, payload);

            // Wait for location from raspi
            bool res = m_wardrobeEvent.WaitOne(s_timeOut);
            m_wardrobeEvent.Reset();
            m_mqttH.Unsubscribe(sendTopic);

            return m_wardrobeResponse.ids;
        }

        public Location ServiceInit<T>(string user_id, T item)
        {
            m_logger.Message("Service Init");
            Type type = typeof(T);
            string sendTopic = "";

            m_locationResponse.x = -1;
            m_locationResponse.x = -1;

            string locationTopic = "1" + s_locationTopic;
            string statusTopic = "1" + s_newItemStatusTopic;
            m_mqttH.Subscribe(locationTopic, LocationCallback);
            m_mqttH.Subscribe(statusTopic, StatusCallback);

            if (type == typeof(NewItemInit))
            {
                sendTopic = user_id + s_newItemInitTopic;
            }

            // Send message to raspi
            string payload = JsonConvert.SerializeObject(item);
            m_mqttH.Publish(sendTopic, payload);

            // Wait for location from raspi
            bool res = m_locationEvent.WaitOne(s_timeOut);
            m_locationEvent.Reset();
            m_mqttH.Unsubscribe(locationTopic);


            return m_locationResponse;
        }

        public Response ServiceFinish<T>(string user_id, Status status)
        {
            m_logger.Message("Service Finish");
            Type type = typeof(T);

            Response response = new Response();
            string statusTopic = "";
            string sendTopic = "";
            if (type == typeof(Response))
            {
                sendTopic = user_id + s_newItemInsertedTopic;
                statusTopic = user_id + s_newItemStatusTopic;
            }

            string payload = JsonConvert.SerializeObject(status);
            m_mqttH.Publish(sendTopic, payload);

            // To Do: alternatives to timeout
            bool res = m_statusEvent.WaitOne(s_timeOut);
            m_statusEvent.Reset();
            m_mqttH.Unsubscribe(statusTopic);

            if (type == typeof(Response))
            {
                response = m_currentNewItemResponse;
            }

            return response;
        }
    }
}
