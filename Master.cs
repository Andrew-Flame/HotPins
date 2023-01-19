using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace HotPins {
    [BepInPlugin("Flame.HotPins", "HotPins", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class Master : BaseUnityPlugin {
        private readonly Harmony harmony = new Harmony("Flame.HotPins");
        private Dictionary<string, Pin> pinDictionary = new Dictionary<string, Pin>() {
            { "backspace", new Pin("Fireplace", "Test") }
        };  //A dictionary that contains keycode-pin bundles

        void Awake() {
            harmony.PatchAll();  //Patching the harmony
        }

        void Update() {
            foreach (KeyValuePair<string, Pin> pair in pinDictionary)  //Checking whether any of the custom keys are pressed
                if (Input.GetKeyDown(pair.Key)) AddPin.Run(pair.Value);  //If pressed, add the pin to the game map
        }
    }
}