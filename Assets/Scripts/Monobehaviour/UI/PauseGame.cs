using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static bool isPaused; // Флаг состояния паузы
    public GameObject Menu;
    public GameObject Settings;
    [SerializeField] Slider[] sliders;

    private void Start()
    {
        Menu.SetActive(false);
        Settings.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Pause();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0; // Пауза игры
        Menu.SetActive(true); // Сделать панель видимой
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // Возобновление игры
        Menu.SetActive(false); // Скрыть панель
        HideSettings();
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main-Menu");
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

    public void Quit()
    {
        Application.Quit();
    }
}