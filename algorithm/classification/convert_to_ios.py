import coremltools
import keras
from keras.models import model_from_json, load_model
import numpy
import os
import datetime
import time

date = time.strftime("%Y-%m-%d_%H-%M")

os.chdir("/mnt/d/New folder/Google Drive/School/FYDP/Software/Classification/Models")
class_names = ["Blazer",
                "Blouse",
                "Button-Down",
                "Coat",
                "Dress",
                "Hoodie",
                "Jackets",
                "Jeans",
                "Jumpsuit",
                "Tee"]
# json_file = open('model.json', 'r')
# loaded_model_json = json_file.read()
# json_file.close()
# loaded_model = model_from_json(loaded_model_json)
# # load weights into new model
# loaded_model.load_weights("model.h5")
# loaded_model.layers.pop()

loaded_model = load_model('weights-improvement-98-0.28.h5')
# loaded_model.layers.pop()
print("Loaded model from disk")

print (loaded_model.summary())

print("Coverting model to CoreML model...")
coreml_model = coremltools.converters.keras.convert(
    loaded_model,
    input_names='image',
    input_name_shape_dict = {'image':[None,224,224,3]},
    output_names='classProbs',
    image_input_names='image',
    class_labels=class_names
)

print("Saving CoreML model to: " + os.getcwd() + "/armari_" + str(date) + ".mlmodel")
coreml_model.save(os.getcwd() + "/armari_" + str(date) + ".mlmodel")