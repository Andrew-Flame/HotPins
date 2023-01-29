using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotPins {
    class AddPin {
        private static readonly Dictionary<string, int> pinTypeDictionary = new Dictionary<string, int>() {
            { "Fireplace", 0 },
            { "House", 1 },
            { "Hammer", 2 },
            { "Ball", 3 },
            { "Cave", 6 }
        };  //Dictionary for default pin types

        /** Method for adding a pin to the game map */
        public static void Run(Terminal.ConsoleEventArgs args) {
            if (args.Length != 3) {  //If we don't have enough arguments
                args.Context.AddString("Error!\nTwo arguments are required\nArguments: pin-type pin-name");
                return;
            }
            
            try {  //Trying to add a pin to the map
                string pinTypeRaw = args.Args[1];  //Get the pin type as a raw string
                string pinName = args.Args[2];  //Get the pin name
                SetPin(pinTypeRaw, pinName);  //Set the pin
            } catch {  //Otherwise, we output an error message
                args.Context.AddString("Error!\nTry to check if the arguments are entered in the correct format and try again");
            }
        }
        public static void Run(Pin pin) {
            SetPin(pin.getType(), pin.getName());
        }

        private static void SetPin(String pinTypeRaw, string pinName) {
            if (GameMinimap.GetInstance() == null) return;  //If there is no instance of the minimap, then the player is in the main menu
            Vector3 position = GamePlayer.GetPosition();  //Get the player's position as a pin position
            Minimap.PinType pinType = (Minimap.PinType)pinTypeDictionary[pinTypeRaw];  //Get the pin type as a standard value
            GameMinimap.GetInstance().AddPin(position, pinType, pinName, true, false);  //Add pin to the map
            Debug.Log($"The pin \"{pinName}\" (type: {pinTypeRaw}) was successfully marked on the map {Math.Round(position.x)}:{Math.Round(position.z)}");  //Output info
        }
    }
}
