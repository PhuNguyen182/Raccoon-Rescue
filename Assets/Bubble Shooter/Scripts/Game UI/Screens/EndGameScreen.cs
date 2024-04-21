using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BubbleShooter.Scripts.GameUI.Screens
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private CompletePanel completePanel;
        [SerializeField] private FailurePanel failurePanel;

        public void ShowWinPanel()
        {
            completePanel.gameObject.SetActive(true);
        }

        public UniTask<bool> ShowLosePanel()
        {
            return failurePanel.Show();
        }
    }
}
