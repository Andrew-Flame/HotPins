using BepInEx;
using HarmonyLib;

namespace HotPins {
    [BepInPlugin("Flame.HotPins", "HotPins", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class Master : BaseUnityPlugin {
        private readonly Harmony harmony = new Harmony("Flame.HotPins");

        void Awake() {
            harmony.PatchAll();  //Patching the harmony
        }
    }
}