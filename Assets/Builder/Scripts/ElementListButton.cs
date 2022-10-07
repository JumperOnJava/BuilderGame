using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Mono.Cecil.Cil;
using System;
using System.Drawing;

public class ElementListButton : ElementContainer
{
	[SerializeField]
	private RectTransform _rectTransform;
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Image _elementIcon;
	private int _count;
	public void Init( GridCell cell,int count)
    {
		base.Init(cell);
        _count = count;
		UpdateText();
		UpdateIcon();
	}
	private void UpdateText()
	{
		_text.text = $"{Cell.Type} - {_count}";
	}
	public override void OnSuccessfullRecieve(GridCell gridCell)
	{
		Debug.Log(gridCell.Type != Cell.Type);
		_count++;
		UpdateText();
	}
	public override bool AllowRecieve() {return false;}
	public override bool AllowSend() { return _count>0;}
	public override void OnSuccessfullSend(GridCell gridCell)
	{
		Debug.Log("send?");
		_count--;
		UpdateText();
	}
	private void UpdateIcon()
    {
        _elementIcon.sprite = Cell.GetInfo().Sprite;
    }
}
