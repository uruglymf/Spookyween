﻿using Alchemy;
using DG.Tweening;
using PickupableSystem;
using UnityEngine;

namespace Wispers {
	public class WispPuzzle : PotionEffectReceiver {
		[SerializeField] private Wisp[] _wispers;
		[SerializeField] private ParticleSystem _targetParticles;
		[SerializeField] private Pickupable _target;

		private Wisp _currentWisp;
		private bool _passed;
		
		private void Awake() {
			for (int i = 0; i < _wispers.Length; i++) {
				_wispers[i].Init(i, OnTrigger);
			}
		}

		private void OnTrigger(int index) {
			_currentWisp?.Disable();
			
			if (index + 1 >= _wispers.Length) {
				_passed = true;
				_targetParticles.Play();
				DOTween.Sequence().InsertCallback(0.1f, ()=> {
					_target.gameObject.SetActive(true);
					_target.GetComponent<Rigidbody>().isKinematic = false; // ouch
				});
				PotionEffectController.INSTANCE.End();
				return;
			}

			_currentWisp = _wispers[index + 1];
			_currentWisp.Enable();
		}

		protected override PotionType _potionType => PotionType.GhostBreath;
		
		protected override void OnPotionDrinkAction() {
			if(_passed)	return;
			
			if(_currentWisp) _currentWisp.Enable();
			else OnTrigger(-1);
		}

		protected override void OnPotionEndAction() {
			if (_passed) return;
			foreach (Wisp wisp in _wispers) {
				wisp.Disable();
			}
		}
	}
}