using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;
using System;

[IncludeInSettings(true)]
public class Entity : MonoBehaviour, Damager
{
    //public stuff
    public EntityStats _stats => stats;
    public CharacterController _characterController => characterController;

    Animator anim;
    RootTransforms rootTransforms;
    CharacterController characterController;
    EquippedObjects equippedObjects;

    [SerializeField] EntityStats stats;
    [SerializeField] GameObject model;

    public bool isEntity => true;
    public Entity entity => this;


    public bool inAimMode;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rootTransforms = GetComponentInChildren<RootTransforms>();
        characterController = GetComponent<CharacterController>();
        equippedObjects = GetComponent<EquippedObjects>();
    }

    private void Start()
    {
        if (anim == null) enabled = false;

        UnOutline();
        MovementStart();
        StartHealth();
        RotationStart();
        GravityStart();
    }

    private void Update()
    {
        MoversUpdate();
        MovementUpdate();
        RotationUpdate();
        AttackUpdate();
        GravityUpdate();
    }

    #region general combat

    #region health

    float _health;
    bool isDead;
    float health
    {
        get
        {
            return _health;
        }
        set
        {
            if (isDead) return;
            _health = value;
            if (_health <= 0f)
            {
                anim.SetTrigger("Death");
                isDead = true;
                _health = 0f;
            }
        }
    }

    void StartHealth()
    {
        health = stats.MaxHealth;
    }

    #endregion

    #region damage recieve

    public void Damage(DamageData damageData)
    {
        health -= damageData.damage;
        anim.SetFloat("Knockback", damageData.knockback);
        RecieveKnockback(damageData.source);
    }

    #region knockback
    Vector3 knockbackDirection;

    public bool isInKnockback { get; set; }

    public void RecieveKnockback(Vector3 source)
    {
        anim.SetTrigger("Knockback");
        knockbackDirection = (source - transform.position).normalized;
        knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
    }

    #endregion




    public void DamageAnimationEvent(int i)
    {

    }

    #endregion

    #region attacking
    string currentAttackName;
    bool _isAttacking;
    public bool readyToAttackTransition;
    Vector3 attackMouseTarget;

    public bool isAttacking
    {
        get
        {
            return _isAttacking;
        }
        set
        {
            _isAttacking = value;
            if (value)
            {
                readyToAttackTransition = false;
            }
        }
    }
    [SerializeField] Combo[] combos;

    public void AttackCombo(int I)
    {
        if (!readyToAttackTransition & isAttacking || isDashing) return;
        if (combos[I].hasAttackName(currentAttackName))
        {
            currentAttackName = combos[I].nextAttackName(currentAttackName);
            anim.Play(currentAttackName);
        }
        else
        {
            currentAttackName = combos[I].attackNames[0];
            anim.Play(currentAttackName);
        }
    }

    public void Attack(string attackName)
    {
        if (isDashing) return;
        anim?.Play(attackName);
        attackMouseTarget = _SceneManager.singleton.worldMousePosition;
    }

    public void OnAttackStarted()
    {
        Vector3 result = (lookTarget - transform.position);
        Debug.DrawRay(transform.position, result * 3f, Color.red, 1f);
        result = new Vector3(result.x, 0f, result.z);
        attackLookTarget = result;
    }

    Coroutine comboClearRoutine;

    public void OnAttackStopped()
    {

    }

    float isAttackingTime;
    float isNotAttackingTime;

    void AttackUpdate()
    {
        anim.SetBool("ReadyToAttackTransition", readyToAttackTransition);
        anim.SetBool("IsAttacking", isAttacking);
        
        if (isAttacking)
        {
            isNotAttackingTime = 0f;
            isAttackingTime += Time.deltaTime;
        }
        else
        {
            isAttackingTime = 0f;
            isNotAttackingTime += Time.deltaTime;
        }

        if (isNotAttackingTime > 0.1f)
        {
            currentAttackName = "";
        }
    }

    [System.Serializable]
    class Combo
    {
        public string Name;
        public string[] attackNames;
        public bool hasAttackName(string name)
        {
            return System.Array.Exists(attackNames, element => element == name);
        }
        public string nextAttackName(string name)
        {
            int index = System.Array.IndexOf(attackNames, name);
            if (index == attackNames.Length-1)
            {
                return attackNames[0];
            }
            else
            {
                return attackNames[index + 1];
            }
        }
    }

    #endregion

    #endregion

    #region general movement

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
    Vector3 movementDir => new Vector3(_horizontal, 0f, _vertical);
    Vector3 nonZeroMovementDir = Vector3.forward;
    Vector3 desiredMovementValue;
    Vector3 movementVelocity;
    bool isMoving;

    void MovementStart()
    {
        movers.Add(movement);
    }

    void MovementUpdate()
    {
        isMoving = movementDir.magnitude > 0f;
        Vector3 rootTransformInfluence = Vector3.one;

        rootTransformInfluence = rootTransforms.deltaPositionUnscaled;


        Quaternion relativeTo = Quaternion.Euler(0, 0, 0);
        if (_localTo != null)
        {
            relativeTo = Quaternion.Euler(0f, _localTo.transform.eulerAngles.y, 0f);
        }

        if (isAttacking)
        {
            Vector3 attackingInfluence = Vector3.zero;
            if (isAttacking)
            {
                attackingInfluence = relativeTo * movementDir * stats.movementSpeedWhenAttacking;
            }
            desiredMovementValue = rootTransformInfluence + attackingInfluence;

            movement.value = desiredMovementValue;
        }
        else if (isDashing)
        {
            desiredMovementValue = relativeTo * movementDirWhenDashed * rootTransformInfluence.magnitude;

            movement.value = desiredMovementValue;
        }
        else
        {
            desiredMovementValue = relativeTo * movementDir * stats.movementSpeed;

            movement.value = Vector3.SmoothDamp(movement.value, desiredMovementValue, ref movementVelocity, stats.movementSmoothTime, 1000f, Time.deltaTime);
        }

        anim?.SetFloat("Speed", movement.value.magnitude /stats.movementSpeed);

        Vector3 movementAnimValues = model.transform.InverseTransformDirection(desiredMovementValue);
        movementAnimValues.Normalize();

        Debug.DrawLine(transform.position, transform.position + model.transform.rotation * movementAnimValues);

        anim?.SetFloat("MovementX", movementAnimValues.x);
        anim?.SetFloat("MovementY", movementAnimValues.z);

        if (movementDir.magnitude > 0f)
        {
            nonZeroMovementDir = movementDir;
        }
    }

    [SerializeField] float rotationOffsetTest;

    float _horizontal;
    float _vertical;
    Transform _localTo;

    public void MoveTo(float horizontal, float vertical)
    {
        _horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        _vertical = Mathf.Clamp(vertical, -1f, 1f);
        _localTo = null;
    }

    public void MoveTo(float horizontal, float vertical, Transform localTo)
    {
        _horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        _vertical = Mathf.Clamp(vertical, -1f, 1f);
        _localTo = localTo;
    }

    #region dashing

    Vector3 movementDirWhenDashed;
    public bool isDashing { get; set; }

    public void Dash(string name)
    {
        if (isDashing) return;
        movementDirWhenDashed = nonZeroMovementDir;
        anim.Play(name);
    }

    #endregion

    #endregion

    #region gravity

    Mover gravity = new Mover();

    void GravityStart()
    {
        movers.Add(gravity);
    }

    void GravityUpdate()
    {
        gravity.value = Vector3.down * stats.GravityForce;
    }

    #endregion

    #region rotation
    Quaternion desiredRotation;
    Vector3 lookTarget;
    Vector3 attackLookTarget;

    void RotationStart()
    {
        desiredRotation = transform.rotation;
    }

    void RotationUpdate()
    {
        float damping = stats.rotationDamping;

        if (isMoving & !inAimMode)
        {
            desiredRotation = Quaternion.LookRotation(movement.value, Vector3.up);
        }
        if (inAimMode)
        {
            desiredRotation = Quaternion.LookRotation(lookTarget - transform.position, Vector3.up);
            anim.SetLayerWeight(anim.GetLayerIndex("Aimed"), 1f);
        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Aimed"), 0f);
        }

        if (isAttacking)
        {
            Vector3 forward = new Vector3(attackLookTarget.x, 0f, attackLookTarget.z);
            desiredRotation = Quaternion.LookRotation(forward, Vector3.up);
            damping = stats.attackDamping;
        }
        if (isInKnockback)
        {
            desiredRotation = Quaternion.LookRotation(knockbackDirection, Vector3.up);
        }

        var rot = Quaternion.Lerp(model.transform.rotation, desiredRotation, damping * Time.deltaTime);
        model.transform.rotation = rot;
    }

    public void SetLookTarget(Vector3 target)
    {
        lookTarget = target;
    }

    #endregion

    #endregion

    #region general visuals

    #region swinging

    public Action OnSwingStart;
    public Action OnSwingEnd;

    #endregion

    #region outline

    [SerializeField] GameObject outlineObject;

    void OutlineStart()
    {
        outlineObject.SetActive(false);
    }

    public void Outline()
    {
        if (outlineObject == null) return;
        outlineObject.SetActive(true);
    }

    public void UnOutline()
    {
        if (outlineObject == null) return;
        outlineObject.SetActive(false);
    }

    #endregion

    #endregion
}
