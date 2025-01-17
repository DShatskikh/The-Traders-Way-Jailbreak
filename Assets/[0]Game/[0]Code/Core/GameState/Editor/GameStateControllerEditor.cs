using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(GameStateController))]
    public class GameStateControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var gameStateController = (GameStateController)target;

            if (GUILayout.Button("Open Skin Shop"))
            {
                gameStateController.OpenShop();
            }
        }
    }
}