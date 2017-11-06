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
    Rigidbody iconRb;
    public Color32 targetColor;
    public float colorRange;
    // in s
    public float processRate = 3;
    // int cameraWidth;
    // int cameraHeight;
    public int sceneWidth;
    public int sceneHeight;

    private float timer = 0;

    void Start(){
        iconRb = myIcon.GetComponent<Rigidbody>();
    }

    // Timer acts as our image capture rate. I currently set this to something
    // like 3 seconds because of the high overhead of processing but as we get quicker
    // we'll be able to lower it substantially.
    void FixedUpdate () {
        timer += Time.deltaTime;
        if (timer > processRate) {
            timer = 0;
            if (imageCapture.setupComplete == false) {
                return;
            }
            Color32[] lastFrame = imageCapture.GetColor();
            ProcessImage(lastFrame);
        }
    }

    // Search every pixel in the lastFrame array and check if it's
    // within out accepted range of target color.
    void ProcessImage(Color32[] lastFrame) {
        for(int i = 0; i < lastFrame.Length; i++) {
            if (inRange (lastFrame[i], targetColor)) {
                // myIcon.transform.position = GetMidpoint(myIcon.transform.position, GetPos(i));
                // print(lastFrame[i]);
                myIcon.transform.position = GetDirection(i);
                break;
            }
        }
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

    // Find the position of where the particular color was found and translate it
    // to Unity space.
    Vector3 GetDirection(int i){
        float x = (i % imageCapture.cameraWidth);
        float y = Mathf.Floor (i / imageCapture.cameraHeight);
        x = x * 4 - 1280;
        y = y *  3 - (1080 / 2);


        Vector3 targetPosition = new Vector3 (x, -640, y);
        Vector3 heading = targetPosition - myIcon.transform.position;
        float distance = heading.magnitude;
        Vector3 newDirection = Vector3.MoveTowards(iconRb.velocity, heading, Time.deltaTime * 1000);
        print (newDirection);

        // Normalized values
        // print(heading / distance);

        return targetPosition;
    }

    Vector3 GetMidpoint(Vector3 start, Vector3 end) {
        return new Vector3((start.x + end.x) / 2, -625, (start.y + end.y) / 2);
    }
}
