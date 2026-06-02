using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{

    private bool isLoading;
    public GameObject Settings;
    [SerializeField] Slider[] sliders;

    private void Start()
    {
        Settings.SetActive(false);
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("Cutscene1");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void ShowSettings()
    {
        sliders[0].value = Player_container.SoundVolume;
        sliders[1].value = Player_container.MusicVolume;
        sliders[2].value = Player_container.MasterVolume;
        Settings.SetActive(true);
    }

    public void HideSettings()
    {
        Player_container.SoundVolume = sliders[0].value;
        Player_container.MusicVolume = sliders[1].value;
        Player_container.MasterVolume = sliders[2].value;
        SoundManager.instance?.ChangeVolume(sliders[1].value * sliders[2].value);
        Settings.SetActive(false);
    }
}
