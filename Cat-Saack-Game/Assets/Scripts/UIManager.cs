using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private MenuPanel pauseMenu;

    private Stack<MenuPanel> menuStack = new Stack<MenuPanel>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void PushMenuPanel(MenuPanel aPanel)
    {
        Instance.menuStack.Push(aPanel);
        aPanel.OpenMenuPanel();

        Instance.StartCoroutine(Instance.LateSetInputMode());
    }

    // wait a frame
    IEnumerator LateSetInputMode()
    {
        yield return new WaitForEndOfFrame();
        // input settings
        InputManager.SwitchInputModeMenu();
        InputManager.SetCursorButton(PeekMenuPanel().GetDefaultButton());
    }

    public static MenuPanel PopMenuPanel()
    {
        if (Instance.menuStack.Count > 0)
        {
            MenuPanel poppedMenu = Instance.menuStack.Pop();
            poppedMenu.CloseMenuPanel();

            // if just popped the last menu in the stack close menu mode
            if (Instance.menuStack.Count == 0)
            {
                Instance.CloseMenu();
            }
            // if still elements in the stack set cursor inside the next menu
            else 
            {
                // return settings to last menu panel
                MenuPanel nextMenu = Instance.menuStack.Peek();
                InputManager.SetCursorButton(nextMenu.GetDefaultButton());
            }

            return poppedMenu;
        }
        else
        {
            Instance.CloseMenu();
            return null;
        }
    }

    public static void PopMenuPanelNoReturn()
    {
        PopMenuPanel();
    }

    public static MenuPanel PeekMenuPanel()
    {
        return Instance.menuStack.Peek();
    }

    void CloseMenu()
    {
        menuStack.Clear();
        // input settings
        // set input mode to the previous input mode different from the current. For now just using InputModeOverworld
        InputManager.SwitchInputModeOverworld();
        InputManager.SetCursorButton(null);
        GameManager.Unpause();
    }

    public static void OpenPauseMenu()
    {
        if (Instance.pauseMenu != null)
        {
            PushMenuPanel(Instance.pauseMenu);
        }
    }

    public static void SetPauseMenu(MenuPanel aPauseMenu)
    {
        Instance.pauseMenu = aPauseMenu;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        menuStack.Clear();
    }
}
