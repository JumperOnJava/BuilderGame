using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Mono.Cecil.Cil;

public class ElementListButton : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IClickable
{
	[SerializeField]
	private RectTransform _rectTransform;
	private Canvas _canvas => _controller.Canvas;
	private GameObject _dragSprite;
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Image _elementIcon;
    [SerializeField]
    AllElementsInfo AllElementsInfo;
    private ElementType _elementType;
    private int _count;
    private BuilderUiController _controller;
	public void Init(BuilderUiController controller,ElementType type,int count)
    {
		_controller = controller;
        _elementType = type;
        _count = count;
        _text.text = $"{_elementType} - {count}";

        UpdateIcon();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
		
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
		_dragSprite = Instantiate(gameObject);
		_dragSprite.transform.position = _dragSprite.transform.position.ComponentMultiply(_canvas.transform.localScale);
		_dragSprite.transform.localScale = _canvas.gameObject.transform.localScale;
		_dragSprite.transform.SetParent(_canvas.gameObject.transform);
	}
	public void OnDrag(PointerEventData eventData)
	{
		var vc = (eventData.);
		_dragSprite.transform.position += new Vector3(vc.x,vc.y,0);
		Debug.DrawLine(_dragSprite.transform.position, Vector3.zero	,Color.red,1);
		GameObject.FindGameObjectWithTag("MainCamera");
	}
	public void OnEndDrag(PointerEventData eventData)
    {
		Destroy(_dragSprite);
    }
    public void OnClick()
    {
        _controller.SelectElementType(_elementType);
    }
    private void UpdateIcon()
    {
        _elementIcon.sprite = AllElementsInfo.GetInfo(_elementType).Sprite;
    }
}
