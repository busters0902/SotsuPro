using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimationController : MonoBehaviour
{

    Animator animation;

    bool stop = false;

    public string[] targetNames;

    void Start()
    {
        animation = GetComponent<Animator>();

        //AudioManager.Instance.Load("Audio/SE/Voice");
        //StartCoroutine(allPlayAnimation());
    }

    public enum State
    {
        SET,            //0
        NOKING,         //1
        SETUP,          //2
        FULLDROW,       //3
        DROWING,        //4
        RELEASE,        //5
        FOLLOW_THROUGH,
        DEFAULT,
        MAX
    }

    State state = State.DEFAULT;

    //アニメーションのステートを切り替える
    public void SetState(State _state)
    {
        state = _state;
        //Debug.Log(state.ToString().ToLower());


        animation.SetTrigger(state.ToString().ToLower());

    }


    //全体のモーションを流します
    //ループはしません
    //SEを流しません
    public void SetFlow(bool is_flow)
    {

        //そのまま値入れてもよくね？
        if (is_flow)
        {
            animation.SetBool("flow", true);
            animation.SetTrigger("loop 0");
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
    //一個の動きをループして
    //ナレーションが終わったら次のモーションに移動します
    public IEnumerator waitAnimation(bool is_loop)
    {
        animation.SetBool("loop", is_loop);
        if (stop) yield break;
        animationStop();
        yield return new WaitForSeconds(0.1f);
        stop = false;

        bool is_next = false;
        //AudioManager.Instance.PlaySE("01", () =>
        //{
        //    AudioManager.Instance.PlaySE("02", () =>
        //    {
        //        is_next = true;
        //    });
        //});

        //yield return new WaitUntil(() =>
        //{
        //    if (stop)
        //    {

        //        return true;
        //    }
        //    return is_next;
        //});

        //if (stop) yield break;

        is_next = false;
        StartCoroutine(Utility.TimerCrou(SoundWaitTime[0], () =>
        {
            AudioManager.Instance.PlaySE("03", () =>
            {
                StartCoroutine(Utility.TimerCrou(SoundWaitTime[1], () =>
                {
                    AudioManager.Instance.PlaySE("04", () =>
                    {
                        is_next = true;
                    });
                }));
            });
        }));
        StartCoroutine(Utility.TimerCrou(waitTime[0], () =>
       {
           SetState(State.SET);
       }));
        yield return new WaitUntil(() =>
        {
            if (stop)
            {
                AudioManager.Instance.StopSE("03");
                AudioManager.Instance.StopSE("04");
                return true;
            }
            return is_next;
        });
        if (stop) yield break;

        is_next = false;
        StartCoroutine(Utility.TimerCrou(SoundWaitTime[2], () =>
        {
            AudioManager.Instance.PlaySE("05", () => { is_next = true; });
        }));
        StartCoroutine(Utility.TimerCrou(waitTime[1], () =>
        {
            SetState(State.NOKING);
        }));
        yield return new WaitUntil(() =>
    {
        if (stop)
        {
            AudioManager.Instance.StopSE("05");
            return true;
        }
        return is_next;
    });
        if (stop) yield break;

        is_next = false;
        StartCoroutine(Utility.TimerCrou(SoundWaitTime[3], () =>
        {
            AudioManager.Instance.PlaySE("06", () => { is_next = true; });
        }));
        StartCoroutine(Utility.TimerCrou(waitTime[2], () =>
        {
            SetState(State.SETUP);
        }));
        yield return new WaitUntil(() =>
        {
            if (stop)
            {
                AudioManager.Instance.StopSE("06");
                return true;
            }
            return is_next;
        });
        is_next = false;
        StartCoroutine(Utility.TimerCrou(SoundWaitTime[4], () =>
        {
            AudioManager.Instance.PlaySE("07", () => { is_next = true; });
        }));
        StartCoroutine(Utility.TimerCrou(waitTime[3], () =>
        {
            SetState(State.DROWING);
        }));
        yield return new WaitUntil(() =>
        {
            if (stop)
            {
                AudioManager.Instance.StopSE("07");
                return true;
            }
            return is_next;
        });
        if (stop) yield break;

        is_next = false;
        StartCoroutine(Utility.TimerCrou(SoundWaitTime[5], () =>
        {
            AudioManager.Instance.PlaySE("08", () => { is_next = true; });
        }));
        StartCoroutine(Utility.TimerCrou(waitTime[4], () =>
        {
            SetState(State.FULLDROW);
        }));
        yield return new WaitUntil(() =>
        {
            if (stop)
            {
                AudioManager.Instance.StopSE("08");
                return true;
            }
            return is_next;
        });
        if (stop) yield break;

        is_next = false;
        StartCoroutine(Utility.TimerCrou(SoundWaitTime[6], () =>
        {
            AudioManager.Instance.PlaySE("09", () => { is_next = true; });
        }));
        StartCoroutine(Utility.TimerCrou(waitTime[5], () =>
         {
             SetState(State.RELEASE);
         }));
        yield return new WaitUntil(() =>
        {
            if (stop)
            {
                AudioManager.Instance.StopSE("09");
                return true;
            }
            return is_next;
        });
        if (stop) yield break;

        //最後の挨拶
        is_next = false;
        SetState(State.DEFAULT); StartCoroutine(Utility.TimerCrou(SoundWaitTime[7], () =>
        {
            AudioManager.Instance.PlaySE("10", () =>
        {
            StartCoroutine(Utility.TimerCrou(SoundWaitTime[8], () =>
            {
                AudioManager.Instance.PlaySE("11", () =>
            {
                StartCoroutine(Utility.TimerCrou(SoundWaitTime[9], () =>
                {
                    AudioManager.Instance.PlaySE("12", () =>
                {
                    is_next = true;
                });
                }));
            });
            }));
        });
        }));

        yield return new WaitUntil(() =>
        {
            if (stop)
            {
                AudioManager.Instance.StopSE("10");
                AudioManager.Instance.StopSE("11");
                AudioManager.Instance.StopSE("12");
                return true;
            }
            return is_next;
        });
        yield return null;
    }

    //流しで再生します
    //一個の動きをループしません
    //ナレーションが終わったら次のモーションに移動します
    //(使用していません)
    public IEnumerator waitAnimation_Second()
    {
        animation.SetBool("loop", false);

        if (stop) yield break;
        bool is_next = false;
        AudioManager.Instance.PlaySE("01", () =>
        {
            AudioManager.Instance.PlaySE("02", () =>
            {
                is_next = true;
            });
        });

        yield return new WaitUntil(() => is_next);

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("03", () =>
        {
            AudioManager.Instance.PlaySE("04", () =>
            {
                is_next = true;
            });
        });
        SetState(State.SET);
        yield return new WaitUntil(() => is_next);

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("05", () => { is_next = true; });
        SetState(State.NOKING);

        yield return new WaitUntil(() => is_next);
        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("06", () => { is_next = true; });
        SetState(State.SETUP);
        yield return new WaitUntil(() => is_next);

        is_next = false;
        AudioManager.Instance.PlaySE("07", () => { is_next = true; });
        SetState(State.DROWING);
        yield return new WaitUntil(() => is_next);

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("08", () => { is_next = true; });
        SetState(State.FULLDROW);
        yield return new WaitUntil(() => is_next);

        if (stop) yield break;

        is_next = false;
        AudioManager.Instance.PlaySE("09", () => { is_next = true; });
        SetState(State.RELEASE);
        yield return new WaitUntil(() => is_next);

        if (stop) yield break;

        //最後の挨拶
        is_next = false;
        SetState(State.DEFAULT);
        AudioManager.Instance.PlaySE("10", () =>
        {
            AudioManager.Instance.PlaySE("11", () =>
            {
                AudioManager.Instance.PlaySE("12", () =>
                {
                    is_next = true;
                });
            });
        });

        yield return new WaitUntil(() => is_next);

        yield return null;
    }
    public void ChangeChannel(int _state)
    {
        ChangeChannel((State)_state);
    }

    //アニメーションを一つだけ流します
    //ループしないで
    //ナレーションが終わったら止まります
    public void ChangeChannel(State _state, bool is_loop = false)
    {
        animation.SetBool("loop", is_loop);
        animationStop();
        StartCoroutine(Utility.TimerCrou(0.2f, () =>
        {
            //SetState(_state);
            System.Action callback = () =>
            {
                //stop = false;
                SetState(State.DEFAULT);
                animationStop();
            };

            switch (_state)
            {
                case State.SET:
                    AudioManager.Instance.PlaySE("03", () =>
                    {
                        StartCoroutine(Utility.TimerCrou(SoundWaitTime[1], () =>
                        {
                            AudioManager.Instance.PlaySE("04");
                        }));
                    });
                    StartCoroutine(Utility.TimerCrou(waitTime[0], () =>
                    {
                        SetState(State.SET);
                    }));

                    //StartCoroutine(checkStop(()=> {
                    //    AudioManager.Instance.StopSE("03");
                    //    AudioManager.Instance.StopSE("04");
                    //    callback();
                    //}));
                    break;
                case State.NOKING:

                    AudioManager.Instance.PlaySE("05");
                    StartCoroutine(Utility.TimerCrou(waitTime[1], () =>
                    {
                        SetState(State.NOKING);
                    }));

                    //AudioManager.Instance.PlaySE("05", callback);
                    //StartCoroutine(checkStop(() => {
                    //    AudioManager.Instance.StopSE("05");
                    //    callback();
                    //}));
                    break;
                case State.SETUP:

                    AudioManager.Instance.PlaySE("06");
                    StartCoroutine(Utility.TimerCrou(waitTime[2], () =>
                    {
                        SetState(State.SETUP);
                    }));
                    //AudioManager.Instance.PlaySE("06", callback);
                    //StartCoroutine(checkStop(() => {
                    //    AudioManager.Instance.StopSE("06");
                    //    callback();
                    //}));
                    break;
                case State.DROWING:

                    AudioManager.Instance.PlaySE("07");
                    StartCoroutine(Utility.TimerCrou(waitTime[3], () =>
                    {
                        SetState(State.DROWING);
                    }));
                    //AudioManager.Instance.PlaySE("07", callback);
                    //StartCoroutine(checkStop(() => {
                    //    AudioManager.Instance.StopSE("07");
                    //    callback();
                    //}));
                    break;
                case State.FULLDROW:

                    AudioManager.Instance.PlaySE("08");
                    StartCoroutine(Utility.TimerCrou(waitTime[4], () =>
                    {
                        SetState(State.FULLDROW);
                    }));
                    //AudioManager.Instance.PlaySE("08", callback);
                    //StartCoroutine(checkStop(() => {
                    //    AudioManager.Instance.StopSE("08");
                    //    callback();
                    //}));
                    break;
                case State.RELEASE:

                    AudioManager.Instance.PlaySE("09");
                    StartCoroutine(Utility.TimerCrou(waitTime[5], () =>
                    {
                        SetState(State.RELEASE);
                    }));
                    //AudioManager.Instance.PlaySE("09", callback);
                    //StartCoroutine(checkStop(() => {
                    //    AudioManager.Instance.StopSE("09");
                    //    callback();
                    //}));
                    break;
            }
        }));
    }

    //animationを停止する関数
    public void animationStop()
    {

        //AudioManager.Instance.StopSE("01");
        //AudioManager.Instance.StopSE("02");
        AudioManager.Instance.StopSE("03");
        AudioManager.Instance.StopSE("04");
        AudioManager.Instance.StopSE("05");
        AudioManager.Instance.StopSE("06");
        AudioManager.Instance.StopSE("07");
        AudioManager.Instance.StopSE("08");
        AudioManager.Instance.StopSE("09");
        stop = true;
        StartCoroutine(checkStop());
    }

    IEnumerator checkStop(System.Action callback = null)
    {
        yield return new WaitUntil(() => stop);
        yield return null;
        //AudioManager.Instance.StopSE("02");
        AudioManager.Instance.StopSE("03");
        AudioManager.Instance.StopSE("04");
        AudioManager.Instance.StopSE("05");
        AudioManager.Instance.StopSE("06");
        AudioManager.Instance.StopSE("07");
        AudioManager.Instance.StopSE("08");
        AudioManager.Instance.StopSE("09");
        SetState(State.DEFAULT);
        stop = false;
        if (callback != null)
        {
            callback();
        }
    }


    [SerializeField]
    float[] waitTime;
    [SerializeField]
    float[] SoundWaitTime;//10個あります
                          //03 <= 0
                          //04 <= 1
                          //05 <= 2
                          //06 <= 3
                          //07 <= 4
                          //08 <= 5
                          //09 <= 6
                          //10 <= 7
                          //11 <= 8
                          //12 <= 9


    //次のステートに切り替える（使用する予定はありません）
    public void nextAnim()
    {
        SetState((State)(((int)state + 1) % ((int)State.MAX)));
    }


    // Update is called once per frame
    void Update()
    {
        return;
        //Debug.Log(string.Format("{0} = = is ",animation.GetCurrentAnimatorStateInfo(0).normalizedTime));
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SetState(State.SET);
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    SetFlow(true);
        //}

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            //ChangeChannel(State.SET);
            StartCoroutine(waitAnimation_Second());
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(waitAnimation(false));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //ChangeChannel(State.NOKING);
            SetFlow(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeChannel(State.SETUP);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            animationStop();

        }



        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    StartCoroutine(waitAnimation());

        //}
    }

    public IEnumerator allPlayAnimation()
    {

        bool isNext = false;

        //SetState(State.DEFAULT);

        //AudioManager.Instance.PlaySE("01", () => isNext = true);
        //StartCoroutine(Utility.TimerCrou(waitTime[0],()=>SetState(State.SET)));
        //yield return new WaitUntil(() => isNext);

        //isNext = false;

        //AudioManager.Instance.PlaySE("02", () => isNext = true);
        //yield return new WaitUntil(() => isNext);

        //isNext = false;

        //nextAnim();

        AudioManager.Instance.PlaySE("03", () => isNext = true);
        StartCoroutine(Utility.TimerCrou(3.0f, () => SetState(State.SET)));
        yield return new WaitForSeconds(3.0f);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("04", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("05", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("06", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("07", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("08", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("09", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("10", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;


        AudioManager.Instance.PlaySE("11", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;

        AudioManager.Instance.PlaySE("12", () => isNext = true);
        yield return new WaitUntil(() => isNext);

        isNext = false;


        yield return null;
    }


}
