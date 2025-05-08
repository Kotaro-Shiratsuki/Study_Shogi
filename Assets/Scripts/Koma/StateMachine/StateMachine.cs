public abstract class StateMachine
{
    protected IState currentState;

    /// <summary>
    /// State switching method
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(IState state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState?.OnEnter();
    }

    /// <summary>
    /// Method to be performed befor NormalUpdate every frame.
    /// </summary>
    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    /// <summary>
    /// Method to be performed every frame.
    /// </summary>
    public void NormalUpdate()
    {
        currentState?.NormalUpdate();
    }

    /// <summary>
    /// Method to be performed every Time.FixedDeltaTime.
    /// </summary>
    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    /// <summary>
    /// Event at start of animation.
    /// </summary>
    public void OnAnimationEnterEvent()
    {
        currentState?.OnAnimationEnterEvent();
    }

    /// <summary>
    /// Event at end of animation.
    /// </summary>
    public void OnAnimationExitEvent()
    {
        currentState?.OnAnimationExitEvent();
    }

    /// <summary>
    /// Event in transition of animation.
    /// </summary>
    public void OnAnimationTransitionEvent()
    {
        currentState?.OnAnimationTransitionEvent();
    }
}
