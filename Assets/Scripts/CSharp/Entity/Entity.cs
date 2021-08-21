using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludiq;

[IncludeInSettings(true)]
public class Entity : MonoBehaviour
{
    //public stuff
    public EntityStats _stats => stats;

    Animator anim;
    RootTransforms rootTransforms;
    CharacterController characterController;
    SimpleDamagerHitbox damagerHitbox;

    [SerializeField] EntityStats stats;
    [SerializeField] GameObject model;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rootTransforms = GetComponentInChildren<RootTransforms>();
        characterController = GetComponent<CharacterController>();
        damagerHitbox = GetComponentInChildren<SimpleDamagerHitbox>();
    }

    private void Start()
    {
        MovementStart();
        StartHealth();
        RotationStart();
    }

    private void Update()
    {
        MoversUpdate();
        MovementUpdate();
        RotationUpdate();
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
    Vector3 knockbackDirection;

    public void RecieveDamage(float damage)
    {
        health -= damage;
        Debug.Log("Entity " + name + " revieved " + damage.ToString() + " damage.");
    }

    #region knockback

    public bool isInKnockback { get; set; }

    public void RecieveKnockback(Vector3 source)
    {
        anim.Play("Knockback");
        knockbackDirection = (source - transform.position).normalized;
        knockbackDirection = new Vector3(knockbackDirection.x, 0, knockbackDirection.z);
    }

    #endregion




    public void DamageAnimationEvent(int i)
    {
        damagerHitbox.DamageEntitiesInside(2f);
    }

    #endregion

    #region attacking
    string currentAttackName;
    bool _isAttacking;
    public bool readyToAttackTransition;
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
    }

    [System.Serializable]
    class Combo
    {
        public string Name;
        public string[] attackNames;
        public bool hasAttackName(string name)
        {
            for (int i = 0; i < attackNames.Length; i++)
            {
                if ( attackNames[i] == name)
                {
                    return true;
                }
            }
            return false;
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
    bool isMoving;

    void MovementStart()
    {
        movers.Add(movement);
    }

    void MovementUpdate()
    {
        isMoving = movementDir.magnitude > 0f;
        Vector3 rootTransformInfluence = Vector3.one;
        if (rootTransforms != null)
        {
            rootTransformInfluence = rootTransforms.deltaPositionUnscaled;
        }

        Quaternion relativeTo = Quaternion.Euler(0, 0, 0);
        if (_localTo != null)
        {
            relativeTo = Quaternion.Euler(0f, _localTo.transform.eulerAngles.y, 0f);
        }

        if (isMoving & !isAttacking)
        {
            movement.value = relativeTo * movementDir * rootTransformInfluence.magnitude * stats.movementSpeed;
        }
        else if(rootTransforms != null) 
        {
            Vector3 attackingInfluence = Vector3.zero;
            if (isAttacking)
            {
                attackingInfluence = relativeTo * movementDir * stats.movementSpeedWhenAttacking;
            }
            movement.value = rootTransformInfluence + attackingInfluence;
        }
        anim?.SetBool("Run", isMoving);
    }

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

    public bool isDashing { get; set; }

    public void Dash(string name)
    {
        if (isDashing) return;
        anim.Play(name);
    }

    #endregion

    #endregion

    #region rotation
    Quaternion desiredRotation;
    Vector3 lookTarget;

    void RotationStart()
    {
        desiredRotation = transform.rotation;
    }

    void RotationUpdate()
    { 
        if (isMoving)
        {
            desiredRotation = Quaternion.LookRotation(movement.value, Vector3.up);
        }
        if (isAttacking)
        {
            Vector3 forward = new Vector3(lookTarget.x, transform.position.y, lookTarget.z) - transform.position;
            desiredRotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
        }
        if (isInKnockback)
        {
            desiredRotation = Quaternion.LookRotation(knockbackDirection, Vector3.up);
        }

        var rot = Quaternion.Lerp(model.transform.rotation, desiredRotation, stats.rotationDamping * Time.deltaTime);
        model.transform.rotation = rot;
    }

    public void SetLookTarget(Vector3 target)
    {
        lookTarget = target;
    }

    #endregion

    #endregion

    #region general visuals

    #region outline

    [SerializeField] GameObject outlineObject;

    void OutlineStart()
    {
        outlineObject.SetActive(false);
    }

    public void Outline()
    {
        outlineObject.SetActive(true);
    }

    public void UnOutline()
    {
        outlineObject.SetActive(false);
    }

    #endregion

    #endregion
}
