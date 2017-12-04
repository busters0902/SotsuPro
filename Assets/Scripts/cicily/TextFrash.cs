using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFrash : MonoBehaviour {


    public Text Qtext;
    float a_color;
    bool flag_G;
    public bool useFrash;
    // Use this for initialization
    void Start()
    {
        a_color = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (useFrash == true) {
            //テキストの透明度を変更する
            Qtext.color = new Color(1, 1, 1, a_color);
            if (flag_G)
                a_color -= Time.deltaTime;
            else
                a_color += Time.deltaTime;
            if (a_color < 0)
            {
                a_color = 0;
                flag_G = false;
            }
            else if (a_color > 1)
            {
                a_color = 1;
                flag_G = true;
            }
        }
    }

}
