using System;
using TMPro;
using UnityEngine;

public class TextHint : MonoBehaviour
{
    public static Action<string> DisplayHint;
    public static Action DisableHint;
    private Animator Hint_Animator;
    private TMP_Text Text;

    void Start()
    {
        Hint_Animator = GetComponent<Animator>();
        Text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        DisplayHint += ShowHint;
        DisableHint += HideHint;
    }

    private void OnDisable()
    {
        DisplayHint -= ShowHint;
        DisableHint -= HideHint;
    }

    private void ShowHint(string text)
    {
        Text.SetText(text);
        Hint_Animator.SetBool("IsShowing", true);

    }
    private void HideHint()
    {
        Hint_Animator.SetBool("IsShowing", false);
    }
}
