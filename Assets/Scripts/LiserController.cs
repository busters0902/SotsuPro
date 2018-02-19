using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiserController : MonoBehaviour
{

    //特定の範囲の時レイざーの表示


    //[SerializeField]
    //GameObject liser;

    public LineRenderer line;

    [SerializeField]
    GameObject obj;

    public AngleLength angleLength;

    public string[] targetNames;

    System.Action<int> act;

    public bool onUsed;

    public bool onUseShow;

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

        if(onUseShow) ShowLiser();

        if (onUseLiser) Liser();   

    }

    public void Setup(string[] names, System.Action<int> call)
    {
        targetNames = names;
        act = call;
    }

    public void Setup( AngleLength length, string[] names, System.Action<int> call)
    {
        angleLength = length;
        targetNames = names;
        act = call;
    }

    public void SetAction(System.Action<int> call)
    {
        act = call;
    }
    
    //消すときにラインは描画するかどうか
    public void NotUse(bool f)
    {
        onUsed = false;
        line.enabled = f;
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
            var trans = obj.transform;
            
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
