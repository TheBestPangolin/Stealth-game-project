using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public static bool isPaused; // Флаг состояния паузы
    public GameObject Menu;

    private void Start()
    {
        Menu.SetActive(false);
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
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("Main-Menu");
    }
}