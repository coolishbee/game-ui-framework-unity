﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TweenRotationStep : Tween<Vector3>
{
    public float step;
    private Vector3 mValue;
    public override Vector3 value
    {
        get
        {
            return mValue;
        }
        set
        {
            mValue = value;
        }
    }

    Transform mTransform;

    Transform cachedTransform
    {
        get
        {
            if (mTransform == null)
            {
                mTransform = transform;
            }
            return mTransform;
        }
    }

    Quaternion QuaternionValue
    {
        get
        {
            return cachedTransform.localRotation;
        }
        set
        {
            cachedTransform.localRotation = value;
        }
    }

    protected override void OnUpdate(float _factor, bool _isFinished)
    {
        int r = Mathf.FloorToInt(_factor / step);
        _factor = r * step;
        mValue = Vector3.Lerp(from, to, _factor);
        QuaternionValue = Quaternion.Euler(mValue);
    }

    public static TweenRotationStep Begin(GameObject go, Vector3 from, Vector3 to, float duration = 1f, float delay = 0f)
    {
        TweenRotationStep comp = Begin<TweenRotationStep>(go, duration);
        comp.value = from;
        comp.from = from;
        comp.to = to;
        comp.duration = duration;
        comp.delay = delay;
        if (duration <= 0)
        {
            comp.Sample(1, true);
            comp.enabled = false;
        }
        return comp;
    }
}