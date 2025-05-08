using System;
using System.Collections.Generic;
using UnityEngine;

public class SideBoard : MonoBehaviour
{
    #region Internal class
    /// <summary>
    /// Internal class for managing each anchor on the side board.
    /// </summary>
    [Serializable]
    internal class ReservedAnchor
    {
        [SerializeField][field: Range(0, 6)]
        internal int Priority = -1;

        [SerializeField]
        internal List<Koma> Reserver;

        internal ReservedAnchor()
        {
            Reserver = new List<Koma>();
        }

        internal void SetReserver(Koma koma)
        {
            Reserver.Add(koma);
        }

        internal void SetPriority(int priority)
        {
            Priority = priority;
        }
    }
    #endregion

    [SerializeField]
    private List<Anchor> anchorPoints;
    [SerializeField]
    private SerializedDictionary<ID, ReservedAnchor> reserves;

    [field: SerializeField]
    public bool IsFriend { get; private set; } = false;

    public void Initialize()
    { 
        reserves = new SerializedDictionary<ID, ReservedAnchor>();

        for(int i = 0; i < anchorPoints.Count; i++)
        {
            anchorPoints[i].SetIndex(i);
            anchorPoints[i].BeFriend(IsFriend);
            anchorPoints[i].SetState(AnchorState.Empty);
        }
    }

    /// <summary>
    /// Transfers anchors in the idle state to the selected state.
    /// </summary>
    /// <param name="index"></param>
    public void OnClickedIdleAnchor(int index)
    {
        ResetSelectedAnchor();

        if (reserves.TryGetValue(anchorPoints[index].ReserversID, out var reserver))
        {
            reserver.Reserver[0].SelectedOnSide();
            anchorPoints[index].SetState(AnchorState.Selected);
        }
    }

    /// <summary>
    /// Ejecte the selected reserves.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Koma EjectSelectedReserver(int index)
    {
        Koma ejected = reserves[anchorPoints[index].ReserversID].Reserver[0];

        reserves[anchorPoints[index].ReserversID].Reserver.Remove(ejected);

        if(reserves[anchorPoints[index].ReserversID].Reserver.Count == 0)
        {
            reserves.Remove(anchorPoints[index].ReserversID);
            ResetAnchor(index);
            UpdateReserverOnEjecting();
        }

        return ejected;
    }

    /// <summary>
    /// Return anchor point according to the priority of the id.
    /// </summary>
    public Vector3 GetTargetPosition(ID id)
    { 
        return anchorPoints[GetPriority(id)].AnchorPoint;
    }

    public Koma GetKomaOnAnchor(int index)
    {
        return reserves[anchorPoints[index].ReserversID].Reserver[0];
    }

    /// <summary>
    /// Add the koma to the list.
    /// </summary>
    /// <param name="koma"></param>
    public void AddReservedKoma(Koma koma)
    {
        var id = koma.Data.Status.KomaID;
        int prioryty = GetPriority(id);

        if (reserves.Count == 0)
        {
            reserves.Add(id, new ReservedAnchor());
            reserves[id].SetPriority(prioryty);
            reserves[id].SetReserver(koma);
        }
        else if (!reserves.ContainsKey(id))
        {
            reserves.Add(id, new ReservedAnchor());
            reserves[id].SetPriority(prioryty);
            reserves[id].SetReserver(koma);
        }
        else
        {
            reserves[id].SetReserver(koma);
        }
    }

    /// <summary>
    /// Update the list of koma player have.
    /// </summary>
    public void UpdateReserves(Koma koma)
    {
        AddReservedKoma(koma);

        foreach(var r in reserves)
        {
            r.Value.SetPriority(GetPriority(r.Key));
            anchorPoints[r.Value.Priority].UpdateID(r.Key);

            if(anchorPoints[r.Value.Priority].State == AnchorState.Empty)
            {
                anchorPoints[r.Value.Priority].SetState(AnchorState.Idle);
            }
        }

        SortReservers();
    }

    public void ResetSelectedAnchor()
    {
        foreach (var anc in anchorPoints)
        {
            if (anc.State == AnchorState.Selected)
            {
                foreach (var koma in reserves[anc.ReserversID].Reserver)
                {
                    koma.RemoveSelectedOnSide();
                }

                anc.SetState(AnchorState.Idle);
            }
        }
    }

    public void BeFriend()
    {
        IsFriend = true;
    }


    /// <summary>
    /// Sort the koma in order of priority.
    /// </summary>
    private void SortReservers()
    {
        foreach(var r in reserves)
        {
            foreach(var koma in r.Value.Reserver)
            {
                float offset = koma.transform.position.y;
                var newPos = anchorPoints[r.Value.Priority].AnchorPoint;
                var sortedPos = new Vector3(newPos.x, offset, newPos.z);

                koma.transform.position = sortedPos;
                koma.UpdateWorldPosition(newPos);
            }
        }
    }

    private void UpdateReserverOnEjecting()
    {
        foreach (var r in reserves)
        {
            int pri = GetPriority(r.Key);
            r.Value.SetPriority(pri);
            anchorPoints[pri].UpdateID(r.Value.Reserver[0].Data.Status.KomaID);
            anchorPoints[pri].SetState(AnchorState.Idle);
        }

        SortReservers();
    }

    private void ResetAnchor(int index)
    {
        anchorPoints[index].SetState(AnchorState.Empty);
        anchorPoints[index].UpdateID(ID.None);
    }

    /// <summary>
    /// Return a number according to the priority of the id.
    /// </summary>
    private int GetPriority(ID id)
    {
        if(reserves.Count == 0)
        {
            return 0;
        }
        else
        {
            int pri = 0;
            foreach(var pair in reserves)
            {
                if((int)id < (int)pair.Key)
                {
                    ++pri;
                }
            }

            return pri;
        }
    }
}
