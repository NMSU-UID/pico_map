using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureCreator : MonoBehaviour {

	public ImageCapture imageCapture;
	public GameObject myIcon;
	public Color32 targetColor;
	public float colorRange;
	// in s
	public float processRate;

	private Color32[] lastFrame;
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
			if (i > 1000) {
				break;
			}
			if (inRange (lastFrame[i], targetColor)) {
				print("in range: " + i);
				print(lastFrame[i] + "  " + targetColor);
				myIcon.transform.position = ClampCursor(myIcon.transform.position, GetPos (i));
				break;
			}
		}
	}


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

	Vector3 ClampCursor(Vector3 start, Vector3 newP){
		return new Vector3(Mathf.Clamp(newP.x, start.x-2, start.x+2), Mathf.Clamp(newP.y, start.y-2, start.y+2), 0);
	}

	Vector3 GetPos(int i){
		float x = (i % 1280);
		float y = Mathf.Floor (i / 640);
		Vector3 final = new Vector3 (x, y, 0);
		return final;
	}
}
