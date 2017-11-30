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
using UnityEngine.UI;



public class LightControl : MonoBehaviour {
    //The light to modify.
    public Light dirLight;
    public Transform icon;
    public Slider mySlider;
    
    void Update() {
        if (icon.position.z < -450 && icon.position.z > -550) {
            if (icon.position.x < -433 && icon.position.x > -878) {
                int newValue = (int) icon.position.x/37;
                print (newValue);
                changeAngle(newValue);
                mySlider.value = newValue + 24;
            }
        }
    }

    //changeIntensity controls intensity of the light
    public void changeIntensity(float val) {
        dirLight.intensity = val / 10;
    }//End changeIntensity

    //changeAngle changes the angle of the light
    public void changeAngle(float val) {
        dirLight.transform.rotation = Quaternion.Euler(40, (val/12) * 180 - 90, -60);
        changeColor(val);
    }//End changeAngle

    //changeColor changes the color of the light
    //(Ranging Yellow to Cyan) Although color uses 0-1, we're using 0-510
    //which is really (0-255), (0-255) first set in Blue, second in Red
    public void changeColor(float val) {
        if(val < 6) {
            dirLight.color = new Color(val/12, 0.8f, 1, 1);
        } else {
            dirLight.color = new Color(1, 0.5f + val/24, 0.5f + val/24, 1);
        }
    }//End changeColor
}
