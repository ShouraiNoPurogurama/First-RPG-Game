using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private GameData gameData;
    public static SaveManager instance;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;

    [ContextMenu("Delete Saved File")]
    private void DeteSavedData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        fileDataHandler.Delete();
    }

    private void Awake()
    {
        if (instance != null)
            DontDestroyOnLoad(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {
        gameData = fileDataHandler.Load();
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
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
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
}

