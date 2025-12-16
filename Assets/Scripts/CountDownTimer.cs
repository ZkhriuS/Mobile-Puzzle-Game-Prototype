using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    private float _fullTime;
    private float _buttonClickTime;
    public static event Action<Order, bool> TimeOut;
    public static event Action<float> UpdateTimeText;
    // Start is called before the first frame update
    void Awake()
    {
        _buttonClickTime = GlobalTimer.GetDestroyTime();
        OrderListGenerator.OnButton += OnButton;
    }

    // Update is called once per frame
    void Update()
    {
        CountDown(0);
    }

    private void CountDown(float unaccountedTime)
    {
        if (_fullTime >= 0)
        {
            _fullTime -= (Time.deltaTime+unaccountedTime);
            UpdateTimeText?.Invoke(_fullTime);
        }
        else
        {
            TimeOut?.Invoke(gameObject.GetComponent<Order>(), false);
        }
    }

    public void SetTimer(Vector2Int time)
    {
        _fullTime = time.x * 60 + time.y;
    }

    private void OnButton()
    {
        float temp = GlobalTimer.GetTime();
        float pause = temp - _buttonClickTime; 
        _buttonClickTime = temp;
        if(!gameObject.activeSelf)
            CountDown(pause);
    }

    private void OnDestroy()
    {
        OrderListGenerator.OnButton -= OnButton;
        GlobalTimer.FixDestroyTime();
    }
}
