using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class ItemAffector : MonoBehaviour
{
    [SerializeField] private float compressionPercent;
    [SerializeField] private TextMeshProUGUI scoreText;
    private static int _selectedItemsCounter;
    private bool _isSelected;
    private Vector3 _sourceScale;
    //flag shows if this is the same selection before mouse button is up
    private Field _parentField;
    public static event Action<GameObject> CurrentItemSelected;
    private ItemScore _score;
    private static int _unselectedItemsCounter;

    private void Awake()
    {
        _score = new ItemScore();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Field.Selected += Select;
        Field.Deselected += Deselect;
        ItemSettler.CalculateScore += Multiply;
        _sourceScale = gameObject.transform.localScale;
        _parentField = gameObject.transform.parent.GetComponent<Field>();
        _unselectedItemsCounter = 0;
        _selectedItemsCounter = 0;
        Multiply(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(_isSelected)
            _score.SetMultiplier(_selectedItemsCounter);
    }

    private void Select(Vector3 position, int number)
    {
        Vector3 itemPosition = gameObject.transform.position;
        float x = Mathf.Abs(position.x - itemPosition.x);
        float y = Mathf.Abs(position.y - itemPosition.y);
        Vector3 result = new Vector3(x, y);
        if (_parentField.grid.cellSize.x / 2 > result.x && _parentField.grid.cellSize.y / 2 > result.y)
        {
            gameObject.transform.localScale = _sourceScale * (1 - compressionPercent);
            CurrentItemSelected?.Invoke(gameObject);
            _isSelected = true;
        }
        else
        {
            _unselectedItemsCounter++;
            if (_unselectedItemsCounter == FieldManager.GetGrabbed())
            {
                CurrentItemSelected?.Invoke(null);
                _unselectedItemsCounter = 0;
            }
        }
    }

    private void Deselect()
    {
        gameObject.transform.localScale = _sourceScale;
        CurrentItemSelected?.Invoke(null);
        _unselectedItemsCounter = 0;
        _isSelected = false;
    }

    private void Multiply(int number)
    {
        if(_isSelected)
            _score.SetMultiplier(number);
        scoreText.text = _score.Calculate().ToString();
        _score.SetMultiplier(1);
    }

    public ItemScore GetItemScore()
    {
        return _score;
    }

    private void OnDestroy()
    {
        Field.Selected -= Select;
        Field.Deselected -= Deselect;
        ItemSettler.CalculateScore -= Multiply;
    }
}
