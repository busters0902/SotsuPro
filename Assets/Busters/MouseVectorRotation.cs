using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseVectorRotation : MonoBehaviour
{

    [SerializeField]
    Transform transform;


    //更新された
    bool isUpdated;
    public bool IsUpdated { get { return isUpdated; } }


    Vector3 mousePos;
    Vector3 prevMousePos;

    void Update()
    { 
        if(Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            prevMousePos = mousePos;
            Debug.Log(mousePos);
        }
        else if (Input.GetMouseButton(0))
        {
            prevMousePos = mousePos;
            mousePos = Input.mousePosition;
            if (prevMousePos != mousePos)
            {
                Debug.Log("Moved");
                var mov = mousePos - prevMousePos;
                RotatationAim(mov.x, mov.y);

            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            
        }

    }

    Quaternion RotatationAim(float angleX, float angleY)
    {
        return Quaternion.AngleAxis(angleX, Vector3.up) * Quaternion.AngleAxis(angleY, Vector3.right);
    }




}
