using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static Singleton<T> _instance;

    public static Singleton<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<Singleton<T>>();
                singletonObject.name = typeof(T).Name + " (Singleton)";
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            if (_instance.transform.parent == null)
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