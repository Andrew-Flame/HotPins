using HarmonyLib;

namespace HotPins {
    class GameTerminal {
        /** Adding a command to the game terminal */
        [HarmonyPatch(typeof(Terminal), "Awake")]
        class AddTerminalCommnd {
            private static void Postfix() {
                new Terminal.ConsoleCommand("addpin", "create a pin", AddPin.Run);  //Init terminal command
            }
        }
    }
}