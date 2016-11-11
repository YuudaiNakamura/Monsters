using UnityEngine;
using System.Collections.Generic;

namespace Sentio.CreaturePack
{
    public class DamageMessageReceiver : MonoBehaviour
    {
        [SerializeField]
        private float _currentHealth = 100f;

        [SerializeField]
        private float _maxHealth = 100f;

        [SerializeField]
        private float _minHealth = 0f;

        [SerializeField]
        private bool _isDead = false;

        private float _currentDamage = 0f;

        public virtual float CurrentDamage
        {
            get { return _currentDamage; }
            set { _currentDamage = value; }
        }

        public virtual float CurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = value; }
        }

        public virtual float MinHealth
        {
            get { return _minHealth; }
            set { _minHealth = value; }
        }

        public virtual float MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        public virtual bool IsDead
        {
            get { return _isDead; }
            set { _isDead = value; }
        }

        public virtual void Start()
        {
            CurrentDamage = 0f;
        }

        public virtual void Damage(DamageMessage aDamageMessage)
        {
            CurrentDamage += aDamageMessage.damageAmount;
        }

        public virtual void Update()
        {
            CurrentHealth = Mathf.Min(MaxHealth, Mathf.Max(MinHealth, CurrentHealth - CurrentDamage));

            if ((CurrentHealth <= MinHealth) && (!IsDead))
                Die();

            CurrentDamage = 0f;
        }

        public virtual void Die()
        {
            IsDead = true;
        }
    }
}