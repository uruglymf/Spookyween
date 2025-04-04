﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using InteractableSystem;
using UnityEngine;

namespace Alchemy {
	public class Potion : Interactable {
		[SerializeField] private MeshRenderer _mesh;
		[SerializeField] private ParticleSystem _particles;
		[SerializeField] private PotionConfig _potionConfig;

		private PotionType _potionType;

		public override bool Enabled { get; protected set; } = true;
		public override string ActionName => AlchemyData.GetPotionName(_potionType);

		public override InteractionType InteractionType => InteractionType.Click;
		public override InteractionKeyType KeyType => InteractionKeyType.Default;

		public override void Interact() {
			//AlchemyData.OnDrinkPotion?.Invoke(_potionType);
			PotionEffectController.INSTANCE?.Drink(_potionType);
			Destroy(gameObject);
		}

		public void Init(PotionType type) {
			Vector3 initScale = transform.localScale;
			transform.localScale = Vector3.zero;
			transform.DOScale(initScale, 0.3f).SetEase(Ease.OutBack);

			_potionType = type;
			PotionData data = _potionConfig.GetPotionData(type);
			if (data == null) return;
			_mesh.material = data.Material;
			var main = _particles.main;
			main.startColor = data.ParticleColor;
		}
	}
}
