using System;
using UnityEngine;

namespace HotPins {
    class AddPin {
        /** Method for adding a pin to the game map */
        public static void Run(Terminal.ConsoleEventArgs args) {
            if (args.Length != 3) {  //If we don't have enough arguments
                args.Context.AddString("Error!\nTwo arguments are required:\npinType pinName");
                return;
            }
            
            try {  //Trying to add a pin code to the card
                Vector3 position = GameCharacter.GetPosition();  //Get the player's position as a pin position
                Debug.Log($"{position.x} {position.y} {position.z}");
                Minimap.PinType pinType = (Minimap.PinType)Enum.Parse(typeof(Minimap.PinType), args.Args[1]);  //Get pin type
                string name = args.Args[2];  //Get pin name
                GameMinimap.GetInstance().AddPin(position, pinType, name, true, false);  //Add pin to the map
                Debug.Log($"The pin \"{name}\" was successfully marked on the map");
            } catch {  //Otherwise, we output an error message
                args.Context.AddString("Error!\nTry to check if the arguments are entered in the correct format and try again");
            }
        }
    }
}
