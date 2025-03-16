using System.Collections.Generic;
using UnityEngine;

namespace Spawn
{
    [System.Serializable]
    public class SpawnPointData
    {
        public Transform spawnPoint;
        public List<EnemyData> enemyList;
    }
}

