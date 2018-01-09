using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{

    [SerializeField]
    GameObject titleLogo;

    [SerializeField]
    GameObject startLogo;

    bool moveTitle;

    Timer timer;

    void Start()
    {
        timer = new Timer();
        timer.Initialize();
    }


    void Update()
    {
        if (moveTitle) Flash();
    } 

    public void Flash()
    {

        timer.Update();

        if(startLogo.activeInHierarchy == false
            && timer.time >= 0.5f )
        {
            startLogo.SetActive(true);
            timer.time = 0;
        }
        else if(startLogo.activeInHierarchy == true
            && timer.time >= 1.0f)
        {
            startLogo.SetActive(false);
            timer.time = 0;
        }

    }

    public void ShowTitle()
    {
        moveTitle = true;
        titleLogo.gameObject.SetActive(true);
        startLogo.gameObject.SetActive(true);
    }

    public void HideTitle()
    {
        moveTitle = false;
        titleLogo.gameObject.SetActive(false);
        startLogo.gameObject.SetActive(false);
    }

    


}
