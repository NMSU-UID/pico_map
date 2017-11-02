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
    // int cameraWidth;
    // int cameraHeight;
    public int sceneWidth;
    public int sceneHeight;

    private Color32[] lastFrame;
    private float timer = 0;

    void Start(){
        // cameraWidth = imageCapture.cameraWidth;
        // cameraHeight = imageCapture.cameraHeight;
    }

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
            lastFrame = imageCapture.GetColor();
            ProcessImage();
        }
    }

    // Search every pixel in the lastFrame array and check if it's
    // within out accepted range of target color.
    // Basically the idea is that every point adds "weight" to the object
    void ProcessImage() {
        //For summing x and y values, then getting averages.
        long xTrue = 0;
        long yTrue = 0;
        Vector3 colorPosition;
        int numTrue = 0;
        
        //Find all the valid spots
        for(int i = 0; i < lastFrame.Length; i++) {
            if (inRange (lastFrame[i], targetColor)) {
                colorPosition = GetPos(i);
                xTrue += colorPosition.x;
                yTrue += colorPosition.y;
                numTrue++;
            }
        }
        
        //IF we found points, we can do this math.  If not, then we need to simply go to 0,0 for now.
        if(numTrue != 0) {
            xTrue = xTrue / numTrue;
            yTrue = yTrue / numTrue;
            myIcon.transform.position = new Vector3(xTrue, -625, yTrue);
        }
        else
            myIcon.transform.position = new Vector3(0,-625, 0);
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
        float x = (i % (imageCapture.cameraWidth / imageCapture.reductionScale));
        float y = Mathf.Floor (i / (imageCapture.cameraHeight / imageCapture.reductionScale));
        x = x * sceneWidth / ((float)imageCapture.cameraWidth) * (imageCapture.reductionScale * (float)1.333) - (sceneWidth/2);
        y = y * sceneHeight / ((float)imageCapture.cameraHeight) * (imageCapture.reductionScale * (float)1.666) - (sceneHeight/2);
        Vector3 final = new Vector3 (x, y, 400);
        print(x + " " + y);
        return final;
    }

    // Vector3 GetMidpoint(Vector3 start, Vector3 end) {
    //     return new Vector3((start.x + end.x) / 2, -625, (start.y + end.y) / 2);
    // }
}
