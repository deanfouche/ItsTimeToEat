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
            double startTime = AudioSettings.dspTime + 2;
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                titleThemeLoop.PlayScheduled(startTime);
            }
            else
            {
                double introDuration = (double)titleThemeIntro.clip.samples / titleThemeIntro.clip.frequency;
                titleThemeIntro.PlayScheduled(startTime);
                titleThemeLoop.PlayScheduled(startTime + introDuration);
            }
        }
    }
}