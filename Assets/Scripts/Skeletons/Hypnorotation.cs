﻿using UnityEngine;

namespace Skeletons {
	public class Hypnorotation : MonoBehaviour {
		private void Update() {
			transform.Rotate(Vector3.forward, -250f * Time.deltaTime);
		}
	}
}