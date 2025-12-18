using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SelectorViewer : MonoBehaviour
{
    [SerializeField] private GameObject selectorPanel;
    private int _multiplier;

    // Start is called before the first frame update
    void Start()
    {
        _multiplier = 1;
        Enable(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetMultiplier(int count)
    {
        _multiplier = count switch
        {
            3 => 2,
            4 => 2,
            5 => 3,
            6 => 3,
            7 => 4,
            8 => 5,
            9 => 6,
            10 => 7,
            11 => 8,
            12 => 9,
            _ => 1
        };
        selectorPanel.GetComponentInChildren<TextMeshProUGUI>().text = "x" + _multiplier;
    }

    public void Enable(bool state)
    {
        selectorPanel.SetActive(state);
    }
    
}
