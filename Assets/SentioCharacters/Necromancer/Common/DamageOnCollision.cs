using UnityEngine;
using System.Collections;
using Sentio.CreaturePack;

namespace Sentio.Necromancer
{
    public class DamageOnCollision : MonoBehaviour
    {
        public Transform rootParent;
        public float damageModifier = 1.0f;
        public bool canDamageSelf = false;
        public string weaponType = "primary";

        private float damageAmount;

        /// <summary>
        /// This can be set to true constantly, or it can be set to true only when you
        /// are in "attack" mode, as determined by an animation or some other trigger
        /// </summary>
        public bool canSendDamage = false;

        public float BaseDamageAmount
        {
            get { return damageAmount; }
            set { damageAmount = value; }
        }

        public float DamageAmount
        {
            get { return damageAmount * damageModifier; }
        }

        public void OnTriggerEnter(Collider aOtherCollider)
        {
            if (!canSendDamage)
                return;

            if (!canDamageSelf && (rootParent != null) && (aOtherCollider.transform.IsChildOf(rootParent)))
                return;

            GameObject tGiver = gameObject;
            if (rootParent != null)
                tGiver = rootParent.gameObject;

            DamageMessage tDamage = new DamageMessage() { damageGiver = tGiver, damageAmount = DamageAmount };
            aOtherCollider.gameObject.SendMessageUpwards("Damage", tDamage, SendMessageOptions.DontRequireReceiver);

            if ((gameObject.GetComponent<AudioSource>() != null) && !gameObject.GetComponent<AudioSource>().isPlaying)
                gameObject.GetComponent<AudioSource>().Play();
        }

        public void AllowWeaponDamage(string aWeaponType)
        {
            if (aWeaponType == weaponType)
            {
                canSendDamage = true;
            }
        }

        public void DisallowWeaponDamage(string aWeaponType)
        {
            if (aWeaponType == weaponType)
            {
                canSendDamage = false;
            }
        }
    }
}