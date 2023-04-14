using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniatureViewScript : MonoBehaviour
{
    public GameObject vrRig1;
    public GameObject vrRig2;
    private VRInputController input;
    private bool isAlternate = false;



    private void Awake()
    {
        input = GetComponent<VRInputController>();
    }

    void Start()
    {
        // Disable the second VR rig by default
        vrRig2.SetActive(false);
    }

    void Update()
    {
        if (input.LeftPrimary_Button_Pressed)
        {
            isAlternate = !isAlternate;

            // Enable/disable the appropriate VR rigs
            vrRig1.SetActive(!isAlternate);
            vrRig2.SetActive(isAlternate);
        }
    }
}
