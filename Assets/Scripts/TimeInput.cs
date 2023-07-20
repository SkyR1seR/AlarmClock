using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeInput : MonoBehaviour
{
    [SerializeField] private Transform arrowForRotate;
    [SerializeField] private Type inputType;
    private TMP_InputField inputField;


    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnInputValueChanged);
        ClockSystem.Instance.ChangeAlarmInput().AddListener(ChangeInput);
    }

    public string GetInput()
    {
        return inputField.text;
    }

    public void ChangeInput()
    {
        inputField.interactable = !inputField.interactable;
    }

    private void OnInputValueChanged(string input)
    {
        string pattern = "";

        switch (inputType)
        {
            case Type.hour:
                pattern = @"\b([0-1]?[0-9]|2[0-3])\b";
                break;
            default:
                pattern = @"\b([0-5]?[0-9])\b";
                break;
        }

        // Проверяем, соответствует ли введенный текст формату времени
        if (!Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
        {
            if (input.Length>0)
            {
                input = input.Remove(input.Length - 1);
            }
        }

        if (input.Length>0)
        {
            switch (inputType)
            {
                case Type.hour:
                    RotateArrow(Convert.ToInt32(input), 12f);
                    break;
                default:
                    RotateArrow(Convert.ToInt32(input), 60f);
                    break;
            }
        }
        inputField.text = input;
    }

    private void RotateArrow(float value, float maxLimit)
    {
        arrowForRotate.rotation = Quaternion.Euler(0,0, value / maxLimit * -360f);
    }
}

public enum Type
{
    hour, minute, second
}
