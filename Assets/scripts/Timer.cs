using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//displays remaining time. When time runs out, load "lost" screen
public class Timer : MonoBehaviour {
	
	private float time = 0;

	private Text timeText;

	// Use this for initialization
	void Start () {
		timeText = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		
		int minutes = (int)(time / 60);
		int seconds = (int)(time % 60);

		
		//display the timer
		timeText.text = string.Format ("{0:00}:{1:00}", minutes, seconds);
		
	}


}
