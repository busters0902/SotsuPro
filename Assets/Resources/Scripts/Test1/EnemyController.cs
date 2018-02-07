using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	[SerializeField]
	EnemySpawner spawner;

	GameObject tgt = null;

	public Action DeathEnemy{ private get; set; }

	bool isCreateEnemy = false;
	public bool canCreate = true;

	void Update ()
	{
		if (tgt != null && canCreate )
		{
			if (spawner.IsOutOfAreaEnemy(tgt))
			{
				DeathEnemy();
				tgt = spawner.StartToSpawnEnemy();
			}
		}

	}

	//ゲームスタート時
	public void InitSpawn()
	{
		if (tgt != null) Destroy(tgt);

		var obj = spawner.StartToSpawnEnemy();
		tgt = obj;
	}

}
