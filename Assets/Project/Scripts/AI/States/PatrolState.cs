using UnityEngine;

public class PatrolState : AIState
{
    private Vector3 targetPosition;
    private float patrolSpeed;
    private float patrolRadius;
    private float patrolTime;
    private float maxPatrolTime;
    private float chanceToIdle;
    private float targetReachThreshold;

    public void SetPatrolConfig(float patrolSpeed, float patrolRadius, float maxPatrolTime, float chanceToIdle, float targetReachThreshold)
    {
        this.patrolSpeed = patrolSpeed;
        this.patrolRadius = patrolRadius;
        this.maxPatrolTime = maxPatrolTime;
        this.chanceToIdle = chanceToIdle;
        this.targetReachThreshold = targetReachThreshold;
    }

    public void EnterState(AIAgent agent)
    {
        SetNewTargetPosition(agent);
        agent.AiAnimator.SetBool("Walking", true);
        agent.NavMeshAgent.speed = patrolSpeed;
        agent.NavMeshAgent.isStopped = false;
        agent.NavMeshAgent.SetDestination(targetPosition);
        patrolTime = 0f;
    }

    public void UpdateState(AIAgent agent)
    {
        patrolTime += Time.deltaTime;

        if (patrolTime >= maxPatrolTime)
        {
            patrolTime = 0f;
            if (Random.value < chanceToIdle && agent.StateMachine.HasState(AIStateType.Idle))
            {
                agent.TransitionToState(AIStateType.Idle);
                return;
            }
        }

        if (agent.NavMeshAgent.remainingDistance <= targetReachThreshold)
        {
            SetNewTargetPosition(agent);
            agent.NavMeshAgent.SetDestination(targetPosition);
        }

        UpdateMovementVector(agent);
    }

    public void ExitState(AIAgent agent)
    {
        agent.AiAnimator.SetBool("Walking", false);
        agent.movementVector = Vector2.zero;
        agent.NavMeshAgent.isStopped = true;
    }

    public AIStateType GetStateType()
    {
        return AIStateType.Patrol;
    }

    private void SetNewTargetPosition(AIAgent agent)
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection.y = 0;
        targetPosition = agent.transform.position + randomDirection;
    }

    private void UpdateMovementVector(AIAgent agent)
    {
        Vector3 localVelocity = agent.transform.InverseTransformDirection(agent.NavMeshAgent.velocity);
        agent.movementVector = new Vector2(localVelocity.x, localVelocity.z);
    }
}
