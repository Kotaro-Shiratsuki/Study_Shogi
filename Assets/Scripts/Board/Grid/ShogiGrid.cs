using UnityEngine;


public class ShogiGrid : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField]
    public GridData GridData { get; private set; }

    [field: Header("Effects")]
    [field: SerializeField]
    public ParticleSystem MovableEffect {  get; private set; }

    [field: SerializeField]
    public ParticleSystem AttackableEffect { get; private set; }

    [field: SerializeField]
    public ParticleSystem SelectedMovableEffect { get; private set; }

    [field: SerializeField]
    public ParticleSystem SelectedAttackableEffect { get; private set; }


    private void Awake()
    {
        InitEffect();
    }

    #region Getter Methods
    public Vector3 GetGridPosition()
    {
        return GridData.WorldPosition;
    }

    public Vector2Int GetShogiPosition()
    {
        return GridData.ShogiPosition;
    }

    public Vector2Int GetElementNumber()
    {
        return GridData.IndexNumber;
    }

    public GridState GetGridState()
    {
        return GridData.State;
    }

    public GridRegion GetGridRegion()
    {
        return GridData.Region;
    }
    #endregion

    #region Setter Methods
    public void SetWorldPosition(Vector3 worldPosition)
    {
        GridData.SetWorldPosition(worldPosition);
    }

    public void SetShogiPosition(Vector2Int shogiPosition)
    {
        GridData.SetShogiPosition(shogiPosition);
    }

    public void SetIndexNumber(Vector2Int element)
    {
        GridData.SetIndexNumber(element);
    }

    public void SetRegion(GridRegion region)
    {
        GridData.SetRegion(region);
    }

    public void ChangeState(GridState newState)
    {
        GridData.UpdateState(newState);
        EffectSwitcher();
    }
    #endregion

    public void KomaRemovedOnGrid()
    {
        GridData.UpdateState(GridState.Empty);
    }

    /// <summary>
    /// Play effects according to the state.
    /// </summary>
    public void EffectSwitcher()
    {
        StopEffect();

        switch (GridData.State)
        {
            case GridState.Movable:
            case GridState.MovableWithEv:
                PlayMovableEffect();
                break;

            case GridState.Attackable:
            case GridState.AttackableWithEv:
                PlayAttackableEffect();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Selected grid behaviour.
    /// </summary>
    public void Selected()
    {
        StopEffect();

        switch (GridData.State)
        {
            case GridState.Movable:
            case GridState.MovableWithEv:
                PlaySelectedMovableEffect();
                break;

            case GridState.Attackable:
            case GridState.AttackableWithEv:
                PlaySelectedAttackableEffectd();
                break;

            default:
                break;
        }
    }

    private void InitEffect()
    {
        MovableEffect?.Stop();
        AttackableEffect?.Stop();
        SelectedMovableEffect?.Stop();
        SelectedAttackableEffect?.Stop();
    }

    private void PlayMovableEffect()
    {
        MovableEffect?.Play();
    }

    private void PlayAttackableEffect()
    {
        AttackableEffect?.Play();
    }

    private void PlaySelectedMovableEffect()
    {
        SelectedMovableEffect?.Play();
    }

    private void PlaySelectedAttackableEffectd()
    {
        SelectedAttackableEffect?.Play();
    }

    private void StopEffect()
    {
        if(MovableEffect.isPlaying)
        {
            MovableEffect?.Stop();
        }

        if(AttackableEffect.isPlaying)
        {
            AttackableEffect?.Stop();
        }

        if(SelectedMovableEffect.isPlaying)
        {
            SelectedMovableEffect?.Stop();
        }

        if(SelectedAttackableEffect.isPlaying)
        {
            SelectedAttackableEffect?.Stop();
        }
    }
}
