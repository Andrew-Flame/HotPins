using HarmonyLib;
using UnityEngine;

namespace HotPins {
    class GamePlayer {
        private static Player instance;  //An instance of the game player

        /** Getting an instance of the game character's visual */
        [HarmonyPatch(typeof(Player), "Awake")]
        class GetCharacterVisualInstance {
            private static void Postfix(ref Player __instance) {
                instance = __instance; 
            }
        }

        /** Getting the position of the game character */
        public static Vector3 GetPosition() {
            return instance.transform.position;
        }
    }
}
