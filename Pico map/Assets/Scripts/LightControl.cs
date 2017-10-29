/* Randall Woodall
 * October 2017
 * LightControl.cs
 * Purpose: Prototype to control light.
 * TODO: link the three functions together, 
 *       make them private and function off one input
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {
    //The light to modify.
    public Light dirLight;

    //changeIntensity controls intensity of the light
    public void changeIntensity(float val) {
        dirLight.intensity = val / 10;
    }//End changeIntensity

    //changeAngle changes the angle of the light
    public void changeAngle(float val) {
        dirLight.transform.rotation = Quaternion.Euler(0, (val/12) * 160 - 80, 0);
    }//End changeAngle
    
    //changeColor changes the color of the light
    //(Ranging Yellow to Cyan) Although color uses 0-1, we're using 0-510 
    //which is really (0-255), (0-255) first set in Blue, second in Red
    public void changeColor(float val) {
        if(val <= 255)
            dirLight.color = new Color(1, 1, val/255, 1);
        else if (val != 510)
            dirLight.color = new Color(1-(val%255)/255, 1, 1, 1);
        else
            dirLight.color = new Color(0,1,1,1);
    }//End changeColor
}
