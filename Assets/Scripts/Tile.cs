using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _highlightColor;
    private SpriteRenderer _sprite;
    private Color _baseColor;
    //private List<ITileObserver> observers = new List<ITileObserver>();

    public event Action<Vector3> OnTileClicked;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _baseColor = _sprite.color;
    }

    private void OnMouseEnter()
    {
        print(name);
        _sprite.color = _highlightColor;
    }

    private void OnMouseDown()
    {
        OnTileClicked?.Invoke(gameObject.transform.position);
    }

    private void OnMouseExit()
    {
        _sprite.color = _baseColor;
    }


}
