using System;
using UnityEngine;

namespace Sentio.CreaturePack
{
    public class DamageMessage
    {
        public Vector3 damagePoint; //position where the damage was delivered
        public GameObject damageObject; //the object delivering the damage
        public GameObject damageGiver; //the object (character, etc.) responsible for the damage
        public float damageAmount;
    }
}
