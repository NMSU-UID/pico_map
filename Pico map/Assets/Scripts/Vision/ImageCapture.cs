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
    public GameObject webCamObject;

    public int cameraWidth;
    public int cameraHeight;
    public int reductionScale;

    private Color32[] data;
    public bool setupComplete = false;

    public void Start() {
        InitCam();
        Show(true);
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

        webCamObject.GetComponent<Renderer>().material.mainTexture = webCamTexture;
        StartCoroutine("ZoomCameraTexture");
    }
    Color32[] cols;
    public Color trackingColor;
    public List<Transform> corners;
    public GameObject TrackingPlane;
    IEnumerator ZoomCameraTexture(){
        // find edges
        cols = GetColor();
        while (cols.Length < 1) {
            yield return new WaitForSeconds(3);
            cols = GetColor();

        }

        float minX = 1000;
        float maxX = 0;
        float minY = 1000;
        float maxY = 0;
        print(cols.Length);
        int iterations = 0;
        int rangeCount = 0;
        for(int j = 0; j < cameraHeight/reductionScale; j++){
            for(int i = 0; i < cameraWidth/reductionScale; i++) {
                iterations += 1;
                //Array is weirdly arranged.  Start at bottom left and work up.
                if (inRange(cols[j*(cameraWidth/reductionScale) + i], trackingColor)) {
                    rangeCount += 1;
                    if (minX > i) minX = i;
                    if (maxX < i) maxX = i;
                    if (minY > j) minY = j;
                    if (maxY < j) maxY = j;
                }
            }
        }

        // Scale to projector
        minX = (1920 / cameraWidth / 3) * minX;
        maxX = (1920 / cameraWidth / 3) * maxX;
        minY = (1020 / cameraHeight / 3) * minY;
        maxY = (1020 / cameraHeight / 3) * maxY;

        // Offset to center
        minX = minX - (1920/2);
        maxX = maxX - (1920/2);
        minY = minY - (1080/2);
        maxY = maxY - (1080/2);

        // Calculate middle
        // float x = - (1920 / 2) + (minX * 3);
        // float z = (1080 / 2) - (minY * 3);

        // Center image
        corners[0].position = new Vector3(minX, -600, maxY); //upper left
        corners[1].position = new Vector3(maxX, -600, maxY); //upper right
        corners[2].position = new Vector3(minX, -600, minY); //lower left
        corners[3].position = new Vector3(maxX, -600, minY); //lower right

        // Scale plane
        float xScale = webCamObject.transform.localScale.x * (1920 / (maxX - minX));
        float zScale = webCamObject.transform.localScale.y * (1080 / (maxY - minY));
        webCamObject.transform.localScale = new Vector3(xScale, zScale, 1);

        TrackingPlane.SetActive(false);
        setupComplete = true;
    }

    // Shows / hides the texture. Useful for debugging.
    public void Show(bool bShow) {
        gameObject.SetActive(bShow);

        if(webCamTexture == null) {
            InitCam();
        }

        webCamTexture.Play();
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
        Color32[] resultColor = new Color32[(width / reductionScale) * (height / reductionScale)];
        int counter = 0;
        for(int j = 0; j < height - reductionScale - 1; j += reductionScale) {
            for(int i = 0; i < width - reductionScale - 1; i += reductionScale) {
                float newRed = 0;
                float newGreen = 0;
                float newBlue = 0;
                for(int k = 0; k < reductionScale; k++) {
                    for(int m = 0; m < reductionScale; m++) {
                        newRed += startData[i + j*height + width * k + m].r; 
                        newGreen += startData[i + j*height + width * k + m].g;
                        newBlue += startData[i + j*height + width * k + m].b;
                    }
                }
                newRed /= reductionScale * reductionScale;
                newGreen /= reductionScale * reductionScale;
                newBlue /= reductionScale * reductionScale;
    
                resultColor[counter] = new Color32((byte) newRed, (byte) newGreen, (byte) newBlue, 1);
                counter++;
            }
        }
        return resultColor;
    }

    bool inRange(Color input, Color targetColor){

        if (Mathf.Abs(input[0] - targetColor[0]) > 0.2) {
            return false;
        }
        if (Mathf.Abs(input[1] - targetColor[1]) > 0.2) {
            return false;
        }
        if (Mathf.Abs(input[2] - targetColor[2]) > 0.2) {
            return false;
        }
        return true;
    }

}
