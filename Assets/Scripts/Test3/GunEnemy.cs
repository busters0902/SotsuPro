using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : MonoBehaviour
{

    [SerializeField]
    HandGun gun;

    [SerializeField]
    float power;

    [SerializeField]
    Timer timer;

    [SerializeField]
    float shotTime;

    private void Start()
    {
        timer = new Timer();
        timer.Initialize();
    }

    void Update ()
    {
        timer.Update();

        if (timer.time > shotTime)
        {
            gun.Shot(power);
            timer.Initialize();
        }

	}
}
