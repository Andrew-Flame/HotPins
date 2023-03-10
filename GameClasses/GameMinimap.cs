using System.Collections.Generic;
using HarmonyLib;

namespace HotPins.GameClasses; 

internal static class GameMinimap {
    /// <summary>Existing of game minimap</summary>
    private static bool _exist;  
    /// <summary>A minimap instance</summary>
    private static Minimap _instance;
    /// <summary>List of pins</summary>
    private static List<Minimap.PinData> _pinList = new();

    /// <summary>Get an instance of a game minimap</summary>
    [HarmonyPatch(typeof(Minimap), "Awake")]
    private static class GetMinimapInstance {
        private static void Postfix(ref Minimap ___m_instance, ref List<Minimap.PinData> ___m_pins) {
            _instance = ___m_instance;
            _pinList = ___m_pins;
            _exist = true;
        }
    }

    /// <summary>Get an instance of a game minimap</summary>
    /// <returns>An instance of a game minimap</returns>
    public static Minimap GetInstance() => _instance;

    /// <summary>Get an instance of a pin list</summary>
    /// <returns>An instance of a pin list</returns>
    public static List<Minimap.PinData> GetPinList() => _pinList;
    
    /// <summary>Find out if a minimap object exists</summary>
    /// <returns>Existing of game minimap</returns>
    public static bool Exist() => _exist;
}