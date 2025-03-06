using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, int> skillTree;
    public List<string> equipmentId;
    public string closeCheckpointId;

    public SerializableDictionary<string, bool> checkpoints;
    public GameData()
    {
        this.currency = 0;
        this.inventory = new SerializableDictionary<string, int>();
        this.skillTree = new SerializableDictionary<string, int>();
        this.equipmentId = new List<string>();
        this.closeCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
    }
}
