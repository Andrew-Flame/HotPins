using HarmonyLib;
using UnityEngine;

namespace HotPins {
    class GameCharacter {
        private static GameObject characterVisual;  //An instance of the game character's visual

        /** Getting an instance of the game character's visual */
        [HarmonyPatch(typeof(Character), "Awake")]
        class GetCharacterVisualInstance {
            private static void Postfix(ref GameObject ___m_visual) {
                characterVisual = ___m_visual;
            }
        }

        /** Getting the position of the game character */
        public static Vector3 GetPosition() {
            return characterVisual.transform.position;
        }
    }
}
