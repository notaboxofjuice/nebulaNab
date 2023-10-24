using Photon.Realtime;

public static class PlayerExtensions
{

    private const string TeamKey = "Team";

    public static void SetTeam(this Player player, string team)
    {
        player.CustomProperties[TeamKey] = team;
    }

    public static string GetTeam(this Player player)
    {
        return player.CustomProperties.ContainsKey(TeamKey) ? (string)player.CustomProperties[TeamKey] : null ;
    }


}
