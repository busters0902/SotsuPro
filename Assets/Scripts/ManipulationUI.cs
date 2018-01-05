using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManipulationUI : MonoBehaviour
{
    
    [Range(0, 30)]
    public float triggerRotation;

    public bool showL;

    public bool showControllerSide;

    public bool onTriggerAnimetion;

    public bool onTouchPadPosition;

    [Range(0, 360)]
    public float padAngle;


    [SerializeField]
    Image lImage;

    [SerializeField]
    Image rImage;

    [SerializeField]
    GameObject controllerFront;

    [SerializeField]
    GameObject controllerSide;

    [SerializeField]
    GameObject triggerAxis;

    [SerializeField]
    GameObject padAxis;

    [SerializeField]
    Image touchPadPosition;

    void Update()
    {
        if (onTriggerAnimetion) AnimateTrigger();
    }


    void OnValidate()
    {
        var a = triggerAxis.transform.rotation.eulerAngles;
        a.z = -Mathf.Clamp(triggerRotation, 0, 30);
        triggerAxis.transform.rotation = Quaternion.Euler(a);

        var b = triggerAxis.transform.rotation.eulerAngles;
        b.z = -padAngle;
        padAxis.transform.rotation = Quaternion.Euler(b);


        if (showL) ShowL();
        else       ShowR();

        if (showControllerSide) ShowSide();
        else                    ShowFront();

        touchPadPosition.gameObject.SetActive(onTouchPadPosition);

    }

    void ShowL()
    {
        lImage.gameObject.SetActive(true);
        rImage.gameObject.SetActive(false);
    }

    void ShowR()
    {
        lImage.gameObject.SetActive(false);
        rImage.gameObject.SetActive(true);
    }

    void ShowSide()
    {
        controllerSide.SetActive(true);
        controllerFront.SetActive(false);
    }

    void ShowFront()
    {
        controllerSide.SetActive(false);
        controllerFront.SetActive(true);
    }

    void AnimateTrigger()
    {
        triggerRotation -= 0.6f;
        if (triggerRotation <= 0f) triggerRotation = 30f;

        var a = triggerAxis.transform.rotation.eulerAngles;
        a.z = -Mathf.Clamp(triggerRotation, 0, 30);
        triggerAxis.transform.rotation = Quaternion.Euler(a);
    }

}
