using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowMovement : MonoBehaviour, IDragHandler
{
    [SerializeField] private Type inputType;
    public TMP_InputField input;
    public Transform Clock;

    bool nightTime = false;
    bool inputEnable = false;
    private void Start()
    {
        ClockSystem.Instance.ChangeAlarmInput().AddListener(SwitchMode);
    }
    public void SwitchMode()
    {
        inputEnable = !inputEnable;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inputEnable) return;
        Vector3 directionToTarget = (Vector2)Clock.position - eventData.position;
        float signedAngleToTarget = Vector3.SignedAngle(Clock.up*-1f, directionToTarget, Clock.forward);
        transform.rotation = Quaternion.Euler(0, 0, signedAngleToTarget);
        float angelForTime = Vector3.SignedAngle(Clock.up, directionToTarget, Clock.forward*-1f);


        float time = 0;
        switch (inputType)
        {
            case Type.hour:
                nightTime = Vector2.Distance((Vector2)Clock.position, eventData.position) > 200f;
                if (nightTime)
                {
                    time = (angelForTime + 180) / 360 * 12;
                }
                else
                {
                    time = (angelForTime + 180) / 360 * 12 + 12;
                }
                break;
            default:
                time = (angelForTime + 180) / 360 * 60;
                break;
        }
        input.text = $"{Math.Truncate(time).ToString("00")}";
    }

    
}
