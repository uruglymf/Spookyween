﻿using DG.Tweening;
using InteractableSystem;
using SfxSystem;
using UnityEngine;

namespace PickupableSystem {
	public abstract class Pickupable : Interactable {
		[SerializeField] private bool _resettable;
		private Rigidbody _rigidbody;
		private Collider _collider;

		// to avoid wall clipping
		private LayerMask _pickupableLayer => LayerMask.NameToLayer("Pickupable");
		private LayerMask _defaultLayer => LayerMask.NameToLayer("Default");
			
		public abstract string Name { get; }
		public abstract PickupableType Type { get; }

		private Vector3 _initialPosition;
		private Quaternion _initialRotation;
		
		private Transform _parent;
		protected virtual Vector3 _customOrientation { get; }
		protected virtual Vector3 _offset { get; }
		
		public override bool Enabled { get; protected set; } = true; 
		public override string ActionName => Name;
		public override InteractionType InteractionType => InteractionType.Click;
		public override InteractionKeyType KeyType => InteractionKeyType.Default;

		private void Awake() {
			_initialPosition = transform.position;
			_initialRotation = transform.rotation;
			_rigidbody = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();
			_rigidbody.isKinematic = true;
		}

		protected override void OnEnable() {
			base.OnEnable();
			if(_resettable) PickupableResetter.AddPickupable(this);
		}

		protected override void OnDisable() {
			base.OnDisable();
			if(_resettable) PickupableResetter.RemovePickupable(this);
		}

		public override void Interact() {
			PickupableHolderPlayer.INSTANCE.Pickup(this, useCustomOrientation: true);
		}

		public virtual void OnPickup(Transform parent, Vector3[] customPath = null, bool useCustomOrientation = false) {
			SfxPlayer.Play(SfxType.Pickup);
			Enabled = false;
			_rigidbody.velocity *= 0f;
			_rigidbody.angularVelocity *= 0f;
			_rigidbody.isKinematic = true;
			_collider.enabled = false;
			transform.SetParent(parent, true);

			Vector3[] path = customPath ?? new [] {
				Vector3.zero + _offset, 
				Vector3.up,
				Vector3.up / 2f 
			};

			if (customPath != null) customPath[0] += _offset;
			
			transform.DOKill();
			transform.DOLocalPath(path, 1f, PathType.CubicBezier).SetEase(Ease.OutBack);
			transform.DOLocalRotate(useCustomOrientation ? _customOrientation : Vector3.zero, 0.3f);
		}

		public void OnDrop(Vector3 dropOrientation) {
			SfxPlayer.Play(SfxType.Drop);
			Enabled = true;
			_collider.enabled = true;
			_rigidbody.isKinematic = false;
			_rigidbody.AddForce(dropOrientation * 5f, ForceMode.Impulse);
			transform.DOKill();
			transform.SetParent(null);
		}

		public void SetPickupableLayer() {
			SetLayer(transform, _pickupableLayer);
		}

		public void ResetLayer() {
			SetLayer(transform, _defaultLayer);
		}

		private void SetLayer(Transform obj, LayerMask layer) {
			obj.gameObject.layer = layer;
			for (int i = 0; i < obj.childCount; i++) {
				SetLayer(obj.GetChild(i), layer);
			}
		}
		
		public void ResetPickupable() {
			if(!Enabled) return;
			_rigidbody.velocity *= 0;
			_rigidbody.angularVelocity *= 0f;
			_rigidbody.isKinematic = true;
			transform.position = _initialPosition;
			transform.rotation = _initialRotation;
		}
	}

	public enum PickupableType {
		None,
		Ingredient,
		Potion,
		Skeleton,
		Pumpkin,
		Aspen,
		Torch
	}
}