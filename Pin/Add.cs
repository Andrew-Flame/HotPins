using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotPins.Pin {
    class Add {
        private static readonly Dictionary<string, int> pinTypeDictionary = new Dictionary<string, int>() {
            { "Fireplace", 0 },
            { "House", 1 },
            { "Hammer", 2 },
            { "Ball", 3 },
            { "Cave", 6 }
        };  //Dictionary for default pin types

        /* Using this method, we add the pin to the map */
        public static void Run(Pin pin, Vector3 position = new Vector3()) {
            if (GameMinimap.GetInstance() == null) return;  //If there is no instance of the minimap, then the player is in the main menu
            if (position == new Vector3()) position = GamePlayer.GetPosition();  //Get the player's position as a pin position if it not stated before
            Minimap.PinType pinType = (Minimap.PinType)pinTypeDictionary[pin.getType()];  //Get the pin type as a standard 
            if (HaveSamePin(ref position, pin)) return;  //If we already have such a pin, we exit the method
            GameMinimap.GetInstance().AddPin(position, pinType, pin.getName(), true, false);  //Add pin to the map
            Debug.Log($"The pin \"{pin.getName()}\" (type: {pin.getType()}) was successfully marked on the map {Math.Round(position.x)}:{Math.Round(position.z)}");  //Output info
        }

        /* Using this method, we check if there are no other pins with same position, name and type */
        private static bool HaveSamePin(ref Vector3 playerPos, Pin pin) {
            foreach (Minimap.PinData existingPin in GameMinimap.GetPinList()) {  //Loop through all the pins
                Vector3 vectorDistance = playerPos - existingPin.m_pos;  //Get vector distance
                double linearDistance = Math.Sqrt(vectorDistance.x *   vectorDistance.x + vectorDistance.z * vectorDistance.z);  //Get linear distance

                /* If at least one pin is very close, has the same type and name, return true */
                if (linearDistance < 5 && existingPin.m_name == pin.getName() && existingPin.m_type == (Minimap.PinType)pinTypeDictionary[pin.getType()])
                    return true;
            }

            return false;  //We return false if we didn't find the same pins
        }
    }
}
