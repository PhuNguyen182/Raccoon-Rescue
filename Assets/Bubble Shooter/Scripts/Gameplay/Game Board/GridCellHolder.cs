using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BubbleShooter.Scripts.Gameplay.GameBoard
{
    [ExecuteInEditMode]
    public class GridCellHolder : MonoBehaviour
    {
        [SerializeField] private bool isCeil = false;
        
        public Vector3Int GridPosition { get; set; }

        public bool IsCeilGrid 
        { 
            get => isCeil; 
            set => isCeil = value; 
        }

        private void OnDrawGizmos()
        {
            string pos = $"{GridPosition.x}:{GridPosition.y}";
            DrawString(pos, transform.position + Vector3.up * 0.5f, Color.black);
        }

        static void DrawString(string text, Vector3 worldPos, Color? colour = null)
        {
            Handles.BeginGUI();
            if (colour.HasValue) GUI.color = colour.Value;
            var view = SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
            Handles.EndGUI();
        }
    }
}
