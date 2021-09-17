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

    public Vector3 mousePosition { get; private set; }
    public Vector3 worldMousePosition { get; private set; }
    public Vector2 lookDelta { get; private set; }

    public Entity[] allEntities { get; set; }
    public Entity previouslyOutlinedEntity { get; private set; }

    [SerializeField] float selectionRadius;

    private void Awake()
    {
        singleton = this;
        inputActions = new DefaultInputActions();
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player");
        allEntities = FindObjectsOfType<Entity>(true);
    }

    private void Start()
    {
        #region input actions assignment
        inputActions.Player.Move.performed += ctx => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Move { value = ctx.ReadValue<Vector2>() });
        inputActions.Player.Move.canceled += ctx => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Move { value = ctx.ReadValue<Vector2>() });
        inputActions.Player.Attack.performed += _ => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Attack());
        inputActions.Player.Attack.performed += _ => Select();
        inputActions.Player.Dash.performed += _ => DefinedEvent.TriggerGlobal(new DefinedEvents.Input.Dash());
        inputActions.Player.MousePosition.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        inputActions.Player.Delta.performed += ctx => lookDelta = ctx.ReadValue<Vector2>();
        inputActions.Player.Delta.canceled += ctx => lookDelta = ctx.ReadValue<Vector2>();
        #endregion
    }

    void Outline(Entity entity)
    {
        previouslyOutlinedEntity?.UnOutline();
        previouslyOutlinedEntity = entity;
        entity.Outline();
    }

    void UnUutline()
    {
        previouslyOutlinedEntity?.UnOutline();
        previouslyOutlinedEntity = null;
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

    void Select()
    {
        Collider[] entityColliders = Physics.OverlapSphere(worldMousePosition, selectionRadius, LayerMask.GetMask("Entity"));

        if (entityColliders.Length == 1)
        {
            if (entityColliders[0].gameObject == player)
            {
                UnUutline();
                return;
            }
        }

        if (entityColliders.Length <= 0)
        {
            UnUutline();
            return;
        }

        Collider closestCollider = null;
        float minDistance = float.MaxValue;

        foreach (var collider in entityColliders)
        {
            if (collider.gameObject == player) continue;

            float distance = Vector3.Distance(worldMousePosition, collider.ClosestPoint(worldMousePosition));
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCollider = collider;
            }
        }

        Debug.Log(closestCollider);
        Entity entitySelected = closestCollider.gameObject.GetComponent<Entity>();
        Debug.Log(entitySelected);
        Outline(entitySelected);
    }

    private void Update()
    {
        MousePositionToWorldPosition(mousePosition);
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
