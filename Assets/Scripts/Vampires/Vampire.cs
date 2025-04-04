﻿using System;
using DG.Tweening;
using PickupableSystem;
using SfxSystem;
using UnityEngine;

namespace Vampires {
	public class Vampire : PickupableHolder {
		[SerializeField] private VampireEyeball[] _eyes;
		[SerializeField] private ParticleSystem _blood;
		[SerializeField] private ParticleSystem _flame;

		public override string ActionName => "vampire";
		public bool Dead { get; private set; }
		
		private VampirePuzzle _puzzle;
		
		public void Init(VampirePuzzle puzzle) {
			_puzzle = puzzle;
		}
				
		public override void Pickup(Pickupable pickupable, Vector3[] customPath = null, bool useCustomOrientation = false) {
			if (!_puzzle.IsActive) {
				SfxPlayer.Play(SfxType.VampireReject);
				return;
			}
			
			Vector3[] path = {
				Vector3.zero, 
				Vector3.forward * 5f,
				Vector3.forward
			};
			
			base.Pickup(pickupable, path, useCustomOrientation);
			Enabled = false;
			SfxPlayer.Play(SfxType.VampireDie);
			DOTween.Sequence().InsertCallback(0.25f, () => {
				Die();
				_puzzle.OnKillVampire();
			});
		}
		
		private void Die() {
			Dead = true;
			_blood.Play();
			_flame.gameObject.SetActive(true);
			foreach (VampireEyeball eye in _eyes) eye.SetDead();
		}

		private void Awake() {
			foreach (VampireEyeball eye in _eyes) eye.SetDefault();
		}
	}
}