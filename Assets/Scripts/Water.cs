using UnityEngine;

namespace ML
{
    public class Water : MonoBehaviour
    {
        [SerializeField]
        private float _buoyancy = 4;

        [SerializeField] private float _drag = 0.5f;
        [SerializeField]
        private float _speed=1;
        [SerializeField]
        private float _wave=1;
        [SerializeField]
        private float _height=0.4f;
        
        private Transform _transform;
        private float _waterLine;
        public float Buoyancy => _buoyancy;
        public float WaterLine => _waterLine;
        public float Drag => _drag;

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
