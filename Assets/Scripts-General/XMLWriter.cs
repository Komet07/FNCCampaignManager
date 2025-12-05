using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public class XMLWriter : MonoBehaviour
{
    #region Singleton
    public static XMLWriter Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public bool _save = false;
    public bool _load = false;
    public bool _export = false;
    public bool _bulk = false;
    public string _saveFileName = "save1";
    public string _exportFileName = "";
    public bool _exportLock = false;
    public int _playerFaction = -1;

    public float _secWaitTime = 5f;
    float _waitTime = 0;

    // File path object
    public GameObject _filePathCanvas;
    public Text _filePathCanvasText;

    void Save(string _name)
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "saves")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "saves"));
        }
        var serializer = new XmlSerializer(typeof(Map));
        using (var stream = new FileStream(Path.Combine(Application.persistentDataPath, "saves", _name+".xml"), FileMode.Create))
        {
            serializer.Serialize(stream, MapManager.Instance._map);
            Debug.Log(Path.Combine(Application.persistentDataPath, "saves", _name + ".xml"));
            _waitTime = 0;
            StartCoroutine(DisplayPath(Path.Combine(Application.persistentDataPath, "saves", _name + ".xml")));
        }
    }

    public IEnumerator DisplayPath(string _path)
    {
        while (_waitTime < _secWaitTime)
        {
            _filePathCanvas.SetActive(true);
            _filePathCanvasText.text = _path;
            _waitTime++;
            yield return new WaitForSeconds(1f);

        }
        _waitTime = 0;
        _filePathCanvas.SetActive(false);

    }
    
    public void Export(bool _lock, int _player, string _name)
    {
        Map _mapCopy = MapManager.Instance._map;

        _mapCopy._lockSelection = _lock;
        _mapCopy._playerFactionId = _player;
        
        Debug.Log(_player);

        if (_player >= 0 && _lock)
        {
            _mapCopy._debug = false;

            // Fleets
            for (int i = 0; i < _mapCopy._fleets.Count; i++)
            {
                int _playerF = _mapCopy._playerFactions[_player]._regFactionID;
                

                if (!MapManager.Instance.Fleet_IsVisible(i, _playerF))
                {
                    _mapCopy._fleets.Remove(_mapCopy._fleets[i]);

                    
                    // REMOVE FLEETS FROM LISTS
                    for (int j = 0; j < _mapCopy._factions.Count; j++)
                    {
                        Debug.Log("A" + Random.Range(0, 10000));
                        for (int k = 0; k < _mapCopy._factions[j]._knownFleets.Count; k++)
                        {
                            if (_mapCopy._factions[j]._knownFleets[k] == i)
                            {
                                _mapCopy._factions[j]._knownFleets.Remove(_mapCopy._factions[j]._knownFleets[k]);
                                k--;
                                continue;
                            }
                            else if (_mapCopy._factions[j]._knownFleets[k] > i)
                            {
                                _mapCopy._factions[j]._knownFleets[k]--;
                            }
                        }
                        Debug.Log("B" + Random.Range(0, 10000));
                        for (int k = 0; k < _mapCopy._factions[j]._knownFleetContents.Count; k++)
                        {
                            if (_mapCopy._factions[j]._knownFleetContents[k] == i)
                            {
                                _mapCopy._factions[j]._knownFleetContents.Remove(_mapCopy._factions[j]._knownFleetContents[k]);
                                k--;
                                continue;
                            }
                            else if (_mapCopy._factions[j]._knownFleetContents[k] > i)
                            {
                                _mapCopy._factions[j]._knownFleetContents[k]--;
                            }
                        }
                        Debug.Log("C" + Random.Range(0, 10000));
                        for (int k = 0; k < _mapCopy._factions[j]._knownFleetOwners.Count; k++)
                        {
                            if (_mapCopy._factions[j]._knownFleetOwners[k] == i)
                            {
                                _mapCopy._factions[j]._knownFleetOwners.Remove(_mapCopy._factions[j]._knownFleetOwners[k]);
                                k--;
                                continue;
                            }
                            else if (_mapCopy._factions[j]._knownFleetOwners[k] > i)
                            {
                                _mapCopy._factions[j]._knownFleetOwners[k]--;
                            }
                        }
                    }

                    i--;
                    continue;
                }
                else if (!MapManager.Instance.Fleet_IsOwnerKnown(i, _playerF))
                {
                    _mapCopy._fleets[i]._name = "";
                    _mapCopy._fleets[i]._faction = -1;
                }

                if (!MapManager.Instance.IsFaction(_mapCopy._fleets[i]._faction))
                {
                    _mapCopy._fleets[i]._maxFuel = 0;
                    _mapCopy._fleets[i]._status = "";
                    _mapCopy._fleets[i]._currentFuel = 0;
                }
            }
            
            // Remove all unknown Sectors
            for (int i = 0; i < _mapCopy._sectors.Count; i++)
            {
                int _playerF = _mapCopy._playerFactions[_player]._regFactionID;

                bool _disco = false;
                bool _kSO = false;
                bool _explored = false;
                for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._discoveredSectors.Count; j++)
                {
                    if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._discoveredSectors[j] == i)
                    {
                        _disco = true;
                    }

                }
                for (int j = 0; j < _mapCopy._fleets.Count; j++)
                {
                    if (_mapCopy._fleets[j]._faction == _playerF && _mapCopy._fleets[j]._currentSector == i && _mapCopy._fleetRevealSectors)
                    {
                        _disco = true;
                        _explored = true;
                    }
                }
                if (_mapCopy._sectors[i]._controlFaction == _mapCopy._playerFactions[_player]._regFactionID)
                {
                    _disco = true;
                    _kSO = true;
                    _explored = true;
                }
                
                for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownSectorOwnership.Count; j++)
                {
                    if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownSectorOwnership[j] == i)
                    {
                        _kSO = true;
                    }

                }
                
                for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._exploredSectors.Count; j++)
                {
                    if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._exploredSectors[j] == i)
                    {
                        _explored = true;
                    }

                }
                
                if (!_disco)
                {
                    
                    // Remove Sector from _sector List
                    _mapCopy._sectors.Remove(_mapCopy._sectors[i]);

                    // Update _sector ref IDs
                    for (int j = 0; j < _mapCopy._sectors.Count; j++)
                    {
                        if (_mapCopy._sectors[j]._refID > i)
                        {
                            _mapCopy._sectors[j]._refID--;
                        }
                    }

                    // Update JG Connections
                    for (int j = 0; j < _mapCopy._jumpGates.Count; j++)
                    {
                        if (_mapCopy._jumpGates[j]._sector1Id == i || !_mapCopy._jumpGates[j]._discoverable1)
                        {
                            _mapCopy._jumpGates[j]._sector1Id = -1;
                            _mapCopy._jumpGates[j]._name = "";
                            _mapCopy._jumpGates[j]._name2 = "Gate: Unknown";
                        }
                        else if (_mapCopy._jumpGates[j]._sector1Id > i)
                        {
                            _mapCopy._jumpGates[j]._sector1Id--;
                        }

                        if (_mapCopy._jumpGates[j]._sector2Id == i || !_mapCopy._jumpGates[j]._discoverable2)
                        {
                            _mapCopy._jumpGates[j]._sector2Id = -1;
                            _mapCopy._jumpGates[j]._name = "";
                            _mapCopy._jumpGates[j]._name1 = "Gate: Unknown";
                        }
                        else if (_mapCopy._jumpGates[j]._sector2Id > i)
                        {
                            _mapCopy._jumpGates[j]._sector2Id--;
                        }
                    }


                    // Update PlayerFaction _exploredSectors
                    for (int j = 0; j < _mapCopy._factions.Count; j++)
                    {
                        // Update PlayerFaction _discoSectors
                        for (int k = 0; k < _mapCopy._factions[j]._discoveredSectors.Count; k++)
                        {
                            if (_mapCopy._factions[j]._discoveredSectors[k] == i)
                            {
                                _mapCopy._factions[j]._discoveredSectors.Remove(_mapCopy._factions[j]._discoveredSectors[k]);
                                k--;
                            }
                            else if (_mapCopy._factions[j]._discoveredSectors[k] > i)
                            {
                                _mapCopy._factions[j]._discoveredSectors[k]--;
                            }
                        }
                        // Update PlayerFaction _exploredSectors
                        for (int k = 0; k < _mapCopy._factions[j]._exploredSectors.Count; k++)
                        {
                            if (_mapCopy._factions[j]._exploredSectors[k] == i)
                            {
                                _mapCopy._factions[j]._exploredSectors.Remove(_mapCopy._factions[j]._exploredSectors[k]);
                                k--;
                            }
                            else if (_mapCopy._factions[j]._exploredSectors[k] >= i)
                            {
                                _mapCopy._factions[j]._exploredSectors[k] = _mapCopy._factions[j]._exploredSectors[k]-1;
                            }
                        }
                        // Update PlayerFaction _knownSectorOwnership
                        for (int k = 0; k < _mapCopy._factions[j]._knownSectorOwnership.Count; k++)
                        {
                            if (_mapCopy._factions[j]._knownSectorOwnership[k] == i)
                            {
                                _mapCopy._factions[j]._knownSectorOwnership.Remove(_mapCopy._factions[j]._knownSectorOwnership[k]);
                                k--;
                            }
                            else if (_mapCopy._factions[j]._knownSectorOwnership[k] > i)
                            {
                                _mapCopy._factions[j]._knownSectorOwnership[k]--;
                            }
                        }
                        // Update PlayerFaction _liveFeed
                        for (int k = 0; k < _mapCopy._factions[j]._sectorLiveFeeds.Count; k++)
                        {
                            if (_mapCopy._factions[j]._sectorLiveFeeds[k] == i)
                            {
                                _mapCopy._factions[j]._sectorLiveFeeds.Remove(_mapCopy._factions[j]._sectorLiveFeeds[k]);
                                k--;
                            }
                            else if (_mapCopy._factions[j]._sectorLiveFeeds[k] > i)
                            {
                                _mapCopy._factions[j]._sectorLiveFeeds[k]--;
                            }
                        }
                    }

                    // UPDATE PLAYER FACTIONS

                    // UPDATE FLEETS
                    for (int j = 0; j < _mapCopy._fleets.Count; j++)
                    {
                        if (_mapCopy._fleets[j]._currentSector == i)
                        {
                            _mapCopy._fleets[j]._currentSector = -1;
                        }
                        else if (_mapCopy._fleets[j]._currentSector > i)
                        {
                            _mapCopy._fleets[j]._currentSector--;
                        }
                    }

                    // LOWER I BY 1
                    i--;
                    continue;
                }
                else if (!_kSO)
                {
                    _mapCopy._sectors[i]._controlFaction = -1;
                    _mapCopy._sectors[i]._hiddenName = "";
                }
                
                if (!_explored)
                {
                    for (int j = 0; j < _mapCopy._jumpGates.Count; j++)
                    {
                        if (_mapCopy._jumpGates[j]._sector1Id == i || !_mapCopy._jumpGates[j]._discoverable1)
                        {
                            _mapCopy._jumpGates[j]._sector1Id = -1;
                            _mapCopy._jumpGates[j]._name = "";
                            _mapCopy._jumpGates[j]._name2 = "Gate: Unknown";
                        }
                        if (_mapCopy._jumpGates[j]._sector2Id == i || !_mapCopy._jumpGates[j]._discoverable2)
                        {
                            _mapCopy._jumpGates[j]._sector2Id = -1;
                            _mapCopy._jumpGates[j]._name = "";
                            _mapCopy._jumpGates[j]._name1 = "Gate: Unknown";
                        }
                    }
                }
                
            }
            
            // Remove all totally invisible jump gate links
            for (int i = 0; i < _mapCopy._jumpGates.Count; i++)
            {
                if (_mapCopy._jumpGates[i]._sector1Id == -1 && _mapCopy._jumpGates[i]._sector2Id == -1)
                {
                    _mapCopy._jumpGates.Remove(_mapCopy._jumpGates[i]);
                    i--;
                }

                if (!_mapCopy._jumpGates[i].Point1Vis(MapManager.Instance._map._playerFactions[_player]._regFactionID) && !_mapCopy._jumpGates[i].Point2Vis(MapManager.Instance._map._playerFactions[_player]._regFactionID))
                {
                    _mapCopy._jumpGates.Remove(_mapCopy._jumpGates[i]);
                    i--;
                }
            }

            // Remove all unknown connection types (JGs)
            for (int i = 0; i < _mapCopy._connType.Count; i++)
            {
                bool _known = false;
                for (int j = 0; j < _mapCopy._jumpGates.Count; j++)
                {
                    if (_mapCopy._jumpGates[j]._typeId == i)
                    {
                        _known = true;
                    }
                }

                if (!_known)
                {
                    _mapCopy._connType.Remove(_mapCopy._connType[i]);

                    for (int j = 0; j < _mapCopy._jumpGates.Count; j++)
                    {
                        if (_mapCopy._jumpGates[j]._typeId > i)
                        {
                            _mapCopy._jumpGates[j]._typeId--;
                        }
                    }

                    i--;
                }
            }
            
            // Remove all unknown factions
            for (int i = 0; i < _mapCopy._factions.Count; i++)
            {

                bool _known = false;
                for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownFactions.Count; j++)
                {
                    if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownFactions[j] == i)
                    {
                        _known = true;
                    }
                }

                if (_mapCopy._playerFactions[_player]._regFactionID == i)
                {
                    _known = true;
                }

                if (!_known)
                {
                    // Remove faction
                    _mapCopy._factions.Remove(_mapCopy._factions[i]);

                    // Update ref ids on other factions
                    for (int j = 0; j < _mapCopy._factions.Count; j++)
                    {
                        if (_mapCopy._factions[j]._refId > i)
                        {
                            _mapCopy._factions[j]._refId--;
                        }
                    }

                    for (int j = 0; j < _mapCopy._playerFactions.Count; j++)
                    {
                        if (_mapCopy._playerFactions[j]._regFactionID > i)
                        {
                            _mapCopy._playerFactions[j]._regFactionID--;
                        }
                    }

                    // Set all sectors referencing the faction to -1 and update Ids of other factions
                    for (int j = 0; j < _mapCopy._sectors.Count; j++)
                    {
                        if (_mapCopy._sectors[j]._controlFaction == i)
                        {
                            _mapCopy._sectors[j]._controlFaction = -1;
                        }
                        else if (_mapCopy._sectors[j]._controlFaction > i)
                        {
                            _mapCopy._sectors[j]._controlFaction--;
                        }
                    }

                    // UPDATE FLEETS
                    for (int j = 0; j < _mapCopy._fleets.Count; j++)
                    {
                        if (_mapCopy._fleets[j]._faction == i)
                        {
                            _mapCopy._fleets[j]._faction = -1;
                        }
                        else if (_mapCopy._fleets[j]._faction > i)
                        {
                            _mapCopy._fleets[j]._faction--;
                        }
                    }

                    // Update membership IDs in alliances
                    for (int j = 0; j < _mapCopy._alliances.Count; j++)
                    {
                        for (int k = 0; k < _mapCopy._alliances[j]._memberStates.Count; k++)
                        {
                            if (_mapCopy._alliances[j]._memberStates[k] == i)
                            {
                                _mapCopy._alliances[j]._memberStates.Remove(_mapCopy._alliances[j]._memberStates[k]);
                                k--;
                            }
                            else if (_mapCopy._alliances[j]._memberStates[k] > i)
                            {
                                _mapCopy._alliances[j]._memberStates[k]--;
                            }
                        }

                    }

                    // Remove faction from _knownFaction lists
                    for (int j = 0; j < _mapCopy._factions.Count; j++)
                    {
                        for (int k = 0; k < _mapCopy._factions[j]._knownFactions.Count; k++)
                        {
                            if (_mapCopy._factions[j]._knownFactions[k] == i)
                            {
                                _mapCopy._factions[j]._knownFactions.Remove(_mapCopy._factions[j]._knownFactions[k]);
                                k--;
                            }
                            else if (_mapCopy._factions[j]._knownFactions[k] > i)
                            {
                                _mapCopy._factions[j]._knownFactions[k]--;
                            }
                        }
                    }

                    // Remove reps referencing that faction
                    for (int j = 0; j < _mapCopy._reps.Count; j++)
                    {
                        if (_mapCopy._reps[j]._faction1 == i || _mapCopy._reps[j]._faction2 == i)
                        {
                            _mapCopy._reps.Remove(_mapCopy._reps[j]);


                            for (int k = 0; k < _mapCopy._factions.Count; k++)
                            {
                                for (int l = 0; l < _mapCopy._factions[k]._repIds.Count; l++)
                                {
                                    if (_mapCopy._factions[k]._repIds[l] == j)
                                    {
                                        _mapCopy._factions[k]._repIds.Remove(_mapCopy._factions[k]._repIds[l]);
                                        l--;
                                    }
                                    else if (_mapCopy._factions[k]._repIds[l] > j)
                                    {
                                        _mapCopy._factions[k]._repIds[l]--;
                                    }
                                }
                            }


                            j--;
                        }
                        else
                        {
                            if (_mapCopy._reps[j]._faction1 > i)
                            {
                                _mapCopy._reps[j]._faction1--;
                            }

                            if (_mapCopy._reps[j]._faction2 > i)
                            {
                                _mapCopy._reps[j]._faction2--;
                            }
                        }
                    }
                    i--;
                }
            }
            
            // Remove Alliances with 0 members
            for (int i = 0; i < _mapCopy._alliances.Count; i++)
            {
                bool _known = false;

                for (int j = 0; j < _mapCopy._factions.Count; j++)
                {
                    if (_mapCopy._factions[j]._allianceId == i)
                    {
                        _known = true;
                    }
                }

                if (!_known)
                {
                    _mapCopy._alliances.Remove(_mapCopy._alliances[i]);

                    for (int j = 0; j < _mapCopy._factions.Count; j++)
                    {
                        if (_mapCopy._factions[j]._allianceId == i)
                        {
                            _mapCopy._factions[j]._allianceId = -1;
                        }
                        else if (_mapCopy._factions[j]._allianceId > i)
                        {
                            _mapCopy._factions[j]._allianceId--;
                        }
                    }

                    i--;
                }

                
            }
            
            // Remove all reps between non-player factions
            for (int i = 0; i < _mapCopy._reps.Count; i++)
            {
                if (_mapCopy._reps[i]._faction1 != _mapCopy._playerFactions[_mapCopy._playerFactionId]._regFactionID && _mapCopy._reps[i]._faction2 != _mapCopy._playerFactions[_mapCopy._playerFactionId]._regFactionID)
                {
                    _mapCopy._reps.Remove(_mapCopy._reps[i]);
                    i--;

                    for (int j = 0; j < _mapCopy._factions.Count; j++)
                    {
                        for (int k = 0; k < _mapCopy._factions[j]._repIds.Count; k++)
                        {
                            if(_mapCopy._factions[j]._repIds[k] == i)
                            {
                                _mapCopy._factions[j]._repIds.Remove(_mapCopy._factions[j]._repIds[k]);
                                k--;
                            }
                            else if (_mapCopy._factions[j]._repIds[k] > i)
                            {
                                _mapCopy._factions[j]._repIds[k]--;
                            }
                        }
                    }
                }
            }
            
            // Remove all Region Categories that aren't known at all *or* Regions that aren't visible anyways
            for (int i = 0; i < _mapCopy._regCats.Count; i++)
            {
                
                bool _vis = false;
                int _t = _mapCopy._regCats[i]._knowledgeType;
                // 0 = Explored Sectors
                // 1 = Known Sector Owner
                if (_t == 0)
                {
                    bool _inc = false;
                    for (int k = 0; k < _mapCopy._sectors.Count; k++)
                    {
                        for (int j = 0; j < _mapCopy._sectors[k]._regionCats.Count; j++)
                        {
                            if (_mapCopy._sectors[k]._regionCats[j] == i)
                            {
                                _inc = true;
                            }
                        }
                        if (!_vis && _inc)
                        {
                            for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._exploredSectors.Count; j++)
                            {
                                if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._exploredSectors[j] == k)
                                {
                                    _vis = true;
                                }

                            }
                        }

                    }
                }
                else if (_t == 1)
                {
                    bool _inc = false;
                    for (int k = 0; k < _mapCopy._sectors.Count; k++)
                    {
                        
                        for (int j = 0; j < _mapCopy._sectors[k]._regionCats.Count; j++)
                        {
                            if (_mapCopy._sectors[k]._regionCats[j] == i)
                            {
                                _inc = true;
                            }
                        }
                        if (!_vis && _inc)
                        {
                            for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownSectorOwnership.Count; j++)
                            {
                                if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownSectorOwnership[j] == k)
                                {
                                    _vis = true;
                                }

                            }
                        }
                            
                    }
                }
                Debug.Log(_vis);
                if (!_vis) // Entire region category not known at all
                {
                    // Remove Region Category
                    _mapCopy._regCats.Remove(_mapCopy._regCats[i]);

                    // Remove all entries denoting the Region Category in sectors
                    for (int j = 0; j < _mapCopy._sectors.Count; j++)
                    {
                        for (int k = 0; k < _mapCopy._sectors[j]._regionCats.Count; k++)
                        {
                            if (_mapCopy._sectors[j]._regionCats[k] == i)
                            {
                                _mapCopy._sectors[j]._regionCats.Remove(_mapCopy._sectors[j]._regionCats[k]);
                                _mapCopy._sectors[j]._regionCatsRegionIds.Remove(_mapCopy._sectors[j]._regionCatsRegionIds[k]);
                                k--;
                            }
                            else if (_mapCopy._sectors[j]._regionCats[k] > i)
                            {
                                _mapCopy._sectors[j]._regionCats[k]--;
                            }
                        }
                    }

                    i--;
                }
                else // Search for specifc unknown regions
                {
                    for (int m = 0; m < _mapCopy._regCats[i]._regions.Count; m++)
                    {
                        bool _visB = false;
                        if (_t == 0) // Explored Sectors
                        {
                            bool _inc = false;
                            for (int k = 0; k < _mapCopy._sectors.Count; k++)
                            {
                                
                                for (int j = 0; j < _mapCopy._sectors[k]._regionCats.Count; j++)
                                {
                                    if (_mapCopy._sectors[k]._regionCats[j] == i && _mapCopy._sectors[k]._regionCatsRegionIds[j] == m)
                                    {
                                        _inc = true;
                                    }
                                }
                                if (!_visB && _inc)
                                {
                                    for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._exploredSectors.Count; j++)
                                    {
                                        if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._exploredSectors[j] == k)
                                        {
                                            _visB = true;
                                        }

                                    }
                                }

                            }
                        }
                        else if (_t == 1) // Known Sector Owners
                        {
                            bool _inc = false;
                            for (int k = 0; k < _mapCopy._sectors.Count; k++)
                            {
                                
                                for (int j = 0; j < _mapCopy._sectors[k]._regionCats.Count; j++)
                                {
                                    if (_mapCopy._sectors[k]._regionCats[j] == i && _mapCopy._sectors[k]._regionCatsRegionIds[j] == m)
                                    {
                                        _inc = true;
                                    }
                                }
                                if (!_visB && _inc)
                                {
                                    for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownSectorOwnership.Count; j++)
                                    {
                                        if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._knownSectorOwnership[j] == k)
                                        {
                                            _visB = true;
                                        }

                                    }
                                }

                            }
                        }

                        if (!_visB)
                        {
                            // Remove Region
                            _mapCopy._regCats[i]._regions.Remove(_mapCopy._regCats[i]._regions[m]);

                            // Set references to this region to -1, -- for all higher ones
                            for (int j = 0; j < _mapCopy._sectors.Count; j++)
                            {
                                for (int k = 0; k < _mapCopy._sectors[j]._regionCats.Count; k++)
                                {
                                    if (_mapCopy._sectors[j]._regionCats[k] == i && _mapCopy._sectors[j]._regionCatsRegionIds[k] == m)
                                    {
                                        _mapCopy._sectors[j]._regionCatsRegionIds[k] = -1;
                                    }
                                    else if (_mapCopy._sectors[j]._regionCats[k] == i && _mapCopy._sectors[j]._regionCatsRegionIds[k] > m)
                                    {
                                        _mapCopy._sectors[j]._regionCatsRegionIds[k]--;
                                    }
                                }
                            }

                            m--;
                        }
                    }
                }
                
            }
            
            // Remove all Player faction stuff 
            for (int i = 0; i < _mapCopy._playerFactions.Count; i++)
            {
                // Remove Anchor positions from non-viewpoint player factions
                if (i != _player)
                {
                    _mapCopy._playerFactions[i]._spawnPosition = new Vector2(0, 0);
                }
            }
            
        }

        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "exports")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "exports"));
        }
        var serializer = new XmlSerializer(typeof(Map));
        using (var stream = new FileStream(Path.Combine(Application.persistentDataPath, "exports", _name + ".xml"), FileMode.Create))
        {
            
            serializer.Serialize(stream, _mapCopy);
            Debug.Log(Path.Combine(Application.persistentDataPath, "exports", _name + ".xml"));
            _waitTime = 0;
            if (!_bulk)
            {
                StartCoroutine(DisplayPath(Path.Combine(Application.persistentDataPath, "exports", _name + ".xml")));
            }
                

        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        if (SceneSwitch.Instance != null && SceneSwitch.Instance._load)
        {
            _load = true;
            _saveFileName = SceneSwitch.Instance._saveName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_save)
        {
            _save = false;
            Save(_saveFileName);
        }

        if (_load)
        {
            _load = false;
            var _map = Map.Load(Path.Combine(Application.persistentDataPath, "saves", _saveFileName+".xml"));
            MapManager.Instance._map = _map;

            MapManager.Instance.ResetRefIDs();

            GalaxyMap.Instance._regen = true;
            GalaxyMap.Instance._regen2 = true;
            CamMovement.Instance._reset = true;
        }

        if (_export)
        {
            _export = false;
            Save("buffer");

            if (_bulk)
            {
                for (int i = 0; i < MapManager.Instance._map._playerFactions.Count; i++)
                {
                    Export(true, i, _exportFileName+"_"+MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[i]._regFactionID]._shorthand);
                    var _map2 = Map.Load(Path.Combine(Application.persistentDataPath, "saves", "buffer.xml"));
                    MapManager.Instance._map = _map2;
                }

                StartCoroutine(DisplayPath(Path.Combine(Application.persistentDataPath, "exports", _exportFileName + "_*" + ".xml")));
            }
            else
            {
                Export(_exportLock, _playerFaction, _exportFileName);
            }
            
            var _map = Map.Load(Path.Combine(Application.persistentDataPath, "saves", "buffer.xml"));
            MapManager.Instance._map = _map;

            GalaxyMap.Instance._regen = true;
            GalaxyMap.Instance._regen2 = true;
        }
    }
}
