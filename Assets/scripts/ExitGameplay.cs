using UnityEngine;
using System.Collections;

public class ExitGameplay : MonoBehaviour {

    public GameObject pauseScreen;

    public bool paused = false;

	void OnMouseDown()
    {
        if (name == "yesButton")
        {
            Application.Quit();
        }
        if (name == "noButton")
        {
            play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                pause();               
            }   
            else
            {
                play();                
            }
        }
    }

    void pause()
    {
        paused = true;
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }
    void play()
    {
        paused = false;
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }
}
