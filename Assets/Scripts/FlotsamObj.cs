using System;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    public class FlotsamObj : MonoBehaviour
    {
        private VoxelCollector _voxelCollector;
        private Rigidbody _rigidbody;
        private bool _inWater;
        private Water _water;
        [SerializeField]
        private float _voxelSize=0.2f;
        private void Awake()
        {
            _voxelCollector = GetComponentInChildren<VoxelCollector>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_inWater)
            {
                int voxelCount = _voxelCollector.Voxels.Count;
                float buyancy = _water.Buoyancy / voxelCount;
                for (int i = 0; i < voxelCount; i++)
                {
                    var voxel = _voxelCollector.Voxels[i];
                    float deep = _water.GetWaterLine(voxel.position) - voxel.transform.position.y+_voxelSize;
                    if (deep > 0)
                    {
                        _rigidbody.AddForceAtPosition((Vector3.up * deep * buyancy) * -Physics.gravity.y, voxel.position,
                            ForceMode.Acceleration);
                        // _rigidbody.AddForceAtPosition(Vector3.up*(WaterLift / _voxels.Count)*deep*-_gravity,v.position,ForceMode.Acceleration);                    }
                    }
                }
                    
            }
            _inWater = false;
        }

        private void OnTriggerStay(Collider other)
        {
            var water = other.GetComponent<Water>();
            if (water)
            {
                _inWater = true;
                _water = water;
            }
        }
    }
}