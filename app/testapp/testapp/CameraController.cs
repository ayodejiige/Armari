using System;
using Foundation;
using UIKit;
using AVFoundation;
using System.Threading.Tasks;
using CoreGraphics;
using CoreMedia;
using CoreVideo;
using ImageIO;

namespace testapp
{
    public class CameraController
    {
        UIView m_cameraStreamView;
        AVCaptureSession m_captureSession;
        AVCaptureDeviceInput m_captureDeviceInput;
        AVCaptureStillImageOutput m_stillImageOutput;
        AVCaptureVideoPreviewLayer m_videoPreviewLayer;
        CMSampleBuffer m_currentBuffer;

        public CameraController(UIView liveCameraStream)
        {
            m_cameraStreamView = liveCameraStream;
        }

        public async void Init()
        {
            await AuthorizeCameraUse();
        }

        public void StartStream(CGRect frame)
        {
            // TO DO: Confirm that frame can be used as class member
            SetupLiveCameraStream(frame);
            StartLiveCameraStream();
        }

        public void StopStream()
        {
            StopLiveCameraStream();
        }

        public async Task<CMSampleBuffer> TakePhoto()
        {
            var videoConnection = m_stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            CMSampleBuffer buffer = await m_stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            Console.WriteLine("Buffer <----- total size {0} {1}", buffer.TotalSampleSize, buffer.NumSamples);
            //var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            //var jpegAsByteArray = jpegImageAsNsData.ToArray();
            m_captureSession.StopRunning();
            Console.WriteLine("Photo saved");

            return buffer;
        }

        // To do: add privision for cvpixelbuffer
        public UIImage ToUIImage(CMSampleBuffer buffer)
        {
            Console.WriteLine("Buffer -----> total size {0} {1}", buffer.TotalSampleSize, buffer.NumSamples);
            UIImage image = null;

            NSData jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(buffer);
            CGImageSource imageSource = CGImageSource.FromData(jpegImageAsNsData);
            var decodeOptions = new CGImageOptions
            {
                ShouldAllowFloat = false,
                ShouldCache = false
            };
            CGImage cgImage = imageSource.CreateImage(0, decodeOptions);
            image = new UIImage(cgImage);

            return image;
        }

        public CMSampleBuffer GetImage()
        {
            return m_currentBuffer;
        }

        private async Task AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }

        private void SetupLiveCameraStream(CGRect frame)
        {
            m_captureSession = new AVCaptureSession();

            var viewLayer = m_cameraStreamView.Layer;
            m_videoPreviewLayer = new AVCaptureVideoPreviewLayer(m_captureSession)
            {
                Frame = frame
            };
            m_cameraStreamView.Layer.AddSublayer(m_videoPreviewLayer);

            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            m_captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
            m_captureSession.AddInput(m_captureDeviceInput);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            m_stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            m_captureSession.AddOutput(m_stillImageOutput);
        }

        private void StartLiveCameraStream()
        {
            m_captureSession.StartRunning();
        }

        private void StopLiveCameraStream()
        {
            m_captureSession.RemoveOutput(m_stillImageOutput);
            m_captureSession.RemoveInput(m_captureDeviceInput);
            m_videoPreviewLayer.RemoveFromSuperLayer();
        }

        private void ConfigureCameraForDevice(AVCaptureDevice device)
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
