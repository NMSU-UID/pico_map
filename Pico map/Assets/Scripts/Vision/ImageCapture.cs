using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour {
    /*
        Camera handler.
        Finds a connected webcam and pipes its image to a
        texture.

        Public parameters:
        imgCam - the texture to render the camera image to.
        showImage - Should the image be rendered
            primarily useful for debugging, deving

        Public functions:
        Show()
        Takes a boolean, sets the camera to show/hide.

        GetTexture()
        Returns the camera texture.

     */
    private WebCamTexture webCamTexture;
    public RawImage imgCam;
    public bool showImage = true;

    private Color32[] data;

    public void Start() {
        InitCam();
        Show(showImage);
    }

    public Color32[] GetColor () {
        print("Height: " + webCamTexture.height + " Width: " + webCamTexture.width);
        Color32[] raw = webCamTexture.GetPixels32(data);
        Color32[] pass = PoolColors(raw, 1280, 720);
        return pass;
    }

    public Color32[] PoolColors (Color32[] startData, int width, int height) {
        Color32[] resultColor = new Color32[(width/3) * (height/3)];
        int counter = 0;
        for(int i = 1; i < width-1; i += 3) {
            for(int j= 1; j < height-1; j += 3) {
                float newRed = (startData[i*j].r + startData[i*j + 1].r + startData[i*j - 1].r + startData[(i-1)*j].r + startData[(i + 1)*j].r) / 5;
                float newGreen = (startData[i*j].g + startData[i*j + 1].g + startData[i*j - 1].g + startData[(i-1)*j].g + startData[(i + 1)*j].g) / 5;
                float newBlue = (startData[i*j].b + startData[i*j + 1].b + startData[i*j - 1].b + startData[(i-1)*j].b + startData[(i + 1)*j].b) / 5;

                resultColor[counter] = new Color32((byte) newRed, (byte) newGreen, (byte) newBlue, 1);
                counter ++;
            }
        }
        print("done pooling, first result: " + resultColor[0] + " from " + startData[width + 2]);
        print("Array size start: " + (width * height) + " to " + counter);
        return resultColor;
    }

    public void Show(bool bShow) {
        gameObject.SetActive(bShow);

        if(webCamTexture == null) {
            InitCam();
        }

        if(bShow) {
            webCamTexture.Play();
        } else {
            webCamTexture.Stop();
        }
    }

    // This should become a cooroutine and handle map placement/scaling.
    void InitCam() {
        webCamTexture = new WebCamTexture();
        webCamTexture.requestedFPS = 1;
        webCamTexture.requestedWidth = 1280;
        webCamTexture.requestedHeight = 720;
        webCamTexture.Play();
        // this should be set using webCamTexture.width/height but it needs to wait
        // until initialization is done.
        data = new Color32[1280 * 720];

        imgCam.texture = webCamTexture;
    }
}
