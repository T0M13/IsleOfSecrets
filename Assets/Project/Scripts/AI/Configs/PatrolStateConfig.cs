using UnityEngine;

[CreateAssetMenu(menuName = "AI/PatrolStateConfig")]
public class PatrolStateConfig : AIStateConfig
{
    public float patrolSpeed = 2f;
    public float patrolRadius = 10f;
    public float maxPatrolTime = 5f;
    public float chanceToIdle = 0.3f;
    public float targetReachThreshold = 0.5f;

    public override AIStateType GetStateType()
    {
        return AIStateType.Patrol;
    }

    public override AIState InitializeState(AIAgent agent)
    {
        PatrolState patrolState = new PatrolState();
        patrolState.SetPatrolConfig(patrolSpeed, patrolRadius, maxPatrolTime, chanceToIdle, targetReachThreshold);
        return patrolState;
    }
}
