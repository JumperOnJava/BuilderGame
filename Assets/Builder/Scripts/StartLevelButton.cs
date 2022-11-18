using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Клас кнопки запуску рівня
public class StartLevelButton : MonoBehaviour
{
    private BuilderUiController _controller;
    public void Init(BuilderUiController controller)
    {
        _controller = controller;
    }
    public void OnClick()
    {
        _controller.StartLevel();
    }
}
