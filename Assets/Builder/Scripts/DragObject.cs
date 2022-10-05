using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
	private ElementType ElementType;
	private IClickable ParentButton;
	public DragObject(ElementType ElementType, IClickable Parent)
	{
		this.ElementType = ElementType;
		this.ParentButton = Parent;
	}
	public Tuple<ElementType, IClickable> GetTuple()
	{
		return new Tuple<ElementType, IClickable>(ElementType,ParentButton);
	}
}
