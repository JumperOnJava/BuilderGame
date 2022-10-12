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
    [SerializeField]
	public InputWireNode WireDot;
	private HashSet<GridCellButton> outputs;
	public void ModifyOutput(GridCellButton cell)
	{
		if (outputs.Contains(cell))
		{
			outputs.Remove(cell);
		}
		else
		{
			outputs.Add(cell);
		}
	}
	public void Init(BuilderUiController controller,GridCell cell)
    {
		base.Init(cell);
        UpdateIcon();
    }
    public void OnRotate()
    {
        Cell.Rotation = (ElementRotation)(((int)Cell.Rotation+3)%4);
        _elementIcon.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (int)Cell.Rotation * 90));
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
    }
}
