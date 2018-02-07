using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{

	[SerializeField]
	GameObject spawnPrefab;

	[SerializeField]
	Vector3 spawnPoint;

	[SerializeField]
	KeyCode spawnKey;

	void Update()
	{
		if (Input.GetKeyDown(spawnKey)) Create();
	}

	GameObject Create()
	{
		var obj = Instantiate<GameObject>(spawnPrefab);
		obj.transform.position = spawnPoint;
		return obj;
	}

}
