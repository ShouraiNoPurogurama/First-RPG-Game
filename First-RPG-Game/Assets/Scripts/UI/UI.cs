using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private GameObject characterUI;
        [SerializeField] public UI_ToolTip toolTipUI;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            toolTipUI = GetComponentInChildren<UI_ToolTip>();
            SwitchTo(null);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                SwitchWithKeyTo(characterUI);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchWithKeyTo(null);
            }
        }
        public void SwitchWithKeyTo(GameObject menu)
        {
            if (menu != null && menu.activeSelf)
            {
                menu.SetActive(false);
                Time.timeScale = 1f;
                return;
            }
            SwitchTo(menu);
        }

        public void SwitchTo(GameObject menu)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            if (menu != null)
            {
                Time.timeScale = 0f;
                menu.SetActive(true);
            }
        }
    }
}