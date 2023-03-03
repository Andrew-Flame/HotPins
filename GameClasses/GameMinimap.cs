using System.Collections.Generic;
using HarmonyLib;

namespace HotPins.GameClasses; 

internal static class GameMinimap {
    private static Minimap _instance;  //A minimap instance
    private static List<Minimap.PinData> _pinList = new List<Minimap.PinData>();  //List of pins

    /** Getting an instance of a game minimap */
    [HarmonyPatch(typeof(Minimap), "Awake")]
    private static class GetMinimapInstance {
        private static void Postfix(ref Minimap ___m_instance, ref List<Minimap.PinData> ___m_pins) {
            _instance = ___m_instance;
            _pinList = ___m_pins;
        }
    }

    /** Method for returning an instance of a game minimap */
    public static ref Minimap GetInstance() {
        return ref _instance;
    }

    /** Method for returning an instance of a pin list */
    public static ref List<Minimap.PinData> GetPinList() {
        return ref _pinList;
    }
}