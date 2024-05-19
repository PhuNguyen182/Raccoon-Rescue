using R3;
using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Mainhome.PopupBoxes.PlayGamePopup;
using BubbleShooter.Scripts.Common.PlayDatas;
using Sirenix.OdinInspector;

namespace BubbleShooter.Scripts.Mainhome.ProgressMap
{
    public class ProgressMapManager : MonoBehaviour
    {
        [SerializeField] private List<LevelNodePath> nodePaths;

        private const string PlayGamePopupPath = "Popups/Play Game Popup";

        private Dictionary<int, LevelNodePath> _nodePathDict;

        private CancellationToken _token;
        private IDisposable _disposable;

        private void Awake()
        {
            _token = this.GetCancellationTokenOnDestroy();
            InitProgressLevel();
        }

        [Button]
        public void FetchBodePaths()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).TryGetComponent<LevelNodePath>(out var node))
                {
                    nodePaths.Add(node);
                }
            }
        }

        private void InitProgressLevel()
        {
            using (var listpool = ListPool<IDisposable>.Get(out var disposables))
            {
                _nodePathDict = nodePaths.ToDictionary(node => node.Level, node =>
                {
                    node.SetAvailableState(true);
                    IDisposable d = node.OnClickObservable.Select(value => (value.Level, value.Star))
                                        .Subscribe(value => OnNodeButtonClick(value.Level, value.Star));
                    disposables.Add(d);
                    return node;
                });

                _disposable = Disposable.Combine(disposables.ToArray());
            }
        }

        private void OnNodeButtonClick(int level, int star)
        {
            var popup = PlayGamePopup.Create(PlayGamePopupPath);
            popup.SetLevelBoxData(new LevelBoxData
            {
                Level = level,
                Star = star
            });
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}
