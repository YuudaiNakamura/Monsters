using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sentio.Necromancer
{
    public class DampedFloat
    {
        private float blendTime = 0f;
        private float priorValue = 0f;
        private float targetValue = 0f;
        private float currentBlend = 1f;

        public DampedFloat() { }
        public DampedFloat(float aInitialValue)
        {
            ResetValue(aInitialValue);
        }

        public void ResetValue(float aInitialValue)
        {
            targetValue = aInitialValue;
            blendTime = 0f;
            currentBlend = 1f;
        }

        public float CurrentValue
        {
            get
            {
                if (blendTime <= 0f)
                    return targetValue;

                return (priorValue * (1f - currentBlend)) + (targetValue * currentBlend);
            }
        }

        public void Set(float aTargetValue, float aBlendTime)
        {
            priorValue = CurrentValue;
            targetValue = aTargetValue;
            blendTime = aBlendTime;
            currentBlend = 0f;
        }

        public float Update(float aDeltaTime)
        {
            if (blendTime <= 0f)
                currentBlend = 1f;
            else
                currentBlend = Mathf.Clamp01(currentBlend + (aDeltaTime / blendTime));

            return CurrentValue;
        }
    }
}