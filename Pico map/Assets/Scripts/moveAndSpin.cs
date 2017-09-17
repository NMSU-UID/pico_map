using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveAndSpin : MonoBehaviour {

    public Transform child;
    void Update () {
        transform.position = new Vector3(Mathf.PingPong(Time.time, 24) * 50, transform.position.y, transform.position.z);
        child.Rotate(Vector3.up * Time.deltaTime * 100);
    }
}
