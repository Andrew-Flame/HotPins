using System;
using BepInEx;
using System.IO;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using System.Reflection;
using HotPins.GameClasses;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HotPins; 

[BepInPlugin(ModInfo.GUID, ModInfo.MODNAME, ModInfo.VERSION)]
internal sealed class Master : BaseUnityPlugin {
    /// <summary>A structure containing AutoPin properties</summary>
    private AutoPin.Properties _autoPin = new ("Burial Chamber", "Troll Cave", "Sunken Crypt", "Frost Cave", "Fuling Village", "Infested Mine");
    
    /// <summary>A bundle of keys and pins that will be marked on the map using these keys</summary>
    private Dictionary<KeyCode[], Pin.Pin> _keyBundles = new();

    private void Awake() {
        #region Harmony patch
        Harmony harmony = new Harmony(ModInfo.GUID);
        harmony.PatchAll();  //Patching the harmony
        #endregion
        FileInfo configFile = new FileInfo(Config.ConfigFilePath);  //Get current configuration file
        CreateConfigFile(configFile);  //Create config file if it does not exist
        GetConfigValues(configFile.FullName);  //Get config values from file
    }

    private void Update() {
        if (Input.GetKeyDown(_autoPin.Key) && _autoPin.Enabled) EvalAutoPin();  //If AutoPin key is pressed, get all close proxy locations
        else CheckForPressedBinds();  //Else check for pressed custom binds
    }
    
    #region Awake methods

    /// <summary>
    /// Create config file if it doesn't exist
    /// </summary>
    /// <param name="configFile">Configuration file</param>
    private void CreateConfigFile(FileInfo configFile) {
        DirectoryInfo configDirectory = configFile.Directory;
        /* If the configuration file is not found */
        if (!configFile.Exists) {
            if (!configDirectory!.Exists) configDirectory.Create();  //Create config directory if it doesn't exist

            /* Read default config file from assembly */
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HotPins.templates.Flame.HotPins.cfg");
            using StreamReader reader = new StreamReader(stream!);
            using StreamWriter writer = new StreamWriter(configFile.FullName);
            while (!reader.EndOfStream)
                writer.WriteLine(reader.ReadLine());  //And write default config into a new config file
        }
    }
    
    /// <summary>
    /// Get config values from the configuration file
    /// </summary>
    /// <param name="configFile">Configuration file path</param>
    private void GetConfigValues(string configFile) {
        /* Reading config file */
        using (StreamReader config = new StreamReader(configFile)) {
            string section = string.Empty;  //Section of the ini document

            foreach (string line in config.ReadToEnd().Split('\n')) {
                if (line.Trim().StartsWith("#")) //If the line is a comment
                    continue;  //Run the next loop iteration
                
                if (Regex.IsMatch(line.Trim(), @"^\[\w+\]$")) {  //If a new section is declared
                    section = new Regex(@"[\[\]]").Replace(line.Trim(), "");  //Change the current section
                } else if (section.Contains("Binds") && Regex.IsMatch(line.Trim(), @"^\S+\s*=\s*""(Fireplace|House|Hammer|Ball|Cave)""\s*"".*""$")) {
                    /* Check the string is a key-pin combination using regex */
                    AddBind(line.Trim());  //Add a bind to the dictionary
                } else if (section.Contains("AutoPin") && Regex.IsMatch(line.Trim(), @"^[a-zA-z]+\s*=\s*[a-zA-z0-9\s]+$")) {
                    /* Check the string is a key-value combination using regex */
                    GetAutoPinsValues(line.Trim());
                }
            }
        }

        //Sort binds (we need the first keyboard shortcuts in the dictionary with the largest number of keys involved)
        _keyBundles = _keyBundles.OrderByDescending(m => m.Key.Length).ToDictionary(m => m.Key, m => m.Value);
    }
    
    /// <summary>
    /// Add key-bind pair to dictionary
    /// </summary>
    /// <param name="line">A config file line</param>
    private void AddBind(string line) {
        string[] keyValueBundle = line.Split('=');  //Get key-value bundle

        /* Get keycodes shortcut */
        string[] keyCodesAsStrings = keyValueBundle[0].Trim().Split('+');  //Get array of keycodes for keyboard shortcut as strings
        var keyCodes = new KeyCode[keyCodesAsStrings.Length];  //Init an array for keycodes
        for (byte i = 0; i < keyCodes.Length; i++) keyCodes[i] = (KeyCode)Enum.Parse(typeof(KeyCode), keyCodesAsStrings[i]);  //Set all keycodes

        /* Get pin info */
        string[] typeNameBundle = Regex.Replace(keyValueBundle[1].Trim(), @"""\s*""", "\"\"").Split(new [] { '\"' },
            StringSplitOptions.RemoveEmptyEntries);  //Get pinType-pinName bundle
        Pin.Pin pin = new Pin.Pin(typeNameBundle[0].Trim(), typeNameBundle[1].Trim());  //Create a pin instance

        _keyBundles.Add(keyCodes, pin);  //Add a key-pin bundle to dictionary
    }

    /// <summary>
    /// Get AutoPins config values
    /// </summary>
    /// <param name="line">A config file line</param>
    private void GetAutoPinsValues(string line) {
        /* Get key and value */
        string[] keyValueBundle = line.Split('=');
        string key = keyValueBundle[0].Trim();
        string value = keyValueBundle[1].Trim();

        /* Assign values to the required fields */
        if (key.Contains("AutoPinKey")) _autoPin.Key = (KeyCode)Enum.Parse(typeof(KeyCode), value);
        else if (key.Contains("AutoPinRadius")) _autoPin.Radius = int.Parse(value);
        else if (key.Contains("AutoPinType")) _autoPin.Type = value;
        else if (key.Contains("AutoPinEnabled")) _autoPin.Enabled = Convert.ToBoolean(value);
        else if (key.Contains("BurialChamber")) _autoPin.Names[0] = value;
        else if (key.Contains("TrollCave")) _autoPin.Names[1] = value;
        else if (key.Contains("SunkenCrypt")) _autoPin.Names[2] = value;
        else if (key.Contains("FrostCave")) _autoPin.Names[3] = value;
        else if (key.Contains("FulingVillage")) _autoPin.Names[4] = value;
        else if (key.Contains("InfestedMine")) _autoPin.Names[5] = value;
    }
    #endregion

    #region Update methods
    
    /// <summary>Evaluate for close proxy locations</summary>
    private void EvalAutoPin() {
        Vector3 playerPos = GamePlayer.GetPosition();  //Get player's position

        foreach (Transform proxyLocation in GameLocationProxy.LocationsProxy) {
            Vector3 proxyPos = proxyLocation.position;  //Get location's position
            Vector3 vectorDistance = proxyPos - playerPos;  //Get vector distance
            float sqrtLinearDistance = vectorDistance.x * vectorDistance.x + vectorDistance.z * vectorDistance.z;  //Get sqrt linear distance to proxy location

            if (sqrtLinearDistance < _autoPin.SqrtRadius) {  //If the location is close
                string pinName = GetProxyLocationName(proxyLocation.name);  //Get location's name
                if (pinName != string.Empty) Pin.Add.Run(new Pin.Pin(_autoPin.Type, pinName), proxyLocation.position);  //If it doesn't empty, create a pin
            }
        }
    }
    
    /// <summary>Get the name of the location to create the pin</summary>
    /// <param name="rawName">Name of unity object of proxy location</param>
    /// <returns>The current name of proxy location</returns>
    private string GetProxyLocationName(string rawName) {
        if (rawName.StartsWith("Crypt")) return _autoPin.Names[0];
        if (rawName.StartsWith("TrollCave")) return _autoPin.Names[1];
        if (rawName.StartsWith("SunkenCrypt")) return _autoPin.Names[2];
        if (rawName.StartsWith("MountainCave")) return _autoPin.Names[3];
        if (rawName.StartsWith("GoblinCamp")) return _autoPin.Names[4];
        if (rawName.StartsWith("Mistlands_DvergrTownEntrance")) return _autoPin.Names[5];
        return string.Empty;
    }

    /// <summary>Check for pressed custom binds</summary>
    private void CheckForPressedBinds() {
        foreach (var bundle in _keyBundles.Where(m => CheckKeysBundle(m.Key))) { //If all the necessary keys are pressed
            Pin.Add.Run(bundle.Value);  //Add the pin to the map
            return;  //You cannot add multiple pins in one frame, so we exit the method
        }
    }
    
    /// <summary>Check whether all the necessary keys are pressed</summary>
    /// <param name="keyCodes">key-pin bundle</param>
    /// <returns>True - if all the necessary keys are pressed<br/>
    /// False - otherwise</returns>
    private static bool CheckKeysBundle(KeyCode[] keyCodes) {
        if (!Input.GetKeyDown(keyCodes[keyCodes.Length - 1])) return false;  //Check the last pressed key (if it is not pressed, then the code should not be executed)
        for (byte i = 0; i < keyCodes.Length - 1; i++) if (!Input.GetKey(keyCodes[i])) return false;  //If at least one key is not pressed, exit the method
        return true;  //If the code execution has reached here, all the necessary keys are pressed
    }
    #endregion
}