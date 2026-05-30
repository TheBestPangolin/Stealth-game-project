using System;
using TMPro;
using UnityEngine;

public class HolsterHint : MonoBehaviour
{
    public static Action DisplayHint;
    public static Action DisableHint;
    private Animator Hint_Animator;

    void Start()
    {
        Hint_Animator = GetComponent<Animator>();
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

    private void ShowHint()
    {
        Hint_Animator.SetBool("IsShowing", true);

    }
    private void HideHint()
    {
        Hint_Animator.SetBool("IsShowing", false);
    }
}
