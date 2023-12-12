using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sector
{
    public string _name = "";
    public int _controlFaction = -1;
    public int _posXInt = 0;
    public int _posYInt = 0;
}

public class MapManager : MonoBehaviour
{

    #region Singleton
    public static MapManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Sector> _sectors = new List<Sector>() { };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
