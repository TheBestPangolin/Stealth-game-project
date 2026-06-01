using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TypewriterText : MonoBehaviour
{
    [SerializeField] private float charactersPerSecond = 40f;
    [SerializeField] private float startDelay = 0f;

    [SerializeField] CanvasGroup fadeCanvasGroup;

    Coroutine TypeTextCoroutine;

    private TMP_Text textComponent;
    private string fullText;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        fullText = textComponent.text;
    }

    private void Start()
    {
        StartCoroutine(Fade(1f, 0f, 1f));
        TypeTextCoroutine = StartCoroutine(TypeText());
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (textComponent.maxVisibleCharacters == textComponent.textInfo.characterCount)
            {
                var name = SceneManager.GetActiveScene().name;
                var number = int.Parse(name.Substring(name.Length - 1));
                if (name.EndsWith("6"))
                    SceneManager.LoadSceneAsync("Main Sketch");
                else
                    SceneManager.LoadScene(name.Substring(0, name.Length - 1) + (number + 1).ToString());
            }
            else
            {
                StopCoroutine(TypeTextCoroutine);
                textComponent.maxVisibleCharacters = textComponent.textInfo.characterCount;
            }
        }
    }

    private IEnumerator TypeText()
    {
        textComponent.text = fullText;
        textComponent.maxVisibleCharacters = 0;

        if (startDelay > 0)
            yield return new WaitForSeconds(startDelay);

        textComponent.ForceMeshUpdate();

        int totalCharacters = textComponent.textInfo.characterCount;
        float delay = 1f / charactersPerSecond;

        for (int i = 0; i <= totalCharacters; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (fadeCanvasGroup == null)
            yield break;

        if (duration <= 0f)
        {
            fadeCanvasGroup.alpha = to;
            yield break;
        }

        float timer = 0f;
        fadeCanvasGroup.alpha = from;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, progress);

            yield return null;
        }

        fadeCanvasGroup.alpha = to;
        yield break;
    }
}
