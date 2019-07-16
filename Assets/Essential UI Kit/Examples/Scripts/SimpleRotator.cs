using UnityEngine;
using System.Collections;

namespace MA.Examples{
	public class SimpleRotator : MonoBehaviour {
		[Range(-10, 10f)]
		[SerializeField]
		private float RotationModifier = 0.5f;
		[SerializeField]
		private Vector3 RotationAxis = Vector3.up;

		void Update () {
			transform.RotateAround(transform.position, RotationAxis, RotationModifier);
		}
	}
}
