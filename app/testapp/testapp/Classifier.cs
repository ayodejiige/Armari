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

namespace testapp
{
    public struct Prediction
    {
        public string ModelName;
        public List<Tuple<double, string>> predictions;
    }

    //public class EventArgsT<T> : EventArgs
    //{
    //    public T Value { get; }

    //    public EventArgsT(T val)
    //    {
    //        this.Value = val;
    //    }
    //}

    public class Classifier
    {
        //public event EventHandler<EventArgsT<ImageDescriptionPrediction>> PredictionsUpdated = delegate { };
        //public event EventHandler<EventArgsT<String>> ErrorOccurred = delegate { };
        //public event EventHandler<EventArgsT<String>> MessageUpdated = delegate { };
        private static readonly string s_modelName = "armari_mnist";
        private static readonly string s_modelExtension = "mlmodelc";

        private MLModel m_model;
        CGSize m_size;

        public Classifier()
        {
            m_model = LoadModel();
            m_size = new CGSize(28, 28);
        }

        private MLModel LoadModel()
        {
            NSBundle bundle = NSBundle.MainBundle;
            var assetPath = bundle.GetUrlForResource(s_modelName, s_modelExtension);
            MLModel mdl = null;
            try
            {
                NSError err;
                mdl = MLModel.Create(assetPath, out err);
                if (err != null)
                {
                    Console.WriteLine("Error occured while loading models");
                }
                else
                {
                    Console.WriteLine("Model loaded");
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("***model probably hasn't been downloaded, built, and added to the project's Resources. Refer to the README for instructions. Error: " + ane.Message);
            }
            return mdl;
        }

        public Prediction Classify(UIImage source)
        {
            CVPixelBuffer pixelBuffer = source.Scale(m_size);
            MLFeatureValue imageValue =  MLFeatureValue.Create(pixelBuffer);

            //imageValue

            var inputs = new NSDictionary<NSString, NSObject>(new NSString("image"), imageValue);
            var prediction = new Prediction();
            NSError error, error2;
            var inputFp = new MLDictionaryFeatureProvider(inputs, out error);
            if (error != null)
            {
                return prediction;
            }
            var outFeatures = m_model.GetPrediction(inputFp, out error2);
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


            prediction.ModelName = s_modelName;
            prediction.predictions = byProbability;

            return prediction;
        }
    }
}
