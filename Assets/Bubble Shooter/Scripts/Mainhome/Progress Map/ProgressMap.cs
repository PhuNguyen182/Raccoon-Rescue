using R3;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Cysharp.Threading.Tasks;
using BubbleShooter.Scripts.Mainhome.PopupBoxes.PlayGamePopup;
using BubbleShooter.Scripts.Common.PlayDatas;
using Sirenix.OdinInspector;

namespace BubbleShooter.Scripts.Mainhome.ProgressMaps
{
    public class ProgressMap : MonoBehaviour
    {
        [SerializeField] private int minLevel = 1;
        [SerializeField] private int maxLevel = 100;
        [SerializeField] private float moveDuration = 1f;

        [Header("Progress Map")]
        [SerializeField] private PathFollower pathFollower;
        [SerializeField] private Transform nodeContainer;
        [SerializeField] private List<LevelNodePath> nodePaths;

        private const string PlayGamePopupPath = "Popups/Play Game Popup";

        private Dictionary<int, LevelNodePath> _nodePathDict;

        private IDisposable _disposable;

        public int MinLevel => minLevel;
        public int MaxLevel => maxLevel;

        private void Awake()
        {
            InitProgressLevel();
        }

        [Button]
        public void FetchBodePaths()
        {
            nodePaths.Clear();

            for (int i = 0; i < nodeContainer.childCount; i++)
            {
                if(nodeContainer.GetChild(i).TryGetComponent<LevelNodePath>(out var node))
                {
                    nodePaths.Add(node);
                }
            }
        }

        public LevelNodePath GetLevelNode(int level)
        {
            return _nodePathDict[level];
        }

        public async UniTask Move(int startIndex, int endIndex)
        {
            await pathFollower.Move(startIndex, endIndex, moveDuration);
        }

        public void Translate(int level)
        {
            pathFollower.SetPositionImediately(level);
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
