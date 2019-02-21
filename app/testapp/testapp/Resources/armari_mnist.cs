// armari_mnist.cs
//
// This file was automatically generated and should not be edited.
//

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using CoreML;
using CoreVideo;
using Foundation;

namespace testapp {
	/// <summary>
	/// Model Prediction Input Type
	/// </summary>
	public class armari_mnistInput : NSObject, IMLFeatureProvider
	{
		static readonly NSSet<NSString> featureNames = new NSSet<NSString> (
			new NSString ("image")
		);

		CVPixelBuffer image;

		/// <summary>
		///  as grayscape (kCVPixelFormatType_OneComponent8) image buffer, 28 pizels wide by 28 pixels high
		/// </summary>
		/// <value></value>
		public CVPixelBuffer Image {
			get { return image; }
			set {
				if (value == null)
					throw new ArgumentNullException (nameof (value));

				image = value;
			}
		}

		public NSSet<NSString> FeatureNames {
			get { return featureNames; }
		}

		public MLFeatureValue GetFeatureValue (string featureName)
		{
			switch (featureName) {
			case "image":
				return MLFeatureValue.Create (Image);
			default:
				return null;
			}
		}

		public armari_mnistInput (CVPixelBuffer image)
		{
			if (image == null)
				throw new ArgumentNullException (nameof (image));

			Image = image;
		}
	}

	/// <summary>
	/// Model Prediction Output Type
	/// </summary>
	public class armari_mnistOutput : NSObject, IMLFeatureProvider
	{
		static readonly NSSet<NSString> featureNames = new NSSet<NSString> (
			new NSString ("output1"), new NSString ("classLabel")
		);

		NSDictionary<NSObject, NSNumber> output1;
		string classLabel;

		/// <summary>
		///  as dictionary of strings to doubles
		/// </summary>
		/// <value></value>
		public NSDictionary<NSObject, NSNumber> Output1 {
			get { return output1; }
			set {
				if (value == null)
					throw new ArgumentNullException (nameof (value));

				output1 = value;
			}
		}

		/// <summary>
		///  as string value
		/// </summary>
		/// <value></value>
		public string ClassLabel {
			get { return classLabel; }
			set {
				if (value == null)
					throw new ArgumentNullException (nameof (value));

				classLabel = value;
			}
		}

		public NSSet<NSString> FeatureNames {
			get { return featureNames; }
		}

		public MLFeatureValue GetFeatureValue (string featureName)
		{
			MLFeatureValue value;
			NSError err;

			switch (featureName) {
			case "output1":
				value = MLFeatureValue.Create (Output1, out err);
				if (err != null)
					err.Dispose ();
				return value;
			case "classLabel":
				return MLFeatureValue.Create (ClassLabel);
			default:
				return null;
			}
		}

		public armari_mnistOutput (NSDictionary<NSObject, NSNumber> output1, string classLabel)
		{
			if (output1 == null)
				throw new ArgumentNullException (nameof (output1));

			if (classLabel == null)
				throw new ArgumentNullException (nameof (classLabel));

			Output1 = output1;
			ClassLabel = classLabel;
		}
	}

	/// <summary>
	/// Class for model loading and prediction
	/// </summary>
	public class armari_mnist : NSObject
	{
		readonly MLModel model;

		static NSUrl GetModelUrl ()
		{
			return NSBundle.MainBundle.GetUrlForResource ("armari_mnist", "mlmodelc");
		}

		public armari_mnist ()
		{
			NSError err;

			model = MLModel.Create (GetModelUrl (), out err);
		}

		armari_mnist (MLModel model)
		{
			this.model = model;
		}

		public static armari_mnist Create (NSUrl url, out NSError error)
		{
			if (url == null)
				throw new ArgumentNullException (nameof (url));

			var model = MLModel.Create (url, out error);

			if (model == null)
				return null;

			return new armari_mnist (model);
		}

		public static armari_mnist Create (MLModelConfiguration configuration, out NSError error)
		{
			if (configuration == null)
				throw new ArgumentNullException (nameof (configuration));

			var model = MLModel.Create (GetModelUrl (), configuration, out error);

			if (model == null)
				return null;

			return new armari_mnist (model);
		}

		public static armari_mnist Create (NSUrl url, MLModelConfiguration configuration, out NSError error)
		{
			if (url == null)
				throw new ArgumentNullException (nameof (url));

			if (configuration == null)
				throw new ArgumentNullException (nameof (configuration));

			var model = MLModel.Create (url, configuration, out error);

			if (model == null)
				return null;

			return new armari_mnist (model);
		}

		/// <summary>
		/// Make a prediction using the standard interface
		/// </summary>
		/// <param name="input">an instance of armari_mnistInput to predict from</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public armari_mnistOutput GetPrediction (armari_mnistInput input, out NSError error)
		{
			if (input == null)
				throw new ArgumentNullException (nameof (input));

			var prediction = model.GetPrediction (input, out error);

			if (prediction == null)
				return null;

			var output1Value = prediction.GetFeatureValue ("output1").DictionaryValue;
			var classLabelValue = prediction.GetFeatureValue ("classLabel").StringValue;

			return new armari_mnistOutput (output1Value, classLabelValue);
		}

		/// <summary>
		/// Make a prediction using the standard interface
		/// </summary>
		/// <param name="input">an instance of armari_mnistInput to predict from</param>
		/// <param name="options">prediction options</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public armari_mnistOutput GetPrediction (armari_mnistInput input, MLPredictionOptions options, out NSError error)
		{
			if (input == null)
				throw new ArgumentNullException (nameof (input));

			if (options == null)
				throw new ArgumentNullException (nameof (options));

			var prediction = model.GetPrediction (input, options, out error);

			if (prediction == null)
				return null;

			var output1Value = prediction.GetFeatureValue ("output1").DictionaryValue;
			var classLabelValue = prediction.GetFeatureValue ("classLabel").StringValue;

			return new armari_mnistOutput (output1Value, classLabelValue);
		}

		/// <summary>
		/// Make a prediction using the convenience interface
		/// </summary>
		/// <param name="image"> as grayscape (kCVPixelFormatType_OneComponent8) image buffer, 28 pizels wide by 28 pixels high</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public armari_mnistOutput GetPrediction (CVPixelBuffer image, out NSError error)
		{
			var input = new armari_mnistInput (image);

			return GetPrediction (input, out error);
		}

		/// <summary>
		/// Make a prediction using the convenience interface
		/// </summary>
		/// <param name="image"> as grayscape (kCVPixelFormatType_OneComponent8) image buffer, 28 pizels wide by 28 pixels high</param>
		/// <param name="options">prediction options</param>
		/// <param name="error">If an error occurs, upon return contains an NSError object that describes the problem.</param>
		public armari_mnistOutput GetPrediction (CVPixelBuffer image, MLPredictionOptions options, out NSError error)
		{
			var input = new armari_mnistInput (image);

			return GetPrediction (input, options, out error);
		}
	}
}
