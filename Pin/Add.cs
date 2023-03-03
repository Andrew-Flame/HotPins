using System;
using System.Collections.Generic;
using HotPins.GameClasses;
using UnityEngine;

namespace HotPins.Pin;

internal static class Add {
    private static readonly Dictionary<string, int> PinTypeDictionary = new() {
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
        Minimap.PinType pinType = (Minimap.PinType)PinTypeDictionary[pin.GetPinType()];  //Get the pin type as a standard 
        if (HaveSamePin(ref position, pin)) return;  //If we already have such a pin, we exit the method
        GameMinimap.GetInstance().AddPin(position, pinType, pin.GetPinName(), true, false);  //Add pin to the map
        Debug.Log($"The pin \"{pin.GetPinName()}\" (type: {pin.GetPinType()}) was successfully marked on the map {Math.Round(position.x)}:{Math.Round(position.z)}");  //Output info
    }

    /* Using this method, we check if there are no other pins with same position, name and type */
    private static bool HaveSamePin(ref Vector3 playerPos, Pin pin) {
        foreach (Minimap.PinData existingPin in GameMinimap.GetPinList()) {  //Loop through all the pins
            Vector3 vectorDistance = playerPos - existingPin.m_pos;  //Get vector distance
            double linearDistance = Math.Sqrt(vectorDistance.x *   vectorDistance.x + vectorDistance.z * vectorDistance.z);  //Get linear distance

            /* If at least one pin is very close, has the same type and name, return true */
            if (linearDistance < 5 && existingPin.m_name == pin.GetPinName() && existingPin.m_type == (Minimap.PinType)PinTypeDictionary[pin.GetPinType()])
                return true;
        }

        return false;  //We return false if we didn't find the same pins
    }
}