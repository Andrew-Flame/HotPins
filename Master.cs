using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HotPins {
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Master : BaseUnityPlugin {
        /* Mod info */
        public const string MODNAME = "HotPins";
        public const string AUTHOR = "Flame";
        public const string GUID = AUTHOR + "." + MODNAME;
        public const string VERSION = "1.4.0";

        /* Config file path */
        private const string configPath = "BepInEx/config/Flame.HotPins.cfg";

        /* A bundle of keys and pins that will be marked on the map using these keys */
        private Dictionary<KeyCode[], Pin> keyBundles = new Dictionary<KeyCode[], Pin>();

        void Awake() {
            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll();  //Patching the harmony

            /* If the configuration file is not found */
            if (!File.Exists(configPath)) {
                /* Read default config file from assembly */
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HotPins.templates.Flame.HotPins.cfg"))
                using (StreamReader reader = new StreamReader(stream))
                    using (StreamWriter writer = new StreamWriter(configPath))
                        while (!reader.EndOfStream)
                            writer.WriteLine(reader.ReadLine());  //And write default config into a new config file
            }

            /* Reading config file */
            using (StreamReader config = new StreamReader(configPath)) {
                foreach (string line in config.ReadToEnd().Split('\n')) {
                    if (Regex.IsMatch(line.Trim(), @"^\S+\s*=\s*""(Fireplace|House|Hammer|Ball|Cave)""\s*"".*""$")) {  //Check the string is a key-pin combination using regex
                        string[] keyValueBandle = line.Trim().Split('=');  //Get key-value bundle

                        /* Get keycodes shortcut */
                        string[] keyCodesAsStrings = keyValueBandle[0].Trim().Split('+');  //Get array of keycodes for keyboard shortcut as strings
                        KeyCode[] keyCodes = new KeyCode[keyCodesAsStrings.Length];  //Init an array for keaycodes
                        for (byte i = 0; i < keyCodes.Length; i++) keyCodes[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyCodesAsStrings[i]);  //Set all keycodes

                        /* Get pin info */
                        string[] typeNameBundle = Regex.Replace(keyValueBandle[1].Trim(), @"""\s*""", "\"\"").Split(new char[] { '\"' },
                            System.StringSplitOptions.RemoveEmptyEntries);  //Get pinType-pinName bundle
                        Pin pin = new Pin(typeNameBundle[0].Trim(), typeNameBundle[1].Trim());  //Create a pin instance

                        keyBundles.Add(keyCodes, pin);  //Add a key-pin bundle to dictionary
                    }
                }
            }

            /* Sorting binds (we need the first keyboard shortcuts in the dictionary with the largest number of keys involved) */
            {
                Dictionary<KeyCode[], Pin> tmpDictionary = new Dictionary<KeyCode[], Pin>();  //Temporary dictionary
                byte bindSize = 1;  //Number of keys used by the bind
                foreach (KeyValuePair<KeyCode[], Pin> bundle in keyBundles)  //Getting the maximum number
                    if (bundle.Key.Length > bindSize) bindSize = (byte)bundle.Key.Length;

                while (tmpDictionary.Count != keyBundles.Count) {  //Until both dictionaries have the same number of entries
                    foreach (KeyValuePair<KeyCode[], Pin> bundle in keyBundles)  //Adding entries from the "longest" to the "shortest"
                        if (bundle.Key.Length == bindSize) tmpDictionary.Add(bundle.Key, bundle.Value);
                    bindSize--;
                }
                keyBundles = tmpDictionary;  //Overwriting the original dictionary
            }
        }

        void Update() {
            foreach (KeyValuePair<KeyCode[], Pin> bundle in keyBundles) {  //Checking whether any of the custom keys are pressed
                if (CheckKeys(bundle.Key)) {
                    AddPin.Run(bundle.Value);  //If all the necessary keys are pressed, add the pin to the map
                    return;  //You cannot add multiple pins in one frame, so we exit the method
                }
            }

            /* Using this method, we check whether all the necessary keys are pressed */
            bool CheckKeys(KeyCode[] keyCodes) {
                if (!Input.GetKeyDown(keyCodes[keyCodes.Length - 1])) return false;  //Check the last pressed key (if it is not pressed, then the code should not be executed)
                for (byte i = 0; i < keyCodes.Length - 1; i++) if (!Input.GetKey(keyCodes[i])) return false;  //If at least one key is not pressed, exit the method
                return true;  //If the code execution has reached here, all the necessary keys are pressed
            }
        }
    }
}