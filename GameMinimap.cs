using HarmonyLib;

namespace HotPins {
    class GameMinimap {
        private static Minimap instance;  //A minimap instance

        /** Class for getting an instance of a game minimap */
        [HarmonyPatch(typeof(Minimap), "Awake")]
        class GetMinimapInstance {
            private static void Postfix(ref Minimap ___m_instance) {
                instance = ___m_instance;
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
