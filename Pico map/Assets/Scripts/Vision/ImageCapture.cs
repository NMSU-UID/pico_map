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

        GetColor()
        returns an Color32 array of processed pixels.


     */
    private WebCamTexture webCamTexture;
    public RawImage imgCam;
    public bool showImage = true;

    public int cameraWidth;
    public int cameraHeight;

    private Color32[] data;

    public void Start() {
        InitCam();
        Show(showImage);
    }

    // This should become a cooroutine and handle map placement/scaling.
    void InitCam() {
        webCamTexture = new WebCamTexture();

        // webCamTexture.requestedFPS = 1;
        webCamTexture.requestedWidth = cameraWidth;
        webCamTexture.requestedHeight = cameraWidth;
        webCamTexture.Play();

        // this should be set using webCamTexture.width/height but it needs to wait
        // until initialization is done.
        // data = new Color32[cameraWidth * cameraHeight];

        imgCam.texture = webCamTexture;
    }

    // Shows / hides the texture. Useful for debugging.
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

    public Color32[] GetColor () {
        if (webCamTexture.width < 100) {
            return new Color32[0];
        }
        cameraWidth = webCamTexture.width;
        cameraHeight = webCamTexture.height;
        Color32[] raw = webCamTexture.GetPixels32(data);
        //TODO: TESTING The PoolColors Algo was tested with C and seems to work, but it remains UNTESTED
        Color32[] pass = PoolColors(raw, cameraWidth, cameraHeight);
        return pass;
    }

    // This function is ment to ease in the tracking of objects. It's essentially
    // a maxPool function of size 3x3. It skips every three pixels horizontally
    // and vertically then averages the surronding pixels. Given an image
    // of ratio 1280 x 720, it will transform to 426 x 240.
    // TODO: This currently only works for 1280 x 720 images, we should make This
    // dynamic for different cameras or multiple passes.  Also, if it continues to hurt
    // framerate, we can try threading it.
    Color32[] PoolColors (Color32[] startData, int width, int height) {
        Color32[] resultColor = new Color32[(width / 3) * (height / 3)];
        int counter = 0;
        for(int j = 0; j < height; j += 3) {
            for(int i = 0; i < width - 2; i += 3) {
                float newRed = 0;
                float newGreen = 0;
                float newBlue = 0;
                for(int k = 0; k < 3; k++) {
                    newRed += startData[i + j*height + width * k].r + startData[i + j*height + width * k + 1].r + startData[i + j*height + width * k + 2].r; 
                    newGreen += startData[i + j*height + width * k].g + startData[i + j*height + width * k + 1].g + startData[i + j*height + width * k + 2].g;
                    newBlue += startData[i + j*height + width * k].b + startData[i + j*height + width * k + 1].b + startData[i + j*height + width * k + 2].b;
                }
                newRed /= 9;
                newGreen /= 9;
                newBlue /= 9;
    
                resultColor[counter] = new Color32((byte) newRed, (byte) newGreen, (byte) newBlue, 1);
                counter++;
            }
        }
        return resultColor;
    }

}
