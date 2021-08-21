using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt.Addons.Community;
using Ludiq;

[IncludeInSettings(true)]
public class _SceneManager : MonoBehaviour
{
    public static _SceneManager singleton;
    DefaultInputActions inputActions;

    public Camera mainCamera { get; private set; }
    public GameObject player { get; private set; }

    public Vector3 worldMousePosition { get; private set; }
    public Vector2 lookDelta { get; private set; }

    private void Awake()
    {
        singleton = this;
        inputActions = new DefaultInputActions();
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        inputActions.Player.Move.performed += ctx => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Move { value = ctx.ReadValue<Vector2>() });
        inputActions.Player.Move.canceled += ctx => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Move { value = ctx.ReadValue<Vector2>() });
        inputActions.Player.Attack.performed += _ => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Attack());
        inputActions.Player.Dash.performed += _ => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Dash());
        inputActions.Player.MousePosition.performed += ctx => MousePositionToWorldPosition(ctx.ReadValue<Vector2>());
        inputActions.Player.Delta.performed += ctx => lookDelta = ctx.ReadValue<Vector2>();
        inputActions.Player.Delta.canceled += ctx => lookDelta = ctx.ReadValue<Vector2>();
    }

    void MousePositionToWorldPosition(Vector2 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay((Vector3)mousePosition + Vector3.forward * 4);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);
        Plane plane = new Plane(Vector3.up, player.transform.position);
        float enter;
        if (plane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            worldMousePosition = hitPoint;
        }
        
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
