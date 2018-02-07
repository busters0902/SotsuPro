using UnityEngine;
using System.Collections;

public class Timer
{

	public float time;
	public float deltaTime;

	float startTime;
	public float StartTime { get{ return startTime; }  }
	float prevTime;
	public float PrevTime { get { return prevTime; } }

	public void Initialize()
	{
		startTime = Time.time;
		prevTime = startTime;
		time = 0f;
	}
	
	public void Update()
	{
		deltaTime = Time.time - prevTime;
		time += deltaTime;
		//time += Time.deltaTime;
		//Debug.Log("TimeTest :" + Time.deltaTime + " : " + (Time.time - prevTime));
		//Debug.Log("sum time: " + time );
		prevTime = Time.time;
	}

}
