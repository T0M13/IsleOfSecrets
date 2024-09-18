using UnityEngine;

[CreateAssetMenu(menuName = "AI/IdleStateConfig")]
public class IdleStateConfig : AIStateConfig
{
    public Vector2 idleDuration = new Vector2(5f, 10f);
    public float idleTime = 0f;
    public float chanceToPatrol = 0.5f;

    public override AIStateType GetStateType()
    {
        return AIStateType.Idle;
    }

    public override AIState InitializeState(AIAgent agent)
    {
        IdleState idleState = new IdleState();
        idleState.SetIdleConfig(idleDuration, idleTime, idleDuration.y, chanceToPatrol);
        return idleState;
    }
}
