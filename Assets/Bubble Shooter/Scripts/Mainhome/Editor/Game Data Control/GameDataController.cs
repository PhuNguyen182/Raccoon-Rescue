using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BubbleShooter.Scripts.Mainhome.Editor.GameDataControl 
{
    public class GameDataController : EditorWindow
    {
        [MenuItem("Game Data/Clear Data")]
        public static void ClearData()
        {
            GameData.Instance.DeleteData();
        }
    }
}
