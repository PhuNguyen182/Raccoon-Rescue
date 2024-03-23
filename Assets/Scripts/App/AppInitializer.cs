using Scripts.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.App
{
    public class AppInitializer : Singleton<AppInitializer>, IService
    {
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            InitializeService.Instance.Initialize();
        }
    }
}
