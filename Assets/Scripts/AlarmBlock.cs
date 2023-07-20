using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlarmBlock : MonoBehaviour
{
    [SerializeField] private Image switchButton;
    [SerializeField] private Sprite keyboardBtn;
    [SerializeField] private Sprite clockBtn;

    [NonSerialized]public UnityEvent SwitchMode = new UnityEvent();

    public void ChangeInputMode()
    {
        if (switchButton.sprite == keyboardBtn)
        {
            switchButton.sprite = clockBtn;
        }
        else
        {
            switchButton.sprite = keyboardBtn;
        }
        SwitchMode?.Invoke();
    }
}
