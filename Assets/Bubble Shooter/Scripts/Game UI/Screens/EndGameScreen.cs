using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private CompletePanel completePanel;
        [SerializeField] private FailurePanel failurePanel;

        public CompletePanel CompletePanel => completePanel;
        public FailurePanel FailurePanel => failurePanel;
    }
}
