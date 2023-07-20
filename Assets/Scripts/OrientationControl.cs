using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationControl : MonoBehaviour
{
    [SerializeField] private Transform timeBlock;
    [SerializeField] private Transform setAlarmBlock;
    [SerializeField] private Transform alarmBlock;


    private ScreenOrientation currentOrientation;
    private void Update()
    {
        currentOrientation = Screen.orientation;
        switch (currentOrientation)
        {
            case ScreenOrientation.Portrait:
                timeBlock.localPosition = new Vector3 (0, 450, 0);
                setAlarmBlock.localPosition = new Vector3(0,-700,0);
                break;
            case ScreenOrientation.PortraitUpsideDown:
                timeBlock.localPosition = new Vector3(0, 450, 0);
                setAlarmBlock.localPosition = new Vector3(0, -700, 0);
                break;
            case ScreenOrientation.LandscapeLeft:
                timeBlock.localPosition = new Vector3(-450, 0, 0);
                setAlarmBlock.localPosition = new Vector3(700, 0, 0);
                break;
            case ScreenOrientation.LandscapeRight:
                timeBlock.localPosition = new Vector3(450, 0, 0);
                setAlarmBlock.localPosition = new Vector3(-700, 0, 0);
                break;
            case ScreenOrientation.AutoRotation:
                timeBlock.localPosition = new Vector3(0, 450, 0);
                setAlarmBlock.localPosition = new Vector3(0, -700, 0);
                break;
            default:
                break;
        }

    }
}
