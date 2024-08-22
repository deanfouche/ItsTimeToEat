using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class StatusMeter
    {
        public float MinVal { get; private set; } = 0f;

        public float MaxVal { get; private set; } = 100f;

        public float CurrentVal { get; private set; } = 100f;

        public float StatusPercentage
        {
            get
            {
                float valAdjusted = CurrentVal - MinVal;
                float maxAdjusted = MaxVal - MinVal;
                return valAdjusted / maxAdjusted * 100;
            }
        }

        public StatusMeter(float min= 0f, float max = 100f, float initialValue = 100f)
        {
            MinVal = min;
            MaxVal = max;
            CurrentVal = initialValue;

            Mathf.Clamp(CurrentVal, MinVal, MaxVal);
        }

        public void IncreaseStatusMeter(float valChange)
        {
            CurrentVal += valChange;
            Mathf.Clamp(CurrentVal, MinVal, MaxVal);
        }

        public void DecreaseStatusMeter(float valChange)
        {
            CurrentVal -= valChange;
            Mathf.Clamp(CurrentVal, MinVal, MaxVal);
        }
    }
}