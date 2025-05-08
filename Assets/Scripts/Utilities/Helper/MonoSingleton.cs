using UnityEngine;

/// <summary>
/// シングルトンパターンクラス
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    /// <summary>
    /// シングルトンなクラスを取得。
    /// もし見つからなければ新たに生成する。
    /// </summary>
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                // シーンからオブジェクトを検索
                instance = FindObjectOfType<T>();

                // シーン上に存在しなければ、新たにGemeObjectを作成
                if(instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).Name.ToString() + "Obj";

                    // シーン遷移で消えないようにする
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// 初期化処理
    /// 既にインスタンスが存在する場合、重複を避けるため自らを破壊する
    /// </summary>
    protected virtual void Awake()
    {
        // インスタンスが未設定なら、自身を登録する
        // 既に別のインスタンスが登録済みなら、自身を破壊する
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad (instance);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
}
