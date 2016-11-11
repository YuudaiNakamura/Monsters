using UnityEngine;
using System.Collections;

namespace Sentio.Necromancer
{
    public class TriggeredEffect : MonoBehaviour
    {
        public void TriggerEffect(string aEffect)
        {
            BroadcastMessage("EffectEvent", aEffect, SendMessageOptions.DontRequireReceiver);
        }
    }
}