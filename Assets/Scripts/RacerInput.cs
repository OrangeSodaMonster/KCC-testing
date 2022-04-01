using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct RacerInputs
{
    public Vector2 directionInput;
    public bool braking;
    public bool fireTriggered;
    public bool fireHolding;
    public bool jump1Triggered;
    public bool jump1Holding;
    public bool jump2Triggered;
    public bool jump2Holding;

    public bool basicProjectile;
    public bool shield;
    public bool boost;
    public bool aoeAttack;
}

public class RacerInput : MonoBehaviour
{
    RacerInputs inputs;
    RacerCharacterController racerController;

    bool untriggerJump1;
    bool untriggerJump2;

    private void Awake()
    {
        racerController = GetComponentInParent<RacerCharacterController>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        inputs.directionInput = context.ReadValue<Vector2>();
    }

    public void Jump1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputs.jump1Triggered = true;
            inputs.jump1Holding = true;
        }
        if (context.canceled)
        {
            inputs.jump1Holding = false;
        }
    }

    public void Jump2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputs.jump2Triggered = true;
            inputs.jump2Holding = true;
        }
        if (context.canceled)
        {
            inputs.jump2Holding = false;
        }
    }

    public void Brake(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputs.braking = true;
        }
        if (context.canceled)
        {
            inputs.braking = false;
        }
    }

    private void Update()
    {
        CleanTriggered(ref inputs.jump1Triggered, ref untriggerJump1);
        CleanTriggered(ref inputs.jump2Triggered, ref untriggerJump2);

        HandleInputs();
    }

    private void CleanTriggered(ref bool triggered, ref bool untrigger)
    {
        if (untrigger)
        {
            triggered = false;
            untrigger = false;
        }
        if (triggered) untrigger = true;
    }

    void HandleInputs()
    { 
        racerController.GetInputs(inputs);
    }
}