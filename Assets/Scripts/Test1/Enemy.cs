using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	
	void Update ()
	{
		if(transform.position.y < -1.0f )
		{
			Destroy(gameObject, 2f);
		}
		
	}
}
