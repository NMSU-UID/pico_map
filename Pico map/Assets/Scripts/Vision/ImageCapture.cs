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
    private int projectorWidth = 1920;
    private int projectorHeight = 1020;
    public int reductionScale = 3;

    private Color32[] data;
    public bool setupComplete = false;

    Color32[] cols;
    public Color trackingColor;
    public List<Transform> corners;
    public GameObject TrackingPlane;

    public void Start() {
        InitCam();
    }

    // This should become a cooroutine and handle map placement/scaling.
    void InitCam() {
        webCamTexture = new WebCamTexture();

        // webCamTexture.requestedFPS = 1;
        webCamTexture.Play();
        // this should be set using webCamTexture.width/height but it needs to wait
        // until initialization is done.
        // data = new Color32[cameraWidth * cameraHeight];

        webCamObject.GetComponent<Renderer>().material.mainTexture = webCamTexture;
        StartCoroutine("ZoomCameraTexture");
    }

    IEnumerator ZoomCameraTexture(){
        // find edges
        cols = GetColor();
        while (cols.Length < 100) {
            yield return new WaitForSeconds(3);
            cameraWidth = webCamTexture.width;
            cameraHeight = webCamTexture.height;
            cols = GetColor();
        }
        print("cols: " + cols.Length);

        float minX = cameraWidth;
        float maxX = 0;
        float minY = cameraHeight;
        float maxY = 0;
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
        print(projectorWidth + " " + projectorHeight + " " + minX + " " + maxX + " " + minY + " " + maxY);
        // Scale to projector
        minX = (projectorWidth / cameraWidth) * minX;
        maxX = (projectorWidth / cameraWidth) * maxX;
        minY = (projectorHeight / cameraHeight) * minY;
        maxY = (projectorHeight / cameraHeight) * maxY;

        // Offset to center
        minX = minX - (projectorWidth/2);
        maxX = maxX - (projectorWidth/2);
        minY = minY - (projectorHeight/2);
        maxY = maxY - (projectorHeight/2);

        // Calculate middle
        // float x = - (1920 / 2) + (minX * 3);
        // float z = (1080 / 2) - (minY * 3);

        // Center image
        corners[0].position = new Vector3(minX, -600, maxY); //upper left
        corners[1].position = new Vector3(maxX, -600, maxY); //upper right
        corners[2].position = new Vector3(minX, -600, minY); //lower left
        corners[3].position = new Vector3(maxX, -600, minY); //lower right

        // Scale plane
        float xScale = webCamObject.transform.localScale.x * (projectorWidth / (maxX - minX));
        float zScale = webCamObject.transform.localScale.y * (projectorHeight / (maxY - minY));
        webCamObject.transform.localScale = new Vector3(xScale, zScale, 1);

        TrackingPlane.SetActive(false);
        setupComplete = true;
    }

    public Color32[] GetColor () {
        if (webCamTexture.width < 100) {
            print("underscan");
            return new Color32[0];
        }

        Color32[] raw = webCamTexture.GetPixels32(data);
        // Color32[] pass = PoolColors(raw);
        return raw;
    }

    // This function is ment to ease in the tracking of objects. It's essentially
    // a maxPool function of size 3x3. It skips every three pixels horizontally
    // and vertically then averages the surronding pixels. Given an image
    // of ratio 1280 x 720, it will transform to 426 x 240.
    // TODO: This currently only works for 1280 x 720 images, we should make This
    // dynamic for different cameras or multiple passes.  Also, if it continues to hurt
    // framerate, we can try threading it.
    Color32[] PoolColors (Color32[] startData) {
        Color32[] resultColor = new Color32[(cameraWidth / reductionScale) * (cameraHeight / reductionScale)];
        print(startData.Length);
        print (resultColor.Length);
        int counter = 0;
        int res = 0;
        for(int i = 1; i < cameraWidth/reductionScale; i += reductionScale) {
            for(int j = 1; j < cameraHeight/reductionScale; j += reductionScale) {
                float newRed = 0;
                float newGreen = 0;
                float newBlue = 0;
                for(int k = 0; k < reductionScale; k++) {
                    for(int l = 0; l < reductionScale; l++) {
                        // i * cameraWidth + j: Our current index
                        // (k-1)*cameraWidth: width line before, current, and after TODO: change this dynamic to use numbers other than 3. NOTE: should always be odd cause even is hard lol
                        //(l-1): height line before, current, and ater.
                        int curIndex = (i * cameraWidth + j) + ((k-1)*cameraWidth) + (l-1);
                        newRed += startData[curIndex].r;
                        newGreen += startData[curIndex].g;
                        newBlue += startData[curIndex].b;
                    }
                }
                newRed /= reductionScale * reductionScale;
                newGreen /= reductionScale * reductionScale;
                newBlue /= reductionScale * reductionScale;

                resultColor[counter] = new Color32((byte) newRed, (byte) newGreen, (byte) newBlue, 1);
                counter++;
                res = i * j;
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
