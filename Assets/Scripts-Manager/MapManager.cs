using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

using UI;

using SystemMap;
using System.Linq;


[System.Serializable]
public class Sector
{
    [XmlAttribute("name")]
    public string _name = "";
    [XmlAttribute("hiddenName")]
    public string _hiddenName = "";
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

    [XmlElement("StarSystem")]
    public StarSystem _system = new StarSystem();

    public string GetName(bool flag)
    {
        if (flag) // Hidden Name Check (Normal Player Faction)
        {
            for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
            {
                if (MapManager.Instance._map._sectors[i] == this && MapManager.Instance.IsInKnownOwnerList(i, false) && _hiddenName != "")
                {
                
                    return _hiddenName;
                    
                }
            }
        }

        return _name;
    }
}

[System.Serializable]
public class StarSystem
{
    [XmlArray("Stars"), XmlArrayItem("Star")]
    public List<Star> _stars = new List<Star>();
    [XmlArray("CelestialBodies"), XmlArrayItem("CelestialBody")]
    public List<CelestialBody> _cB = new List<CelestialBody>();
}

[System.Serializable]
public class OrbitParams
{
    public float _radius = 0; // IN AU
    public bool _orbitingStar = false; // WHETHER OR NOT IT'S ORBITING A STAR
    public int _parentInt = -1; // PARENT OBJECT
    [Range(0, 1)]
    public float _startPosition = 0;
    [Range(0, 90)]
    public float _inclination = 0;
    [Range(0, 1)]
    public float _ascendingNode = 0;

    public float Time
    {
        get
        {
            float _a = 0;
            if (_orbitingStar)
            {
                _a = Mathf.Sqrt(Mathf.Pow(_radius, 3));
            }
            else if (!_orbitingStar && _parentInt >= 0)
            {
                _a = Mathf.Sqrt(Mathf.Pow(_radius * 150, 3));
            }

            return _a;
        }
    }

    public float CurrentPosition
    {
        get
        {
            return Time != 0 ? ((MapManager.Instance._map._time / Time) + _startPosition) % 1 : _startPosition;
        }
    }

    public float CurrentInclination(float _angle)
    {
        _angle -= _ascendingNode;

        float _posY = Mathf.Sin(_angle * 360 * Mathf.Deg2Rad);
        return _posY * _inclination;
    }

    
}

[System.Serializable]
public class CelestialBody
{
    [Header("General Information")]
    public string _name = "";

    [XmlElement("Orbit")]
    [Header("Orbit")]
    public OrbitParams _orbit = new OrbitParams();
}

[System.Serializable]
public class Star
{
    [Header("General Information")]
    public string _name = "";

    [XmlElement("Orbit")]
    [Header("Orbit")]
    public OrbitParams _orbit = new OrbitParams();

    [Header("Classification - Normal")]
    public string _type = "G";
    public int _subType = 0;
    public string _size = "V";
}

[System.Serializable]
[XmlRoot("MapObject")]
public class Map
{

    public int _playerFactionId = -1; // if -1 then GM
    public bool _lockSelection = false; // If locked, then selection *cannot* be changed again
    public bool _fleetRevealSectors = false; // Do Fleets automatically reveal sectors (discover / explore)

    public bool _debug = false; // Shows special debug information, i.e. on sectors which sector id they are *and* which faction id controls them

    public float xBoundaryMin = -5;
    public float xBoundaryMax = 5;
    public float yBoundaryMin = -5;
    public float yBoundaryMax = 5;

    public float _time = 0;

    [XmlArray("Sectors"), XmlArrayItem("Sector")]
    public List<Sector> _sectors = new List<Sector>() { };

    [XmlArray("Factions"), XmlArrayItem("Faction")]
    public List<Faction> _factions = new List<Faction>() { };

    [XmlArray("Alliances"), XmlArrayItem("Alliance")]
    public List<Alliance> _alliances = new List<Alliance>() { };

    [XmlArray("Connections"), XmlArrayItem("Connection")]
    public List<JumpGateConnection> _jumpGates = new List<JumpGateConnection>() { };

    [XmlArray("ConnectionTypes"), XmlArrayItem("ConnectionType")]
    public List<ConnectionType> _connType = new List<ConnectionType>() { };

    [XmlArray("PlayerFactions"), XmlArrayItem("PlayerFaction")]
    public List<PlayerFaction> _playerFactions = new List<PlayerFaction>() { };

    [XmlArray("Players"), XmlArrayItem("Player")]
    public List<Player> _players = new List<Player>() { };

    [XmlArray("ReputationItems"), XmlArrayItem("ReputationItem")]
    public List<Rep> _reps = new List<Rep>() { };

    [XmlArray("RegionCategories"), XmlArrayItem("RegionCategory")]
    public List<RegionCategory> _regCats = new List<RegionCategory>() { };

    [XmlArray("Fleets"), XmlArrayItem("Fleet")]
    public List<Fleet> _fleets = new List<Fleet>() { };

    [XmlArray("Shipsets"), XmlArrayItem("Shipset")]
    public List<Shipset> _shipsets = new List<Shipset>() { };
    

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

    // Known Fleets
    [XmlArray("KnownFleets"), XmlArrayItem("KnownFleet")]
    public List<int> _knownFleets = new List<int>() { };

    // Known Fleet Contents
    [XmlArray("KnownFleetContents"), XmlArrayItem("KnownFleetContent")]
    public List<int> _knownFleetContents = new List<int>() { };

    // Known Fleet Owners
    [XmlArray("KnownFleetOwners"), XmlArrayItem("KnownFleetOwner")]
    public List<int> _knownFleetOwners = new List<int>() { };

    public bool SectorDiscovered(int _s)
    {
        if (_s < 0 || _s > MapManager.Instance._map._sectors.Count)
        {
            return false;
        }

        for (int i = 0; i < _discoveredSectors.Count; i++)
        {
            if (_discoveredSectors[i] == _s)
            {
                return true;
            }
        }

        if (MapManager.Instance._map._sectors[_s]._controlFaction == _refId)
        {
            return true;
        }

        return false;
    }

    public bool SectorExplored(int _s)
    {
        if (_s < 0 || _s > MapManager.Instance._map._sectors.Count)
        {
            return false;
        }

        for (int i = 0; i < _exploredSectors.Count; i++)
        {
            if (_exploredSectors[i] == _s)
            {
                return true;
            }
        }

        if (MapManager.Instance._map._sectors[_s]._controlFaction == _refId)
        {
            return true;
        }

        return false;
    }

    public bool SectorKnownOwner(int _s)
    {
        if (_s < 0 || _s > MapManager.Instance._map._sectors.Count)
        {
            return false;
        }

        for (int i = 0; i < _knownSectorOwnership.Count; i++)
        {
            if (_knownSectorOwnership[i] == _s)
            {
                return true;
            }
        }

        if (MapManager.Instance._map._sectors[_s]._controlFaction == _refId)
        {
            return true;
        }

        return false;
    }
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

    [XmlAttribute("typeId")]
    public int _typeId = -1; // Points to connection type - -1 = generic connection

    public int _sector1Id = -1; // Id of sector 1
    public int _sector2Id = -1; // Id of sector 2


    public Vector2 _s1p = new Vector2(0, 0); // Position for sector 1 point if host sector isn't loaded
    public Vector2 _s2p = new Vector2(0, 0); // Position for sector 2 point if host sector isn't loaded

    [XmlAttribute("discoverable_1")]
    public bool _discoverable1 = true; // Will only be shown once 'true' and system contents known -> JG 1

    [XmlAttribute("discoverable_2")]
    public bool _discoverable2 = true; // Will only be shown once 'true' and system contents known -> JG 2

    [XmlAttribute("requireBothExplored")]
    public bool _reqBothExplored = false; // False by default. If true : Both systems need to be explored for the connection to be visible.

    public bool Point1Vis(int _fac)
    {
        if (_fac == -1)
        {
            return true;
        }

        if (_fac < 0 || _fac > MapManager.Instance._map._factions.Count)
        {
            return false;
        }

        _reqBothExplored = (_typeId != -1 && _typeId < MapManager.Instance._map._connType.Count) ? MapManager.Instance._map._connType[_typeId]._reqBothExplored : false;

        bool _main = MapManager.Instance._map._factions[_fac].SectorExplored(_sector1Id) && _discoverable1;
        bool _add = _reqBothExplored ? MapManager.Instance._map._factions[_fac].SectorExplored(_sector2Id) && _discoverable2 : true;

        return _main && _add; // GENERAL CASE
    }

    public bool Point2Vis(int _fac)
    {
        if (_fac == -1)
        {
            return true;
        }

        if (_fac < 0 || _fac > MapManager.Instance._map._factions.Count)
        {
            return false;
        }

        _reqBothExplored = (_typeId != -1 && _typeId < MapManager.Instance._map._connType.Count) ? MapManager.Instance._map._connType[_typeId]._reqBothExplored : false;

        bool _main = MapManager.Instance._map._factions[_fac].SectorExplored(_sector2Id) && _discoverable2;
        bool _add = _reqBothExplored ? MapManager.Instance._map._factions[_fac].SectorExplored(_sector1Id) && _discoverable1 : true;

        return _main && _add; // GENERAL CASE
    }
}


[System.Serializable]
public class ConnectionType
{
    /*
    CONNECTION TYPE : Type 'overhead' that allows for differentiation between different types of connections on the map and further customization
    Attributes:
    - Name : Name of the connection type
    - Line Color : Color of the connection on the map
    - Line Width : Width of the connection line on the map -> allows for differentiation between major and minor connections
    - Line Type : Should display different line types for further customization)
    */
    public string _name = ""; // i.e. Jumpgate, lane, pirate connection, etc.
    public Color _lineColor = new Color(1, 1, 1, .8f);
    public float _lineWidth = 1f; // Multiples of 'regular' line width
    public int _lineType = 0; // 0 = Regular, 1 = Dashed, 2 = Dotted, 3 = Slanted, 4 = None (Only Points)

    public int _pointType = 0; // 0 = Circle, 1 = Square, 2 = Diamond, 3 = Cross

    public bool _reqBothExplored = false; // Inherit this setting to ALL connections of this type 
}

[System.Serializable]
public class PlayerFaction
{
    [XmlAttribute("factionID")]
    public int _regFactionID = 0; // Points to faction ID that this belongs to, if -1 then GM faction

    [XmlArray("Players"), XmlArrayItem("Player")]
    public List<int> _playerIDs = new List<int>() { };


    public Vector2 _spawnPosition = new Vector2(0,0); // ANCHOR FOR SPAWNING
    
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

[System.Serializable]
public class Fleet
{
    [Header("General"), XmlAttribute("name")]
    public string _name = "";

    [XmlAttribute("Status")]
    public string _status = "Idle";

    [XmlAttribute("Faction")]
    public int _faction = -1;

    public string _AdmName = "";

    [Header("Sector Information")]
    public int _currentSector = -1;

    [Header("Transponder")]
    public bool _transponder = true;
    public bool _FTLTransponder = false;
    public int _FTLTransponderRange = -1;

    [Header("Travel Data")]
    public bool _travelling = false; // CURRENTLY IN FTL

    public Vector2 _travelStart = new Vector2(0, 0);
    public Vector2 _travelEnd = new Vector2(0, 0);

    public float _travelCompleted = 0;

    [Header("Fuel Information - Legacy")]
    public float _maxFuel = 0;
    public float _currentFuel = 0;

    [Header("Ships")]
    [XmlArray("Ships"), XmlArrayItem("Ship")]
    public List<Ship> _ships = new List<Ship>();


    // <-- FUEL FUNCTIONS -->
    public float getMaxFuel // GET MAX REG FUEL
    {
        get
        {
            float f = 0;

            for (int i = 0; i < _ships.Count; i++)
            {
                Ship _s = _ships[i];

                f += _s._maxFuel;
            }

            return f;
        }
    }

    public float getMaxTFuel // GET ONLY MAX TANKER FUEL
    {
        get
        {
            float f = 0;

            for (int i = 0; i < _ships.Count; i++)
            {
                Ship _s = _ships[i];

                f += _s._maxTankerFuel;
            }

            return f;
        }
    }

    public float getCurrentFuel // GET CURRENT FUEL IN FLEET
    {
        get
        {
            float f = 0;

            for (int i = 0; i < _ships.Count; i++)
            {
                Ship _s = _ships[i];

                f += _s._currentFuel;
            }

            return f;
        }
    }

    public float getCurrentTFuel // GET ONLY CURRENT TANKER FUEL
    {
        get
        {
            float f = 0;

            for (int i = 0; i < _ships.Count; i++)
            {
                Ship _s = _ships[i];

                f += _s._currentTankerFuel;
            }

            return f;
        }
    }

    public void RefillFuel(float _amount, out float _fLeft)
    {
        _fLeft = _amount;

        // HOW MUCH FUEL IS MISSING IN FLEET
        float _tFuelDiff = getMaxTFuel - getCurrentTFuel;
        float _fuelDiff = getMaxFuel - getCurrentFuel;

        // BEST CASE SCENARIO: REFILL TO MAX
        if (_amount > (_fuelDiff + _tFuelDiff))
        {
            for (int i = 0; i < _ships.Count; i++)
            {
                _ships[i].RefuelToMax();
            }

            _fLeft = _amount - (_fuelDiff + _tFuelDiff);
        }

        // WORK THROUGH FUEL DIFF FIRST, THEN FILL T FUEL
        // v - REGULAR FUEL - v
        int lowestShipID = -1;

        // BASICS : RECURSIVELY GO THROUGH SHIPS, PICK SHIP WITH LOWEST RANGE AND ADD A FUEL UNIT UNTIL NO FUEL OR SPACE LEFT
        while (_fLeft > 0)
        {
            float lowestRange = 10000000f;
            lowestShipID = -1;

            for (int i = 0; i < _ships.Count; i++)
            {
                if (_ships[i].FuelRange < lowestRange && _ships[i].FuelFraction < 1f && _ships[i]._maxFuel > 0)
                {
                    lowestRange = _ships[i].FuelRange;
                    lowestShipID = i;
                }
            }

            if (lowestShipID == -1)
            {
                break; // BREAK IF NO SHIP IS MISSING FUEL
            }

            _ships[lowestShipID]._currentFuel += (_ships[lowestShipID]._maxFuel - _ships[lowestShipID]._currentFuel > 1f) ? 1f : (_ships[lowestShipID]._maxFuel - _ships[lowestShipID]._currentFuel);
            _fLeft -= (_ships[lowestShipID]._maxFuel - _ships[lowestShipID]._currentFuel > 1f) ? 1f : (_ships[lowestShipID]._maxFuel - _ships[lowestShipID]._currentFuel);

            _ships[lowestShipID]._currentFuel = Mathf.Clamp(_ships[lowestShipID]._currentFuel, 0, _ships[lowestShipID]._maxFuel);
        }

        // v - TANKER FUEL - v
        lowestShipID = -1;

        // BASICS: GO THROUGH ALL TANKERS, PICK TANKER WITH LOWEST CURRENT FRACTION AND ADD A FUEL UNIT UNTIL NO FUEL OR SPACE LEFT
        while (_fLeft > 0)
        {
            float lowestFraction = 1f;
            lowestShipID = -1;

            for (int i = 0; i < _ships.Count; i++)
            {
                if (_ships[i].TankerFuelFraction < lowestFraction && _ships[i]._maxTankerFuel > 0)
                {
                    lowestFraction = _ships[i].TankerFuelFraction;
                    lowestShipID = i;
                }
            }

            if (lowestShipID == -1)
            {
                break; // BREAK IF NO SHIP IS MISSING TANKER FUEL
            }

            _ships[lowestShipID]._currentTankerFuel += (_ships[lowestShipID]._maxTankerFuel - _ships[lowestShipID]._currentTankerFuel > 1f) ? 1f : (_ships[lowestShipID]._maxTankerFuel - _ships[lowestShipID]._currentTankerFuel);
            _fLeft -= (_ships[lowestShipID]._maxTankerFuel - _ships[lowestShipID]._currentTankerFuel > 1f) ? 1f : (_ships[lowestShipID]._maxFuel - _ships[lowestShipID]._currentTankerFuel);

            _ships[lowestShipID]._currentTankerFuel = Mathf.Clamp(_ships[lowestShipID]._currentTankerFuel, 0, _ships[lowestShipID]._maxTankerFuel);
        }
    }

    public float FuelConsumption
    {
        get
        {
            float f = 0;

            for (int i = 0; i < _ships.Count; i++)
            {
                f += _ships[i]._fuelConsumption;
            }

            return f;
        }
    }

    public float fleetLowestRange
    {
        get
        {
            float r = Mathf.Infinity;

            for (int i = 0; i < _ships.Count; i++)
            {
                if (_ships[i].FuelRange < r)
                {
                    r = _ships[i].FuelRange;
                }
            }

            r += Mathf.Floor(getCurrentTFuel / FuelConsumption);

            return r;
        }
    }

    public void ConsumeFuel(int distance)
    {
        float _fuelSpent = distance * FuelConsumption;

        List<float> _hexLeftShip = new List<float>() {};

        // FUEL USAGE INIT ROUND
        for (int i = 0; i < _ships.Count; i++)
        {
            _hexLeftShip.Add(distance);
        }

        // v - USE FUEL - v
        int usedShipID = -1;
        float lowestRangeLeft = Mathf.Infinity;

        while (_fuelSpent > 0)
        {
            // SEARCH FOR SHIP WITH LOWEST RANGE LEFT
            usedShipID = -1;
            lowestRangeLeft = Mathf.Infinity;

            for (int i = 0; i < _ships.Count; i++)
            {
                if (_ships[i].FuelRange - _hexLeftShip[usedShipID] < lowestRangeLeft && _ships[i]._currentFuel > 0 && _hexLeftShip[usedShipID] > 0)
                {
                    usedShipID = i;
                    lowestRangeLeft = _ships[i].FuelRange;
                }
            }

            if (usedShipID == -1)
            {
                _fuelSpent = 0;
                break;
            }

            // USE TANKER FUEL FIRST IF PRESENT
            if (getCurrentTFuel > 0)
            {
                int tankerID = -1;
                float highestTankerFuel = 0;

                for (int i = 0; i < _ships.Count; i++)
                {
                    if (_ships[i]._currentTankerFuel > highestTankerFuel)
                    {
                        tankerID = i;
                        highestTankerFuel = _ships[i]._currentTankerFuel;
                    }
                }

                _fuelSpent -= (_ships[tankerID]._currentTankerFuel > 1f) ? 1f : _ships[tankerID]._currentTankerFuel;
                _hexLeftShip[usedShipID] -= ((_ships[tankerID]._currentTankerFuel > 1f) ? 1f : _ships[tankerID]._currentTankerFuel) / _ships[usedShipID]._fuelConsumption;
                _ships[tankerID]._currentTankerFuel -= (_ships[tankerID]._currentTankerFuel > 1f) ? 1f : _ships[tankerID]._currentTankerFuel;
                _ships[tankerID]._currentTankerFuel = Mathf.Clamp(_ships[tankerID]._currentTankerFuel, 0, _ships[tankerID]._maxTankerFuel);

            }
            else
            {
                _fuelSpent -= (_ships[usedShipID]._currentFuel > 1f) ? 1f : _ships[usedShipID]._currentFuel;
                _hexLeftShip[usedShipID] -= ((_ships[usedShipID]._currentFuel > 1f) ? 1f : _ships[usedShipID]._currentFuel) / _ships[usedShipID]._fuelConsumption;
                _ships[usedShipID]._currentFuel -= (_ships[usedShipID]._currentFuel > 1f) ? 1f : _ships[usedShipID]._currentFuel;
                _ships[usedShipID]._currentFuel = Mathf.Clamp(_ships[usedShipID]._currentFuel, 0, _ships[usedShipID]._maxFuel);
            }

        }
    }

    public void ConsumeFuelAmt(float _amount)
    {
        float _fuelSpent = _amount;

        List<float> _hexLeftShip = new List<float>() {};

        // FUEL USAGE INIT ROUND
        for (int i = 0; i < _ships.Count; i++)
        {
            _hexLeftShip.Add(_amount / FuelConsumption);
        }

        // v - USE FUEL - v
        int usedShipID = -1;
        float lowestRangeLeft = Mathf.Infinity;

        while (_fuelSpent > 0)
        {
            // SEARCH FOR SHIP WITH LOWEST RANGE LEFT
            usedShipID = -1;
            lowestRangeLeft = Mathf.Infinity;

            for (int i = 0; i < _ships.Count; i++)
            {
                if (_ships[i].FuelRange - _hexLeftShip[usedShipID] < lowestRangeLeft && _ships[i]._currentFuel > 0 && _hexLeftShip[usedShipID] > 0)
                {
                    usedShipID = i;
                    lowestRangeLeft = _ships[i].FuelRange;
                }
            }

            if (usedShipID == -1)
            {
                _fuelSpent = 0;
                break;
            }

            // USE TANKER FUEL FIRST IF PRESENT
            if (getCurrentTFuel > 0)
            {
                int tankerID = -1;
                float highestTankerFuel = 0;

                for (int i = 0; i < _ships.Count; i++)
                {
                    if (_ships[i]._currentTankerFuel > highestTankerFuel)
                    {
                        tankerID = i;
                        highestTankerFuel = _ships[i]._currentTankerFuel;
                    }
                }

                _fuelSpent -= (_ships[tankerID]._currentTankerFuel > 1f) ? 1f : _ships[tankerID]._currentTankerFuel;
                _hexLeftShip[usedShipID] -= ((_ships[tankerID]._currentTankerFuel > 1f) ? 1f : _ships[tankerID]._currentTankerFuel) / _ships[usedShipID]._fuelConsumption;
                _ships[tankerID]._currentTankerFuel -= (_ships[tankerID]._currentTankerFuel > 1f) ? 1f : _ships[tankerID]._currentTankerFuel;
                _ships[tankerID]._currentTankerFuel = Mathf.Clamp(_ships[tankerID]._currentTankerFuel, 0, _ships[tankerID]._maxTankerFuel);

            }
            else
            {
                _fuelSpent -= (_ships[usedShipID]._currentFuel > 1f) ? 1f : _ships[usedShipID]._currentFuel;
                _hexLeftShip[usedShipID] -= ((_ships[usedShipID]._currentFuel > 1f) ? 1f : _ships[usedShipID]._currentFuel) / _ships[usedShipID]._fuelConsumption;
                _ships[usedShipID]._currentFuel -= (_ships[usedShipID]._currentFuel > 1f) ? 1f : _ships[usedShipID]._currentFuel;
                _ships[usedShipID]._currentFuel = Mathf.Clamp(_ships[usedShipID]._currentFuel, 0, _ships[usedShipID]._maxFuel);
            }

        }
    }

    public void SetFuelValue(float _amount) // Compare to current fuel and fill up / consume accordingly
    {
        // COMPARE AMOUNT 

        if (_amount > getCurrentFuel)
        {
            float _a;
            RefillFuel(_amount - getCurrentFuel, out _a); // Add difference
        }
        else if (_amount < getCurrentFuel)
        {
            ConsumeFuelAmt(getCurrentFuel - _amount); // Subtract difference
        }
    }
}

[System.Serializable]
public class Ship
{
    public enum Condition { Destroyed, HeavilyDamaged, Damaged, OK };
    public enum Type {Military, Civilian};
    public enum SubType {Warship, FuelTanker, Freighter, TroopTransport}
    public enum Size {Fightercraft, Small, Large, Capital, Supercapital}

    [Header("General"), XmlAttribute("name")]
    public string _name = "";
    public string _className = "";
    public string _classType = "";
    public Type _type = Type.Military;
    public SubType _subType = SubType.Warship;
    public Size _size = Size.Small;

    [Header("Condition")]
    public Condition _condition = Condition.OK;

    [Header("Fuel Information")]
    public float _maxFuel = 0;
    public float _currentFuel = 0;
    public float _fuelConsumption = 0;

    [Header("Fuel Information - Tanker")]
    public float _maxTankerFuel = 0;
    public float _currentTankerFuel = 0;

    [Header("Modules")]
    [XmlArray("mountTypes"), XmlArrayItem("mountType")]
    public List<ModuleList> _mountCounts = new List<ModuleList>();
    [XmlArray("moduleTypes"), XmlArrayItem("moduleType")]
    public List<ModuleList> _moduleCounts = new List<ModuleList>();
    [XmlArray("mounts"), XmlArrayItem("mount")]
    public List<Mount> _mounts = new List<Mount>();
    [XmlArray("modules"), XmlArrayItem("module")]
    public List<Module> _modules = new List<Module>();

    [Header("Meta")]
    public string _shipsetKey = ""; // Key for shipset
    public int _shipDesignId = -1; // Points to design ID in shipset

    // v -- MASTER FUNCTIONS -- v
    public void Refit(ShipDesign _newDesign)
    {
        // GENERAL CONVERSION
        _className = _newDesign._className;
        _classType = _newDesign._classType;
        _type = _newDesign._type;
        _subType = _newDesign._subType;
        _size = _newDesign._size;

        // FUEL
        _maxFuel = _newDesign._maxFuel;
        float _leftoverFuel = (_currentFuel > _maxFuel) ? _currentFuel - _maxFuel : 0f; // TRY TO INTEGRATE LEFTOVER FUEL INTO TANKER FUEL STORAGE, IF POSSIBLE.
        _currentFuel = Mathf.Clamp(_currentFuel, 0, _maxFuel);

        _maxTankerFuel = _newDesign._maxTankerFuel;
        float _leftoverFuelB = (_currentTankerFuel > _maxTankerFuel) ? _currentTankerFuel - _maxTankerFuel : 0f; // REMOVE ANY OVERFLOW - TRY TO INTEGRATE INTO REGULAR FUEL STORAGE
        _currentTankerFuel = Mathf.Clamp(_currentTankerFuel, 0, _maxTankerFuel);
        _currentTankerFuel += (_leftoverFuel > 0) ? Mathf.Clamp(_leftoverFuel, 0, _maxTankerFuel - _currentTankerFuel) : 0; // ADD LEFTOVER FUEL FROM OLD DESIGN TO TANKER FUEL IF THAT HAPPENS

        _currentFuel += (_leftoverFuelB > 0) ? Mathf.Clamp(_leftoverFuelB, 0, _maxFuel - _currentFuel) : 0f; // TRY TO REINTEGRATE TANKER FUEL INTO REGULAR FUEL IF OVERFLOW IS PRESENT

        // MODULES - First update / add any missing mount categories, then remove any that aren't on the new design

        // - MOUNTS -
        for (int i = 0; i < _newDesign._mountCounts.Count; i++)
        {
            if (IsKeyPresent(_newDesign._mountCounts[i]._sizeKey, 0))
            {
                ChangeModuleAmountKey(0, _newDesign._mountCounts[i]._sizeKey, _newDesign._mountCounts[i]._amount);
            }
            else
            {
                _mountCounts.Add(_newDesign._mountCounts[i]);
                AddSocket(0, _newDesign._mountCounts[i]._amount);
            }
        }
        for (int i = 0; i < _mountCounts.Count; i++)
        {
            string _k = _mountCounts[i]._sizeKey;

            bool _p = false;

            foreach (ModuleList m in _newDesign._mountCounts)
            {
                if (m._sizeKey == _k)
                {
                    _p = true;
                }
            }

            if (_p)
            {
                continue;
            }

            ChangeModuleAmountKey(0, _k, 0);
            _mountCounts.Remove(_mountCounts[i]);
        }

        // - MODULES -
        for (int i = 0; i < _newDesign._moduleCounts.Count; i++)
        {
            if (IsKeyPresent(_newDesign._moduleCounts[i]._sizeKey, 0))
            {
                ChangeModuleAmountKey(1, _newDesign._moduleCounts[i]._sizeKey, _newDesign._moduleCounts[i]._amount);
            }
            else
            {
                _moduleCounts.Add(_newDesign._moduleCounts[i]);
                AddSocket(1, _newDesign._moduleCounts[i]._amount);
            }
        }
        for (int i = 0; i < _moduleCounts.Count; i++)
        {
            string _k = _moduleCounts[i]._sizeKey;

            bool _p = false;

            foreach (ModuleList m in _newDesign._moduleCounts)
            {
                if (m._sizeKey == _k)
                {
                    _p = true;
                }
            }

            if (_p)
            {
                continue;
            }

            ChangeModuleAmountKey(1, _k, 0);
            _moduleCounts.Remove(_moduleCounts[i]);
        }

    }

    public Shipset _shipset
    {
        get
        {
            if (_shipsetKey == "")
            {
                return null;
            }

            for (int i = 0; i < MapManager.Instance._map._shipsets.Count; i++)
            {
                if (MapManager.Instance._map._shipsets[i]._key == _shipsetKey)
                {
                    return MapManager.Instance._map._shipsets[i];
                }
            }
            
            return null; // No valid shipset found
        }
    }

    public ShipDesign _shipDesign
    {
        get
        {
            Shipset _s = _shipset;
            if (_s == null)
            {
                return null;
            }

            if (_shipDesignId < _shipset.shipDesigns.Count && _shipDesignId >= 0 && _s.shipDesigns[_shipDesignId] != null)
            {
                return _s.shipDesigns[_shipDesignId];
            }

            return null;
        }
    }

    // v -- FUEL FUNCTIONS -- v
    public void RefuelToMax()
    {
        _currentFuel = _maxFuel;
        _currentTankerFuel = _maxTankerFuel;
    }

    public void Refuel(float _amount)
    {
        if (_amount < 0) // SHOULDN'T END UP WITH LESS FUEL THAN BEFORE
        {
            return;
        }

        if (_amount > (_maxFuel - _currentFuel))
        {
            _currentFuel = _maxFuel;
        }
        else
        {
            _currentFuel += _amount;
        }
    }

    public float FuelFraction
    {
        get
        {
            return (_maxFuel > 0) ? (_currentFuel / _maxFuel) : 0;
        }
        set
        {
            _currentFuel = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public float FuelRange
    {
        get
        {
            return (_maxFuel > 0 && _fuelConsumption > 0) ? (_currentFuel / _fuelConsumption) : Mathf.Infinity;
        }
    }

    public float TankerFuelFraction
    {
        get
        {
            return (_maxTankerFuel > 0) ? (_currentTankerFuel / _maxTankerFuel) : 1f;
        }
        set
        {
            _currentTankerFuel = Mathf.Clamp(value, 0f, 1f);
        }
    }

    // v -- MODULE FUNCTIONS -- v 
    public void ChangeModuleAmountKey(int _type, string _key, int _val)
    {
        if (_type == 0) // MOUNTS
        {
            foreach (ModuleList _m in _mountCounts)
            {
                if (_m.CompareKey(_key))
                {
                    List<int> _a;
                    int _b;

                    int _d = TotalModulesOfType(_key, _type) + GetModulesUpTo(_key, _type);

                    _m.SetAmount(_val, out _a, out _b);

                    int _c = GetModulesUpTo(_key, _type);

                    if (_c < 0)
                    {
                        return;
                    }

                    for (int i = 0; i < _a.Count; i++)
                    {
                        _a[i] += _c;
                    }

                    RemoveSocket(_type, _a);
                    AddSocket(_type, _b, _d);
                }
            }
        }
        else if (_type == 1) // MODULES
        {
            foreach (ModuleList _m in _mountCounts)
            {
                if (_m.CompareKey(_key))
                {
                    List<int> _a;
                    int _b;

                    int _d = TotalModulesOfType(_key, _type) + GetModulesUpTo(_key, _type);

                    _m.SetAmount(_val, out _a, out _b);

                    int _c = GetModulesUpTo(_key, _type);

                    if (_c < 0)
                    {
                        return;
                    }

                    for (int i = 0; i < _a.Count; i++)
                    {
                        _a[i] += _c;
                    }

                    RemoveSocket(_type, _a);
                    AddSocket(_type, _b, _d);
                }
            }
        }
            

    }

    public void RemoveSocket(int _type, List<int> _a)
    {
        if (_type == 0) // EMPTY MOUNT
        {
            foreach (int i in _a)
            {
                if (_mounts.Count > i)
                {
                    _mounts.Remove(_mounts[i]);
                }
            }
        }
        else if (_type == 1) // EMPTY MODULES
        {
            foreach (int i in _a)
            {
                if (_modules.Count > i)
                {
                    _modules.Remove(_modules[i]);
                }
            }
        }
    }

    public void AddSocket(int _type, int _amount, int _slot = -1)
    {
        if (_type == 0)
        {
            if (_slot == -1)
            {
                for (int i = 0; i < _amount; i++)
                {
                    _mounts.Add(null);
                }
            }
            else
            {
                for (int i = 0; i < _amount; i++)
                {
                    _mounts.Insert(_slot, null);
                }
            }
        }
        else if (_type == 1)
        {
            if (_slot == -1)
            {
                for (int i = 0; i < _amount; i++)
                {
                    _modules.Add(null);
                }
            }
            else
            {
                for (int i = 0; i < _amount; i++)
                {
                    _modules.Insert(_slot, null);
                }
            }
        }
    }

    public void EmptySocket(int _type, List<int> _a)
    {
        if (_type == 0) // EMPTY MOUNT
        {
            foreach (int i in _a)
            {
                if (_mounts.Count > i)
                {
                    _mounts[i] = null;
                }
            }
        }
        else if (_type == 1) // EMPTY MODULES
        {
            foreach (int i in _a)
            {
                if (_modules.Count > i)
                {
                    _modules[i] = null;
                }
            }
        }
            
    }

    public void AddModules(Module _module, int _slot)
    {
        _module._uniqueKey = Random.Range(0, 100000000).ToString("X");

        if (_slot < _modules.Count)
        {
            _modules[_slot] = _module;
        }
        else
        {
            _modules.Add(_module);
        }
    }

    public void AddMounts(Mount _mount, int _slot)
    {
        if (_slot < _mounts.Count)
        {
            _mounts[_slot] = _mount;
        }
        else
        {
            _mounts.Add(_mount);
        }
    }

    public int GetModulesUpTo(string _key, int _type)
    {
        int _a = 0;

        if (_type == 0)
        {
            foreach (ModuleList _m in _mountCounts)
            {
                if (_m.CompareKey(_key))
                {
                    return _a;
                }
                else
                {
                    _a += _m._amount;
                }
            }
        }
        else if (_type == 1)
        {
            foreach (ModuleList _m in _moduleCounts)
            {
                if (_m.CompareKey(_key))
                {
                    return _a;
                }
                else
                {
                    _a += _m._amount;
                }
            }
        }

        return -1;
    }

    public int TotalMounts
    {
        get
        {
            int _a = 0;
            foreach (ModuleList _m in _mountCounts)
            {
                _a += _m._amount;
            }

            return _a;
        }
    }

    public int TotalMountsFilled
    {
        get
        {
            int _a = 0;
            for (int i = 0; i < _mounts.Count; i++)
            {
                if (_mounts[i] != null)
                {
                    _a++;
                }
            }

            return _a;
        }
    }

    public int TotalModules
    {
        get
        {
            int _a = 0;
            foreach (ModuleList _m in _moduleCounts)
            {
                _a += _m._amount;
            }

            return _a;
        }
    }

    public int TotalModulesFilled
    {
        get
        {
            int _a = 0;
            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i] != null)
                {
                    _a++;
                }
            }

            return _a;
        }
    }

    public int TotalModulesOfType(string _key, int _type)
    {
        int _a = 0;

        if (_type == 0) // MOUNTS
        {
            foreach (ModuleList _m in _mountCounts)
            {
                if(_m.CompareKey(_key))
                {
                    _a += _m._amount;
                }
            }
        }
        else if (_type == 1) // MODULES
        {
            foreach (ModuleList _m in _moduleCounts)
            {
                if (_m.CompareKey(_key))
                {
                    _a += _m._amount;
                }
            }
        }

        return _a;
    }

    public int TotalModulesOfTypeFilled(string _key, int _type)
    {
        int _a = 0;

        int _b = GetModulesUpTo(_key, _type);
        int _c = TotalModulesOfType(_key, _type);

        if (_type == 0) // MOUNTS
        {
            for (int i = _b; i < _b + _c; i++)
            {
                _a += (_mounts[i] != null) ? 1 : 0;
            }
        }
        else if (_type == 1) // MODULES
        {
            for (int i = _b; i < _b + _c; i++)
            {
                _a += (_modules[i] != null) ? 1 : 0;
            }
        }

        return _a;
    }

    public bool HasSpaceForType(string _key, int _type)
    {
        bool _a = (TotalModulesOfType(_key, _type) > TotalModulesOfTypeFilled(_key, _type)) ? true : false;

        return _a;
    }

    public bool IsKeyPresent(string _key, int _type)
    {

        if (_type == 0) // MOUNT
        {
            foreach (ModuleList m in _mountCounts)
            {
                if (m.CompareKey(_key))
                {
                    return true;
                }
            }
        }
        else if (_type == 1) // MODULE
        {
            foreach (ModuleList m in _moduleCounts)
            {
                if (m.CompareKey(_key))
                {
                    return true;
                }
            }
        }
           

        return false;
    }

    public int FindModuleKey(string _uniqueKey, int _type)
    {
        int _a = -1;

        if (_type == 0) // MOUNT
        {
            for (int i = 0; i < _mounts.Count; i++)
            {
                if (_mounts[i]._uniqueKey == _uniqueKey)
                {
                    _a = i;
                }
            }
        }
        else if (_type == 1) // MODULES
        {
            for (int i = 0; i < _modules.Count; i++)
            {
                if (_modules[i]._uniqueKey == _uniqueKey)
                {
                    _a = i;
                }
            }
        }

        return _a;
    }

    public List<int> ModulesOfType(string _key, int _type)
    {
        List<int> _a = new List<int>();

        if (!IsKeyPresent(_key, _type))
        {
            return _a;
        }

        int _b = GetModulesUpTo(_key, _type);

        for (int i = 0; i < TotalModulesOfType(_key, _type); i++)
        {
            _a.Add(i + _b);
        }

        return _a;
    }

    public void FitMountIntoFirstFittingEmptyModule(Mount _mount)
    {
        List<int> _slots = new List<int>();

        foreach (string _key in _mount._socketKeys)
        {
            _slots = ModulesOfType(_key, 0);

            if (_slots.Count > 0)
            {
                break;
            }
        }

        if (_slots.Count > 0)
        {
            AddMounts(_mount, _slots[0]);
        }

    }

    public void FitModuleIntoFirstFittingEmptyModule(Module _module)
    {
        List<int> _slots = new List<int>();

        foreach (string _key in _module._socketKeys)
        {
            _slots = ModulesOfType(_key, 1);

            if (_slots.Count > 0)
            {
                break;
            }
        }

        if (_slots.Count > 0)
        {
            AddModules(_module, _slots[0]);
        }

    }
}

[System.Serializable]
public class ModuleList
{
    public string _sizeKey = "";
    public int _amount = 0;

    public void SetAmount(int _value, out List<int> removeModules, out int addModules)
    {
        int _oldAmount = _amount;

        _amount = Mathf.Clamp(_value, 0, 10000);

        removeModules = new List<int>();
        addModules = 0;

        if (_oldAmount > _amount)
        {
            while (_oldAmount > _amount)
            {
                removeModules.Add(_oldAmount);
                _oldAmount--;
            }
        }
        else if (_oldAmount < _amount)
        {
            addModules = _amount - _oldAmount;
        }

        
    }

    public bool CompareKey(string _a)
    {
        return (_a == _sizeKey) ? true : false;
    }

    public bool CompareKey(List<string> _a) // Ideal for comparing whether module fits in there
    {
        if (_a.Count == 0) // Can fit into everything
        {
            return true;
        }

        foreach (string _b in _a)
        {
            if (_b == _sizeKey)
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public class Module
{

    public enum ModuleType { Generic, Radar, Drive, CIC, Bridge, Brig, CargoHold, Refinery, DamageControl, Magazine, Shield, Battery, Heatsink, Berthing, Shipyard, EWAR };
    public enum Availability {Default, Blacklist, Whitelist};

    [Header("General")]
    public string _moduleName = "";
    public ModuleType _mType = ModuleType.Generic;
    public string _subType = "";

    [Header("Condition")]
    public Ship.Condition _condition = Ship.Condition.OK;

    [Header("Magazine")]
    public string _magazineType = "";
    public int _magazineVolume = 0;

    [Header("Socket")]
    public List<string> _socketKeys = new List<string>(); // Module Keys that this can fit into. Leave blank to fit into everything.

    [Header("Faction")]
    public Availability _factionLock = Availability.Default;
    public List<int> _factionList = new List<int>(); // OPTIONAL - ONLY USE IF AVAILABILITY IS SET TO 'BLACKLIST' OR 'WHITELIST'. If Blacklist: Exclude these factions, if Whitelist: include those factions

    public bool FactionAccess(int _faction) // CHECKS WHETHER CHOSEN FACTION HAS ACCESS
    {
        if (_faction == -1)
        {
            return true; // GMs
        }

        switch (_factionLock)
        {
            case Availability.Default:
                return true;

            case Availability.Blacklist:
                foreach (int _a in _factionList)
                {
                    if (_a == _faction)
                    {
                        return false; // IN BLACKLIST
                    }
                }

                return true; // NOT IN BLACKLIST

            case Availability.Whitelist:
                foreach (int _a in _factionList)
                {
                    if (_a == _faction)
                    {
                        return true; // IN WHITELIST
                    }
                }
                return false; // NOT IN WHITELIST
        }

        return false;
    }

    [Header("Backend")]
    public string _uniqueKey = ""; // Unique Key that is generated upon being added. Allows identification of, i.e., which ammo goes into which module even if its place has moved.
}

[System.Serializable]
public class Mount
{
    public enum MountType { Generic, Weapon, EWAR, Hangar, MissileLauncher };

    [Header("General")]
    public string _mountName = "";
    public MountType _mType = MountType.Generic;
    public string _subType = "";

    [Header("Condition")]
    public Ship.Condition _condition = Ship.Condition.OK;

    [Header("Magazine")]
    public string _magazineType = "";
    public int _magazineVolume = 0;

    [Header("Socket")]
    public List<string> _socketKeys = new List<string>(); // Mount Keys that this can fit into. Leave blank to fit into everything.

    [Header("Faction")]
    public Module.Availability _factionLock = Module.Availability.Default;
    public List<int> _factionList = new List<int>(); // OPTIONAL - ONLY USE IF AVAILABILITY IS SET TO 'BLACKLIST' OR 'WHITELIST'. If Blacklist: Exclude these factions, if Whitelist: include those factions

    public bool FactionAccess(int _faction) // CHECKS WHETHER CHOSEN FACTION HAS ACCESS
    {
        if (_faction == -1)
        {
            return true; // GMs
        }

        switch (_factionLock)
        {
            case Module.Availability.Default:
                return true;

            case Module.Availability.Blacklist:
                foreach (int _a in _factionList)
                {
                    if (_a == _faction)
                    {
                        return false; // IN BLACKLIST
                    }
                }

                return true; // NOT IN BLACKLIST

            case Module.Availability.Whitelist:
                foreach (int _a in _factionList)
                {
                    if (_a == _faction)
                    {
                        return true; // IN WHITELIST
                    }
                }
                return false; // NOT IN WHITELIST
        }

        return false;
    }

    [Header("Backend")]
    public string _uniqueKey = ""; // Unique Key that is generated upon being added. Allows identification of, i.e., which ammo goes into which module even if its place has moved.
}

[System.Serializable]
public class ShipDesign
{
    [Header("General")]
    public string _className = "";
    public string _classType = "";
    public Ship.Type _type = Ship.Type.Military;
    public Ship.SubType _subType = Ship.SubType.Warship;
    public Ship.Size _size = Ship.Size.Small;

    [Header("Shipset")]
    public string _shipsetKey = "";
    public bool _useShipsetPerms = true; // WHETHER OR NOT THIS COPIES THE FACTION ACCESS SETTINGS FROM THE SHIPSET

    [Header("Fuel Information")]
    public float _maxFuel = 0;
    public float _fuelConsumption = 0;

    [Header("Fuel Information - Tanker")]
    public float _maxTankerFuel = 0;

    [Header("Modules")]
    [XmlArray("mountTypes"), XmlArrayItem("mountType")]
    public List<ModuleList> _mountCounts = new List<ModuleList>();
    [XmlArray("moduleTypes"), XmlArrayItem("moduleType")]
    public List<ModuleList> _moduleCounts = new List<ModuleList>();

    [Header("Faction")]
    public Module.Availability _factionLock = Module.Availability.Default;
    public List<int> _factionList = new List<int>(); // OPTIONAL - ONLY USE IF AVAILABILITY IS SET TO 'BLACKLIST' OR 'WHITELIST'. If Blacklist: Exclude these factions, if Whitelist: include those factions
}

[System.Serializable]
public class Shipset
{
    [Header("General"), XmlAttribute("Key")]
    public string _key = ""; // IDENT. FOR THE SHIPSET
    public string _name = ""; // SHIPSET NAME
    
    [Header("Contents"), XmlArray("shipDesigns"), XmlArrayItem("shipDesign")]
    public List<ShipDesign> shipDesigns = new List<ShipDesign>();
}

[System.Serializable]
public class ResourceValue
{
    public string _resourceKey = "";
    public int _qty = 0;
}

[System.Serializable]
public class Resource
{
    public enum Tag { Generic, Ammo, Missile, RawResource };

    [Header("General"), XmlAttribute("Key")]
    public string _key = "";
    public string _name = "";
    public Tag _tag = Tag.Generic;

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

    // MAPMODE VARS

    public bool _galaxy = true;
    public bool _system = false;

    public GameObject _galaxyObject;
    public GameObject _systemObject;

    // SYSTEM MAP MODE VARS

    public int _selectedStarSystem = -1;

    // GLOBAL UI STUFF
    [Header("UI Settings")]
    public Color32[] _uiButtonColors = { new Color32(60, 60, 60, 255), new Color32(55, 55, 55, 255) };

    // CONTEXT MENU INCOMPATABILITIES
    [Header("UI Menu Incompatabilities")]
    public List<GameObject> _contextMenuGIncompat = new List<GameObject>() { };
    public List<GameObject> _infoMenuGIncompat = new List<GameObject>() { };
    public List<GameObject> _cameraResetIncompat = new List<GameObject>() { }; // When should Camera position Reset ("R") not be triggered

    public List<GameObject> _escapeMenuIncompat = new List<GameObject>() { }; // DON'T OPEN ESCAPE MENU UNTIL THESE ARE ALL CLOSED

    public List<GameObject> _tooltipGIncompat = new List<GameObject>() {}; // Tooltip will not trigger if these menus are open.
    public bool _escapeMenuIncompatTriggered = false;

    // TESTING
    [Header("Testing")]
    public int _s1Id = -1;
    public int _s2Id = -1;

    public List<string> _specialRelationConditions = new List<string>() { "War", "Allied" };

    public void TurnOnGalaxyMap()
    {
        _galaxy = true;
        _system = false;
        GalaxyMap.Instance._regen = true;
    }

    public void TurnOnSystemMap()
    {
        _galaxy = false;
        _system = false;
        SystemMapUI.Instance.RegenMap();
    }

    public void SwitchPlayer()
    {
        if (_map._playerFactionId >= _map._playerFactions.Count-1)
        {
            _map._playerFactionId = -1;
            
        }
        else
        {
            _map._playerFactionId++;
            
        }

        // UI UPDATE
        OutlinerUIGalaxy.Instance.RebuildFleetMenu();
        OutlinerUIGalaxy.Instance._indivFleetIsOn = false;
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
            _faction._refId = _map._factions.Count;

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
        else if (a == 6) // Connection Type 
        {
            ConnectionType _ct = new ConnectionType();
            _ct._name = "Connection";
            _ct._lineColor = new Color(1, 1, 1, 1f);
            _ct._lineWidth = 1;
            _ct._lineType = 0;
            _ct._pointType = 0;

            _map._connType.Add(_ct);
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
                    i--;
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
                for (int k = 0; k < _map._sectors[j]._regionCats.Count; k++)
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
        else if (a == 6) // Connection Types
        {
            // Remove Connection Type
            _map._connType.Remove(_map._connType[b]);

            // Remove all connType references from jumpgates
            for (int j = 0; j < _map._jumpGates.Count; j++)
            {
                if (_map._jumpGates[j]._typeId == b)
                {
                    _map._jumpGates[j]._typeId = -1;
                }
                else if (_map._jumpGates[j]._typeId > b)
                {
                    _map._jumpGates[j]._typeId--;
                }
            }

            // Regen map to update line styles
            GalaxyMap.Instance._regen = true;
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

    void GetExploredSectors()
    {
        for (int i = 0; i < _map._fleets.Count; i++)
        {
            int _sID = _map._fleets[i]._currentSector;
            if (_sID == -1 || _map._fleets[i]._faction < 0 || _map._fleets[i]._faction >= _map._factions.Count || !_map._fleetRevealSectors)
            {
                continue;
            }

            bool _p1 = false;
            bool _p2 = false;

            // CHECK IF DISCOVERED
            for (int j = 0; j < _map._factions[_map._fleets[i]._faction]._discoveredSectors.Count; j++)
            {
                if (_map._factions[_map._fleets[i]._faction]._discoveredSectors[j] == _sID)
                {
                    _p1 = true;
                }
            }

            // CHECK IF EXPLORED
            for (int j = 0; j < _map._factions[_map._fleets[i]._faction]._exploredSectors.Count; j++)
            {
                if (_map._factions[_map._fleets[i]._faction]._exploredSectors[j] == _sID)
                {
                    _p2 = true;
                }
            }

            if (!_p1)
            {
                _map._factions[_map._fleets[i]._faction]._discoveredSectors.Add(_sID);
            }
            if (!_p2)
            {
                _map._factions[_map._fleets[i]._faction]._exploredSectors.Add(_sID);
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

    public bool ContextMenuGCheck
    {
        get
        {
            bool flag = true;
            for (int i = 0; i < _contextMenuGIncompat.Count; i++)
            {
                if (_contextMenuGIncompat[i].activeInHierarchy)
                {
                    flag = false;
                }
            }
            return flag;
        }
    }

    public bool InfoMenuGCheck
    {
        get
        {
            bool flag = true;
            for (int i = 0; i < _infoMenuGIncompat.Count; i++)
            {
                if (_infoMenuGIncompat[i].activeInHierarchy)
                {
                    flag = false;
                }
            }
            return flag;
        }
    }

    public bool CameraResetIncompatCheck
    {
        get
        {
            bool flag = true;
            for (int i = 0; i < _cameraResetIncompat.Count; i++)
            {
                if (_cameraResetIncompat[i].activeInHierarchy)
                {
                    flag = false;
                }
            }

            return flag;
        }
    }

    public bool TooltipGIncompatCheck
    {
        get
        {
            bool flag = true;

            for (int i = 0; i < _tooltipGIncompat.Count; i++)
            {
                if (_tooltipGIncompat[i].activeInHierarchy)
                {
                    flag = false;
                }
            }

            return flag;
        }
    }

    public void GetRepState(int _f1, int _f2, out float _v, out string _s)
    {
        _v = 0f;
        _s = "";

        if (_f1 < 0 || _f1 > _map._factions.Count)
        {
            return;
        }

        for (int j = 0; j < _map._factions[_f1]._repIds.Count; j++)
        {
            if (_map._reps[_map._factions[_f1]._repIds[j]]._faction1 == _f2 || _map._reps[_map._factions[_f1]._repIds[j]]._faction2 == _f2)
            {
                _v = _map._reps[_map._factions[_f1]._repIds[j]]._repVal;
                _s = _map._reps[_map._factions[_f1]._repIds[j]]._specialVal;
            }
        }

        return;
    }

    public int ReturnSector(Vector2 _pos)
    {
        for (int i = 0; i < _map._sectors.Count; i++)
        {
            if (_pos.x == _map._sectors[i]._posXInt && _pos.y == _map._sectors[i]._posYInt)
            {
                return i;
            }
        }

        return -1;
    }

    public void AddSector(Vector2 _pos)
    {
        Sector _sector = new Sector();
        _sector._name = "Sector";
        _sector._controlFaction = -1;
        _sector._posXInt = (int)_pos.x;
        _sector._posYInt = (int)_pos.y;
        _sector._refID = _map._sectors.Count;

        // IF CURRENTLY IN PLAYER FACTION, ADD SECTOR TO DISCOVERED SECTOR LIST
        if (_map._playerFactionId >= 0)
        {
            int _facID = _map._playerFactions[_map._playerFactionId]._regFactionID;

            _map._factions[_facID]._discoveredSectors.Add(_sector._refID);
        }

        _map._sectors.Add(_sector);
        GalaxyMap.Instance._regen = true;
    }

    public void AddConnection(int s1, int s2, object[] additionalData = null)
    {
        JumpGateConnection _jg = new JumpGateConnection();
        _jg._name = "Connection";
        _jg._sector1Id = s1;
        _jg._sector2Id = s2;
        _jg._name1 = "Gate: " + _map._sectors[s1]._name;
        _jg._name2 = "Gate: " + _map._sectors[s2]._name;

        if (additionalData.Length >= 1)
        {
            if (additionalData[0] != null)
            {
                _jg._typeId = (int)additionalData[0]; // GET DATA FOR JUMPGATE TYPE IF AVAILABLE
            }
        }

        _map._jumpGates.Add(_jg);
        GalaxyMap.Instance._regen = true;
    }

    public void RemoveConnection(int s1, int s2)
    {
        for (int i = 0; i < _map._jumpGates.Count; i++)
        {
            if ((_map._jumpGates[i]._sector1Id == s1 || _map._jumpGates[i]._sector1Id == s2) && (_map._jumpGates[i]._sector2Id == s1 || _map._jumpGates[i]._sector2Id == s2))
            {
                RemoveObject(1, i);
                i--;
            }
        }
        GalaxyMap.Instance._regen = true;
    }

    public int GetConnectionID(int s1, int s2)
    {
        if (s2 == -1)
        {
            for (int i = 0; i < _map._jumpGates.Count; i++)
            {
                if (_map._jumpGates[i]._sector1Id == s1 || _map._jumpGates[i]._sector2Id == s1)
                {
                    return i;
                }
            }
        }

        for (int i = 0; i < _map._jumpGates.Count; i++)
        {
            if ((_map._jumpGates[i]._sector1Id == s1 || _map._jumpGates[i]._sector1Id == s2) && (_map._jumpGates[i]._sector2Id == s1 || _map._jumpGates[i]._sector2Id == s2))
            {
                return i;
            }
        }

        return -1;
    }

    public int ConnectionCount(int s)
    {
        int count = 0;
        for (int i = 0; i < _map._jumpGates.Count; i++)
        {
            if (_map._jumpGates[i]._sector1Id == s || _map._jumpGates[i]._sector2Id == s)
            {
                count++;
            }
        }

        return count;
    }

    public Vector2 GetMouseSectorPos
    {
        get
        {
            Vector2 _a = new Vector2(0, 0);

            Vector3 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float xPos = _pos.x / .75f;

            int xPosI = Mathf.RoundToInt(xPos);

            float yPos = 0;
            if (Mathf.Pow(xPosI, 2) % 2 == 0)
            {
                if (xPosI <= 0)
                {
                    yPos = _pos.y / .9f;
                }
                else
                {
                    yPos = _pos.y / .9f;
                }
            }
            else
            {
                yPos = (_pos.y - .45f) / .9f;
            }

            int yPosI = Mathf.RoundToInt(yPos);

            _a = new Vector2(xPosI, yPosI);

            return _a;
        }
    }

    public Vector2 GetScreenSectorPos(Vector3 _b)
    {
        Vector2 _a = new Vector2(0, 0);

        Vector3 _pos = Camera.main.ScreenToWorldPoint(_b);

        float xPos = _pos.x / .75f;

        int xPosI = Mathf.RoundToInt(xPos);

        float yPos = 0;
        if (Mathf.Pow(xPosI, 2) % 2 == 0)
        {
            if (xPosI <= 0)
            {
                yPos = _pos.y / .9f;
            }
            else
            {
                yPos = _pos.y / .9f;
            }
        }
        else
        {
            yPos = (_pos.y - .45f) / .9f;
        }

        int yPosI = Mathf.RoundToInt(yPos);

        _a = new Vector2(xPosI, yPosI);

        return _a;
        
    }

    public bool IsInDiscoveredList(int _sec, bool flag)
    {
        if (_map._playerFactionId < 0 || (!_map._lockSelection && flag))
        {
            return true;
        }

        int _facID = _map._playerFactions[_map._playerFactionId]._regFactionID;

        for (int i = 0; i < _map._factions[_facID]._discoveredSectors.Count; i++)
        {
            if (_map._factions[_facID]._discoveredSectors[i] == _sec || _map._sectors[_sec]._controlFaction == _facID)
            {
                return true;
            }
        }

        if (_map._sectors[_sec]._controlFaction == _facID)
        {
            return true;
        }

        return false;
    }

    public bool IsInKnownOwnerList(int _sec, bool flag)
    {
        if (_map._playerFactionId < 0 || (!_map._lockSelection && flag))
        {
            return true;
        }

        int _facID = _map._playerFactions[_map._playerFactionId]._regFactionID;

        for (int i = 0; i < _map._factions[_facID]._knownSectorOwnership.Count; i++)
        {
            if (_map._factions[_facID]._knownSectorOwnership[i] == _sec || _map._sectors[_sec]._controlFaction == _facID)
            {
                return true;
            }
        }

        if (_map._sectors[_sec]._controlFaction == _facID)
        {
            return true;
        }

        return false;
    }

    public bool HasConnections(int _sec)
    {
        for (int i = 0; i < _map._jumpGates.Count; i++)
        {
            if (_map._jumpGates[i]._sector1Id == _sec || _map._jumpGates[i]._sector2Id == _sec)
            {
                return true;
            }
        }

        return false;
    }

    public int SectorDistance(int s1Id, int s2Id)
    {

        if (s1Id < 0 || s2Id < 0 || s1Id >= _map._sectors.Count || s2Id >= _map._sectors.Count)
        {
            return -1;
        }

        // Debug.Log(Mathf.Sqrt(Mathf.Pow(_map._sectors[s1Id]._posXInt,2) + Mathf.Pow(_map._sectors[s2Id]._posYInt, 2)));

        // -- COORD CONVERSION --
        int x = _map._sectors[s1Id]._posXInt - _map._sectors[s2Id]._posXInt;
        int y = _map._sectors[s1Id]._posYInt - _map._sectors[s2Id]._posYInt;

        int q1 = x;
        int r1 = (_map._sectors[s1Id]._posXInt%2 == 0) ? y - (x + (x*x % 2)) / 2 : y - (x - (x * x % 2)) / 2;
        int s1 = (q1 * -1) - r1;

        int r = Mathf.Max(Mathf.Abs(q1), Mathf.Abs(r1), Mathf.Abs(s1));

        /* int[] a = {q1, r1, s1};
        Debug.Log("q: " + q1 + "r: " + r1 + "s: " + s1);
        Debug.Log(r); */

        return r;
    }

    public int SectorDistance(Vector2 _a, Vector2 _b)
    {

        // Debug.Log(Mathf.Sqrt(Mathf.Pow(_map._sectors[s1Id]._posXInt,2) + Mathf.Pow(_map._sectors[s2Id]._posYInt, 2)));

        // -- COORD CONVERSION --
        int x = (int)_a.x - (int)_b.x;
        int y = (int)_a.y - (int)_b.y;

        int q1 = x;
        int r1 = (_a.x%2 == 0) ? y - (x + (x * x % 2)) / 2 : y - (x - (x * x % 2)) / 2;
        int s1 = (q1 * -1) - r1;

        int r = Mathf.Max(Mathf.Abs(q1), Mathf.Abs(r1), Mathf.Abs(s1));

        int[] a = {q1, r1, s1};

        return r;
    }

    public Vector2 PositionAsSectorPosition(Vector2 _a)
    {
        float xPos = _a.x / .75f;

        int xPosI = Mathf.RoundToInt(xPos);

        float yPos = 0;
        if (Mathf.Pow(xPosI, 2) % 2 == 0)
        {
            if (xPosI <= 0)
            {
                yPos = _a.y / .9f;
            }
            else
            {
                yPos = _a.y / .9f;
            }
        }
        else
        {
            yPos = (_a.y - .45f) / .9f;
        }

        int yPosI = Mathf.RoundToInt(yPos);

        Vector2 _b = new Vector2(xPosI, yPosI);

        return _b;
    }

    public bool Fleet_IsVisible(int _fleet)
    {

        _fleet = Mathf.Clamp(_fleet, -1, _map._fleets.Count - 1);

        if (_fleet < 0)
        {
            return false;
        }

        /* if (!_map._lockSelection)
        {
            return true;
        } */

        if (_map._playerFactionId < 0)
        {
            return true;
        }

        int _player = _map._playerFactions[_map._playerFactionId]._regFactionID;
        Fleet F = _map._fleets[_fleet];

        /* if (F._currentSector < 0 && !F._travelling)
        {
            return false;
        }  */

        if (_player == F._faction)
        {
            return true; // FLEETS BELONGING TO A FACTION SHOULD ALWAYS BE VISIBLE
        }

        for (int i = 0; i < _map._factions[_player]._knownFleets.Count; i++)
        {
            if (_map._factions[_player]._knownFleets[i] == _fleet)
            {
                return true;
            }
        }

        if (!F._travelling) // Fleet is in System
        {
            

            if (F._currentSector >= 0 && _map._sectors[F._currentSector]._controlFaction == _player)
            {
                return true; // FLEETS IN A SYSTEM OWNED BY THAT FACTION ARE ALWAYS VISIBLE
            }

            for (int i = 0; i < _map._fleets.Count; i++)
            {
                if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling == false && _map._fleets[i]._currentSector >= 0 && _map._fleets[i]._currentSector == F._currentSector)
                {
                    return true; // FLEET IS VISIBLE IF A FLEET OF THE PLAYER IS ALSO IN THE SECTOR
                }
            }
        }
        else
        {
            if (F._FTLTransponder)
            {
                if (F._FTLTransponderRange >= 0)
                {
                    Vector2 _p = TurnSectorIntoRealPos(F._travelStart);
                    Vector2 _v = TurnSectorIntoRealPos(F._travelEnd) - _p;

                    Vector2 _currentPos = _p + (_v * F._travelCompleted);
                    Vector2 _currentSPos = PositionAsSectorPosition(_currentPos);

                    for (int i = 0; i < _map._fleets.Count; i++)
                    {
                        if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling)
                        {
                            Vector2 _p2 = TurnSectorIntoRealPos(F._travelStart);
                            Vector2 _v2 = TurnSectorIntoRealPos(F._travelEnd) - _p;

                            Vector2 _currentPos2 = _p + (_v * F._travelCompleted);
                            Vector2 _currentSPos2 = PositionAsSectorPosition(_currentPos2);

                            int _a = SectorDistance(_currentSPos, _currentSPos2);

                            if (_a <= F._FTLTransponderRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

        }

        return false;
       
    }

    public bool Fleet_IsVisible(int _fleet, int _faction)
    {

        _fleet = Mathf.Clamp(_fleet, -1, _map._fleets.Count - 1);

        if (_fleet < 0)
        {
            return false;
        }

        /* if (!_map._lockSelection)
        {
            return true;
        } */

        int _player = _faction;
        Fleet F = _map._fleets[_fleet];

        /* if (F._currentSector < 0 && !F._travelling)
        {
            return false;
        } */

        if (_player == F._faction)
        {
            return true; // FLEETS BELONGING TO A FACTION SHOULD ALWAYS BE VISIBLE
        }

        for (int i = 0; i < _map._factions[_player]._knownFleets.Count; i++)
        {
            if (_map._factions[_player]._knownFleets[i] == _fleet)
            {
                return true;
            }
        }

        if (!F._travelling) // Fleet is in System
        {


            if (F._currentSector >= 0 && _map._sectors[F._currentSector]._controlFaction == _player)
            {
                return true; // FLEETS IN A SYSTEM OWNED BY THAT FACTION ARE ALWAYS VISIBLE
            }

            for (int i = 0; i < _map._fleets.Count; i++)
            {
                if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling == false && _map._fleets[i]._currentSector >= 0 && _map._fleets[i]._currentSector == F._currentSector)
                {
                    return true; // FLEET IS VISIBLE IF A FLEET OF THE PLAYER IS ALSO IN THE SECTOR
                }
            }
        }
        else
        {
            if (F._FTLTransponder)
            {
                if (F._FTLTransponderRange >= 0)
                {
                    Vector2 _p = TurnSectorIntoRealPos(F._travelStart);
                    Vector2 _v = TurnSectorIntoRealPos(F._travelEnd) - _p;

                    Vector2 _currentPos = _p + (_v * F._travelCompleted);
                    Vector2 _currentSPos = PositionAsSectorPosition(_currentPos);

                    for (int i = 0; i < _map._fleets.Count; i++)
                    {
                        if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling)
                        {
                            Vector2 _p2 = TurnSectorIntoRealPos(F._travelStart);
                            Vector2 _v2 = TurnSectorIntoRealPos(F._travelEnd) - _p;

                            Vector2 _currentPos2 = _p + (_v * F._travelCompleted);
                            Vector2 _currentSPos2 = PositionAsSectorPosition(_currentPos2);

                            int _a = SectorDistance(_currentSPos, _currentSPos2);

                            if (_a <= F._FTLTransponderRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

        }

        return false;

    }

    public bool Fleet_ContentsKnown(int _fleet)
    {
        _fleet = Mathf.Clamp(_fleet, -1, _map._fleets.Count - 1);

        if (_fleet < 0)
        {
            return false;
        }

        /* if (!_map._lockSelection)
        {
            return true;
        } */

        if (_map._playerFactionId < 0)
        {
            return true;
        }

        int _player = _map._playerFactions[_map._playerFactionId]._regFactionID;
        Fleet F = _map._fleets[_fleet];

        /* if (F._currentSector < 0 && !F._travelling)
        {
            return false;
        } */

        if (_player == F._faction)
        {
            return true; // FLEETS BELONGING TO A FACTION SHOULD ALWAYS BE VISIBLE
        }

        for (int i = 0; i < _map._factions[_player]._knownFleetContents.Count; i++)
        {
            if (_map._factions[_player]._knownFleetContents[i] == _fleet)
            {
                return true;
            }
        }

        if (!F._travelling) // Fleet is in System
        {


            if (F._currentSector >= 0 && _map._sectors[F._currentSector]._controlFaction == _player && F._transponder)
            {
                return true; // FLEETS IN A SYSTEM OWNED BY THAT FACTION ARE ALWAYS VISIBLE
            }

            for (int i = 0; i < _map._fleets.Count; i++)
            {
                if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling == false && _map._fleets[i]._currentSector >= 0 && _map._fleets[i]._currentSector == F._currentSector && F._transponder)
                {
                    return true; // FLEET IS VISIBLE IF A FLEET OF THE PLAYER IS ALSO IN THE SECTOR
                }
            }
        }
        else
        {
            if (F._FTLTransponder)
            {
                if (F._FTLTransponderRange >= 0)
                {
                    Vector2 _p = TurnSectorIntoRealPos(F._travelStart);
                    Vector2 _v = TurnSectorIntoRealPos(F._travelEnd) - _p;

                    Vector2 _currentPos = _p + (_v * F._travelCompleted);
                    Vector2 _currentSPos = PositionAsSectorPosition(_currentPos);

                    for (int i = 0; i < _map._fleets.Count; i++)
                    {
                        if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling)
                        {
                            Vector2 _p2 = TurnSectorIntoRealPos(F._travelStart);
                            Vector2 _v2 = TurnSectorIntoRealPos(F._travelEnd) - _p;

                            Vector2 _currentPos2 = _p + (_v * F._travelCompleted);
                            Vector2 _currentSPos2 = PositionAsSectorPosition(_currentPos2);

                            int _a = SectorDistance(_currentSPos, _currentSPos2);

                            if (_a <= F._FTLTransponderRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

        }

        return false;
    }

    public bool Fleet_ContentsKnown(int _fleet, int _faction)
    {
        _fleet = Mathf.Clamp(_fleet, -1, _map._fleets.Count - 1);

        if (_fleet < 0)
        {
            return false;
        }

        int _player = _faction;
        Fleet F = _map._fleets[_fleet];

        /* if (F._currentSector < 0 && !F._travelling)
        {
            return false;
        } */

        if (_player == F._faction)
        {
            return true; // FLEETS BELONGING TO A FACTION SHOULD ALWAYS BE VISIBLE
        }

        for (int i = 0; i < _map._factions[_player]._knownFleetContents.Count; i++)
        {
            if (_map._factions[_player]._knownFleetContents[i] == _fleet)
            {
                return true;
            }
        }

        if (!F._travelling) // Fleet is in System
        {


            if (F._currentSector >= 0 && _map._sectors[F._currentSector]._controlFaction == _player && F._transponder)
            {
                return true; // FLEETS IN A SYSTEM OWNED BY THAT FACTION ARE ALWAYS VISIBLE
            }

            for (int i = 0; i < _map._fleets.Count; i++)
            {
                if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling == false && _map._fleets[i]._currentSector >= 0 && _map._fleets[i]._currentSector == F._currentSector && F._transponder)
                {
                    return true; // FLEET IS VISIBLE IF A FLEET OF THE PLAYER IS ALSO IN THE SECTOR
                }
            }
        }
        else
        {
            if (F._FTLTransponder)
            {
                if (F._FTLTransponderRange >= 0)
                {
                    Vector2 _p = TurnSectorIntoRealPos(F._travelStart);
                    Vector2 _v = TurnSectorIntoRealPos(F._travelEnd) - _p;

                    Vector2 _currentPos = _p + (_v * F._travelCompleted);
                    Vector2 _currentSPos = PositionAsSectorPosition(_currentPos);

                    for (int i = 0; i < _map._fleets.Count; i++)
                    {
                        if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling)
                        {
                            Vector2 _p2 = TurnSectorIntoRealPos(F._travelStart);
                            Vector2 _v2 = TurnSectorIntoRealPos(F._travelEnd) - _p;

                            Vector2 _currentPos2 = _p + (_v * F._travelCompleted);
                            Vector2 _currentSPos2 = PositionAsSectorPosition(_currentPos2);

                            int _a = SectorDistance(_currentSPos, _currentSPos2);

                            if (_a <= F._FTLTransponderRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

        }

        return false;
    }

    public bool Fleet_IsOwnerKnown(int _fleet)
    {
        _fleet = Mathf.Clamp(_fleet, -1, _map._fleets.Count - 1);

        if (_fleet < 0)
        {
            return false;
        }

        /* if (!_map._lockSelection)
        {
            return true;
        } */

        if (_map._playerFactionId < 0)
        {
            return true;
        }

        int _player = _map._playerFactions[_map._playerFactionId]._regFactionID;
        Fleet F = _map._fleets[_fleet];

        /* if (F._currentSector < 0 && !F._travelling)
        {
            return false;
        } */

        if (_player == F._faction)
        {
            return true; // FLEETS BELONGING TO A FACTION SHOULD ALWAYS BE VISIBLE
        }

        for (int i = 0; i < _map._factions[_player]._knownFleetOwners.Count; i++)
        {
            if (_map._factions[_player]._knownFleetOwners[i] == _fleet)
            {
                return true;
            }
        }

        if (!F._travelling) // Fleet is in System
        {


            if (F._currentSector >= 0 && _map._sectors[F._currentSector]._controlFaction == _player && F._transponder)
            {
                return true; // FLEETS IN A SYSTEM OWNED BY THAT FACTION ARE ALWAYS VISIBLE
            }

            for (int i = 0; i < _map._fleets.Count; i++)
            {
                if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling == false && _map._fleets[i]._currentSector >= 0 && _map._fleets[i]._currentSector == F._currentSector && F._transponder)
                {
                    return true; // FLEET IS VISIBLE IF A FLEET OF THE PLAYER IS ALSO IN THE SECTOR
                }
            }
        }
        else
        {
            if (F._FTLTransponder)
            {
                if (F._FTLTransponderRange >= 0)
                {
                    Vector2 _p = TurnSectorIntoRealPos(F._travelStart);
                    Vector2 _v = TurnSectorIntoRealPos(F._travelEnd) - _p;

                    Vector2 _currentPos = _p + (_v * F._travelCompleted);
                    Vector2 _currentSPos = PositionAsSectorPosition(_currentPos);

                    for (int i = 0; i < _map._fleets.Count; i++)
                    {
                        if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling)
                        {
                            Vector2 _p2 = TurnSectorIntoRealPos(F._travelStart);
                            Vector2 _v2 = TurnSectorIntoRealPos(F._travelEnd) - _p;

                            Vector2 _currentPos2 = _p + (_v * F._travelCompleted);
                            Vector2 _currentSPos2 = PositionAsSectorPosition(_currentPos2);

                            int _a = SectorDistance(_currentSPos, _currentSPos2);

                            if (_a <= F._FTLTransponderRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

        }

        return false;
    }

    public bool Fleet_IsOwnerKnown(int _fleet, int _faction)
    {
        _fleet = Mathf.Clamp(_fleet, -1, _map._fleets.Count - 1);

        if (_fleet < 0)
        {
            return false;
        }

        /* if (!_map._lockSelection)
        {
            return true;
        } */

        if (_map._playerFactionId < 0)
        {
            return true;
        }

        int _player = _faction;
        Fleet F = _map._fleets[_fleet];

        /* if (F._currentSector < 0 && !F._travelling)
        {
            return false;
        } */

        if (_player == F._faction)
        {
            return true; // FLEETS BELONGING TO A FACTION SHOULD ALWAYS BE VISIBLE
        }

        for (int i = 0; i < _map._factions[_player]._knownFleetOwners.Count; i++)
        {
            if (_map._factions[_player]._knownFleetOwners[i] == _fleet)
            {
                return true;
            }
        }

        if (!F._travelling) // Fleet is in System
        {


            if (F._currentSector >= 0 && _map._sectors[F._currentSector]._controlFaction == _player && F._transponder)
            {
                return true; // FLEETS IN A SYSTEM OWNED BY THAT FACTION ARE ALWAYS VISIBLE
            }

            for (int i = 0; i < _map._fleets.Count; i++)
            {
                if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling == false && _map._fleets[i]._currentSector >= 0 && _map._fleets[i]._currentSector == F._currentSector && F._transponder)
                {
                    return true; // FLEET IS VISIBLE IF A FLEET OF THE PLAYER IS ALSO IN THE SECTOR
                }
            }
        }
        else
        {
            if (F._FTLTransponder)
            {
                if (F._FTLTransponderRange >= 0)
                {
                    Vector2 _p = TurnSectorIntoRealPos(F._travelStart);
                    Vector2 _v = TurnSectorIntoRealPos(F._travelEnd) - _p;

                    Vector2 _currentPos = _p + (_v * F._travelCompleted);
                    Vector2 _currentSPos = PositionAsSectorPosition(_currentPos);

                    for (int i = 0; i < _map._fleets.Count; i++)
                    {
                        if (_map._fleets[i]._faction == _player && _map._fleets[i]._travelling)
                        {
                            Vector2 _p2 = TurnSectorIntoRealPos(F._travelStart);
                            Vector2 _v2 = TurnSectorIntoRealPos(F._travelEnd) - _p;

                            Vector2 _currentPos2 = _p + (_v * F._travelCompleted);
                            Vector2 _currentSPos2 = PositionAsSectorPosition(_currentPos2);

                            int _a = SectorDistance(_currentSPos, _currentSPos2);

                            if (_a <= F._FTLTransponderRange)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    return true;
                }
            }

        }

        return false;
    }

    public void AddFleet()
    {
        Fleet F = new Fleet();

        F._name = "Fleet";
        F._currentSector = -1;
        F._faction = -1;

        _map._fleets.Add(F);

        // RESET UI
        OutlinerUIGalaxy.Instance.RebuildFleetMenu();
    }

    public void AddFleet(int faction)
    {
        Fleet F = new Fleet();

        F._name = "Fleet";

        
        int _a = -1;
        for (int i = 0; i < _map._sectors.Count; i++)
        {
            if (_map._sectors[i]._controlFaction == faction)
            {
                _a = i;
                break;
            }
        }

        F._currentSector = _a;
        
            
        F._faction = faction;

        _map._fleets.Add(F);

        // RESET UI
        OutlinerUIGalaxy.Instance.RebuildFleetMenu();
    }

    public void RemoveFleet(int _fleet)
    {
        if (_fleet < 0 || _fleet>= _map._fleets.Count)
        {
            return;
        }

        _map._fleets.Remove(_map._fleets[_fleet]);

        // REMOVE FLEETS FROM LISTS
        for (int i = 0; i < _map._factions.Count; i++)
        {
            for (int j = 0; j < _map._factions[i]._knownFleets.Count; j++)
            {
                if (_map._factions[i]._knownFleets[j] == _fleet)
                {
                    _map._factions[i]._knownFleets.Remove(_map._factions[i]._knownFleets[j]);
                    j--;
                    continue;
                }
                else if (_map._factions[i]._knownFleets[j] > _fleet)
                {
                    _map._factions[i]._knownFleets[j]--;
                }
            }

            for (int j = 0; j < _map._factions[i]._knownFleetContents.Count; j++)
            {
                if (_map._factions[i]._knownFleetContents[j] == _fleet)
                {
                    _map._factions[i]._knownFleetContents.Remove(_map._factions[i]._knownFleetContents[j]);
                    j--;
                    continue;
                }
                else if (_map._factions[i]._knownFleetContents[j] > _fleet)
                {
                    _map._factions[i]._knownFleetContents[j]--;
                }
            }

            for (int j = 0; j < _map._factions[i]._knownFleetOwners.Count; j++)
            {
                if (_map._factions[i]._knownFleetOwners[j] == _fleet)
                {
                    _map._factions[i]._knownFleetOwners.Remove(_map._factions[i]._knownFleetOwners[j]);
                    j--;
                    continue;
                }
                else if (_map._factions[i]._knownFleetOwners[j] > _fleet)
                {
                    _map._factions[i]._knownFleetOwners[j]--;
                }
            }
        }

        // REMOVE FROM UI
        FleetUIGalaxy.Instance.ProcessRemovedID(_fleet);

        // RESET UI
        OutlinerUIGalaxy.Instance.RebuildFleetMenu();
        OutlinerUIGalaxy.Instance.INITIALIZE_INDIV_FLEET_MENU(-1);
    }


    public Vector2 TurnSectorIntoRealPos(Vector2 _a)
    {
        if (_a.x % 2 == 0)
        {
            return new Vector3(.75f * _a.x, .9f * _a.y, 0);
        }
        else
        {
            return new Vector3(.75f * _a.x, (.9f * _a.y) + .45f, 0);
        }
    }

    public bool Faction_Allied(int _f1, int _f2)
    {
        if (_f1 < 0 || _f1 >= _map._factions.Count)
        {
            return false;
        }

        if (_f2 < 0 || _f2 >= _map._factions.Count)
        {
            return false;
        }

        for (int i = 0; i < _map._factions[_f1]._repIds.Count; i++)
        {
            int id = _map._factions[_f1]._repIds[i];
            if ((_map._reps[id]._faction1 == _f1 || _map._reps[id]._faction2 == _f1) && (_map._reps[id]._faction1 == _f2 || _map._reps[id]._faction2 == _f2))
            {
                if (_map._reps[id]._specialVal == "Allied")
                {
                    return true;
                }
            }
        }


        return false;
    }

    public bool Faction_Hostile(int _f1, int _f2)
    {
        if (_f1 < 0 || _f1 >= _map._factions.Count)
        {
            return false;
        }

        if (_f2 < 0 || _f2 >= _map._factions.Count)
        {
            return false;
        }

        for (int i = 0; i < _map._factions[_f1]._repIds.Count; i++)
        {
            int id = _map._factions[_f1]._repIds[i];
            if ((_map._reps[id]._faction1 == _f1 || _map._reps[id]._faction2 == _f1) && (_map._reps[id]._faction1 == _f2 || _map._reps[id]._faction2 == _f2))
            {
                if (_map._reps[id]._specialVal == "War")
                {
                    return true;
                }
            }
        }


        return false;
    }

    public bool IsFaction(int _faction)
    {
        if (_faction < 0 || _faction >= _map._factions.Count)
        {
            return false;
        }

        if (_map._playerFactionId < 0)
        {
            return false;
        }

        if (_faction == _map._playerFactions[_map._playerFactionId]._regFactionID)
        {
            return true;
        }

        return false;
    }

    public bool Fleet_IsInKnownList(int _fleet, int _faction)
    {

        for (int i = 0; i < _map._factions[_faction]._knownFleets.Count; i++)
        {
            if (_map._factions[_faction]._knownFleets[i] == _fleet)
            {
                return true;
            }
        }

        return false;
    }

    public bool Fleet_IsInKnownOwnerList(int _fleet, int _faction)
    {

        for (int i = 0; i < _map._factions[_faction]._knownFleetOwners.Count; i++)
        {
            if (_map._factions[_faction]._knownFleetOwners[i] == _fleet)
            {
                return true;
            }
        }

        return false;
    }

    public Color32 Return_ShipConditionColor(Ship.Condition _c)
    {
        switch (_c)
        {
            default:
                return new Color32(0, 200, 25, 255);

            case Ship.Condition.OK:
                return new Color32(0, 200, 25, 255);

            case Ship.Condition.Damaged:
                return new Color32(200, 150, 0, 255);

            case Ship.Condition.HeavilyDamaged:
                return new Color32(200, 25, 0, 255);

            case Ship.Condition.Destroyed:
                return new Color32(100, 100, 100, 255);
        }
    }

    // MAP VALIDATION //
    public void ResetRefIDs()
    {
        // Sectors
        for (int i = 0; i < _map._sectors.Count; i++)
        {
            _map._sectors[i]._refID = i;
        }

        // Factions
        for (int i = 0; i < _map._factions.Count; i++)
        {
            _map._factions[i]._refId = i;
        }

        // Alliances
        for (int i = 0; i < _map._alliances.Count; i++)
        {
            _map._alliances[i]._refId = i;
        }
        
    }

    public void JumpgatePointPos(int _cID, out Vector2[] _pos)
    {
        _pos = new Vector2[2];

        // Set sector 1 & 2 positions

            float _s1X = 0;
            float _s1Y = 0;

            float _s2X = 0;
            float _s2Y = 0;

            if (_map._jumpGates[_cID]._sector1Id != -1)
            {
                Sector _sector1 = _map._sectors[_map._jumpGates[_cID]._sector1Id];
                _s1X = _sector1._posXInt;
                _s1Y = _sector1._posYInt;

                _map._jumpGates[_cID]._s1p = new Vector2(_sector1._posXInt, _sector1._posYInt);
            }
            else
            {
                _s1X = _map._jumpGates[_cID]._s1p.x;
                _s1Y = _map._jumpGates[_cID]._s1p.y;
            }

            if (_map._jumpGates[_cID]._sector2Id != -1)
            {
                Sector _sector2 = _map._sectors[_map._jumpGates[_cID]._sector2Id];
                _s2X = _sector2._posXInt;
                _s2Y = _sector2._posYInt;

                _map._jumpGates[_cID]._s2p = new Vector2(_sector2._posXInt, _sector2._posYInt);
            }
            else
            {
                _s2X = _map._jumpGates[_cID]._s2p.x;
                _s2Y = _map._jumpGates[_cID]._s2p.y;
            }

            // Calculate deltaX, deltaY and theta (using tan-1) for Sector 1 and 2

            float _deltaXs1 = (.75f * _s2X) - (.75f * _s1X);
            float _deltaXs2 = (.75f * _s1X) - (.75f * _s2X);

            float _sector1YPos = 0;
            float _sector2YPos = 0;

            if (_s1X * _s1X % 2 == 1)
            {
                _sector1YPos = .9f * _s1Y + .45f;
            }
            else
            {
                _sector1YPos = .9f * _s1Y;
            }

            if (_s2X*_s2X % 2 == 1)
            {
                _sector2YPos = .9f * _s2Y + .45f;
            }
            else
            {
                _sector2YPos = .9f * _s2Y;
            }

            float _deltaYs1 = _sector2YPos - _sector1YPos;
            float _deltaYs2 = _sector1YPos - _sector2YPos;

            float theta1 = Mathf.Atan2(_deltaYs1, _deltaXs1);
            float theta2 = Mathf.Atan2(_deltaYs2, _deltaXs2);

            Vector2 _p1 = new Vector3(_s1X*.75f + Mathf.Cos(theta1) * .3f, _sector1YPos + Mathf.Sin(theta1) * .3f);
            Vector2 _p2 = new Vector3(_s2X*.75f + Mathf.Cos(theta2) * .3f, _sector2YPos + Mathf.Sin(theta2) * .3f);

            _pos[0] = _p1;
            _pos[1] = _p2;

        return;
    }

    // LEGACY DISTRIBUTIONS //
    public void DistributeLegacyFuel()
    {
        for (int i = 0; i < _map._fleets.Count; i++)
        {
            if (_map._fleets[i]._currentFuel > 0)
            {
                float _1;
                _map._fleets[i].RefillFuel(_map._fleets[i]._currentFuel, out _1);
                _map._fleets[i]._currentFuel -= _map._fleets[i].getMaxFuel;
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Random.Range(0, 100000000).ToString("X"));

        ResetRefIDs();
    }

    // Update is called once per frame
    void Update()
    {
        if (_escapeMenuIncompatTriggered)
        {
            _escapeMenuIncompatTriggered = false;
        }
        

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
        GetExploredSectors();

        if (!_map._lockSelection)
        {
            RepManagerAdd();
            RepManagerRemove();
        }

        if (_galaxy)
        {
            _galaxyObject.SetActive(true);
        }
        else
        {
            _galaxyObject.SetActive(false);
        }


        if (_system)
        {
            if (_selectedStarSystem == -1)
            {
                _system = false;
                _galaxy = true;
                return;
            }

            _systemObject.SetActive(true);
        }
        else
        {
            _systemObject.SetActive(false);
        }

            
    }


}
