using UnityEngine;
using System.Collections;

namespace Sentio.Necromancer
{
    public class DamageGiver : MonoBehaviour
    {
        public void AllowDamage(string aWeaponType)
        {
            BroadcastMessage("AllowWeaponDamage", aWeaponType, SendMessageOptions.DontRequireReceiver);
        }

        public void DisallowDamage(string aWeaponType)
        {
            BroadcastMessage("DisallowWeaponDamage", aWeaponType, SendMessageOptions.DontRequireReceiver);
        }
    }
}