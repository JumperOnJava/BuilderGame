using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//Клас який при запуску рівня:
//Створює сітку редактора
//Заповнює список елементів
//"Збирає" всі елементи в винахід
public class BuilderUiController : MonoBehaviour
{
    [Header("LevelData")]
    [SerializeField]
    private BuilderData _builderData;
	public BuilderData BuilderData
	{
		get { return _builderData; }
		set { _builderData = value; }
	}

    [SerializeField]
    private AllElementsInfo _allElementsInfo;
    [SerializeField]
    private SimulationController _simulationController;

    [Space(5)]
    [Header("Buttons")]
    [SerializeField]
    private StartLevelButton _startLevelButton;

	[Space(5)]
    [Header("ElementList")]
    [SerializeField]
    private GameObject _elementList;
    [SerializeField]
    private GameObject _elementListButtonTemplate;
    
    [Space(1)]
    [Header("ElementGrid")]
    [SerializeField]
    private GameObject _elementGrid;
    [SerializeField]
    private GameObject _IOGrid;
    [SerializeField]
    private GameObject _gridCellTemplate;

    private Dictionary<ElementType, int> _elementCount;

	//private List<List<GridCell>> _gridData;
	public void Init(BuilderData builderData)
    {

		_builderData = builderData;
		_elementCount = new Dictionary<ElementType, int>(_builderData.Elements);
        _startLevelButton.Init(this);
        //Заповнення списку елементів та сітки
        InitButtons();
        InitGrid();
    }
    private void InitButtons()
    {
        _elementCount[ElementType.Empty] = 0;
        //На всякий випадок очищаємо список перед заповненням
        foreach (Transform child in _elementList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //заповнюємо список згідно даним рівня
        foreach (KeyValuePair<ElementType,int> element in _elementCount)
        {
            if (element.Value <= 0)
                continue;
            GameObject button = Instantiate(_elementListButtonTemplate);
            //Створюємо елемент списку та ініціюємо його
            ElementListButton buttonComponent = button.GetComponent<ElementListButton>();
            buttonComponent.Init(new GridCell(element.Key,_allElementsInfo.GetInfo(element.Key)), element.Value);
            button.transform.SetParent(_elementList.transform);
        }

		_elementList.GetComponentInParent<ElementReturnBox>().Init(new GridCell(ElementType.Empty, _allElementsInfo.GetInfo(ElementType.Empty)));
    }
    private void InitGrid()
    {
        //На всякий випадок очищаємо сітку перед заповненням
        foreach (Transform child in _elementGrid.transform)
        {
            Destroy(child.gameObject);
        }
        //отримуємо об'єкт сітки та об'єкт сітки вузлів схеми, встановлюємо йому необхідну ширину (в комірках) в залежності від даних рівня
        _elementGrid.GetComponent<GridLayoutGroup>().constraintCount = _builderData.GridWidth;
		_IOGrid.GetComponent<GridLayoutGroup>().constraintCount= _builderData.GridWidth;

        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
                //створюємо n комірок, n = висота * ширина (в комірках)
				GameObject gridCell = Instantiate(_gridCellTemplate);
				GridCellButton gridCellComponent = gridCell.GetComponent<GridCellButton>();
                gridCellComponent.Init(this, new GridCell(ElementType.Empty, _allElementsInfo.GetInfo(ElementType.Empty)));
				gridCell.transform.SetParent(_elementGrid.transform);
			}
		}
    }
    //непотрібна фунцкія, потрібно видалити
    public void SelectElementType(ElementType elementType)
    {
        //_currentElementType = elementType;
    }

    //дії при запуску рівня
    public void StartLevel()
    {
        Transform gridTransform = _elementGrid.transform;
        //створюємо двовимірний масив комірок та об'єктів елементів 
		GridCellButton[,] gridElements = new GridCellButton[_builderData.GridHeight, _builderData.GridWidth];
		GameObject[,] elements = new GameObject[_builderData.GridHeight, _builderData.GridWidth];

		for(int i =0;i< _builderData.GridHeight; i++)
		{
			for (int j = 0; j < _builderData.GridWidth; j++)
			{
                //заповнюємо масив комірок комірками, які належать сітці 
				gridElements[i,j] = gridTransform.GetChild(i*_builderData.GridWidth+j).GetComponent<GridCellButton>();
			}
		}
        //проходимося по кожному елементу масиву комірок
        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
                //отримуємо поточну комірку
				GridCellButton gridElement = gridElements[i, j];
                //якщо комірка пуста то пропускаємо наступні дії
                if (gridElement.Cell.Type == ElementType.Empty)
                {
                    elements[i, j] = null;
                    continue;
                }
                //створюємо об'єкт за шаблоном елементу з комірки та налаштовуємо його 
                GameObject element = GameObject.Instantiate(gridElement.Cell.GetInfo().Prefab,
                                                            gridElement.transform.position,
                                                             Quaternion.Euler(
                                                                new Vector3(0, 0, (int)gridElement.Cell.Rotation * 90)
                                                                )
                                                            );
                element.transform.position = gridElement.transform.position;
                element.transform.SetParent(_simulationController.BuildParent.transform);
				//element.GetComponent<GenericElement>().transform.rotation =);
				elements[i, j] = element;
			}
		}
        //проходимося по кожному елементу масиву об'єктів
        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
                //якщо елемент пустий то пропускаємо ітерацію циклу
                if (elements[i, j] == null)
                    continue;

                //невеличке пояснення до цих двох масивів
                //в наступному циклі нам потрібно для кожну сторону елементу з'єднати з елементом який знаходиться по цю сторону(якщо він існує там)
                //я міг 4 рази для кожної сторони здублювати код накшталп Connect(x+1, y); Connect(x-1, y); Connect(x, y+1); Connect(x, y-1);
                //але замість цього можна створити масив необхідних зміщень, і з'єднати необхінді елементи за цим масивом
                int[] i1 = { -1, 0, 1, 0 }; 
                int[] j1 = { 0, 1, 0, -1 };
                //в цьому випадку це також зручно тим що в інших місцях поворот тако

                //проходимося по кожній стороні елементу
				for (int k = 0; k < 4; k++)
                {
                    GameObject jointObject;
                    try
                    {
                        //отримуємо необхідний елемент (якщо він існує)
                        jointObject = elements[i + i1[k], j + j1[k]];
                    }
                    catch { continue; }
                    if (jointObject == null)
                        continue;

					GridCellButton gridElement = gridElements[i, j];
					GridCellButton gridElement2 = gridElements[i + i1[k],j + j1[k]];
                    //перевіряємо чи повинні з'єднуватися ці об'єкти
					if (!(gridElement.Cell.ShouldConnect(k) && gridElement2.Cell.ShouldConnect(k + 2))) { continue; }
                    //якщо так, то з'єднуємо їх
                    elements[i, j].GetComponent<GenericElement>().ConnectWith(
                        //об'єкт, до якого повинні приєднуватися інші об'єкти які не є цим елементом
                        jointObject.GetComponent<GenericElement>().GetJointObject()
                        );
                }
            }
        }
        //Проходимось по кожній комірці сітки
		for (int i = 0; i < _builderData.GridHeight; i++)
		{
			for (int j = 0; j < _builderData.GridWidth; j++)
			{
                //якщо цей елемент є елементом схеми
				if (gridElements[i,j].WireNodeMinus.gameObject.activeInHierarchy)
				{
                    //для кожного з його з'єднань
					foreach (InputWireNode output in gridElements[i, j].WireNodeMinus.GetNodes())
					{
                        //знаходимо елемент цього з'єднання
						for (int i2 = 0; i2 < _builderData.GridHeight; i2++)
						{
							for (int j2 = 0; j2 < _builderData.GridWidth; j2++)
							{
                                //якщо це необхідний елемент
								if (gridElements[i2,j2].WireNodeMinus == output)
								{
                                    //додаємо зв'язок між цими елементами
									elements[i, j].GetComponent<ElectricElement>().AddOutput(elements[i2, j2].GetComponent<ElectricElement>());
									break;
								}
							}
						}
					}
				}
			}
		}
        //активуємо контроллер "симуляції"
		_simulationController.gameObject.SetActive(true);
        //передаємо камері список елементів за якими потрібно слідкувати
		Camera.main.GetComponent<CameraMovement>().UpdateCenterElements(FindObjectsOfType<GenericElement>().ToList());
    }
}
