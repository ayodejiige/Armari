from flask import Flask
from flask_sqlalchemy import *
import threading
import os

from database import DBManager
from mqtt_client import MQTTClient
from led import LedController

try:
    IS_PI = (os.uname()[4][:3] == 'arm')
except AttributeError:
    IS_PI = False

# app = Flask(__name__)
# app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:////home/ayodeji/test.db'
# db = SQLAlchemy(app)

class Manager(object):
    _TIMEOUT = 20
    _HOST = "test.mosquitto.org"
    _PORT = 1883

    _WAREDROBE_TOPIC="%s/wardrobe/"
    def __init__(self):
        if IS_PI:
            self._led = LedController()
        self._insert_event = threading.Event()
        self._retrieve_event = threading.Event()
        self._return_event = threading.Event()
        self._mqttc = MQTTClient(self._HOST, self._PORT)
        self._mqttc.subscribe("+/wardrobe/new/init", self.new_item_init)
        self._mqttc.subscribe("+/wardrobe/new/done", self.new_item_inserted)
        self._mqttc.subscribe("+/wardrobe/retrieve/init", self.retrieve_item)
        self._mqttc.subscribe("+/wardrobe/retrieve/done", self.item_retrieved)
        self._mqttc.subscribe("+/wardrobe/return/init", self.return_item)
        self._mqttc.subscribe("+/wardrobe/return/done", self.item_returned)
        self._mqttc.subscribe("+/wardrobe/delete", self.delete_item)
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
        ret = self._insert_event.wait(self._TIMEOUT)
        print ("Event done")
        self._insert_event.clear()

        # Turn off led
        if IS_PI:
            self._led.set_pixel(x, y, 0)

        if not ret:
            print ("Took too long")
        else:
            # Update database
            cloth_type = payload["type"]
            id_ = self._db_manager.new_cloth(compartment, cloth_type)
        
        payload = {"status": ret, "id": id_}
        topic = "%s/wardrobe/new/status" %user_id
        self._mqttc.publish(topic, payload)
        
    def new_item_inserted(self, obj, user_id, payload):
        self._insert_event.set()
    
    def retrieve_item(self, obj, user_id, payload):
        cloth_id = payload["cloth_id"]
        cloth = self._db_manager.retrieve_cloth(cloth_id)
        compartment = cloth.get_compartment()

        x = 0
        y = 1
        # Turn on led
        if IS_PI:
            self._led.set_pixel(x, y, 1)
        
        # Wait for done
        ret = self._retrieve_event.wait(self._TIMEOUT)
        print ("Event done")
        self._retrieve_event.clear()

        # Turn off led
        if IS_PI:
            self._led.set_pixel(x, y, 0)

        if not ret:
            print ("Took too long")
        else:
            # Update database
            self._db_manager.retrieved_cloth(cloth_id)
        
        payload = {"status": ret}
        topic = "+/wardrobe/return/done" %user_id
    
    def item_retrieved(self, obj, user_id, payload):
        self._retrieve_event.set()

    def return_item(self, obj, user_id, payload):
        compartment =  self._db_manager.get_compartment()
        cloth_id = payload["cloth_id"]

        x = 0
        y = 1
        # Turn on led
        if IS_PI:
            self._led.set_pixel(x, y, 1)
        
        # Wait for done
        print ("Waiting for item to be returned")
        ret = self._return_event.wait(self._TIMEOUT)
        print ("Event done")
        self._return_event.clear()

        # Turn off led
        if IS_PI:
            self._led.set_pixel(x, y, 0)

        if not ret:
            print ("Took too long")
        else:
            # Update database
            print ("Item returned")
            self._db_manager.return_cloth(compartment, cloth_id)
        
        payload = {"status": ret}
        topic = "%s/wardrobe/return/status" %user_id
    
    def item_returned(self, obj, user_id, payload):
        self._return_event.set()
    
    def delete_item(self, obj, user_id, payload):
        cloth_id = payload["cloth_id"]   

        print ("Deleting item")
        self._db_manager.remove_cloth(cloth_id)
        
        payload = {"status": True}
        topic = "%s/wardrobe/delete/status" %user_id
    
    def get_wardrobe(self):
        pass

def main():
    mgr = Manager()
    mgr.run()

if __name__ == "__main__":
    main()
    