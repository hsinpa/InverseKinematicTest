using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IK {
    public class Segment : MonoBehaviour
    {

        [SerializeField]
        Vector3 _HeadPoint;

        public Vector3 HeadPoint { get { return _HeadPoint; } }

        [SerializeField]
        Vector3 _TailPoint;

        public Vector3 TailPoint { get { return _TailPoint; } }

        float _Angle;

        float _Length;

        public Segment Parent;
        public Segment Child;

        public void SetUp(Segment parent, float length)
        {
            this.Parent = parent;

            SetUp(parent.TailPoint, parent._Angle, length);
        }

        public void SetUp(Vector3 start_point, float angle, float length)
        {
            _HeadPoint = start_point;
            _TailPoint = GetEndPoint(angle, length);

            transform.localRotation = Quaternion.Euler(0, 0, angle);
            transform.position =  (_TailPoint + _HeadPoint) * 0.5f;
            Debug.Log("_HeadPoint " + _HeadPoint +" _TailPoint " + _TailPoint);

            _Angle = angle;
            _Length = length;

            transform.localScale = new Vector3(_Length, 1, 1);

            //UpateSegPos();
        }

        public void Follow()
        {
            if (Child != null) { 
                
            }

            if (Parent != null)
            {
                Parent.Follow();
            }
        }

        public void Follow(Vector2 target_point) { 
            
        }

        private void UpateSegPos() {

            //_HeadPoint = GetEndPoint(_Angle, _Length);
            Debug.Log("_TailPoint " + _HeadPoint);
        }

        private Vector3 GetEndPoint(float p_angle, float p_radius)
        {
            float dx = p_radius * Mathf.Cos(p_angle);
            float dy = p_radius * Mathf.Sin(p_angle);

            return _HeadPoint + new Vector3(dx, dy, 0);
        }


    }
}
