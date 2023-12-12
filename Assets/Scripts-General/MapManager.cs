using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[System.Serializable]
public class Sector
{
    [XmlAttribute("name")]
    public string _name = "";
    [XmlAttribute("control")]
    public int _controlFaction = -1;
    public int _posXInt = 0;
    public int _posYInt = 0;
}

[System.Serializable]
[XmlRoot("MapObject")]
public class Map
{
    [XmlArray("Sectors"), XmlArrayItem("Sector")]
    public List<Sector> _sectors = new List<Sector>() { };

    public static Map Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Map));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Map;
        }
    }
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

    public Map _map = new Map();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
