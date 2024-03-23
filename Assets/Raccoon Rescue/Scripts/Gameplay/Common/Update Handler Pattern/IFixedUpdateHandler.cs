using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaccoonRescue.Scripts.Gameplay.Common.UpdateHandlerPattern
{
    public interface IFixedUpdateHandler
    {
        public void OnFixedUpdate();
    }
}
