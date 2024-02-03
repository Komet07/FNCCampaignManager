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
            // Remove all unknown Sectors
            for (int i = 0; i < _mapCopy._sectors.Count; i++)
            {
                bool _disco = false;
                for (int j = 0; j < _mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._discoveredSectors.Count; j++)
                {
                    if (_mapCopy._factions[_mapCopy._playerFactions[_player]._regFactionID]._discoveredSectors[j] == i)
                    {
                        _disco = true;
                    }
                }
                if (_mapCopy._sectors[i]._controlFaction == _mapCopy._playerFactions[_player]._regFactionID)
                {
                    _disco = true;
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
                    i--;
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
                            j--;

                            for (int k = 0; k < _mapCopy._factions.Count; k++)
                            {
                                for (int l = 0; l < _mapCopy._factions[k]._repIds.Count; l++)
                                {
                                    if (_mapCopy._factions[k]._repIds[l] == j)
                                    {
                                        _mapCopy._factions[k]._repIds.Remove(_mapCopy._factions[k]._repIds[l]);
                                        k--;
                                    }
                                    else if (_mapCopy._factions[k]._repIds[l] > j)
                                    {
                                        _mapCopy._factions[k]._repIds[l]--;
                                    }
                                }
                            }
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
                if (_mapCopy._alliances[i]._memberStates.Count == 0)
                {
                    _mapCopy._alliances.Remove(_mapCopy._alliances[i]);
                    i--;
                }
            }

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
            StartCoroutine(DisplayPath(Path.Combine(Application.persistentDataPath, "exports", _name + ".xml")));

        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
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

            GalaxyMap.Instance._regen = true;
        }

        if (_export)
        {
            _export = false;
            Save("buffer");
            Export(_exportLock, _playerFaction, _exportFileName);
            var _map = Map.Load(Path.Combine(Application.persistentDataPath, "saves", "buffer.xml"));
            MapManager.Instance._map = _map;

            GalaxyMap.Instance._regen = true;
        }
    }
}
