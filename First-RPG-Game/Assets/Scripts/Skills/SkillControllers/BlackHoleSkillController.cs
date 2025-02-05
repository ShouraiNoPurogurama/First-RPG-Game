using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Skills.SkillControllers
{
    public class BlackHoleSkillController : MonoBehaviour
    {
        public float maxSize;
        public float growSpeed;
        public bool canGrow;

        public List<Transform> targets;

        private void Update()
        {
            if (canGrow)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy is not null)
            {
                enemy.FreezeTime(true);
                // targets.Add(collision.transform);
                //Respawn prefab of a hot key above enemy
                
                
            }
        }
    }
}
