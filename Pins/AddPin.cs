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

        public static void Run(Pin pin, Vector3 position = new Vector3()) {
            if (GameMinimap.GetInstance() == null) return;  //If there is no instance of the minimap, then the player is in the main menu
            if (position == new Vector3()) position = GamePlayer.GetPosition();  //Get the player's position as a pin position if it not stated before
            Minimap.PinType pinType = (Minimap.PinType)pinTypeDictionary[pin.getType()];  //Get the pin type as a standard value
            GameMinimap.GetInstance().AddPin(position, pinType, pin.getName(), true, false);  //Add pin to the map
            Debug.Log($"The pin \"{pin.getName()}\" (type: {pin.getType()}) was successfully marked on the map {Math.Round(position.x)}:{Math.Round(position.z)}");  //Output info
        }
    }
}
