using Photon.Realtime;

public static class PlayerExtensions
{

    private const string TeamKey = "Team";//used in get method to check player team assgined
    private const int PlayerIndex = 0;
    public const float SoundLevel = 0.05f;

    public static void SetTeam(this Player player, string team)
    {
        player.CustomProperties[TeamKey] = team;
    }

    public static string GetTeam(this Player player)
    {
        return player.CustomProperties.ContainsKey(TeamKey) ? (string)player.CustomProperties[TeamKey] : null ;
    }

    public static void SetPlayerIndex(this Player player, int index)//set in room scene, when player joins a Team
    {
        player.CustomProperties[PlayerIndex] = index;
    }

    public static int GetPlayerIndex(this Player player)//used in game scene to determine where to spawn the character, and which character to give them
    {
        return player.CustomProperties.ContainsKey(PlayerIndex) ? (int)player.CustomProperties[PlayerIndex] : 0;
    }

    public static void SetVolume(this Player player, float soundLevel)//player set volume carryes over
    {
        player.CustomProperties[SoundLevel] = soundLevel;
    }

    public static float GetVolume(this Player player)
    {
        return player.CustomProperties.ContainsKey(SoundLevel) ? (float)player.CustomProperties[SoundLevel] : 0.0f;
    }
}
