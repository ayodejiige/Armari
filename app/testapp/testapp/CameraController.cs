using System;
using Foundation;
using UIKit;
using AVFoundation;
using System.Threading.Tasks;
using CoreGraphics;

namespace testapp
{
    public class CameraController
    {
        //UIView m_cameraStreamView;
        AVCaptureSession m_captureSession;
        AVCaptureDeviceInput m_captureDeviceInput;
        AVCaptureStillImageOutput m_stillImageOutput;
        AVCaptureVideoPreviewLayer m_videoPreviewLayer;

        public CameraController()
        {
        }

        public async void Init()
        {
            await AuthorizeCameraUse();
        }

        public void StartStream(CGRect frame, UIView liveCameraStream)
        {
            SetupLiveCameraStream(frame, liveCameraStream);
            StartLiveCameraStream();
        }

        public void StopStream()
        {
            StopLiveCameraStream();
        }

        public async void TakePhoto()
        {
            var videoConnection = m_stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await m_stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            //var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            //var jpegAsByteArray = jpegImageAsNsData.ToArray();
            m_captureSession.StopRunning();
        }

        private async Task AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }

        private void SetupLiveCameraStream(CGRect frame, UIView liveCameraStream)
        {
            m_captureSession = new AVCaptureSession();

            var viewLayer = liveCameraStream.Layer;
            m_videoPreviewLayer = new AVCaptureVideoPreviewLayer(m_captureSession)
            {
                Frame = frame
            };
            liveCameraStream.Layer.AddSublayer(m_videoPreviewLayer);

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
