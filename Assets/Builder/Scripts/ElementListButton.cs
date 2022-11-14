using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementListButton : ElementContainer
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Image _elementIcon;
	[SerializeField]
	private Sprite _avaliable; 
	[SerializeField]
	private Sprite _unavaliable;
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
		_text.text = $"{Cell.GetInfo().Name} - {_count}";
		GetComponent<Image>().sprite = _count != 0 ? _avaliable : _unavaliable;
	}
	public override void DropHandler(ElementContainer container)
	{
		Debug.Log("drop on list");
		foreach (ElementListButton btn in FindObjectsOfType<ElementListButton>())
		{
			Debug.Log("searching " + container.Cell.Type + " but found " + btn.Cell);
			if (btn.Cell.Type == container.Cell.Type)
			{
				btn.OnSuccessfullRecieve(container.Cell);
				container.OnSuccessfullSend(GridCell.EmptyCell);
				break;
			}
		}
	}
	public override void OnSuccessfullRecieve(GridCell gridCell)
	{
		_count++;
		UpdateText();
	}
	public override bool AllowRecieve() {return true;}
	public override bool AllowSend() { return _count>0;}
	public override void OnSuccessfullSend(GridCell gridCell)
	{
		_count--;
		UpdateText();
	}
	private void UpdateIcon()
    {
        _elementIcon.sprite = Cell.GetInfo().Sprite;
    }
}
