using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeMenuManager : MonoBehaviour
{
    public GameObject menuObject;
    public InputActionReference menuButtonAction;
    public bool menuButtonPressed = false;
    public GameObject rightHandObject;
    private MenuRaycaster menuRaycaster;

    // Start is called before the first frame update
    void Start()
    {
        // Enable the input for menuButtonAction
        menuButtonAction.action.Enable();

        // Set menuObject as not active
        menuObject.SetActive(false);

        // Get the script MenuRaycaster on rightHandObject
        menuRaycaster = rightHandObject.GetComponent<MenuRaycaster>();

        // Disable the script MenuRaycaster on rightHandObject
        menuRaycaster.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Update menuButtonPressed
        menuButtonPressed = menuButtonAction.action.triggered;
        if (menuButtonPressed)
        {
            // If the menu is not active, activate it, and enable the script MenuRaycaster on rightHandObject
            if (!menuObject.activeSelf)
            {
                menuObject.SetActive(true);
                menuRaycaster.enabled = true;
            }
            // If the menu is active, deactivate it, and disable the script MenuRaycaster on rightHandObject
            else
            {
                menuObject.SetActive(false);
                // Call the function ClearLineRenderer() on the script MenuRaycaster on rightHandObject
                MenuRaycaster menuRaycaster = rightHandObject.GetComponent<MenuRaycaster>();
                menuRaycaster.ClearLineRenderer();
                menuRaycaster.enabled = false;

            }
        }
    }
}
