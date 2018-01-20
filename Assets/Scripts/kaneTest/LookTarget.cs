using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTarget : MonoBehaviour {

    [SerializeField]
    Transform target;
	void Update () {
        var targetVector = transform.position - target.transform.position;
        var cross = Vector3.Cross(targetVector,transform.forward);
        var anser = Vector3.Cross(cross, targetVector);
        var angle = Mathf.Atan2(anser.y, anser.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle + 90);
    }
}
