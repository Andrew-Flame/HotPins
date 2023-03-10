using HarmonyLib;

namespace HotPins.GameClasses;

internal static class GamePlayer {
    /// <summary>An instance of the game player</summary>
    private static Player _instance;

    /// <summary>Get an instance of the game character's visual</summary>
    [HarmonyPatch(typeof(Player), "Awake")]
    private static class GetCharacterVisualInstance {
        private static void Postfix(ref Player __instance) {
            _instance = __instance; 
        }
    }

    /// <summary>Get the position of the game character</summary>
    /// <returns>The position of the game character</returns>
    public static UnityEngine.Vector3 GetPosition() {
        return _instance.transform.position;
    }
}