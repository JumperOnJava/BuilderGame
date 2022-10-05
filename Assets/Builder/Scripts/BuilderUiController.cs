using Newtonsoft.Json.Linq;
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
    private GameObject _builderParent;

    [Space(5)]
    [Header("Buttons")]
    [SerializeField]
    private DeleteElementButton _deleteButton;
    [SerializeField]
    private StartLevelButton _startLevelButton;

	[Space(5)]
	[Header("Canvas")]
	[SerializeField]
	public Canvas Canvas;
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
    private ElementType _currentElementType;

    private List<List<GridCell>> _gridData;
    
    private void Start()
    {
        _gridData = new GridData(_builderData.GridHeight, _builderData.GridWidth,_allElementsInfo).Data;
        _elementCount = new Dictionary<ElementType, int>(_builderData.Elements);
        _deleteButton.Init(this);
        _startLevelButton.Init(this);
        UpdateButton();
        UpdateGrid();
    }
    private void UpdateButton()
    {
        _elementCount[ElementType.Empty] = 0;
        foreach (Transform child in _elementList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (var element in _elementCount)
        {
            if (element.Value <= 0)
                continue;
            var button = GameObject.Instantiate(_elementListButtonTemplate);
            var buttonEdit = button.GetComponent<ElementListButton>();
            buttonEdit.Init(this, element.Key, element.Value);
            button.transform.SetParent(_elementList.transform);
        }
    }
    private void UpdateGrid()
    {
        foreach (Transform child in _elementGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        var gridEdit = _elementGrid.GetComponent<GridLayoutGroup>();
        gridEdit.constraintCount = _builderData.GridWidth;
        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
                var gridCell = GameObject.Instantiate(_gridCellTemplate);
                var gridCellEdit = gridCell.GetComponent<GridCellButton>();
                gridCellEdit.Init(this, _gridData[i][j], i, j);
                gridCell.transform.SetParent(_elementGrid.transform);
            }
        }
    }
    public void ClickElement(int height, int width)
    {
        var cell = _gridData[height][width];
        if (_currentElementType != _gridData[height][width].Type)
        {
            if (_elementCount[_currentElementType] > 0 || _currentElementType == ElementType.Empty)
            {
                var prevType = _gridData[height][width].Type;
                _elementCount[_currentElementType]--;
                _elementCount[prevType]++;
                cell.Type = _currentElementType;
            };
        }
        else
        {
            _elementCount[cell.Type]++;
            cell.Type = ElementType.Empty;
        }
        UpdateButton();
        UpdateGrid();
    }
    public void SelectElementType(ElementType elementType)
    {
        _currentElementType = elementType;

    }
    public void StartLevel()
    {
        gameObject.SetActive(false);
        var gridTransform = _elementGrid.transform;
        GameObject[,] elements = new GameObject[_builderData.GridHeight, _builderData.GridWidth];
        for (int i = 0; i < _builderData.GridHeight; i++)
        {
            for (int j = 0; j < _builderData.GridWidth; j++)
            {
                var gridElement = gridTransform.GetChild(i*_builderData.GridWidth+j);
                if (_gridData[i][j].Type == ElementType.Empty)
                {
                    elements[i, j] = null;
                    continue;
                }
                var element = GameObject.Instantiate(_gridData[i][j].GetInfo().GetPrefab((int)_gridData[i][j].Rotation),gridElement.position, Quaternion.Euler(new Vector3(0, 0, (int)_gridData[i][j].Rotation * 90)));
                element.transform.position = gridElement.position;
                element.transform.SetParent(_builderParent.transform);
                //element.GetComponent<GenericElement>().transform.rotation =);
                elements[i, j] = element;
            }
        }
        //Debug.Break();
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
                    if (!(_gridData[i][j].ShouldConnect(k) && _gridData[i + i1[k]][j + j1[k]].ShouldConnect(k + 2))) { continue; }
                    
                    elements[i, j].GetComponent<GenericElement>().ConnectWith(jointObject.GetComponent<GenericElement>().GetJointObject());
                }
            }
        }
    }
}
