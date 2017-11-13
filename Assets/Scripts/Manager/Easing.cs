using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System;
//イージング関数
public class Easing
{
    static public IEnumerator Tween(float time, Action<float> call, Action callback = null)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            call(t / time);
            yield return null;
        }
        call(1.0f);
        if (callback != null)
            callback();
    }
    static public IEnumerator Deyray(float time,Action call)
    {
        yield return new WaitForSeconds(time);
        call();
    }

    static public float InQuad(float t)
    {
        return t * t;
    }
    static public float OutQuad(float t)
    {
        return -t * (t - 2);
    }
    static public float InOutQuad(float t)
    {
        t *= 2;
        if (t < 1) return 0.5f * t * t;

        t -= 1;
        return -0.5f * ((t) * (t - 2) - 1);
    }
    static public float InCubic(float t)
    {
        return t * t * t;
    }
    static public float OutCubic(float t)
    {
        t -= 1;
        return t * t * t + 1;
    }
    static public float InOutCubic(float t)
    {
        t *= 2;
        if (t < 1)
            return 0.5f * t * t * t;
        t -= 2;
        return 0.5f * (t * t * t + 2);
    }
    static public float InQuart(float t)
    {
        return t * t * t * t;
    }
    static public float OutQuart(float t)
    {
        t -= 1;
        return -(t * t * t * t - 1);
    }
    static public float InOutQuart(float t)
    {
        t *= 2;
        if (t < 1) return 0.5f * t * t * t * t;
        else {
            t -= 2;
            return -0.5f * (t * t * t * t - 2);
        }
    }
    static public float OutInQuart(float t)
    {
        if (t < 0.5f) return OutQuart(2 * t) / 2;
        return InQuart(2 * t - 1) / 2 + 0.5f;
    }

    static public float InQuint(float t)
    {
        return t * t * t * t * t;
    }
    static public float OutQuint(float t)
    {
        t -= 1;
        return t * t * t * t * t + 1;
    }
    static public float InOutQuint(float t)
    {
        t *= 2;
        if (t < 1) return 0.5f * t * t * t * t * t;
        else {
            t -= 2;
            return 0.5f * (t * t * t * t * t + 2);
        }
    }
    static public float OutInQuint(float t)
    {
        if (t < 0.5f) return OutQuint(2 * t) / 2;
        return InQuint(2 * t - 1) / 2 + 0.5f;
    }
    static public float InSine(float t)
    {
        return Mathf.Cos(t * Mathf.PI / 2) + 1;
    }
    static public float OutSine(float t)
    {

        return Mathf.Sin(t * Mathf.PI / 2);
    }

    static public float InOutSine(float t)
    {
        return -0.5f * (Mathf.Cos(Mathf.PI * t) - 1);
    }
    static public float OutInSine(float t)
    {
        if (t < 0.5f) return OutSine(2 * t) / 2;
        return InSine(2 * t - 1) / 2 + 0.5f;
    }

    static public float InExpo(float t)
    {

        return t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1));
    }
    static public float OutExpo(float t)
    {
        return t == 1 ? 1 : -Mathf.Pow(2, -10 * t) + 1;
    }
    static public float InOutExpo(float t)
    {
        if (t == 0) return 0;
        if (t == 1) return 1;
        t *= 2;
        if (t < 1) return 0.5f * Mathf.Pow(2, 10 * (t - 1));
        return 0.5f * (-Mathf.Pow(2, -10 * (t - 1)) + 2);
    }

    static public float OutInExpo(float t)
    {
        if (t < 0.5f) return OutExpo(2 * t) / 2;
        return InExpo(2 * t - 1) / 2 + 0.5f;
    }
    static public float InCirc(float t)
    {

        return -(Mathf.Sqrt(1 - t * t) - 1);
    }
    static public float OutCirc(float t)
    {
        t -= 1;
        return Mathf.Sqrt(1 - t * t);
    }

    static public float InOutCirc(float t)
    {
        t *= 2;
        if (t < 1)
        {
            return -0.5f * (Mathf.Sqrt(1 - t * t) - 1);
        }
        else {
            t -= 2;
            return 0.5f * (Mathf.Sqrt(1 - t * t) + 1);
        }
    }

    static public float OutInCirc(float t)
    {
        if (t < 0.5f) return OutCirc(2 * t) / 2;
        return InCirc(2 * t - 1) / 2 + 0.5f;
    }

    static public float OutBounceHelper_(float t, float c, float a)
    {
        if (t == 1) return c;
        if (t < (4 / 11.0f))
        {
            return c * (7.5625f * t * t);
        }
        else if (t < (8 / 11.0f))
        {
            t -= (6 / 11.0f);
            return -a * (1 - (7.5625f * t * t + 0.75f)) + c;
        }
        else if (t < (10 / 11.0f))
        {
            t -= (9 / 11.0f);
            return -a * (1 - (7.5625f * t * t + 0.9375f)) + c;
        }
        else {
            t -= (21 / 22.0f);
            return -a * (1 - (7.5625f * t * t + 0.984375f)) + c;
        }
    }
    static public float InBounce(float t, float a = 1.70158f)
    {
        return 1 - OutBounceHelper_(1 - t, 1, a);
    }

    static public float OutBounce(float t, float a = 1.70158f)
    {
        return OutBounceHelper_(t, 1, a);
    }

    static public float InOutBounce(float t, float a = 1.70158f)
    {
        if (t < 0.5f) return InBounce(2 * t, a) / 2;
        else return (t == 1) ? 1 : OutBounce(2 * t - 1, a) / 2 + 0.5f;
    }

    static public float OutInBounce(float t, float a = 1.70158f)
    {
        if (t < 0.5f) return OutBounceHelper_(t * 2, 0.5f, a);
        return 1 - OutBounceHelper_(2 - 2 * t, 0.5f, a);
    }
    static public float InBack(float t, float s = 1.70158f)
    {
        return t * t * ((s + 1) * t - s);
    }

    static public float OutBack(float t, float s = 1.70158f)
    {
        t -= 1;
        return (t * t * ((s + 1) * t + s) + 1);
    }
    static public float InOutBack(float t, float s = 1.70158f)
    {
        t *= 2;
        if (t < 1)
        {
            s *= 1.525f;
            return 0.5f * (t * t * ((s + 1) * t - s));
        }
        else {
            t -= 2;
            s *= 1.525f;
            return 0.5f * (t * t * ((s + 1) * t + s) + 2);
        }
    }
    static public float OutInBack(float t, float s)
    {
        if (t < 0.5f) return OutBack(2 * t, s) / 2;
        return InBack(2 * t - 1, s) / 2 + 0.5f;
    }

    static public float InAtan(float t, float a = 15)
    {

        float m = Mathf.Atan(a);
        return (Mathf.Atan((t - 1) * a) / m) + 1;
    }
    static public float OutAtan(float t, float a = 15)
    {
        float m = Mathf.Atan(a);
        return Mathf.Atan(t * a) / m;
    }

    static public float InOutAtan(float t, float a = 15)
    {
        float m = Mathf.Atan(0.5f * a);
        return (Mathf.Atan((t - 0.5f) * a) / (2 * m)) + 0.5f;
    }



}

















    