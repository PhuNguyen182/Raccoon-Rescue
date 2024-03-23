using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Common.UpdateHandlerPattern
{
    public interface IFixedUpdateHandler
    {
        public void OnFixedUpdate();
    }
}
