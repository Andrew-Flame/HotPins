using HarmonyLib;
using System.Collections.Generic;

namespace HotPins {
    class GameMinimap {
        private static Minimap instance;  //A minimap instance
        private static List<Minimap.PinData> pinList = new List<Minimap.PinData>();  //List of pins

        /** Getting an instance of a game minimap */
        [HarmonyPatch(typeof(Minimap), "Awake")]
        class GetMinimapInstance {
            private static void Postfix(ref Minimap ___m_instance, ref List<Minimap.PinData> ___m_pins) {
                instance = ___m_instance;
                pinList = ___m_pins;
            }
        }

        /** Method for returning an instance of a game minimap */
        public static ref Minimap GetInstance() {
            return ref instance;
        }

        /** Method for returning an instance of a pin list */
        public static ref List<Minimap.PinData> GetPinList() {
            return ref pinList;
        }
    }
}
