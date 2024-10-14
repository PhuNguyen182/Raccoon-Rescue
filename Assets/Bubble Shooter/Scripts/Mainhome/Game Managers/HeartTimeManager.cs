using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleShooter.Scripts.Mainhome.GameManagers
{
    public class HeartTimeManager : MonoBehaviour
    {
        private DateTime _savedHeartTime;
        private TimeSpan _heartTimeDiff;
        private TimeSpan _offset;

        private static readonly int _maxHeart = GameDataConstants.MaxHeart;
        private static readonly int _heartCooldown = GameDataConstants.HeartCooldown;

        public TimeSpan HeartTimeDiff => _heartTimeDiff;

        public void UpdateHeartTime()
        {
            if (GameData.Instance.GetHeart() < _maxHeart)
            {
                _savedHeartTime = GameData.Instance.GetCurrentHeartTime();
                _offset = DateTime.Now.Subtract(_savedHeartTime);
                _heartTimeDiff = TimeSpan.FromSeconds(_heartCooldown).Subtract(_offset);

                if (_heartTimeDiff.TotalSeconds <= 0)
                {
                    GameData.Instance.AddHeart(1);
                    if (GameData.Instance.GetHeart() >= _maxHeart)
                    {
                        _savedHeartTime = DateTime.Now;
                        GameData.Instance.SetHeart(_maxHeart);
                        GameData.Instance.SaveHeartTime(_savedHeartTime);
                    }

                    else
                    {
                        _savedHeartTime = _savedHeartTime.AddSeconds(_heartCooldown);
                        GameData.Instance.SaveHeartTime(_savedHeartTime);
                    }
                }
            }
        }

        public void LoadHeartOnStart()
        {
            _savedHeartTime = GameData.Instance.GetCurrentHeartTime();
            TimeSpan diff = DateTime.Now.Subtract(_savedHeartTime);

            do
            {
                TimeSpan cooldown = TimeSpan.FromSeconds(_heartCooldown);
                diff = diff.Subtract(cooldown);

                if (diff.TotalSeconds > 0)
                    GameData.Instance.AddHeart(1);

                if (GameData.Instance.GetHeart() >= _maxHeart)
                {
                    _savedHeartTime = DateTime.Now;
                    GameData.Instance.SetHeart(_maxHeart);
                    GameData.Instance.SaveHeartTime(DateTime.Now);
                    break;
                }

                else
                    _savedHeartTime = _savedHeartTime.Add(TimeSpan.FromSeconds(_heartCooldown));
            } while (diff.TotalSeconds > 0);
        }
    }
}
