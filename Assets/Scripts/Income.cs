using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Income : MonoBehaviour
{
    private Dictionary<GameObject, int> _incomes;
    [SerializeField] private List<GameObject> keys;
    [SerializeField] private List<int> values;

    public static event Action<int> GetMoney;
    public static event Action<int> GetMoves;
    // Start is called before the first frame update
    void Start()
    {
        _incomes = new Dictionary<GameObject, int>();
        for (int i = 0; i < keys.Count; i++)
        {
            _incomes.Add(keys[i], values[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetIncomesNumber()
    {
        return _incomes.Count;
    }
    public GameObject GetIncome(int index)
    {
        return _incomes.Keys.ToArray()[index];
    }
    public int GetIncomeValue(int index)
    {
        return _incomes.Values.ToArray()[index];
    }

    public void Reward()
    {
        foreach (var income in _incomes)
        {
            switch (income.Key.tag)
            {
              case "money": GetMoney?.Invoke(income.Value);
                  break;
              case "moves": GetMoves?.Invoke(income.Value);
                  break;
            }
        }
    }
}
