﻿/* Randall Woodall
 * October 2017
 * LightControl.cs
 * Purpose: Prototype to control light.
 * TODO: link the two functions together, 
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
        dirLight.intensity = val;
    }//End changeIntensity

    //changeAngle changes the angle of the light
    public void changeAngle(float ammt) {
        dirLight.transform.rotation = Quaternion.Euler(0, ammt * 160 - 80, 0);
    }//End changeAngle
}
