using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MutationIndicator : MonoBehaviour
    {
        public Image indicatorObj;
        public Text indicatorNameObj;
        private MutationUIElement mutationUIObj;

        private class MutationUIElement
        {
            public string mutationName;
            public float timeToExpire;
            public float totalTime;

            public MutationUIElement(string name, float timeToExpire, float totalTime)
            {
                this.mutationName = name;
                this.timeToExpire = timeToExpire;
                this.totalTime = totalTime;
            }
        }

        public void AddMutationUIElement(string name, float timeToExpire, float totalTime)
        {
            mutationUIObj = new MutationUIElement(name, timeToExpire, totalTime);
        }

        public void RemoveMutationUIElement()
        {
            Destroy(this.gameObject);
        }

        public 

        void Start()
        {
            this.indicatorObj = this.GetComponentInChildren<Image>();
            this.indicatorNameObj = this.GetComponentInChildren<Text>();
            this.indicatorNameObj.text = this.mutationUIObj.mutationName;
        }

        // Update is called once per frame
        void Update()
        {
            if (mutationUIObj != null)
            {
                mutationUIObj.timeToExpire -= Time.deltaTime;

                if (mutationUIObj.timeToExpire <= 0)
                {
                    // End mutation
                }
                else
                {
                    float fillAmount = mutationUIObj.timeToExpire / mutationUIObj.totalTime;
                    this.indicatorObj.fillAmount = fillAmount;
                }
            }
        }

        void _UpdateMutationDisplay()
        {
            this.indicatorObj = this.GetComponentInChildren<Image>();
            this.indicatorNameObj = this.GetComponentInChildren<Text>();
            this.indicatorNameObj.text = this.mutationUIObj.mutationName;
        }
    }
}