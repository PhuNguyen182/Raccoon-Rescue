using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Mainhome.Inputs;

namespace BubbleShooter.Scripts.Mainhome.Handlers
{
    public class InteractiveController : MonoBehaviour
    {
        [SerializeField] private MainhomeInput mainhomeInput;
        [SerializeField] private GameObject eventSystemObject;

        public void SetInteractive(bool interactable)
        {
            mainhomeInput.IsActived = interactable;
            eventSystemObject.SetActive(interactable);
        }
    }
}
