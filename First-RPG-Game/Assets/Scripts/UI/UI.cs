using System.Collections;
using UnityEngine;

namespace UI
{
    public class UI : MonoBehaviour
    {
        [Header("End Screen")]
        [SerializeField] private UI_FadeScreen fadeScreen;
        [SerializeField] private GameObject endText;
        [SerializeField] private GameObject restartButton;
        [Space]

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
            if (Input.GetKeyDown(KeyCode.P))
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
                bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
                if (!fadeScreen)
                    transform.GetChild(i).gameObject.SetActive(false);
            }
            if (menu != null)
            {
                Time.timeScale = 0f;
                menu.SetActive(true);
            }
        }
        /// <summary>
        /// Switch on end screen
        /// </summary>
        public void SwitchOnEndScreen()
        {
            fadeScreen.FadeOut();
            StartCoroutine(EndScreenCorutione());
        }
        IEnumerator EndScreenCorutione()
        {
            yield return new WaitForSeconds(1);
            endText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            restartButton.SetActive(true);
            Time.timeScale = 1f;
        }
        public void RestartGameButton() => SceneController.instance.RestartScene();
        public void ExitGame() => Application.Quit();
    }
}