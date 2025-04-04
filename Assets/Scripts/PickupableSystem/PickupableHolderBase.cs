﻿using InteractableSystem;
using UnityEngine;

namespace PickupableSystem {
	public abstract class PickupableHolderBase : Interactable {
		[SerializeField] private Transform _dropOrientaion;
		[SerializeField] private Transform _container;

		public Vector3 DropOrientation => _dropOrientaion.forward;

		public Pickupable CurrentPickupable { get; private set; }

		public virtual void Pickup(Pickupable pickupable, Vector3[] customPath = null, bool useCustomOrientation = false) {
			DropCurrentPickupable();
			CurrentPickupable = pickupable;
			CurrentPickupable.OnPickup(_container, customPath, useCustomOrientation);
		}

		public bool TryClaimPickupable(PickupableType type, out Pickupable pickupable) {
			if (CurrentPickupable == null || type != CurrentPickupable.Type) {
				pickupable = null;
				return false;
			}
			
			pickupable = CurrentPickupable;
			DropCurrentPickupable();
			return pickupable != null;
		}

		public virtual void DropCurrentPickupable() {
			if (!CurrentPickupable) return;
			CurrentPickupable.OnDrop(DropOrientation);
			CurrentPickupable = null;
		}
	}
}