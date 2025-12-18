using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{ 
    private static float _time;

    private static float _destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        _time = 0;
        _destroyTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
    }

    public static float GetTime()
    {
        return _time;
    }

    public static void FixDestroyTime()
    {
        _destroyTime = _time;
    }

    public static float GetDestroyTime()
    {
        return _destroyTime;
    }
}
