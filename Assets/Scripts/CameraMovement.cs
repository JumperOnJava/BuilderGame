using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.U2D;
//клас камери
public class CameraMovement : MonoBehaviour
{
    private Camera _camera;
	[SerializeField]
	private float _minSize = 2, _maxSize = 16;
	[SerializeField]
	public float _upClamp;
	[SerializeField]
	public float _downClamp;

	[SerializeField]
	public float _leftClamp;
	[SerializeField]
	public float _rightClamp;

    private Vector3 _mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private Vector3 _prevMousePos = Vector3.one;
	//зміна позиції миші відносно минулого кадру
    private Vector3 _mouseDelta =>  _prevMousePos - _mousePos;

	[SerializeField]
	public List<Vector2> _startPath;
	[SerializeField]
	private float _pathSpeed;

	[SerializeField]
	public Vector3 mousepos;
	[SerializeField]
	public Vector3 mousedelta;
	public float completedLenght = 0;

	[SerializeField]
    public Vector3 _targetPos;
    private float _targetSize;
	[SerializeField]
	private float _coreCounter=10;
	private float _coreTime=4;

	private List<GenericElement> _elements;
	
	IEnumerator PathCorountine()
	{
		//встановлюємо стандартний розмір камери
		var startSize = _targetSize;
		//дізнаємося довжину шляху
		float length = 0;
		for (int i = 0; i < _startPath.Count - 1; i++)
		{
			length += Vector2.Distance(_startPath[i], _startPath[i + 1]);
		}
		//поки пройдена відстань менше довжини шляху
		for (; completedLenght <= length; completedLenght += _pathSpeed)
		{
			try
			{
				//збільшуємо пройдений шлях на цей кадр
				completedLenght += Time.deltaTime * _pathSpeed;
				//якщо гравець нажме пкм, то закінчуємо шлях
				if (_coreCounter < _coreTime)
					break;
			
				//тепер нам потрібно знайти точку, на якій повинна бути камера цього кадру

				float countLenght = 0;
				int i = 0;
				//знаходимо на якому відрізку повинна бути камера
				while (countLenght < completedLenght)
				{
					countLenght += Vector2.Distance(_startPath[i], _startPath[i + 1]);
					i++;
				}
				//змінюємо розмір камери в залежності від того скільки яку частину шляху ми пройшли
				_targetSize = startSize + CustomMathFunctions.Lerp010(completedLenght / length)*4;


				i = Mathf.Clamp(i, 1, _startPath.Count - 1);
				completedLenght = Mathf.Clamp(completedLenght, 0, length);

				//знаходимо яку відстань ми пройшли по поточному відрізку
				var last = countLenght - completedLenght;
				//знаходимо довжину поточного відрізка
				var dist = Vector2.Distance(_startPath[i], _startPath[i - 1]);
				//Знаходимо яку частину поточного відрізка ми пройшли 
				var perc = last / dist;
				//знаходимо на якій точці відрізку повинна бути камера  				
				_targetPos = Vector2.Lerp(_startPath[i], _startPath[i - 1], perc);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				break;
			}
			//чекаємо наступний кадр
			yield return null;
		}
	}
	
	void Start()
    {
        _camera = GetComponent<Camera>();
		_targetPos = _camera.transform.position;
		_targetSize = _camera.orthographicSize;
		//при старті рівня рухаємо камеру від фінішу до старту по заданим точкам
		StartCoroutine(PathCorountine());
    }
	//кожен кадр
	void FixedUpdate()
    {

		mousedelta = _mouseDelta;
		mousepos =_mousePos;
		//Змінні плавного переміщення
		//позиції камери
		float n = 8;
		//розміру камери
		float m = 3;
		//якщо ПКМ (права кнопка миші) натиснута
		if (Input.GetMouseButton(1))
        {
			//встановлюємо швидкість плавного переміщення на 3
			n = 3;
			//скидуємо лічильник бездії
			_coreCounter = 0;
			//переміщаємо ціль камери разом з мишою відносно поточної позиції (миша 10 одиниць вліво - ціль 10 одиниць вліво)
			_targetPos += _mouseDelta;
        }
		//інакше
		else
		{
			//Збільшуємо час бездії
			_coreCounter += Time.deltaTime;
		}
		//якщо лічильник бездії більше певного часу, переміщаємо камеру за візком
		if(_coreCounter > _coreTime)
		{
			try
			{
				//я написав цей код перед сном, а ось проснувсь і вже не можу спам'ятати як він працює, і що він робить
				n = (1-Mathf.Clamp(_coreCounter-_coreTime,0,1))*30+1;

				//встановлюємо ціль камери як центральну точку між всіма елементами винаходу
				_targetPos = GetCenterPoint();
			}
			catch 
			{
				//якщо виникає якась помилка пов'язана з відсутністю елементів то скидуємо лічильник бездії для запобігання помилок
				_coreCounter = 0;
			}
		}

		

		_targetPos.x = Math.Clamp(_targetPos.x, _leftClamp, _rightClamp);
		_targetPos.y = Math.Clamp(_targetPos.y, _downClamp, _upClamp);

		//Досить цікава формула плавного переміщення: чим ближче ми до необхіного значення, тим повільніше ми приближаємося до нього
		//працює вона таким чином: у нас є два числа - поточна позиція та цільова позиція
		//нехай поточна позиція = 0, а цільова = 100
		//швидкість плавного переміщення, n = 4
		//кожен кадр ми знаходимо середнє арифметичне n-1 поточних та однієї цільової позиції 
		//тоді на перший кадр позиція буде 0
		//на другому (0+0+0+100)/4 = (0*3+100) = 25
		//на третьому (25*3+100)/4 = 43.75 
		//на четвертому (43.75*3+100) ~= 57
		//і так далі

		transform.position = (transform.position * (n - 1) + (_targetPos)) / n;
        transform.position = new Vector3(transform.position.x,transform.position.y,-1);

		//така сама формула використовується для зміни розміру камери

        _targetSize += -Input.mouseScrollDelta.y*2;
        _targetSize = Mathf.Clamp(_targetSize, _minSize, _maxSize);
        _camera.orthographicSize = (_camera.orthographicSize * (m-1) + _targetSize)/m;

        _prevMousePos = _mousePos;
	}
	//Встановлення списку елементів, за якими буде вираховуватись цільова позиція при бездії
	public void UpdateCenterElements(List<GenericElement> elements)
	{
		_elements = elements;
	}
	//Функція яка вираховує цільову позицію при бездії
	private Vector3 GetCenterPoint()
	{
		try
		{
			//знаходимо середнє арифметичне позицій всіх елементів
			Vector3 center = Vector3.zero;
			foreach (GenericElement element in _elements)
			{
				center += element.transform.position;
			}
			//якщо в списку нуль елементів, то повертаємо виключення
			if(_elements.Count!=0)
			center /= _elements.Count;
			else
			throw new Exception();

			return center;
		}
		catch
		{
			throw new Exception();
		};
	}
	//Встановлюємо цільову точку камери 
	internal void MoveTargetPosTo(GameObject gameObject)
	{
		_targetPos = gameObject.transform.position;
	}
}
