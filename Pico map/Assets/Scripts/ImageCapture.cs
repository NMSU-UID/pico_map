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

    public void Start() {
        InitCam();
        Show(showImage);
    }

    public WebCamTexture GetTexture () {
        return webCamTexture;
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

    void InitCam() {
        webCamTexture = new WebCamTexture(640,480);

        Debug.Log("Camera devices:");

        WebCamDevice[] devices = WebCamTexture.devices;

        int i = 0;
        while (i < devices.Length) {
            Debug.Log(devices[i].name);
            i++;
        }

        imgCam.texture = webCamTexture;
    }
}
