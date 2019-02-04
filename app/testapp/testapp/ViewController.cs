using System;

using UIKit;

using Foundation;
using AVFoundation;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace testapp
{
    public partial class ViewController : UIViewController
    {
        private MessageHandler mh;
        bool flashOn = false;

        UIActionSheet actionSheet;
    //    actionSheet.AddButton("delete");
    //actionSheet.AddButton("cancel");
    //actionSheet.AddButton("a different option!");
    //actionSheet.AddButton("another option");
    //actionSheet.DestructiveButtonIndex = 0;
    //actionSheet.CancelButtonIndex = 1;
    ////actionSheet.FirstOtherButtonIndex = 2;
    //actionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
    //    Console.WriteLine("Button " + b.ButtonIndex.ToString() + " clicked");
    //};
    //actionSheet.ShowInView(View);
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
            actionSheet.Clicked += delegate (object a, UIButtonEventArgs b) {
                Console.WriteLine("Button " + b.ButtonIndex.ToString() + " clicked");
                Status status = new Status();
                status.status = 1;

                mh.NewItemFinish("1", status);
            };
                //await AuthorizeCameraUse();
                //SetupLiveCameraStream();

                TranslateButton.TouchUpInside += (object sender, EventArgs e) => {

                Cloth cloth = new Cloth();
                cloth.type = "dress";
                var t = Task.Run(() => mh.NewItemInit("1", cloth));

                // wait for done
                actionSheet.ShowInView(View);


            };
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        //async partial void TakePhotoButtonTapped(UIButton sender)
        //{
        //    var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
        //    var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

        //    var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
        //    var jpegAsByteArray = jpegImageAsNsData.ToArray();

        //    // TODO: Send this to local storage or cloud storage such as Azure Storage.
        //}

        //partial void SwitchCameraButtonTapped(UIButton sender)
        //{

        //}

        //partial void FlashButtonTapped(UIButton sender)
        //{

        //}

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

            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
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
            captureSession.StartRunning();
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
