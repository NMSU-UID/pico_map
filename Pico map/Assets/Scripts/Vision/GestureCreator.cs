using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureCreator : MonoBehaviour {

	/*
		Gesture handler
		Searches image given from ImageCapture.cs for particular colors
		then moves a cube towards those new coordinates.
		All function are currently private.
		TODO:
		we need to be able to keep track of "things" over time
		we need to be able to handle multiple objects
		we should probably create a script with gesture definitions.

	*/

    public ImageCapture imageCapture;
    public GameObject myIcon;
    public Color32 targetColor;
    public float colorRange;
    // in s
    public float processRate = 3;

    private Color32[] lastFrame;
    private float timer = 0;

	// timer acts as our image capture rate. I currently set this to something
	// like 3 seconds because of the high overhead of processing but as we get quicker
	// we'll be able to lower it substantially.
    void Update () {
        timer += Time.deltaTime;
        if (timer > processRate) {
            lastFrame = imageCapture.GetColor();
            ProcessImage();
            timer = 0;
        }
    }

	// Search every pixel in the lastFrame array and check if it's
	// within out accepted range of target color.
    void ProcessImage() {
        for(int i = 0; i < lastFrame.Length; i++) {
            if (inRange (lastFrame[i], targetColor)) {
                print("in range: " + i);
                print(lastFrame[i] + "  " + targetColor);
                myIcon.transform.position = ClampCursor(myIcon.transform.position, GetPos (i));
                break;
            }
        }
    }

	// Checks if the current pixel is with colorRange of the target color.
    bool inRange(Color input, Color targetColor){
        bool redTru = false;
        bool blueTru = false;
        bool greenTru = false;
        if (Mathf.Abs(input[0] - targetColor[0]) < colorRange) {
            redTru = true;
        }
        if (Mathf.Abs(input[1] - targetColor[1]) < colorRange) {
            blueTru = true;
        }
        if (Mathf.Abs(input[2] - targetColor[2]) < colorRange) {
            greenTru = true;
        }
        if (redTru && blueTru && greenTru) {
            return true;
        } else {
            return false;
        }
    }

	// How far to move the thing. This will later be replaced by physics.
    Vector3 ClampCursor(Vector3 start, Vector3 newP){
        return new Vector3(Mathf.Clamp(newP.x, start.x - 2, start.x + 2), Mathf.Clamp(newP.y, start.y - 2, start.y + 2), 0);
    }

	// Find the position of where the particular color was found and translate it
	// to Unity space.
    Vector3 GetPos(int i){
        float x = (i % 1280);
        float y = Mathf.Floor (i / 640);
        Vector3 final = new Vector3 (x, y, 0);
        return final;
    }
}
