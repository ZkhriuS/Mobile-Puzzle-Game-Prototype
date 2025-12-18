using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class QueueGenerator : MonoBehaviour
{
    //list of different items, that can be randomly generated from it
    [SerializeField] private List<GameObject> itemPrefabList;
    [SerializeField] private QueueViewer viewer;
    private Queue<GameObject> _itemQueue;

    public static event Action NotMatch;
    // Start is called before the first frame update
    void Start()
    {
        _itemQueue = new Queue<GameObject>();
        GenerateItems(viewer.GetHolderCount());
        ItemSettler.NextStep += Match;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateItems(int count)
    {
        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            int value = random.Next(0, itemPrefabList.Count);
            _itemQueue.Enqueue(itemPrefabList[value]);
        }
        viewer.UpdateHolders(_itemQueue.ToArray());
    }

    private void ClearQueue(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _itemQueue.Dequeue();
        }
        viewer.UpdateHolders(_itemQueue.ToArray());
    }

    public GameObject Next()
    {
        GameObject nextPrefab = _itemQueue.Peek();
        ClearQueue(1);
        return nextPrefab;
    }

    private void Match(int index)
    {
        if (!ItemSettler.GetCurrentItem()) return;
        if (_itemQueue.ToArray()[index].tag.Equals(ItemSettler.GetCurrentItem().tag)) return;
        NotMatch?.Invoke();
    }

    public QueueViewer GetViewer()
    {
        return viewer;
    }
}
