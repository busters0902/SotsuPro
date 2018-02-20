using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 利き手選択
/// </summary>
public class SelectHandController : MonoBehaviour
{

    [SerializeField]
    GameObject panel;

    [SerializeField]
    Transform left;

    [SerializeField]
    Transform right;

    [SerializeField]
    Transform scoop;

    [SerializeField]
    Transform dot;

    public bool isLeft;

    /// <summary>
    /// 右、左
    /// </summary>
    public string[] targetNames;

    public void Initialize()
    {
        SetRightMode();
    }

    public void Show()
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }

    public void SetRightMode()
    {
        left.transform.localScale  = Vector3.one;
        right.transform.localScale = Vector3.one;
        scoop.transform.localScale = Vector3.one;
        dot.transform.localScale   = Vector3.one;
        isLeft = false;
    }

    public void SetLeftMode()
    {
        left.transform.localScale  = new Vector3(-1, 1, 1);
        right.transform.localScale = new Vector3(-1, 1, 1);
        scoop.transform.localScale = new Vector3(-1, 1, 1);
        dot.transform.localScale   = new Vector3(-1, 1, 1);
        isLeft = true;
    }

    



}
