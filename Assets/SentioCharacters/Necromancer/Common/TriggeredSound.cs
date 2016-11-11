using UnityEngine;
using System.Collections;

namespace Sentio.Necromancer
{
    public class TriggeredSound : MonoBehaviour
    {
        public void TriggerSound(string aSound)
        {
            BroadcastMessage("SoundEvent", aSound, SendMessageOptions.DontRequireReceiver);
        }
    }
}