using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Клас, який контролює рух хмар на фоні/
public class BackgroundController : MonoBehaviour
{
	[SerializeField]
	List<Sprite> _cloudSprites;
	[SerializeField]
	Material _spriteMaterial;
	[SerializeField]
	int _upDownSpread=2;
	[SerializeField]
	int _leftRightSpread = 1;
	[SerializeField]
	int _cloudCount = 10;
	[SerializeField]
	float _cloudSpeed = 10;
	List<GameObject> _clouds = new();

    void Start()
    {
		//створюємо _cloudCount хмар
		for (int i = 0;i<_cloudCount;i++)
		{
			//створюємо пустий об'єкт
			var cloud = new GameObject();
			//додаємо йому спрайт, налаштовуємо його
			SpriteRenderer sprite = cloud.AddComponent<SpriteRenderer>();
			sprite.material = _spriteMaterial;
			sprite.sprite = _cloudSprites[Random.Range(0,_cloudSprites.Count)];
			sprite.sortingLayerName = "BackgroundSky";
			//встановлюємо випадкову позицію
			cloud.transform.position = new Vector3(i*_leftRightSpread,Random.Range(-_upDownSpread,+_upDownSpread+1)	);
	
			//робимо хмару дочірнім об'єктом контроллера
			sprite.transform.SetParent(transform);
			//додаємо хмару в списоу
			_clouds.Add(cloud);
		}
    }

	void Update()
	{
		//кожен кадр для кожної хмари
		Vector3 camPos = Camera.main.transform.position;
		foreach (GameObject cloud in _clouds)
		{			
			Vector3 newPos = (cloud.transform.position);

			//якщо хмара достатньо далеко від камери переміщаємо її в іншу сторону
			if ((newPos.x - camPos.x) > 50)
				newPos.x -= 100;
			if ((newPos.x - camPos.x) < -50)
				newPos.x += 100;

			if ((newPos.y - camPos.y) > 50)
				newPos.y -= 100;
			if ((newPos.y - camPos.y) < -50)
				newPos.y += 100;

			//отримуємо випадкове число, яке завжди однакове для цієї хмари 
			Random.InitState(cloud.GetInstanceID());
			float rand = Random.Range(0, _cloudSpeed);
			float speed = rand * Time.deltaTime;
			//рухаємо хмару вправо лінійно та вверх-вниз використовуючи функцію сінуса 
			newPos.x += speed;
			newPos.y += Mathf.Sin(Time.time*rand + rand ) * speed;

			cloud.transform.position = newPos;
		}
	}
}
