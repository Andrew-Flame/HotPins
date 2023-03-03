using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace HotPins.GameClasses;

internal static class GameLocationProxy {
    public static List<Transform> LocationsProxy { get; } = new ();

    [HarmonyPatch(typeof(LocationProxy), "Awake")]
    private static class GetLocationProxyWhenAwake {
        private static void Postfix(ref GameObject ___m_instance) {
            /* Extend the LocationProxy class */
            if (___m_instance != null) ___m_instance.AddComponent<LocationProxyExtended>();
        }

        private class LocationProxyExtended : MonoBehaviour {
            private bool _isCorrect;  //Do we need this point of interest

            void Awake() {
                /* We check whether this point of interest is the one we need */
                _isCorrect = System.Text.RegularExpressions.Regex.IsMatch(name.Trim(),
                    @"^(Crypt|TrollCave|SunkenCrypt|MountainCave|GoblinCamp|Mistlands_DvergrTownEntrance).*$");
            }

            void Start() {
                /* Adding the location we need to the list when ot appears */
                if (_isCorrect) LocationsProxy.Add(gameObject.transform);
            }

            void OnDestroy() {
                /* Removing the location from the list when it disappears */
                if (_isCorrect) LocationsProxy.Remove(gameObject.transform);
            }
        }
    }
}