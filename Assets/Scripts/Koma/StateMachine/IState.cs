public interface IState
{
    /// <summary>
    /// The first process executed when entering the state.
    /// </summary>
    public void OnEnter();

    /// <summary>
    /// The final process executed when exiting the state.
    /// </summary>
    public void OnExit();

    /// <summary>
    /// Method to be performed befor NormalUpdate every frame.
    /// </summary>
    public void HandleInput();

    /// <summary>
    /// Method to be performed every frame.
    /// </summary>
    public void NormalUpdate();

    /// <summary>
    /// Method to be performed every Time.FixedDeltaTime.
    /// </summary>
    public void PhysicsUpdate();

    /// <summary>
    /// Event at start of animation.
    /// </summary>
    public void OnAnimationEnterEvent();

    /// <summary>
    /// Event at end of animation.
    /// </summary>
    public void OnAnimationExitEvent();

    /// <summary>
    /// Event in transition of animation.
    /// </summary>
    public void OnAnimationTransitionEvent();
}