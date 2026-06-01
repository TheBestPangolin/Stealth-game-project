using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    private bool isLoading;

    public void PlayButton()
    {
        SceneManager.LoadScene("Cutscene1");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
