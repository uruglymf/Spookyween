﻿using System;
using System.Collections.Generic;
using InteractableSystem;
using SfxSystem;
using UnityEngine;

namespace PickupableSystem {
	public class PickupableResetter : Interactable {
		[SerializeField] private ParticleSystem _particles;
		private static List<Pickupable> _items = new List<Pickupable>();
		
		public static void AddPickupable(Pickupable pickupable) {
			_items.Add(pickupable);	
		}

		public static void RemovePickupable(Pickupable pickupable) {
			_items.Remove(pickupable);	
		}

		public override bool Enabled { get; protected set; } = true;
		public override string ActionName => "sands of time";
		public override InteractionType InteractionType => InteractionType.Click;
		public override InteractionKeyType KeyType => InteractionKeyType.Default;

		public override void Interact() {
			_particles.Play();
			SfxPlayer.Play(SfxType.SandsOfTime);
			foreach (Pickupable item in _items) {
				item.ResetPickupable();
			}
		}
	}
}