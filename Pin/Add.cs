using System.Collections.Generic;
using HotPins.GameClasses;
using UnityEngine;

using static Minimap;

namespace HotPins.Pin;

internal static class Add {
    /// <summary>Dictionary for default pin types</summary>
    private static readonly Dictionary<string, PinType> PinTypeDictionary = new() {
        { "Fireplace", PinType.Icon0 },
        { "House", PinType.Icon1 },
        { "Hammer", PinType.Icon2 },
        { "Ball", PinType.Icon3 },
        { "Cave", PinType.Icon4 }
    };

    /// <summary>Add the pin to the map</summary>
    /// <param name="pin">Instance of the pin</param>
    /// <param name="position">Current pin position</param>
    public static void Run(Pin pin, Vector3 position = new()) {
        if (!GameMinimap.Exist()) return;  //If there is no instance of the minimap, then the player is in the main menu
        if (position == new Vector3()) position = GamePlayer.GetPosition();  //Get the player's position as a pin position if it not stated before
        PinType pinType = PinTypeDictionary[pin.Type];  //Get the pin type as a standard 
        if (HaveSamePin(ref position, pin)) return;  //If we already have such a pin, we exit the method
        GameMinimap.GetInstance().AddPin(position, pinType, pin.Name, true, false);  //Add pin to the map
    }

    /// <summary>Check if there are no other pins with same position, name and type</summary>
    /// <param name="position">Vector of position where we want to create pin</param>
    /// <param name="pin">Pin instance which we want to add to the map</param>
    /// <returns>True - if there is same pin in this position<br/>
    /// False - if there are no same pins in this position</returns>
    private static bool HaveSamePin(ref Vector3 position, Pin pin) {
        foreach (PinData existingPin in GameMinimap.GetPinList()) {  //Loop through all the pins
            Vector3 vectorDistance = position - existingPin.m_pos;  //Get vector distance
            float sqrtLinearDistance = vectorDistance.x * vectorDistance.x + vectorDistance.z * vectorDistance.z;  //Get linear sqrt distance

            /* If at least one pin is very close, has the same type and name, return true */
            if (sqrtLinearDistance < 25 && existingPin.m_name == pin.Name && existingPin.m_type == PinTypeDictionary[pin.Type])
                return true;
        }
        return false;  //We return false if we didn't find the same pins
    }
}