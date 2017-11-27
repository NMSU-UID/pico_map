using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class googleMapAPI : MonoBehaviour {

    string url = "";
    private string apiKey = "AIzaSyCZU5UFZtuCX2Otf2WUmvke8Ff_b6ciuNo";
    private bool loadingMap;

    /// <summary>
    /// Langitude/latitude of area. default Karachi is set
    /// </summary>
    public float lat = 24.917828f;

    public float lon = 67.097096f;
    LocationInfo li;
    /// <summary>
    /// Maps on Google Maps have an integer 'zoom level' which defines the resolution of the current view.
    /// Zoom levels between 0 (the lowest zoom level, in which the entire world can be seen on one map) and 
    /// 21+ (down to streets and individual buildings) are possible within the default roadmap view. 
    /// </summary>
    public int zoom = 14;
    /// <summary>
    /// not more then 640 * 640 
    /// </summary>
    public int mapWidth = 640;
    public int mapHeight = 640;

    public enum mapType { roadmap, satellite, hybrid, terrain };
    public mapType mapSelected;
    /// <summary>
    /// scale can be 1,2 for free plan and can also be 4 for paid
    /// </summary>
    public int scale;
    IEnumerator GetGoogleMap()
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon +
                    "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + scale
                    + "&maptype=" + mapSelected +
                    "&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=" + apiKey;
        loadingMap = true;
        WWW www = new WWW(url);
        yield return www;
        loadingMap = false;
        //Assign downloaded map texture to any gameobject e.g., plane
        gameObject.GetComponent<Renderer>().material.mainTexture = www.texture;
    }
    void Start()
    {

        StartCoroutine(GetGoogleMap());
    }
}
