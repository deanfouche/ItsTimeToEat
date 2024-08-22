using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class TitleSceneController : MonoBehaviour
    {
        /// <summary>
        /// The main UI object which used for the menu.
        /// </summary>
        public TitleUIController titleMenu;

        public AudioSource titleThemeIntro;
        public AudioSource titleThemeLoop;

        void Start()
        {
            double introDuration = (double)titleThemeIntro.clip.samples / titleThemeIntro.clip.frequency;
            double startTime = AudioSettings.dspTime + 2;
            titleThemeIntro.PlayScheduled(startTime);
            //titleThemeIntro.SetScheduledEndTime(startTime + introDuration - 2);
            titleThemeLoop.PlayScheduled(startTime + introDuration);
        }
    }
}