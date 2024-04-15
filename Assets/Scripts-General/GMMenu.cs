using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GMMenu : MonoBehaviour
{

    #region Singleton
    public static GMMenu Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion


    // Main Menu
    public GameObject _buttonDebug;
    public GameObject _buttonExport;
    public GameObject _buttonMenu;
    public GameObject _buttonRender;
    public GameObject _buttonSettings;

    public GameObject _menuMain;

    // Export Menu Objects
    public GameObject _menuExport;

    // Object Menu stuff
    public GameObject _menuObjects;
    public GameObject _menuObjectL1Button; // Original Menu Object L1 button
    public GameObject _menuObjectL2Button; // Original Menu Object L2 button

    public GameObject _menuObjectsL2; // Menu for 2nd layer of stuff (List of sectors, Connections, etc.)
    public int _currentL2Int = 0;
    public int _currentL3Int = -1;
    public int _currentL3SubInt = 0;

    bool _mObjectsm1Built = false;

    int _mObjectsm2Min = 0;
    int _mObjectsm2Max = 13;
    public int _mObjectsm2Offset = 0;
    int _mObjectsm2PrevOffset = 0;
    public float _mObjectsm2OffsetVal = 0;
    float _mObjectsm2ScrollSpeed = 5f;

    // Object Menu S3 Sector
    public GameObject _menuObjectsL3Sector;

    bool _menuObjectsL3SectorNameActive = false;
    bool _menuObjectsL3SectorPlacementActive = false;
    bool _menuObjectsL3SectorDescActive = false;
    bool _menuObjectsL3SectorLoreActive = false;

    int _sectorExtraId = 0;
    int _sectorRegionInt = 0;

    // Object Menu S3 Faction
    public GameObject _menuObjectsL3Faction;

    bool _menuObjectsL3FactionNameActive = false;
    bool _menuObjectsL3FactionShorthandActive = false;
    bool _menuObjectsL3FactionRedActive = false;
    bool _menuObjectsL3FactionGreenActive = false;
    bool _menuObjectsL3FactionBlueActive = false;

    int _factionInt = 0;

    // Object Menu S3 Alliance
    public GameObject _menuObjectsL3Alliance;

    bool _menuObjectsL3AllianceNameActive = false;
    bool _menuObjectsL3AllianceShorthandActive = false;
    bool _menuObjectsL3AllianceRedActive = false;
    bool _menuObjectsL3AllianceGreenActive = false;
    bool _menuObjectsL3AllianceBlueActive = false;

    // Object Menu S3 Jumpgates
    public GameObject _menuObjectsL3Jumpgate;

    bool _menuObjectsL3JumpgateNameActive = false;
    public bool _mOL3JumpGatesS1PlacementActive = false;
    public bool _mOL3JumpGatesS2PlacementActive = false;

    // Object Menu S3 Player Factions
    public GameObject _menuObjectsL3PlayerFaction;

    // Object Menu S3 Region
    public GameObject _menuObjectsL3Region;

    bool _menuObjectsL3RegionNameActive = false;
    bool _menuObjectsL3SubRegionNameActive = false;

    bool _menuObjectsL3SubRegionRedActive = false;
    bool _menuObjectsL3SubRegionGreenActive = false;
    bool _menuObjectsL3SubRegionBlueActive = false;

    
    int _subRegionInt = 0;

    // Object Menu Context menu
    public GameObject _contextMenu;
    public GameObject _menuObjectContextButton; // Original Context Menu Button

    public List<GameObject> _contextMenuObjects = new List<GameObject>() { };

    public int _mObjectsContextOffset = 0;
    int _mObjectsContextPrevOffset = 0;
    public float _mObjectsContextOffsetVal = 0;
    float _mObjectsContextScrollSpeed = 5f;
    bool _contextSelection = false;

    int _currentContextInt = 0;
    int _currentActiveContext = 0;

    // Object Menu Object Lists

    public List<GameObject> _menuObject1Objects = new List<GameObject>() { };
    public List<GameObject> _menuObject2Objects = new List<GameObject>() { };
    public List<string> _menuObject1Themes = new List<string>() { };

    // Map Settings
    public GameObject _settingsMenu;

    // Export variables
    bool _lockSel = false;
    int _playerFaction = -1;

    bool _activeInputExportM = false;

    void SelectAll()
    {
        if (_menuObjectsL3Faction.activeSelf)
        {
            if (_currentActiveContext == 0) // Discovered Sector
            {
                for (int j = 0; j < MapManager.Instance._map._sectors.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (!c) // Add Assignment
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Add(j);
                    }
                }
            }
            else if (_currentActiveContext == 1) // Explored Sector
            {
                for (int j = 0; j < MapManager.Instance._map._sectors.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (!c) // Add Assignment
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Add(j);
                    }
                }
            }
            else if (_currentActiveContext == 2) // Known Sector Owner Sector
            {
                for (int j = 0; j < MapManager.Instance._map._sectors.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (!c) // Add Assignment
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Add(j);
                    }
                }
            }
            else if (_currentActiveContext == 3) // Known Factions
            {
                for (int j = 0; j < MapManager.Instance._map._factions.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._knownFactions[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (!c) // Add Assignment
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Add(j);
                    }
                }
            }
        }
        else if (_menuObjectsL3Sector.activeSelf)
        {
            if (_currentActiveContext == 0) // Region Category Assignment
            {
                for (int a = 0; a < MapManager.Instance._map._regCats.Count; a++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count; i++)
                    {
                        if (MapManager.Instance._map._sectors[_currentL3Int]._regionCats[i] == a)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (!c) // Select all
                    {
                        MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Add(a);
                        MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds.Add(-1);
                    }
                }
            }
        }
    }

    void DeselectAll()
    {
        if (_menuObjectsL3Faction.activeSelf)
        {
            if (_currentActiveContext == 0) // Discovered Sector
            {
                for (int j = 0; j < MapManager.Instance._map._sectors.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[i] == j)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Remove(MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[i]);
                                i--;
                            }
                        }
                    }
                }
            }
            else if (_currentActiveContext == 1) // Explored Sector
            {
                for (int j = 0; j < MapManager.Instance._map._sectors.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[i] == j)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Remove(MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[i]);
                                i--;
                            }
                        }
                    }
                }
            }
            else if (_currentActiveContext == 2) // Known Sector Owner Sector
            {
                for (int j = 0; j < MapManager.Instance._map._sectors.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[i] == j)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Remove(MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[i]);
                                i--;
                            }
                        }
                    }
                }
            }
            else if (_currentActiveContext == 3) // Known Factions
            {
                for (int j = 0; j < MapManager.Instance._map._factions.Count; j++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._knownFactions[i] == j)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._knownFactions[i] == j)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Remove(MapManager.Instance._map._factions[_currentL3Int]._knownFactions[i]);
                                i--;
                            }
                        }
                    }
                }
            }
        }
        else if (_menuObjectsL3Sector.activeSelf)
        {
            if (_currentActiveContext == 0) // Region Category Assignment
            {
                for (int a = 0; a < MapManager.Instance._map._regCats.Count; a++)
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count; i++)
                    {
                        if (MapManager.Instance._map._sectors[_currentL3Int]._regionCats[i] == a)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Select all
                    {
                        for (int i = 0; i < MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count; i++)
                        {
                            if (MapManager.Instance._map._sectors[_currentL3Int]._regionCats[i] == a)
                            {
                                MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Remove(MapManager.Instance._map._sectors[_currentL3Int]._regionCats[i]);
                                MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds.Remove(MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[i]);
                                i--;
                            }
                        }
                    }
                }
            }
        }
    }

    void DebugOnOff()
    {
        if (MapManager.Instance._map._debug)
        {
            MapManager.Instance._map._debug = false;
        }
        else
        {
            MapManager.Instance._map._debug = true;
        }
    }

    void ExportMenuOnOff()
    {
        if (_menuExport.activeSelf)
        {
            _menuExport.SetActive(false);
            _activeInputExportM = false;
        }
        else
        {
            _menuExport.SetActive(true);
            
        }
    }

    void ObjectMenuOnOff()
    {
        if (_menuObjects.activeSelf)
        {
            _menuObjects.SetActive(false);
            _menuObjectsL2.SetActive(false);
            _menuObjectsL3Sector.SetActive(false);
            _menuObjectsL3Alliance.SetActive(false);
            _menuObjectsL3Faction.SetActive(false);
            _menuObjectsL3Jumpgate.SetActive(false);
            _menuObjectsL3PlayerFaction.SetActive(false);
            _contextMenu.SetActive(false);
        }
        else
        {
            _menuObjects.SetActive(true);

        }
    }

    void RebuildObjectMenuL1()
    {
        // Destroy old buttons
        List<GameObject> _buttons = _menuObject1Objects;
        _menuObject1Objects = new List<GameObject>();
        for (int i = 0; i < _buttons.Count; i++)
        {
            Destroy(_buttons[i]);
        }

        // Generate new buttons
        for (int i = 0; i < _menuObject1Themes.Count; i++)
        {
            GameObject _buttonClone = Instantiate(_menuObjectL1Button, _menuObjects.transform);
            _buttonClone.SetActive(true);

            _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = _menuObject1Themes[i];
            _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20*_menuObject1Objects.Count);

            _menuObject1Objects.Add(_buttonClone);
        }


    }


    void RebuildObjectMenuL2(int a)
    {
        _menuObjectsL2.SetActive(true);
        _currentL2Int = a;

        // Destroy old buttons
        List<GameObject> _buttons = _menuObject2Objects;
        _menuObject2Objects = new List<GameObject>();
        for (int i = 0; i < _buttons.Count; i++)
        {
            Destroy(_buttons[i]);
        }
        // -- Instantiate new Buttons --
        if (a == 0) // Sectors
        {
            // Offset stuff
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, MapManager.Instance._map._sectors.Count);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectL2Button, _menuObjectsL2.transform);
                if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }
                    

                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._sectors[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));

                _menuObject2Objects.Add(_buttonClone);
            }
        }
        else if (a == 1) // Jumpgate Connections
        {
            // Offset stuff
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, MapManager.Instance._map._jumpGates.Count);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < MapManager.Instance._map._jumpGates.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectL2Button, _menuObjectsL2.transform);
                if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._jumpGates[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));

                _menuObject2Objects.Add(_buttonClone);
            }
        }
        else if (a == 2) // Factions
        {
            // Offset stuff
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, MapManager.Instance._map._factions.Count);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < MapManager.Instance._map._factions.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectL2Button, _menuObjectsL2.transform);
                if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._factions[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));

                _menuObject2Objects.Add(_buttonClone);
            }
        }
        else if (a == 3) // alliances
        {
            // Offset stuff
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, MapManager.Instance._map._factions.Count);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < MapManager.Instance._map._alliances.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectL2Button, _menuObjectsL2.transform);
                if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._alliances[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));

                _menuObject2Objects.Add(_buttonClone);
            }
        }
        else if (a == 4) // Player Factions
        {
            // Offset stuff
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, MapManager.Instance._map._playerFactions.Count);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < MapManager.Instance._map._playerFactions.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectL2Button, _menuObjectsL2.transform);
                if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }

                if (MapManager.Instance._map._playerFactions[i]._regFactionID != -1)
                {
                    _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[i]._regFactionID]._name;
                }
                else
                {
                    _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Unassigned";
                }
                    
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));

                _menuObject2Objects.Add(_buttonClone);
            }
        }
        else if (a == 5) // Regions
        {
            // Offset stuff
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, MapManager.Instance._map._regCats.Count);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < MapManager.Instance._map._regCats.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectL2Button, _menuObjectsL2.transform);
                if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._regCats[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));

                _menuObject2Objects.Add(_buttonClone);
            }
        }
        else if (a == 6) // Players
        {
            // Offset stuff
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, MapManager.Instance._map._players.Count);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < MapManager.Instance._map._players.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectL2Button, _menuObjectsL2.transform);
                if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._players[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));

                _menuObject2Objects.Add(_buttonClone);
            }
        }

        // Resize menu
        if (_menuObject2Objects.Count <= 13)
        {
            _menuObjectsL2.GetComponent<RectTransform>().sizeDelta = new Vector2(90f, 15 + 20 * _menuObject2Objects.Count);
            _menuObjectsL2.GetComponent<BoxCollider>().size = new Vector3(90, 15 + 20 * _menuObject2Objects.Count, 1);
            _menuObjectsL2.GetComponent<BoxCollider>().center = new Vector3(0, (15 + 20 * _menuObject2Objects.Count) / 2 * -1, 0);
        }
        else
        {
            _menuObjectsL2.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 275);
            _menuObjectsL2.GetComponent<BoxCollider>().size = new Vector3(90, 275, 1);
            _menuObjectsL2.GetComponent<BoxCollider>().center = new Vector3(0, -137.5f, 0);
        }
           
    }

    void RepositionObjectsL2()
    {
        _mObjectsm2Min = _mObjectsm2Offset;
        _mObjectsm2Max = _mObjectsm2Offset + 13;
        _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, _menuObject2Objects.Count);
        for (int i = 0; i < _menuObject2Objects.Count; i++)
        {
            if (i >= _mObjectsm2Offset && i < _mObjectsm2Offset + 13)
            {
                _menuObject2Objects[i].SetActive(true);
            }
            else
            {
                _menuObject2Objects[i].SetActive(false);
            }

            _menuObject2Objects[i].GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -20 * (i - _mObjectsm2Offset));
            
        }
            
    }

    void RepositionObjectsContext()
    {
       
        for (int i = 0; i < _contextMenuObjects.Count; i++)
        {
            if (i >= _mObjectsContextOffset && i < _mObjectsContextOffset + 13)
            {
                _contextMenuObjects[i].SetActive(true);
            }
            else
            {
                _contextMenuObjects[i].SetActive(false);
            }

            _contextMenuObjects[i].GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (i - _mObjectsContextOffset));

        }

    }

    void EvaluateContextMenu(int a, bool b)
    {
        if (!b)
        {
            if (_menuObjectsL3Sector.activeSelf)
            {
                if (_currentActiveContext == 0) // Sector Faction Assignment
                {
                    MapManager.Instance._map._sectors[_currentL3Int]._controlFaction = a;
                }
            }
            else if (_menuObjectsL3Faction.activeSelf)
            {
                if (_currentActiveContext == 0) // Alliance Assignment
                {
                    MapManager.Instance._map._factions[_currentL3Int]._allianceId = a;
                }
                else if (_currentActiveContext == 1) // Assign Special Condition
                {
                    _factionInt = Mathf.Clamp(_factionInt, 0, MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1);
                    int _facInt = MapManager.Instance._map._factions[_currentL3Int]._knownFactions[_factionInt];
                    int _repInt = -1;
                    for (int i = 0; i < MapManager.Instance._map._reps.Count; i++)
                    {
                        if ((MapManager.Instance._map._reps[i]._faction1 == _currentL3Int || MapManager.Instance._map._reps[i]._faction1 == _facInt) && (MapManager.Instance._map._reps[i]._faction2 == _currentL3Int || MapManager.Instance._map._reps[i]._faction2 == _facInt))
                        {
                            _repInt = i;
                        }
                    }
                    if (_repInt != -1)
                    {
                        if (a == -1)
                        {
                            MapManager.Instance._map._reps[_repInt]._specialVal = "";
                        }
                        else
                        {
                            MapManager.Instance._map._reps[_repInt]._specialVal = MapManager.Instance._specialRelationConditions[a];
                        }
                    }
                }
            }
            else if (_menuObjectsL3Jumpgate.activeSelf)
            {
                if (_currentActiveContext == 0) // Sector Assignment 1
                {
                    MapManager.Instance._map._jumpGates[_currentL3Int]._sector1Id = a;
                    GalaxyMap.Instance._regen = true;
                }
                else if (_currentActiveContext == 1) // Sector Assignment 2
                {
                    MapManager.Instance._map._jumpGates[_currentL3Int]._sector2Id = a;
                    GalaxyMap.Instance._regen = true;
                }
            }
            else if (_menuObjectsL3PlayerFaction.activeSelf)
            {
                if (_currentActiveContext == 0) // Faction Assignment
                {
                    MapManager.Instance._map._playerFactions[_currentL3Int]._regFactionID = a;
                }
            }
        }
        else
        {
            if (_menuObjectsL3Faction.activeSelf)
            {
                if (_currentActiveContext == 0) // Discovered Sectors Assignment
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[i] == a)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[i] == a)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Remove(MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[i]);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Add(a);
                    }
                }
                else if (_currentActiveContext == 1) // Explored Sectors Assignment
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[i] == a)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[i] == a)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Remove(MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[i]);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Add(a);
                    }
                }
                else if (_currentActiveContext == 2) // Known Owner of Sectors Assignment
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[i] == a)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[i] == a)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Remove(MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[i]);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Add(a);
                    }
                }
                else if (_currentActiveContext == 3) // Known Factions Assignment
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_currentL3Int]._knownFactions[i] == a)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._knownFactions[i] == a)
                            {
                                MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Remove(MapManager.Instance._map._factions[_currentL3Int]._knownFactions[i]);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Add(a);
                    }
                }
            }
            else if (_menuObjectsL3Sector.activeSelf)
            {
                if (_currentActiveContext == 0) // Region Category Assignment
                {
                    bool c = false;

                    for (int i = 0; i < MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count; i++)
                    {
                        if (MapManager.Instance._map._sectors[_currentL3Int]._regionCats[i] == a)
                        {
                            c = true; // Already assigned
                        }
                    }

                    if (c) // Remove Assignment
                    {
                        for (int i = 0; i < MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count; i++)
                        {
                            if (MapManager.Instance._map._sectors[_currentL3Int]._regionCats[i] == a)
                            {
                                MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Remove(MapManager.Instance._map._sectors[_currentL3Int]._regionCats[i]);
                                MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds.Remove(MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[i]);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Add(a);
                        MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds.Add(-1);
                    }
                }
            }
        } 
    }

    void RebuildContextMenu(int a, bool _nOption)
    {
        _contextMenu.SetActive(true);
        _currentContextInt = a;

        // Destroy old buttons
        List<GameObject> _buttons = _contextMenuObjects;
        _contextMenuObjects = new List<GameObject>();
        for (int i = 0; i < _buttons.Count; i++)
        {
            Destroy(_buttons[i]);
        }
        // -- Instantiate new Buttons --
        if (a == 0) // Sectors
        {
            // Offset stuff
            _mObjectsContextOffset = 0;
            _mObjectsContextOffsetVal = 0;
            _mObjectsContextPrevOffset = 0;

            float _offset = 0;
            if (_nOption)
            {
                GameObject _buttonCloneB = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (0 >= _mObjectsContextOffset && 0 < _mObjectsContextOffset + 13)
                {
                    _buttonCloneB.SetActive(true);
                }
                else
                {
                    _buttonCloneB.SetActive(false);
                }

                _offset = 1;

                _buttonCloneB.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "None";
                _buttonCloneB.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (0 - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonCloneB);
            }

            for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (i+_offset >= _mObjectsContextOffset && i+_offset < _mObjectsContextOffset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._sectors[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (i+_offset - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonClone);
            }
        }
        else if (a == 1) // Jumpgate Connections
        {
            
        }
        else if (a == 2) // Factions
        {

            // Offset stuff
            _mObjectsContextOffset = 0;
            _mObjectsContextOffsetVal = 0;
            _mObjectsContextPrevOffset = 0;

            float _offset = 0;
            if (_nOption)
            {
                GameObject _buttonCloneB = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (0 >= _mObjectsContextOffset && 0 < _mObjectsContextOffset + 13)
                {
                    _buttonCloneB.SetActive(true);
                }
                else
                {
                    _buttonCloneB.SetActive(false);
                }
                _offset = 1;

                _buttonCloneB.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Neutral";
                _buttonCloneB.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (0 - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonCloneB);
            }
                
            for (int i = 0; i < MapManager.Instance._map._factions.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (i+_offset >= _mObjectsContextOffset && i+_offset < _mObjectsContextOffset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._factions[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (i+_offset - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonClone);
            }
        }
        else if (a == 3) // alliances
        {
            // Offset stuff
            _mObjectsContextOffset = 0;
            _mObjectsContextOffsetVal = 0;
            _mObjectsContextPrevOffset = 0;

            float _offset = 0;
            if (_nOption)
            {
                GameObject _buttonCloneB = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (0 >= _mObjectsContextOffset && 0 < _mObjectsContextOffset + 13)
                {
                    _buttonCloneB.SetActive(true);
                }
                else
                {
                    _buttonCloneB.SetActive(false);
                }

                _offset = 1;
                _buttonCloneB.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Unaligned";
                _buttonCloneB.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (0 - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonCloneB);
            }
            
            for (int i = 0; i < MapManager.Instance._map._alliances.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (i+_offset >= _mObjectsContextOffset && i + _offset < _mObjectsContextOffset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._alliances[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (i + _offset - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonClone);
            }
        }
        else if (a == 4) // Player Factions
        {
            
        }
        else if (a == 5) // Regions
        {
            // Offset stuff
            _mObjectsContextOffset = 0;
            _mObjectsContextOffsetVal = 0;
            _mObjectsContextPrevOffset = 0;

            float _offset = 0;
            if (_nOption)
            {
                GameObject _buttonCloneB = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (0 >= _mObjectsContextOffset && 0 < _mObjectsContextOffset + 13)
                {
                    _buttonCloneB.SetActive(true);
                }
                else
                {
                    _buttonCloneB.SetActive(false);
                }
                _offset = 1;

                _buttonCloneB.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Neutral";
                _buttonCloneB.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (0 - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonCloneB);
            }

            for (int i = 0; i < MapManager.Instance._map._regCats.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (i + _offset >= _mObjectsContextOffset && i + _offset < _mObjectsContextOffset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._regCats[i]._name;
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (i + _offset - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonClone);
            }
        }
        else if (a == 100) // Special Diplomatic Conditions
        {
            // Offset stuff
            _mObjectsContextOffset = 0;
            _mObjectsContextOffsetVal = 0;
            _mObjectsContextPrevOffset = 0;

            float _offset = 0;
            if (_nOption)
            {
                GameObject _buttonCloneB = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (0 >= _mObjectsContextOffset && 0 < _mObjectsContextOffset + 13)
                {
                    _buttonCloneB.SetActive(true);
                }
                else
                {
                    _buttonCloneB.SetActive(false);
                }

                _offset = 1;
                _buttonCloneB.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "None";
                _buttonCloneB.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (0 - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonCloneB);
            }

            for (int i = 0; i < MapManager.Instance._specialRelationConditions.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_menuObjectContextButton, _contextMenu.transform);
                if (i + _offset >= _mObjectsContextOffset && i + _offset < _mObjectsContextOffset + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._specialRelationConditions[i];
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(30f, -14f + -20 * (i + _offset - _mObjectsContextOffset));

                _contextMenuObjects.Add(_buttonClone);
            }
        }

        // Resize menu
        if (_contextMenuObjects.Count <= 13)
        {
            _contextMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(70f, 15 + 20 * _contextMenuObjects.Count);
            _contextMenu.GetComponent<BoxCollider>().size = new Vector3(70, 15 + 20 * _contextMenuObjects.Count, 1);
            _contextMenu.GetComponent<BoxCollider>().center = new Vector3(0, (15 + 20 * _contextMenuObjects.Count) / 2 * -1, 0);
        }
        else
        {
            _contextMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 275);
            _contextMenu.GetComponent<BoxCollider>().size = new Vector3(70, 275, 1);
            _contextMenu.GetComponent<BoxCollider>().center = new Vector3(0, -137.5f, 0);
        }

    }

    void GetRepName(float _repVal, out string _name, string _specialCond)
    {
        _name = "";

        if (_repVal >= 3.875f)
        {
            
            _name = "Excellent";
        }
        else if (_repVal >= 2.75f)
        {
            
            _name = "Very Good";
        }
        else if (_repVal >= 1.625f)
        {
            
            _name = "Good";
        }
        else if (_repVal >= 0.5f)
        {
        
            _name = "Mediocre";
        }
        else if (_repVal >= -0.5f)
        {
            _name = "Neutral / Unknown";
        }
        else if (_repVal >= -1.625f)
        {
            _name = "Poor";
        }
        else if (_repVal >= -2.75f)
        {
            _name = "Bad";
        }
        else if (_repVal >= -3.875f)
        {
            _name = "Very Bad";
        }
        else
        {
            _name = "Horrible";
        }

        if (_specialCond == "War")
        {
            _name += " / War";
        }
        else if (_specialCond == "Allied")
        {
            _name += " / Allied";
        }
    }

    void EnterText(InputField _inputField, bool _lettersAllowed, bool _numbersAllowed, bool _spaceAllowed, bool _interpunctuationAllowed, bool _specialCharAllowed, int _maxLength, out bool _active)
    {
        _active = true;
        if (_inputField.text.Length < _maxLength)
        {
            if (_lettersAllowed) {
                if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "A";
                }
                else if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "B";
                }
                else if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "C";
                }
                else if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "D";
                }
                else if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "E";
                }
                else if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "F";
                }
                else if (Input.GetKeyDown(KeyCode.G) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "G";
                }
                else if (Input.GetKeyDown(KeyCode.H) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "H";
                }
                else if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "I";
                }
                else if (Input.GetKeyDown(KeyCode.J) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "J";
                }
                else if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "K";
                }
                else if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "L";
                }
                else if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "M";
                }
                else if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "N";
                }
                else if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "O";
                }
                else if (Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "P";
                }
                else if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "Q";
                }
                else if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "R";
                }
                else if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "S";
                }
                else if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "T";
                }
                else if (Input.GetKeyDown(KeyCode.U) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "U";
                }
                else if (Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "V";
                }
                else if (Input.GetKeyDown(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "W";
                }
                else if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "X";
                }
                else if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "Y";
                }
                else if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "Z";
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    _inputField.text = _inputField.text + "a";
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    _inputField.text = _inputField.text + "b";
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    _inputField.text = _inputField.text + "c";
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _inputField.text = _inputField.text + "d";
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    _inputField.text = _inputField.text + "e";
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    _inputField.text = _inputField.text + "f";
                }
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    _inputField.text = _inputField.text + "g";
                }
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    _inputField.text = _inputField.text + "h";
                }
                else if (Input.GetKeyDown(KeyCode.I))
                {
                    _inputField.text = _inputField.text + "i";
                }
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    _inputField.text = _inputField.text + "j";
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    _inputField.text = _inputField.text + "k";
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    _inputField.text = _inputField.text + "l";
                }
                else if (Input.GetKeyDown(KeyCode.M))
                {
                    _inputField.text = _inputField.text + "m";
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    _inputField.text = _inputField.text + "n";
                }
                else if (Input.GetKeyDown(KeyCode.O))
                {
                    _inputField.text = _inputField.text + "o";
                }
                else if (Input.GetKeyDown(KeyCode.P))
                {
                    _inputField.text = _inputField.text + "p";
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    _inputField.text = _inputField.text + "q";
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    _inputField.text = _inputField.text + "r";
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    _inputField.text = _inputField.text + "s";
                }
                else if (Input.GetKeyDown(KeyCode.T))
                {
                    _inputField.text = _inputField.text + "t";
                }
                else if (Input.GetKeyDown(KeyCode.U))
                {
                    _inputField.text = _inputField.text + "u";
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    _inputField.text = _inputField.text + "v";
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    _inputField.text = _inputField.text + "w";
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    _inputField.text = _inputField.text + "x";
                }
                else if (Input.GetKeyDown(KeyCode.Y))
                {
                    _inputField.text = _inputField.text + "y";
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    _inputField.text = _inputField.text + "z";
                }
            }
            if (_numbersAllowed) {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _inputField.text = _inputField.text + "1";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _inputField.text = _inputField.text + "2";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt)))
                {
                    _inputField.text = _inputField.text + "3";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    _inputField.text = _inputField.text + "4";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    _inputField.text = _inputField.text + "5";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "6";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "7";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "8";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "9";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    _inputField.text = _inputField.text + "0";
                }
                else if (Input.GetKeyDown(KeyCode.Minus) && Input.GetKey(KeyCode.Space))
                {
                    _inputField.text = _inputField.text + "_";
                }
            }
            if (_spaceAllowed && Input.GetKeyDown(KeyCode.Space))
            {
                _inputField.text += " ";
            }
            if (_interpunctuationAllowed)
            {
                if (Input.GetKeyDown(KeyCode.Period) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += ".";
                }
                else if (Input.GetKeyDown(KeyCode.Comma) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += ",";
                }

                if (Input.GetKeyDown(KeyCode.Colon) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += ";";
                }
                else if (Input.GetKeyDown(KeyCode.Period) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += ":";
                }
                else if (Input.GetKeyDown(KeyCode.Quote) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "?";
                }
                else if (Input.GetKeyDown((KeyCode)33) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "!";
                }
            }
            if (_specialCharAllowed)
            {
                if (Input.GetKeyDown(KeyCode.Quote) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "'";
                }
                else if (Input.GetKeyDown(KeyCode.Minus) && !Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "-";
                }
                if (Input.GetKeyDown(KeyCode.Minus) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "_";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
                {
                    _inputField.text += "#";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "(";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += ")";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "/";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text += "&";
                }
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (_inputField.text.Length > 0)
                {
                    _inputField.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
                }

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                _active = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                
                if (_inputField.text.Length > 0)
                {
                    _inputField.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
                }

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                _active = false;
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (MapManager.Instance._map._lockSelection)
        {
            _menuMain.SetActive(false);
        }
        else
        {
            _menuMain.SetActive(true);
        }
        
        Ray ray;
        RaycastHit hit;

        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.transform.gameObject == _buttonDebug && Input.GetMouseButtonDown(0))
        {
            DebugOnOff();
        }
        else if (hit.transform.gameObject == _buttonExport && Input.GetMouseButtonDown(0))
        {
            ExportMenuOnOff();
        }
        else if (hit.transform.gameObject == _buttonMenu && Input.GetMouseButtonDown(0))
        {
            ObjectMenuOnOff();
        }
        else if (hit.transform.gameObject == _buttonRender && Input.GetMouseButtonDown(0))
        {
            Camera.main.GetComponent<CameraRender>()._render = true;
        }
        else if (hit.transform.gameObject == _buttonSettings && Input.GetMouseButtonDown(0))
        {
            if (_settingsMenu.activeSelf)
            {
                _settingsMenu.SetActive(false);
            }
            else
            {
                _settingsMenu.SetActive(true);
            }
        }


        if (_contextMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            _contextMenu.SetActive(false);
        }
        else if (_menuObjectsL2.activeSelf && _currentL3Int >= 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            _currentL3Int = -1;
            _menuObjectsL3Sector.SetActive(false);
            _menuObjectsL3Faction.SetActive(false);
            _menuObjectsL3Alliance.SetActive(false);
            _menuObjectsL3PlayerFaction.SetActive(false);
            _menuObjectsL3Jumpgate.SetActive(false);
            _menuObjectsL3Region.SetActive(false);
            _contextMenu.SetActive(false);
        }
        else if (_menuObjectsL2.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            _currentL2Int = -1;
            _menuObjectsL2.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menuObjects.SetActive(false);
        }

        for (int i = 0; i < _menuObject1Objects.Count; i++)
        {
            if (hit.transform.gameObject == _menuObject1Objects[i] && Input.GetMouseButtonDown(0))
            {
                RebuildObjectMenuL2(i);
                _menuObjectsL3Sector.SetActive(false);
                _menuObjectsL3Faction.SetActive(false);
                _menuObjectsL3Alliance.SetActive(false);
                _menuObjectsL3Jumpgate.SetActive(false);
                _menuObjectsL3Region.SetActive(false);
                _menuObjectsL3PlayerFaction.SetActive(false);
                _contextMenu.SetActive(false);
            }
        }
        
        for (int i = 0; i < _menuObject2Objects.Count; i++)
        {
            if (hit.transform.gameObject == _menuObject2Objects[i] && Input.GetMouseButtonDown(0))
            {
                _contextMenu.SetActive(false);
                if (_currentL2Int == 0) // Sector
                {
                    _currentL3Int = i;
                    _menuObjectsL3Sector.SetActive(true);
                    _menuObjectsL3Faction.SetActive(false);
                    _menuObjectsL3Alliance.SetActive(false);
                    _menuObjectsL3Jumpgate.SetActive(false);
                    _menuObjectsL3Region.SetActive(false);
                    _menuObjectsL3PlayerFaction.SetActive(false);

                    _sectorExtraId = 0;
                }
                else if (_currentL2Int == 1) // Jumpgates
                {
                    _currentL3Int = i;
                    _menuObjectsL3Sector.SetActive(false);
                    _menuObjectsL3Faction.SetActive(false);
                    _menuObjectsL3Alliance.SetActive(false);
                    _menuObjectsL3Jumpgate.SetActive(true);
                    _menuObjectsL3Region.SetActive(false);
                    _menuObjectsL3PlayerFaction.SetActive(false);
                }
                else if (_currentL2Int == 2) // Factions
                {
                    _factionInt = 0;
                    _currentL3Int = i;
                    _menuObjectsL3Faction.SetActive(true);
                    _menuObjectsL3Sector.SetActive(false);
                    _menuObjectsL3Alliance.SetActive(false);
                    _menuObjectsL3Jumpgate.SetActive(false);
                    _menuObjectsL3Region.SetActive(false);
                    _menuObjectsL3PlayerFaction.SetActive(false);
                }
                else if (_currentL2Int == 3) // Alliances
                {
                    _currentL3Int = i;
                    _menuObjectsL3Faction.SetActive(false);
                    _menuObjectsL3Sector.SetActive(false);
                    _menuObjectsL3Alliance.SetActive(true);
                    _menuObjectsL3Jumpgate.SetActive(false);
                    _menuObjectsL3Region.SetActive(false);
                    _menuObjectsL3PlayerFaction.SetActive(false);
                }
                else if (_currentL2Int == 4) // Player Faction
                {
                    _currentL3Int = i;
                    _menuObjectsL3Faction.SetActive(false);
                    _menuObjectsL3Sector.SetActive(false);
                    _menuObjectsL3Alliance.SetActive(false);
                    _menuObjectsL3Jumpgate.SetActive(false);
                    _menuObjectsL3Region.SetActive(false);
                    _menuObjectsL3PlayerFaction.SetActive(true);
                }
                else if (_currentL2Int == 5) // Region
                {
                    _subRegionInt = 0;
                    _currentL3Int = i;
                    _menuObjectsL3Faction.SetActive(false);
                    _menuObjectsL3Sector.SetActive(false);
                    _menuObjectsL3Alliance.SetActive(false);
                    _menuObjectsL3Jumpgate.SetActive(false);
                    _menuObjectsL3Region.SetActive(true);
                    _menuObjectsL3PlayerFaction.SetActive(false);
                }
            }

            if (hit.transform.gameObject == _menuObject2Objects[i] && Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                _mObjectsm2OffsetVal += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -1 * _mObjectsm2ScrollSpeed;
                _mObjectsm2OffsetVal = Mathf.Clamp(_mObjectsm2OffsetVal, 0, 1);

                _mObjectsm2Offset = (int)Mathf.Round(_mObjectsm2OffsetVal * (_menuObject2Objects.Count - 13));
                _mObjectsm2Offset = Mathf.Clamp(_mObjectsm2Offset, 0, 10000);
            }
        }

        for (int i = 0; i < _contextMenuObjects.Count; i++)
        {
            if (hit.transform.gameObject == _contextMenuObjects[i] && Input.GetMouseButtonDown(0))
            {
                
                if (_currentContextInt == 0) // Sectors
                {
                    int _val = 0;
                    if (_currentL2Int == 1)
                    {
                        _val = 1;
                    }
                    EvaluateContextMenu(i - _val, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }
                else if (_currentContextInt == 1) // Jumpgate Connections
                {
                    EvaluateContextMenu(i, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }
                else if (_currentContextInt == 2) // Factions
                {
                    int _val = 0;
                    if (_currentL2Int == 4 || _currentL2Int == 0)
                    {
                        _val = 1;
                    }
                    EvaluateContextMenu(i-_val, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }
                else if (_currentContextInt == 3) // Alliances
                {
                    int _val = 0;
                    if (_currentL2Int == 2 && _contextSelection == false)
                    {
                        _val = 1;
                    }
                    Debug.Log(_val);
                    EvaluateContextMenu(i-_val, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }
                else if (_currentContextInt == 4) // Player Factions
                {
                    EvaluateContextMenu(i, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }
                else if (_currentContextInt == 5) // Regions
                {
                    EvaluateContextMenu(i, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }
                else if (_currentContextInt == 100) // Special Relation Conditions
                {
                    EvaluateContextMenu(i-1, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }



            }
            if (hit.transform.gameObject == _contextMenuObjects[i] && Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                _mObjectsContextOffsetVal += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -1 * _mObjectsContextScrollSpeed;
                _mObjectsContextOffsetVal = Mathf.Clamp(_mObjectsContextOffsetVal, 0, 1);

                _mObjectsContextOffset = (int)Mathf.Round(_mObjectsContextOffsetVal * (_contextMenuObjects.Count - 13));
                _mObjectsContextOffset = Mathf.Clamp(_mObjectsContextOffset, 0, 10000);
            }
        }

        

        if (MapManager.Instance._map._debug)
        {
            _buttonDebug.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Disable Debug";
        }
        else
        {
            _buttonDebug.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Enable Debug";
        }
        
        if (_menuExport.activeSelf)
        {

            if (_lockSel)
            {
                _menuExport.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Locked: Yes";
            }
            else
            {
                _menuExport.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Locked: No";
            }

            if (_menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text.Length > 0)
            {
                _menuExport.GetComponent<IndexScript>()._obj5.SetActive(true);
            }
            else
            {
                _menuExport.GetComponent<IndexScript>()._obj5.SetActive(false);
            }

            Ray rayB;
            RaycastHit hitB;

            rayB = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayB, out hitB) && hitB.transform.gameObject == _menuExport.GetComponent<IndexScript>()._obj5 && Input.GetMouseButtonDown(0))
            {
                XMLWriter.Instance._export = true;
                XMLWriter.Instance._exportFileName = _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text;
                XMLWriter.Instance._exportLock = _lockSel;
                XMLWriter.Instance._playerFaction = _playerFaction;

            }
            else if (Physics.Raycast(rayB, out hitB) && hitB.transform.gameObject == _menuExport.GetComponent<IndexScript>()._obj6 && Input.GetMouseButtonDown(0))
            {
                _activeInputExportM = true;
            }
            else if (Physics.Raycast(rayB, out hitB) && hitB.transform.gameObject == _menuExport.GetComponent<IndexScript>()._obj4 && Input.GetMouseButtonDown(0))
            {
                if (_lockSel)
                {
                    _lockSel = false;
                    _menuExport.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Locked: No";
                }
                else
                {
                    _lockSel = true;
                    _menuExport.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Locked: Yes";
                }
            }
            else if (Physics.Raycast(rayB, out hitB) && hitB.transform.gameObject == _menuExport.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
            {
                if (_playerFaction >= MapManager.Instance._map._playerFactions.Count - 1)
                {
                    _playerFaction = -1;
                    
                }
                else
                {
                    _playerFaction++;
                    
                    
                }
                while (_playerFaction > -1 && MapManager.Instance._map._playerFactions[_playerFaction]._regFactionID == -1)
                {
                    _playerFaction++;
                    if (_playerFaction >= MapManager.Instance._map._playerFactions.Count - 1)
                    {
                        _playerFaction = -1;
                    }
                }
                if (_playerFaction == -1)
                {
                    _menuExport.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Perspective: GM";
                }
                else
                {
                    _menuExport.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Perspective: " + MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[_playerFaction]._regFactionID]._shorthand;
                }
            }

            if (_activeInputExportM)
            {
                _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(220, 220, 220, 255);
                

                EnterText(_menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>(), true, true, false, false, false, 20, out _activeInputExportM);


                if (Physics.Raycast(rayB, out hitB) && hitB.transform.gameObject != _menuExport.GetComponent<IndexScript>()._obj6 && Input.GetMouseButtonDown(0))
                {

                    _activeInputExportM = false;
                    _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
                }
            }
            else
            {
                _activeInputExportM = false;
                _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
            }
        }
        else
        {
            
        }
        
        if (_menuObjects.activeSelf)
        {
            
            if (!_mObjectsm1Built)
            {
                RebuildObjectMenuL1();
                _mObjectsm1Built = true;
            }
                

            _menuObjects.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 15 + 20 * _menuObject1Themes.Count);
        }
        
        if (hit.transform.gameObject == _menuObjectsL2 && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            _mObjectsm2OffsetVal += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -1 * _mObjectsm2ScrollSpeed;
            _mObjectsm2OffsetVal = Mathf.Clamp(_mObjectsm2OffsetVal, 0, 1);

            _mObjectsm2Offset = (int)Mathf.Round(_mObjectsm2OffsetVal * (_menuObject2Objects.Count - 13));
            _mObjectsm2Offset = Mathf.Clamp(_mObjectsm2Offset, 0, 10000);
        }
        
        if (hit.transform.gameObject == _contextMenu && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            _mObjectsContextOffsetVal += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * -1 * _mObjectsContextScrollSpeed;
            _mObjectsContextOffsetVal = Mathf.Clamp(_mObjectsContextOffsetVal, 0, 1);

            _mObjectsContextOffset = (int)Mathf.Round(_mObjectsContextOffsetVal * (_contextMenuObjects.Count - 13));
            _mObjectsContextOffset = Mathf.Clamp(_mObjectsContextOffset, 0, 10000);
        }
        
        if (_menuObjectsL2.activeSelf)
        {
            Ray rayB;
            RaycastHit hitB;

            rayB = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayB, out hitB) && hitB.transform.gameObject == _menuObjectsL2.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance.AddObject(_currentL2Int);
            }

            if (_currentL2Int == 0)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._sectors.Count)
                {
                    RebuildObjectMenuL2(0);
                }
            }
            else if (_currentL2Int == 1)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._jumpGates.Count)
                {
                    RebuildObjectMenuL2(1);
                }
            }
            else if (_currentL2Int == 2)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._factions.Count)
                {
                    RebuildObjectMenuL2(2);
                }
            }
            else if (_currentL2Int == 3)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._alliances.Count)
                {
                    RebuildObjectMenuL2(3);
                }
            }
            else if (_currentL2Int == 4)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._playerFactions.Count)
                {
                    RebuildObjectMenuL2(4);
                }
            }
            else if (_currentL2Int == 5)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._regCats.Count)
                {
                    RebuildObjectMenuL2(5);
                }
            }
            for (int i = 0; i < _menuObject2Objects.Count; i++)
            {
                if (_currentL2Int == 0) // Sectors
                {
                    // Set button text to name
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._sectors[i]._name;

                    if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                    {
                        if (MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId == -1)
                        {
                            _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._shorthand + " / Unaligned" ;
                        }
                        else
                        {
                            _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._shorthand + " / " + MapManager.Instance._map._alliances[MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId]._shorthand;
                        }
                    }
                    else
                    {
                        _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = "Neutral";
                    }
                }
                else if (_currentL2Int == 1) // Jumpgates
                {
                    // Set button text to name
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._jumpGates[i]._name;

                    string _comp1 = "Neutral";
                    string _comp2 = "Neutral";

                    if (MapManager.Instance._map._jumpGates[i]._sector1Id != -1 && MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector1Id]._controlFaction != -1)
                    {
                        _comp1 = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector1Id]._controlFaction]._shorthand;
                    }

                    if (MapManager.Instance._map._jumpGates[i]._sector2Id != -1 && MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector2Id]._controlFaction != -1)
                    {
                        _comp2 = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector2Id]._controlFaction]._shorthand;
                    }

                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = _comp1 + " / " + _comp2;
                    
                }
                else if (_currentL2Int == 2) // Factions
                {
                    // Set button text to name
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._factions[i]._name;
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = MapManager.Instance._map._factions[i]._shorthand;

                }
                else if (_currentL2Int == 3) // Alliances
                {
                    // Set button text to name
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._alliances[i]._name;
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = MapManager.Instance._map._alliances[i]._shorthand;

                }
                else if (_currentL2Int == 4) // Player Factions
                {
                    // Set button text to name
                    if (MapManager.Instance._map._playerFactions[i]._regFactionID != -1)
                    {
                        _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[i]._regFactionID]._name;
                        _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[i]._regFactionID]._shorthand;

                    }
                    else
                    {
                        _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Unassigned";
                        _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = "";

                    }
                        
                }
                else if (_currentL2Int == 5) // Regions
                {
                    // Set button text to name
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._regCats[i]._name;
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>().text = "";

                }
            }
        }
        
        if (_menuObjectsL3Sector.activeSelf)
        {
            if (_currentL3Int != -1)
            {
                if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
                {
                    RebuildContextMenu(2, true);
                    _contextSelection = false;
                    _currentActiveContext = 0;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj4 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance._map._sectors[_currentL3Int]._posXInt++;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MapManager.Instance._map._sectors[_currentL3Int]._posXInt += 4;
                    }
                    MapManager.Instance._map._sectors[_currentL3Int]._posXInt = Mathf.Clamp(MapManager.Instance._map._sectors[_currentL3Int]._posXInt, -100, 100);
                    GalaxyMap.Instance._regen = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj5 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance._map._sectors[_currentL3Int]._posXInt--;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MapManager.Instance._map._sectors[_currentL3Int]._posXInt -= 4;
                    }
                    MapManager.Instance._map._sectors[_currentL3Int]._posXInt = Mathf.Clamp(MapManager.Instance._map._sectors[_currentL3Int]._posXInt, -100, 100);
                    GalaxyMap.Instance._regen = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj7 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance._map._sectors[_currentL3Int]._posYInt++;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MapManager.Instance._map._sectors[_currentL3Int]._posYInt += 4;
                    }
                    MapManager.Instance._map._sectors[_currentL3Int]._posYInt = Mathf.Clamp(MapManager.Instance._map._sectors[_currentL3Int]._posYInt, -100, 100);
                    GalaxyMap.Instance._regen = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj8 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance._map._sectors[_currentL3Int]._posYInt--;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MapManager.Instance._map._sectors[_currentL3Int]._posYInt -= 4;
                    }
                    MapManager.Instance._map._sectors[_currentL3Int]._posYInt = Mathf.Clamp(MapManager.Instance._map._sectors[_currentL3Int]._posYInt, -100, 100);
                    GalaxyMap.Instance._regen = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj10 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3Sector.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj11 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance.RemoveObject(_currentL2Int, _currentL3Int);
                    RebuildObjectMenuL2(_currentL2Int);
                    _menuObjectsL3Sector.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                
                if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SectorNameActive = true;
                }
                _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._name; 

                if (_menuObjectsL3SectorNameActive)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(87, 87, 87, 255);
                    


                    EnterText(_menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<InputField>(), true, true, true, false, true, 31, out _menuObjectsL3SectorNameActive);

                    MapManager.Instance._map._sectors[_currentL3Int]._name = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3SectorNameActive = false;
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3SectorNameActive = false;
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

                if (MapManager.Instance._map._sectors[_currentL3Int]._controlFaction != -1)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Controlling Faction: " + MapManager.Instance._map._factions[MapManager.Instance._map._sectors[_currentL3Int]._controlFaction]._shorthand;
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Controlling Faction: Neutral";
                }

                _menuObjectsL3Sector.GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = MapManager.Instance._map._sectors[_currentL3Int]._posXInt.ToString();
                _menuObjectsL3Sector.GetComponent<IndexScript>()._obj9.GetComponent<Text>().text = MapManager.Instance._map._sectors[_currentL3Int]._posYInt.ToString();

                if (_menuObjectsL3SectorPlacementActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape)))
                {
                    _menuObjectsL3SectorPlacementActive = false;
                }
                else if (_menuObjectsL3SectorPlacementActive && Input.GetMouseButtonDown(0))
                {
                    Vector3 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    float xPos = _pos.x / .75f;
                    
                    int xPosI = Mathf.RoundToInt(xPos);
                    
                    float yPos = 0;
                    if (Mathf.Pow(xPosI,2)%2 == 0)
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

                    

                    MapManager.Instance._map._sectors[_currentL3Int]._posXInt = xPosI;
                    MapManager.Instance._map._sectors[_currentL3Int]._posYInt = yPosI;

                    GalaxyMap.Instance._regen = true;
                    _menuObjectsL3SectorPlacementActive = false;
                }

                if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj12 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SectorPlacementActive = true;
                }

                if (_menuObjectsL3SectorPlacementActive)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj12.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj12.GetComponent<Image>().color = new Color32(118, 118, 118, 255);
                }

                List<string> _t1 = new List<string>() { "Description", "Lore", "Regions" };

                if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj16 && Input.GetMouseButtonDown(0))
                {
                    if (_sectorExtraId == _t1.Count - 1)
                    {
                        _sectorExtraId = 0;
                    }
                    else
                    {
                        _sectorExtraId++;
                    }
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj17 && Input.GetMouseButtonDown(0))
                {
                    if (_sectorExtraId == 0)
                    {
                        _sectorExtraId = _t1.Count - 1;
                    }
                    else
                    {
                        _sectorExtraId--;
                    }
                }

                if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SectorDescActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SectorLoreActive = true;
                }

                _menuObjectsL3Sector.GetComponent<IndexScript>()._obj13.GetComponent<Text>().text = _t1[_sectorExtraId];

                if (_sectorExtraId == 0)
                {   
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.SetActive(true);
                    
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.SetActive(false);
                    _menuObjectsL3SectorDescActive = false;
                }

                if (_sectorExtraId == 1)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.SetActive(true);
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.SetActive(false);
                    _menuObjectsL3SectorLoreActive = false;
                }

                if (_sectorExtraId == 2)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj18.SetActive(true);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj19.SetActive(true);
                    if (MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count > 0)
                    {
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj20.SetActive(true);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj21.SetActive(true);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj22.SetActive(true);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj23.SetActive(true);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj24.SetActive(true);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj25.SetActive(true);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj26.SetActive(true);

                        _sectorRegionInt = Mathf.Clamp(_sectorRegionInt, 0, MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count - 1);
                        
                        if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj21 && Input.GetMouseButtonDown(0))
                        {
                            _sectorRegionInt++;
                            if (_sectorRegionInt > MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count - 1)
                            {
                                _sectorRegionInt = 0;
                            }
                        }
                        else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj22 && Input.GetMouseButtonDown(0))
                        {
                            _sectorRegionInt--;
                            if (_sectorRegionInt < 0)
                            {
                                _sectorRegionInt = MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count - 1;
                            }
                        }

                        int a = MapManager.Instance._map._sectors[_currentL3Int]._regionCats[_sectorRegionInt];

                        if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj24 && Input.GetMouseButtonDown(0))
                        {
                            
                            MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt]++;
                            if (MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] > MapManager.Instance._map._regCats[a]._regions.Count - 1)
                            {
                                MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] = -1;
                            }
                        }
                        else if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj25 && Input.GetMouseButtonDown(0))
                        {
                            
                            MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt]--;
                            if (MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] < -1)
                            {
                                MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] = MapManager.Instance._map._regCats[a]._regions.Count - 1;
                            }
                        }

                        int b = MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt];

                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj20.GetComponent<Text>().text = MapManager.Instance._map._regCats[a]._name;
                        if (b != -1)
                        {
                            _menuObjectsL3Sector.GetComponent<IndexScript>()._obj26.GetComponent<Text>().text = MapManager.Instance._map._regCats[a]._regions[b]._name;
                        }
                        else
                        {
                            _menuObjectsL3Sector.GetComponent<IndexScript>()._obj26.GetComponent<Text>().text = "No Data";
                        }
                            
                    }
                    else
                    {
                        _sectorRegionInt = 0;
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj20.SetActive(false);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj21.SetActive(false);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj22.SetActive(false);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj23.SetActive(false);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj24.SetActive(false);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj25.SetActive(false);
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj26.SetActive(false);
                    }
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj18.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj19.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj20.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj21.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj22.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj23.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj24.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj25.SetActive(false);
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj26.SetActive(false);
                }

                _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._description;
                _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._lore;

                if (_menuObjectsL3SectorDescActive)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<InputField>(), true, true, true, true, true, 500, out _menuObjectsL3SectorDescActive);

                    MapManager.Instance._map._sectors[_currentL3Int]._description = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3SectorDescActive = false;
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3SectorDescActive = false;
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

                if (_menuObjectsL3SectorLoreActive)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<InputField>(), true, true, true, true, true, 500, out _menuObjectsL3SectorLoreActive);

                    MapManager.Instance._map._sectors[_currentL3Int]._lore = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3SectorLoreActive = false;
                        _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3SectorLoreActive = false;
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

                if (hit.transform.gameObject == _menuObjectsL3Sector.GetComponent<IndexScript>()._obj18 && Input.GetMouseButtonDown(0))
                {
                    RebuildContextMenu(5,false);
                    _contextSelection = true;
                    _currentActiveContext = 0;
                }
            }

            else
            {
                _menuObjectsL3Sector.SetActive(false);
            }
        }
        
        if (_menuObjectsL3Faction.activeSelf)
        {
            if (_currentL3Int != -1)
            {
                
                if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
                {
                    _contextSelection = false;
                    RebuildContextMenu(3, true);
                    _currentActiveContext = 0;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj10 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3Faction.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj11 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance.RemoveObject(_currentL2Int, _currentL3Int);
                    RebuildObjectMenuL2(_currentL2Int);
                    _menuObjectsL3Faction.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3FactionNameActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3FactionShorthandActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3FactionRedActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3FactionGreenActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3FactionBlueActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj12 && Input.GetMouseButtonDown(0))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._defaultRep -= 0.5f;
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._defaultRep -= 0.1f;
                    }
                    MapManager.Instance._map._factions[_currentL3Int]._defaultRep = Mathf.Round(MapManager.Instance._map._factions[_currentL3Int]._defaultRep * 10) / 10;
                    MapManager.Instance._map._factions[_currentL3Int]._defaultRep = Mathf.Clamp(MapManager.Instance._map._factions[_currentL3Int]._defaultRep, -5, 5);
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj13 && Input.GetMouseButtonDown(0))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._defaultRep += 0.5f;
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_currentL3Int]._defaultRep += 0.1f;
                    }
                        
                    MapManager.Instance._map._factions[_currentL3Int]._defaultRep = Mathf.Round(MapManager.Instance._map._factions[_currentL3Int]._defaultRep * 10) / 10;
                    MapManager.Instance._map._factions[_currentL3Int]._defaultRep = Mathf.Clamp(MapManager.Instance._map._factions[_currentL3Int]._defaultRep, -5, 5);
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj19 && Input.GetMouseButtonDown(0))
                {
                    _contextSelection = true;
                    RebuildContextMenu(0, false);
                    _currentActiveContext = 0;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj20 && Input.GetMouseButtonDown(0))
                {
                    _contextSelection = true;
                    RebuildContextMenu(0, false);
                    _currentActiveContext = 1;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj21 && Input.GetMouseButtonDown(0))
                {
                    _contextSelection = true;
                    RebuildContextMenu(0, false);
                    _currentActiveContext = 2;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj22 && Input.GetMouseButtonDown(0))
                {
                    _contextSelection = true;
                    RebuildContextMenu(2, false);
                    _currentActiveContext = 3;
                }
                
                if (MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count == 0)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj14.SetActive(false);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj15.SetActive(false);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj16.SetActive(false);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj17.SetActive(false);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj18.SetActive(false);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj23.SetActive(false);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj24.SetActive(false);

                    _factionInt = 0;
                }
                else
                {
                    
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj14.SetActive(true);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj15.SetActive(true);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj16.SetActive(true);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj17.SetActive(true);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj18.SetActive(true);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj23.SetActive(true);
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj24.SetActive(true);
                    
                    int _repInt = -1;
                    _factionInt = Mathf.Clamp(_factionInt, 0, MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1);
                    int _facInt = MapManager.Instance._map._factions[_currentL3Int]._knownFactions[_factionInt];
                    while (_facInt == _currentL3Int)
                    {
                        _factionInt++;
                        if (_factionInt > MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count -1)
                        {
                            _factionInt = 0;
                        }
                        _facInt = MapManager.Instance._map._factions[_currentL3Int]._knownFactions[_factionInt];
                    }
                    
                    for (int i = 0; i < MapManager.Instance._map._reps.Count; i++)
                    {
                        if ((MapManager.Instance._map._reps[i]._faction1 == _currentL3Int || MapManager.Instance._map._reps[i]._faction1 == _facInt) && (MapManager.Instance._map._reps[i]._faction2 == _currentL3Int || MapManager.Instance._map._reps[i]._faction2 == _facInt))
                        {
                            _repInt = i;
                        }
                    }

                    
                    if (_repInt != -1 && hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj15 && Input.GetMouseButtonDown(0))
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            MapManager.Instance._map._reps[_repInt]._repVal += 0.5f;
                        }
                        else
                        {
                            MapManager.Instance._map._reps[_repInt]._repVal += 0.1f;
                        }

                        MapManager.Instance._map._reps[_repInt]._repVal = Mathf.Round(MapManager.Instance._map._reps[_repInt]._repVal * 10) / 10;
                        MapManager.Instance._map._reps[_repInt]._repVal = Mathf.Clamp(MapManager.Instance._map._reps[_repInt]._repVal, -5, 5);
                    }
                    else if (_repInt != -1 && hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj14 && Input.GetMouseButtonDown(0))
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            MapManager.Instance._map._reps[_repInt]._repVal -= 0.5f;
                        }
                        else
                        {
                            MapManager.Instance._map._reps[_repInt]._repVal -= 0.1f;
                        }

                        MapManager.Instance._map._reps[_repInt]._repVal = Mathf.Round(MapManager.Instance._map._reps[_repInt]._repVal * 10) / 10;
                        MapManager.Instance._map._reps[_repInt]._repVal = Mathf.Clamp(MapManager.Instance._map._reps[_repInt]._repVal, -5, 5);
                    }
                    
                    if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj16 && Input.GetMouseButtonDown(0))
                    {
                        _factionInt++;
                        _factionInt %= MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count;
                    }
                    
                    if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj17 && Input.GetMouseButtonDown(0))
                    {
                        _factionInt--;
                        if (_factionInt < 0)
                        {
                            _factionInt = MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1;
                        }
                    }
                    
                    if (hit.transform.gameObject == _menuObjectsL3Faction.GetComponent<IndexScript>()._obj23 && Input.GetMouseButtonDown(0))
                    {
                        _contextSelection = false;
                        RebuildContextMenu(100, true);
                        _currentActiveContext = 1;
                    }

                    
                    string _rpN2;
                    if (_repInt != -1)
                    {
                        GetRepName(MapManager.Instance._map._reps[_repInt]._repVal, out _rpN2, MapManager.Instance._map._reps[_repInt]._specialVal);
                        _menuObjectsL3Faction.GetComponent<IndexScript>()._obj18.GetComponent<Text>().text = "Reputation with " + MapManager.Instance._map._factions[_facInt]._shorthand + ": " + MapManager.Instance._map._reps[_repInt]._repVal.ToString() + " (" + _rpN2 + ")";
                    }


                    

                }
                
                _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._name;
                _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._shorthand;
                _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._factionColor.r.ToString();
                _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._factionColor.g.ToString();
                _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._factionColor.b.ToString();
                
                if (_menuObjectsL3FactionNameActive)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<InputField>(), true, false, true, false, true, 40, out _menuObjectsL3FactionNameActive);

                    MapManager.Instance._map._factions[_currentL3Int]._name = _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3FactionNameActive = false;
                        _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3FactionNameActive = false;
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                
                if (_menuObjectsL3FactionShorthandActive)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<InputField>(), true, false, false, false, false, 4, out _menuObjectsL3FactionShorthandActive);

                    MapManager.Instance._map._factions[_currentL3Int]._shorthand = _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3FactionShorthandActive = false;
                        _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3FactionShorthandActive = false;
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                
                if (_menuObjectsL3FactionRedActive)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3FactionRedActive);

                    int _col = 0;
                    if (_menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col,0, 255);
                    MapManager.Instance._map._factions[_currentL3Int]._factionColor = new Color32((byte)_col, MapManager.Instance._map._factions[_currentL3Int]._factionColor.g, MapManager.Instance._map._factions[_currentL3Int]._factionColor.b, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3FactionRedActive = false;
                        _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3FactionRedActive = false;
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                if (_menuObjectsL3FactionGreenActive)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3FactionGreenActive);

                    int _col = 0;
                    if (_menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._factions[_currentL3Int]._factionColor = new Color32(MapManager.Instance._map._factions[_currentL3Int]._factionColor.r, (byte)_col, MapManager.Instance._map._factions[_currentL3Int]._factionColor.b, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3FactionGreenActive = false;
                        _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3FactionGreenActive = false;
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                if (_menuObjectsL3FactionBlueActive)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3FactionBlueActive);

                    int _col = 0;
                    if (_menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._factions[_currentL3Int]._factionColor = new Color32(MapManager.Instance._map._factions[_currentL3Int]._factionColor.r, MapManager.Instance._map._factions[_currentL3Int]._factionColor.g, (byte)_col, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3FactionBlueActive = false;
                        _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3FactionBlueActive = false;
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                
                if (MapManager.Instance._map._factions[_currentL3Int]._allianceId != -1)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Alliance: " + MapManager.Instance._map._alliances[MapManager.Instance._map._factions[_currentL3Int]._allianceId]._shorthand;
                }
                else
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Alliance: Unaligned";
                }

                
                _menuObjectsL3Faction.GetComponent<IndexScript>()._obj8.GetComponent<Image>().color = MapManager.Instance._map._factions[_currentL3Int]._factionColor;
                string _repName = "";
                GetRepName(MapManager.Instance._map._factions[_currentL3Int]._defaultRep, out _repName, "");
                _menuObjectsL3Faction.GetComponent<IndexScript>()._obj9.GetComponent<Text>().text = "Default Relation: " + MapManager.Instance._map._factions[_currentL3Int]._defaultRep + " (" + _repName + ")";
                
            }
            else
            {
                _menuObjectsL3Faction.SetActive(false);
            }
        }
        else
        {
            _factionInt = 0;
        }
        
        if (_menuObjectsL3Alliance.activeSelf)
        {
            if (_currentL3Int != -1)
            {
                if (hit.transform.gameObject == _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj10 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3Alliance.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj11 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance.RemoveObject(_currentL2Int, _currentL3Int);
                    RebuildObjectMenuL2(_currentL2Int);
                    _menuObjectsL3Alliance.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3AllianceNameActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3AllianceShorthandActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3AllianceRedActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3AllianceGreenActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3AllianceBlueActive = true;
                }
                _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._name;
                _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._shorthand;
                _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.r.ToString();
                _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.g.ToString();
                _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.b.ToString();

                if (_menuObjectsL3AllianceNameActive)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<InputField>(), true, false, true, false, true, 40, out _menuObjectsL3AllianceNameActive);

                    MapManager.Instance._map._alliances[_currentL3Int]._name = _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3AllianceNameActive = false;
                        _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3AllianceNameActive = false;
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

                if (_menuObjectsL3AllianceShorthandActive)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<InputField>(), true, false, false, false, false, 4, out _menuObjectsL3AllianceShorthandActive);

                    MapManager.Instance._map._alliances[_currentL3Int]._shorthand = _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3AllianceShorthandActive = false;
                        _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3AllianceShorthandActive = false;
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

                if (_menuObjectsL3AllianceRedActive)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3AllianceRedActive);

                    int _col = 0;
                    if (_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._alliances[_currentL3Int]._allianceColor = new Color32((byte)_col, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.g, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.b, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3AllianceRedActive = false;
                        _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3AllianceRedActive = false;
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                if (_menuObjectsL3AllianceGreenActive)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3AllianceGreenActive);

                    int _col = 0;
                    if (_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._alliances[_currentL3Int]._allianceColor = new Color32(MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.r, (byte)_col, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.b, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3AllianceGreenActive = false;
                        _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3AllianceGreenActive = false;
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                if (_menuObjectsL3AllianceBlueActive)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3AllianceBlueActive);

                    int _col = 0;
                    if (_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._alliances[_currentL3Int]._allianceColor = new Color32(MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.r, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.g, (byte)_col, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3AllianceBlueActive = false;
                        _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3AllianceBlueActive = false;
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

            

                _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj8.GetComponent<Image>().color = MapManager.Instance._map._alliances[_currentL3Int]._allianceColor;

            }
            else
            {
                _menuObjectsL3Alliance.SetActive(false);
            }
        }
        
        if (_menuObjectsL3Jumpgate.activeSelf)
        {
            if (_currentL3Int != -1)
            {
                if (hit.transform.gameObject == _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj10 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3Jumpgate.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj11 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance.RemoveObject(_currentL2Int, _currentL3Int);
                    RebuildObjectMenuL2(_currentL2Int);
                    _menuObjectsL3Jumpgate.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3JumpgateNameActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
                {
                    RebuildContextMenu(0, true);
                    _contextSelection = false;
                    _currentActiveContext = 0;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj3 && Input.GetMouseButtonDown(0))
                {
                    RebuildContextMenu(0, true);
                    _contextSelection = false;
                    _currentActiveContext = 1;
                }


                _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._jumpGates[_currentL3Int]._name;
                if (MapManager.Instance._map._jumpGates[_currentL3Int]._sector1Id != -1)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj4.GetComponent<Text>().text = "Sector 1: " + MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[_currentL3Int]._sector1Id]._name;
                }
                else
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj4.GetComponent<Text>().text = "Sector 1: None";
                }

                if (MapManager.Instance._map._jumpGates[_currentL3Int]._sector2Id != -1)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = "Sector 2: " + MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[_currentL3Int]._sector2Id]._name;
                }
                else
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = "Sector 2: None";
                }

                
                if (_menuObjectsL3JumpgateNameActive)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(87, 87, 87, 255);


                    EnterText(_menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<InputField>(), true, false, true, true, true, 40, out _menuObjectsL3JumpgateNameActive);

                    MapManager.Instance._map._jumpGates[_currentL3Int]._name = _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3JumpgateNameActive = false;
                        _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3JumpgateNameActive = false;
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }


                if (_mOL3JumpGatesS1PlacementActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape)))
                {
                    _mOL3JumpGatesS1PlacementActive = false;
                }
                else if (_mOL3JumpGatesS1PlacementActive && Input.GetMouseButtonDown(0))
                {
                    Vector3 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    float xPos = _pos.x / .75f;
                    int xPosI = 0;
                    if (xPos > 0)
                    {
                        xPosI = Mathf.RoundToInt(xPos);
                    }
                    else
                    {
                        xPosI = Mathf.RoundToInt(xPos);
                    }
                    float yPos = 0;
                    if (Mathf.Pow(xPos, 2) % 2 == 0)
                    {
                        if (xPosI <= 0)
                        {
                            yPos = (_pos.y + .45f) / .9f;
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

                    int yPosI = 0;

                    if (yPos > 0)
                    {
                        yPosI = Mathf.RoundToInt(yPos);
                    }
                    else
                    {
                        yPosI = Mathf.RoundToInt(yPos);
                    }


                    for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
                    {
                        if (MapManager.Instance._map._sectors[i]._posXInt == xPosI && MapManager.Instance._map._sectors[i]._posYInt == yPosI)
                        {
                            MapManager.Instance._map._jumpGates[_currentL3Int]._sector1Id = i;
                        }
                    }

                    GalaxyMap.Instance._regen = true;
                    _mOL3JumpGatesS1PlacementActive = false;
                }

                if (hit.transform.gameObject == _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj6 && Input.GetMouseButtonDown(0))
                {
                    _mOL3JumpGatesS1PlacementActive = true;
                }

                if (_mOL3JumpGatesS1PlacementActive)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                }
                else
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj6.GetComponent<Image>().color = new Color32(118, 118, 118, 255);
                }

                if (_mOL3JumpGatesS2PlacementActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape)))
                {
                    _mOL3JumpGatesS2PlacementActive = false;
                }
                else if (_mOL3JumpGatesS2PlacementActive && Input.GetMouseButtonDown(0))
                {
                    Vector3 _pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    float xPos = _pos.x / .75f;
                    int xPosI = 0;
                    if (xPos > 0)
                    {
                        xPosI = Mathf.RoundToInt(xPos);
                    }
                    else
                    {
                        xPosI = Mathf.RoundToInt(xPos);
                    }
                    float yPos = 0;
                    if (Mathf.Pow(xPos, 2) % 2 == 0)
                    {
                        if (xPosI <= 0)
                        {
                            yPos = (_pos.y + .45f) / .9f;
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

                    int yPosI = 0;

                    if (yPos > 0)
                    {
                        yPosI = Mathf.RoundToInt(yPos);
                    }
                    else
                    {
                        yPosI = Mathf.RoundToInt(yPos);
                    }


                    for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
                    {
                        if (MapManager.Instance._map._sectors[i]._posXInt == xPosI && MapManager.Instance._map._sectors[i]._posYInt == yPosI)
                        {
                            MapManager.Instance._map._jumpGates[_currentL3Int]._sector2Id = i;
                        }
                    }

                    GalaxyMap.Instance._regen = true;
                    _mOL3JumpGatesS2PlacementActive = false;
                }

                if (hit.transform.gameObject == _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj7 && Input.GetMouseButtonDown(0))
                {
                    _mOL3JumpGatesS2PlacementActive = true;
                }

                if (_mOL3JumpGatesS2PlacementActive)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                }
                else
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj7.GetComponent<Image>().color = new Color32(118, 118, 118, 255);
                }

            }
            else
            {
                _menuObjectsL3Jumpgate.SetActive(false);
            }
        }
        
        if (_menuObjectsL3PlayerFaction.activeSelf)
        {
            if (_currentL3Int != -1)
            {
                if (hit.transform.gameObject == _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
                {
                    RebuildContextMenu(2, true);
                    _contextSelection = false;
                    _currentActiveContext = 0;
                }
                else if (hit.transform.gameObject == _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj10 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3PlayerFaction.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj11 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance.RemoveObject(_currentL2Int, _currentL3Int);
                    RebuildObjectMenuL2(_currentL2Int);
                    _menuObjectsL3PlayerFaction.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }

                if (MapManager.Instance._map._playerFactions[_currentL3Int]._regFactionID != -1)
                {
                    _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Parent Faction: " + MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[_currentL3Int]._regFactionID]._name;
                }
                else
                {
                    _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Parent Faction: None";
                }
                    

            }
            else
            {
                _menuObjectsL3PlayerFaction.SetActive(false);
            }
        }

        if (_menuObjectsL3Region.activeSelf)
        {
            if (_currentL3Int != -1)
            {
                if (MapManager.Instance._map._regCats[_currentL3Int]._regions.Count == 0)
                {
                    Region _rg = new Region();
                    _rg._regionColor = new Color32(255, 255, 255, 255);

                    MapManager.Instance._map._regCats[_currentL3Int]._regions.Add(_rg);
                }

                _subRegionInt = Mathf.Clamp(_subRegionInt, 0, MapManager.Instance._map._regCats[_currentL3Int]._regions.Count - 1);

                if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance._map._regCats[_currentL3Int]._knowledgeType = (1 - MapManager.Instance._map._regCats[_currentL3Int]._knowledgeType);

                    
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj10 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3Region.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj11 && Input.GetMouseButtonDown(0))
                {
                    MapManager.Instance.RemoveObject(_currentL2Int, _currentL3Int);
                    RebuildObjectMenuL2(_currentL2Int);
                    _menuObjectsL3Region.SetActive(false);
                    _contextMenu.SetActive(false);
                    return;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3RegionNameActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj20 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SubRegionNameActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj21 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SubRegionRedActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj22 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SubRegionGreenActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj23 && Input.GetMouseButtonDown(0))
                {
                    _menuObjectsL3SubRegionBlueActive = true;
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj13 && Input.GetMouseButtonDown(0))
                {
                    // Remove that specific reference

                    MapManager.Instance._map._regCats[_currentL3Int]._regions.Remove(MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]);


                    // Remove all sector references to it and replace with -1 ("No Data")
                    for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
                    {
                        for (int j = 0; j < MapManager.Instance._map._sectors[i]._regionCats.Count; j++)
                        {
                            if (MapManager.Instance._map._sectors[i]._regionCats[j] == _currentL3Int && MapManager.Instance._map._sectors[i]._regionCatsRegionIds[j] == _subRegionInt)
                            {
                                MapManager.Instance._map._sectors[i]._regionCatsRegionIds[j] = -1;
                            }
                            else if (MapManager.Instance._map._sectors[i]._regionCats[j] == _currentL3Int && MapManager.Instance._map._sectors[i]._regionCatsRegionIds[j] > _subRegionInt)
                            {
                                MapManager.Instance._map._sectors[i]._regionCatsRegionIds[j]--;
                            }
                        }
                        
                    }

                    _subRegionInt = Mathf.Clamp(_subRegionInt, 0, MapManager.Instance._map._regCats[_currentL3Int]._regions.Count - 1);
                }

                if (MapManager.Instance._map._regCats[_currentL3Int]._regions.Count <= 1)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj16.SetActive(false);
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj17.SetActive(false);
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj15.SetActive(false);
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj13.SetActive(false);
                }
                else
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj16.SetActive(true);
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj17.SetActive(true);
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj15.SetActive(true);
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj13.SetActive(true);
                }

                if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj14 && Input.GetMouseButtonDown(0))
                {
                    Region _rg = new Region();
                    _rg._name = "";
                    _rg._regionColor = new Color32(255, 255, 255, 255);

                    MapManager.Instance._map._regCats[_currentL3Int]._regions.Add(_rg);
                }
                else if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj15 && Input.GetMouseButtonDown(0))
                {
                    // Remove that specific reference

                    MapManager.Instance._map._regCats[_currentL3Int]._regions.Remove(MapManager.Instance._map._regCats[_currentL3Int]._regions[MapManager.Instance._map._regCats[_currentL3Int]._regions.Count - 1]);


                    // Remove all sector references to it and replace with -1 ("No Data")
                    for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
                    {
                        for (int j = 0; j < MapManager.Instance._map._sectors[i]._regionCats.Count; j++)
                        {
                            if (MapManager.Instance._map._sectors[i]._regionCats[j] == _currentL3Int && MapManager.Instance._map._sectors[i]._regionCatsRegionIds[j] == MapManager.Instance._map._regCats[_currentL3Int]._regions.Count)
                            {
                                MapManager.Instance._map._sectors[i]._regionCatsRegionIds[j] = -1;
                            }
                        }
                    }
                }

                if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj16 && Input.GetMouseButtonDown(0))
                {
                    if (_subRegionInt == MapManager.Instance._map._regCats[_currentL3Int]._regions.Count - 1)
                    {
                        _subRegionInt = 0;
                    }
                    else
                    {
                        _subRegionInt++;
                    }
                }

                if (hit.transform.gameObject == _menuObjectsL3Region.GetComponent<IndexScript>()._obj17 && Input.GetMouseButtonDown(0))
                {
                    if (_subRegionInt == 0)
                    {
                        _subRegionInt = MapManager.Instance._map._regCats[_currentL3Int]._regions.Count - 1;
                    }
                    else
                    {
                        _subRegionInt--;
                    }
                }


                _menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._name;
                _menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._name;

                _menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.r.ToString();
                _menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.g.ToString();
                _menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.b.ToString();

                _menuObjectsL3Region.GetComponent<IndexScript>()._obj18.GetComponent<Text>().text = "Region Amount: " + MapManager.Instance._map._regCats[_currentL3Int]._regions.Count;
                _menuObjectsL3Region.GetComponent<IndexScript>()._obj19.GetComponent<Text>().text = "Region #" + (_subRegionInt+1).ToString();

                

                if (MapManager.Instance._map._regCats[_currentL3Int]._knowledgeType == 0)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "EXPLORED SECTOR";
                }
                else
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "KNOWN SECTOR OWNER";
                }

                if (_menuObjectsL3RegionNameActive)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<InputField>(), true, true, true, true, true, 40, out _menuObjectsL3RegionNameActive);

                    MapManager.Instance._map._regCats[_currentL3Int]._name = _menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Region.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3RegionNameActive = false;
                        _menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3RegionNameActive = false;
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

                if (_menuObjectsL3SubRegionNameActive)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<Image>().color = new Color32(87, 87, 87, 255);



                    EnterText(_menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<InputField>(), true, true, true, true, true, 40, out _menuObjectsL3SubRegionNameActive);

                    MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._name = _menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<InputField>().text;
                    if (hit.transform.gameObject != _menuObjectsL3Region.GetComponent<IndexScript>()._obj20 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3SubRegionNameActive = false;
                        _menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3SubRegionNameActive = false;
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }

                if (_menuObjectsL3SubRegionRedActive)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3SubRegionRedActive);

                    int _col = 0;
                    if (_menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor = new Color32((byte)_col, MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.g, MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.b, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Region.GetComponent<IndexScript>()._obj21 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3SubRegionRedActive = false;
                        _menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3SubRegionRedActive = false;
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }
                if (_menuObjectsL3SubRegionGreenActive)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3SubRegionGreenActive);

                    int _col = 0;
                    if (_menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor = new Color32(MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.r, (byte)_col, MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.b, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Region.GetComponent<IndexScript>()._obj22 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3SubRegionGreenActive = false;
                        _menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3SubRegionGreenActive = false;
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }


                if (_menuObjectsL3SubRegionBlueActive)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<Image>().color = new Color32(87, 87, 87, 255);

                    EnterText(_menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<InputField>(), false, true, false, false, false, 3, out _menuObjectsL3SubRegionBlueActive);

                    int _col = 0;
                    if (_menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<InputField>().text != "")
                    {
                        _col = int.Parse(_menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<InputField>().text);
                    }
                    _col = Mathf.Clamp(_col, 0, 255);
                    MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor = new Color32(MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.r, MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.g, (byte)_col, 255);
                    if (hit.transform.gameObject != _menuObjectsL3Region.GetComponent<IndexScript>()._obj23 && Input.GetMouseButtonDown(0))
                    {

                        _menuObjectsL3SubRegionBlueActive = false;
                        _menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                    }
                }
                else
                {
                    _menuObjectsL3SubRegionBlueActive = false;
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<Image>().color = new Color32(67, 67, 67, 255);
                }


                _menuObjectsL3Region.GetComponent<IndexScript>()._obj24.GetComponent<Image>().color = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor;
                

            }
            else
            {
                _menuObjectsL3Region.SetActive(false);
            }
        }
        else
        {
            
            _subRegionInt = 0;
        }

        if (_menuObject2Objects.Count > 13)
        {

            _mObjectsm2Offset = Mathf.Clamp(_mObjectsm2Offset, 0, _menuObject2Objects.Count - 13);
            if (_mObjectsm2Offset != _mObjectsm2PrevOffset)
            {
                RepositionObjectsL2();
                _mObjectsm2PrevOffset = _mObjectsm2Offset;
            }
        }
        else
        {
            
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            if (_mObjectsm2Offset != _mObjectsm2PrevOffset)
            {
                RepositionObjectsL2();
            }
            _mObjectsm2PrevOffset = 0;
        }
        
        if (_contextMenu.activeSelf)
        {
            if (_contextSelection)
            {
                _contextMenu.GetComponent<IndexScript>()._obj2.SetActive(true);
                _contextMenu.GetComponent<IndexScript>()._obj3.SetActive(true);

                if (hit.transform.gameObject == _contextMenu.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
                {
                    SelectAll();
                }
                else if (hit.transform.gameObject == _contextMenu.GetComponent<IndexScript>()._obj3 && Input.GetMouseButtonDown(0))
                {
                    DeselectAll();
                }

                for (int i = 0; i < _contextMenuObjects.Count; i++)
                {
                    bool c = false;

                    if (_currentL2Int == 2 && _currentActiveContext == 0) // Discovered Sectors
                    {
                        for (int j = 0; j < MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._discoveredSectors[j] == i)
                            {
                                c = true;
                            }
                        }
                    }
                    else if (_currentL2Int == 2 && _currentActiveContext == 1) // Explored Sectors
                    {
                        for (int j = 0; j < MapManager.Instance._map._factions[_currentL3Int]._exploredSectors.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._exploredSectors[j] == i)
                            {
                                c = true;
                            }
                        }
                    }
                    else if (_currentL2Int == 2 && _currentActiveContext == 2) // Known Sector Owners
                    {
                        for (int j = 0; j < MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._knownSectorOwnership[j] == i)
                            {
                                c = true;
                            }
                        }
                    }
                    else if (_currentL2Int == 2 && _currentActiveContext == 3) // Known Factions
                    {
                        for (int j = 0; j < MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[_currentL3Int]._knownFactions[j] == i)
                            {
                                c = true;
                            }
                        }
                    }
                    else if (_currentL2Int == 0 && _currentActiveContext == 0) // Region Categories
                    {
                        for (int j = 0; j < MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count; j++)
                        {
                            if (MapManager.Instance._map._sectors[_currentL3Int]._regionCats[j] == i)
                            {
                                c = true;
                            }
                        }
                    }

                    if (!c)
                    {
                        _contextMenuObjects[i].GetComponent<IndexScript>()._obj2.SetActive(false);
                    }
                    else
                    {
                        _contextMenuObjects[i].GetComponent<IndexScript>()._obj2.SetActive(true);
                    } 
                        
                }
            }
            else
            {
                _contextMenu.GetComponent<IndexScript>()._obj2.SetActive(false);
                _contextMenu.GetComponent<IndexScript>()._obj3.SetActive(false);

                for (int i = 0; i < _contextMenuObjects.Count; i++)
                {
                    _contextMenuObjects[i].GetComponent<IndexScript>()._obj2.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
            {
                _contextMenu.SetActive(false);
            }
        }
        
        if (_contextMenuObjects.Count > 13)
        {

            _mObjectsContextOffset = Mathf.Clamp(_mObjectsContextOffset, 0, _contextMenuObjects.Count - 13);
            if (_mObjectsContextOffset != _mObjectsContextPrevOffset)
            {
                RepositionObjectsContext();
                _mObjectsContextPrevOffset = _mObjectsContextOffset;
            }
        }
        else
        {

            _mObjectsContextOffset = 0;
            _mObjectsContextOffsetVal = 0;
            if (_mObjectsContextOffset != _mObjectsContextPrevOffset)
            {
                RepositionObjectsContext();
            }
            _mObjectsContextPrevOffset = 0;
        }

        if (_settingsMenu.activeSelf)
        {
            if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj1 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.yBoundaryMax++;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.yBoundaryMax += 4;
                }
                MapManager.Instance._map.yBoundaryMax = Mathf.Clamp(MapManager.Instance._map.yBoundaryMax, 0, 10000);
            }
            else if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj2 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.yBoundaryMax--;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.yBoundaryMax -= 4;
                }
                MapManager.Instance._map.yBoundaryMax = Mathf.Clamp(MapManager.Instance._map.yBoundaryMax, 0, 10000);
            }
            else if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj4 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.yBoundaryMin++;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.yBoundaryMin += 4;
                }
                MapManager.Instance._map.yBoundaryMin = Mathf.Clamp(MapManager.Instance._map.yBoundaryMin, -10000, 0);
            }
            else if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj5 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.yBoundaryMin--;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.yBoundaryMin -= 4;
                }
                MapManager.Instance._map.yBoundaryMin = Mathf.Clamp(MapManager.Instance._map.yBoundaryMin, -10000, 0);
            }
            else if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj7 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.xBoundaryMax++;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.xBoundaryMax += 4;
                }
                MapManager.Instance._map.xBoundaryMax = Mathf.Clamp(MapManager.Instance._map.xBoundaryMax, 0, 10000);
            }
            else if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj8 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.xBoundaryMax--;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.xBoundaryMax -= 4;
                }
                MapManager.Instance._map.xBoundaryMax = Mathf.Clamp(MapManager.Instance._map.xBoundaryMax, 0, 10000);
            }
            else if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj10 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.xBoundaryMin++;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.xBoundaryMin += 4;
                }
                MapManager.Instance._map.xBoundaryMin = Mathf.Clamp(MapManager.Instance._map.xBoundaryMin, -10000, 0);
            }
            else if (hit.transform.gameObject == _settingsMenu.GetComponent<IndexScript>()._obj11 && Input.GetMouseButtonDown(0))
            {
                MapManager.Instance._map.xBoundaryMin--;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MapManager.Instance._map.xBoundaryMin -= 4;
                }
                MapManager.Instance._map.xBoundaryMin = Mathf.Clamp(MapManager.Instance._map.xBoundaryMin, -10000, 0);
            }


            _settingsMenu.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map.yBoundaryMax.ToString();
            _settingsMenu.GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = MapManager.Instance._map.yBoundaryMin.ToString();
            _settingsMenu.GetComponent<IndexScript>()._obj9.GetComponent<Text>().text = MapManager.Instance._map.xBoundaryMax.ToString();
            _settingsMenu.GetComponent<IndexScript>()._obj12.GetComponent<Text>().text = MapManager.Instance._map.xBoundaryMin.ToString();
        }
        
    }
}
