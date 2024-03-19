using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaccoonRescue.Scripts.Gameplay.Common.UpdateHandlerPattern
{
    public class UpdateHandlerManager : Singleton<UpdateHandlerManager>, IDisposable
    {
        private HashSet<IUpdateHandler> _updateHandlers = new();

        private void Update()
        {
            foreach (IUpdateHandler updateHandler in _updateHandlers)
            {
                updateHandler.OnUpdate(Time.deltaTime);
            }
        }

        public void Add(IUpdateHandler updateHandler)
        {
            if (!_updateHandlers.Contains(updateHandler))
            {
                _updateHandlers.Add(updateHandler);
            }
        }

        public void Remove(IUpdateHandler updateHandler)
        {
            if (_updateHandlers.Contains(updateHandler))
            {
                _updateHandlers.Remove(updateHandler);
            }
        }

        public void Dispose()
        {
            _updateHandlers.Clear();
        }
    }
}
