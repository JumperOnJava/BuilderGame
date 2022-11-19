using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Клас комірки сітки редактора
public class GridCellButton : ElementContainer ,IDropHandler
{
    [SerializeField]
    private Image _elementIcon; 
    [SerializeField]
    private GameObject _rotateButton;
    [SerializeField]
	public NodeInput WireNode;
	private HashSet<GridCellButton> outputs;
	public void Init(BuilderUiController controller,GridCell cell)
    {
		base.Init(cell);
        UpdateIcon();
	}
	public void OnRotate()
    {
        Cell.Rotation = (ElementRotation)(((int)Cell.Rotation+3)%4);
		UpdateIcon();
	}
	//обробка успішного отримання 
	public override void OnSuccessfullRecieve(GridCell recieveObject)
	{
		this.Cell = recieveObject;
		UpdateIcon();
	}
	//обробка успішної відправки
	public override void OnSuccessfullSend(GridCell sendObject)
	{
		this.Cell = sendObject;
		UpdateIcon();
	}
	//дозвіл на отримання
	public override bool AllowRecieve() { return true; }
	//дозвіл на відправку
	public override bool AllowSend() { return true; }
	//оновлення комірки
	private void UpdateIcon()
    {
		//повертаємо іконку елементу в залежності від кута повороту комірки
		_elementIcon.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, (int)Cell.Rotation * 90));
		//вмикаємо/вимикаємо вузли з'єднання в залежності від того чи є цей елемент електричним
		if (Cell.GetInfo().CircuitElement == CircuitElement.None)
		{
			WireNode.DisableNode();	 
		}
		else
		{
			WireNode.EnableNode();
		}
		//вмикаємо кнопку повороту в залежності від того чи можна обертати елемент
		_rotateButton.SetActive(Cell.GetInfo().Rotates);
		//змінюємо спрайт
		_elementIcon.sprite = Cell.GetInfo().Sprite;
	}
}
