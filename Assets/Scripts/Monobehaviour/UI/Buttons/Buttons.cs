using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Cutscene1");
    }

    public void ExitButton()
    {
        Application.Quit();
        SoundManager.instance.StopPlayingLoopSound();
    }
}
