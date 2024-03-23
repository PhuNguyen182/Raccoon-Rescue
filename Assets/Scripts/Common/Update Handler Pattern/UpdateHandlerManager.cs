using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Common.UpdateHandlerPattern
{
    public class UpdateHandlerManager : Singleton<UpdateHandlerManager>, IDisposable
    {
        private HashSet<IUpdateHandler> _updateHandlers;
        private HashSet<IFixedUpdateHandler> _fixedUpdateHandlers;
        private HashSet<ILateUpdateHandler> _lateUpdateHandlers;

        protected override void OnAwake()
        {
            _updateHandlers = new();
            _fixedUpdateHandlers = new();
            _lateUpdateHandlers = new();
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

        private void LateUpdate()
        {
            foreach (ILateUpdateHandler lateUpdateHandler in _lateUpdateHandlers)
            {
                lateUpdateHandler.OnLateUpdate(Time.deltaTime);
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

        public void AddLateUpdateBehaviour(ILateUpdateHandler handler)
        {
            if (!_lateUpdateHandlers.Contains(handler))
            {
                _lateUpdateHandlers.Add(handler);
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

        public void RemoveLateUpdateBehaviour(ILateUpdateHandler handler)
        {
            if (_lateUpdateHandlers.Contains(handler))
            {
                _lateUpdateHandlers.Remove(handler);
            }
        }

        public void Dispose()
        {
            _updateHandlers.Clear();
            _fixedUpdateHandlers.Clear();
        }
    }
}
