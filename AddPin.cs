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
            
            try {  //Trying to add a pin code to the card
                Vector3 position = GamePlayer.GetPosition();  //Get the player's position as a pin position
                string pinTypeRaw = args.Args[1];  //Get the pin type as a raw string
                Minimap.PinType pinType = (Minimap.PinType)pinTypeDictionary[pinTypeRaw];  //Get the pin type as a standard value
                string name = args.Args[2];  //Get pin name
                GameMinimap.GetInstance().AddPin(position, pinType, name, true, false);  //Add pin to the map
                Debug.Log($"The pin \"{name}\" (type: {pinTypeRaw}) was successfully marked on the map {{{Math.Round(position.x)}:{Math.Round(position.z)}}}");
            } catch {  //Otherwise, we output an error message
                args.Context.AddString("Error!\nTry to check if the arguments are entered in the correct format and try again");
            }
        }
    }
}
