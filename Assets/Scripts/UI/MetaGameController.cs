using UnityEngine;

/// <summary>
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

    void OnEnable()
    {
        _ToggleMainMenu(showMainCanvas);
    }

    /// <summary>
    /// Turn the main menu on or off.
    /// </summary>
    /// <param name="show"></param>
    public void ToggleMainMenu(bool show)
    {
        if (this.showMainCanvas != show)
        {
            _ToggleMainMenu(show);
        }
    }

    void _ToggleMainMenu(bool show)
    {
        if (show)
        {
            Time.timeScale = 0;
            mainMenu.gameObject.SetActive(true);
            foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            mainMenu.gameObject.SetActive(false);
            foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
        }
        this.showMainCanvas = show;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMainMenu(show: !showMainCanvas);
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
                Application.Quit();
            }
        }
    }
}
