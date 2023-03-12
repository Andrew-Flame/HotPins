using HarmonyLib;

namespace HotPins.GameClasses;

internal static class GamePlayer {
    /// <summary>An instance of the game player</summary>
    private static Player _instance;

    /// <summary>Get an instance of the game character</summary>
    [HarmonyPatch(typeof(Player), "SetPlayerID")]
    private static class GetCharacterInstance {
        private static void Postfix(ref Player __instance) {
            long id = __instance.GetPlayerID();  //Get player id
            string name = __instance.GetPlayerName();  //Get player name
            /* If this player is our, update the current player's field */
            if (id == GamePlayerProfile.GetPlayerId() && name == GamePlayerProfile.GetPlayerName()) 
                _instance = __instance;
        }
    }

    /// <summary>Get the position of the game character</summary>
    /// <returns>The position of the game character</returns>
    public static UnityEngine.Vector3 GetPosition() {
        return _instance.transform.position;
    }
}