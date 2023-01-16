using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace HotPins {
    [BepInPlugin("Flame.HotPins", "HotPins", "1.0")]
    [BepInProcess("valheim.exe")]
    public class ExtendedMinimap : BaseUnityPlugin {
        private readonly Harmony harmony = new Harmony("Flame.HotPins");

        void Awake() {
            harmony.PatchAll();  //Patching the harmony


        }

        /* Working with player markers */
        [HarmonyPatch(typeof(Minimap), "UpdatePlayerMarker")]
        class PlayerMarkerPatch {
            private static void Postfix(ref RectTransform ___m_smallMarker, ref RectTransform ___m_smallShipMarker) {

            }
        }
    }
}