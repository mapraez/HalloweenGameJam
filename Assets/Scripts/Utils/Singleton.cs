using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool SetDontDestroyOnLoad = true;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).Name + " (Singleton)";
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_instance.transform.parent == null && SetDontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}