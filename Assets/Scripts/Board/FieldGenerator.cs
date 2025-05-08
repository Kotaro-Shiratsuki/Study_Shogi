using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject boardParent;

    [SerializeField]
    private GameObject shogiGridObj;

    [SerializeField]
    private GameObject friendParent;

    [SerializeField]
    private GameObject enemyParent;

    [SerializeField]
    private SerializedDictionary<ID, GameObject> KomaList;

    [SerializeField]
    private float gridOffset;

    [SerializeField]
    private float komaYposOffset;

    private readonly Vector2Int boardSize = new Vector2Int(StaticMembers.BoardColumn, StaticMembers.BoardRow);

    // 駒の初期配置
    private KomaInitLayout layout;

    private void Start()
    {
        layout = new KomaInitLayout();

        // 盤面の生成
        if(GenerateBoard())
        {
            Debug.Log("Sho-gi board is generated!");
        }
        else
        {
            Debug.LogError("Generate is failed...");
        }
    }

    /// <summary>
    /// 盤面生成処理
    /// </summary>
    /// <returns></returns>
    private bool GenerateBoard()
    {
        if(shogiGridObj == null)
        {
            return false;
        }

        for(int y = boardSize.y - 1; y >= 0; y--)
        {
            for(int x = 0; x < boardSize.x; x++)
            {
                // 盤面の真ん中が(0, 0, 0)になるよう、座標を調整
                float posX = (x - (boardSize.x / 2)) * gridOffset;
                float posZ = (y - (boardSize.y / 2)) * gridOffset;
                Vector3 spawnPosition = new Vector3(posX, 0f, posZ);

                // 配列の要素としての番号
                Vector2Int index = new Vector2Int((boardSize.y - 1) - y, x);

                // 調整した座標に対してマス目を実体化させる
                GameObject go = Instantiate(shogiGridObj, spawnPosition, Quaternion.identity);

                // 各種データの初期化               
                SetParent(go);
                SetGridDictionary(go, index);
                SetInitialData(spawnPosition, index);
            }
        }

        return true;
    }


    #region Initialize Methods
    /// <summary>
    /// 生成したマス目をボードの子オブジェクトにセット
    /// </summary>
    private void SetParent(GameObject obj)
    {
        obj.transform.parent = boardParent.transform;
    }

    /// <summary>
    /// 生成したマス目オブジェクトを配列に格納
    /// </summary>
    private void SetGridDictionary(GameObject obj, Vector2Int index)
    {
        GameManager.Instance.GridDictionary.Add(GameManager.Instance.ParseTwoDimentionalIndexToShogiPos(index), index, obj.GetComponent<ShogiGrid>());
    }

    /// <summary>
    /// マス目情報の初期化処理
    /// </summary>
    private void SetInitialData(Vector3 position, Vector2Int index)
    {
        ShogiGrid grid;
        GridData data;

        if (GameManager.Instance.GridDictionary.TryGetValueFirstKey(GameManager.Instance.ParseTwoDimentionalIndexToShogiPos(index), out grid))
        {
            data = grid.GridData;
        }
        else
        {
            Debug.LogError("Initialize is failed");
            return;
        }

        SetInitialPosition(data, position, index);
        SetInitialRegion(data, index);
        GenerateKoma(position, index, data);
    }

    /// <summary>
    /// マス目の座標をセット
    /// </summary>
    private void SetInitialPosition(GridData data, Vector3 position, Vector2Int index)
    {
        data.SetWorldPosition(position);
        data.SetShogiPosition(GameManager.Instance.ParseTwoDimentionalIndexToShogiPos(index));
        data.SetElementNumber(index);
    }

    /// <summary>
    /// 駒生成処理
    /// </summary>
    private void GenerateKoma(Vector3 position, Vector2Int index, GridData data)
    {
        Vector2Int shogiPos = GameManager.Instance.ParseTwoDimentionalIndexToShogiPos(index);
        GameObject koma;

        if(KomaList.TryGetValue(layout.GetID(index), out koma))
        {
            Vector3 spawnPosition = new Vector3(position.x, position.y + komaYposOffset, position.z);

            if (data.Region == GridRegion.Friend)
            {
                koma = Instantiate(koma, spawnPosition, Quaternion.identity);
                koma.transform.parent = friendParent.transform;
                SetKomaInitialData(koma, spawnPosition, index, shogiPos, true);
            }
            else if(data.Region == GridRegion.Enemy)
            {
                koma = Instantiate(koma, spawnPosition, Quaternion.AngleAxis(180.0f, Vector3.up));
                koma.transform.parent = enemyParent.transform;
                SetKomaInitialData(koma, spawnPosition, index, shogiPos, false);
            }

            koma.name = layout.GetID(shogiPos).ToString();

            data.SetState(GridState.Ruled);
        }
        else
        {
            data.SetState(GridState.Empty);
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
    /// マスの所属をセット
    /// </summary>
    private void SetInitialRegion(GridData data, Vector2Int index)
    {
        // 0-2段目は自陣、6-8段目は敵陣とし、真ん中を中立陣とする
        switch(index.x)
        {
            case 0:
            case 1:
            case 2:
                data.SetRegion(GridRegion.Enemy);
                break;

            case 6:
            case 7:
            case 8:
                data.SetRegion(GridRegion.Friend);
                break;

            default:
                data.SetRegion(GridRegion.Neutral);
                break;
        }
    }
    #endregion
}
