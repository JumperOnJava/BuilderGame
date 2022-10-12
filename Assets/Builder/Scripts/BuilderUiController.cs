using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class BuilderUiController : MonoBehaviour
{
    [Header("LevelData")]
    [SerializeField]
    private BuilderData _builderData;
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
    private GameObject _gridCellTemplate;

    private Dictionary<ElementType, int> _elementCount;

    //private List<List<GridCell>> _gridData;
    
    private void Start()
    {
        //_gridData = new List<List<GridCell>>.Data;
        _elementCount = new Dictionary<ElementType, int>(_builderData.Elements);
        _startLevelButton.Init(this);
        InitButtons();
        InitGrid();
    }
    private void InitButtons()
    {
        _elementCount[ElementType.Empty] = 0;
        foreach (Transform child in _elementList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (KeyValuePair<ElementType,int> element in _elementCount)
        {
            if (element.Value <= 0)
                continue;
            GameObject button = Instantiate(_elementListButtonTemplate);
            ElementListButton buttonComponent = button.GetComponent<ElementListButton>();
            buttonComponent.Init(new GridCell(element.Key,_allElementsInfo.GetInfo(element.Key)), element.Value);
            button.transform.SetParent(_elementList.transform);
        }
		_elementList.GetComponentInParent<ElementReturnBox>().Init(new GridCell(ElementType.Empty, _allElementsInfo.GetInfo(ElementType.Empty)));
    }
    private void InitGrid()
    {
        foreach (Transform child in _elementGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GridLayoutGroup gridObject = _elementGrid.GetComponent<GridLayoutGroup>();
        gridObject.constraintCount = _builderData.GridWidth;
        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
                GameObject gridCell = GameObject.Instantiate(_gridCellTemplate);
                GridCellButton gridCellComponent = gridCell.GetComponent<GridCellButton>();
                gridCellComponent.Init(this, new GridCell(ElementType.Empty, _allElementsInfo.GetInfo(ElementType.Empty)));
                gridCell.transform.SetParent(_elementGrid.transform);
            }
        }
    }
    public void SelectElementType(ElementType elementType)
    {
        //_currentElementType = elementType;

    }
    public void StartLevel()
    {
        Transform gridTransform = _elementGrid.transform;
		GridCellButton[,] gridElements = new GridCellButton[_builderData.GridHeight, _builderData.GridWidth];
		GameObject[,] elements = new GameObject[_builderData.GridHeight, _builderData.GridWidth];
		for(int i =0;i< _builderData.GridHeight; i++)
		{
			for (int j = 0; j < _builderData.GridWidth; j++)
			{
				gridElements[i,j] = gridTransform.GetChild(i*_builderData.GridWidth+j).GetComponent<GridCellButton>();
			}
		}
        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
				GridCellButton gridElement = gridElements[i, j];
                if (gridElement.Cell.Type == ElementType.Empty)
                {
                    elements[i, j] = null;
                    continue;
                }
                GameObject element = GameObject.Instantiate(gridElement.Cell.GetInfo().GetPrefab((int)gridElement.Cell.Rotation),gridElement.transform.position, Quaternion.Euler(new Vector3(0, 0, (int)gridElement.Cell.Rotation * 90)));
                element.transform.position = gridElement.transform.position;
                element.transform.SetParent(_simulationController.BuildParent.transform);
				//element.GetComponent<GenericElement>().transform.rotation =);
				elements[i, j] = element;
			}
		}
        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
                if (elements[i, j] == null)
                    continue;
                int[] i1 = { -1, 0, 1, 0 }; 
                int[] j1 = { 0, 1, 0, -1 };


				for (int k = 0; k < 4; k++)
                {
                    GameObject jointObject;
                    try
                    {
                        jointObject = elements[i + i1[k], j + j1[k]];
                    }
                    catch { continue; }
                    if (jointObject == null)
                        continue;
					GridCellButton gridElement = gridElements[i, j];
					GridCellButton gridElement2 = gridElements[i + i1[k],j + j1[k]];

					if (!(gridElement.Cell.ShouldConnect(k) && gridElement2.Cell.ShouldConnect(k + 2))) { continue; }
                    
                    elements[i, j].GetComponent<GenericElement>().ConnectWith(jointObject.GetComponent<GenericElement>().GetJointObject());
                }
            }
        }
		for (int i = 0; i < _builderData.GridHeight; i++)
		{
			Debug.Log(6);
			for (int j = 0; j < _builderData.GridWidth; j++)
			{
				Debug.Log(5);
				if (gridElements[i,j].WireDot.gameObject.activeInHierarchy)
				{
					Debug.Log(4);
					Debug.Log($"{gridElements[i, j].WireDot.isActiveAndEnabled}[{i},{j}] - {gridElements[i, j].WireDot.GetWireDots().Count}");
					foreach (InputWireNode output in gridElements[i, j].WireDot.GetWireDots())
					{
						Debug.Log(3);
						for (int i2 = 0; i2 < _builderData.GridHeight; i2++)
						{
							Debug.Log(2);
							for (int j2 = 0; j2 < _builderData.GridWidth; j2++)
							{
								Debug.Log(1);
								if (gridElements[i2,j2].WireDot == output)
								{
									Debug.Log($"{elements[i, j].GetComponent<ElectricElement>().GetType()} outputs to {elements[i2, j2].GetComponent<ElectricElement>().GetType()}");
									elements[i, j].GetComponent<ElectricElement>().AddOutput(elements[i2, j2].GetComponent<ElectricElement>());
									break;
								}
							}
						}
					}
				}
			}
		}
		_simulationController.gameObject.SetActive(true);
    }
}
