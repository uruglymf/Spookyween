﻿using System;
using TMPro;
using UnityEngine;

namespace Ui {
	public class InteractionUi : MonoBehaviour {
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private GameObject _actionKeyContainer;
		[SerializeField] private TextMeshProUGUI _actionKey;
		[SerializeField] private TextMeshProUGUI _actionName;

		private const float _clearTime = 0.1f;
		private float _currentClearTime;
		
		private static Action<string, string> _updateUi;

		public static void Invoke(string actionKey, string actionName) {
			_updateUi?.Invoke(actionKey, actionName);
		}

		private void Awake() => Clear();
		private void OnEnable() => _updateUi += UpdateUi;
		private void OnDisable() => _updateUi -= UpdateUi;

		private void UpdateUi(string actionKey, string actionName) {
			_actionKeyContainer.SetActive(!string.IsNullOrEmpty(actionKey));
			_actionKey.text = actionKey;
			_actionName.text = actionName;
			_canvasGroup.alpha = 1f;
			_currentClearTime = Time.time + _clearTime;
		}

		private void Clear() {
			_canvasGroup.alpha = 0f;
		}
		
		private void Update() {
			if (Time.time < _currentClearTime) return;
			Clear();
		}
	}
}