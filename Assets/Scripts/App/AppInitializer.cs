using Scripts.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.App
{
    public class AppInitializer : Singleton<AppInitializer>, IService
    {
        protected override void OnAwake()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitializeService.Instance.Initialize();
        }
    }
}
