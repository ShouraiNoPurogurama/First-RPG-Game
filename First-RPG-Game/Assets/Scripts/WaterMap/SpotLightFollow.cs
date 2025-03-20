using UnityEngine;

namespace WaterMap
{
    public class SpotLightFollow : MonoBehaviour
    {
        public Transform player;
        public
            void Update()
        {
            Vector3 offset = new Vector3(0, 1, 0);
            if (player != null)
            {
                transform.position = player.position + offset;
            }
        }
    }
}
