using System;
using MainCharacter;
using UnityEngine;

namespace Skills
{
    public class Skill : MonoBehaviour
    {
        public float coolDown;
        private float _cooldownTimer;
        protected Player Player;

        private void Start()
        {
            Player = PlayerManager.Instance.player;
        }

        protected virtual void Update()
        {
            _cooldownTimer -= Time.deltaTime;
        }

        public virtual bool CanUseSkill()
        {
            if (_cooldownTimer <= 0)
            {
                UseSkill();
                _cooldownTimer = coolDown;
                return true;
            }
        
            Debug.Log("SKill is on cooldown");
            return false;
        }

        public virtual void UseSkill()
        {
            //Do some skill specific things
        }
    }
}
