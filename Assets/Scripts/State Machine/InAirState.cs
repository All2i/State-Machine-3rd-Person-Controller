using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InAirState : BaseState
{
    //timeout deltatime
    private float fallTimeoutDelta;
    private float minAirTime = 0.1f; // Minimum time to stay in air, this is needed because sometimes the character goes back to GroundState before jumping because it still touching the ground layer
    private float airTime;

    public InAirState(StateMachine currentContext, StateFactory stateFactory) : base(currentContext, stateFactory)
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("In Air State Enter");
        // if we are not grounded, do not jump
        ctx.Input.jump = false;
    }

    public override void UpdateState()
    {
        Debug.Log("In Air State Update");
        ctx.Move();
        ctx.RotatePlayerToMoveDirection();

        // fall timeout
        if (fallTimeoutDelta >= 0.0f)
        {
            fallTimeoutDelta -= Time.deltaTime;
        }
        else
        {
            // update animator if using character
            if (ctx.HasAnimator)
            {
                ctx.Animator.SetBool(ctx.AnimIDFreeFall, true);
            }
        }
        airTime += Time.deltaTime; // Increment air time
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // Debug.Log("In Air State Exit");
        // reset the fall timeout timer
        fallTimeoutDelta = ctx.FallTimeout;

        // update animator if using character
        if (ctx.HasAnimator)
        {
            ctx.Animator.SetBool(ctx.AnimIDJump, false);
            ctx.Animator.SetBool(ctx.AnimIDFreeFall, false);
        }
    }

    public override void CheckSwitchStates()
    {
        if (ctx.Grounded && airTime > minAirTime)
        {
            SwitchState(factory.GroundedState());
        }
    }

    public override void InitializeSubState()
    {
    }
}
