using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ItemSettler : MonoBehaviour
{
    //Contains and manipulates items on the game field
    [SerializeField] private Field field;
    //list of different items, that can be randomly generated from it
    [SerializeField] private QueueGenerator generator;

    [SerializeField] private Button setItemButton;
    //counter of mouse collisions with cells
    public int colliderCounter;
    public static event Action<int> NextStep;
    public static event Action<int> CalculateScore;
    public static event Action<GameObject> SetNewItem;
    public static event Action<Vector3> OnTouchDown;
    private static GameObject _currentItem;
    // Start is called before the first frame update
    void Start()
    {
        colliderCounter = 0;
        _currentItem = null;
        ItemAffector.CurrentItemSelected += SetCurrentItem;
        QueueGenerator.NotMatch += Cancel;
        setItemButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Camera.main is not null && !field.extensionPanel.activeSelf) 
            OnTouchDown?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        setItemButton.interactable = false;
    }

    private IEnumerator OnMouseOver()
    {
        if (field.GetMouseFlag())
        {
            colliderCounter++;
            if (field.CurrentSelection.GetNewPositions().Count > generator.GetViewer().GetHolderCount())
            {
                Cancel();
                yield break;
            }
            if (Camera.main is not null)
            {
                Vector3 positionToInstantiate = AlignWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (!field.IsPositionSelectable(positionToInstantiate))
                {
                    Cancel();
                    yield break;
                }
                yield return StartCoroutine(field.CurrentSelection.AddNewPositionInSelection(positionToInstantiate));
                if(field.CurrentSelection.GetNewPositions().Count>0)
                    NextStep?.Invoke(field.CurrentSelection.GetNewPositions().Count-1);
            }
        }
    }

    private void OnMouseUp()
    {
        if(field.CurrentSelection == null) return;
        if (field.CurrentSelection.GetNewPositions().Count < 3)
        {
            Cancel();
            return;
        }
        colliderCounter = 0;
        field.SetMouseFlag(false);
        setItemButton.interactable = true;
    }

    public void SetItems()
    {
        field.SetMouseFlag(false);
        foreach(var position in field.CurrentSelection.GetNewPositions())
        {
            if (!FieldManager.IsGrabbedPosition(position))
            {
                GameObject newItem = Instantiate(generator.Next(), position, Quaternion.identity, field.gameObject.transform);
                SetNewItem?.Invoke(newItem);
            }
            else
            {
                generator.Next();
            }
        }
        generator.GenerateItems(field.CurrentSelection.GetNewPositions().Count);
        CalculateScore?.Invoke(field.CurrentSelection.GetNewPositions().Count);
        field.Deselect();
        setItemButton.interactable = false;
    }

    private void Cancel()
    {
        colliderCounter = 0;
        field.SetMouseFlag(false);
        field.Deselect();
        setItemButton.interactable = false;
    }

    private Vector3 AlignWorldPosition(Vector3 rawPosition)
    {
        Vector3Int cellPosition = field.grid.WorldToCell(rawPosition);
        return field.grid.CellToWorld(cellPosition) + 0.25f*field.grid.cellSize+field.grid.cellGap;
    }

    private static void SetCurrentItem(GameObject item)
    {
        _currentItem =  item;
    }

    public static GameObject GetCurrentItem()
    {
        return _currentItem;
    }
}
