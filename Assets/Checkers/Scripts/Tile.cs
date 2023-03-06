using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int position;
    public bool isOccupied;
    public Color defaultColor;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        defaultColor = _renderer.material.color;
    }

    public void SetHighlight(bool highlight)
    {
        if (highlight)
        {
            _renderer.material.color = Color.yellow;
        }
        else
        {
            _renderer.material.color = defaultColor;
        }
    }

}
