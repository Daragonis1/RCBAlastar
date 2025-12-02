using UnityEngine;

public class GameController : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("[BOOT] GameController initialized");
    }
}