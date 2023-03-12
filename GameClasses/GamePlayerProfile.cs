using HarmonyLib;

namespace HotPins.GameClasses; 

internal static class GamePlayerProfile {
    private static string _playerName;
    private static long _playerId;
    
    /// <summary>Get the last loaded player profile</summary>
    [HarmonyPatch(typeof(PlayerProfile), "Load")]
    private static class GetPlayerProfile {
        private static void Postfix(ref PlayerProfile __instance) {
            _playerName = __instance.GetName();  //Get player name
            _playerId = __instance.GetPlayerID();  //Get player id
        }
    }

    /// <summary>Get current player's name</summary>
    /// <returns>Player's name</returns>
    public static string GetPlayerName() => _playerName;
    /// <summary>Get current player's id</summary>
    /// <returns>Player's id</returns>
    public static long GetPlayerId() => _playerId;
}