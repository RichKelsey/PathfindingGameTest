using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUnit : BaseUnit
{
    private PlayerInput _playerInput;
    
    private CircleCollider2D _playerCollider;
    
    private InputAction _moveAction;
    private InputAction _dashAction;

    private Vector2 _moveDirection;
    private Vector2 _velocity;
    
    private bool _dashOnCooldown = false;

    public PlayerUnit(UnitType unitType, Stats stats) : base(unitType, stats)
    {
        
    }

    private void OnEnable()
    {
        SetUnitType(UnitType.Player);
        SetStats(100, 5f, 0, 75, 70, 15, 3f);
        _playerInput = GetComponent<PlayerInput>();
        _playerCollider = GetComponent<CircleCollider2D>();

        _moveAction = _playerInput.actions["Move"];
        _dashAction = _playerInput.actions["Dash"];
        //_playerInput.enabled = true;

        _moveAction.performed += ctx => OnMoveInput(ctx);
        _moveAction.canceled += ctx => ZeroInput(ctx);
        _dashAction.performed += ctx => Dash(ctx);
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
            StartCoroutine(DashCooldown());
            _velocity += _moveDirection * Stats.DashPower;
        }
    }
    
    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(Stats.DashCooldown);
        _dashOnCooldown = false;
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
