using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimationController : MonoBehaviour
{

    Animator animation;

    // Use this for initialization
    void Start()
    {
        animation = GetComponent<Animator>();
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

        if (is_flow)
        {
            //for (int i = 0; i < (int)State.MAX; i++)
            //{
            //    SetState((State)i);
            //}
            animation.SetBool("flow", true);
        }
        else
        {
            animation.SetBool("flow", false);
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
        if (Input.GetMouseButtonDown(0))
        {
            SetState(State.SET);
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetFlow(true);
        }


    }
}
