﻿using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class ItNightScreen : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _label;

        [SerializeField]
        private Image _image;
        
        public IEnumerator AwaitAnimation(Action action = null)
        {
            _label.color = _label.color.SetA(0);
            yield return new WaitForSeconds(0.5f);
            
            var sequrnce = DOTween.Sequence();
            yield return sequrnce.Append(_label.DOColor(Color.white, 1f)).WaitForCompletion();
            yield return new WaitForSeconds(2f);
            sequrnce = DOTween.Sequence();
            yield return sequrnce
                .Append(_image.DOColor(Color.clear, 1f))
                .Insert(0, _label.DOColor(Color.clear, 1f)).WaitForCompletion();
            gameObject.SetActive(false);
            
            action?.Invoke();
        }

        public void Show(Action action = null)
        {
            gameObject.SetActive(true);
            StartCoroutine(AwaitAnimation(action));
        }
    }
}