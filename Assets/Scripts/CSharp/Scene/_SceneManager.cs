using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt.Addons.Community;

public class _SceneManager : MonoBehaviour
{
    public static _SceneManager singleton;
    DefaultInputActions inputActions;

    private void Awake()
    {
        singleton = this;
        inputActions = new DefaultInputActions();
    }

    private void Start()
    {
        inputActions.Player.Move.performed += ctx => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Move { value = ctx.ReadValue<Vector2>() });
        inputActions.Player.Move.canceled += ctx => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Move { value = ctx.ReadValue<Vector2>() });
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
