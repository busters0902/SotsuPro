using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSelectAnimation : MonoBehaviour {

    [SerializeField]
    GameObject push_;
    [SerializeField]
    GameObject pull_;
    // Use this for initialization
    void Awake()
    {
        push_.SetActive(true);
        pull_.SetActive(false);
        StartCoroutine(controllerAnim());
    }

    public void animationStart()
    {
        push_.SetActive(true);
        pull_.SetActive(false);
        StartCoroutine(controllerAnim());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator controllerAnim()
    {
        while (true)
        {
            pull_.SetActive(true);
            push_.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            pull_.SetActive(false);
            push_.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            pull_.SetActive(true);
            push_.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            pull_.SetActive(false);
            push_.SetActive(true);

        }
    }
}
