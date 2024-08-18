using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Assets.Scripts.Core
{
    public class DoubleClick : MonoBehaviour
    {
        public float DoubleClickInterval = 0.5f;
        public UnityEvent OnDoubleClick;

        float secondClickTimeout = -1;

        void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (secondClickTimeout < 0)
                {
                    // This is the first click, calculate the timeout
                    secondClickTimeout = Time.time + DoubleClickInterval;
                }
                else
                {
                    // This is the second click, is it within the interval 
                    if (Time.time < secondClickTimeout)
                    {
                        // Invoke the event
                        OnDoubleClick.Invoke();

                        // Reset the timeout
                        secondClickTimeout = -1;
                    }
                }

            }

            // If we wait too long for a second click, just cancel the double click
            if (secondClickTimeout > 0 && Time.time >= secondClickTimeout)
            {
                secondClickTimeout = -1;
            }

        }
    }
}