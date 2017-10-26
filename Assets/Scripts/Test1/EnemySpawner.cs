using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	[SerializeField]
	GameObject enemyPrefab;

	[SerializeField]
	BoxCollider area;

	//List<GameObject> enemies;

	[SerializeField]
	Vector3 enemyFloorCenter;

	[SerializeField]
	Vector3 enemyFloorSize;

	Vector3 sizeHalf;

	GameObject testTgt = null;

	void Start() { sizeHalf = enemyFloorSize / 2; }

	public void SpawnEnemyOnWorld()
	{

	}

	public Vector3 GetRandSpawnPos()
	{
		var s = area.size;
		var p = area.center;

		var ini = area.size - area.center;

		Vector3 area_ = new Vector3();
		var scl = area.gameObject.transform.localScale;
		area_.x = ini.x * scl.x;
		area_.y = ini.y * scl.y;
		area_.z = ini.z * scl.z;

		var pos = area.gameObject.transform.position - area_ / 2;

		//Debug.Log( pos + " : " + area_);

		var spawnPos = new Vector3(
			pos.x + UnityEngine.Random.Range(0, area_.x),
			pos.y + UnityEngine.Random.Range(0, area_.y),
			pos.z + UnityEngine.Random.Range(0, area_.z));

		Debug.Log("出現座標: " + spawnPos);
		return spawnPos;
	}


	public void StartToSpawnEnemy(Action<GameObject> action)
	{
		//出現座標取得
		var enePos = GetRandSpawnPos();

		//生成して出現
		var obj = SpawnEnemy(enePos);

		//任意の処理
		action(obj);
	}

	public GameObject StartToSpawnEnemy()
	{
		//出現座標取得
		var enePos = GetRandSpawnPos();

		//生成して出現
		var obj = SpawnEnemy(enePos);

		return obj;
	}

	public GameObject SpawnEnemy(Vector3 pos)
	{
		var obj = Instantiate<GameObject>(enemyPrefab);
		obj.transform.position = pos;
		return obj;
	}

	public bool IsOutOfAreaEnemy(GameObject enemy)
	{
		//Debug.Log("範囲判定: " + enemy.transform.position );
		var enePos = enemy.transform.position;
		var c = enemyFloorCenter;
		var s2 = sizeHalf;

		if (enePos.x < c.x - s2.x || enePos.x > c.x + s2.x ||
			enePos.z < c.z - s2.z || enePos.z > c.z + s2.z)
			return true;

		return false;
	}


}
