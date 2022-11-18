using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
		for (int i = 0;i<_cloudCount;i++)
		{
			var cloud = new GameObject();
			SpriteRenderer sprite = cloud.AddComponent<SpriteRenderer>();
			sprite.material = _spriteMaterial;
			sprite.sprite = _cloudSprites[Random.Range(0,_cloudSprites.Count)];
			sprite.sortingLayerName = "BackgroundSky";
			cloud.transform.position = new Vector3(i*_leftRightSpread,Random.Range(-_upDownSpread,+_upDownSpread+1)	);
			sprite.transform.SetParent(transform);
			_clouds.Add(cloud);
		}
    }

	// Update is called once per frame
	void Update()
	{
		foreach (GameObject cloud in _clouds)
		{
			Vector3 camPos = Camera.main.transform.position;
			Vector3 newPos = (cloud.transform.position);
			if ((newPos.x - camPos.x) > 50)
				newPos.x -= 100;
			if ((newPos.x - camPos.x) < -50)
				newPos.x += 100;

			if ((newPos.y - camPos.y) > 50)
				newPos.y -= 100;
			if ((newPos.y - camPos.y) < -50)
				newPos.y += 100;

			Random.InitState(cloud.GetInstanceID());
			float rand = Random.Range(0, _cloudSpeed);
			float speed = rand * Time.deltaTime;
			newPos.x += speed;
			newPos.y += Mathf.Sin(Time.time*rand + rand ) * speed;

			cloud.transform.position = newPos;
		}
	}
}
