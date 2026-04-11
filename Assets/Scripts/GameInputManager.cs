using UnityEngine;
using System.Collections.Generic;

public enum InputActionType
{
    LiftLeftPlatform,
    LiftRightPlatform,
    QuitGame,
    MoveLeft,
    MoveRight,
    MoveUp,
    MoveDown
}

[System.Serializable]
public class InputBinding
{
    public InputActionType actionType;
    public List<KeyCode> keys = new List<KeyCode>();
}

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance { get; private set; }

    public List<InputBinding> bindings = new List<InputBinding>();

    private void Awake()
    {
        Instance = this;
    }

    public bool IsActionPressed(InputActionType actionType)
    {
        foreach (var binding in bindings)
        {
            if (binding.actionType == actionType)
            {
                foreach (var key in binding.keys)
                {
                    if (Input.GetKey(key)) return true;
                }
            }
        }
        return false;
    }

    public bool IsActionDown(InputActionType actionType)
    {
        foreach (var binding in bindings)
        {
            if (binding.actionType == actionType)
            {
                foreach (var key in binding.keys)
                {
                    if (Input.GetKeyDown(key)) return true;
                }
            }
        }
        return false;
    }
}