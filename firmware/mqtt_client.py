import paho.mqtt.client as mqtt
import os, urlparse

class MQTTClient(mqtt.Client):
    def __init__(self, hostname="localhost", port="1883", callback_dict=None):
        mqtt.Client.__init__(self)
        # self.username_pw_set("", "")
        self.connect(hostname, port)

        self._callback_dict = callback_dict # key, value pair; key -topic, value - function
        
    # Define event callbacks
    def on_connect(self, client, userdata, flags, rc):
        print("rc: " + str(rc))

    def on_message(self, client, obj, msg):
        print(msg.topic + " " + str(msg.qos) + " " + str(msg.payload))
        if msg.topic in self._callback_dict.keys():
            callback = self._callback_dict[msg.topic]
            try:
                callback(obj, msg.payload)
            except Exception as e:
                print("Poorly constructed callback")
        else:
            pass

    def on_publish(self, client, obj, mid):
        print("mid: " + str(mid))

    def on_subscribe(self, client, obj, mid, granted_qos):
        print("Subscribed: " + str(mid) + " " + str(granted_qos))

    def on_log(self, client, obj, level, string):
        print("Log: " + string)

def main():
    topic =  'test'
    mqttc = MQTTClient()
    mqttc.subscribe(topic, 0)
    rc = 0
    while rc == 0:
        rc = mqttc.loop()
    print("rc: " + str(rc))

if __name__ == "__main__":
    main()