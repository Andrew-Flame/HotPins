using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace HotPins {
    [BepInPlugin("Flame.HotPins", "HotPins", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class ExtendedMinimap : BaseUnityPlugin {
        private readonly Harmony harmony = new Harmony("Flame.HotPins");
        public static Minimap minimapInstance;

        void Awake() {
            harmony.PatchAll();  //Patching the harmony
        }

        /* Getting an instance of the minimap */
        [HarmonyPatch(typeof(Minimap), "Awake")]
        class GetMinimapInstance {
            private static void Postfix(ref Minimap ___m_instance) {
                minimapInstance = ___m_instance;
            }
        }

        /* Assigning keys to control */
        [HarmonyPatch(typeof(Terminal), nameof(Terminal.Awake))]
        class AssignKeys {
            private static void Postfix(ref Terminal __instance, ref List<Terminal.ConsoleCommand> ___m_commandList) {
                /* Init add pin command */
                Terminal.ConsoleEvent addPinEvent = AddPin.Run;  //Event that will occur after entering the command
                Terminal.ConsoleCommand addPinCommand = new Terminal.ConsoleCommand("addpin", "create a pin", addPinEvent);
            }
        }
    }
}