using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Works with BOTH the new Input System and the old Input Manager.
public static class InputHelper
{
    public static bool LeftPressed()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        return k != null && (k.aKey.wasPressedThisFrame || k.leftArrowKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
#endif
    }

    public static bool RightPressed()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        return k != null && (k.dKey.wasPressedThisFrame || k.rightArrowKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
#endif
    }

    public static bool JumpPressed()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        return k != null && (k.spaceKey.wasPressedThisFrame || k.wKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
#endif
    }

    public static bool EscapePressed()
    {
#if ENABLE_INPUT_SYSTEM
        Keyboard k = Keyboard.current;
        return k != null && k.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }
}
