using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementReturnBox : ElementContainer, IDropHandler
{
	[SerializeField]
	private GameObject _listParent;
	public override bool AllowRecieve()
	{
		return true;
	}

	public override bool AllowSend()
	{
		return false;
	}
	public void OnDrop(PointerEventData eventData)
	{
		Debug.Log("igotdropped");

		base.OnDrop(eventData);
	}

	public override void OnSuccessfullRecieve(GridCell recieveCell)
	{
		foreach (ElementListButton btn in _listParent.transform.GetComponentsInChildren<ElementListButton>())
		{
			Debug.Log("searching "+recieveCell.Type+" but found "+btn.Cell);
			if (btn.Cell.Type == recieveCell.Type)
			{
				btn.OnSuccessfullRecieve(recieveCell);
				break;
			}
		}
			Debug.Log("not found");
	}

	public override void OnSuccessfullSend(GridCell SendCell)
	{
		throw new System.NotImplementedException();
	}
}
