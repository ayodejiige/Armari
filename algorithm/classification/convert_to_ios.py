import coremltools
import keras
from keras.models import model_from_json, load_model
import numpy as np
import os
import datetime
import time

date = time.strftime("%Y-%m-%d_%H-%M")

os.chdir("/mnt/d/New folder/Google Drive/School/FYDP/Software/Classification/Models")
class_names = ["Blouse",
                "Cardigan",
                "Hoodie",
                "Jackets",
                "Jeans",
                "Shorts",
                "Skirt",
                "Sweater",
                "Tee"]
json_file = open('112_model.json', 'r')
loaded_model_json = json_file.read()
json_file.close()
loaded_model = model_from_json(loaded_model_json)
# load weights into new model
loaded_model.load_weights("weights.112-1.42.hdf5")
# loaded_model.layers.pop()
# from keras.preprocessing import image
# img = image.load_img("cardigan.jpg", target_size=(224, 224))
# x = image.img_to_array(img)
# res = loaded_model.predict(np.array([x]))
# print(res)
# for i in range(9):
#     print ("%s : %.5f" %(class_names[i], res[0][i]*100))

# loaded_model.layers.pop()
print("Loaded model from disk")

# print (loaded_model.summary())

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