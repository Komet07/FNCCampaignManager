using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    #region Singleton
    public static SceneSwitch Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [Header("Main Scene Loading Information")]

    public bool _load = false;
    public string _saveName = "";
}
