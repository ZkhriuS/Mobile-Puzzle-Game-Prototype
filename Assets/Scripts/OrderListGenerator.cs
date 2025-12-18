using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class OrderListGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> orderPrefabs;
    [SerializeField] private int orderCount;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    public static event Action OnButton;
    private List<Order> _orderList;

    private GameObject _activeOrder;

    private int _activeIndex;
    // Start is called before the first frame update
    void Start()
    {
        _orderList = new List<Order>();
        _activeIndex = 0;
        GenerateOrders();
        _orderList[_activeIndex].gameObject.SetActive(true);
        SetButtonsState();
        Order.OrderIsDone += RemoveOrder;
        CountDownTimer.TimeOut += RemoveOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateOrders()
    {
        while (_orderList.Count < orderCount)
        {
            Random random = new Random();
            int value = random.Next(orderPrefabs.Count);
            GameObject newOrder = Instantiate(orderPrefabs[value], gameObject.transform);
            _orderList.Add(newOrder.GetComponent<Order>());
            newOrder.SetActive(false);
        }
    }

    private void RemoveOrder(Order order, bool isDone)
    {
        if (_orderList.Remove(order))
        {
            if(isDone)
                FieldManager.ClearItems(order);
            Destroy(order.gameObject);
            GenerateOrders();
        }
        foreach (var eachOrder in _orderList)
        {
            eachOrder.GetComponent<OrderViewer>().UpdateReadyButtonState();
            eachOrder.gameObject.SetActive(false);
        }
        _orderList[_activeIndex].gameObject.SetActive(true);
    }

    public void Next()
    {
        OnButton?.Invoke();
        _activeIndex++;
        _orderList[_activeIndex].gameObject.SetActive(true);
        _orderList[_activeIndex-1].gameObject.SetActive(false);
        SetButtonsState();
    }

    public void Previous()
    {
        OnButton?.Invoke();
        _activeIndex--;
        _orderList[_activeIndex].gameObject.SetActive(true);
        _orderList[_activeIndex+1].gameObject.SetActive(false);
        SetButtonsState();
    }
    private void SetButtonsState()
    {
        nextButton.interactable = true;
        previousButton.interactable = true;
        if (_activeIndex == orderCount - 1)
        {
            nextButton.interactable = false;
        }
        else if (_activeIndex == 0)
        {
            previousButton.interactable = false;
        }
    }
}
