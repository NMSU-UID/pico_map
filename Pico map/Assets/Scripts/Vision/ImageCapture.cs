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

    private Color32[] data;

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
    IEnumerator ZoomCameraTexture(){
        // find edges
        cols = GetColor();
        while (cols.Length < 1) {
            yield return new WaitForSeconds(3);
            cols = GetColor();

        }

        int minX = 1000;
        int maxX = 0;
        int minY = 1000;
        int maxY = 0;
        print(cols.Length);
        int iterations = 0;
        int rangeCount = 0;
        for(int i = 0; i < cameraWidth; i+=10){
            for(int j = 0; j < cameraHeight; j+=10) {

                iterations += 1;
                // print("i: " + i + " j: " + j);
                // print(cols[i*(j+1) + j]);
                if (inRange(cols[i*(j+1) + j], trackingColor)) {
                    // print("In range: " + i + " " + j);
                    rangeCount += 1;
                    if (minX > i) minX = i;
                    if (maxX < i) maxX = i;
                    if (minY > j) minY = j;
                    if (maxY < j) maxY = j;
                }
            }
        }
        print(" " + minX + " " + maxX + " " + minY + " " + maxY);
        // Scale to projector
        minX = (1920 / cameraWidth) * minX;
        maxX = (1920 / cameraWidth) * maxX;
        minY = (1020 / cameraHeight) * minY;
        maxY = (1020 / cameraHeight) * maxY;

        // Offset to center
        minX = minX - (1920/2);
        maxX = maxX - (1920/2);
        minY = minY - (1080/2);
        maxY = maxY - (1080/2);

        print("done with " + iterations + " iters");
        print("found " + rangeCount + " in range");

        print (webCamTexture.width);
        print(webCamTexture.height);
        // calculate middle
        Vector2 middle = new Vector2(maxX - minX, maxY - minY);
        // center image
        corners[0].position = new Vector3(minX, -600, maxY); //upper left
        corners[1].position = new Vector3(maxX, -600, maxY); //upper right
        corners[2].position = new Vector3(minX, -600, minY); //lower left
        corners[3].position = new Vector3(maxX, -600, minY); //lower right
        //scale plane
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
        // Color32[] raw = webCamTexture.GetPixels32(data);
        //TODO: PoolColors not currently working. we should be returning pass
        // Color32[] pass = PoolColors(raw, cameraWidth, cameraHeight);
        return webCamTexture.GetPixels32();
    }

    // BUG: This doesn't currently work and is hurting our frame rate :(
    // This function is ment to ease in the tracking of objects. It's essentially
    // a maxPool function of size 3x3. It skips every three pixels horizontally
    // and vertically then averages the surronding pixels. Given an image
    // of ratio 1280 x 720, it will transform to 426 x 240.
    // TODO: This currently only works for 1280 x 720 images, we should make This
    // dynamic for different cameras or multiple passes.
    // Color32[] PoolColors (Color32[] startData, int width, int height) {
    //     Color32[] resultColor = new Color32[(width / 3) * (height / 3)];
    //     int counter = 0;
    //     for(int i = 1; i < width - 1; i += 3) {
    //         for(int j = 1; j < height - 1; j += 3) {
    //             float newRed = (startData[i * j].r + startData[i * j + 1].r + startData[i * j - 1].r + startData[(i - 1) * j].r + startData[(i + 1) * j].r) / 5;
    //             float newGreen = (startData[i * j].g + startData[i * j + 1].g + startData[i * j - 1].g + startData[(i - 1) * j].g + startData[(i + 1) * j].g) / 5;
    //             float newBlue = (startData[i * j].b + startData[i * j + 1].b + startData[i * j - 1].b + startData[(i - 1) * j].b + startData[(i + 1) * j].b) / 5;
    //
    //             resultColor[counter] = new Color32((byte) newRed, (byte) newGreen, (byte) newBlue, 1);
    //             counter++;
    //         }
    //     }
    //     return resultColor;
    // }

    bool inRange(Color input, Color targetColor){

        if (Mathf.Abs(input[0] - targetColor[0]) > 0.2) {
            // print("fail red");
            return false;
        }
        if (Mathf.Abs(input[1] - targetColor[1]) > 0.2) {
            // print("fail green");
            return false;
        }
        if (Mathf.Abs(input[2] - targetColor[2]) > 0.2) {
            // print("fail blue");
            return false;
        }
        // print("Gotem");
        return true;
    }

}
