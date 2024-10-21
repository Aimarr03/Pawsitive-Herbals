public interface IDataPersistance
{
    public void LoadScene(GameData gameData);

    public void SaveScene(ref GameData gameData);
}
