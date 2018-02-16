using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimationController : MonoBehaviour
{
    
    Animator animation;

    bool stop = false;

    // Use this for initialization
    void Start()
    {
        animation = GetComponent<Animator>();
        //AudioManager.Instance.Load("Audio/SE/Voice");
    }

    public enum State
    {
        SET,
        NOKING,
        SETUP,
        FULLDROW,
        DROWING,
        SULLDROW,
        RELEASE,
        FOLLOW_THROUGH,
        DEFAULT,
        MAX
    }

    State state = State.SET;

    //アニメーションのステートを切り替える
    public void SetState(State _state)
    {
        state = _state;
        //Debug.Log(state.ToString().ToLower());
        animation.SetTrigger(state.ToString().ToLower());

    }



    public void SetFlow(bool is_flow)
    {

        //そのまま値入れてもよくね？
        if (is_flow)
        {
            animation.SetBool("flow", true);
            animation.SetTrigger("Loop 0");
        }
        else
        {
            animation.SetBool("flow", false);
        }
    }

    public void Stop()
    {
        stop = true;
    }

    //流しで再生します
    public IEnumerator waitAnimation()
    {
        animation.SetBool("Loop", false);

        if (stop) yield break;

        bool is_next = false;
        AudioManager.Instance.PlaySE("01", () => {
            AudioManager.Instance.PlaySE("02", () => {
            is_next = true;
        }); });

        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("03", () => {
            AudioManager.Instance.PlaySE("04", () => {
                is_next = true;
            });
        });
        SetState(State.SET);
        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("05", () => { is_next = true; });
        SetState(State.NOKING);

        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("06", () => { is_next = true; });
        SetState(State.SETUP);
        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }
        is_next = false;
        AudioManager.Instance.PlaySE("07", () => { is_next = true; });
        SetState(State.DROWING);
        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("08", () => { is_next = true; });
        SetState(State.FULLDROW);
        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("09", () => { is_next = true; });
        SetState(State.RELEASE);
        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }

        if (stop) yield break;

        //最後の挨拶
        is_next = false;
        SetState(State.DEFAULT);
        AudioManager.Instance.PlaySE("10", () => {
            AudioManager.Instance.PlaySE("11", () => {
                AudioManager.Instance.PlaySE("12", () => {
                    is_next = true;
                });
            });
        });

        while (!is_next)
        {
            yield return null;
            if (stop) yield break;
        }

        yield return null;
    }

    //Channelを切り替える関数
    void ChangeChannel(State _state)
    {
        animation.SetBool("Loop", true);
        SetState(_state);

        System.Action callback = () => {
            SetState(State.DEFAULT);
        };

        switch (_state) {
            case State.SET:
                AudioManager.Instance.PlaySE("03", () => {
                    AudioManager.Instance.PlaySE("04", callback);
                });
                break;
            case State.NOKING:
                AudioManager.Instance.PlaySE("05", callback);
                break;
            case State.SETUP:
                AudioManager.Instance.PlaySE("06", callback);
                break;
            case State.DROWING:
                AudioManager.Instance.PlaySE("07", callback);
                break;
            case State.FULLDROW:
                AudioManager.Instance.PlaySE("08", callback);
                break;
            case State.RELEASE:
                AudioManager.Instance.PlaySE("09", callback);
                break;

        }
    }

    //次のステートに切り替える
    public void nextAnim()
    {
        SetState((State)(((int)state + 1) % ((int)State.MAX)));
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(string.Format("{0} = = is ",animation.GetCurrentAnimatorStateInfo(0).normalizedTime));
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SetState(State.SET);
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    SetFlow(true);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ChangeChannel(State.SET);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    ChangeChannel(State.NOKING);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    ChangeChannel(State.SETUP);
        //}



        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    StartCoroutine(waitAnimation());

        //}
    }
}
