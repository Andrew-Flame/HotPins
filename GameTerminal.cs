using HarmonyLib;

namespace HotPins {
    class GameTerminal {
        /** Class for adding a command to the game terminal */
        [HarmonyPatch(typeof(Terminal), "Awake")]
        class AddTerminalCommnd {
            private static void Postfix() {
                Terminal.ConsoleEvent addPinEvent = AddPin.Run;  //Event that will occur after entering the command
                new Terminal.ConsoleCommand("addpin", "create a pin", addPinEvent);  //Declare our terminal command
            }
        }
    }
}
