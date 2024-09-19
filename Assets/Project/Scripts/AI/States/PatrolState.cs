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
    private float viewAngle;

    public void SetPatrolConfig(float patrolSpeed, float patrolRadius, float maxPatrolTime, float chanceToIdle, float targetReachThreshold, float viewAngle)
    {
        this.patrolSpeed = patrolSpeed;
        this.patrolRadius = patrolRadius;
        this.maxPatrolTime = maxPatrolTime;
        this.chanceToIdle = chanceToIdle;
        this.targetReachThreshold = targetReachThreshold;
        this.viewAngle = viewAngle;
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

        if (IsTargetWithinView(agent))
        {
            if (agent.NavMeshAgent.remainingDistance <= targetReachThreshold)
            {
                SetNewTargetPosition(agent);
                agent.NavMeshAgent.SetDestination(targetPosition);
            }
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
        float randomAngle = Random.Range(-viewAngle / 2, viewAngle / 2);
        Vector3 direction = Quaternion.Euler(0, randomAngle, 0) * agent.transform.forward;
        targetPosition = agent.transform.position + direction * patrolRadius;
    }


    private void UpdateMovementVector(AIAgent agent)
    {
        Vector3 localVelocity = agent.transform.InverseTransformDirection(agent.NavMeshAgent.velocity);
        agent.movementVector = new Vector2(localVelocity.x, localVelocity.z);
    }

    private bool IsTargetWithinView(AIAgent agent)
    {
        Vector3 directionToTarget = (targetPosition - agent.transform.position).normalized;
        float angleToTarget = Vector3.Angle(agent.transform.forward, directionToTarget);

        return angleToTarget <= viewAngle / 2;
    }

    public void DrawGizmos(AIAgent agent)
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(agent.transform.position, patrolRadius);

        Vector3 forwardDirection = agent.transform.forward * patrolRadius;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forwardDirection;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forwardDirection;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(agent.transform.position, agent.transform.position + leftBoundary);
        Gizmos.DrawLine(agent.transform.position, agent.transform.position + rightBoundary);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.3f);

        UnityEditor.Handles.Label(agent.transform.position + Vector3.up * 2f, $"Chance to Idle: {chanceToIdle * 100}%");
    }
}

