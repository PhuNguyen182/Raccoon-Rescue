using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Gameplay.Confiner
{
    public class SideWall : MonoBehaviour
    {
        [SerializeField] private GameObject fadeObject;

        public void ShowReflectWall(bool active)
        {
            fadeObject.SetActive(active);
        }
    }
}
