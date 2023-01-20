using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HotPins {
    [BepInPlugin("Flame.HotPins", "HotPins", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class Master : BaseUnityPlugin {
        private readonly Harmony harmony = new Harmony("Flame.HotPins");
        private Dictionary<KeyCode, Pin> keyBundles = new Dictionary<KeyCode, Pin>();  //A bundle of keys and pins that will be marked on the map using these keys

        void Awake() {
            harmony.PatchAll();  //Patching the harmony

            /* Reading config file */
            StreamReader config = new StreamReader("BepInEx/config/Flame.HotPins.ini");
            foreach (string line in config.ReadToEnd().Split('\n')) {
                if (Regex.IsMatch(line.Trim(), @"^\S+\s*=\s*""(Fireplace|House|Hammer|Ball|Cave)""\s*"".*""$")) {  //Check the string is a key-pin combination
                    string[] keyValueBandle = line.Trim().Split('=');  //Get key-value bundle
                    KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyValueBandle[0].Trim());  //Get keycode

                    string[] typeNameBundle = Regex.Replace(keyValueBandle[1].Trim(), @"""\s*""", "\"\"").Split(new char[] { '\"' }, 
                        System.StringSplitOptions.RemoveEmptyEntries);  //Get pinType-pinName bundle
                    Pin pin = new Pin(typeNameBundle[0].Trim(), typeNameBundle[1].Trim());  //Create a pin instance

                    keyBundles.Add(keyCode, pin);  //Add a key-pin bundle to dictionary
                }
            }
            config.Close();  //Close the stream
        }

        void Update() {
            foreach (KeyValuePair<KeyCode, Pin> bundle in keyBundles)  //Checking whether any of the custom keys are pressed
                if (Input.GetKeyDown(bundle.Key)) AddPin.Run(bundle.Value);  //If pressed, add the pin to the game map*/
        }
    }
}