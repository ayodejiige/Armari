using System;
using Foundation;
using UIKit;
using AVFoundation;
using System.Threading.Tasks;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using CoreImage;
using ImageIO;
using System.Drawing;

namespace armari
{
    public class ImagePickerControllerDelegate : UIImagePickerControllerDelegate
    {
        //public event EventHandler<EventArgsT<String>> MessageUpdated = delegate { };
        //public event EventHandler<EventArgsT<String>> ErrorOccurred = delegate { };
        public event EventHandler<EventArgsT<UIImage>> ImagePicked = delegate { };

        private Logger m_logger = Logger.Instance;

        public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            // Close the picker
            picker.DismissViewController(true, null);

            m_logger.Message("Analyzing image...");

            // Read Image from returned data
            UIImage uiImage = info[UIImagePickerController.OriginalImage] as UIImage;
            if (uiImage == null)
            {
                m_logger.Error("Unable to read image from picker.");
                return;
            }

            // Crop calculation
            double squareLength = Math.Min(uiImage.Size.Width, uiImage.Size.Height);
            nfloat x, y;
            x = (nfloat)((uiImage.Size.Width - squareLength) / 2.0);
            y = (nfloat)((uiImage.Size.Height - squareLength) / 2.0);

            string msg = string.Format("Using {0} x {1} image", uiImage.Size.Width, uiImage.Size.Width);
            m_logger.Message(msg);

            // Convert to CIImage
            CIImage ciImage = new CIImage(uiImage);
            if (ciImage == null)
            {
                m_logger.Error("Unable to create required CIImage from UIImage.");
                return;
            }
            CIImage inputImage = ciImage.CreateWithOrientation(uiImage.Orientation.ToCIImageOrientation());

            // Create Crop Filter
            CICrop crop = new CICrop () 
            {
                Image = inputImage,
                Rectangle = new CIVector (x, y, x + (nfloat)squareLength, y + (nfloat)squareLength)
            };

            // Get the Cropped image from the filter
            CIImage output = crop.OutputImage;
            CIContext context = CIContext.FromOptions (null);
            CGImage cgimage = context.CreateCGImage (output, output.Extent);
            msg = string.Format("Using {0} x {1} image", cgimage.Width, cgimage.Height);
            m_logger.Message(msg);


            // Get UI Image with correct orientation
            uiImage = UIImage.FromImage(output);

            ImagePicked(this, new EventArgsT<UIImage>(uiImage));
        }
    }

    public class CameraController : ImagePickerControllerDelegate
    {
        public CameraController()
        {
        }

        public UIImagePickerController ShowCamera()
        {
            // Create a picker to get the camera image
            var picker = new UIImagePickerController()
            {
                Delegate = this,
                SourceType = UIImagePickerControllerSourceType.Camera,
                CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo
            };

            return picker;
        }

    }

    //public class CameraController
    //{
        //UIView m_cameraStreamView;
        //AVCaptureSession m_captureSession;
        //AVCaptureDeviceInput m_captureDeviceInput;
        //AVCaptureStillImageOutput m_stillImageOutput;
        //AVCaptureVideoPreviewLayer m_videoPreviewLayer;
        //CMSampleBuffer m_currentBuffer;

        //public CameraController(UIView liveCameraStream)
        //{
        //    m_cameraStreamView = liveCameraStream;
        //}

        //public async void Init()
        //{
        //    await AuthorizeCameraUse();
        //}

        //public void StartStream(CGRect frame)
        //{
        //    // TO DO: Confirm that frame can be used as class member
        //    SetupLiveCameraStream(frame);
        //    StartLiveCameraStream();
        //}

        //public void StopStream()
        //{
        //    StopLiveCameraStream();
        //}

        //public async Task<CMSampleBuffer> TakePhoto()
        //{
        //    var videoConnection = m_stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
        //    CMSampleBuffer buffer = await m_stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
        //    Console.WriteLine("Buffer <----- total size {0} {1}", buffer.TotalSampleSize, buffer.NumSamples);
        //    m_captureSession.StopRunning();
        //    Console.WriteLine("Photo saved");

        //    return buffer;
        //}

        //// To do: add privision for cvpixelbuffer
        //public static UIImage ToUIImage(CMSampleBuffer buffer)
        //{
        //    Console.WriteLine("Buffer -----> total size {0} {1}", buffer.TotalSampleSize, buffer.NumSamples);
        //    UIImage image = null;

        //    CMVideoFormatDescription formatDescription = buffer.GetVideoFormatDescription();
        //    var subType = formatDescription.MediaSubType;
        //    CMBlockBuffer blockBuffer = buffer.GetDataBuffer();

        //    // Can only convert if image is jpeg encoded..why?
        //    if (subType != (int)CMVideoCodecType.JPEG)
        //        throw new Exception("Block buffer must be JPEG encoded.");

        //    NSMutableData jpegData = new NSMutableData();
        //    jpegData.Length = blockBuffer.DataLength;

        //    blockBuffer.CopyDataBytes(0, blockBuffer.DataLength, jpegData.Bytes);
        //    CGImageSource imageSource = CGImageSource.FromData(jpegData);
        //    var decodeOptions = new CGImageOptions
        //    {
        //        ShouldAllowFloat = false,
        //        ShouldCache = false
        //    };
        //    CGImage cgImage = imageSource.CreateImage(0, decodeOptions);
        //    image = new UIImage(cgImage);

        //    return image;
        //}

        //public static UIImage ConvertToGrayScale(UIImage image)
        //{
        //    SizeF imageSize = new SizeF((float)image.Size.Width, (float)image.Size.Height);
        //    RectangleF imageRect = new RectangleF(PointF.Empty, imageSize);
        //    var colorSpace = CGColorSpace.CreateDeviceGray();
        //    var context = new CGBitmapContext(IntPtr.Zero,
        //        (int) imageRect.Width,
        //        (int)imageRect.Height,
        //        8, 
        //        0,
        //        colorSpace,
        //        CGImageAlphaInfo.None);
        //    context.DrawImage(imageRect, image.CGImage);
        //    var imageRef = context.ToImage();

        //    return new UIImage(imageRef);
        //}

        //public CMSampleBuffer GetImage()
        //{
        //    return m_currentBuffer;
        //}

        //private async Task AuthorizeCameraUse()
        //{
        //    var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

        //    if (authorizationStatus != AVAuthorizationStatus.Authorized)
        //    {
        //        await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
        //    }
        //}

        //private void SetupLiveCameraStream(CGRect frame)
        //{
        //    m_captureSession = new AVCaptureSession();

        //    var viewLayer = m_cameraStreamView.Layer;
        //    m_videoPreviewLayer = new AVCaptureVideoPreviewLayer(m_captureSession)
        //    {
        //        Frame = frame
        //    };
        //    m_cameraStreamView.Layer.AddSublayer(m_videoPreviewLayer);

        //    var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
        //    ConfigureCameraForDevice(captureDevice);
        //    m_captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
        //    m_captureSession.AddInput(m_captureDeviceInput);

        //    var dictionary = new NSMutableDictionary();
        //    dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
        //    m_stillImageOutput = new AVCaptureStillImageOutput()
        //    {
        //        OutputSettings = new NSDictionary()
        //    };

        //    m_captureSession.AddOutput(m_stillImageOutput);
        //}

        //private void StartLiveCameraStream()
        //{
        //    m_captureSession.StartRunning();
        //}

        //private void StopLiveCameraStream()
        //{
        //    m_captureSession.RemoveOutput(m_stillImageOutput);
        //    m_captureSession.RemoveInput(m_captureDeviceInput);
        //    m_videoPreviewLayer.RemoveFromSuperLayer();
        //}

        //private void ConfigureCameraForDevice(AVCaptureDevice device)
        //{
        //    var error = new NSError();
        //    if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
        //    {
        //        device.LockForConfiguration(out error);
        //        device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
        //        device.UnlockForConfiguration();
        //    }
        //    else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
        //    {
        //        device.LockForConfiguration(out error);
        //        device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
        //        device.UnlockForConfiguration();
        //    }
        //    else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
        //    {
        //        device.LockForConfiguration(out error);
        //        device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
        //        device.UnlockForConfiguration();
        //    }
        //}
    //}
}
