using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages input & switches between menu and overworld modes
public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    private PlayerOverworld playerOverworld;
    private PlayerInput playerInput;
    private InputAction selectAction;
    
    private OverworldInteractable cursorOverworld;
    private MenuButton cursorButton;
    private Stack<GameObject> menuStack = new Stack<GameObject>();

    string actionMapName;
    private Vector2 moveDirection;
    

    bool currentlyOverworld = false; // FOR DEBUG ONLY

    void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        playerOverworld = FindObjectOfType<PlayerOverworld>();
        actionMapName = playerInput.currentActionMap.name;
    }

    void Update() // FOR DEBUG ONLY, SWITCHES INPUT MODE WHEN PRESSING Y
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            if(currentlyOverworld)
            {
                SwitchInputModeMenu();
                currentlyOverworld = false;
            }
            else
            {
                SwitchInputModeOverworld();
                currentlyOverworld = true;
            }
        }

    }

    // When the Move action is performed
    public void OnMove(InputAction.CallbackContext aContext)
    {

        if (actionMapName == "Player")
        {
            if (playerOverworld != null)
            {
                playerOverworld.Move(aContext.ReadValue<Vector2>());
            }
        }

        if(actionMapName == "Menu")
        {
            if (aContext.phase == InputActionPhase.Performed)
            {
                if (cursorButton != null)
                {
                    Debug.Log("moved in the menu system");
                    moveDirection = aContext.ReadValue<Vector2>().normalized;
                    //disable the old button's hover status
                    cursorButton.SetIsHighlighted(false); 
                    cursorButton = cursorButton.GetButtonInDirection(moveDirection);
                    //set the new button to hover status
                    cursorButton.SetIsHighlighted(true);
                }
            }
        }

    }

    // When the Select action is performed
    public void OnSelect(InputAction.CallbackContext aContext)
    {

        if (aContext.phase == InputActionPhase.Performed)
        {
            if (actionMapName == "Player")
            {
                Debug.Log("OnSelect pressed in Overworld mode");
                playerOverworld.OnSelect();
            }

            if (actionMapName == "Menu")
            {
                if (cursorButton != null)
                {
                    cursorButton.OnSelect();
                }
            }
        }

    }

    // Switch to Overworld input mode
    public static void SwitchInputModeOverworld()
    {
        Instance.playerInput.SwitchCurrentActionMap("Player");
        Instance.actionMapName = Instance.playerInput.currentActionMap.name;
        if (Instance.cursorButton != null)
        {
            Instance.cursorButton.SetIsHighlighted(false);
        }
        Debug.Log("Switched action map to Player (overworld)");
    }

    // Switch to Menu UI input mode
    public static void SwitchInputModeMenu()
    {
        Instance.playerInput.SwitchCurrentActionMap("Menu");
        Instance.actionMapName = Instance.playerInput.currentActionMap.name;
        if (Instance.cursorButton == null)
        {
            Instance.cursorButton = FindObjectOfType<MenuButton>();
        }
        if (Instance.cursorButton != null)
        {
            Instance.cursorButton.SetIsHighlighted(true);
        }
        Debug.Log("Switched action map to Menu");
    }

    public static void SetCursorButton(MenuButton aButton)
    {
        if (aButton != null)
        {
            Instance.cursorButton = aButton;
        }
        else
        {
            Debug.LogWarning("Tried to assign cursorButton when given parameter is null");
        }
    }

    // public static void PushMenuToStack(GameObject aObject)
    // {
    //     Instance.menuStack.Push(aObject);
    // }

    // public static void PopMenuFromStack()
    // {
    //     if (Instance.menuStack.Count > 0)
    //     {
    //         Instance.menuStack.Pop();
    //     }
    //     if (Instance.menuStack.Count == 0)
    //     {
    //         // check if the current scene should be unloaded
    //             // if so unload the scene
    //     }
    // }

    
}
