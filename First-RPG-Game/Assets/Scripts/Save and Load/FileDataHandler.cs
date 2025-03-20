using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Save_and_Load
{
    public class FileDataHandler : MonoBehaviour
    {
        private string dataDirPath;
        private string dataFileName;
        private bool encryptData;
        private string apiUrl = "http://prn-222.food/api/v1/Data";
        private string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJ1c2VybmFtZSI6ImRhdGhsZWNueCIsImV4cCI6MTc0MjExMjg4OCwiaXNzIjoiUlBHLUFQSSIsImF1ZCI6IlVuaXR5R2FtZUNsaWVudCJ9.Y-MS5ncVMvGrJxoqWdu4N-Q-N958PMkYcDx6XpclexQ";
        public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)
        {
            dataDirPath = _dataDirPath;
            dataFileName = _dataFileName;
            encryptData = _encryptData;
        }

        public async void Save(GameData data)
        {
            try
            {
                string jsonData = JsonUtility.ToJson(data, true);
                GameData gameData = JsonUtility.FromJson<GameData>(jsonData);
                TargetData targetData = new TargetData();
                targetData.screenName = gameData.screenName;
                targetData.gold = gameData.gold;
                targetData.strength = gameData.strength;
                targetData.agility = gameData.agility;
                targetData.intelligence = gameData.intelligence;
                targetData.vitality = gameData.vitality;
                targetData.inventory = new Dictionary<string, int>(gameData.inventory);
                targetData.equipmentId = gameData.equipmentId;
                targetData.closeCheckpointId = gameData.closeCheckpointId;
                targetData.checkPoints = new Dictionary<string, bool>(gameData.checkpoints);
                string targetJson = JsonConvert.SerializeObject(targetData, Formatting.Indented);
                await PostDataAsync(targetJson);
            }
            catch (Exception e)
            {
                Debug.LogError("Error saving data: " + e.Message);
            }
        }
        public async Task PostDataAsync(string jsonData)
        {
            Debug.Log("🟢 Dữ liệu gửi lên API: " + jsonData);
            byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + token);
                request.SetRequestHeader("accept", "*/*");

                var tcs = new TaskCompletionSource<bool>();
                request.SendWebRequest().completed += operation => tcs.SetResult(true);
                await tcs.Task;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("🟢 Dữ liệu gửi thành công: " + request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("🔴 Lỗi khi gửi API: " + request.error);
                }
            }
        }
        public async Task<GameData> Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            try
            {
                string jsonData = File.ReadAllText(fullPath);
                string json = JsonUtility.ToJson(await GetDataFromAPIAsync("dathlecnx"));
                return JsonUtility.FromJson<GameData>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading data: " + e.Message);
                return null;
            }
        }

        public void Delete()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        public async Task<GameData> GetDataFromAPIAsync(string username)
        {
            string getUrl = $"{apiUrl}?username={username}";
            using (UnityWebRequest request = UnityWebRequest.Get(getUrl))
            {
                request.SetRequestHeader("accept", "*/*");
                request.SetRequestHeader("Authorization", "Bearer " + token);

                var tcs = new TaskCompletionSource<bool>();
                request.SendWebRequest().completed += operation => tcs.SetResult(true);
                await tcs.Task;

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string jsonResult = request.downloadHandler.text;
                    TargetData targetData = JsonConvert.DeserializeObject<TargetData>(jsonResult);
                    GameData gameData = ConvertTargetDataToGameData(targetData);
                    Debug.Log("🟢 Dữ liệu GET API thành công: ");
                    return gameData;
                }
                else
                {
                    Debug.LogError("🔴 Lỗi GET API: " + request.error);
                    return null;
                }
            }
        }
        private GameData ConvertTargetDataToGameData(TargetData targetData)
        {
            GameData gameData = new GameData();
            gameData.screenName = targetData.screenName;
            gameData.gold = targetData.gold;
            gameData.strength = targetData.strength;
            gameData.agility = targetData.agility;
            gameData.intelligence = targetData.intelligence;
            gameData.vitality = targetData.vitality;

            gameData.inventory = new SerializableDictionary<string, int>();
            foreach (var kvp in targetData.inventory)
            {
                gameData.inventory.Add(kvp.Key, kvp.Value);
            }

            List<string> equipList = new List<string>(targetData.equipmentId);
            equipList.Reverse();
            gameData.equipmentId = equipList;

            gameData.closeCheckpointId = "";

            gameData.checkpoints = new SerializableDictionary<string, bool>();
            foreach (var kvp in targetData.checkPoints)
            {
                gameData.checkpoints.Add(kvp.Key, kvp.Value);
            }

            return gameData;
        }
    }
}