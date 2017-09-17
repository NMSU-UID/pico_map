using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCapture : MonoBehaviour {

	private WebCamTexture webCamTexture;
    public RawImage imgCam;

	public void Start() {
		InitCam();
		Show(true);
	}

    public void Show(bool bShow) {
        gameObject.SetActive(bShow);

        if(webCamTexture==null)
            InitCam();

        if(bShow)
            webCamTexture.Play();
        else
            webCamTexture.Stop();
    }

    void InitCam() {
        webCamTexture=new WebCamTexture(640,480);

        Debug.Log("Camera devices:");

        WebCamDevice[] devices=WebCamTexture.devices;

        int i = 0;
        while (i < devices.Length)
        {
            Debug.Log(devices[i].name);
            i++;
        }

        imgCam.texture=webCamTexture;
}
}
