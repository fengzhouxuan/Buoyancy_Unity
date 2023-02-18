using System;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public class VoxelCollector:MonoBehaviour
    {
        private List<Transform> _voxels;
        private Transform _transform;
        public List<Transform> Voxels => _voxels;
        private void Awake()
        {
            _transform = transform;
            _voxels = new List<Transform>(_transform.childCount);
            for (int i = 0; i < _transform.childCount; i++)
            {
                var voxel = _transform.GetChild(i);
                _voxels.Add(voxel);
            }
        }
    }
}