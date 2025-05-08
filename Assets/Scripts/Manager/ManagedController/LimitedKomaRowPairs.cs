using System.Collections.Generic;

public class LimitedKomaRowPairs
{
    internal class LimitedKomaRowPair
    {
        public ID id { get; private set; }
        public List<int> limitRows { get; private set; }

        public LimitedKomaRowPair(ID id, List<int> limitRows)
        {
            this.id = id;
            this.limitRows = limitRows;
        }
    }

    internal LimitedKomaRowPair FriendHuhyo { get; private set; }
    internal LimitedKomaRowPair FriendKyosya { get; private set; }
    internal LimitedKomaRowPair FriendKeima { get; private set; }
    internal LimitedKomaRowPair EnemyHuhyo { get; private set; }
    internal LimitedKomaRowPair EnemyKyosya { get; private set; }
    internal LimitedKomaRowPair EnemyKeima { get; private set; }

    public LimitedKomaRowPairs()
    {
        FriendHuhyo = new LimitedKomaRowPair(ID.Huhyo, new List<int> { 0 });
        FriendKyosya = new LimitedKomaRowPair(ID.Kyosya, new List<int> { 0 });
        FriendKeima = new LimitedKomaRowPair(ID.Keima, new List<int> { 0, 1 });
        EnemyHuhyo = new LimitedKomaRowPair(ID.Huhyo, new List<int> { 8 });
        EnemyKyosya = new LimitedKomaRowPair(ID.Kyosya, new List<int> { 8 });
        EnemyKeima = new LimitedKomaRowPair(ID.Keima, new List<int> { 7, 8 });
    }

    public List<int> GetLimitedRow(ID id, bool isFriend)
    {
        if(isFriend)
        {
            return GetFriendLimitedRows(id);
        }
        else
        {
            return GetEnemyLimitedRows(id);
        }
    }

    private List<int> GetFriendLimitedRows(ID id)
    {
        switch (id)
        {
            case ID.Huhyo:
                return FriendHuhyo.limitRows;

            case ID.Kyosya:
                return FriendKyosya.limitRows;

            case ID.Keima:
                return FriendKeima.limitRows;

            default:
                return new List<int> { -1 };
        }
    }

    private List<int> GetEnemyLimitedRows(ID id)
    {
        switch (id)
        {
            case ID.Huhyo:
                return EnemyHuhyo.limitRows;

            case ID.Kyosya:
                return EnemyKyosya.limitRows;

            case ID.Keima:
                return EnemyKeima.limitRows;

            default:
                return new List<int> { -1 };
        }
    }
}
