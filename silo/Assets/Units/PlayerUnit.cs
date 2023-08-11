using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerUnit : BaseUnit
{
    private PlayerInput _playerInput;
    private CircleCollider2D _playerCollider;
    private SpriteRenderer _playerSpriteRenderer;
    
    public GameObject basicBullet;
    
    private InputAction _moveAction;
    private InputAction _dashAction;
    private InputAction _attackAction;

    private Vector2 _moveDirection;
    private Vector2 _velocity;
    private Vector2 _attackDirection;

    private Vector3 _bulletOffset;
    private Vector3 _bulletSpawnPosition;

    private float _playerRadius;
    
    private Quaternion _attackQuaternion;
    
    private bool _dashOnCooldown = false;
    private bool _attackOnCooldown = false;
    private bool _isAttacking = false;

    public PlayerUnit(UnitType unitType, Stats stats) : base(unitType, stats)
    {
        
    }

    private void OnEnable()
    {
        SetUnitType(UnitType.Player);
        SetStats(100, 5f, 0, 75, 70, 15, 3f, .25f);
        
        _playerInput = GetComponent<PlayerInput>();
        _playerCollider = GetComponent<CircleCollider2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();

        basicBullet = Resources.Load<GameObject>("Bullets/BulletPrefabs/BasicBullet");
        
        _moveAction = _playerInput.actions["Move"];
        _dashAction = _playerInput.actions["Dash"];
        _attackAction = _playerInput.actions["Attack"];
        //_playerInput.enabled = true;

        _moveAction.performed += ctx => OnMoveInput(ctx);
        _moveAction.canceled += ctx => ZeroInput(ctx);
        _dashAction.performed += ctx => Dash(ctx);
        _attackAction.performed += ctx => OnAttackInput(ctx);
        _attackAction.canceled += ctx => StopAttack(ctx);
        
        _playerRadius = _playerCollider.radius;
        _bulletOffset = new Vector3(_playerRadius +.5f, _playerRadius +.5f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        HandleCollision();
        if (_isAttacking && !_attackOnCooldown)
        {
            Attack();
        }
    }

    public override void Move()
    {
        if (_moveDirection != Vector2.zero)
        {
            _velocity = Vector2.MoveTowards(_velocity, Stats.Speed * _moveDirection, Stats.WalkAcceleration * Time.deltaTime);
        }
        else
        {
            _velocity = Vector2.MoveTowards(_velocity, Stats.Speed * _moveDirection, Stats.WalkDeceleration * Time.deltaTime);
        }

        transform.Translate(_velocity * Time.deltaTime);
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
    
    private void ZeroInput(InputAction.CallbackContext context)
    {
        _moveDirection = Vector2.zero;
    }
    
    private void Dash(InputAction.CallbackContext context)
    {
        
        if (!_dashOnCooldown)
        {
            _dashOnCooldown = true;
            StartCoroutine(DashCooldown());
            _velocity += _moveDirection * Stats.DashPower;
        }
    }
    
    private void OnAttackInput(InputAction.CallbackContext context)
    { 
        _attackDirection = context.ReadValue<Vector2>();
        _isAttacking = true;
    }

    private void Attack()
    {
        float angle = Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg;
        _attackQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        
        _bulletOffset = _bulletOffset * _attackDirection;
        _bulletSpawnPosition = transform.position + _bulletOffset;
        
        Instantiate(basicBullet, _bulletSpawnPosition, _attackQuaternion);
        _attackOnCooldown = true;
        StartCoroutine(AttackCooldown());
        _bulletOffset = new Vector3(_playerRadius +.5f, _playerRadius +.5f, 0f);
    }
    
    private void StopAttack(InputAction.CallbackContext context)
    {
        _isAttacking = false;
    }
    
    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(Stats.DashCooldown);
        _dashOnCooldown = false;
        _playerSpriteRenderer.color = Color.green;
        yield return new WaitForSeconds(.2f);
        _playerSpriteRenderer.color = Color.white;
    }
    
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(Stats.AttackCooldown);
        _attackOnCooldown = false;
    }

    private void HandleCollision()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _playerCollider.radius);
        foreach(Collider2D hit in hits)
        {
            if (hit == _playerCollider)
                continue;
            
            ColliderDistance2D colliderDistance = hit.Distance(_playerCollider);
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
            }
        }
    }

}
