using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    private static List<List<GameObject>> _items;
    // Start is called before the first frame update
    void Start()
    {
        _items = new List<List<GameObject>>();
        ItemSettler.SetNewItem += AddItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void AddItem(GameObject item)
    {
        foreach (var element in _items)
        {
            if (element[0].tag.Equals(item.tag))
            {
                element.Add(item);
                return;
            }
        }
        _items.Add(new List<GameObject>());
        _items[^1].Add(item);
    }

    public static List<GameObject> FindElementWithTag(string str)
    {
        foreach (var element in _items)
        {
            if (element[0].tag.Equals(str))
            {
                return element;
            }
        }
        return null;
    }

    public static void ClearItems(Order order)
    {
        for (int i = 0; i < order.GetComponentsNumber(); i++)
        {
            string componentTag = order.GetOrderComponent(i).tag;
            List<GameObject> elementToClear = FindElementWithTag(componentTag);
            foreach (var item in elementToClear)
            {
                Destroy(item);
            }
            _items.Remove(FindElementWithTag(componentTag));
        }
    }

    public static bool IsGrabbedPosition(Vector2 position)
    {
        foreach (var element in _items)
        {
            foreach (var item in element)
            {
                var itemPosition = item.transform.position;
                Vector2 itemPosition2 = new Vector2(itemPosition.x, itemPosition.y);
                if (itemPosition2 == position)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public static int GetGrabbed()
    {
        int count = 0;
        foreach (var element in _items)
        {
            count += element.Count;
        }
        return count;
    }
}
