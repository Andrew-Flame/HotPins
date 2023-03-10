using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace HotPins.GameClasses;

internal static class GameLocationProxy {
    /// <summary>A list with all proxy locations which we need</summary>
    public static List<Transform> LocationsProxy { get; } = new();

    [HarmonyPatch(typeof(LocationProxy), "Awake")]
    private static class GetLocationProxyWhenAwake {
        private static void Postfix(ref GameObject ___m_instance) {
            /* Extend the LocationProxy objects */
            if (___m_instance != null) ___m_instance.AddComponent<LocationProxyExtended>();
        }

        private class LocationProxyExtended : MonoBehaviour {
            private bool _isCorrect;  //Do we need this point of interest

            /// <summary>Check whether this point of interest is the one we need</summary>
            private void Awake() {
                _isCorrect = System.Text.RegularExpressions.Regex.IsMatch(name.Trim(),
                    @"^(Crypt|TrollCave|SunkenCrypt|MountainCave|GoblinCamp|Mistlands_DvergrTownEntrance).*$");
            }

            /// <summary>Adding the location we need to the list when ot appears</summary>
            private void Start() {
                if (_isCorrect) LocationsProxy.Add(gameObject.transform);
            }

            /// <summary>Removing the location from the list when it disappears</summary>
            private void OnDestroy() {
                if (_isCorrect) LocationsProxy.Remove(gameObject.transform);
            }
        }
    }
}