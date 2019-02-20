using System;
using CoreML;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Foundation;
using CoreVideo;
using CoreGraphics;
using CoreImage;
using CoreMedia;

namespace testappEventArgsT
{
    struct ImageDescriptionPrediction
    {
        public string ModelName;
        public List<Tuple<double, string>> predictions;
    }

    public class EventArgsT<T> : EventArgs
    {
        public T Value { get; }

        public EventArgsT(T val)
        {
            this.Value = val;
        }
    }

    class MachineLearningModel
    {
        //public event EventHandler<EventArgsT<ImageDescriptionPrediction>> PredictionsUpdated = delegate { };
        //public event EventHandler<EventArgsT<String>> ErrorOccurred = delegate { };
        //public event EventHandler<EventArgsT<String>> MessageUpdated = delegate { };

        MLModel model;
        //CGSize size;

        MachineLearningModel()
        {
            model = LoadModel();
        }


        MLModel LoadModel()
        {
            NSBundle bundle = NSBundle.MainBundle;
            var assetPath = bundle.GetUrlForResource("model", "mlmodelc");
            MLModel mdl = null;
            try
            {
                NSError err;
                mdl = MLModel.Create(assetPath, out err);
                if (err != null)
                {
                    Console.WriteLine("Error occured while loading models");
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("*** VGG16 model probably hasn't been downloaded, built, and added to the project's Resources. Refer to the README for instructions. Error: " + ane.Message);
            }
            return mdl;
        }

        ImageDescriptionPrediction Classify(CMSampleBuffer source)
        {
            //var pixelBuffer = source.Scale(size).ToCVPixelBuffer();
            var pixelBuffer = source.GetImageBuffer() as CVPixelBuffer;
            var imageValue =  MLFeatureValue.Create(pixelBuffer);
            //imageValue

            var inputs = new NSDictionary<NSString, NSObject>(new NSString("image"), imageValue);
            var prediction = new ImageDescriptionPrediction();
            NSError error, error2;
            var inputFp = new MLDictionaryFeatureProvider(inputs, out error);
            if (error != null)
            {
                return prediction;
            }
            var outFeatures = model.GetPrediction(inputFp, out error2);
            if (error2 != null)
            {
                return prediction;
            }

            var predictionsDictionary = outFeatures.GetFeatureValue("classLabelProbs").DictionaryValue;
            var byProbability = new List<Tuple<double, string>>();
            foreach (var key in predictionsDictionary.Keys)
            {
                var description = (string)(NSString)key;
                var prob = (double)predictionsDictionary[key];
                byProbability.Add(new Tuple<double, string>(prob, description));
            }
            //Sort descending
            byProbability.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1) * -1);


            prediction.ModelName = "model";
            prediction.predictions = byProbability;

            return prediction;
        }
    }
}
