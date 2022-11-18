using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Клас який представляє елемент в списку елементів
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
	public void Init(GridCell cell,int count)
    {
		base.Init(cell);
        _count = count;
		UpdateCount();
		UpdateIcon();
	}
	private void UpdateCount()
	{
		//оновлюємо текст, спрайт елементу списку
		_text.text = $"{Cell.GetInfo().Name} - {_count}";
		GetComponent<Image>().sprite = _count != 0 ? _avaliable : _unavaliable;
	}
	public override void DropHandler(ElementContainer container)
	{
		//при отриманні елементу виконуємо ті ж самі кроки, що і в функції OnSuccessfullRecieve в класі ElementReturnBox
		foreach (ElementListButton btn in FindObjectsOfType<ElementListButton>())
		{
			if (btn.Cell.Type == container.Cell.Type)
			{
				btn.OnSuccessfullRecieve(container.Cell);
				container.OnSuccessfullSend(GridCell.EmptyCell);
				break;
			}
		}
	}
	//при успішному отриманні збільшуємо к-сть елементів в елементі списку на 1
	public override void OnSuccessfullRecieve(GridCell gridCell)
	{
		_count++;
		UpdateCount();
	}
	//дозволяємо отримання
	public override bool AllowRecieve() {return true;}
	//дозволяємо відправку тільки якщо кількість елементів більше нуля
	public override bool AllowSend() { return _count>0;}
	
	//при успішній відправці зменшуємо к-сть елементів в елементі списку на 1	
	public override void OnSuccessfullSend(GridCell gridCell)
	{
		_count--;
		UpdateCount();
	}
	private void UpdateIcon()
    {
		//оновлення іконки
        _elementIcon.sprite = Cell.GetInfo().Sprite;
    }
}
