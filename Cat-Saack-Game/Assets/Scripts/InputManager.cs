using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages input & switches between menu and overworld modes
public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    private PlayerTopDown playerTopDown;
    private PlayerInput playerInput;
    private InputAction selectAction;
    
    private OverworldInteractable cursorOverworld;
    private MenuButton cursorButton;
    private Stack<GameObject> menuStack = new Stack<GameObject>();

    string actionMapName;
    private Vector2 moveDirection;
    
    // input select cooldown
    private bool selectCooldownActive = false;
    public float selectCooldown = 0.1f;

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
        RegisterSelf();
    }

    public static void RegisterSelf()
    {
        Instance.playerInput = Instance.gameObject.GetComponent<PlayerInput>();
        Instance.playerTopDown = FindObjectOfType<PlayerTopDown>();
        Instance.actionMapName = Instance.playerInput.currentActionMap.name;
    }

    // When the Move action is performed
    public void OnMove(InputAction.CallbackContext aContext)
    {

        if (actionMapName == "Player")
        {
            if (playerTopDown != null)
            {
                playerTopDown.Move(aContext.ReadValue<Vector2>());
            }
        }

        if(actionMapName == "Menu")
        {
            if (aContext.phase == InputActionPhase.Performed)
            {
                if (cursorButton != null)
                {
                    //Debug.Log("moved in the menu system"); //DEBUG ONLY

                    moveDirection = aContext.ReadValue<Vector2>().normalized;
                    //disable the old button's hover status
                    cursorButton.SetIsHighlighted(false); 
                    cursorButton = cursorButton.GetButtonInDirection(moveDirection);
                    //set the new button to hover status
                    cursorButton.SetIsHighlighted(true);

                    //Debug.Log("Cursor button name: "+cursorButton.gameObject.name); //DEBUG ONLY
                }
            }
        }

    }

    // When the Select action is performed
    public void OnSelect(InputAction.CallbackContext aContext)
    {

        if (aContext.phase == InputActionPhase.Performed)
        {
            if (!selectCooldownActive)
            {
                StartCoroutine(WaitForSelectCooldown());
                if (actionMapName == "Player")
                {
                    if (playerTopDown is PlayerOverworld playerOverworld)
                    {
                        //Debug.Log("OnSelect pressed in Overworld mode");
                        playerOverworld.OnSelect();
                    }
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

    }

    public void OnEscape(InputAction.CallbackContext aContext)
    {
        if (aContext.phase == InputActionPhase.Performed)
        {
            if (GameManager.canPause)
            {
                if (actionMapName == "Player")
                {
                    GameManager.Pause();
                }

                if (actionMapName == "Menu")
                {
                    UIManager.PopMenuPanel();
                }
            }
        }
    }

    private IEnumerator WaitForSelectCooldown()
    {
        selectCooldownActive = true;
        yield return new WaitForSeconds(selectCooldown);
        selectCooldownActive = false;
    }

    // Switch to Overworld input mode
    public static void SwitchInputModeOverworld()
    {
        SetCursorButton(null);
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
        // Unhighlight the previous button if there is one
        if (Instance.cursorButton != null)
        {
            Instance.cursorButton.SetIsHighlighted(false);
        }

        // Set the new cursor button and highlight it
        Instance.cursorButton = aButton;
        if (Instance.cursorButton != null)
        {
            Instance.cursorButton.SetIsHighlighted(true);
        }
    }
}
