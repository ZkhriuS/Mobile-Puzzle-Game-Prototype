using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection
{
    //list of new coordinates in selection
    private List<Vector3> _newItemsPositions;
    public Selection()
    {
        _newItemsPositions = new List<Vector3>();
    }

    public List<Vector3> GetNewPositions()
    {
        return _newItemsPositions;
    }

    public void Clear()
    {
        _newItemsPositions.Clear();
    }
    
    public IEnumerator AddNewPositionInSelection(Vector3 position)
    {
        if (_newItemsPositions.Contains(position))
        {
            var startIndex = _newItemsPositions.IndexOf(position);
            var removeCount = _newItemsPositions.Count - startIndex;
            _newItemsPositions.RemoveRange(startIndex, removeCount);
        }
        _newItemsPositions.Add(position);
        yield return null;
    }
    
    
}
