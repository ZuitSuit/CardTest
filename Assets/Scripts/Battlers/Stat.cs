using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Stat 
{
    public UnityAction OnStatChanged;

    public float defaultValue, minValue, maxValue, regenRate;

    float currentValue;

    public float CurrentValue
    {
        get { return currentValue; }
        set { 
            currentValue = Mathf.Clamp(value, minValue, maxValue);
            OnStatChanged?.Invoke();
        }
    }

    public float StatPercentage()
    {
        return Mathf.InverseLerp(minValue, maxValue, currentValue);
    }

    public void Regen(float deltaTime)
    {
        CurrentValue += (regenRate * deltaTime);
    }

    public Stat (float _defaultValue, float _minValue, float _maxValue, float _regenRate)
    {
        defaultValue = _defaultValue;
        minValue = _minValue;
        maxValue = _maxValue;
        regenRate = _regenRate;

        currentValue = defaultValue;
    }
}
