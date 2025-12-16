using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QueueViewer : MonoBehaviour
{
    [SerializeField] private RectTransform holderPanelPrefab;
    [SerializeField] private RectTransform parentPanel;
    [SerializeField] private RectTransform deckPanel;
    //quantity of holders for items and 1 holder for deck;
    [SerializeField] private int holderCount;

    [SerializeField] private float offset;

    private List<RectTransform> _holderList;
    // Start is called before the first frame update
    void Awake()
    {
        
        float holderWidth = (parentPanel.rect.width - ++holderCount * offset) / holderCount--;
        float holderHeight = holderWidth;
        _holderList = new List<RectTransform>();
        CreateHolders(holderHeight, holderWidth);
        SetDeck(holderHeight, holderWidth);
        ItemSettler.NextStep += ViewSelector;
        Field.Deselected += HideSelector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateHolders(float height, float width)
    {
        holderPanelPrefab.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        holderPanelPrefab.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        for (int i = 0; i < holderCount; i++)
        {
            GameObject holder = Instantiate(holderPanelPrefab.gameObject, parentPanel.gameObject.transform.position, Quaternion.identity, parentPanel);
            RectTransform holderTransform = holder.GetComponent<RectTransform>();
            holderTransform.anchoredPosition = new Vector2(i*(width+offset), 0);
            _holderList.Add(holderTransform);
            holder.GetComponent<SelectorViewer>().SetMultiplier(i+1);
        }
    }

    private void SetDeck(float height, float width)
    {
        deckPanel.anchoredPosition = new Vector2(holderCount*(width+offset), 0);
        deckPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        deckPanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    public void UpdateHolders(GameObject[] currentItemsQueue)
    {
        for (int i = 0; i < holderCount; i++)
        {
            if (i < currentItemsQueue.Length)
            {
                Image holderImage = _holderList[i].gameObject.GetComponent<Image>();
                Image currentItemImage = currentItemsQueue[i].GetComponentInChildren<Image>();
                holderImage.sprite = currentItemImage.sprite;
                holderImage.color = currentItemImage.color;
            }
        }
    }

    private void ViewSelector(int index)
    {
        _holderList[index].GetComponent<SelectorViewer>().Enable(true);
    }
    
    private void HideSelector()
    {
        foreach (var holder in _holderList)
        {
            holder.GetComponent<SelectorViewer>().Enable(false);
        }
        
    }

    public int GetHolderCount()
    {
        return holderCount;
    }
}
