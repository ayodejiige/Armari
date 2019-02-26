from flask import Flask
from flask_sqlalchemy import SQLAlchemy
import enum
import threading
import os

from database import DBManager
from mqtt_client import MQTTClient
from led import LedController

IS_PI = (os.uname()[4][:3] == 'arm')
app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:////home/ayodeji/test.db'
db = SQLAlchemy(app)

class Manager(object):
    _TIMEOUT = 20
    _HOST = "test.mosquitto.org"
    _PORT = 1883
    def __init__(self):
        if IS_PI:
            self._led = LedController()
        self._done_event = threading.Event()
        self._mqttc = MQTTClient(self._HOST, self._PORT)
        self._mqttc.subscribe("+/wardrobe/new/init", self.new_item_init)
        self._mqttc.subscribe("+/wardrobe/new/inserted", self.new_item_inserted)
        self._db_manager = DBManager(1)
    
    def run(self):
        print ("IS_PI ->>.", IS_PI) 
        rc = 0
        while rc == 0:
            rc = self._mqttc.loop()

    def new_item_init(self, obj, user_id, payload):
        compartment =  self._db_manager.get_compartment()
        x = 0
        y = 1
        # Turn on led
        if IS_PI:
            self._led.set_pixel(x, y, 1)

        # Wait for done
        ret = self._done_event.wait(self._TIMEOUT)
        print ("Event done")
        self._done_event.clear()

        # Turn off led
        if IS_PI:
            self._led.set_pixel(x, y, 0)

        if not ret:
            print ("Took too long")
        else:
            # Update database
            cloth_type = payload["type"]
            self._db_manager.new_cloth(compartment, cloth_type)
        
        payload = {"status": ret}
        topic = "%s/wardrobe/new/status" %user_id
        self._mqttc.publish(topic, payload)
        
    def new_item_inserted(self, obj, user_id, payload):
        self._done_event.set()
    
    def retrieve_item(self, obj, user_id, payload):
        pass
    
    def return_item(self, obj, user_id, payload):
        pass
        
def main():
    mgr = Manager()
    mgr.run()

if __name__ == "__main__":
    main()
    