using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiserController : MonoBehaviour
{

    public LineRenderer line;

    [SerializeField]
    GameObject obj;

    public AngleLength angleLength;

    public string[] targetNames;

    System.Action<int> act;

    public bool onUsed;

    public bool onUseShow;

    public bool onUseLiser;

    [SerializeField]
    TutorialAnimationController tutorialAnimationController;
    
    [SerializeField]
    ViveLiser viveLiser;

    [SerializeField]
    Image rButton;
    [SerializeField]
    Image lButton;

    void Update()
    {
        if (onUsed)
        {
            Update_();
        }
    }

    public void Update_()
    {

        if (onUseShow) ShowLiser();

        if (onUseLiser) Liser();

    }

    public void Setup(string[] names, System.Action<int> call)
    {
        targetNames = names;
        act = call;
    }

    public void Setup(AngleLength length, string[] names, System.Action<int> call)
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

    public bool GetLineEnable()
    {
        return line.enabled;
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

        var trans = obj.transform;

        Ray ray = new Ray(trans.position, trans.forward);
        RaycastHit hit = new RaycastHit();

        viveLiser.length = 100;

        if (Physics.Raycast(ray, out hit, 15))
        {
            viveLiser.length = Vector3.Distance(hit.point, trans.position);
            if (hit.collider.gameObject.name == "migikiki")
            {
                rButton.color = new Color(238.0f/255.0f,255.0f / 255.0f, 161.0f / 255.0f, 1);
                lButton.color = new Color(1, 1, 1, 1);
            }
            else if(hit.collider.gameObject.name == "hidarikiki")
            {
                lButton.color = new Color(238.0f / 255.0f, 255.0f / 255.0f, 161.0f / 255.0f, 1);
                rButton.color = new Color(1, 1, 1, 1);
            }
            else
            {
                rButton.color = new Color(1, 1, 1, 1);
                lButton.color = new Color(1, 1, 1, 1);
            }
          
            if (ViveController.Instance.ViveRightDown)
            {
                Debug.Log("右トリガー");

                //if (Physics.Raycast(ray, out hit, line.endWidth - line.startWidth))

                var name = hit.collider.gameObject.name;
                Debug.Log(name);

                for (int i = 0; i < targetNames.Length; i++)
                {
                    if (name == targetNames[i])
                    {
                        if (act != null)
                            act(i);
                        break;
                    }
                }

            }
        }
    }

}
