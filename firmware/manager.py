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
    _TIMEOUT = 10
    _HOST = "broker.hivemq.com"
    _PORT = 1883

    _WAREDROBE_TOPIC="%s/wardrobe/"
    def __init__(self):
        if IS_PI:
            self._led = LedController()
            self._led.reset()
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
        self._mqttc.subscribe("+/wardrobe/get", self.get_wardrobe)
        self._db_manager = DBManager(1)
    
    def run(self):
        print ("IS_PI ->>.", IS_PI) 
        rc = 0
        while rc == 0:
            rc = self._mqttc.loop()
    
    def _set_leds(self, poss, val):
        for pos in poss:
           if IS_PI:
            self._led.set_pixel(pos.x, pos.y, val)

    def _pos_payload(self, poss):
        res = {"locs":[]}
        for pos in poss:
            v = {"x":pos.x, "y":pos.y}
            res["locs"].append(v)
        
        return res

    def new_item_init(self, obj, user_id, payload):
        cloth_type = payload["type"]
        compartment =  self._db_manager.get_compartment(cloth_type)
        if compartment is None:
            return
        x = 0
        y = 1
        # Turn on led
        self._set_leds(compartment.positions, 1)

        # 
        payload_ = self._pos_payload(compartment.positions)
        topic = "%s/wardrobe/location" %user_id
        self._mqttc.publish(topic, payload_)

        # Wait for done
        ret = self._insert_event.wait(self._TIMEOUT)
        print ("Event done")
        self._insert_event.clear()
        id_ = -1
        # Turn off led
        self._set_leds(compartment.positions, 0)

        if not ret:
            print ("Took too long")
        else:
            # Update database
            id_ = self._db_manager.new_cloth(compartment, cloth_type)
        
        payload = {"status": int(ret), "id": str(id_)}
        topic = "%s/wardrobe/status" %user_id
        self._mqttc.publish(topic, payload)
        
    def new_item_inserted(self, obj, user_id, payload):
        self._insert_event.set()
    
    def retrieve_item(self, obj, user_id, payload):
        cloth_id = payload["id"]
        cloth = self._db_manager.retrieve_cloth(cloth_id)
        compartment = cloth.get_compartment()
        if compartment is None:
            return

        x = 0
        y = 1
        # Turn on led
        self._set_leds(compartment.positions, 1)

        payload_ = self._pos_payload(compartment.positions)
        topic = "%s/wardrobe/location" %user_id
        self._mqttc.publish(topic, payload_)
        
        # Wait for done
        ret = self._retrieve_event.wait(self._TIMEOUT)
        print ("Event done")
        self._retrieve_event.clear()

        # Turn off led
        self._set_leds(compartment.positions, 0)

        if not ret:
            print ("Took too long")
        else:
            # Update database
            self._db_manager.retrieved_cloth(cloth_id)
        
        payload = {"status": int(ret), "id": str(cloth_id)}
        topic = "%s/wardrobe/status" %user_id
        self._mqttc.publish(topic, payload)

    def item_retrieved(self, obj, user_id, payload):
        self._retrieve_event.set()

    def return_item(self, obj, user_id, payload):
        cloth_id = payload["id"]
        cloth_type = self._db_manager.get_cloth_type(cloth_id)
        compartment =  self._db_manager.get_compartment(cloth_type)
        if compartment is None:
            return

        # Turn on led
        self._set_leds(compartment.positions, 1)
        payload_ = self._pos_payload(compartment.positions)
        topic = "%s/wardrobe/location" %user_id
        self._mqttc.publish(topic, payload_)
        
        # Wait for done
        print ("Waiting for item to be returned")
        ret = self._return_event.wait(self._TIMEOUT)
        print ("Event done")
        self._return_event.clear()

        # Turn off led
        self._set_leds(compartment.positions, 0)

        if not ret:
            print ("Took too long")
        else:
            # Update database
            print ("Item returned")
            self._db_manager.return_cloth(compartment, cloth_id)
        
        payload = {"status": int(ret), "id": str(cloth_id)}
        topic = "%s/wardrobe/status" %user_id
        self._mqttc.publish(topic, payload)

    
    def item_returned(self, obj, user_id, payload):
        self._return_event.set()
    
    def delete_item(self, obj, user_id, payload):
        cloth_id = payload["cloth_id"]   

        print ("Deleting item")
        self._db_manager.remove_cloth(cloth_id)
        
        payload = {"status": True}
        topic = "%s/wardrobe/status" %user_id
    
    def get_wardrobe(self, obj, user_id, payload):
        cloth_type = payload["type"]
        print ("Getting wd for %s" %cloth_type)
        ids = []
        if cloth_type == "Dangling":
            ids = self._db_manager.get_dangling_cloths()
            print(ids)
        elif cloth_type != "all":
            ids = self._db_manager.get_cloth_by_type(cloth_type)
        
        payload = {"ids": ids}
        topic = "%s/wardrobe/get/items" %user_id
        self._mqttc.publish(topic, payload)
    
    def get_wardrobe_type(self):
        pass

def main():
    mgr = Manager()
    mgr.run()

if __name__ == "__main__":
    main()
    