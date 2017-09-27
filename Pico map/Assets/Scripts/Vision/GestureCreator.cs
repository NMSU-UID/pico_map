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

        Setup:
        Use an object that has a completly different hue and value intensity
        than anything else that will be in the camera shot.
        Change targetColor (exposed) to that color(or at least close engouh)
        The cube (myIcon) should now track to the first pixel of the objects
        color that it sees.
        TODO: object should track to enter of object rather than just the first one.

        Troubleshooting:
        Try plyaing around with the color value, the default setting is to be
        within 20% of Red, Green, and Blue values.
        To change this value, change the colorRange. To high a number though
        and the object will start tracking to anything, too low a value and
        the object won't track to anything.

     */

    public ImageCapture imageCapture;
    public GameObject myIcon;
    public Color32 targetColor;
    public float colorRange;
    // in s
    public float processRate = 3;
    int cameraWidth;
    int cameraHeight;
    public int sceneWidth;
    public int sceneHeight;

    private Color32[] lastFrame;
    private float timer = 0;

    void Start(){
        cameraWidth = imageCapture.cameraWidth;
        cameraHeight = imageCapture.cameraHeight;
    }

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
                myIcon.transform.position = GetPos(i);
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

    // Find the position of where the particular color was found and translate it
    // to Unity space.
    Vector3 GetPos(int i){
        float x = (i % cameraWidth);
        float y = Mathf.Floor (i / cameraHeight);
        x = x * sceneWidth / cameraWidth;
        y = y *  sceneHeight / cameraHeight;
        Vector3 final = new Vector3 (x, y, 400);
        print(x + " " + y);
        return final;
    }
}
