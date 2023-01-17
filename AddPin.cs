using System;
using UnityEngine;

namespace HotPins {
    internal class AddPin {
        /** Method for adding a pin to the game map */
        public static void Run(Terminal.ConsoleEventArgs args) {
            if (args.Length != 5) {  //If we don't have enough arguments
                args.Context.AddString("Error!\nFour arguments are required:\npositionX positionY pinType pinName");
                return;
            }
            
            try {  //Trying to add a pin code to the card
                Vector3 position = new Vector3(Convert.ToSingle(args.Args[1]), Convert.ToSingle(args.Args[2]), 0f);  //Get position as Vector3
                Minimap.PinType pinType = (Minimap.PinType)Enum.Parse(typeof(Minimap.PinType), args.Args[3]);  //Get pin type
                string name = args.Args[4];  //Get pin name
                Main.minimapInstance.AddPin(position, pinType, name, true, false);  //Add pin to the map
                Debug.Log($"The pin \"${name}\" was successfully marked on the map");
            } catch {  //Otherwise, we output an error message
                args.Context.AddString("Error!\nTry to check if the arguments are entered in the correct format and try again");
            }
        }
    }
}
