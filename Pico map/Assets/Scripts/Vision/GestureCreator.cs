using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureCreator : MonoBehaviour {

	public ImageCapture imageCapture;
	public GameObject myIcon;
	public Color targetColor;
	public float colorRange;
	// in s
	public float processRate;

	private Color[] lastFrame;
	private float timer = 0;

	void Update () {
		timer += Time.deltaTime;
		if (timer > processRate) {
			lastFrame = imageCapture.GetColor();
			ProcessImage();
			timer = 0;
		}
	}

	void ProcessImage() {
		for(int i =0; i < lastFrame.Length; i++){
			if (i > 100) {
				break;
			}
		// 	if (inRange (lastFrame[i], targetColor)) {
		// 		print("in range");
		// 		myIcon.transform.position = ClampCursor(myIcon.transform.position, GetPos (i));
		// 		break;
			// }
		}
	}







	bool inRange(Color input, Color targetColor){
		bool redTru = false;
		bool blueTru = false;
		bool greenTru = false;
		if (input[0] > targetColor.r - colorRange && input[0] < targetColor.r + colorRange) {
			redTru = true;
		}
		if (input[2] > targetColor.b - colorRange && input[2]< targetColor.b + colorRange) {
			blueTru = true;
		}
		if (input[1]> targetColor.g - colorRange && input[1] < targetColor.g + colorRange) {
			greenTru = true;
		}
		if (redTru && blueTru && greenTru) {
			print("returning true");
			return true;
		} else {
			print("fetureing false");
			return false;
		}
	}

	Vector3 ClampCursor(Vector3 start, Vector3 newP){
		return new Vector3(Mathf.Clamp(newP.x, start.x-2, start.x+2), Mathf.Clamp(newP.y, start.y-2, start.y+2), 0);
	}

	Vector3 GetPos(int i){
		print ("i = " + i + " camHeight = " + 480 + " camWidth = " + 640+ " i % camHeight = " + i % 480 + " Mathf.Floor i / camWidth = " + Mathf.Floor( i / 640));

		float x = (i % 640);
		float y = Mathf.Floor (i / 480);
		Vector3 final = new Vector3 (x, y, 0);
		return final;
	}
}
