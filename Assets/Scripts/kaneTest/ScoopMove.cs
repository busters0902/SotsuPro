using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopMove : MonoBehaviour
{
    [SerializeField]
    GameObject scoop;
    Vector3 defortPosition;
    void Start()
    {
        defortPosition = transform.localPosition;
        scoop.transform.localScale = new Vector3(25, scoop.transform.localScale.y,
              scoop.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localPosition += Vector3.forward * 0.5f;

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 
                Mathf.Min( transform.localPosition.z,28));
            scoop.transform.localScale += Vector3.right;
            scoop.transform.localScale = new Vector3(Mathf.Min(scoop.transform.localScale.x, 104), scoop.transform.localScale.y,
              scoop.transform.localScale.z);

        }
        if (Input.GetMouseButtonUp(0))
        {
            transform.localPosition = defortPosition;
            scoop.transform.localScale = new Vector3(25, scoop.transform.localScale.y, scoop.transform.localScale.z);


        }
    }
}
