using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Mover mover;

    public static event System.Action OnPlayerIndexSwitched;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        AssignMoverBasedOnPlayerIndex();
        OnPlayerIndexSwitched += AssignMoverBasedOnPlayerIndex;
    }

    private void OnDestroy()
    {
        OnPlayerIndexSwitched -= AssignMoverBasedOnPlayerIndex;
    }

    private void AssignMoverBasedOnPlayerIndex()
    {
        var movers = FindObjectsOfType<Mover>();
        var index = playerInput.playerIndex;
        mover = movers.FirstOrDefault(m => m.GetPlayerIndex() == index);

        if (mover == null)
            Debug.LogWarning($"No Mover found for player index {index}");
        else
            Debug.Log($"Mover assigned for player index {index}");
    }

    public void OnMove(CallbackContext context)
    {
        if (mover != null)
            mover.SetInputVector(context.ReadValue<Vector2>());
    }

    public void OnJump(CallbackContext context)
    {
        if (mover != null && context.performed)
            mover.OnJump();
    }

    public void OnSwitch(CallbackContext context)
    {
        if (mover != null && context.performed)
        {
            mover.SetInputVector(Vector2.zero);
            mover.OnSwitch();
            OnPlayerIndexSwitched?.Invoke();
        }
    }
    public void OnBark(CallbackContext context)
    {
        if (mover != null && context.performed)
        {
            if (mover.CompareTag("Dog"))
            {
                mover.Bark();
            }
        }
    }

    public void OnSprint(CallbackContext context)
    {
        if (mover == null) return;

        if (context.started) // When sprint button is pressed
        {
            mover.StartSprint();
        }
        else if (context.canceled) // When sprint button is released
        {
            mover.StopSprint();
        }
    }

    public void OnDig(CallbackContext context)
    {
        if (mover != null && context.performed)
            mover.OnDig();
    }
     public void OnGrabRope(CallbackContext context)
    {
        if (mover != null && context.performed)
        {
            mover.CheckForRope();
        }
    }

    public void OnAttack(CallbackContext context)
    {
        if (mover != null && context.performed)
        {
            if (mover.CompareTag("Cat"))
            {
                mover.Attack();

            }

        }
    }
}
