using UnityEngine;

/// <summary>
/// Singleton pattern class inheriting from MonoBehaviour.
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    /// <summary>
    /// Get singleton class.
    /// If not found, generate a new one.
    /// </summary>
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                // Searching for objects in a scene.
                instance = FindObjectOfType<T>();

                // If it does not exist on the scene, create a new GemeObject.
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).Name.ToString() + "Obj";

                    // Ensure that they do not destroy with scene transitions.
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

    /// <summary>
    /// If an instance already exists, it destroys itself to avoid duplication.
    /// </summary>
    protected virtual void Awake()
    {
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
