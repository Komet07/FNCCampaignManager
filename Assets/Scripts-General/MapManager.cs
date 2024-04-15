using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Sector
{
    [XmlAttribute("name")]
    public string _name = "";
    [XmlAttribute("control")]
    public int _controlFaction = -1;
    public int _posXInt = 0;
    public int _posYInt = 0;

    public string _description = "";
    public string _lore = "";

    [XmlAttribute("id")]
    public int _refID = 0;

    [XmlArray("RegionCategoryIds")]
    public List<int> _regionCats = new List<int>();

    [XmlArray("RegionCategoryRegionIds")]
    public List<int> _regionCatsRegionIds = new List<int>();
}

[System.Serializable]
[XmlRoot("MapObject")]
public class Map
{

    public int _playerFactionId = -1; // if -1 then GM
    public bool _lockSelection = false; // If locked, then selection *cannot* be changed again

    public bool _debug = false; // Shows special debug information, i.e. on sectors which sector id they are *and* which faction id controls them

    public float xBoundaryMin = -5;
    public float xBoundaryMax = 5;
    public float yBoundaryMin = -5;
    public float yBoundaryMax = 5;

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

    [XmlArray("ReputationItems"), XmlArrayItem("ReputationItem")]
    public List<Rep> _reps = new List<Rep>() { };

    [XmlArray("RegionCategories"), XmlArrayItem("RegionCategory")]
    public List<RegionCategory> _regCats = new List<RegionCategory>() { };
    

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

    [XmlAttribute("id")]
    public int _refId = 0;

    public string _description = "";
    public string _lore = "";

    [XmlArray("KnownFactions"), XmlArrayItem("KnownFaction")]
    public List<int> _knownFactions = new List<int>() { };

    [XmlArray("RepIds"), XmlArrayItem("RepId")]
    public List<int> _repIds = new List<int>() { };

    public float _defaultRep = 0;

    // Knows sector exists
    [XmlArray("DiscoSectors"), XmlArrayItem("DiscoSector")]
    public List<int> _discoveredSectors = new List<int>() { };

    // Knows sector system layout
    [XmlArray("ExploSectors"), XmlArrayItem("ExploSector")]
    public List<int> _exploredSectors = new List<int>() { };

    // Knows sector ownership
    [XmlArray("SectorOwners"), XmlArrayItem("SectorOwners")]
    public List<int> _knownSectorOwnership = new List<int>() { };

    // Sectors in which player faction assets are in
    [XmlArray("SectorsLive"), XmlArrayItem("SectorLive")]
    public List<int> _sectorLiveFeeds = new List<int>() { };
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

    [XmlAttribute("id")]
    public int _refId = 0;


    public string _description = "";
    public string _lore = "";
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

    public int _sector1Id = -1; // Id of sector 1
    public int _sector2Id = -1; // Id of sector 2


    public Vector2 _s1p = new Vector2(0, 0); // Position for sector 1 point if host sector isn't loaded
    public Vector2 _s2p = new Vector2(0, 0); // Position for sector 2 point if host sector isn't loaded

    [XmlAttribute("discoverable_1")]
    public bool _discoverable1 = true; // Will only be shown once 'true' and system contents known -> JG 1

    [XmlAttribute("discoverable_2")]
    public bool _discoverable2 = true; // Will only be shown once 'true' and system contents known -> JG 2
}

[System.Serializable]
public class PlayerFaction
{
    [XmlAttribute("factionID")]
    public int _regFactionID = 0; // Points to faction ID that this belongs to, if -1 then GM faction

    [XmlArray("Players"), XmlArrayItem("Player")]
    public List<int> _playerIDs = new List<int>() { };

    

    
}

[System.Serializable]
public class Player
{
    [XmlAttribute("name")]
    public string _name = ""; // Player name, i.e. "Yggy" or "BlueNexa"

    [XmlAttribute("playerFactionID")]
    public int _factionID = -1; // Player faction this player belongs to, -1 = independent

    [XmlAttribute("id")]
    public int _refId = 0;
}

[System.Serializable]
public class Rep
{
    [XmlAttribute("faction1")]
    public int _faction1 = 0;
    [XmlAttribute("faction2")]
    public int _faction2 = 0;


    public float _repVal = 0; // goes from -5 to 5
    public string _specialVal = ""; // Stuff like 'war', 'rivals', 'allied', etc.
}

[System.Serializable]
public class RegionCategory
{
    public string _name = "";

    public int _knowledgeType = 0; // 0 = Sector has to be explored, 1 = Sector Owner has to be known

    [XmlArray("Regions"), XmlArrayItem("Region")]
    public List<Region> _regions = new List<Region>() { };
}

[System.Serializable]
public class Region
{
    public string _name = "";

    public Color32 _regionColor = new Color32(255, 255, 255, 255);
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

    public GameObject _viewingAsButton;
    public Text _viewingAsText;

    bool _savesDirCreated = false;
    bool _exportsDirCreated = false;
    bool _imgDirCreated = false;

    public List<string> _specialRelationConditions = new List<string>() { "War", "Allied" };

    void SwitchPlayer()
    {
        if (_map._playerFactionId >= _map._playerFactions.Count-1)
        {
            _map._playerFactionId = -1;
            
        }
        else
        {
            _map._playerFactionId++;
            
        }
    }
    public void AddObject(int a)
    {
        if (a == 0) // Create new Sector
        {
            Sector _sector = new Sector();
            _sector._name = "Sector";
            _sector._controlFaction = -1;
            _sector._posXInt = 0;
            _sector._posYInt = 0;
            _sector._refID = _map._sectors.Count;

            _map._sectors.Add(_sector);
            GalaxyMap.Instance._regen = true;
        }
        else if (a == 1 && _map._sectors.Count > 0) // Jump Gate Connection
        {
            JumpGateConnection _jg = new JumpGateConnection();
            _jg._name = "Jumpgate Connection";
            _jg._sector1Id = -1;
            _jg._sector2Id = -1;
            _jg._name1 = "Gate 1";
            _jg._name2 = "Gate 2";

            _map._jumpGates.Add(_jg);
            GalaxyMap.Instance._regen = true;
        }
        else if (a == 2) // Faction
        {
            Faction _faction = new Faction();
            _faction._name = "Faction";
            _faction._shorthand = "EXA";
            _faction._knownFactions = new List<int>();
            _faction._defaultRep = 0;
            _faction._allianceId = -1;

            _map._factions.Add(_faction);
        }
        else if (a == 3) // Alliance
        {
            Alliance _alliance = new Alliance();
            _alliance._name = "Alliance";
            _alliance._shorthand = "EXA";
            

            _map._alliances.Add(_alliance);
        }
        else if (a == 4) // Player Faction
        {
            PlayerFaction _pf = new PlayerFaction();
            _pf._regFactionID = -1;


            _map._playerFactions.Add(_pf);
        }
        else if (a == 5) // Region Category
        {
            RegionCategory _rg = new RegionCategory();
            _rg._name = "New Category";
            _rg._knowledgeType = 0;

            Region _r1 = new Region();
            _r1._regionColor = new Color32(255, 255, 255, 255);
            _rg._regions.Add(_r1);

            _map._regCats.Add(_rg);
        }
    }

    public void RemoveObject(int a, int b)
    {
        GalaxyMap.Instance._regen = true;
        if (a == 0) // Sector
        {
            // Remove Sector from _sector List
            _map._sectors.Remove(_map._sectors[b]);

            // Update _sector ref IDs
            for (int j = 0; j < _map._sectors.Count; j++)
            {
                if (_map._sectors[j]._refID > b)
                {
                    _map._sectors[j]._refID--;
                }
            }

            // Update JG Connections
            for (int j = 0; j < _map._jumpGates.Count; j++)
            {
                if (_map._jumpGates[j]._sector1Id == b || !_map._jumpGates[j]._discoverable1)
                {
                    _map._jumpGates[j]._sector1Id = -1;
                    _map._jumpGates[j]._name = "";
                    _map._jumpGates[j]._name2 = "Gate: Unknown";
                }
                else if (_map._jumpGates[j]._sector1Id > b)
                {
                    _map._jumpGates[j]._sector1Id--;
                }

                if (_map._jumpGates[j]._sector2Id == b || !_map._jumpGates[j]._discoverable2)
                {
                    _map._jumpGates[j]._sector2Id = -1;
                    _map._jumpGates[j]._name = "";
                    _map._jumpGates[j]._name1 = "Gate: Unknown";
                }
                else if (_map._jumpGates[j]._sector2Id > b)
                {
                    _map._jumpGates[j]._sector2Id--;
                }
            }


            // Update PlayerFaction _exploredSectors
            for (int j = 0; j < _map._factions.Count; j++)
            {
                // Update PlayerFaction _discoSectors
                for (int k = 0; k < _map._factions[j]._discoveredSectors.Count; k++)
                {
                    if (_map._factions[j]._discoveredSectors[k] == b)
                    {
                        _map._factions[j]._discoveredSectors.Remove(_map._factions[j]._discoveredSectors[k]);
                        k--;
                    }
                    else if (_map._factions[j]._discoveredSectors[k] > b)
                    {
                        _map._factions[j]._discoveredSectors[k]--;
                    }
                }
                // Update PlayerFaction _exploredSectors
                for (int k = 0; k < _map._factions[j]._exploredSectors.Count; k++)
                {
                    if (_map._factions[j]._exploredSectors[k] == b)
                    {
                        _map._factions[j]._exploredSectors.Remove(_map._factions[j]._exploredSectors[k]);
                        k--;
                    }
                    else if (_map._factions[j]._exploredSectors[k] >= b)
                    {
                        _map._factions[j]._exploredSectors[k] = _map._factions[j]._exploredSectors[k] - 1;
                    }
                }
                // Update PlayerFaction _knownSectorOwnership
                for (int k = 0; k < _map._factions[j]._knownSectorOwnership.Count; k++)
                {
                    if (_map._factions[j]._knownSectorOwnership[k] == b)
                    {
                        _map._factions[j]._knownSectorOwnership.Remove(_map._factions[j]._knownSectorOwnership[k]);
                        k--;
                    }
                    else if (_map._factions[j]._knownSectorOwnership[k] > b)
                    {
                        _map._factions[j]._knownSectorOwnership[k]--;
                    }
                }
                // Update PlayerFaction _liveFeed
                for (int k = 0; k < _map._factions[j]._sectorLiveFeeds.Count; k++)
                {
                    if (_map._factions[j]._sectorLiveFeeds[k] == b)
                    {
                        _map._factions[j]._sectorLiveFeeds.Remove(_map._factions[j]._sectorLiveFeeds[k]);
                        k--;
                    }
                    else if (_map._factions[j]._sectorLiveFeeds[k] > b)
                    {
                        _map._factions[j]._sectorLiveFeeds[k]--;
                    }
                }
            }
            for (int i = 0; i < _map._jumpGates.Count; i++)
            {
                if (_map._jumpGates[i]._sector1Id == -1 || _map._jumpGates[i]._sector2Id == -1)
                {
                    _map._jumpGates.Remove(_map._jumpGates[i]);
                }
            }

            GalaxyMap.Instance._regen = true;
        }
        else if (a == 1) // Jump Gates
        {
            _map._jumpGates.Remove(_map._jumpGates[b]);
        }
        else if (a == 2) // Factions
        {
            // Remove faction
            _map._factions.Remove(_map._factions[b]);

            

            // Set all sectors referencing the faction to -1 and update Ids of other factions
            for (int j = 0; j < _map._sectors.Count; j++)
            {
                if (_map._sectors[j]._controlFaction == b)
                {
                    _map._sectors[j]._controlFaction = -1;
                }
                else if (_map._sectors[j]._controlFaction > b)
                {
                    _map._sectors[j]._controlFaction--;
                }
            }

            // Update membership IDs in alliances
            for (int j = 0; j < _map._alliances.Count; j++)
            {
                for (int k = 0; k < _map._alliances[j]._memberStates.Count; k++)
                {
                    if (_map._alliances[j]._memberStates[k] == b)
                    {
                        _map._alliances[j]._memberStates.Remove(_map._alliances[j]._memberStates[k]);
                        k--;
                    }
                    else if (_map._alliances[j]._memberStates[k] > b)
                    {
                        _map._alliances[j]._memberStates[k]--;
                    }
                }

            }

            // Remove faction from _knownFaction lists
            for (int j = 0; j < _map._factions.Count; j++)
            {
                for (int k = 0; k < _map._factions[j]._knownFactions.Count; k++)
                {
                    if (_map._factions[j]._knownFactions[k] == b)
                    {
                        _map._factions[j]._knownFactions.Remove(_map._factions[j]._knownFactions[k]);
                        k--;
                    }
                    else if (_map._factions[j]._knownFactions[k] > b)
                    {
                        _map._factions[j]._knownFactions[k]--;
                    }
                }
            }

            // Remove reps referencing that faction
            for (int j = 0; j < _map._reps.Count; j++)
            {
                
                if (_map._reps[j]._faction1 == b)
                {
                    int _fac = _map._reps[j]._faction2;
                    if (_fac > b)
                    {
                        _fac--;
                    }
                    for (int i = 0; i < _map._factions[_fac]._repIds.Count; i++)
                    {

                        if (_map._factions[_fac]._repIds[i] == j)
                        {
                            
                            _map._factions[_fac]._repIds.Remove(_map._factions[_fac]._repIds[i]);
                            i--;
                        }
                        
                    }
                        
                    for (int i = 0; i < _map._factions.Count; i++)
                    {
                        for (int k = 0; k < _map._factions[i]._repIds.Count; k++)
                        {
                            if (_map._factions[i]._repIds[k] > j)
                            {
                                _map._factions[i]._repIds[k]--;
                            }
                        }
                    }
                    _map._reps.Remove(_map._reps[j]);
                    j--;

                }
                else if (_map._reps[j]._faction2 == b)
                {

                    int _fac = _map._reps[j]._faction1;
                    if (_fac > b)
                    {
                        _fac--;
                    }
                    for (int i = 0; i < _map._factions[_fac]._repIds.Count; i++)
                    {

                        if (_map._factions[_fac]._repIds[i] == j)
                        {

                            _map._factions[_fac]._repIds.Remove(_map._factions[_fac]._repIds[i]);
                            i--;
                        }
                        
                    }
                    for (int i = 0; i < _map._factions.Count; i++)
                    {
                        for (int k = 0; k < _map._factions[i]._repIds.Count; k++)
                        {
                            if (_map._factions[i]._repIds[k] > j)
                            {
                                _map._factions[i]._repIds[k]--;
                            }
                        }
                    }
                    _map._reps.Remove(_map._reps[j]);
                    j--;
                }
                else 
                {
                    if (_map._reps[j]._faction1 > b)
                    {
                        _map._reps[j]._faction1--;
                    }

                    if (_map._reps[j]._faction2 > b)
                    {
                        _map._reps[j]._faction2--;
                    }
                }

            }

            // Update ref ids on other factions
            for (int j = 0; j < _map._factions.Count; j++)
            {
                if (_map._factions[j]._refId > b)
                {
                    _map._factions[j]._refId--;
                }
            }
        }
        else if (a == 3) // Alliances
        {
            // Remove Alliance
            _map._alliances.Remove(_map._alliances[b]);

            // Replace allianceId with -1 in all factions belonging to it.
            for (int i = 0; i < _map._factions.Count; i++)
            {
                if (_map._factions[i]._allianceId == b)
                {
                    _map._factions[i]._allianceId = -1;
                }
            }
        }
        else if (a == 4) // Player Factions
        {
            // Remove Player Factions
            _map._playerFactions.Remove(_map._playerFactions[b]);

            // Replace Player faction id with -1 in all players belonging to it.
            for (int i = 0; i < _map._players.Count; i++)
            {
                if (_map._players[i]._factionID == b)
                {
                    _map._players[i]._factionID = -1;
                }
            }
        }
        else if (a == 5) // Region Categories
        {
            // Remove faction
            _map._regCats.Remove(_map._regCats[b]);

            // Remove all regCat references
            for (int j = 0; j < _map._sectors.Count; j++)
            {
                for(int k = 0; k < _map._sectors[j]._regionCats.Count; k++)
                {
                    if (_map._sectors[j]._regionCats[k] == b)
                    {
                        _map._sectors[j]._regionCats.Remove(_map._sectors[j]._regionCats[k]);
                        _map._sectors[j]._regionCatsRegionIds.Remove(_map._sectors[j]._regionCatsRegionIds[k]);
                        k--;
                    }
                    else if (_map._sectors[j]._regionCats[k] > b)
                    {
                        _map._sectors[j]._regionCats[k]--;
                    }
                }
            }

        }
    }

    void GetAllKnownFactions()
    {
        for (int i = 0; i < _map._sectors.Count; i++)
        {
            
            for (int j = 0; j < _map._factions.Count; j++)
            {
                bool _knownA = false;
                bool _knownB = false;
                for (int k = 0; k < _map._factions[j]._knownSectorOwnership.Count; k++)
                {
                    if (i == _map._factions[j]._knownSectorOwnership[k])
                    {
                        _knownA = true;
                    }
                }
                if (_knownA)
                {
                    for (int k = 0; k < _map._factions[j]._knownFactions.Count; k++)
                    {
                        if (_map._sectors[i]._controlFaction == _map._factions[j]._knownFactions[k])
                        {
                            _knownB = true;
                        }
                    }
                }
                    

                if (!_knownB && _knownA && _map._sectors[i]._controlFaction != -1)
                {
                    _map._factions[j]._knownFactions.Add(_map._sectors[i]._controlFaction);
                }
            }
        }
    }

    void RepManagerAdd()
    {
        for (int i = 0; i < _map._factions.Count; i++)
        {
            for (int j = 0; j < _map._factions[i]._knownFactions.Count; j++)
            {
                bool _repAdded = false;
                if (i == _map._factions[i]._knownFactions[j])
                {
                    _repAdded = true; // Don't add anything if it's the same faction.
                }
                int b = _map._factions[i]._knownFactions[j];
                // check if it's an already added connection
                for (int k = 0; k < _map._factions[i]._repIds.Count; k++)
                {
                    int a = _map._factions[i]._repIds[k];
                    
                    if ((_map._reps[a]._faction1 == i || _map._reps[a]._faction1 == b) && (_map._reps[a]._faction2 == i || _map._reps[a]._faction2 == b))
                    {
                        _repAdded = true;
                    }
                }

                // Check remaining reps if addition has not yet been found.
                if (!_repAdded)
                {
                    for (int k = 0; k < _map._reps.Count; k++)
                    {
                        if ((_map._reps[k]._faction1 == i || _map._reps[k]._faction1 == b) && (_map._reps[k]._faction2 == i || _map._reps[k]._faction2 == b))
                        {
                            _repAdded = true;
                            _map._factions[i]._repIds.Add(k);
                        }
                    }
                }

                // Add new rep now, if it really hasn't been added yet.
                if (!_repAdded)
                {
                    int a = _map._reps.Count;

                    // Add new Rep
                    Rep _r = new Rep();
                    _r._faction1 = i;
                    _r._faction2 = b;

                    float _sRep1 = _map._factions[i]._defaultRep;
                    float _sRep2 = _map._factions[b]._defaultRep;

                    if (_sRep1 > _sRep2)
                    {
                        _r._repVal = _sRep2;
                    }
                    else
                    {
                        _r._repVal = _sRep1;
                    }


                    _map._reps.Add(_r);

                    // Add rep ref in faction 1
                    _map._factions[i]._repIds.Add(a);

                    // Add rep ref in faction 2
                    _map._factions[b]._repIds.Add(a);
                }
                    
            }
        }
    }

    void RepManagerRemove()
    {
        for (int i = 0; i < _map._reps.Count; i++)
        {
            int a = _map._reps[i]._faction1;
            int b = _map._reps[i]._faction2;

            bool _eA = false;
            bool _eB = false;

            for (int j = 0; j < _map._factions[a]._knownFactions.Count; j++)
            {
                if (_map._factions[a]._knownFactions[j] == b)
                {
                    _eA = true;
                }
            }

            for (int j = 0; j < _map._factions[b]._knownFactions.Count; j++)
            {
                if (_map._factions[b]._knownFactions[j] == a)
                {
                    _eB = true;
                }
            }

            if (!_eA && !_eB)
            {
                _map._reps.Remove(_map._reps[i]);

                for (int j = 0; j < _map._factions.Count; j++)
                {
                    for (int k = 0; k < _map._factions[j]._repIds.Count; k++)
                    {
                        if (_map._factions[j]._repIds[k] == i)
                        {
                            _map._factions[j]._repIds.Remove(_map._factions[j]._repIds[k]);
                            k--;
                        }
                        else if (_map._factions[j]._repIds[k] > i)
                        {
                            _map._factions[j]._repIds[k]--;
                        }
                    }
                }


                i--;
            }
        }
    }

    public static Texture2D LoadImage(string path)
    {
        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            return tex;
        }
        else
        {
            return null;
        }
    }

    public static Sprite LoadImageAsSprite(string path)
    {
        Sprite sprite = Sprite.Create(LoadImage(path), new Rect(0.0f, 0.0f, LoadImage(path).width,
        LoadImage(path).height), new Vector2(0.5f, 0.5f), 100.0f);

        return sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!_savesDirCreated)
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "saves")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "saves"));
                _savesDirCreated = !_savesDirCreated;
            }
            else
            {
                _savesDirCreated = !_savesDirCreated;
            }
        }

        if (!_exportsDirCreated)
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "exports")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "exports"));
                _exportsDirCreated = !_exportsDirCreated;
            }
            else
            {
                _exportsDirCreated = !_exportsDirCreated;
            }
        }

        if (!_imgDirCreated)
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "images")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "images"));
                _imgDirCreated = !_imgDirCreated;
            }
            else
            {
                _imgDirCreated = !_imgDirCreated;
            }
        }


        Ray rayC;
        RaycastHit hitC;
        rayC = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayC, out hitC) && hitC.transform.gameObject == _viewingAsButton && Input.GetMouseButtonDown(0))
        {
            SwitchPlayer();
        }

        if (_map._playerFactionId >= 0)
        {
            _map._debug = false;
        }
        if (_map._lockSelection)
        {
            _viewingAsButton.GetComponent<Image>().enabled = false;
            _viewingAsButton.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            _viewingAsButton.GetComponent<Image>().enabled = true;
            _viewingAsButton.GetComponent<BoxCollider>().enabled = true;
        }

        if (_map._playerFactionId == -1)
        {
            _viewingAsText.text = "Viewing as: GM";
        }
        else if (_map._playerFactions[_map._playerFactionId]._regFactionID != -1)
        {
            _viewingAsText.text = "Viewing as: " + _map._factions[_map._playerFactions[_map._playerFactionId]._regFactionID]._name;
        }
        else
        {
            while (_map._playerFactionId > -1 && _map._playerFactions[_map._playerFactionId]._regFactionID == -1)
            {
                _map._playerFactionId++;
                if (_map._playerFactionId >= _map._playerFactions.Count)
                {
                    _map._playerFactionId = -1;
                }
            }
        }

        

        GetAllKnownFactions();
        if (!_map._lockSelection)
        {
            RepManagerAdd();
            RepManagerRemove();
        }
            
    }


}
