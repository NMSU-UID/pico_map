using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightControl : MonoBehaviour {
	public void changeInt (float inp) {
        Light dirLight = GetComponent<Light>();

        dirLight.intensity = inp;
	}
}
