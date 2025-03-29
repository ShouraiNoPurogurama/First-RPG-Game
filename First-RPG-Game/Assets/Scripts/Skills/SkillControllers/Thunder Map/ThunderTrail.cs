using UnityEngine;

namespace Skills.SkillControllers.Thunder_Map
{
    public class ThunderTrail : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer; // Gán TrailRenderer từ Inspector
        [SerializeField] private Color trailColor = new Color(0, 1, 1); // Màu xanh lam (cyan)

        void Awake()
        {
            if (trailRenderer == null)
            {
                trailRenderer = gameObject.AddComponent<TrailRenderer>();
            }

            // Cấu hình TrailRenderer
            trailRenderer.startWidth = 0.2f;
            trailRenderer.endWidth = 0f;
            trailRenderer.time = 0.5f;
            trailRenderer.startColor = trailColor;
            trailRenderer.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0);
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
    }
}