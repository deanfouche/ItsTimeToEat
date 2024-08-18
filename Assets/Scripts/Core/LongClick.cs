using Assets.Scripts.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Core
{
    public class LongClick : MonoBehaviour
    {
        public float clickDuration = 0.3f;
        public UnityEvent onShortClick;
        public UnityEvent onLongClick;
        public UnityEvent onLongClickRelease;
        public PlayerInteraction playerInteraction;

        bool _clicking = false;
        bool _isLongClick = false;
        float _totalDownTime = 0;

        void Update()
        {
            if (playerInteraction.playerCanInteract)
            {
                // Detect the first click
                if (Input.GetMouseButtonDown(0))
                {
                    _totalDownTime = 0;
                    _clicking = true;
                }

                // If a first click detected, and still clicking,
                // measure the total click time, and fire an event
                // if we exceed the duration specified
                if (_clicking && Input.GetMouseButton(0))
                {
                    _totalDownTime += Time.deltaTime;

                    if (!_isLongClick && _totalDownTime >= clickDuration)
                    {
                        _isLongClick = true;
                        onLongClick.Invoke();
                    }
                }

                // If a first click detected, and we release before the
                // duration, do nothing, just cancel the click
                if (_clicking && Input.GetMouseButtonUp(0))
                {
                    if (_isLongClick)
                    {
                        _clicking = false;
                        _isLongClick = false;
                        onLongClickRelease.Invoke();
                    } else
                    {
                        _clicking = false;
                        onShortClick.Invoke();
                    }
                }
            }
        }
    }
}