using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;

public class QRScaner : MonoBehaviour
{
    public WebCamTexture camera;
    public GameObject plane;
    public Text resultDecode;
    public InputField UserText;
    public GameObject panelGenerate;

    public void GenerateOrScaner(bool scaner)
    {
        if (scaner)
        {
            StartCoroutine(Scaner());
        }

        else
        {
            panelGenerate.SetActive(true);
        }
    }

    IEnumerator Scaner()
    {
        resultDecode.text = string.Empty;
        resultDecode.gameObject.SetActive(false);
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        plane.gameObject.SetActive(true);
        camera = new WebCamTexture();
        plane.GetComponent<Renderer>().material.mainTexture = camera;
        camera.Play();

        while ((resultDecode.text = QrReader()) == null)
        {
            yield return new WaitForSeconds(1f);
        }

        plane.gameObject.SetActive(false);
        resultDecode.gameObject.SetActive(true);
        camera.Stop();
    }

    string QrReader()
    {
        IBarcodeReader barcodeReader = new BarcodeReader();
        // decode the current frame
        var result = barcodeReader.Decode(camera.GetPixels32(),
          camera.width, camera.height);

        if (result != null)
        {
            return result.Text;
        }
        else
            return null;
    }


    public void Generate()
    {
        if (UserText.text != null)
        {
            plane.GetComponent<Renderer>().material.mainTexture = GenerateBarcode(UserText.text, BarcodeFormat.QR_CODE, 400, 400);
            panelGenerate.SetActive(false);
            plane.SetActive(true);
        }
    }


    private Texture2D GenerateBarcode(string data, BarcodeFormat format, int width, int height)
    {
        ZXing.QrCode.QrCodeEncodingOptions opt = new ZXing.QrCode.QrCodeEncodingOptions
        {
            CharacterSet = "utf-8",
            Height = height,
            Width = width
        };

        IBarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = opt
        };

        // Generate the BitMatrix
        ZXing.Common.BitMatrix bitMatrix = writer.Encode(data);
        //new MultiFormatWriter().encode(data, format, width, height);
        // Generate the pixel array
        Color[] pixels = new Color[bitMatrix.Width * bitMatrix.Height];
        int pos = 0;
        for (int y = 0; y < bitMatrix.Height; y++)
        {
            for (int x = 0; x < bitMatrix.Width; x++)
            {
                pixels[pos++] = bitMatrix[x, y] ? Color.black : Color.white;
            }
        }
        // Setup the texture
        Texture2D tex = new Texture2D(bitMatrix.Width, bitMatrix.Height);
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }
}
