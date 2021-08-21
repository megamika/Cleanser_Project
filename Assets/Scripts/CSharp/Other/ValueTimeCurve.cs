using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ValueTimeCurve
{
    public float value;
    public float time;
    public AnimationCurve curve;
}

public class ValueTimeCurveRoutine
{
    MonoBehaviour mono;
    ValueTimeCurve valueTimeCurve;
    public float value { get; private set; }
    public bool isPlaying { get; private set; }
    Coroutine coroutine;

    public void StartDeltaTime()
    {
        if (coroutine != null)
        {
            mono.StopCoroutine(coroutine);
        }
        coroutine = mono.StartCoroutine(IEroutineDeltaTime());
    }

    public void StartFixedTime()
    {
        if (coroutine != null)
        {
            mono.StopCoroutine(coroutine);
        }
        coroutine = mono.StartCoroutine(IEroutineFixedTime());
    }

    public void Cancel()
    {
        if (coroutine != null)
        {
            mono.StopCoroutine(coroutine);
        }
        isPlaying = false;
    }

    IEnumerator IEroutineDeltaTime()
    {
        yield return IEroutine(DeltaTimeWait());
    }

    IEnumerator IEroutineFixedTime()
    {
        yield return IEroutine(FixedTimeWait());
    }

    IEnumerator IEroutine(IEnumerator wait)
    {
        isPlaying = true;
        float timer = 0f;
        while (timer <= valueTimeCurve.time)
        {
            timer += Time.deltaTime;
            float percent = timer / valueTimeCurve.time;
            value = valueTimeCurve.curve.Evaluate(percent) * valueTimeCurve.value;
            yield return wait;
        }
        isPlaying = false;
    }


    IEnumerator DeltaTimeWait()
    {
        yield return null;
    }
    IEnumerator FixedTimeWait()
    {
        yield return new WaitForFixedUpdate();
    }

    public ValueTimeCurveRoutine(ValueTimeCurve valueTimeCurve, MonoBehaviour mono)
    {
        this.valueTimeCurve = valueTimeCurve;
        this.mono = mono;
    }
}
