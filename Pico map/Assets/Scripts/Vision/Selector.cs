using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

    /*
        selector
		handles selecting light controls

     */
    public ImageCapture imageCapture;
    public RectTransform selector;
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
                selector.anchoredPosition = GetPosition(i);
                break;
            }
        }
    }


    Vector3 GetPosition(int i){
        float y = Mathf.Floor (i / imageCapture.cameraHeight);
        int loc = 1;
        if (y < 80) {
            loc = 0;
        }
        if (y > 120) {
            loc = 2;
        }
        print(y);
        return new Vector3(-88, positions[loc], 0);
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
        // print("inRange");
        return true;
    }
}
