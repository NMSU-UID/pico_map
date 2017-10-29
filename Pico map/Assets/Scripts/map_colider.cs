using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map_colider : MonoBehaviour {
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
