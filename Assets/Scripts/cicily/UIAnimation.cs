using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour {

   
    public Image animImage;
    float a_color;
    // Use this for initialization
    bool flag_G;
    public UIState uiState;

    public enum UIState
    {
        NONE,
        FRASH,
        MOVE,
        ROTATE,
        SCALE
    }

    public AnimState animState;

    public enum AnimState
    {
        PLAY,
        LOOP,
        PAUSE,
    } 

    public Vector3 pos;

    public float time;

    public iTween.LoopType loopType;

    // Use this for initialization
    void Start()
    {
        a_color = 0;
        textColor_red = 1;
        textColor_green = 1;
        textColor_blue = 1;
        StartCoroutine(uiAnimation());
        StartCoroutine(chackAnimState());

    }
    // Update is called once per frame
    void Update()
    {
       
              

        //if (uiState == UIState.FRASH)
        //{
        //    //テキストの透明度を変更する
        //    animImage.color = new Color(textColor_red, textColor_green, textColor_blue, a_color);
        //    if (flag_G)
        //        a_color -= Time.deltaTime;
        //    else
        //        a_color += Time.deltaTime;
        //    if (a_color < 0)
        //    {
        //        a_color = 0;
        //        flag_G = false;
        //    }
        //    else if (a_color > 1)
        //    {
        //        a_color = 1;
        //        flag_G = true;
        //    }
        //}

    }



    [SerializeField]
    public float textColor_red;
    [SerializeField]
    public float textColor_green;
    [SerializeField]
    public float textColor_blue;

    public void setColor(float _red, float _green, float _blue)
    {
        textColor_red = _red;
        textColor_green = _green;
        textColor_blue = _blue;
        animImage.color = new Color(textColor_red, textColor_green, textColor_blue, a_color);
    }

    public void setAlpha(float _alpha)
    {
        a_color = _alpha;
        animImage.color = new Color(textColor_red, textColor_green, textColor_blue, a_color);
    }

    public void setPos(Vector3 _pos)
    {
        animImage.transform.localPosition = new Vector3(_pos.x, _pos.y, _pos.z);
    }

    public void setSize(Vector3 _size)
    {
        animImage.transform.localScale = new Vector3(_size.x, _size.y, _size.z);
    }

    public void setUIAnimation(UIState _uiState,Vector3 _pos,float _time, iTween.LoopType _loopType)
    {
        pos = _pos;

        time = _time;

        loopType = _loopType;

        switch (_uiState)
        {
            case UIState.NONE:
                uiState = UIState.NONE;
            break;
            case UIState.FRASH:
                uiState = UIState.FRASH;
                break;
            case UIState.MOVE:
                uiState = UIState.MOVE;
                break;
            case UIState.ROTATE:
                uiState = UIState.ROTATE;
                break;
            case UIState.SCALE:
                uiState = UIState.SCALE;
                break;
        }
    }

    public IEnumerator uiAnimation()
    {
        while (true)
        {
            if (uiState == UIState.NONE)
            {
                iTween.Stop(this.gameObject);
                Debug.Log("NONE");
                yield return null;
            }
            if (uiState == UIState.MOVE)
            {
                iTween.Stop(this.gameObject, "rotate");
                iTween.MoveTo(this.gameObject, iTween.Hash("x", pos.x, "y", pos.y, "z", pos.z, "time", time, "looptype", loopType));
                yield return null;
            }
            if (uiState == UIState.FRASH)
            {
                //テキストの透明度を変更する
                    animImage.color = new Color(textColor_red, textColor_green, textColor_blue, a_color);
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
                yield return null;
            }
            if (uiState == UIState.ROTATE)
            {
                iTween.Stop(this.gameObject, "move");
                iTween.RotateTo(this.gameObject, iTween.Hash("x", pos.x, "y", pos.y, "z", pos.z, "time", time, "looptype", loopType));
                yield return null;
            }
            if (uiState == UIState.SCALE)
            {
                // iTween.ScaleTo(this.gameObject, iTween.Hash("x", _pos.x, "y", _pos.y, "z", _pos.z, "time", _time, _isLoop));
                yield return null;
            }
            yield return null;
        }
        
    
    }


    IEnumerator chackAnimState()
    {
        while (true) {
            if (animState == AnimState.PLAY)
            {
                iTween.Resume(this.gameObject);
                yield return null;
            }
            if (animState == AnimState.PAUSE)
            {
                iTween.Pause(this.gameObject);
                yield return null;
            }
            yield return null;
        }
    }


    }


    //IEnumerator chackCoroutineState(CorState _corState)
    //{
    //    while (true)
    //    {
    //        if (_corState == CorState.PLAY)
    //        {
    //            StartCoroutine(uiAnimation(uiState, pos, time, iTween.LoopType.none));
    //            yield break;
    //        }
    //        if (_corState == CorState.PLAY)
    //        {
    //            StartCoroutine(uiAnimation(uiState, pos, time, iTween.LoopType.loop));
    //            yield break;
    //        }
    //        if (_corState == CorState.PAUSE)
    //        {
    //            StopCoroutine(uiAnimation(uiState, pos, time, iTween.LoopType.none));
    //            yield break;
    //        }
    //    }





