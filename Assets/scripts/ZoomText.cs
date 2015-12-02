using UnityEngine;
using System.Collections;

public class ZoomText : MonoBehaviour {

    public float zoomSpeed = 10;

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (transform.position.z > -10)
        {
            transform.position = new Vector3(0f, 0f, transform.position.z - Time.deltaTime * zoomSpeed);
            transform.localScale += new Vector3(Time.deltaTime * zoomSpeed/100, Time.deltaTime * zoomSpeed/100, 0f);
        }
        else
        {
            //gameObject.SetActive(false);
        }
	}
}
