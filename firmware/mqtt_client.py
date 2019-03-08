import paho.mqtt.client as mqtt
import os
import json
import threading

class MQTTClient(mqtt.Client):
    def __init__(self, hostname="localhost", port=1883, callback_dict=None):
        super(MQTTClient, self).__init__()
        # self.username_pw_set("", "")
        self.connect(hostname, port)

        self._callback_dict = {}# key, value pair; key -topic, value - function
    
    # Define event callbacks
    def on_connect(self, client, userdata, flags, rc):
        print("rc: " + str(rc))

    def on_message(self, client, obj, msg):
        print(msg.topic + " " + str(msg.qos) + " " + str(msg.payload))

        topic = msg.topic.split("/")
        user_id = topic[0]
        key = key = "".join(topic[1:])

        # To do: Add regex to check topic is of right format
        if key in self._callback_dict.keys():
            callback = self._callback_dict[key]
            message = (msg.payload).decode("utf-8") 
            payload = json.loads(message)
            try:
                thread = threading.Thread(target=callback, args=[obj, user_id, payload])
                thread.start()
            except Exception as e:
                print(e)
                print("Poorly constructed callback")
        else:
            pass

    def on_publish(self, client, obj, mid):
        print("mid: " + str(mid))

    def on_subscribe(self, client, obj, mid, granted_qos):
        print("Subscribed: " + str(mid) + " " + str(granted_qos))

    def on_log(self, client, obj, level, string):
        print("Log: " + string)
    
    # Subscribe to topic and assign callback
    def subscribe(self, topic, callback):
        try:
            key = "".join(topic.split("/")[1:])
            self._callback_dict[key] = callback
            super().subscribe(topic, 0)
        except Exception as e:
            print (e)
            print ("Topic not in right format")
    
    def publish(self, topic, payload):
        payload = json.dumps(payload)
        print("Publishing %s to %s" %(payload, topic))
        super().publish(topic, payload)
        
def test(obj, payload):
    print (payload)

def main():
    topic =  'test'
    mqttc = MQTTClient()
    mqttc.subscribe(topic, test)
    rc = 0
    while rc == 0:
        rc = mqttc.loop()
    print("rc: " + str(rc))

if __name__ == "__main__":
    main()