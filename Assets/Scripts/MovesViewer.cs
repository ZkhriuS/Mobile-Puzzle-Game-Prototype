using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovesViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private int defaultValue;
    private static Move _moves;
    // Start is called before the first frame update
    void Start()
    {
        _moves = new Move(defaultValue);
        Income.GetMoves += UpdateMove;
        ItemSettler.CalculateScore += Move;
        UpdateMove(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateMove(int value)
    {
        _moves.Update(value);
        movesText.text = _moves.GetValue().ToString();
    }

    private void Move(int smth)
    {
        UpdateMove(-1);
    }
}
