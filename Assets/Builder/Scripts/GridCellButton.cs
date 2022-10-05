using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCellButton : MonoBehaviour ,IDropHandler
{
    private GridCell _element;
    private BuilderUiController _controller;
    [SerializeField]
    private Image _elementIcon; 
    [SerializeField]
    private GameObject _rotateButton;
    private int height;
    private int width;
    public void Init(BuilderUiController controller,GridCell element, int height, int width)
    {
        _controller = controller;
        _element = element;
        _rotateButton.SetActive(element.GetInfo().Rotates);
        this.height = height;  
        this.width = width;
        UpdateIcon();
    }
    public void OnClick()
    {
        _controller.ClickElement(height, width);
    }
    public void OnRotate()
    {
        _element.Rotation = (ElementRotation)(((int)_element.Rotation+3)%4);
        UpdateIcon();
    }
    public void OnDrop(PointerEventData eventData)
    {

    }
    private void UpdateIcon()
    {
        _elementIcon.sprite = _element.GetInfo().Sprite;
        _elementIcon.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (int)_element.Rotation * 90));
    }
}
