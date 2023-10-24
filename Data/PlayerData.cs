[Serializable]
public class PlayerData : ISaveData
{
    private ulong coins;

    public PlayerData(ulong coins)
    {
        Coins = coins;
    }

    public ulong Coins { get => coins; set => coins = value; }
}