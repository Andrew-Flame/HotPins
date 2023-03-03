using HarmonyLib;

namespace HotPins.GameClasses;

internal static class GamePlayer {
    private static Player _instance;  //An instance of the game player

    /** Getting an instance of the game character's visual */
    [HarmonyPatch(typeof(Player), "Awake")]
    private static class GetCharacterVisualInstance {
        private static void Postfix(ref Player __instance) {
            _instance = __instance; 
        }
    }

    /** Getting the position of the game character */
    public static UnityEngine.Vector3 GetPosition() {
        return _instance.transform.position;
    }
}