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

    public PlayerUnit(UnitType unitType, Stats stats) : base(unitType, stats)
    {
        SetUnitType(UnitType.Player);
        Stats.Health = 100;
        Stats.Speed = 5f;
    }

    private void OnEnable()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _dashAction = _playerInput.actions["Dash"];

        _moveAction.performed += ctx => GetMoveInput(ctx);
        _dashAction.performed += _ => Dash();
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
        Debug.Log(_moveAction.ReadValue<Vector2>());
        
        Debug.Log(Keyboard.current.wKey.isPressed.ToString());
    }

    public override void Move()
    {
        //Vector2 moveDirection = GetMoveInput();
        //transform.Translate(moveDirection * (Time.deltaTime * Stats.Speed));
    }

    private Vector2 GetMoveInput(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        return _moveAction.ReadValue<Vector2>();
    }
    
    private void Dash()
    {
        Debug.Log("Dash");
    }
}
