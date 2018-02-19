using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiserController : MonoBehaviour
{

    //特定の範囲の時レイざーの表示


    //[SerializeField]
    //GameObject liser;

    [SerializeField]
    LineRenderer line;

    [SerializeField]
    GameObject obj;

    [SerializeField]
    AngleLength angleLength;

    [SerializeField]
    string[] targetNames;

    System.Action<int> act;

    public bool onUsed;

    public bool onUseShower;

    public bool onUseLiser;

    void Update()
    {
        if(onUsed)
        {
            Update_();
        }
    }

    public void Update_()
    {

        ShowLiser();

        Liser();   

    }

    public void SetAction(System.Action<int> call_)
    {
        act = call_;
    }

    public void ShowLiser()
    {
        if (angleLength.InAngle(obj))
        {   
            line.enabled = true;
        }
        else
        {
            line.enabled = false;
        }
    }

    public void Liser()
    {
        if (ViveController.Instance.ViveRight)
        {
            var trans = ViveController.Instance.RightController.transform;
            
            Ray ray = new Ray(trans.position, trans.forward);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, line.endWidth - line.startWidth))
            {
                
                var name = hit.collider.gameObject.name;

                for(int i = 0; i < targetNames.Length; i++)
                {
                    if(name == targetNames[i])
                    {
                        if(act != null)
                            act(i);
                        break;
                    }
                }
                
            }
        }
    }

}
