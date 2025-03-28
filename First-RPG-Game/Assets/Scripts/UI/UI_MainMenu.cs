using Save_and_Load;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UI_MainMenu : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private GameObject continueButton;
        [SerializeField] UI_FadeScreen fadeScreen;
        private void Start()
        {
            if ((SaveManager.instance.HasSavedData() == false))
            {
                continueButton.SetActive(false);
            }
        }
        public void ContinueGame()
        {
            StartCoroutine(LoadSceneWithFadeEffect(1.5f));
        }
        public void NewGame()
        {
            SaveManager.instance.NewGame();
            //SaveManager.instance.DeleteSavedData();
            SaveManager.instance.SaveGame();
            SceneManager.LoadScene(sceneName);
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        IEnumerator LoadSceneWithFadeEffect(float _delay)
        {
            fadeScreen.FadeOut();
            yield return new WaitForSeconds(_delay);
            SaveManager.instance.LoadGame();
            yield return new WaitUntil(() =>
                 SaveManager.instance.gameData != null &&
            !string.IsNullOrEmpty(SaveManager.instance.gameData.screenName));
            string sceneToLoad = SaveManager.instance.gameData.screenName;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
