using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleShooter.Scripts.Common.Interfaces;

namespace BubbleShooter.Scripts.Gameplay.Models
{
    public class BallClusterModel : IDisposable
    {
        public bool IsCeilAttached;
        public List<IGridCell> Cluster;

        public BallClusterModel()
        {
            IsCeilAttached = false;
            Cluster = new();
        }

        public void Dispose()
        {
            IsCeilAttached = false;
            Cluster.Clear();
        }
    }
}
