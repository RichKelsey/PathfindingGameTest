using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUnit : BaseUnit
{
    private PlayerInput _playerInput;
    
    private InputAction _moveAction;
    private InputAction _dashAction;

    private Vector2 _moveDirection;

    public PlayerUnit(UnitType unitType, Stats stats) : base(unitType, stats)
    {
        
    }

    private void OnEnable()
    {
        SetUnitType(UnitType.Player);
        Stats.Health = 100;
        Stats.Speed = 5f;
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _dashAction = _playerInput.actions["Dash"];
        //_playerInput.enabled = true;

        _moveAction.performed += ctx => GetMoveInput(ctx);
        _moveAction.canceled += ctx => ClearInput(ctx);
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
    }

    public override void Move()
    {
        Stats.Speed = 10f;
        transform.Translate(new Vector3(_moveDirection.x, _moveDirection.y, 0) * (Stats.Speed * Time.deltaTime));
    }

    private void GetMoveInput(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
    
    private void ClearInput(InputAction.CallbackContext context)
    {
        _moveDirection = Vector2.zero;
    }
    
    public void Dash(InputAction.CallbackContext context)
    {
        Debug.Log("Dash");
    }
}
