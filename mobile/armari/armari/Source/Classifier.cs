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

namespace armari
{
    public struct Prediction
    {
        public string ModelName;
        public List<Tuple<double, string>> predictions;
    }

    public class Classifier
    {
        private static readonly string s_modelExtension = "mlmodelc";
        private static List<string> s_modelNames = new List<string>() { "armari_mnist", "VGG16", "RESNET50" };
        private static List<CGSize> s_modelSizes = new List<CGSize>() { new CGSize(28, 28), new CGSize(224, 224), new CGSize(224, 224) };
        private Dictionary<string, Tuple<CGSize, MLModel>> m_modelData = new Dictionary<string, Tuple<CGSize, MLModel>>();

        public Classifier()
        {
            for (int i = 0; i < s_modelNames.Count; i++)
            {
                var model = LoadModel(s_modelNames[i]);
                m_modelData[s_modelNames[i]] = new Tuple<CGSize, MLModel>(s_modelSizes[i], model);
            }
        }

        ~Classifier()
        {
        }

        private MLModel LoadModel(string modelName)
        {
            NSBundle bundle = NSBundle.MainBundle;
            NSUrl assetPath = bundle.GetUrlForResource(modelName, s_modelExtension);
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
            string modelName = "VGG16";
            //string modelName = "RESNET50";
            //string modelName = "armari_mnist";
            var modelData             = m_modelData[modelName];
            MLModel model             = modelData.Item2;
            CGSize size               = modelData.Item1;
            UIImage image             = source.Scale(size);
            CVPixelBuffer pixelBuffer = image.ToCVPixelBuffer();
            MLFeatureValue imageValue =  MLFeatureValue.Create(pixelBuffer);

            //imageValue

            var inputs = new NSDictionary<NSString, NSObject>(new NSString("image"), imageValue);
            var prediction = new Prediction();
            NSError error, error2;
            var inputFp = new MLDictionaryFeatureProvider(inputs, out error);
            if (error != null)
            {
                Console.Error.WriteLine("Error creating inputFp");
                return prediction;
            }

            var outFeatures = model.GetPrediction(inputFp, out error2);
            if (error2 != null)
            {
                Console.Error.WriteLine("Error getting outFeatures");
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


            prediction.ModelName = modelName;
            prediction.predictions = byProbability;

            return prediction;
        }
    }
}
