using Manager_Controller;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Save_and_Load
{
    public class SaveManager : MonoBehaviour
    {
        public GameData gameData;
        public static SaveManager instance;
        [SerializeField] private string fileName;
        [SerializeField] private bool encryptData;
        private List<ISaveManager> saveManagers;
        private FileDataHandler fileDataHandler;

        [ContextMenu("Delete Saved File")]
        public void DeleteSavedData()
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            fileDataHandler.Delete();
        }

        private void Awake()
        {
            instance = this;
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        }
        private void Start()
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            saveManagers = FindAllSaveManagers();
            LoadGame();
        }

        public void NewGame()
        {
            fileDataHandler.Delete();

            // Khởi tạo dữ liệu game mới
            gameData = new GameData();
            gameData.screenName = "Level2";
            gameData.gold = 0;
            gameData.strength = 10;
            gameData.agility = 10;
            gameData.intelligence = 10;
            gameData.vitality = 10;

            // Reset lại các danh sách và dictionary về trạng thái rỗng
            gameData.inventory = new SerializableDictionary<string, int>();
            gameData.equipmentId = new List<string>();
            gameData.closeCheckpointId = "";
            gameData.checkpoints = new SerializableDictionary<string, bool>();

            Debug.Log("New game data đã được reset.");
        }
        public async void LoadGame()
        {
            gameData = await fileDataHandler.Load();
            Debug.Log("Load game data " + gameData.closeCheckpointId);
            await APITrigger.Instance.LoadRubyDB();
            if (this.gameData == null)
            {
                Debug.Log("No save data found");
                NewGame();
            }
            foreach (ISaveManager saveManager in saveManagers)
            {
                saveManager.LoadData(gameData);
            }
        }

        public IEnumerator SaveGameCoroutine()
        {
            foreach (ISaveManager saveManager in saveManagers)
            {
                saveManager.SaveData(ref gameData);
            }

            yield return fileDataHandler.SaveCoroutine(gameData);
        }
        public void SaveGame()
        {
            if (GameManager.Instance.isDied)
            {
                foreach (ISaveManager saveManager in saveManagers)
                {
                    Debug.Log("Save data" + gameData.closeCheckpointId);
                    saveManager.SaveData(ref gameData);
                }
                fileDataHandler.Save(gameData);
            }
            else
            {
                foreach (ISaveManager saveManager in saveManagers)
                {
                    Debug.Log("Save data" + gameData.closeCheckpointId);
                    saveManager.SaveData(ref gameData);
                }
                fileDataHandler.Save(gameData);
            }
            //foreach (ISaveManager saveManager in saveManagers)
            //{
            //    saveManager.SaveData(ref gameData);
            //}
            //fileDataHandler.Save(gameData);
        }
        private void OnApplicationQuit()
        {
            SaveGame();
        }
        private List<ISaveManager> FindAllSaveManagers()
        {
            IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }

        public bool HasSavedData()
        {
            if (fileDataHandler.Load() != null)
            {
                return true;
            }
            return false;
        }
        public GameData GetGameData()
        {
            foreach (ISaveManager saveManager in saveManagers)
            {
                saveManager.SaveData(ref gameData);
            }
            return gameData;
        }

        public string GetSceneName()
        {
            //StartCoroutine(LoadGame());
            return gameData.screenName;
        }
    }
}

