using Assets.Scripts.Mechanics;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{/// <summary>
 /// The MetaGameController is responsible for switching control between the high level
 /// contexts of the application, eg the Main Menu and Gameplay systems.
 /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        /// <summary>
        /// The main UI object which used for the menu.
        /// </summary>
        public MainUIController mainMenu;

        /// <summary>
        /// A list of canvas objects which are used during gameplay (when the main ui is turned off)
        /// </summary>
        public Canvas[] gamePlayCanvasii;

        /// <summary>
        /// The gameOver object which used for the end game.
        /// </summary>
        public GameOver gameOver;

        bool showMainCanvas = false;

        [SerializeField]
        FirstPersonController playerController;
        [SerializeField]
        PlayerInteraction playerInteraction;

        void OnEnable()
        {
            _ToggleMainMenu(showMainCanvas);
        }

        /// <summary>
        /// Turn the main menu on or off.
        /// </summary>
        /// <param name="showMenu"></param>
        public void ToggleMainMenu(bool showMenu)
        {
            if (this.showMainCanvas != showMenu)
            {
                _ToggleMainMenu(showMenu);

                _TogglePlayerControl(!showMenu);
            }
        }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                Time.timeScale = 0;
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
            }
            this.showMainCanvas = show;
        }

        void _TogglePlayerControl(bool hasControl)
        {
            this.playerController.cameraCanMove = hasControl;
            this.playerController.playerCanMove = hasControl;
            this.playerInteraction.playerCanInteract = hasControl;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleMainMenu(showMenu: !showMainCanvas);
            }

            if (this.showMainCanvas)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    mainMenu.SetActivePanel(0);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    mainMenu.SetActivePanel(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    mainMenu.SetActivePanel(2);
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    _ExitGame();
                }
            }
        }

        void _ExitGame()
        {
            LoadSceneAsync("TitleScreen");
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadYourAsyncScene(sceneName));
        }

        IEnumerator LoadYourAsyncScene(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                // Update a loading bar here if you have one
                yield return null;
            }
        }
    }
}

