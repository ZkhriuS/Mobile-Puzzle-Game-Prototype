using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Value : ICountable
{
    protected int value;

    public bool Update(int delta)
    {
        if (value + delta >= 0)
        {
            value += delta;
            return true;
        }

        return false;
    }
    
    public int GetValue()
    {
        return value;
    }
}
