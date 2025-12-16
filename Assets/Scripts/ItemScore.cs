using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScore : ICountable
{
    private int _score;
    private int _multiplier;
    public static event Action ScoreChanged;

    public ItemScore()
    {
        _score = 0;
        _multiplier = 1;
    }

    public int Calculate()
    {
        _score *= _multiplier;
        if (_multiplier == 1) _score++;
        ScoreChanged?.Invoke();
        return _score;
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
    }

    public int GetValue()
    {
        return _score;
    }
}
