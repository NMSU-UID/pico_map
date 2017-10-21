using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {
    //changeIntensity controls intensity of the light
    public void changeIntensity(float val)
    {
        Light dirLight = GetComponent<Light>();
        dirLight.intensity = val;
    }
}
