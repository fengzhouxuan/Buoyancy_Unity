using System;
using System.Collections.Generic;
using UnityEngine;

namespace ML
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlotsamObj : MonoBehaviour
    {
        private VoxelCollector _voxelCollector;
        private Rigidbody _rigidbody;
        private bool _inWater;
        private Water _water;
        [Min(0.1f)]
        [SerializeField] private float _voxelSize = 0.2f;

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
                    var voxelPosition = voxel.transform.position;
                    var voxelVelocity = _rigidbody.GetPointVelocity(voxelPosition);
                    var voxelVelocityNor = voxelVelocity.normalized;
                    var voxelVelocitySqrMag = voxelVelocity.sqrMagnitude;
                    float depth = _water.GetWaterLine(voxel.position) - voxelPosition.y + _voxelSize;
                    if (depth > 0)
                    {
                        //计算在水中的阻力
                        _rigidbody.AddForceAtPosition(-1 * 0.5f * voxelVelocitySqrMag * _water.Drag * voxelVelocityNor,
                            voxel.position, ForceMode.Acceleration);
                        //计算浮力
                        //计算水下体积 TODO:未来会计算真实体积，目前先只算高度
                        float volumeInWater = Mathf.Clamp(depth, 0, _voxelSize);
                        _rigidbody.AddForceAtPosition((Vector3.up * volumeInWater * buyancy) * -Physics.gravity.y,
                            voxel.position,
                            ForceMode.Acceleration);
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