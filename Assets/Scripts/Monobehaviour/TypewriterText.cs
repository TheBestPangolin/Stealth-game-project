using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterText : MonoBehaviour
{
    [SerializeField] private float charactersPerSecond = 40f;
    [SerializeField] private float startDelay = 0f;

    private TMP_Text textComponent;
    private string fullText;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        fullText = textComponent.text;
    }

    private void Start()
    {
        StartCoroutine(TypeText());
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
}
