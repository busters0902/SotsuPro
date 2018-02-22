using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectHandController : MonoBehaviour {
    [SerializeField]
    GameObject push_;
    [SerializeField]
    GameObject pull_;
	// Use this for initialization
	void Start () {
        push_.SetActive(true);
        pull_.SetActive(false);
        StartCoroutine(controllerAnim());
    }
	
	// Update is called once per frame
	void Update () {
		
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
