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
        /// The GameOver object which used for the end game.
        /// </summary>
        public GameOver gameOver;

        /// <summary>
        /// The Victory object which used for the completing the level.
        /// </summary>
        public Victory victory;

        bool _showMainCanvas = false;
        bool _showVictoryCanvas = false;
        bool _showGameOverCanvas = false;

        [SerializeField]
        FirstPersonController playerController;
        [SerializeField]
        PlayerInteraction playerInteraction;

        void OnEnable()
        {
            _ToggleGameCanvas(_showMainCanvas, mainMenu.gameObject);
            _ToggleGameCanvas(_showVictoryCanvas, gameOver.gameObject);
            _ToggleGameCanvas(_showGameOverCanvas, victory.gameObject);
        }

        /// <summary>
        /// Turn the main menu on or off.
        /// </summary>
        /// <param name="showMenu"></param>
        public void ToggleMainMenu(bool showMenu)
        {
            if (this._showMainCanvas != showMenu)
            {
                _ToggleGameCanvas(showMenu, mainMenu.gameObject);
                this._showMainCanvas = showMenu;

                _TogglePlayerControl(!showMenu);
            }
        }

        public void ToggleGameOver(bool showGameOverScreen)
        {
            if (this._showGameOverCanvas != showGameOverScreen)
            {
                _ToggleGameCanvas(showGameOverScreen, gameOver.gameObject);
                this._showGameOverCanvas = showGameOverScreen;

                _TogglePlayerControl(!showGameOverScreen);
            }
        }

        public void ToggleVictory(bool showVictoryScreen)
        {
            if (this._showVictoryCanvas != showVictoryScreen)
            {
                _ToggleGameCanvas(showVictoryScreen, victory.gameObject);
                this._showVictoryCanvas = showVictoryScreen;

                _TogglePlayerControl(!showVictoryScreen);
            }
        }

        void _TogglePlayerControl(bool hasControl)
        {
            this.playerController.cameraCanMove = hasControl;
            this.playerController.playerCanMove = hasControl;
            this.playerInteraction.playerCanInteract = hasControl;
        }

        void _ToggleGameCanvas(bool show, GameObject canvas)
        {
            if (show)
            {
                Time.timeScale = 0;
                canvas.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Time.timeScale = 1;
                canvas.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void Update()
        {
            if (!gameOver.isGameOver && !victory.isVictory)
            {
                if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
                {
                    ToggleMainMenu(showMenu: !_showMainCanvas);
                }
            }

            if (this._showMainCanvas)
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

            if (this._showGameOverCanvas)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _RestartGame();
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    _ExitGame();
                }
            }
        }

        void _RestartGame()
        {
            LoadSceneAsync("Game");
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

