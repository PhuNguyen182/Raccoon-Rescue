using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
