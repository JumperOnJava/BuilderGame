using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeleteElementButton : MonoBehaviour
{
    private BuilderUiController _controller;
    public void Init(BuilderUiController controller)
    {
        _controller = controller;
    }
    public void OnClick()
    {
        _controller.SelectElementType(ElementType.Empty);
    }
}
