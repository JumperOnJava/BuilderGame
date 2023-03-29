using UnityEngine;

//Клас який дає інформацію про різні види елементів
[CreateAssetMenu(fileName = "AllElementsInfo", menuName = "All Elements Info")]
public class AllElementsInfo : ScriptableObject
{
    [SerializeField]
    private UDictionary<ElementType, ElementInfo> _elementInfo = new UDictionary<ElementType, ElementInfo>();
    public ElementInfo GetInfo(ElementType type)
    {
        return _elementInfo[type];
    }
}
