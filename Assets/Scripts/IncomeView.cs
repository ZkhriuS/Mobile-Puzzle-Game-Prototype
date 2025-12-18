using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IncomeView : MonoBehaviour
{
    [SerializeField] private Income income;
    [SerializeField] private RectTransform incomePrefab;

    [SerializeField] private RectTransform parent;

    [SerializeField] private Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        InitializeIncomes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void InitializeIncomes()
    {
        int incomesNumber = income.GetIncomesNumber();
        float spaceX = incomesNumber * income.GetIncome(0).GetComponent<RectTransform>().rect.width + (incomesNumber - 1) * offset.x;
        for (int i = 0; i < incomesNumber; i++)
        {
            GameObject currentIncome = Instantiate(income.GetIncome(i), parent);
            Image incomeSrcImage = income.GetIncome(i).GetComponent<Image>();
            RectTransform incomeRect = currentIncome.GetComponent<RectTransform>();
            float width = incomeRect.rect.width;
            incomeRect.anchoredPosition = new Vector2(i*(width+offset.x)-spaceX/2,offset.y);
            Image incomeDstImage = currentIncome.GetComponent<Image>();
            incomeDstImage.sprite = incomeSrcImage.sprite;
            incomeDstImage.color = incomeSrcImage.color;
            TextMeshProUGUI count = currentIncome.GetComponentInChildren<TextMeshProUGUI>();
            count.text = income.GetIncomeValue(i).ToString();
        }
    }
}
