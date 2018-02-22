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

    [SerializeField]
    TutorialAnimationController tutorialAnimationController;
    [SerializeField]
    ViveLiser viveLiser;

    void Update()
    {
        if (onUsed)
        {
            Update_();
        }
    }

    private void Start()
    {

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
