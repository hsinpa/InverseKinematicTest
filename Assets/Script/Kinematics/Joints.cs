using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hsinpa.Kinematics
{
	public class Joints {

		private Joints parent;
		private Joints child;

		public float angle;
		public float radius;

		public Vector2 endPoint;

		private GameObject spriteObject;

		public Joints(GameObject spriteObject) {
			this.spriteObject = spriteObject;
			this.endPoint = GetEndPoint(this.angle, this.radius);
		}

		public void UpdateChild(Joints child) {
			this.child = child;
		}

		public void SetHeadLocation(Vector3 p_headPos) {
			
		}

		private Vector2 GetEndPoint(float p_angle, float p_radius) {
			float dx = p_radius * Mathf.Cos(p_angle);
			float dy = p_radius * Mathf.Sin(p_angle);

			return spriteObject.transform.position + new Vector3(dx, dy, 0);
		}

		void OnDrawGizmosSelected()
		{
			// Draw a yellow sphere at the transform's position
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(GetEndPoint(angle, radius), 0.5f);
		}

	}	
}

