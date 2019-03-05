using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;



public class QRScaner : MonoBehaviour
{
    private bool cameraInitialized;

    private WebCamTexture camTexture;
    private Rect screenRect;

    private BarcodeReader barCodeReader;

    void Start()
    {
        barCodeReader = new BarcodeReader();

        screenRect = new Rect(0, 0, Screen.width, Screen.height);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
        if (camTexture != null)
        {
            camTexture.Play();
        }

        StartCoroutine(InitializeCamera());
    }

    private IEnumerator InitializeCamera()
    {
        WebCamDevice CameraDevice = new WebCamDevice();

        // Waiting a little seem to avoid the Vuforia crashes.
        yield return new WaitForSeconds(1.25f);

        var isFrameFormatSet = CameraDevice. stance.SetFrameFormat(Image.PIXEL_FORMAT.RGB888, true);
        Debug.Log(string.Format("FormatSet : {0}", isFrameFormatSet));

        // Force autofocus.
        var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (!isAutoFocus)
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }
        Debug.Log(string.Format("AutoFocus : {0}", isAutoFocus));
        cameraInitialized = true;
    }

    private void Update()
    {
        if (cameraInitialized)
        {
            try
            {
                var cameraFeed = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.RGB888);
                if (cameraFeed == null)
                {
                    return;
                }
                var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.RGB24);
                if (data != null)
                {
                    // QRCode detected.
                    Debug.Log(data.Text);
                }
                else
                {
                    Debug.Log("No QR code detected !");
                }
            }
            catch 
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
