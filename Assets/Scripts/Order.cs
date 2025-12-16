using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Order : MonoBehaviour
{
    private Dictionary<GameObject, int> _orderComponent;
    [SerializeField] private List<GameObject> keys;
    [SerializeField] private List<int> values;
    public static event Action<Order, bool> OrderIsDone;

    // Start is called before the first frame update
    void Awake()
    {
        _orderComponent = new Dictionary<GameObject, int>();
        for (int i = 0; i < keys.Count; i++)
        {
            _orderComponent.Add(keys[i], values[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetComponentsNumber()
    {
        return _orderComponent.Count;
    }
    
    public bool IsOrderReady()
    {
        bool ready = true;
        if (_orderComponent == null) return false;
        foreach (var component in _orderComponent)
        {
            GameObject orderItem = component.Key;
            int orderQuantity = component.Value;
            List<GameObject> element = FieldManager.FindElementWithTag(orderItem.tag);
            if (element != null)
            {
                int result = 0;
                foreach (var item in element)
                {
                    result += item.GetComponent<ItemAffector>().GetItemScore().GetValue();
                }

                if (result < orderQuantity) 
                    ready = false;
            }
            else
            {
                ready = false;
            }
        }
        return ready;
    }

    public GameObject GetOrderComponent(int index)
    {
        return _orderComponent.Keys.ToArray()[index];
    }
    
    public int GetComponentValue(int index)
    {
        return _orderComponent.Values.ToArray()[index];
    }

    public void Execute()
    {
        OrderIsDone?.Invoke(this, true);
    }
}
