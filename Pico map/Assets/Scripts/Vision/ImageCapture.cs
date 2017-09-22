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
        webCamTexture.GetPixels32(data);
        // call pooling here.
        return data;
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
