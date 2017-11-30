using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {

    /*
        selector
		handles selecting light controls

     */
    public ImageCapture imageCapture;
    public RectTransform selector;
    public List<Slider> lightSliders;
    public Color32 targetColor;
    public float colorRange;
    // in s
    public float processRate = 3;
    public int sceneWidth;
    public int sceneHeight;

    private float timer = 0;

	public List<int> positions;

    // Timer acts as our image capture rate. I currently set this to something
    // like 3 seconds because of the high overhead of processing but as we get quicker
    // we'll be able to lower it substantially.
    void Update () {
        timer += Time.deltaTime;
        if (timer > processRate) {
            timer = 0;
            if (imageCapture.setupComplete == false) {
                return;
            }
            ProcessImage();
        }
    }

    // Search every pixel in the lastFrame array and check if it's
    // within out accepted range of target color.
    void ProcessImage() {
        Color32[] lastFrame = imageCapture.GetColor();
        // print(lastFrame.Length);
        for(int i = 0; i < lastFrame.Length; i++) {
            if (inRange (lastFrame[i], targetColor)) {
                int loc = GetX(i);
                selector.anchoredPosition = new Vector3(-88, positions[loc], 0);

                float y = GetY(i);
                float clamppedValue;
                float maxStep;
                if (loc == 0) {
                    clamppedValue = 510 * y;
                    maxStep = 10;
                } else if (loc == 1){
                    clamppedValue = 8 * y + 2;
                    maxStep = 0.1f;
                } else {
                    clamppedValue = 10 * y;
                    maxStep = 0.3f;
                }
                float sloppedValue = clamppedValue > lightSliders[loc].value ? maxStep : - maxStep;

                print(clamppedValue + " " + lightSliders[loc] + " " + sloppedValue);
                lightSliders[loc].value = lightSliders[loc].value + sloppedValue;
                break;
            }
        }
    }

    int GetX(int i){
        float y = Mathf.Floor (i / imageCapture.cameraHeight);
        int loc = 1;
        if (y < 70) {
            loc = 0;
        }
        if (y > 140) {
            loc = 2;
        }

        return loc;
    }

    float GetY(int i ) {
        float x = (i % imageCapture.cameraWidth);
        x = x / 600;
        // print(x);
        return x;
    }

	// Checks if the current pixel is with colorRange of the target color.
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
