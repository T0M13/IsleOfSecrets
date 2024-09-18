using UnityEngine;
using UnityEngine.WSA;

public class IdleState : AIState
{
    private Vector2 idleDuration;
    private float idleTime = 0f;
    private float maxIdleTime = 3f;
    private float chanceToPatrol = 0.5f;


    public void EnterState(AIAgent agent)
    {
        agent.AiAnimator.SetBool("Idle", true);
        idleTime = 0f;
    }

    public void UpdateState(AIAgent agent)
    {
        idleTime += Time.deltaTime;

        if (idleTime >= maxIdleTime)
        {
            idleTime = 0f;

            if (Random.value < chanceToPatrol && agent.StateMachine.HasState(AIStateType.Patrol))
            {
                agent.TransitionToState(AIStateType.Patrol);
                Debug.Log("Changing State to: " + AIStateType.Patrol);
                return;
            }
        }
    }

    public void ExitState(AIAgent agent)
    {
        agent.AiAnimator.SetBool("Idle", false);
    }

    public AIStateType GetStateType()
    {
        return AIStateType.Idle;
    }

    public void SetIdleConfig(Vector2 idleDuration, float idleTime, float maxIdleTime, float chanceToPatrol)
    {
        this.idleDuration = idleDuration;
        this.idleTime = idleTime;
        this.maxIdleTime = maxIdleTime;
        this.chanceToPatrol = chanceToPatrol;
    }
}
