namespace Save_and_Load
{
    public interface ISaveManager
    {
        void LoadData(GameData _data);
        void SaveData(ref GameData _data);
    }
}
