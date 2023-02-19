using System;
using UnityEngine;

namespace ML
{
    public class Water : MonoBehaviour
    {
        [SerializeField]
        [Range(0,10)]
        private float _buoyancy = 4;
        
        [SerializeField]
        [Min(0)]
        private float _drag = 0.5f;
        [SerializeField]
        private float _speed=1;
        [SerializeField]
        [Min(0)]
        private float _wave=1;
        [SerializeField]
        [Min(0)]
        private float _height=0.4f;
        private Transform _transform;
        private float _waterLine;
        [SerializeField]
        private Material _material;
        
        private static readonly int WavesSpeedPropertyID = Shader.PropertyToID("_WavesSpeed");
        private static readonly int WavesHeightPropertyID = Shader.PropertyToID("_WavesHeight");
        private static readonly int WavesFrequencyPropertyID = Shader.PropertyToID("_WavesFrequency");
        public float Buoyancy => _buoyancy;
        public float WaterLine => _waterLine;
        public float Drag => _drag;

        public float Height
        {
            set
            {
                _height = value;
                RefreshWaterMat();
            }
        }

        public float Wave
        {
            set
            {
                _wave = value;
                RefreshWaterMat();
            }
        }

        public float Speed
        {
            set
            {
                _speed = value;
                RefreshWaterMat();
            }
        }
        public float GetWaterLine(Vector3 worldPosition)
        {
            float x = worldPosition.x;
            float z = worldPosition.z;
            var y1 = Mathf.Sin((x*_wave+Time.timeSinceLevelLoad*_speed))*_height;
            var y2 = Mathf.Sin((z*_wave+Time.timeSinceLevelLoad*_speed))*_height;
            return (y1+y2)+_waterLine;
        }
        private void Awake()
        {
            _transform = transform;
            _waterLine = _transform.position.y;
        }

        private void RefreshWaterMat()
        {
            _material.SetFloat(WavesSpeedPropertyID,_speed);
            _material.SetFloat(WavesHeightPropertyID,_height);
            _material.SetFloat(WavesFrequencyPropertyID,_wave);
        }
        private void OnValidate()
        {
            RefreshWaterMat();
        }

        private void OnDrawGizmos()
        {
            int count = 50;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    float x = -10 + i*20f/count;
                    float z = -10 + j*20f/count;
                    var y1 = Mathf.Sin((x*_wave+Time.timeSinceLevelLoad*_speed))*_height;
                    var y2 = Mathf.Sin((z*_wave+Time.timeSinceLevelLoad*_speed))*_height;
                    Vector3 pos = new Vector3(x, (y1+y2)+_waterLine, z);
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(pos,0.1f);
                }
            }
        }
    }
}
