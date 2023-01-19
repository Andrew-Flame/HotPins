using HarmonyLib;
using UnityEngine;

namespace HotPins {
    class GameMinimap {
        private static Minimap instance;  //A minimap instance
        public static Vector3 position;

        /** Getting an instance of a game minimap */
        [HarmonyPatch(typeof(Minimap), "Awake")]
        class GetMinimapInstance {
            private static void Postfix(ref Minimap ___m_instance) {
                instance = ___m_instance;
            }
        }

        [HarmonyPatch(typeof(Minimap), "UpdatePlayerMarker")]
        class FUCK {
            private static void Postfix(object[] __args) {
                position = ((Player)__args[0]).transform.position;
            }
        }

        /** Method for returning an instance of a game minimap
         * return an instance of a game minimap
         */
        public static ref Minimap GetInstance() {
            return ref instance;
        }
    }
}
