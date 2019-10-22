using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IK {
    public class IK_Manager : MonoBehaviour
    {

        [SerializeField]
        private Transform _Target;

        [SerializeField]
        private Segment SegmentPrefab;


        private void Start()
        {
            CreateSegment(1);


        }

        private void CreateSegment(int num) {
            GameObject emptyObject = new GameObject();
            emptyObject.name = "Tenacles";
            emptyObject.transform.localPosition = Vector3.zero;

            Segment root = Instantiate<Segment>(SegmentPrefab);
            root.SetUp(Vector3.zero, angle : -5, length : 2);
            root.transform.SetParent(emptyObject.transform);
            num--;

            Segment lastSegment = root;
            for (int i = 0; i < num; i++) {
                Segment childSegment = Instantiate<Segment>(SegmentPrefab);
                childSegment.SetUp(lastSegment, length : 2);
                childSegment.transform.SetParent(emptyObject.transform);

                lastSegment.Child = childSegment;
                lastSegment = childSegment;
            }

            SegmentHolder holder = new SegmentHolder();
            holder.RootSegment = root;
            holder.LastSegment = lastSegment;
        }



        private struct SegmentHolder {
            public Segment RootSegment;
            public Segment LastSegment;
        }
    }
}
