using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MutationIndicator : MonoBehaviour
    {
        public GameObject indicatorObj;
        public GameObject indicatorNameObj;

        public float timeToExpire;
        public float totalTime;

        public MutationIndicator(float timeToExpire, float totalTime)
        {
            this.timeToExpire = timeToExpire;
            this.totalTime = totalTime;
        }

        // Update is called once per frame
        void Update()
        {
            Image indicator = this.GetComponentInChildren<Image>();
            Debug.Log(indicator.name);
            var testValue = timeToExpire - Time.deltaTime / totalTime;
            indicator.fillAmount = testValue;
            Debug.Log($"Fill amount = {testValue}, time = {Time.deltaTime}");
            Debug.Log(indicator.fillAmount);
        }
    }
}