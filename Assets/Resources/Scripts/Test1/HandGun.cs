using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{

	[SerializeField]
	GameObject handObj;

	[SerializeField]
	GameObject gunTurret;

	[SerializeField]
	GameObject shotPos;

	[SerializeField]
	GameObject shotDir;

	[SerializeField]
	GameObject bulletPrefab;

	private bool isToAttack;
	private bool canAttack;

	private float shotPower;


	void Start ()
	{
		canAttack = true;
	}
	
	void Update ()
	{

		if(isToAttack)
		{
			Shot(shotPower);
			isToAttack = false;
		}

	}

	public void ToAttack( float power )
	{
		//Debug.Log("攻撃させる");
		if (canAttack == false)
			return;
		if (isToAttack == false)
		{
			isToAttack = true;
			shotPower = power;
		}
	}

	public void Shot( float power )
	{
		var obj = Instantiate<GameObject>(bulletPrefab);
		obj.transform.position = shotPos.transform.position;
		var dir = (shotDir.transform.position - shotPos.transform.position);
		var rig =　obj.GetComponent<Rigidbody>();
		rig.velocity = dir * power;
	}

}
