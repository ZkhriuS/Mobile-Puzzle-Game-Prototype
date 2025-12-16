using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    public static event Action<Vector3, int> Selected;
    public static event Action Deselected;
    public static event Action<int> Expanded;
    public Selection CurrentSelection;
    //Tilemap for selection
    [SerializeField] private Tilemap selectionTilemap;
    [SerializeField] private Tilemap baseTilemap;
    //TileBase to select cells
    [SerializeField] private TileBase selectionTile;

    [SerializeField] private TileBase emptyTile;
    [SerializeField] private TileBase extensionTile;

    public GameObject extensionPanel;

    [SerializeField] private int extensionCost;
    [SerializeField] private TextMeshProUGUI extensionCostText;

    [SerializeField] private Vector3Int fieldStartCorner;
    [SerializeField] private Vector3Int fieldSize;
    //grid of tilemap provide access to it cells and other parameters
    public Grid grid;
    //flag shows if this is the same selection before mouse button is up
    private static bool _isMousePressed;

    private Vector3Int _extensionPosition;
    // Start is called before the first frame update
    void Start()
    {
        _isMousePressed = false;
        extensionPanel.SetActive(false);
        ItemSettler.OnTouchDown += ManageSwitch;
        BoundsInt bounds = baseTilemap.cellBounds;
        TileBase[] tiles = baseTilemap.GetTilesBlock(bounds);
        for (int j = 0; j < bounds.size.y; j++)
        {
            for (int i = 0; i < bounds.size.x; i++)
            {
                if (tiles[i + j * bounds.size.x])
                {
                    Vector3Int position = new Vector3Int(bounds.xMin+i, bounds.yMin+j);
                    SetExtensionTiles(position);
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (CurrentSelection!=null && CurrentSelection.GetNewPositions().Count > 0)
        {
            Select();
        }
    }

    private void ManageSwitch(Vector3 worldPosition)
    {
        Vector3Int cellPosition = baseTilemap.WorldToCell(worldPosition);
        TileBase tile = baseTilemap.GetTile(cellPosition);
        if (tile == null) return;
        if(tile.Equals(emptyTile))
        {
            StartSelection();
            return;
        }

        if (tile.Equals(extensionTile))
        {
            extensionPanel.transform.parent.gameObject.SetActive(true);
            extensionPanel.SetActive(true);
            extensionCostText.text = extensionCost.ToString();
            Button extensionButton = extensionPanel.GetComponentInChildren<Button>();
            extensionButton.interactable = MoneyViewer.IsEnough(extensionCost);
            _extensionPosition = cellPosition;
        }
    }

    public bool IsPositionSelectable(Vector3 position)
    {
        return baseTilemap.GetTile(baseTilemap.WorldToCell(position)).Equals(emptyTile);
    }
    private void StartSelection()
    {
        CurrentSelection = new Selection();
        Deselect();
        _isMousePressed = true;
    }

    private void Select()
    {
        selectionTilemap.ClearAllTiles();
        Deselected?.Invoke();
        int number = 0;
        foreach (var position in CurrentSelection.GetNewPositions())
        {
            Vector3Int cellCoord = grid.WorldToCell(position);
            selectionTilemap.SetTile(cellCoord, selectionTile);
            Selected?.Invoke(position, ++number);
        }
    }

    public void Deselect()
    {
        selectionTilemap.ClearAllTiles();
        CurrentSelection.Clear();
        Deselected?.Invoke();
    }

    public void SetMouseFlag(bool value)
    {
        _isMousePressed = value;
    }

    public bool GetMouseFlag()
    {
        return _isMousePressed;
    }

    private void SetExtensionTiles(Vector3Int coord)
    {
        Vector3Int nCoord = new Vector3Int(coord.x, coord.y-1);
        Vector3Int sCoord = new Vector3Int(coord.x, coord.y+1);
        Vector3Int wCoord = new Vector3Int(coord.x-1, coord.y);
        Vector3Int eCoord = new Vector3Int(coord.x+1, coord.y);
        SetNeighbourTile(nCoord);
        SetNeighbourTile(sCoord);
        SetNeighbourTile(wCoord);
        SetNeighbourTile(eCoord);
    }

    private void SetNeighbourTile(Vector3Int coord)
    {
        BoundsInt fieldBounds = new BoundsInt(fieldStartCorner, fieldSize);
        if (!baseTilemap.HasTile(coord) && fieldBounds.Contains(coord))
        {
            baseTilemap.SetTile(coord, extensionTile);
        }
    }

    public void Expand(int costMultiplier)
    {
        Expanded?.Invoke(-extensionCost);
        baseTilemap.SetTile(_extensionPosition, emptyTile);
        SetExtensionTiles(_extensionPosition);
        extensionCost *= costMultiplier;
    }
}
