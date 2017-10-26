using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemyCreator : MonoBehaviour
{

	[SerializeField]
	GameObject enemyPrefab;

	[SerializeField]
	BoxCollider area;

	List<GameObject> enemies;

	[SerializeField]
	Vector3 enemyFloorCenter;
	[SerializeField]
	Vector3 enemyFloorSize;

	Vector3 sizeHalf;

	GameObject testTgt = null;

	//CX,CZ,SX,SZ
	//左下 CX-SX/2, CY-SY/2
	//左上 CX-SX/2, CY+SY/2
	//右上 CX+SX/2, CY+SY/2
	//右下 CX+SX/2, CY-SY/2
	//if ( r.top > y || r.bottom<y || r.left> x || r.right<x ) {

	void Start()
	{
		sizeHalf = enemyFloorSize / 2;
		//StartToSpawnEnemy(c => testTgt = c);
	}


	void Update ()
	{

		if(Input.GetKeyDown(KeyCode.E))
		{
			//StartToSpawnEnemy( c => testTgt = c );


			var enePos = GetRandSpawnPos();
			var obj = SpawnEnemy(enePos);
			testTgt = obj;
			//Debug.Log("敵出現" );
		}

		if (testTgt != null)
		{
			if(IsOutOfAreaEnemy(testTgt))
			{
				//Debug.Log("範囲外");
				var enePos = GetRandSpawnPos();
				var obj = SpawnEnemy(enePos);
				testTgt = obj;
				//Debug.Log("敵出現");
			}
		}
	}

	Vector3 GetRandSpawnPos()
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


	void StartToSpawnEnemy( Action<GameObject> action )
	{
		var enePos = GetRandSpawnPos();
		var obj = SpawnEnemy(enePos);
		action(obj);

		//testTgt = obj;
	}

	GameObject SpawnEnemy( Vector3 pos )
	{
		var obj = Instantiate<GameObject>( enemyPrefab );
		//var sc = obj.GetComponent<GameObject>();
		obj.transform.position = pos;
		return obj;
	}

	bool IsOutOfAreaEnemy( GameObject enemy)
	{
		//Debug.Log("範囲判定: " + enemy.transform.position );
		var enePos = enemy.transform.position;
		var c = enemyFloorCenter;
		var s2 = sizeHalf;

		if (enePos.x < c.x - s2.x || enePos.x > c.x + s2.x|| 
			enePos.z < c.z - s2.z || enePos.z > c.z + s2.z)
			return true;

		return false;
	}
	

}

