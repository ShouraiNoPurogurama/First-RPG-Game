using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class UIButtonSound : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            Button btn = GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => SoundManager.PlayClick());
            }
        }
    }
}


