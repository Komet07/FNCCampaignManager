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

    [XmlArray("Factions"), XmlArrayItem("Faction")]
    public List<Faction> _factions = new List<Faction>() { };

    [XmlArray("Alliances"), XmlArrayItem("Alliance")]
    public List<Alliance> _alliances = new List<Alliance>() { };

    [XmlArray("Connections"), XmlArrayItem("Connection")]
    public List<JumpGateConnection> _jumpGates = new List<JumpGateConnection>() { };

    [XmlArray("PlayerFactions"), XmlArrayItem("PlayerFaction")]
    public List<PlayerFaction> _playerFactions = new List<PlayerFaction>() { };

    [XmlArray("Players"), XmlArrayItem("Player")]
    public List<Player> _players = new List<Player>() { };


    public static Map Load(string path)
    {
        var serializer = new XmlSerializer(typeof(Map));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as Map;
        }
    }
}

[System.Serializable]
public class Faction
{
    [XmlAttribute("name")]
    public string _name = "";

    [XmlAttribute("shorthand")]
    public string _shorthand = "";

    public Color32 _factionColor = new Color32(255, 255, 255, 255);

    public string _GovType = "";

    [XmlAttribute("nationType")]
    public string _nationType = "";

    [XmlAttribute("allianceId")]
    public int _allianceId = -1;
}

[System.Serializable]
public class Alliance
{
    [XmlAttribute("name")]
    public string _name = "";

    [XmlAttribute("shorthand")]
    public string _shorthand = "";

    public Color32 _allianceColor = new Color32(150, 150, 150, 255);

    [XmlArray("Members"), XmlArrayItem("Member")]
    public List<int> _memberStates = new List<int>();
}

[System.Serializable]
public class JumpGateConnection
{
    [XmlAttribute("name")]
    public string _name = ""; // i.e. Outbound Star - Getsu Fune

    [XmlAttribute("name1")]
    public string _name1 = ""; // [System Name] Jumpgate

    [XmlAttribute("name2")]
    public string _name2 = ""; // [System Name] Jumpgate

    public int _sector1Id = 0; // Id of sector 1
    public int _sector2Id = 0; // Id of sector 2
}

[System.Serializable]
public class PlayerFaction
{
    [XmlAttribute("factionID")]
    public int _regFactionID = 0; // Points to faction ID that this belongs to, if -1 then GM faction

    public List<int> _playerIDs = new List<int>() { };
}

[System.Serializable]
public class Player
{
    [XmlAttribute("name")]
    public string _name = ""; // Player name, i.e. "Yggy" or "BlueNexa"

    [XmlAttribute("playerFactionID")]
    public int _factionID = -1; // Player faction this player belongs to, -1 = independent
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
