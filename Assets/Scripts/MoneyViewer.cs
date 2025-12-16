using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int defaultValue;
    private static Money _money;
    // Start is called before the first frame update
    void Start()
    {
        _money = new Money(defaultValue);
        Income.GetMoney += UpdateMoney;
        Field.Expanded += UpdateMoney;
        UpdateMoney(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateMoney(int value)
    {
        _money.Update(value);
        moneyText.text = _money.GetValue().ToString();
    }

    public static bool IsEnough(int cost)
    {
        return _money.GetValue() >= cost;
    }
}
