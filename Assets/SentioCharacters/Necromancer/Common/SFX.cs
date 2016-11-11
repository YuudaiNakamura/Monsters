using UnityEngine;
using System.Collections;

namespace Sentio.Necromancer
{
    public class SFX : MonoBehaviour
    {
        public string soundName;
        public Animator animator = null;
        public string parameterName = null;
        public float parameterThreshold = 0f;

        public void SoundEvent(string aSound)
        {
            if (aSound == soundName)
            {
                try
                {
                    if ((animator == null) || string.IsNullOrEmpty(parameterName) || animator.GetFloat(parameterName) >= parameterThreshold)
                    {
                        if (gameObject.GetComponent<AudioSource>().isPlaying)
                        {
                            gameObject.GetComponent<AudioSource>().Stop();
                            gameObject.GetComponent<AudioSource>().Play();
                        }
                        else
                            gameObject.GetComponent<AudioSource>().Play();
                    }
                }
                catch { }
            }
        }
    }
}