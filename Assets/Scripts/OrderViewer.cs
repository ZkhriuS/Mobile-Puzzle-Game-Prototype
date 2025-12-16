using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderViewer : MonoBehaviour
{
    [SerializeField] private Button readyButton;

    [SerializeField] private Order order;

    [SerializeField] private RectTransform componentPrefab;

    [SerializeField] private RectTransform parent;

    [SerializeField] private Vector2 offset;
    [SerializeField] private Vector2Int fullTime;

    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        gameObject.GetComponent<CountDownTimer>().SetTimer(fullTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        readyButton.interactable = false;
        ItemScore.ScoreChanged += UpdateReadyButtonState;
        CountDownTimer.UpdateTimeText += UpdateTimerText;
        InitializeComponents();
        timerText.text = $"{fullTime.x}:{fullTime.y}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeComponents()
    {
        int componentNumber = order.GetComponentsNumber();
        float spaceX = componentNumber * order.GetOrderComponent(0).GetComponent<RectTransform>().rect.width + (componentNumber - 1) * offset.x;
        for (int i = 0; i < componentNumber; i++)
        {
            GameObject component = Instantiate(order.GetOrderComponent(i).gameObject, parent);
            Image componentSrcImage = order.GetOrderComponent(i).GetComponent<Image>();
            RectTransform componentRect = component.GetComponent<RectTransform>();
            float width = componentRect.rect.width;
            componentRect.anchoredPosition = new Vector2(i*(width+offset.x)-spaceX/2,offset.y);
            Image componentDstImage = component.GetComponent<Image>();
            componentDstImage.sprite = componentSrcImage.sprite;
            componentDstImage.color = componentSrcImage.color;
            TextMeshProUGUI count = component.GetComponentInChildren<TextMeshProUGUI>();
            count.text = order.GetComponentValue(i).ToString();
        }
    }

    public void UpdateReadyButtonState()
    {
        readyButton.interactable = order.IsOrderReady();
    }

    private void OnDestroy()
    {
        ItemScore.ScoreChanged -= UpdateReadyButtonState;
    }
    
    private void UpdateTimerText(float time)
    {
        Vector2Int timer = new Vector2Int((int)time/60, (int)time%60);
        timerText.text = (timer.y/10==0)? $"{timer.x}:0{timer.y}":$"{timer.x}:{timer.y}";
    }
}
