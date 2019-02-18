using System;

using UIKit;

using Foundation;
using AVFoundation;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;

namespace testapp
{
    public partial class ViewController : UIViewController
    {
        private MessageHandler mh;
        bool flashOn = false;
        bool photoTaken = false;

        UIActionSheet actionSheet;
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;
        AVCaptureVideoPreviewLayer videoPreviewLayer;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            mh = new MessageHandler();
            actionSheet = new UIActionSheet("action sheet with other buttons");
            actionSheet.AddButton("done");
            actionSheet.AddButton("cancel");
            actionSheet.DestructiveButtonIndex = 0;
            actionSheet.CancelButtonIndex = 1;
            actionSheet.Clicked += delegate (object a, UIButtonEventArgs b)
            {
                Console.WriteLine("Button " + b.ButtonIndex.ToString() + " clicked");
                Status status = new Status();
                status.status = 1;

                mh.NewItemFinish("1", status);
            };

            UploadButton.Hidden = true;
            TakePhotoButton.Hidden = true;
            await AuthorizeCameraUse();


        }

        partial void AddButtonTapped(UIButton sender)
        {
            SetupLiveCameraStream();
            StartLiveCameraStream();
            AddButton.Hidden = true;
            TakePhotoButton.Hidden = false;
        }

        async partial void TakePhotoButtonTapped(UIButton sender)
        {
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            //var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            //var jpegAsByteArray = jpegImageAsNsData.ToArray();
            captureSession.StopRunning();
            TakePhotoButton.Hidden = true;
            UploadButton.Hidden = false;
        }

        partial void UploadButtonTapped(UIButton sender)
        {
            Cloth cloth = new Cloth();
            cloth.type = "dress";
            var task = Task.Run(() => mh.NewItemInit("1", cloth));

            //Wait for done
            actionSheet.ShowInView(View);
            AddButton.Hidden = false;
            UploadButton.Hidden = true;
            StopLiveCameraStream();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        async Task AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }

        public void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();

            var viewLayer = liveCameraStream.Layer;
            videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
            {
                Frame = this.View.Frame
            };
            liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
            captureSession.AddInput(captureDeviceInput);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            captureSession.AddOutput(stillImageOutput);
        }

        public void StartLiveCameraStream()
        {
            captureSession.StartRunning();
        }

        public void StopLiveCameraStream()
        {
            captureSession.RemoveOutput(stillImageOutput);
            captureSession.RemoveInput(captureDeviceInput);
            videoPreviewLayer.RemoveFromSuperLayer();
        }

        void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }
    }
}
