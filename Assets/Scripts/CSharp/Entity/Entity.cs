using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;

[IncludeInSettings(true)]
public class Entity : MonoBehaviour
{
    Animator anim;
    RootTransforms rootTransforms;
    CharacterController characterController;

    [SerializeField] EntityStats stats;
    [SerializeField] GameObject model;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rootTransforms = GetComponentInChildren<RootTransforms>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        MovementStart();
    }

    private void Update()
    {
        MoversUpdate();
        MovementUpdate();
        RotationUpdate();
    }

    #region movers

    List<Mover> movers = new List<Mover>();

    class Mover
    {
        public Vector3 value;
    }

    void MoversUpdate()
    {
        Vector3 finalMove = new Vector3();
        for (int i = 0; i < movers.Count; i++)
        {
            finalMove += movers[i].value;
        }
        characterController.Move(finalMove * Time.deltaTime);
    }

    #endregion

    #region movement
    Mover movement = new Mover();
    Vector3 movementDir;
    bool isMoving;

    void MovementStart()
    {
        movers.Add(movement);
    }

    void MovementUpdate()
    {
        movement.value = movementDir * rootTransforms.deltaPositionUnscaled.magnitude * stats.movementSpeed;
        isMoving = movementDir.magnitude > 0f;
        anim.SetBool("Run", isMoving);
        
    }

    public void MoveTo(float horizontal, float vertical)
    {
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = Mathf.Clamp(vertical, -1f, 1f);
        movementDir = new Vector3(horizontal, 0f, vertical);
    }

    #endregion

    #region rotation
    Quaternion desiredRotation;

    void RotationUpdate()
    {
        if (isMoving)
        {
            desiredRotation = Quaternion.LookRotation(movementDir, Vector3.up);
        }
        model.transform.rotation = Quaternion.Lerp(model.transform.rotation, desiredRotation, stats.rotationDamping * Time.deltaTime);
    }

    #endregion
}
