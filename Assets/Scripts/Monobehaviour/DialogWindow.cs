using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogWindow : MonoBehaviour
{
    public static Action<string> DisplayDialog;
    public static Action DisableDialog;
    public static Action<string> ReadFileDialogs;

    private TMP_Text Text;
    private Animator Animator;
    private RectTransform Size;

    private Queue<string> DialogQueue = new Queue<string>();

    private int EtalonLength = "oooooooooooooooooooooooooooooo".Length;

    void Start()
    {
        Text = GetComponentInChildren<TMP_Text>();
        Animator = GetComponent<Animator>();
        Size = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        DisplayDialog += ShowDialog;
        DisableDialog += HideDialog;
        ReadFileDialogs += ReadDialogs;
    }

    private void OnDisable()
    {
        DisplayDialog -= ShowDialog;
        DisableDialog -= HideDialog;
        ReadFileDialogs -= ReadDialogs;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (DialogQueue.Count > 0)
                SetText(DialogQueue.Dequeue());
            else if (Animator.GetBool("IsShowing"))
                HideDialog();
        }
    }

    private void ShowDialog(string dialog)
    {
        SetText(dialog);
        Animator.SetBool("IsShowing", true);
    }

    private void HideDialog()
    {
        Animator.SetBool("IsShowing", false);
    }

    private void ReadDialogs(string path)
    {
        var lines = File.ReadLines(path);
        foreach (var line in lines)
        {
            DialogQueue.Enqueue(line);
        }
        ShowDialog(DialogQueue.Dequeue());
    }

    private void SetText(string text)
    {
        Text.SetText(text);
        Size.anchoredPosition = new Vector2(Size.anchoredPosition.x, -Size.rect.height / 2 - 40);
    }
}
