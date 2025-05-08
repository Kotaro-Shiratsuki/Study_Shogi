using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 盤面上にある状態
/// 盤面上でのほかの全ての状態は、このクラスを継承される
/// </summary>
public class KomaFieldedState : KomaBasementState
{
    public KomaFieldedState(KomaMovementStateMachine komaMovementStateMachine) : base(komaMovementStateMachine)
    {

    }
}