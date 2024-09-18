public interface AIState
{
    void EnterState(AIAgent agent); 
    void UpdateState(AIAgent agent);
    void ExitState(AIAgent agent);

    AIStateType GetStateType();

}
