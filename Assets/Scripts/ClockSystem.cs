using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class ClockSystem : MonoBehaviour
{
    [SerializeField] private int _timezone;

    [SerializeField] private TimeInput hourInput;
    [SerializeField] private TimeInput minuteInput;
    [SerializeField] private TimeInput secondInput;

    [SerializeField] private GameObject alarmMessage;
    [SerializeField] private TMP_Text alarmBtnText;
    [SerializeField] private AlarmBlock alarmBlock;
    [SerializeField] private TMP_Text alarmText;
    private DisplayClock _displayClock;

    private DateTime currentTime = new DateTime();
    private TimeSpan alarmTime = new TimeSpan();
    private bool _alarmCreated;

    public static ClockSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public UnityEvent ChangeAlarmInput()
    {
        return alarmBlock.SwitchMode;
    }

    private void Start()
    {
        _displayClock = GetComponentInChildren<DisplayClock>();
        if (PlayerPrefs.HasKey("Time"))
        {
            TimeSpan tmpAlarm = TimeSpan.Parse(PlayerPrefs.GetString("Time"));
            SetAlarm(tmpAlarm);
        }
        StartCoroutine(SyncTime());
        StartCoroutine(UpdateClock());
    }
    IEnumerator UpdateClock()
    {
        while (true)
        {

            _displayClock.SetClock(currentTime);
            CheckAlarm();
            currentTime =currentTime.AddSeconds(1f);
            
            yield return new WaitForSeconds(1f);
        }
    }


    IEnumerator GetTimeApi1()
    {
        string serverURL = "https://worldtimeapi.org/api/timezone/Europe/Moscow";
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(serverURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Если запрос успешен, получаем данные в формате JSON и извлекаем текущее время
                string jsonResponse = webRequest.downloadHandler.text;
                ServerResponse1 response = JsonUtility.FromJson<ServerResponse1>(jsonResponse);

                Debug.Log("Время синхронизировано с сервером 1. Время: " + DateTime.Parse(response.utc_datetime).ToUniversalTime());
                currentTime = DateTime.Parse(response.utc_datetime).ToUniversalTime();
                currentTime = currentTime.AddHours(_timezone);
            }
            else
            {
                Debug.LogError("Ошибка при получении серверного времени: " + webRequest.error);
            }
        }
    }
    private class ServerResponse1
    {
        public string utc_datetime;
    }

    IEnumerator GetTimeApi2()
    {
        string serverURL = "https://timeapi.io/api/TimeZone/zone?timeZone=Europe/Moscow";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(serverURL))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Если запрос успешен, получаем данные в формате JSON и извлекаем текущее время
                string jsonResponse = webRequest.downloadHandler.text;
                ServerResponse2 response = JsonUtility.FromJson<ServerResponse2>(jsonResponse);

                Debug.Log("Время синхронизировано с сервером 2. Время: " + DateTime.Parse(response.currentLocalTime));
                currentTime = DateTime.Parse(response.currentLocalTime);
                currentTime = currentTime.AddHours(-3f);
                currentTime = currentTime.AddHours(_timezone);
            }
            else
            {
                Debug.LogError("Ошибка при получении серверного времени: " + webRequest.error);
            }
        }
    }
    private class ServerResponse2
    {
        public string currentLocalTime;
    }

    IEnumerator SyncTime()
    {
        while (true)
        {
            currentTime = DateTime.UtcNow;
            StartCoroutine(GetTimeApi1());
            StartCoroutine(GetTimeApi2());
            yield return new WaitForSeconds(3600f);
        }
    }

    public void ShowAlarm()
    {
        alarmBlock.gameObject.SetActive(!alarmBlock.gameObject.activeSelf);
        if (alarmBlock.gameObject.activeSelf)
        {
            alarmBtnText.text = "Назад";
        }
        else 
        {
            alarmBtnText.text = "Установить будильник";
        }
    }

    private void CheckAlarm()
    {
        if (_alarmCreated)
        {
            if (currentTime.TimeOfDay.Hours == alarmTime.Hours && currentTime.TimeOfDay.Minutes == alarmTime.Minutes && currentTime.TimeOfDay.Seconds == alarmTime.Seconds)
            {
                Debug.Log("Будильник сработал");
                StartAlarm();
            }
        }
    }
    private bool alarmEnabled;

    public void StopAlarm()
    {
        alarmEnabled = false;
        alarmMessage.SetActive(false);
    }

    private void StartAlarm()
    {
        alarmMessage.SetActive(true);
        alarmEnabled = true;
        StartCoroutine(StartVibrate());
    }

    IEnumerator StartVibrate()
    {
        while (alarmEnabled)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(1f);
        }
    }

    private void SetAlarm(TimeSpan time)
    {
        PlayerPrefs.SetString("Time", time.ToString());
        _alarmCreated = true;
        alarmTime = time;
        alarmText.text = $"Будильник установлен на {time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";
    }

    public void SetAlarm()
    {
        if (hourInput.GetInput() == "" || minuteInput.GetInput() == "" || secondInput.GetInput() == "") return;

        TimeSpan alarmTime = new TimeSpan
            (
                Convert.ToInt32(hourInput.GetInput()),
                Convert.ToInt32(minuteInput.GetInput()),
                Convert.ToInt32(secondInput.GetInput())
            );
        SetAlarm(alarmTime);
        ShowAlarm();
    }
}
