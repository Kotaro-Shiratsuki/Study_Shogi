using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject boardObject;

    [SerializeField]
    private GameObject sideObject;

    [SerializeField]
    private GameObject gridObject;

    [SerializeField]
    private GameObject friendParent;

    [SerializeField]
    private GameObject enemyParent;

    [SerializeField]
    private SerializedDictionary<ID, GameObject> KomaList;

    [SerializeField]
    private float boardYposOffset;

    [SerializeField]
    private Vector2 sideOffset;

    [SerializeField]
    private Vector2 gridOffset;

    [SerializeField]
    private float komaYposOffset;

    private readonly Vector2Int boardSize = new Vector2Int(StaticMembers.BoardColumn, StaticMembers.BoardRow);

    // Initial layout of Koma.
    private KomaInitLayout layout;

    private void Start()
    {
        layout = new KomaInitLayout();

        if(GenerateBoard())
        {
            Debug.Log("Sho-gi board is generated!");
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Generate is failed...");
        }
    }

    /// <summary>
    /// The process to generate Shogi Board.
    /// </summary>
    /// <returns></returns>
    private bool GenerateBoard()
    {
        if(boardObject == null || gridObject == null)
        {
            return false;
        }


        GameObject board = Instantiate(boardObject, new Vector3(0, boardYposOffset, 0), Quaternion.identity);
        
        GameObject friendSide = Instantiate(sideObject, new Vector3(sideOffset.x, boardYposOffset, sideOffset.y), Quaternion.identity);
        GameObject enemySide = Instantiate(sideObject, new Vector3(-sideOffset.x, boardYposOffset, -sideOffset.y), Quaternion.AngleAxis(180.0f, Vector3.up));
        SetSideBoard(friendSide, enemySide);


        for (int y = boardSize.y - 1; y >= 0; y--)
        {
            for(int x = 0; x < boardSize.x; x++)
            {
                // Adjust coordinates so that the center of the board is (0, 0, 0)
                float posX = (x - (boardSize.x / 2)) * gridOffset.x;
                float posZ = (y - (boardSize.y / 2)) * gridOffset.x;
                Vector3 spawnPosition = new Vector3(posX, gridOffset.y, posZ);

                // Array index
                Vector2Int index = new Vector2Int((boardSize.y - 1) - y, x);

                // Instantiate the grid for the adjusted coordinates.
                GameObject go = Instantiate(gridObject, spawnPosition, Quaternion.identity);

                // Data init.
                SetParent(board, go);
                SetGridDictionary(go, index);
                SetInitialData(spawnPosition, index);
            }
        }

        return true;
    }


    #region Initialize Methods
    private void SetSideBoard(GameObject friend, GameObject enemy)
    {
        GameManager.Instance.SetSideBoard(friend.GetComponent<SideBoard>(), enemy.GetComponent<SideBoard>());
        GameManager.Instance.FriendSideBoard.BeFriend();
        GameManager.Instance.FriendSideBoard.Initialize();
        GameManager.Instance.EnemySideBoard.Initialize();
    }
    
    /// <summary>
    /// Set the instantiated grid to parent object as a child.
    /// </summary>
    private void SetParent(GameObject parent, GameObject children)
    {
        children.transform.parent = parent.transform;
    }

    /// <summary>
    /// Set the instantiated grid to array.
    /// </summary>
    private void SetGridDictionary(GameObject obj, Vector2Int index)
    {
        GameManager.Instance.GridDictionary.Add(GameManager.Instance.ParseIndexToPos(index), index, obj.GetComponent<ShogiGrid>());
    }

    /// <summary>
    /// Set grid initial data.
    /// </summary>
    private void SetInitialData(Vector3 position, Vector2Int index)
    {
        ShogiGrid grid;
        string key = GameManager.Instance.ParseIndexToPos(index);

        if (GameManager.Instance.GridDictionary.ContainsKey(key))
        {
            grid = GameManager.Instance.GridDictionary.GetValue(key);
        }
        else
        {
            Debug.LogError("Initialize is failed");
            return;
        }

        SetInitialPosition(grid, position, index);
        SetInitialRegion(grid, index);
        GenerateKoma(position, index, grid);
    }

    /// <summary>
    /// Set grid positions.
    /// </summary>
    private void SetInitialPosition(ShogiGrid grid, Vector3 position, Vector2Int index)
    {
        grid.SetWorldPosition(position);
        grid.SetShogiPosition(GameManager.Instance.GetPosAsInt(index));
        grid.SetIndexNumber(index);
    }

    /// <summary>
    /// The process to generate Koma.
    /// </summary>
    private void GenerateKoma(Vector3 position, Vector2Int index, ShogiGrid grid)
    {
        Vector2Int shogiPos = GameManager.Instance.GetPosAsInt(index);
        GameObject koma;

        if(KomaList.TryGetValue(layout.GetID(index), out koma))
        {
            Vector3 spawnPosition = new Vector3(position.x, position.y + komaYposOffset, position.z);

            if (grid.GetGridRegion() == GridRegion.Friend)
            {
                koma = Instantiate(koma, spawnPosition, Quaternion.identity);
                koma.transform.parent = friendParent.transform;
                SetKomaInitialData(koma, spawnPosition, index, shogiPos, true);
                grid.ChangeState(GridState.Friend);
            }
            else if(grid.GetGridRegion() == GridRegion.Enemy)
            {
                koma = Instantiate(koma, spawnPosition, Quaternion.AngleAxis(180.0f, Vector3.up));
                koma.transform.parent = enemyParent.transform;
                SetKomaInitialData(koma, spawnPosition, index, shogiPos, false);
                grid.ChangeState(GridState.Enemy);
            }

            koma.name = layout.GetID(index).ToString();
        }
        else
        {
            grid.ChangeState(GridState.Empty);
        }
    }


    private void SetKomaInitialData(GameObject obj, Vector3 position, Vector2Int index, Vector2Int shogiPos, bool isPlayer)
    {
        var koma = obj.GetComponent<Koma>();
        koma.InitKomaPosition(position, shogiPos);
        koma.SetInitialOwner(isPlayer);
        GameManager.Instance.KomaDictionary.Add(shogiPos, index, koma);
    }

    /// <summary>
    /// Set the region to which the grid belongs.
    /// </summary>
    private void SetInitialRegion(ShogiGrid grid, Vector2Int index)
    {
        // The 0-2 rows are the enemy region, the 6-8 rows are the own region, and the middle is the neutral region.
        switch (index.x)
        {
            case 0:
            case 1:
            case 2:
                grid.SetRegion(GridRegion.Enemy);
                break;

            case 6:
            case 7:
            case 8:
                grid.SetRegion(GridRegion.Friend);
                break;

            default:
                grid.SetRegion(GridRegion.Neutral);
                break;
        }
    }
    #endregion
}
