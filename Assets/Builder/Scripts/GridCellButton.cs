using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridCellButton : ElementContainer ,IDropHandler
{
    [SerializeField]
    private Image _elementIcon; 
    [SerializeField]
    private GameObject _rotateButton;
    private int height;
    private int width;
    public void Init(BuilderUiController controller,GridCell cell, int height, int width)
    {
		base.Init(cell);
        this.height = height;  
        this.width = width;
        UpdateIcon();
    }
    public void OnRotate()
    {
        Cell.Rotation = (ElementRotation)(((int)Cell.Rotation+3)%4);
        UpdateIcon();
    }
	public override void OnSuccessfullRecieve(GridCell recieveObject)
	{
		Debug.Log("recieved "+recieveObject.Type);
		this.Cell = recieveObject;
		UpdateIcon();
	}
	public override void OnSuccessfullSend(GridCell sendObject)
	{
		Debug.Log("recieved " + sendObject.Type);
		this.Cell = sendObject;
		UpdateIcon();
	}
	public override bool AllowRecieve() { return true; }
	public override bool AllowSend() { return true; }
	private void UpdateIcon()
    {
		_rotateButton.SetActive(Cell.GetInfo().Rotates);	
		_elementIcon.sprite = Cell.GetInfo().Sprite;
        _elementIcon.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (int)Cell.Rotation * 90));
    }
}
