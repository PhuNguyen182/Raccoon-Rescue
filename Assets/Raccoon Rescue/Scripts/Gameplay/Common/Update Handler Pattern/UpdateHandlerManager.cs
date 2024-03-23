using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaccoonRescue.Scripts.Gameplay.Common.UpdateHandlerPattern
{
    public class UpdateHandlerManager : Singleton<UpdateHandlerManager>, IDisposable
    {
        private HashSet<IUpdateHandler> _updateHandlers;
        private HashSet<IFixedUpdateHandler> _fixedUpdateHandlers;

        protected override void OnAwake()
        {
            _updateHandlers = new();
            _fixedUpdateHandlers = new();
        }

        private void Update()
        {
            foreach (IUpdateHandler updateHandler in _updateHandlers)
            {
                updateHandler.OnUpdate(Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            foreach (IFixedUpdateHandler fixedUpdateHandler in _fixedUpdateHandlers)
            {
                fixedUpdateHandler.OnFixedUpdate();
            }
        }

        public void AddUpdateBehaviour(IUpdateHandler handler)
        {
            if (!_updateHandlers.Contains(handler))
            {
                _updateHandlers.Add(handler);
            }
        }

        public void AddFixedUpdateBehaviour(IFixedUpdateHandler handler)
        {
            if (!_fixedUpdateHandlers.Contains(handler))
            {
                _fixedUpdateHandlers.Add(handler);
            }
        }

        public void RemoveUpdateBehaviour(IUpdateHandler handler)
        {
            if (_updateHandlers.Contains(handler))
            {
                _updateHandlers.Remove(handler);
            }
        }

        public void RemoveFixedUpdateBehaviour(IFixedUpdateHandler handler)
        {
            if (_fixedUpdateHandlers.Contains(handler))
            {
                _fixedUpdateHandlers.Remove(handler);
            }
        }

        public void Dispose()
        {
            _updateHandlers.Clear();
            _fixedUpdateHandlers.Clear();
        }
    }
}
