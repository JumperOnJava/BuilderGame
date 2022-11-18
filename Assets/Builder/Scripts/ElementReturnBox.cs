using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//клас який повертає елемент в необхідний елемент списку якщо його перетягнули в пусте місце списку
public class ElementReturnBox : ElementContainer, IDropHandler
{
	[SerializeField]
	private GameObject _listParent;
	//дозволяємо отримання
	public override bool AllowRecieve()
	{
		return true;
	}
	//забороняємо відправку
	public override bool AllowSend()
	{
		return false;
	}
	public override void OnSuccessfullRecieve(GridCell recieveCell)
	{
		//проходимося по кожному елементу списку
		foreach (ElementListButton btn in _listParent.transform.GetComponentsInChildren<ElementListButton>())
		{
			
			if (btn.Cell.Type == recieveCell.Type)
			{
				//якщо знаходимо елемент в списку з підходящим типом елементу то відправляємо до нього даний елемент 
				btn.OnSuccessfullRecieve(recieveCell);
				break;
			}
		}
	}
	//заглушка, ця функція не повинна викликатися за нормальних умов так як відправка заблокована
	public override void OnSuccessfullSend(GridCell SendCell)
	{
		throw new System.NotImplementedException();
	}
}
