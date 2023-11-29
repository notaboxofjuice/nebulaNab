using Photon.Realtime;

public static class PlayerExtensions
{

    private const string TeamKey = "Team";
    private const int PlayerIndex = 0;

    public static void SetTeam(this Player player, string team)
    {
        player.CustomProperties[TeamKey] = team;
    }

    public static string GetTeam(this Player player)
    {
        return player.CustomProperties.ContainsKey(TeamKey) ? (string)player.CustomProperties[TeamKey] : null ;
    }

    public static void SetPlayerIndex(this Player player, int index)
    {
        player.CustomProperties[PlayerIndex] = index;
    }

    public static int GetPlayerIndex(this Player player)
    {
        return player.CustomProperties.ContainsKey(PlayerIndex) ? (int)player.CustomProperties[PlayerIndex] : 0;
    }

}
