using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

    [SerializeField]
    GameObject arrowPanel;

    bool useTutorial;

    bool isInvert;

    bool useArrowAnim = false;

    Vector3 basePos;
    Quaternion baseRot;

    [SerializeField]
    float movDist;
    [SerializeField]
    float movSpd;

    void Start ()
    {
        Initialize();
    }

	void Update ()
    {
        if (useArrowAnim) DrawArrowAnim();
	}

    public void Initialize()
    {
        arrowPanel.SetActive(false);
        basePos = arrowPanel.transform.position;
        baseRot = arrowPanel.transform.rotation;

        isInvert = false;
    }

    public void Reset_()
    {
        arrowPanel.transform.position = basePos;
        arrowPanel.transform.rotation = baseRot;
        isInvert = false;
    }

    public void ShowArrowAnime()
    {
        useArrowAnim = true;
        arrowPanel.SetActive(true);
    }

    public void HideArrowAnime()
    {
        useArrowAnim = false;
        arrowPanel.SetActive(false);
    }

    public void DrawArrowAnim()
    {
        if (isInvert == false) DrawArrowAnim_();
        else DrawArrowAnimInvert();

    }

    public void DrawArrowAnim_()
    {
        var trans = arrowPanel.transform;

        if(trans.position.x < basePos.x + movDist)
        {
            trans.position += trans.right * movSpd;
        }
        else
        {
            trans.position = basePos;
        }
    }

    public void DrawArrowAnimInvert()
    {
        var trans = arrowPanel.transform;

        if (trans.position.x > basePos.x - movDist)
        {
            trans.position -= trans.right * movSpd;
        }
        else
        {
            trans.position = basePos;
        }
    }

    public IEnumerator PlayTutorial()
    {
        yield return null;
    }

    public void Invert()
    {
        arrowPanel.transform.Rotate(Vector3.up, 180f);
        //  isInvert = true;
        isInvert = !isInvert;
    }

}
