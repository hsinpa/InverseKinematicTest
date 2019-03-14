using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Kinematics;

namespace Hsinpa.Creature {
	public class Snake : MonoBehaviour {

	#region  Inspector parameter
		[Header("Components")]
		[SerializeField]
		private List<Joints> joints = new List<Joints>();

		[SerializeField]
		private int length;

		[Header("Prefabs")]
		[SerializeField]
		private GameObject head;

		[SerializeField]
		private GameObject body;

		[SerializeField]
		private GameObject tail;
	#endregion

		private void Start() {
			CreateSegment();
		}

		public void CreateSegment() {
			if (body != null) {
				GameObject gSegment = Instantiate(body, Vector3.zero, Quaternion.identity);
				PairSegToJoint(gSegment);
			}
		}

		private void PairSegToJoint(GameObject p_segment) {
			var joint = new Joints(p_segment);
			joints.Add(joint);
		}

	}
}
