using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{

    Vector2 prevPos = Vector2.zero;
    public float shakeValue;
    public float scopSize = 1;
    [SerializeField]
    RectTransform Scoop;


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            scopSize = 0;


        Vector2 mp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var vec = mp - prevPos;
        shakeValue = vec.sqrMagnitude;
        scopSize += shakeValue / 40f - 0.1f;

        scopSize = Mathf.Min(Mathf.Max(scopSize,1f),6f);
        Scoop.localScale = new Vector3(scopSize, scopSize, 1);

        Debug.Log(shakeValue);

        prevPos = mp;
    }
}
