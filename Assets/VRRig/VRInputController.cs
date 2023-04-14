using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class VRInputController : MonoBehaviour
{
    // Publics are usually prefaced with a capital letter.
    public float RightPrimary_Button_PressThreshold = 0.8f;
    public Vector2 LeftJoystick;
    public Vector2 RightJoystick;
    public float RightTrigger;
    public float RightPrimary_Button;
    public float LeftPrimary_Button;
    public bool RightPrimary_Button_Pressed => previous_RightPrimary_Button < RightPrimary_Button_PressThreshold && RightPrimary_Button > RightPrimary_Button_PressThreshold;
    public bool LeftPrimary_Button_Pressed => previous_LeftPrimary_Button < RightPrimary_Button_PressThreshold && LeftPrimary_Button > RightPrimary_Button_PressThreshold;
    private VRInputActions actions;
    private float previous_RightPrimary_Button;
    private float previous_LeftPrimary_Button;

    // This is called ONLY in the editor when you modify any public
    // fields.
    private void OnValidate()
    {
        // Set the *length* of the joystick vector to never exceed 1.
        LeftJoystick = Vector3.ClampMagnitude(LeftJoystick, 1);
        RightJoystick = Vector3.ClampMagnitude(RightJoystick, 1);
        RightTrigger = Mathf.Clamp01(RightTrigger);
        RightPrimary_Button = Mathf.Clamp01(RightPrimary_Button);
        LeftPrimary_Button = Mathf.Clamp01(LeftPrimary_Button);
    }

    private void Awake()
    {
        actions = new VRInputActions();

        // If you don't call this, you won't be able to read input.
        // (Why is this not enabled by default? Beats me, ask Unity.)
        actions.Enable();
    }

    private void Update()
    {
        XRHMD hmd = InputSystem.GetDevice<XRHMD>();

        if (hmd != null)
        {
            LeftJoystick = actions.Default.LeftJoystick.ReadValue<Vector2>();
            RightJoystick = actions.Default.RightJoystick.ReadValue<Vector2>();
            RightTrigger = actions.Default.RightTrigger.ReadValue<float>();
            RightPrimary_Button = actions.Default.RightPrimary_Button.ReadValue<float>();
            LeftPrimary_Button = actions.Default.LeftPrimary_Button.ReadValue<float>();
        }

        if (RightPrimary_Button_Pressed)
            Debug.Log($"Right press: {RightPrimary_Button_Pressed}");
    }

    private void LateUpdate()
    {
        previous_RightPrimary_Button = RightPrimary_Button;
        previous_LeftPrimary_Button = LeftPrimary_Button;
    }
}
