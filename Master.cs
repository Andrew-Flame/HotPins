﻿using System;
using BepInEx;
using System.IO;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using HotPins.GameClasses;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HotPins {
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Master : BaseUnityPlugin {
        #region General modification info
        /* Mod info */
        public const string MODNAME = "HotPins";
        public const string AUTHOR = "Flame";
        public const string GUID = AUTHOR + "." + MODNAME;
        public const string VERSION = "1.5.0";
        #endregion

        #region AutoPin values
        private static KeyCode autoPinKey = KeyCode.G;
        private static int autoPinRadius = 15;
        private static string autoPinType = "Hammer";
        private static string[] autoPinsNames { get; set; } = {
            "Burial Chamber",
            "Troll Cave",
            "Sunken Crypt",
            "Frost Cave",
            "Fuling Village",
            "Infested Mine"
        };
        #endregion

        #region Other fields
        /* A bundle of keys and pins that will be marked on the map using these keys */
        private Dictionary<KeyCode[], Pin.Object> keyBundles = new Dictionary<KeyCode[], Pin.Object>();
        #endregion

        void Awake() {
            #region Harmony patch
            Harmony harmony = new Harmony(GUID);
            harmony.PatchAll();  //Patching the harmony
            #endregion

            #region Get configuration file path
            /* Get current configuration file and directory */
            FileInfo configFile = new FileInfo(Config.ConfigFilePath);
            DirectoryInfo configDirectory = configFile.Directory;
            #endregion

            #region Create the configuration file
            /* If the configuration file is not found */
            if (!configFile.Exists) {
                if (!configDirectory.Exists) configDirectory.Create();  //Create config directory if it doesn't exist

                /* Read default config file from assembly */
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HotPins.templates.Flame.HotPins.cfg"))
                using (StreamReader reader = new StreamReader(stream))
                    using (StreamWriter writer = new StreamWriter(configFile.FullName))
                        while (!reader.EndOfStream)
                            writer.WriteLine(reader.ReadLine());  //And write default config into a new config file
            }
            #endregion

            #region Get config values
            /* Reading config file */
            using (StreamReader config = new StreamReader(configFile.FullName)) {
                string section = string.Empty;  //Section of the ini document

                foreach (string line in config.ReadToEnd().Split('\n')) {
                    if (line.Trim().StartsWith("#")) //If the line is a comment
                        continue;  //Run the next loop iteration

                    if (Regex.IsMatch(line.Trim(), @"^\[\w+\]$")) {  //If a new section is declared
                        section = new Regex(@"[\[\]]").Replace(line.Trim(), "");  //Change the current section
                        continue;  //Run the next loop iteration
                    }

                    if (section.Contains("Binds") && 
                            Regex.IsMatch(line.Trim(), @"^\S+\s*=\s*""(Fireplace|House|Hammer|Ball|Cave)""\s*"".*""$")) {  //Check the string is a key-pin combination using regex
                        string[] keyValueBandle = line.Trim().Split('=');  //Get key-value bundle

                        /* Get keycodes shortcut */
                        string[] keyCodesAsStrings = keyValueBandle[0].Trim().Split('+');  //Get array of keycodes for keyboard shortcut as strings
                        KeyCode[] keyCodes = new KeyCode[keyCodesAsStrings.Length];  //Init an array for keaycodes
                        for (byte i = 0; i < keyCodes.Length; i++) keyCodes[i] = (KeyCode)Enum.Parse(typeof(KeyCode), keyCodesAsStrings[i]);  //Set all keycodes

                        /* Get pin info */
                        string[] typeNameBundle = Regex.Replace(keyValueBandle[1].Trim(), @"""\s*""", "\"\"").Split(new char[] { '\"' },
                            StringSplitOptions.RemoveEmptyEntries);  //Get pinType-pinName bundle
                        Pin.Object pin = new Pin.Object(typeNameBundle[0].Trim(), typeNameBundle[1].Trim());  //Create a pin instance

                        keyBundles.Add(keyCodes, pin);  //Add a key-pin bundle to dictionary
                        continue;  //Run the next loop iteration
                    }

                    if (section.Contains("AutoPin") && Regex.IsMatch(line.Trim(), @"^[a-zA-z]+\s*=\s*[a-zA-z0-9\s]+$")) {  //Check the string is a key-value combination using regex
                        /* Get key and value */
                        string[] keyValueBundle = line.Trim().Split('=');
                        string key = keyValueBundle[0].Trim();
                        string value = keyValueBundle[1].Trim();

                        /* Assign values to the required fields */
                        if (key.Contains("AutoPinKey")) autoPinKey = (KeyCode)Enum.Parse(typeof(KeyCode), value);
                        else if (key.Contains("AutoPinRadius")) autoPinRadius = int.Parse(value);
                        else if (key.Contains("AutoPinType")) autoPinType = value;
                        else if (key.Contains("BurialChamber")) autoPinsNames[0] = value;
                        else if (key.Contains("TrollCave")) autoPinsNames[1] = value;
                        else if (key.Contains("SunkenCrypt")) autoPinsNames[2] = value;
                        else if (key.Contains("FrostCave")) autoPinsNames[3] = value;
                        else if (key.Contains("FulingVillage")) autoPinsNames[4] = value;
                        else if (key.Contains("InfestedMine")) autoPinsNames[5] = value;

                        continue;  //Run the next loop iteration
                    }
                }
            }

            /* Sorting binds (we need the first keyboard shortcuts in the dictionary with the largest number of keys involved) */
            {
                Dictionary<KeyCode[], Pin.Object> tmpDictionary = new Dictionary<KeyCode[], Pin.Object>();  //Temporary dictionary
                byte bindSize = 1;  //Number of keys used by the bind
                foreach (KeyValuePair<KeyCode[], Pin.Object> bundle in keyBundles)  //Getting the maximum number
                    if (bundle.Key.Length > bindSize) bindSize = (byte)bundle.Key.Length;

                while (tmpDictionary.Count != keyBundles.Count) {  //Until both dictionaries have the same number of entries
                    foreach (KeyValuePair<KeyCode[], Pin.Object> bundle in keyBundles)  //Adding entries from the "longest" to the "shortest"
                        if (bundle.Key.Length == bindSize) tmpDictionary.Add(bundle.Key, bundle.Value);
                    bindSize--;
                }
                keyBundles = tmpDictionary;  //Overwriting the original dictionary
            }
            #endregion
        }

        void Update() {
            #region First check
            /* The first check for pressing the button to automatically create a pin */
            if (Input.GetKeyDown(autoPinKey)) {
                Vector3 playerPos = GamePlayer.GetPosition();  //Get player's position

                foreach (Transform proxyLocation in GameLocationProxy.locationsProxy) {
                    Vector3 proxyPos = proxyLocation.position;  //Get location's position
                    Vector3 vectorDistance = new Vector3(proxyPos.x - playerPos.x, proxyPos.y - playerPos.y, proxyPos.z - playerPos.z);  //Get vector distance
                    double linearDistance = Math.Sqrt(vectorDistance.x * vectorDistance.x + vectorDistance.y * vectorDistance.y 
                        + vectorDistance.z * vectorDistance.z);  //Get linear distance to proxy location

                    if (linearDistance < autoPinRadius) {  //If the location is close
                        string pinName = GetProxyLocationName(proxyLocation.name);  //Get location's name
                        if (pinName != string.Empty) Pin.Add.Run(new Pin.Object(autoPinType, pinName), proxyLocation.position);  //If it doesn't empty, create a pin
                    }
                }

                /* Using this method, we get the name of the location to create the pin */
                string GetProxyLocationName(string rawName) {
                    if (rawName.StartsWith("Crypt")) return autoPinsNames[0];
                    if (rawName.StartsWith("TrollCave")) return autoPinsNames[1];
                    if (rawName.StartsWith("SunkenCrypt")) return autoPinsNames[2];
                    if (rawName.StartsWith("MountainCave")) return autoPinsNames[3];
                    if (rawName.StartsWith("GoblinCamp")) return autoPinsNames[4]; 
                    if (rawName.StartsWith("Mistlands_DvergrTownEntrance")) return autoPinsNames[5];
                    return string.Empty;
                }
                return;  //Leaving the method
            }
            #endregion

            #region Second check
            /* Second check for custom binds */
            foreach (KeyValuePair<KeyCode[], Pin.Object> bundle in keyBundles) {  //Checking whether any of the custom keys are pressed
                if (CheckKeys(bundle.Key)) {  //If all the necessary keys are pressed
                    Pin.Add.Run(bundle.Value);  //Add the pin to the map
                    return;  //You cannot add multiple pins in one frame, so we exit the method
                }

                /* Using this method, we check whether all the necessary keys are pressed */
                bool CheckKeys(KeyCode[] keyCodes) {
                    if (!Input.GetKeyDown(keyCodes[keyCodes.Length - 1])) return false;  //Check the last pressed key (if it is not pressed, then the code should not be executed)
                    for (byte i = 0; i < keyCodes.Length - 1; i++) if (!Input.GetKey(keyCodes[i])) return false;  //If at least one key is not pressed, exit the method
                    return true;  //If the code execution has reached here, all the necessary keys are pressed
                }
            }
            #endregion
        }
    }
}