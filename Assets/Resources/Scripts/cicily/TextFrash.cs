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
            Qtext.color = new Color(textColor_red, textColor_green, textColor_blue, a_color);
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




    [SerializeField]
    public float textColor_red;
    [SerializeField]
    public float textColor_green;
    [SerializeField]
    public float textColor_blue;

    public void setColor(float _red, float _green,float _blue)
    {
        textColor_red = _red;
        textColor_green = _green;
        textColor_blue = _blue;
        Qtext.color = new Color(textColor_red, textColor_green, textColor_blue, a_color);
    }

    public void setAlpha(float _alpha)
    {
        a_color = _alpha;
        Qtext.color = new Color(textColor_red, textColor_green, textColor_blue, a_color);
    }

    public void setPos(Vector3 _pos)
    {
        Qtext.transform.localPosition = new Vector3(_pos.x,_pos.y,_pos.z);
    }

    public void setSize(Vector3 _size)
    {
        Qtext.transform.localScale = new Vector3(_size.x, _size.y, _size.z);
    }
}


