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

    public Scrollbar _scrollbarObj2;
    public Scrollbar _scrollbarObjContext;
    public Scrollbar _scrollbarObjConnContext;

    // Object Menu S3 Sector
    public GameObject _menuObjectsL3Sector;

    bool _menuObjectsL3SectorPlacementActive = false;

    int _sectorExtraId = 0;
    int _sectorRegionInt = 0;

    // Object Menu S3 Faction
    public GameObject _menuObjectsL3Faction;

    int _factionInt = 0;

    // Object Menu S3 Alliance
    public GameObject _menuObjectsL3Alliance;

    // Object Menu S3 Jumpgates
    [Header("Object Menu - Connections")]
    public GameObject _menuObjectsL3Jumpgate;

    public bool _mOL3JumpGatesS1PlacementActive = false;
    public bool _mOL3JumpGatesS2PlacementActive = false;

    // Connection type configuration menu
    [Header("Object Menu - Connection Type Config")]
    public GameObject _mConnType;
    public GameObject _mConnType_Context;
    public GameObject _mConnType_ContextButton; // Original Context Menu Button
    int _cType = -1;
    List<GameObject> _mConnTypeContextObjs = new List<GameObject>() { }; // LIST used to store context menu buttons -> INDEX - 1 = Connection Type ID (Because regular also has to be included)
    public int[] _mConnTypeContextOffset = { 0, 0 };
    public float _mConnTypeContextOffsetVal = 0;

    // Connection type color menu
    public GameObject _mConnColor;

    // Object Menu S3 Player Factions
    [Header("Object Menu - Player Factions")]
    public GameObject _menuObjectsL3PlayerFaction;
    public bool _mOL3PlayerFactionAnchorPlacementActive = false;

    // Object Menu S3 Region
    [Header("Object Menu - Regions")]
    public GameObject _menuObjectsL3Region;

    int _subRegionInt = 0;

    // Object Menu Context menu
    [Header("Object Menu - Context")]
    public GameObject _contextMenu;
    public GameObject _menuObjectContextButton; // Original Context Menu Button

    public List<GameObject> _contextMenuObjects = new List<GameObject>() { };

    public int _mObjectsContextOffset = 0;
    int _mObjectsContextPrevOffset = 0;
    public float _mObjectsContextOffsetVal = 0;
    bool _contextSelection = false;

    int _currentContextInt = 0;
    int _currentActiveContext = 0;

    // Object Menu Object Lists

    public List<GameObject> _menuObject1Objects = new List<GameObject>() { };
    public List<GameObject> _menuObject2Objects = new List<GameObject>() { };
    public List<string> _menuObject1Themes = new List<string>() { };

    // Map Settings
    [Header("Map Settings")]
    public GameObject _settingsMenu;

    // Export variables
    [Header("Export Settings")]
    bool _lockSel = false;
    int _playerFaction = -1;

    // Context Menu - Galaxy-Level
    [Header("Context Menu - Galaxy")]
    public GameObject _contextMenuG;
    public GameObject _contextMenuG_TextTemplate;
    public GameObject _contextMenuG_ButtonTemplate;
    public List<GameObject> _contextMenuG_objs = new List<GameObject>() { };
    protected int _contextMenuGSector = -1;
    protected Vector2 _contextMenuGSPos = new Vector2(0, 0);

    public GameObject _acmMenu;

    // ADD CONNECTION INTERFACE - Galaxy-level
    [Header("Connection Interface - Galaxy-Level")]
    public bool _addConnectionMActive = false;
    public int _s1Int = -1;
    public int _s2Int = -2;
    public int _aCMMode = 0; // 0 - Add, 1 - Remove, 2 - Edit
    int _aCMInt1 = -1;

    // Delete Confirmation Menu
    [Header("Delete Confirmation Menu")]
    public GameObject _delMenu;
    public int _delIntA = 0;
    public int _delIntB = -1;
    public bool _delMenuActive = false;

    // Reveal to - Menu
    public GameObject _revToMenu;
    public bool _revToMenuActive = false;
    public int _revToSec = -1;
    public int _typeMode = 0; // 0 - DISCOVERED; 1 - EXPLORED; 2 - KNOWN OWNER
    public List<GameObject> _revToMenu_objs = new List<GameObject>() { };
    public GameObject _revToMenu_ButtonTemplate;
    public GameObject _revToMenu_Button2;
    public bool _rTMSetup = true;

    // DISTANCE MEASUREMENT TOOL - Galaxy-level
    [Header("Distance Measurement Tool - Galaxy-Level")]
    public bool _dMeasurementActive = false;
    public bool _dActive = false;
    public int _toolS1Int = -1;
    public int _toolS2Int = -2;
    public GameObject _dMToolObj;
    public GameObject _dMMenu;
    public GameObject _dMDistanceNotice;

    // ADD CONNECTION INTERFACE - Galaxy-level
    [Header("Move Sector")]
    public bool _mSectorActive = false;
    public int _mSSInt = -1;
    public GameObject _mSMenu;

    // Change Owner - Menu
    public GameObject _changeOwnerMenu;
    public bool _changeOwnerMenuActive = false;
    public int _changeOwnerSec = -1;
    public List<GameObject> _changeOwnerMenu_objs = new List<GameObject>() { };
    public GameObject _changeOwnerMenu_ButtonTemplate;

    [Header("Paint Tool")]
    public GameObject _paintToolMenu;
    public bool _paintToolActive = false;

    int _paintToolModes = 3;

    int _paintToolMode = 0; // 0 - Connection Type, 1 - Regions, 2 - Visibility Status
    int _paintToolType = 0; // Connection Type - Irrelevant (By default 0) | Regions - Region Category ID | Visibility Status - Faction ID
    int _paintToolSubType = -1; // Connection Type | Regions -Region Category Subtype | Visibility Status - 0: Discovered, 1: Explored, 2: Known Sector

    int _paintToolS1 = -1; // Sector 1
    int _paintToolS2 = -1; // Sector 2

    public void SelectAll()
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

    public void DeselectAll()
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

    public void DebugOnOff()
    {
        MapManager.Instance._map._debug = !MapManager.Instance._map._debug;
    }

    public void ExportMenuOnOff()
    {
        if (_menuExport.activeSelf)
        {
            _menuExport.SetActive(false);
            
        }
        else
        {
            _menuExport.SetActive(true);
            
        }
    }

    public void ObjectMenuOnOff()
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

            _menuObject2Objects[i].GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -17 * (i - _mObjectsm2Offset));
            
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

    void RepositionObjectsConnContext()
    {
       
        for (int i = 0; i < _mConnTypeContextObjs.Count; i++)
        {
            if (i >= _mConnTypeContextOffset[0] && i < _mConnTypeContextOffset[0] + 13)
            {
                _mConnTypeContextObjs[i].SetActive(true);
            }
            else
            {
                _mConnTypeContextObjs[i].SetActive(false);
            }

            _mConnTypeContextObjs[i].GetComponent<RectTransform>().localPosition = new Vector2(32.5f, -15f + -15f * (i - _mConnTypeContextOffset[0]));

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


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._sectors[i].GetName(true);
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

    public void OBJ1(GameObject _obj)
    {
        for (int i = 0; i < _menuObject1Objects.Count; i++)
        {
            if (_obj == _menuObject1Objects[i])
            {
                OBJ2_FUNCTIONS(0, i);
                _menuObjectsL3Sector.SetActive(false);
                _menuObjectsL3Faction.SetActive(false);
                _menuObjectsL3Alliance.SetActive(false);
                _menuObjectsL3Jumpgate.SetActive(false);
                _menuObjectsL3Region.SetActive(false);
                _menuObjectsL3PlayerFaction.SetActive(false);
                _contextMenu.SetActive(false);

                break;
            } 
        }
            
    }

    public void OBJ2(GameObject _obj)
    {
        for (int i = 0; i < _menuObject2Objects.Count; i++)
        {
            if (_obj == _menuObject2Objects[i])
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

                

                break;
            }
        }
    }

    public void OBJ2_ADD()
    {
        MapManager.Instance.AddObject(_currentL2Int);
    }

    public void OBJ2_FUNCTIONS(int _index, int a)
    {
        if (_index == 0) // BUILD UI ELEMENTS
        {
            /*
            PURPOSE : Rebuild the L2 Object Menu based on the selected object
            INPUTS : a = index of the selected object type in L1 menu

                - (a):
                0 = Sectors
                1 = Jumpgate Connections
                2 = Factions
                3 = Alliances
                4 = Player Factions
                5 = Regions
                6 = Players

            METHOD : 
                (1) Activate L2 Menu
                (2) Set current L2 index
                (3) Destroy old buttons
                (4) Instantiate new buttons based on selected object type
                    - Set button text to object name
                    - Position buttons in menu
                    - Add buttons to list for future reference
                (5) Adjust offset and visibility of buttons based on total number of objects
                (6) Resize menu to fit buttons
            */

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

            // - Get count of objects based on type for later reference -
            int[] _count = { MapManager.Instance._map._sectors.Count,
                             MapManager.Instance._map._jumpGates.Count,
                             MapManager.Instance._map._factions.Count,
                             MapManager.Instance._map._alliances.Count,
                             MapManager.Instance._map._playerFactions.Count,
                             MapManager.Instance._map._regCats.Count,
                             MapManager.Instance._map._players.Count
            };

            
            // Offset stuff for Button
            _mObjectsm2Offset = 0;
            _mObjectsm2OffsetVal = 0;
            _mObjectsm2PrevOffset = 0;
            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, 0, _count[a]);
            _mObjectsm2Min = Mathf.Clamp(_mObjectsm2Min, 0, _mObjectsm2Max);

            _mObjectsm2Max = Mathf.Clamp(_mObjectsm2Max, _mObjectsm2Min, _mObjectsm2Min + 13);

            for (int i = 0; i < _count[a]; i++)
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

                // Alternating button colors
                if ((i - _mObjectsm2Offset) % 2 == 0)
                {
                    _buttonClone.GetComponent<Image>().color = MapManager.Instance._uiButtonColors[0]; // LIGHT
                }
                else
                {
                    _buttonClone.GetComponent<Image>().color = MapManager.Instance._uiButtonColors[1]; // DARK
                }

                // GET NAME OF OBJECT BASED ON TYPE
                string[] _t = {(i < _count[0]) ? MapManager.Instance._map._sectors[i].GetName(false) : "",
                               (i < _count[1]) ? MapManager.Instance._map._jumpGates[i]._name : "",
                               (i < _count[2]) ? MapManager.Instance._map._factions[i]._name : "",
                               (i < _count[3]) ? MapManager.Instance._map._alliances[i]._name : "",
                               (i < _count[4]) ? ((MapManager.Instance._map._playerFactions[i]._regFactionID != -1) ? MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[i]._regFactionID]._name : "Unassigned") : "",
                               (i < _count[5]) ? MapManager.Instance._map._regCats[i]._name : "",
                               (i < _count[6]) ? MapManager.Instance._map._players[i]._name : ""
                };

                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = _t[a]; // Get displayed text on button
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(40f, -14f + -17 * (i - _mObjectsm2Offset)); // Set position of button

                _menuObject2Objects.Add(_buttonClone);
            }

            // Resize menu
            if (_menuObject2Objects.Count <= 13)
            {
                _menuObjectsL2.GetComponent<RectTransform>().sizeDelta = new Vector2(90f, 15 + 17 * _menuObject2Objects.Count);
                _menuObjectsL2.GetComponent<BoxCollider>().size = new Vector3(90, 15 + 17 * _menuObject2Objects.Count, 1);
                _menuObjectsL2.GetComponent<BoxCollider>().center = new Vector3(0, (15 + 17 * _menuObject2Objects.Count) / 2 * -1, 0);
            }
            else
            {
                _menuObjectsL2.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 236);
                _menuObjectsL2.GetComponent<BoxCollider>().size = new Vector3(90, 236, 1);
                _menuObjectsL2.GetComponent<BoxCollider>().center = new Vector3(0, -118f, 0);
            }
        }
    }

    public void OBJ3_SECTOR_FUNCTIONS(int _a)
    {
        if (_a == 0)
        {
            RebuildContextMenu(2, true);
            _contextSelection = false;
            _currentActiveContext = 0;
        }
        else if (_a == 1) // POS X INT
        {
            InputField _in = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj4.GetComponent<InputField>();
            if (_in.text != "" || _in.text != "-")
            {
                MapManager.Instance._map._sectors[_currentL3Int]._posXInt = int.Parse(_in.text);
            }
            else
            {
                MapManager.Instance._map._sectors[_currentL3Int]._posXInt = 0;
            }

            GalaxyMap.Instance._regen = true;

        }
        else if (_a == 2) // POS Y INT
        {
            InputField _in = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj5.GetComponent<InputField>();
            if (_in.text != "" || _in.text != "-")
            {
                MapManager.Instance._map._sectors[_currentL3Int]._posYInt = int.Parse(_in.text);
            }
            else
            {
                MapManager.Instance._map._sectors[_currentL3Int]._posYInt = 0;
            }

            GalaxyMap.Instance._regen = true;
        }
        else if (_a == 3) // CLOSE
        {
            _menuObjectsL3Sector.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 4) // REMOVE
        {
            _delIntA = _currentL2Int;
            _delIntB = _currentL3Int;
            _delMenuActive = true;

            _menuObjects.SetActive(false);
            _menuObjectsL2.SetActive(false);
            _menuObjectsL3Sector.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 5) // NAME
        {
            InputField _in = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<InputField>();

            MapManager.Instance._map._sectors[_currentL3Int]._name = _in.text;
        }
        else if (_a == 6) // SECTOR PLACEMENT ACTIVE
        {
            _menuObjectsL3SectorPlacementActive = true;
        }
        else if (_a == 7) // Sector Extra ID Increase
        {
            List<string> _t1 = new List<string>() { "Description", "Lore", "Regions" };
            if (_sectorExtraId == _t1.Count - 1)
            {
                _sectorExtraId = 0;
            }
            else
            {
                _sectorExtraId++;
            }
        }
        else if (_a == 8) // Sector Extra ID Decrease
        {
            List<string> _t1 = new List<string>() { "Description", "Lore", "Regions" };
            if (_sectorExtraId == 0)
            {
                _sectorExtraId = _t1.Count - 1;
            }
            else
            {
                _sectorExtraId--;
            }
        }
        else if (_a == 9) // Description Text
        {
            InputField _in = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<InputField>();

            MapManager.Instance._map._sectors[_currentL3Int]._description = _in.text;
        }
        else if (_a == 10) // Lore Text
        {
            InputField _in = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<InputField>();

            MapManager.Instance._map._sectors[_currentL3Int]._lore = _in.text;
        }
        else if (_a == 11) // Region Sub Increase
        {
            _sectorRegionInt++;
            if (_sectorRegionInt > MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count - 1)
            {
                _sectorRegionInt = 0;
            }
        }
        else if (_a == 12) // Region Sub Decrease
        {
            _sectorRegionInt--;
            if (_sectorRegionInt < 0)
            {
                _sectorRegionInt = MapManager.Instance._map._sectors[_currentL3Int]._regionCats.Count - 1;
            }
        }
        else if (_a == 13) // Region Val Increase
        {
            int a = MapManager.Instance._map._sectors[_currentL3Int]._regionCats[_sectorRegionInt];

            MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt]++;
            if (MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] > MapManager.Instance._map._regCats[a]._regions.Count - 1)
            {
                MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] = -1;
            }
        }
        else if (_a == 14) // Region Val Decrease
        {
            int a = MapManager.Instance._map._sectors[_currentL3Int]._regionCats[_sectorRegionInt];

            MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt]--;
            if (MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] < -1)
            {
                MapManager.Instance._map._sectors[_currentL3Int]._regionCatsRegionIds[_sectorRegionInt] = MapManager.Instance._map._regCats[a]._regions.Count - 1;
            }
        }
        else if (_a == 15) // Region Context Menu
        {
            RebuildContextMenu(5, false);
            _contextSelection = true;
            _currentActiveContext = 0;
        }
        else if (_a == 16) // HIDDEN NAME
        {
            InputField _in = _menuObjectsL3Sector.GetComponent<IndexScript>()._obj6.GetComponent<InputField>();

            MapManager.Instance._map._sectors[_currentL3Int]._hiddenName = _in.text;
        }

    }

    public void OBJ3_FACTION_FUNCTIONS(int _a)
    {
        if (_a == 0) // Alliance selection
        {
            _contextSelection = false;
            RebuildContextMenu(3, true);
            _currentActiveContext = 0;
        }
        else if (_a == 1) // Close
        {
            _menuObjectsL3Faction.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 2) // Remove
        {
            _delIntA = _currentL2Int;
            _delIntB = _currentL3Int;
            _delMenuActive = true;

            _menuObjects.SetActive(false);
            _menuObjectsL2.SetActive(false);
            _menuObjectsL3Faction.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 3) // Name
        {
            InputField _in = _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<InputField>();

            MapManager.Instance._map._factions[_currentL3Int]._name = _in.text;
        }
        else if (_a == 4) // Shorthand
        {
            InputField _in = _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<InputField>();

            MapManager.Instance._map._factions[_currentL3Int]._shorthand = _in.text;
        }
        else if (_a == 5) // Red
        {
            InputField _in = _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<InputField>();

            int _b = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _b = int.Parse(_in.text);
            }
                

            MapManager.Instance._map._factions[_currentL3Int]._factionColor = new Color32((byte)_b, MapManager.Instance._map._factions[_currentL3Int]._factionColor.g, MapManager.Instance._map._factions[_currentL3Int]._factionColor.b, 255);
        }
        else if (_a == 6) // Green
        {
            InputField _in = _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<InputField>();

            int _b = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _b = int.Parse(_in.text);
            }


            MapManager.Instance._map._factions[_currentL3Int]._factionColor = new Color32(MapManager.Instance._map._factions[_currentL3Int]._factionColor.r, (byte)_b, MapManager.Instance._map._factions[_currentL3Int]._factionColor.b, 255);
        }
        else if (_a == 7) // Blue
        {
            InputField _in = _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<InputField>();

            int _b = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _b = int.Parse(_in.text);
            }


            MapManager.Instance._map._factions[_currentL3Int]._factionColor = new Color32(MapManager.Instance._map._factions[_currentL3Int]._factionColor.r, MapManager.Instance._map._factions[_currentL3Int]._factionColor.g, (byte)_b, 255);
        }
        else if (_a == 8) // Default Rep Adjustment - Down
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
        else if (_a == 9) // Default Rep Adjustment - Up
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
        else if (_a == 10) // Rep Adjustment - Up
        {
            int _repInt = -1;
            _factionInt = Mathf.Clamp(_factionInt, 0, MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1);
            int _facInt = MapManager.Instance._map._factions[_currentL3Int]._knownFactions[_factionInt];
            while (_facInt == _currentL3Int)
            {
                _factionInt++;
                if (_factionInt > MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1)
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
        else if (_a == 11) // Rep Adjustment - Down
        {
            int _repInt = -1;
            _factionInt = Mathf.Clamp(_factionInt, 0, MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1);
            int _facInt = MapManager.Instance._map._factions[_currentL3Int]._knownFactions[_factionInt];
            while (_facInt == _currentL3Int)
            {
                _factionInt++;
                if (_factionInt > MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1)
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
        else if (_a == 12) // Discovered Sectors
        {
            _contextSelection = true;
            RebuildContextMenu(0, false);
            _currentActiveContext = 0;
        }
        else if (_a == 13) // Explored Sectors
        {
            _contextSelection = true;
            RebuildContextMenu(0, false);
            _currentActiveContext = 1;
        }
        else if (_a == 14) // Known Sector Owners
        {
            _contextSelection = true;
            RebuildContextMenu(0, false);
            _currentActiveContext = 2;
        }
        else if (_a == 15) // Known Factions
        {
            _contextSelection = true;
            RebuildContextMenu(2, false);
            _currentActiveContext = 3;
        }
        else if (_a == 16) // Rep Increase faction Int
        {
            _factionInt++;
            _factionInt %= MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count;
        }
        else if (_a == 17) // Rep Decrease Faction Int
        {
            _factionInt--;
            if (_factionInt < 0)
            {
                _factionInt = MapManager.Instance._map._factions[_currentL3Int]._knownFactions.Count - 1;
            }
        }
        else if (_a == 18) // Special Rep Condition Context
        {
            _contextSelection = false;
            RebuildContextMenu(100, true);
            _currentActiveContext = 1;
        }
    }

    public void OBJ3_ALLIANCE_FUNCTIONS(int _a)
    {
        if (_a == 0) // Close
        {
            _menuObjectsL3Alliance.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 1) // Remove
        {
            _delIntA = _currentL2Int;
            _delIntB = _currentL3Int;
            _delMenuActive = true;

            _menuObjects.SetActive(false);
            _menuObjectsL2.SetActive(false);
            _menuObjectsL3Alliance.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 2) // Name
        {
            InputField _in = _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<InputField>();

            MapManager.Instance._map._alliances[_currentL3Int]._name = _in.text;
        }
        else if (_a == 3) // Shorthand
        {
            InputField _in = _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<InputField>();

            MapManager.Instance._map._alliances[_currentL3Int]._shorthand = _in.text;
        }
        else if (_a == 4) // Red
        {
            InputField _in = _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<InputField>();

            int _b = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _b = int.Parse(_in.text);
            }


            MapManager.Instance._map._alliances[_currentL3Int]._allianceColor = new Color32((byte)_b, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.g, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.b, 255);
        }
        else if (_a == 5) // Green
        {
            InputField _in = _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<InputField>();

            int _b = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _b = int.Parse(_in.text);
            }


            MapManager.Instance._map._alliances[_currentL3Int]._allianceColor = new Color32(MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.r, (byte)_b, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.b, 255);
        }
        else if (_a == 6) // Blue
        {
            InputField _in = _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<InputField>();

            int _b = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _b = int.Parse(_in.text);
            }


            MapManager.Instance._map._alliances[_currentL3Int]._allianceColor = new Color32(MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.r, MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.g, (byte)_b, 255);
        }

    }

    public void OBJ3_CONNECTION_FUNCTIONS(int _a)
    {
        if (_a == 0) // Close
        {
            _menuObjectsL3Jumpgate.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 1) // Remove
        {
            _delIntA = _currentL2Int;
            _delIntB = _currentL3Int;
            _delMenuActive = true;

            _menuObjects.SetActive(false);
            _menuObjectsL2.SetActive(false);
            _menuObjectsL3Jumpgate.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 2) // Name
        {
            InputField _in = _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<InputField>();

            MapManager.Instance._map._jumpGates[_currentL3Int]._name = _in.text;
        }
        else if (_a == 3) // Sector 1 Context
        {
            RebuildContextMenu(0, true);
            _contextSelection = false;
            _currentActiveContext = 0;
        }
        else if (_a == 4) // Sector 2 Context
        {
            RebuildContextMenu(0, true);
            _contextSelection = false;
            _currentActiveContext = 1;
        }
        else if (_a == 5) // Sector 1 Manual Placement
        {
            _mOL3JumpGatesS1PlacementActive = true;
        }
        else if (_a == 6) // Sector 2 Manual Placement
        {
            _mOL3JumpGatesS2PlacementActive = true;
        }
        // ---- CONNECTION TYPE MENU FUNCTIONS ----
        else if (_a == 7)
        {
            /* OPEN MENU
            PURPOSE : Open menu for selected connection type
            INPUTS : _cType = Index of selected connection type
            METHOD :
                (1) Set Connection Type Menu to active
                (2) Initialize menu fields based on conn. type data (Name, width, color)
            */

            if (_cType <= -1 || _cType >= MapManager.Instance._map._connType.Count)
            {
                return;
            }

            GameObject _menu = _mConnType;
            _menu.SetActive(true); // ACTIVATE

            // Set fields
            if (!_menu.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().isFocused)
            {
                _menu.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._connType[_cType]._name; // SET NAME IF NOT FOCUSED
            }
            Text _tLW = _menu.GetComponent<IndexScript>()._obj2.GetComponent<Text>(); // LINE WIDTH TEXT
            _tLW.text = "LINE WIDTH: " + MapManager.Instance._map._connType[_cType]._lineWidth.ToString("0.00") + "x";

            Slider _s1 = _menu.GetComponent<IndexScript>()._obj3.GetComponent<Slider>(); // LINE WIDTH SLIDER
            _s1.enabled = false;
            _s1.value = MapManager.Instance._map._connType[_cType]._lineWidth;
            _s1.enabled = true;

            Text _tLT = _menu.GetComponent<IndexScript>()._obj6.GetComponent<Text>(); // LINE TYPE TEXT;
            string[] _tT = { "REGULAR", "DASHED", "DOTTED", "SLANTED" };
            _tLT.text = "LINE TYPE: " + _tT[MapManager.Instance._map._connType[_cType]._lineType];

            Slider _s2 = _menu.GetComponent<IndexScript>()._obj7.GetComponent<Slider>(); // LINE TYPE SLIDER
            _s2.enabled = false;
            _s2.value = MapManager.Instance._map._connType[_cType]._lineType;
            _s2.enabled = true;

            Image _colImg = _menu.GetComponent<IndexScript>()._obj8.GetComponent<Image>();
            _colImg.color = MapManager.Instance._map._connType[_cType]._lineColor; // COLOR

            return;
        }
        else if (_a == 8)
        {
            /* CONN TYPE VARIABLE CHANGED
            PURPOSE : Refresh variables in connection type menu
            INPUTS : None (button interaction)
            METHOD :
                (1) Get input field values
                (2) Validate values
                (3) Set values to relevant connection type variables
            */

            if (_cType <= -1 || _cType >= MapManager.Instance._map._connType.Count)
            {
                return;
            }

            GameObject _menu = _mConnType;

            MapManager.Instance._map._connType[_cType]._name = _menu.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text; // NAME
            MapManager.Instance._map._connType[_cType]._lineWidth = Mathf.Round(_menu.GetComponent<IndexScript>()._obj3.GetComponent<Slider>().value * 100) / 100; // LINE WIDTH
            MapManager.Instance._map._connType[_cType]._lineType = Mathf.RoundToInt(_menu.GetComponent<IndexScript>()._obj7.GetComponent<Slider>().value);

            Text _tLW = _menu.GetComponent<IndexScript>()._obj2.GetComponent<Text>(); // LINE WIDTH TEXT
            _tLW.text = "LINE WIDTH: " + MapManager.Instance._map._connType[_cType]._lineWidth.ToString("0.00") + "x";

            Text _tLT = _menu.GetComponent<IndexScript>()._obj6.GetComponent<Text>(); // LINE TYPE TEXT;
            string[] _tT = { "REGULAR", "DASHED", "DOTTED", "SLANTED" };
            _tLT.text = $"LINE TYPE: {_tT[MapManager.Instance._map._connType[_cType]._lineType]}";

            Image _colImg = _menu.GetComponent<IndexScript>()._obj8.GetComponent<Image>();
            _colImg.color = MapManager.Instance._map._connType[_cType]._lineColor; // COLOR

            Text _tRB = _menu.GetComponent<IndexScript>()._obj9.GetComponent<Text>(); // REQUIRE BOTH TEXT
            string[] _tRBOpt = {"Yes", "No"};
            int _tRB1 = MapManager.Instance._map._connType[_cType]._reqBothExplored ? 0 : 1;
            _tRB.text = $"HIDE UNTIL BOTH ENDPOINTS EXPLORED: {_tRBOpt[_tRB1]}";

            return;
        }
        else if (_a == 9) // Close Conn Type Menu
        {
            _mConnType.SetActive(false);
            GalaxyMap.Instance._regen = true;
            return;
        }
        else if (_a == 10) // Delete Conn Type
        {
            if (_cType < 0 || _cType >= MapManager.Instance._map._connType.Count)
            {
                _mConnType.SetActive(false);
                return;
            }

            _delIntA = 6;
            _delIntB = _cType;
            _delMenuActive = true;

            _mConnType.SetActive(false);
            return;
        }
        else if (_a == 11)
        {
            /* REBUILD CONNECTION TYPE CONTEXT MENU
            PURPOSE : Rebuild context menu for connection type selection
            INPUTS : None (button interaction)
            METHOD :
                (1) Set context menu to active
                (2) Destroy old buttons
                (3) Instantiate new buttons based on connection types (+ Regular & Add options)
                (4) Resize menu based on number of buttons
            */
            _mConnType_Context.SetActive(true);
            GameObject _m = _mConnType_Context;

            // Destroy old buttons
            List<GameObject> _buttons = _mConnTypeContextObjs;
            _mConnTypeContextObjs = new List<GameObject>();
            for (int i = 0; i < _buttons.Count; i++)
            {
                Destroy(_buttons[i]);
            }

            // -- Instantiate new Buttons --

            // Offset stuff
            _mConnTypeContextOffset = new int[] { 0, 0 };
            _mConnTypeContextOffsetVal = 0;

            // 'Regular' option (-1)
            GameObject _buttonCloneB = Instantiate(_mConnType_ContextButton, _m.transform);

            if (0 >= _mConnTypeContextOffset[0] && 0 < _mConnTypeContextOffset[0] + 13)
            {
                _buttonCloneB.SetActive(true);
            }
            else
            {
                _buttonCloneB.SetActive(false);
            }

            float _offset = 1;

            _buttonCloneB.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "REGULAR";
            _buttonCloneB.GetComponent<RectTransform>().localPosition = new Vector2(32.5f, -15f + -15 * (0 - _mConnTypeContextOffset[0]));

            _buttonCloneB.GetComponent<IndexScript>()._obj2.SetActive(false);

            _mConnTypeContextObjs.Add(_buttonCloneB);

            // TYPE SELECTIONS
            for (int i = 0; i < MapManager.Instance._map._connType.Count; i++)
            {
                GameObject _buttonClone = Instantiate(_mConnType_ContextButton, _m.transform);
                if (i + _offset >= _mConnTypeContextOffset[0] && i + _offset < _mConnTypeContextOffset[0] + 13)
                {
                    _buttonClone.SetActive(true);
                }
                else
                {
                    _buttonClone.SetActive(false);
                }


                _buttonClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._connType[i]._name.ToUpper();
                _buttonClone.GetComponent<RectTransform>().localPosition = new Vector2(32.5f, -15f + -15f * (i + _offset - _mConnTypeContextOffset[0]));

                _mConnTypeContextObjs.Add(_buttonClone);
            }

            // 'Add' Button (Count) -> Add new Type
            GameObject _buttonCloneC = Instantiate(_mConnType_ContextButton, _m.transform);

            if (MapManager.Instance._map._connType.Count + _offset >= _mConnTypeContextOffset[0] && MapManager.Instance._map._connType.Count + _offset < _mConnTypeContextOffset[0] + 13)
            {
                _buttonCloneC.SetActive(true);
            }
            else
            {
                _buttonCloneC.SetActive(false);
            }

            _buttonCloneC.GetComponent<IndexScript>()._obj2.SetActive(false);

            _buttonCloneC.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "ADD (+)";
            _buttonCloneC.GetComponent<RectTransform>().localPosition = new Vector2(32.5f, -15f + -15f * (MapManager.Instance._map._connType.Count + _offset - _mConnTypeContextOffset[0]));

            _mConnTypeContextObjs.Add(_buttonCloneC);

            // Resize menu
            if (_mConnTypeContextObjs.Count <= 13)
            {
                _m.GetComponent<RectTransform>().sizeDelta = new Vector2(70f, 15 + 15f * _mConnTypeContextObjs.Count);
                _m.GetComponent<BoxCollider>().size = new Vector3(70, 15 + 15f * _mConnTypeContextObjs.Count, 1);
                _m.GetComponent<BoxCollider>().center = new Vector3(0, (15 + 15f * _mConnTypeContextObjs.Count) / 2 * -1, 0);
            }
            else
            {
                _m.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 210f);
                _m.GetComponent<BoxCollider>().size = new Vector3(70, 210f, 1);
                _m.GetComponent<BoxCollider>().center = new Vector3(0, -105f, 0);
            }

            return;
        }
        else if (_a == 12) // Close Conn Type Context Menu
        {
            _mConnType_Context.SetActive(false);
            return;
        }
        else if (_a == 13) // Add Conn Type
        {
            /* ADD NEW CONNECTION TYPE
            PURPOSE : Add new connection type to map after "Add" button pressed in context menu
            INPUTS : None (button interaction)
            METHOD :
                (1) Add new connection type to map with default values
                (2) Set GalaxyMap to reset
            */
            MapManager.Instance.AddObject(6); // Add new connection type
            GalaxyMap.Instance._regen = true;

            return;
        }
        else if (_a == 14) // OPEN / CLOSE CONN TYPE COLOR CONFIG
        {
            

            Slider _sR = _mConnColor.GetComponent<IndexScript>()._obj1.GetComponent<Slider>(); // RED SLIDER
            Slider _sG = _mConnColor.GetComponent<IndexScript>()._obj2.GetComponent<Slider>(); // GREEN SLIDER
            Slider _sB = _mConnColor.GetComponent<IndexScript>()._obj3.GetComponent<Slider>(); // BLUE SLIDER

            _sG.SetValueWithoutNotify(Mathf.RoundToInt(MapManager.Instance._map._connType[_cType]._lineColor.g * 255));
            _sR.SetValueWithoutNotify(Mathf.RoundToInt(MapManager.Instance._map._connType[_cType]._lineColor.r * 255));
            _sB.SetValueWithoutNotify(Mathf.RoundToInt(MapManager.Instance._map._connType[_cType]._lineColor.b * 255));

            // Text
            _mConnColor.GetComponent<IndexScript>()._obj4.GetComponent<Text>().text = "RED: " + Mathf.RoundToInt(_sR.value);
            _mConnColor.GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = "GREEN: " + Mathf.RoundToInt(_sG.value);
            _mConnColor.GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "BLUE: " + Mathf.RoundToInt(_sB.value);

            _mConnColor.SetActive(!_mConnColor.activeSelf);

            return;
        }
        else if (_a == 15) // CONN TYPE COLOR CHANGED
        {
            if (_cType <= -1 || _cType >= MapManager.Instance._map._connType.Count)
            {
                return;
            }

            GameObject _menu = _mConnType;

            // -- Get Sliders & Update Color value --
            Slider _sR = _mConnColor.GetComponent<IndexScript>()._obj1.GetComponent<Slider>(); // RED SLIDER
            Slider _sG = _mConnColor.GetComponent<IndexScript>()._obj2.GetComponent<Slider>(); // GREEN SLIDER
            Slider _sB = _mConnColor.GetComponent<IndexScript>()._obj3.GetComponent<Slider>(); // BLUE SLIDER

            MapManager.Instance._map._connType[_cType]._lineColor = new Color32((byte)Mathf.RoundToInt(_sR.value), (byte)Mathf.RoundToInt(_sG.value), (byte)Mathf.RoundToInt(_sB.value), 255);

            // -- Update Color displays --
            // Color Square
            Image _colImg = _menu.GetComponent<IndexScript>()._obj8.GetComponent<Image>();
            _colImg.color = MapManager.Instance._map._connType[_cType]._lineColor; // COLOR UPDATE

            // Text
            _mConnColor.GetComponent<IndexScript>()._obj4.GetComponent<Text>().text = "RED: " + Mathf.RoundToInt(_sR.value);
            _mConnColor.GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = "GREEN: " + Mathf.RoundToInt(_sG.value);
            _mConnColor.GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "BLUE: " + Mathf.RoundToInt(_sB.value);

            return;
        }
        else if (_a == 16) // CHANGE Req Both Requirement
        {
            if (_cType <= -1 || _cType >= MapManager.Instance._map._connType.Count)
            {
                return;
            }

            MapManager.Instance._map._connType[_cType]._reqBothExplored = !MapManager.Instance._map._connType[_cType]._reqBothExplored;

            OBJ3_CONNECTION_FUNCTIONS(8); // Refresh Menu
        }

    }

    public void OBJ3_CONNTYPE_CONTEXT_HANDLER(GameObject _obj)
    {
        /* HANDLER FOR CONNECTION TYPE CONTEXT MENU SELECTIONS -> CONNECT TO OBJ3_CONNECTION_FUNCTIONS
        PURPOSE : Handle context menu selections for connection types -> redirect button interactions to relevant functions & figure out necessary variables
        INPUTS : _obj = Button obj
        METHOD :
            (1) Get index of selected connection type by comparing button object to list of buttons & list offset (in case of 'overflow')
            (2) Set _cType to selected index
            (3) Redirect to important function
            (4) Close context menu
        */

        // - Select Connection Type -
        for (int i = 0; i < _mConnTypeContextObjs.Count; i++)
        {
            if (_obj == _mConnTypeContextObjs[i] && i <= MapManager.Instance._map._connType.Count)
            {
                MapManager.Instance._map._jumpGates[_currentL3Int]._typeId = i - 1; // -1 = Regular
                GalaxyMap.Instance._regen = true;

                OBJ3_CONNECTION_FUNCTIONS(9); // Close Menu
                OBJ3_CONNECTION_FUNCTIONS(12); // Close Context Menu

                return;
            }
            else if (_obj == _mConnTypeContextObjs[i] && i == MapManager.Instance._map._connType.Count + 1)
            {
                OBJ3_CONNECTION_FUNCTIONS(13); // Add Conn Type
                OBJ3_CONNECTION_FUNCTIONS(11); // Rebuild Context Menu

                return;
            }
        }

        // - Open Menu -
        for (int i = 1; i < _mConnTypeContextObjs.Count; i++)
        {
            if (_obj == _mConnTypeContextObjs[i].GetComponent<IndexScript>()._obj2 && i <= MapManager.Instance._map._connType.Count)
            {
                _cType = i - 1; // -1 = Regular - shouldn't open
                OBJ3_CONNECTION_FUNCTIONS(7); // Open Conn Type Menu

                return;
            }
        }
    }

    public void OBJ3_PLAYERFACTION_FUNCTIONS(int _a)
    {
        if (_a == 0) // Close
        {
            _menuObjectsL3PlayerFaction.SetActive(false);
            _mOL3PlayerFactionAnchorPlacementActive = false;
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 1) // Remove
        {
            _delIntA = _currentL2Int;
            _delIntB = _currentL3Int;
            _delMenuActive = true;

            _menuObjects.SetActive(false);
            _menuObjectsL2.SetActive(false);
            _menuObjectsL3PlayerFaction.SetActive(false);
            _mOL3PlayerFactionAnchorPlacementActive = false;
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 2) // Context Menu: Factions
        {
            RebuildContextMenu(2, true);
            _contextSelection = false;
            _currentActiveContext = 0;
        }
        else if (_a == 3) // Start Anchor Placement
        {
            _mOL3PlayerFactionAnchorPlacementActive = true;
        }
        else if (_a == 4) // Cancel Anchor Placement
        {
            _mOL3PlayerFactionAnchorPlacementActive = false;
        }
        else if (_a == 5) // Confirm Anchor Placement
        {
            _mOL3PlayerFactionAnchorPlacementActive = false;
            MapManager.Instance._map._playerFactions[_currentL3Int]._spawnPosition = MapManager.Instance.GetMouseSectorPos;
        }
    }

    public void OBJ3_REGION_FUNCTIONS(int _a)
    {
        if (_a == 0) // KNOWLEDGE TYPE
        {
            MapManager.Instance._map._regCats[_currentL3Int]._knowledgeType = (1 - MapManager.Instance._map._regCats[_currentL3Int]._knowledgeType);
        }
        else if (_a == 1) // CLOSE
        {
            _menuObjectsL3Region.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 2) // REMOVE
        {
            _delIntA = _currentL2Int;
            _delIntB = _currentL3Int;
            _delMenuActive = true;

            _menuObjects.SetActive(false);
            _menuObjectsL2.SetActive(false);
            _menuObjectsL3Region.SetActive(false);
            _contextMenu.SetActive(false);
            return;
        }
        else if (_a == 3) // CATEGORY NAME
        {
            InputField _in = _menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<InputField>();

            MapManager.Instance._map._regCats[_currentL3Int]._name = _in.text;
        }
        else if (_a == 4) // SUBCATEGORY NAME
        {
            InputField _in = _menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<InputField>();

            MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._name = _in.text;
        }
        else if (_a == 5) // SUBCATEGORY RED
        {
            InputField _in = _menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<InputField>();

            int _c = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _c = int.Parse(_in.text);
            }

            Color32 _b = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor;
            MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor = new Color32((byte)_c, _b.g,_b.b, 255);
        }
        else if (_a == 6) // SUBCATEGORY GREEN
        {
            InputField _in = _menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<InputField>();

            int _c = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _c = int.Parse(_in.text);
            }

            Color32 _b = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor;
            MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor = new Color32(_b.r, (byte)_c, _b.b, 255);
        }
        else if (_a == 7) // SUBCATEGORY BLUE
        {
            InputField _in = _menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<InputField>();

            int _c = 0;
            if (_in.text != "" && _in.text != "-" && int.Parse(_in.text) >= 0)
            {
                _c = int.Parse(_in.text);
            }

            Color32 _b = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor;
            MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor = new Color32(_b.r, _b.g, (byte)_c, 255);
        }
        else if (_a == 8) // ADD REGION
        {
            Region _rg = new Region();
            _rg._name = "";
            _rg._regionColor = new Color32(255, 255, 255, 255);

            MapManager.Instance._map._regCats[_currentL3Int]._regions.Add(_rg);
        }
        else if (_a == 9) // REMOVE SPECIFIC REGION
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
        else if (_a == 10) // REMOVE REGION
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

            _subRegionInt = Mathf.Clamp(_subRegionInt, 0, MapManager.Instance._map._regCats[_currentL3Int]._regions.Count - 1);
        }
        else if (_a == 11) // CYCLE UP
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
        else if (_a == 12) // CYCLE DOWN
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
    }

    public void SETTINGS_MENU_FUNCTIONS(int _a)
    {
        if (_a == 0)
        {
            MapManager.Instance._map.yBoundaryMax++;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.yBoundaryMax += 4;
            }
            MapManager.Instance._map.yBoundaryMax = Mathf.Clamp(MapManager.Instance._map.yBoundaryMax, 0, 10000);

            GalaxyMap.Instance._regen2 = true;
        }
        else if (_a == 1)
        {
            MapManager.Instance._map.yBoundaryMax--;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.yBoundaryMax -= 4;
            }
            MapManager.Instance._map.yBoundaryMax = Mathf.Clamp(MapManager.Instance._map.yBoundaryMax, 0, 10000);

            GalaxyMap.Instance._regen2 = true;
        }
        else if (_a == 2)
        {
            MapManager.Instance._map.yBoundaryMin++;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.yBoundaryMin += 4;
            }
            MapManager.Instance._map.yBoundaryMin = Mathf.Clamp(MapManager.Instance._map.yBoundaryMin, -10000, 0);

            GalaxyMap.Instance._regen2 = true;

        }
        else if (_a == 3)
        {
            MapManager.Instance._map.yBoundaryMin--;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.yBoundaryMin -= 4;
            }
            MapManager.Instance._map.yBoundaryMin = Mathf.Clamp(MapManager.Instance._map.yBoundaryMin, -10000, 0);

            GalaxyMap.Instance._regen2 = true;
        }
        else if (_a == 4)
        {
            MapManager.Instance._map.xBoundaryMax++;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.xBoundaryMax += 4;
            }
            MapManager.Instance._map.xBoundaryMax = Mathf.Clamp(MapManager.Instance._map.xBoundaryMax, 0, 10000);

            GalaxyMap.Instance._regen2 = true;
        }
        else if (_a == 5)
        {
            MapManager.Instance._map.xBoundaryMax--;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.xBoundaryMax -= 4;
            }
            MapManager.Instance._map.xBoundaryMax = Mathf.Clamp(MapManager.Instance._map.xBoundaryMax, 0, 10000);

            GalaxyMap.Instance._regen2 = true;
        }
        else if (_a == 6)
        {
            MapManager.Instance._map.xBoundaryMin++;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.xBoundaryMin += 4;
            }
            MapManager.Instance._map.xBoundaryMin = Mathf.Clamp(MapManager.Instance._map.xBoundaryMin, -10000, 0);

            GalaxyMap.Instance._regen2 = true;
        }
        else if (_a == 7)
        {
            MapManager.Instance._map.xBoundaryMin--;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MapManager.Instance._map.xBoundaryMin -= 4;
            }
            MapManager.Instance._map.xBoundaryMin = Mathf.Clamp(MapManager.Instance._map.xBoundaryMin, -10000, 0);

            GalaxyMap.Instance._regen2 = true;
        }
        else if (_a == 8)
        {
            MapManager.Instance._map._fleetRevealSectors = !MapManager.Instance._map._fleetRevealSectors;
        }
    }

    public void CONTEXT(GameObject _obj)
    {
        for (int i = 0; i < _contextMenuObjects.Count; i++)
        {
            if (_obj == _contextMenuObjects[i])
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
                    EvaluateContextMenu(i - _val, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }
                else if (_currentContextInt == 3) // Alliances
                {
                    int _val = 0;
                    if (_currentL2Int == 2 && _contextSelection == false)
                    {
                        _val = 1;
                    }
                    EvaluateContextMenu(i - _val, _contextSelection);
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
                    EvaluateContextMenu(i - 1, _contextSelection);
                    _contextMenu.SetActive(_contextSelection);
                }

                break;

            }
        }
    }

    public void EXPORT_MENU_FUNCTIONS(int _a)
    {
        if (_a == 0) // NAME
        {
            InputField _in = _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>();

            XMLWriter.Instance._saveFileName = _in.text;
        }
        else if (_a == 1) // EXPORT
        {
            XMLWriter.Instance._export = true;
            XMLWriter.Instance._bulk = false;
            XMLWriter.Instance._exportFileName = _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text;
            XMLWriter.Instance._exportLock = _lockSel;
            XMLWriter.Instance._playerFaction = _playerFaction;
        }
        else if (_a == 2) // LOCK SELECTION
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
        else if (_a == 3) // PERSPECTIVE
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
        else if (_a == 4) // EXPORT BULK
        {
            XMLWriter.Instance._export = true;
            XMLWriter.Instance._bulk = true;
            XMLWriter.Instance._exportFileName = _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text;
            XMLWriter.Instance._exportLock = true;
            XMLWriter.Instance._playerFaction = _playerFaction;
        }
    }

    public void Render()
    {
        Camera.main.GetComponent<CameraRender>()._render = true;
    }

    public void SettingsMenu()
    {
        
        _settingsMenu.SetActive(!_settingsMenu.activeSelf);
        
    }

    public void CONTEXT_MENU_G_FUNCTIONS(int _a)
    {
        if (_a == 0) // TURN ON
        {
            _contextMenuG.SetActive(true);

            // - Place Context Menu -
            _contextMenuG.transform.localPosition = new Vector3((Input.mousePosition.x - Screen.width / 2), (Input.mousePosition.y - Screen.height / 2), 0);


            // - GET SECTOR POSITION -


            Vector2 _pos = MapManager.Instance.GetMouseSectorPos;

            _contextMenuGSector = MapManager.Instance.ReturnSector(_pos);
            _contextMenuGSPos = _pos;

            float _verticalHeight = 0;
            // - Title -
            _verticalHeight += 12;

            GameObject _titleObj = Instantiate(_contextMenuG_TextTemplate, _contextMenuG.transform);

            if (_contextMenuGSector == -1 || !MapManager.Instance.IsInDiscoveredList(_contextMenuGSector, true))
            {
                Text _t = _titleObj.GetComponent<Text>();
                _t.text = "EMPTY SPACE";
            }
            else
            {
                Text _t = _titleObj.GetComponent<Text>();
                _t.text = MapManager.Instance._map._sectors[_contextMenuGSector].GetName(true).ToUpper();
            }

            _titleObj.SetActive(true);
            _contextMenuG_objs.Add(_titleObj);

            // -- GM MENU SETTINGS --
            if (!MapManager.Instance._map._lockSelection)
            {
                // - HEADER -
                GameObject _headerObj = Instantiate(_contextMenuG_TextTemplate, _contextMenuG.transform);
                _headerObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t1 = _headerObj.GetComponent<Text>();
                _t1.text = "-- GM --";

                _headerObj.SetActive(true);
                _contextMenuG_objs.Add(_headerObj);

                if (_contextMenuGSector == -1)
                {
                    // - CREATE NEW SECTOR -
                    GameObject _cSObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _cSObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t2 = _cSObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t2.text = "Create (+)";

                    _cSObj.SetActive(true);
                    _contextMenuG_objs.Add(_cSObj);
                    _cSObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(1));
                }
                else
                {
                    // - EDIT SECTOR -
                    GameObject _eSObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _eSObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t2 = _eSObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t2.text = "Edit";

                    _eSObj.SetActive(true);
                    _contextMenuG_objs.Add(_eSObj);
                    _eSObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(3));

                    // - CHANGE OWNER -
                    GameObject _cOObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _cOObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t9 = _cOObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t9.text = "Change Owner";

                    _cOObj.SetActive(true);
                    _contextMenuG_objs.Add(_cOObj);
                    _cOObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(14));

                    // - MOVE SECTOR -
                    GameObject _mSObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _mSObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t8 = _mSObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t8.text = "Move";

                    _mSObj.SetActive(true);
                    _contextMenuG_objs.Add(_mSObj);
                    _mSObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(12));

                    // - REVEAL TO: -
                    GameObject _rTSObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _rTSObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t6 = _rTSObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t6.text = "Reveal to >";

                    _rTSObj.SetActive(true);
                    _contextMenuG_objs.Add(_rTSObj);
                    _rTSObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(10));

                    // - ADD CONNECTION -
                    GameObject _coSObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _coSObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t3 = _coSObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t3.text = "Add Connection";

                    _coSObj.SetActive(true);
                    _contextMenuG_objs.Add(_coSObj);
                    _coSObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(4));

                    if (MapManager.Instance.HasConnections(_contextMenuGSector))
                    {
                        // - EDIT CONNECTION -
                        /*
                        PURPOSE : Add button that serves as link to Connection object menu (similar to 'Edit sector')
                        FUNCTION: Builds button, links to function that'll serve for selecting connection & opening menu
                        */
                        GameObject _coECObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                        _coECObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                        _verticalHeight += 6;

                        Text _tEC = _coECObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        _tEC.text = "Edit Connection";

                        _coECObj.SetActive(true);
                        _contextMenuG_objs.Add(_coECObj);
                        _coECObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(16));

                        // - REMOVE CONNECTION -
                        GameObject _coRSObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                        _coRSObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                        _verticalHeight += 6;

                        Text _t4 = _coRSObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        _t4.text = "Remove Connection";

                        _coRSObj.SetActive(true);
                        _contextMenuG_objs.Add(_coRSObj);
                        _coRSObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(7));
                    }

                    // - DELETE -
                    GameObject _dSObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _dSObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t5 = _dSObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t5.text = "Delete";

                    _dSObj.SetActive(true);
                    _contextMenuG_objs.Add(_dSObj);
                    _dSObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(8));
                }
            }

            // - SYSTEM CONTEXT -
            if (_contextMenuGSector >= 0 && MapManager.Instance.IsInDiscoveredList(_contextMenuGSector, true))
            {
                // - HEADER -
                GameObject _headerObj = Instantiate(_contextMenuG_TextTemplate, _contextMenuG.transform);
                _headerObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t1 = _headerObj.GetComponent<Text>();
                _t1.text = "-- SYSTEM --";

                _headerObj.SetActive(true);
                _contextMenuG_objs.Add(_headerObj);

                // - SECTOR INFORMATION -
                GameObject _oSIObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                _oSIObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t2 = _oSIObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                _t2.text = "System Information";

                _oSIObj.SetActive(true);
                _contextMenuG_objs.Add(_oSIObj);
                _oSIObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(9));

                // - DISTANCE TO SECTOR -
                GameObject _DTObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                _DTObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t3 = _DTObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                _t3.text = "Measure Distance";

                _DTObj.SetActive(true);
                _contextMenuG_objs.Add(_DTObj);
                _DTObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(11));

                if (!MapManager.Instance._map._lockSelection)
                {
                    // - PAINT -
                    // - DISTANCE TO SECTOR -
                    GameObject _PObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
                    _PObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _tPaint = _PObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _tPaint.text = "Paint";

                    _PObj.SetActive(true);
                    _contextMenuG_objs.Add(_PObj);
                    _PObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(17));
                }
            }

            // - CLOSE -
            GameObject _cObj = Instantiate(_contextMenuG_ButtonTemplate, _contextMenuG.transform);
            _cObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
            _verticalHeight += 6;

            Text _t7 = _cObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
            _t7.text = "Close";

            _cObj.SetActive(true);
            _contextMenuG_objs.Add(_cObj);
            _cObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_G_FUNCTIONS(2));


            _contextMenuG.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
        }
        else if (_a == 1) // ADD SECTOR AT POSITION
        {
            MapManager.Instance.AddSector(_contextMenuGSPos);

            CONTEXT_MENU_G_FUNCTIONS(2);

        }
        else if (_a == 2) // CLOSE CONTEXT MENU
        {
            _contextMenuG.SetActive(false);

            for (int i = 0; i < _contextMenuG_objs.Count; i++)
            {
                GameObject _obj = _contextMenuG_objs[i];
                _contextMenuG_objs.Remove(_contextMenuG_objs[i]);
                Destroy(_obj);
                i--;
            }
        }
        else if (_a == 3) // EDIT SELECTED SECTOR
        {
            _menuObjects.SetActive(true);
            RebuildObjectMenuL1();
            OBJ2_FUNCTIONS(0, 0); // SECTOR
            _menuObjectsL3Sector.SetActive(true);
            _currentL3Int = _contextMenuGSector;

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 4) // ADD CONNECTION
        {
            _addConnectionMActive = true;
            _aCMMode = 0;
            _s1Int = _contextMenuGSector;
            _s2Int = -1;

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 5) // ADD CONNECTION EXEC
        {
            if (_s2Int == -1 || _s2Int == _s1Int)
            {
                return;
            }

            _addConnectionMActive = false;

            MapManager.Instance.AddConnection(_s1Int, _s2Int, new object[] { _aCMInt1 });
        }
        else if (_a == 6) // REMOVE CONNECTION EXEC
        {
            if (_s2Int == -1)
            {
                return;
            }

            _addConnectionMActive = false;

            MapManager.Instance.RemoveConnection(_s1Int, _s2Int);
        }
        else if (_a == 7) // REMOVE CONNECTION
        {
            if (MapManager.Instance.ConnectionCount(_contextMenuGSector) == 1)
            {
                _s1Int = _contextMenuGSector;
                int _s1I = MapManager.Instance._map._jumpGates[MapManager.Instance.GetConnectionID(_s1Int, -1)]._sector1Id;
                int _s2I = MapManager.Instance._map._jumpGates[MapManager.Instance.GetConnectionID(_s1Int, -1)]._sector2Id;

                int _sI = (_s1I == _s1Int) ? _s2I : _s1I;

                if (_sI >= 0 && _sI < MapManager.Instance._map._sectors.Count)
                {
                    _s2Int = _sI;
                }
                else
                {
                    return;
                }

                CONTEXT_MENU_G_FUNCTIONS(6);
            }
            else
            {
                _addConnectionMActive = true;
                _aCMMode = 1;
                _s1Int = _contextMenuGSector;
                _s2Int = -1;
            }

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 8) // SET UP DELETE CONFIRMATION - SECTOR
        {
            _delMenuActive = true;
            _delIntA = 0;
            _delIntB = _contextMenuGSector;

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 9) // OPEN INFORMATION PANEL
        {
            GalaxyMap.Instance._selectedInt = _contextMenuGSector;
            GalaxyMap.Instance._selectedType = "sector";

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 10) // REVEAL TO - PANEL INIT
        {
            _revToMenuActive = true;

            _revToSec = _contextMenuGSector;
            _typeMode = 0;

            REVEAL_TO_MENU_FUNCTIONS(1);

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 11) // DISTANCE TO TOOL INIT
        {
            _dActive = true;
            _dMeasurementActive = true;

            _toolS1Int = _contextMenuGSector;

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 12) // MOVE SECTOR INIT
        {
            _mSSInt = _contextMenuGSector;
            _mSectorActive = true;

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 13) // MOVE SECTOR
        {
            Vector2 _b = MapManager.Instance.GetMouseSectorPos;
            MapManager.Instance._map._sectors[_mSSInt]._posXInt = (int)_b.x;
            MapManager.Instance._map._sectors[_mSSInt]._posYInt = (int)_b.y;

            GalaxyMap.Instance._regen = true;

            _mSSInt = -1;
            _mSectorActive = false;
        }
        else if (_a == 14) // CHANGE OWNER - PANEL INIT
        {
            _changeOwnerMenuActive = true;

            _changeOwnerSec = _contextMenuGSector;

            CHANGE_OWNER_MENU_FUNCTIONS(1);

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 15) // EDIT CONNECTION EXEC
        {
            if (_s2Int == -1)
            {
                return;
            }

            _addConnectionMActive = false;

            _menuObjects.SetActive(true);
            RebuildObjectMenuL1();
            OBJ2_FUNCTIONS(0, 1); // CONNECTIONS
            _menuObjectsL3Jumpgate.SetActive(true);
            _currentL3Int = MapManager.Instance.GetConnectionID(_s1Int, _s2Int);

        }
        else if (_a == 16) // EDIT CONNECTION
        {

            if (MapManager.Instance.ConnectionCount(_contextMenuGSector) == 1)
            {
                _s1Int = _contextMenuGSector;
                int _s1I = MapManager.Instance._map._jumpGates[MapManager.Instance.GetConnectionID(_s1Int, -1)]._sector1Id;
                int _s2I = MapManager.Instance._map._jumpGates[MapManager.Instance.GetConnectionID(_s1Int, -1)]._sector2Id;

                int _sI = (_s1I == _s1Int) ? _s2I : _s1I;

                if (_sI >= 0 && _sI < MapManager.Instance._map._sectors.Count)
                {
                    _s2Int = _sI;
                }
                else
                {
                    return;
                }

                CONTEXT_MENU_G_FUNCTIONS(15);
            }
            else
            {
                _addConnectionMActive = true;
                _aCMMode = 2;
                _s1Int = _contextMenuGSector;
                _s2Int = -1;
            }

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
        else if (_a == 17) // PAINT TOOL INIT
        {
            PAINT_TOOL_G_FUNCTIONS(0);

            CONTEXT_MENU_G_FUNCTIONS(2);
        }
    }

    public void PAINT_TOOL_G_FUNCTIONS(int _a)
    {
        if (_a == 0) // TOGGLE
        {
            _paintToolActive = !_paintToolActive;
            if (_paintToolActive)
            {
                PAINT_TOOL_G_FUNCTIONS(1);
            }
            else
            {
                PAINT_TOOL_G_FUNCTIONS(6);
            }
        }
        else if (_a == 1) // INITIALIZE
        {
            _paintToolMenu.SetActive(true);

            _paintToolMode = 0; // DEFAULT TO CONNECTION TYPE TOOL WHEN INITIALIZING
            _paintToolType = 0;
            _paintToolSubType = -1;
        }
        else if (_a == 2) // CYCLE MODE
        {
            if ((_paintToolMode != 1 && _paintToolMode != 2) || (_paintToolMode == 1 && _paintToolType >= MapManager.Instance._map._regCats.Count - 1) || (_paintToolMode == 2 && _paintToolType >= MapManager.Instance._map._factions.Count - 1))
            {
                _paintToolMode++;

                if (_paintToolMode == 1 && MapManager.Instance._map._regCats.Count == 0)
                {
                    _paintToolMode++;
                }

                if (_paintToolMode == 2 && (MapManager.Instance._map._factions.Count == 0 || MapManager.Instance._map._playerFactionId != -1))
                {
                    _paintToolMode++;
                }

                if (_paintToolMode >= _paintToolModes)
                {
                    _paintToolMode = 0;
                }

                _paintToolType = 0;
                _paintToolSubType = -1;
            }
            else
            {
                _paintToolType++;

                _paintToolSubType = -1;
            }

        }
        else if (_a == 3) // CYCLE TYPE UP
        {
            if (_paintToolMode == 0) // CONNECTION TYPE
            {
                _paintToolSubType++;
                if (_paintToolSubType >= MapManager.Instance._map._connType.Count)
                {
                    _paintToolSubType = -1;
                }
            }
            else if (_paintToolMode == 1) // REGION
            {
                _paintToolSubType++;
                if (_paintToolSubType >= MapManager.Instance._map._regCats[_paintToolType]._regions.Count)
                {
                    _paintToolSubType = -1;
                }
            }
            else if (_paintToolMode == 2) // SECTOR VIS. (GM)
            {
                _paintToolSubType++;

                if (_paintToolSubType > 2)
                {
                    _paintToolSubType = 0;
                }
            }
        }
        else if (_a == 4) // PAINT
        {
            if (_paintToolS1 == -1) // CANNOT DO ANYTHING WITHOUT SECTOR 1
            {
                return;
            }

            if (_paintToolMode == 0 && (_paintToolS2 != -1 || MapManager.Instance.ConnectionCount(_paintToolS1) <= 1)) // CONNECTION TYPE
            {
                int _id = MapManager.Instance.GetConnectionID(_paintToolS1, _paintToolS2);

                if (_id > -1 && _id < MapManager.Instance._map._jumpGates.Count)
                {
                    MapManager.Instance._map._jumpGates[_id]._typeId = _paintToolSubType;
                    GalaxyMap.Instance._regen = true;
                }

                _paintToolS1 = -1;
                _paintToolS2 = -1;

                return;
            }
            else if (_paintToolMode == 1)
            {
                
                if (_paintToolS1 == -1 || _paintToolS1 >= MapManager.Instance._map._sectors.Count || _paintToolType == -1 || _paintToolType >= MapManager.Instance._map._regCats.Count)
                {
                    _paintToolS1 = -1;
                    return; // CONDITIONAL CASES TO QUIT
                }

                int val = (_paintToolSubType < MapManager.Instance._map._regCats[_paintToolType]._regions.Count) ? _paintToolSubType : -1;

                // CHECK IF REGION CAT EXISTS IN SECTOR; CHANGE VALUE
                bool _flag1 = false;

                for (int i = 0; i < MapManager.Instance._map._sectors[_paintToolS1]._regionCats.Count; i++)
                {
                    if (MapManager.Instance._map._sectors[_paintToolS1]._regionCats[i] == _paintToolType)
                    {
                        _flag1 = true;
                        MapManager.Instance._map._sectors[_paintToolS1]._regionCatsRegionIds[i] = val;
                        _paintToolS1 = -1;
                        _paintToolS2 = -1;
                        return;
                    }
                }
                if (!_flag1) // ADD REGION CATEGORY
                {
                    MapManager.Instance._map._sectors[_paintToolS1]._regionCats.Add(_paintToolType);
                    MapManager.Instance._map._sectors[_paintToolS1]._regionCatsRegionIds.Add(val);

                    _paintToolS1 = -1;
                    _paintToolS2 = -1;
                }

                return;
            }
            else if (_paintToolMode == 2)
            {
                if (_paintToolS1 == -1 || _paintToolS1 >= MapManager.Instance._map._sectors.Count || _paintToolType <= -1 || _paintToolType >= MapManager.Instance._map._factions.Count) // QUIT IF : NO Sector, TYPE > faction count, NO valid faction
                {
                    _paintToolS1 = -1;
                    return; // CONDITIONAL CASES TO QUIT
                }

                int val = Mathf.Clamp(_paintToolSubType, 0, 2); // COVERS ALL THREE MODES - 0: DISCOVERED, 1: EXPLORED, 2: KNOWN SECTOR
                
                if (val == 0) // DISCOVER
                {
                    if (MapManager.Instance._map._factions[_paintToolType].SectorDiscovered(_paintToolS1))
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_paintToolType]._discoveredSectors.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_paintToolType]._discoveredSectors[i] == _paintToolS1)
                            {
                                MapManager.Instance._map._factions[_paintToolType]._discoveredSectors.Remove(MapManager.Instance._map._factions[_paintToolType]._discoveredSectors[i]);
                                i--;
                            }
                                
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_paintToolType]._discoveredSectors.Add(_paintToolS1);
                    }
                }
                else if (val == 1) // EXPLORED
                {
                    if (MapManager.Instance._map._factions[_paintToolType].SectorExplored(_paintToolS1))
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_paintToolType]._exploredSectors.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_paintToolType]._exploredSectors[i] == _paintToolS1)
                            {
                                MapManager.Instance._map._factions[_paintToolType]._exploredSectors.Remove(MapManager.Instance._map._factions[_paintToolType]._exploredSectors[i]);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_paintToolType]._exploredSectors.Add(_paintToolS1);
                    }
                }
                else if (val == 2) // KNOWN SECTOR OWNER
                {
                    if (MapManager.Instance._map._factions[_paintToolType].SectorKnownOwner(_paintToolS1))
                    {
                        for (int i = 0; i < MapManager.Instance._map._factions[_paintToolType]._knownSectorOwnership.Count; i++)
                        {
                            if (MapManager.Instance._map._factions[_paintToolType]._knownSectorOwnership[i] == _paintToolS1)
                            {
                                MapManager.Instance._map._factions[_paintToolType]._knownSectorOwnership.Remove(MapManager.Instance._map._factions[_paintToolType]._knownSectorOwnership[i]);
                                i--;
                            }
                        }
                    }
                    else
                    {
                        MapManager.Instance._map._factions[_paintToolType]._knownSectorOwnership.Add(_paintToolS1);
                    }
                }

                GalaxyMap.Instance._regen = true;

                _paintToolS1 = -1;
                _paintToolS2 = -1;
                return;
            }
            return;
        }
        else if (_a == 5) // CLICK HANDLER
        {
            if (_paintToolS1 == -1) // SET SECTOR 1
            {
                _paintToolS1 = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);

                if (_paintToolMode == 0 && MapManager.Instance.ConnectionCount(_paintToolS1) <= 1)
                {
                    PAINT_TOOL_G_FUNCTIONS(4);
                }
                else if (_paintToolMode == 1 || _paintToolMode == 2)
                {
                    PAINT_TOOL_G_FUNCTIONS(4);
                }
            }
            else if (_paintToolS2 == -1) // SET SECTOR 2
            {
                _paintToolS2 = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);

                PAINT_TOOL_G_FUNCTIONS(4);
            }
        }
        else if (_a == 6) // EXIT
        {
            _paintToolMenu.SetActive(false);
        }
        else if (_a == 7) // GENERAL UI MANAGEMENT
        {
            if (_paintToolMode == 0) // CONNECTION TYPE
            {
                if (_paintToolSubType >= MapManager.Instance._map._connType.Count)
                {
                    _paintToolSubType = -1;
                }

                Text[] _tObj = { _paintToolMenu.GetComponent<IndexScript>()._obj3.GetComponent<Text>(), _paintToolMenu.GetComponent<IndexScript>()._obj5.GetComponent<Text>(), _paintToolMenu.GetComponent<IndexScript>()._obj8.GetComponent<Text>() };

                _paintToolMenu.GetComponent<IndexScript>()._obj6.SetActive(true);
                _paintToolMenu.GetComponent<IndexScript>()._obj7.SetActive(true);

                _tObj[0].text = "(TAB) MODE: CONNECTION TYPE";
                _tObj[1].text = "(< / >) TYPE: " + ((_paintToolSubType > -1) ? MapManager.Instance._map._connType[_paintToolSubType]._name.ToUpper() : "REGULAR");

                _paintToolS1 = Mathf.Clamp(_paintToolS1, -1, MapManager.Instance._map._sectors.Count - 1);
                _paintToolS2 = Mathf.Clamp(_paintToolS2, -1, MapManager.Instance._map._sectors.Count - 1);

                _tObj[2].text = "S1: " + ((_paintToolS1 > -1) ? MapManager.Instance._map._sectors[_paintToolS1].GetName(true) : "-");
            }
            else if (_paintToolMode == 1) // REGION
            {
                if (_paintToolType >= MapManager.Instance._map._regCats.Count)
                {
                    PAINT_TOOL_G_FUNCTIONS(0);
                    return;
                }

                if (_paintToolSubType >= MapManager.Instance._map._regCats[_paintToolType]._regions.Count)
                {
                    _paintToolSubType = -1;
                }

                GalaxyMap.Instance._viewMode = "regions";
                GalaxyMap.Instance._selFacInt = _paintToolType;

                Text[] _tObj = { _paintToolMenu.GetComponent<IndexScript>()._obj3.GetComponent<Text>(), _paintToolMenu.GetComponent<IndexScript>()._obj5.GetComponent<Text>() };

                _paintToolMenu.GetComponent<IndexScript>()._obj6.SetActive(false);
                _paintToolMenu.GetComponent<IndexScript>()._obj7.SetActive(false);

                _tObj[0].text = "(TAB) MODE: REGION (" + MapManager.Instance._map._regCats[_paintToolType]._name.ToUpper() + ")";
                _tObj[1].text = "(< / >) TYPE: " + ((_paintToolSubType > -1) ? MapManager.Instance._map._regCats[_paintToolType]._regions[_paintToolSubType]._name : "No Data");

                _paintToolS1 = Mathf.Clamp(_paintToolS1, -1, MapManager.Instance._map._sectors.Count - 1);
            }
            else if (_paintToolMode == 2) // SECTOR VISIBILITY SETTINGS
            {
                if (_paintToolType >= MapManager.Instance._map._factions.Count)
                {
                    PAINT_TOOL_G_FUNCTIONS(0);
                    return; // CLOSE IF TYPE > FACTION COUNT
                }

                
                _paintToolSubType = Mathf.Clamp(_paintToolSubType, 0, 2);

                GalaxyMap.Instance._viewMode = "special_SectorVisibility"; // TOGGLE MODE THAT DISPLAYS ALL SECTORS MAKING IT EASY TO CHECK WHETHER OR NOT THEY SEE IT
                GalaxyMap.Instance._selFacInt = _paintToolType;

                Text[] _tObj = { _paintToolMenu.GetComponent<IndexScript>()._obj3.GetComponent<Text>(), _paintToolMenu.GetComponent<IndexScript>()._obj5.GetComponent<Text>() };

                _paintToolMenu.GetComponent<IndexScript>()._obj6.SetActive(false);
                _paintToolMenu.GetComponent<IndexScript>()._obj7.SetActive(false);

                string[] _t1Text = {"Discovered", "Explored", "Knows Sector Owner"};

                _tObj[0].text = "(TAB) MODE: FACTION SECTOR VIS. (" + MapManager.Instance._map._factions[_paintToolType]._shorthand.ToUpper() + ")";
                _tObj[1].text = "(< / >) TYPE: " + (_t1Text[_paintToolSubType]);

                _paintToolS1 = Mathf.Clamp(_paintToolS1, -1, MapManager.Instance._map._sectors.Count - 1);
            }
        }
        else if (_a == 8) // CYCLE TYPE DOWN
        {
            if (_paintToolMode == 0) // CONNECTION TYPE
            {
                _paintToolSubType--;
                if (_paintToolSubType < -1)
                {
                    _paintToolSubType = MapManager.Instance._map._connType.Count - 1;
                }
            }
            else if (_paintToolMode == 1) // REGION
            {
                _paintToolSubType--;
                if (_paintToolSubType < -1)
                {
                    _paintToolSubType = MapManager.Instance._map._regCats[_paintToolType]._regions.Count - 1;
                }
            }
            else if (_paintToolMode == 2) // SECTOR VISIBILITY
            {
                _paintToolSubType--;

                if (_paintToolSubType < 0)
                {
                    _paintToolSubType = 2;
                }
            }
        }
        else if (_a == 9) // CYCLE MODE DOWN
        {
            if ((_paintToolMode != 1 && _paintToolMode != 2) || (_paintToolMode == 1 && _paintToolType == 0) || (_paintToolMode == 2 && _paintToolType == 0))
            {
                _paintToolMode--;

                

                

                if (_paintToolMode == 2 && (MapManager.Instance._map._factions.Count == 0 || MapManager.Instance._map._playerFactionId != -1))
                {
                    _paintToolMode--;
                }

                if (_paintToolMode == 1 && MapManager.Instance._map._regCats.Count == 0)
                {
                    _paintToolMode--;
                }

                if (_paintToolMode < 0)
                {
                    _paintToolMode = _paintToolModes - 1;
                }

                if (_paintToolMode == 1) // set _paintToolType to Max Region Count
                {
                    _paintToolType = MapManager.Instance._map._regCats.Count - 1;
                }
                else if (_paintToolMode == 2) // Set _paintToolType to Last Faction
                {
                    _paintToolType = MapManager.Instance._map._factions.Count - 1;
                }
                else
                {
                    _paintToolType = 0;
                }
                
                _paintToolSubType = -1;
            }
            else
            {
                _paintToolType--;

                _paintToolSubType = -1;
            }
        }
    }

    public void DELETE_CONF_MENU_FUNCTIONS(int _a)
    {
        if (_a == 0) // Delete
        {


            _delMenuActive = false;

            if (_delIntB == -1)
            {
                _delIntA = -1;
                return;
            }

            MapManager.Instance.RemoveObject(_delIntA, _delIntB);

            _delIntA = -1;
            _delIntB = -1;

            return;
        }
        else if (_a == 1) // EXIT
        {
            _delMenuActive = false;

            _delIntA = -1;
            _delIntB = -1;
        }
    }

    public void REVEAL_TO_MENU_FUNCTIONS(int _a)
    {
        if (_a == 0)
        {
            _revToMenuActive = false;

            for (int i = 0; i < _revToMenu_objs.Count; i++)
            {
                GameObject _obj = _revToMenu_objs[i];
                _revToMenu_objs.Remove(_revToMenu_objs[i]);
                Destroy(_obj);
                i--;
            }
        }
        else if (_a == 1) // BUILD MENU
        {
            REVEAL_TO_MENU_FUNCTIONS(0);

            _revToMenuActive = true;

            _revToMenu.transform.localPosition = new Vector3((Input.mousePosition.x - Screen.width / 2), (Input.mousePosition.y - Screen.height / 2), 0);
            float _verticalHeight = 6;

            // TYPE BUTTON

            _revToMenu_Button2.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 3f, 0);
            _verticalHeight += 6;

            // FACTIONS
            for (int i = 0; i < MapManager.Instance._map._factions.Count; i++)
            {

                GameObject _fObj = Instantiate(_revToMenu_ButtonTemplate, _revToMenu.transform);

                _fObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._factions[i]._shorthand;
                _fObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 3f, 0);

                _verticalHeight += 6;

                _fObj.SetActive(true);
                _revToMenu_objs.Add(_fObj);
            }


            _revToMenu.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);

            REVEAL_TO_MENU_FUNCTIONS(2);
        }
        else if (_a == 2) // UPDATE LIST
        {
            _rTMSetup = false;
            for (int i = 0; i < _revToMenu_objs.Count; i++)
            {
                _revToMenu_objs[i].GetComponent<IndexScript>()._obj2.GetComponent<Toggle>().isOn = false;

                if(_typeMode == 0)
                {
                    _revToMenu_Button2.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Discovered";

                    if (MapManager.Instance._map._factions[i].SectorDiscovered(_revToSec))
                    {
                        _revToMenu_objs[i].GetComponent<IndexScript>()._obj2.GetComponent<Toggle>().isOn = true;
                    }
                }
                else if (_typeMode == 1)
                {
                    _revToMenu_Button2.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Explored";

                    if (MapManager.Instance._map._factions[i].SectorExplored(_revToSec))
                    {
                        _revToMenu_objs[i].GetComponent<IndexScript>()._obj2.GetComponent<Toggle>().isOn = true;
                    }
                }
                else if (_typeMode == 2)
                {
                    _revToMenu_Button2.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Knows Sector Owner";
                    
                    if (MapManager.Instance._map._factions[i].SectorKnownOwner(_revToSec))
                    {
                        _revToMenu_objs[i].GetComponent<IndexScript>()._obj2.GetComponent<Toggle>().isOn = true;
                    }
                }

            }
            

            _rTMSetup = true;
        }
        else if (_a == 3) // SWITCH TYPE UP
        {
            _typeMode++;
            if (_typeMode > 2)
            {
                _typeMode = 0;
            }

            REVEAL_TO_MENU_FUNCTIONS(2);
        }
        else if (_a == 4) // SWITCH TYPE DOWN
        {
            _typeMode--;
            if (_typeMode < 0)
            {
                _typeMode = 2;
            }

            REVEAL_TO_MENU_FUNCTIONS(2);
        }
    }

    public void REVEAL_TO_MENU_SWITCH(GameObject _obj)
    {
        if (!_rTMSetup)
        {
            return;
        }

        // Find Object in list
        int _index = -1;
        for (int i = 0; i < _revToMenu_objs.Count; i++)
        {
            if (_revToMenu_objs[i] == _obj)
            {
                _index = i;
            }
        }

        if (_index < 0) // RETURN IF NO MATCH FOUND
        {
            Debug.Log("A");
            return;
        }
        Debug.Log(_index);

        _index = Mathf.Clamp(_index, 0, MapManager.Instance._map._factions.Count - 1);

        bool state = _revToMenu_objs[_index].GetComponent<IndexScript>()._obj2.GetComponent<Toggle>().isOn;

        if (state)
        {
            if (_typeMode == 0)
            {
                for (int i = 0; i < MapManager.Instance._map._factions[_index]._discoveredSectors.Count; i++)
                {
                    if (MapManager.Instance._map._factions[_index]._discoveredSectors[i] == _revToSec)
                    {
                        return; // ALREADY PRESENT
                    }
                }

                MapManager.Instance._map._factions[_index]._discoveredSectors.Add(_revToSec); // ADD TO LIST
            }
            else if (_typeMode == 1)
            {
                for (int i = 0; i < MapManager.Instance._map._factions[_index]._exploredSectors.Count; i++)
                {
                    if (MapManager.Instance._map._factions[_index]._exploredSectors[i] == _revToSec)
                    {
                        return; // ALREADY PRESENT
                    }
                }

                MapManager.Instance._map._factions[_index]._exploredSectors.Add(_revToSec); // ADD TO LIST
            }
            else if (_typeMode == 2)
            {
                for (int i = 0; i < MapManager.Instance._map._factions[_index]._knownSectorOwnership.Count; i++)
                {
                    if (MapManager.Instance._map._factions[_index]._knownSectorOwnership[i] == _revToSec)
                    {
                        return; // ALREADY PRESENT
                    }

                }

                MapManager.Instance._map._factions[_index]._knownSectorOwnership.Add(_revToSec); // ADD TO LIST
            }

        }
        else
        {
            if (_typeMode == 0)
            {
                for (int i = 0; i < MapManager.Instance._map._factions[_index]._discoveredSectors.Count; i++)
                {
                    if (MapManager.Instance._map._factions[_index]._discoveredSectors[i] == _revToSec)
                    {
                        MapManager.Instance._map._factions[_index]._discoveredSectors.Remove(MapManager.Instance._map._factions[_index]._discoveredSectors[i]);
                        i--;

                        for (int j = 0; j < MapManager.Instance._map._factions[_index]._exploredSectors.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[_index]._exploredSectors[j] == _revToSec)
                            {
                                MapManager.Instance._map._factions[_index]._exploredSectors.Remove(MapManager.Instance._map._factions[_index]._exploredSectors[j]);
                                j--;
                            }
                        }

                        for (int j = 0; j < MapManager.Instance._map._factions[_index]._knownSectorOwnership.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[_index]._knownSectorOwnership[j] == _revToSec)
                            {
                                MapManager.Instance._map._factions[_index]._knownSectorOwnership.Remove(MapManager.Instance._map._factions[_index]._knownSectorOwnership[j]);
                                j--;
                            }
                        }
                    }
                }
            }
            else if (_typeMode == 1)
            {
                for (int i = 0; i < MapManager.Instance._map._factions[_index]._exploredSectors.Count; i++)
                {
                    if (MapManager.Instance._map._factions[_index]._exploredSectors[i] == _revToSec)
                    {
                        MapManager.Instance._map._factions[_index]._exploredSectors.Remove(MapManager.Instance._map._factions[_index]._exploredSectors[i]);
                        i--;
                    }
                }
            }
            else if (_typeMode == 2)
            {
                for (int i = 0; i < MapManager.Instance._map._factions[_index]._knownSectorOwnership.Count; i++)
                {
                    if (MapManager.Instance._map._factions[_index]._knownSectorOwnership[i] == _revToSec)
                    {
                        MapManager.Instance._map._factions[_index]._knownSectorOwnership.Remove(MapManager.Instance._map._factions[_index]._knownSectorOwnership[i]);
                        i--;
                    }
                }
            }

        }

        REVEAL_TO_MENU_FUNCTIONS(2);
    }

    public void DISTANCE_M_MENU_FUNCTIONS(int _a)
    {
        if (_a == 0) // MEASUREMENT FUNCTION
        {
            if (_toolS1Int < 0)
            {
                _dActive = false;
                _dMeasurementActive = false;
                return;
            }

            // --- LINE ---

            // Set sector 1 & 2 positions

            float _s1X = 0;
            float _s1Y = 0;
            
            Sector _sector = MapManager.Instance._map._sectors[_toolS1Int];
            _s1X = _sector._posXInt;
            _s1Y = _sector._posYInt;
            
            // Calculate deltaX, deltaY and theta (using tan-1) for Sector 1 and 2

            float _deltaXs1 = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - (.75f * _s1X);
            float _deltaXs2 = (.75f * _s1X) - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

            float _sector1YPos = 0;
            float _sector2YPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            if ((_s1X * _s1X) % 2 == 1)
            {
                _sector1YPos = .9f * _s1Y + .45f;
            }
            else
            {
                _sector1YPos = .9f * _s1Y;
            }

            float _deltaYs1 = _sector2YPos - _sector1YPos;
            float _deltaYs2 = _sector1YPos - _sector2YPos;

            float theta1 = Mathf.Atan2(_deltaYs1, _deltaXs1);
            float theta2 = Mathf.Atan2(_deltaYs2, _deltaXs2);

            Vector3 _p1 = new Vector3(_s1X * .75f + Mathf.Cos(theta1) * .05f, _sector1YPos + Mathf.Sin(theta1) * .05f, -5);
            Vector3 _p2 = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, _sector2YPos, -5);

            _dMToolObj.GetComponent<LineRenderer>().SetPosition(0, _p1);
            _dMToolObj.GetComponent<LineRenderer>().SetPosition(1, _p2);
            _dMToolObj.GetComponent<IndexScript>()._obj1.transform.position = _p1;
            _dMToolObj.GetComponent<IndexScript>()._obj2.transform.position = _p2;

            // --- NOTICE ---

            Vector2 _sPos = MapManager.Instance.GetMouseSectorPos;

            int _dist = MapManager.Instance.SectorDistance(new Vector2(_s1X, _s1Y), _sPos);

            Vector2 pC1 = new Vector2(_p1.x, _p1.y);
            Vector2 pC2 = new Vector2(_p2.x, _p2.y);

            _dMDistanceNotice.transform.position = pC1 + ((pC2 - pC1) / 2);
            _dMDistanceNotice.transform.position += new Vector3(0, 0, 5);
            _dMDistanceNotice.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = (_dist > 1 || _dist < 1) ? _dist + " Hexes" : _dist + " Hex";

        }
        else if (_a == 1) // STATIC PLACEMENT
        {
            if (_toolS1Int < 0 || _toolS2Int < 0)
            {
                _dActive = false;
                return;
            }

            // Set sector 1 & 2 positions

            float _s1X = 0;
            float _s1Y = 0;

            float _s2X = 0;
            float _s2Y = 0;

            Sector _sector = MapManager.Instance._map._sectors[_toolS1Int];
            _s1X = _sector._posXInt;
            _s1Y = _sector._posYInt;

            Sector _sector2 = MapManager.Instance._map._sectors[_toolS2Int];
            _s2X = _sector2._posXInt;
            _s2Y = _sector2._posYInt;

            // Calculate deltaX, deltaY and theta (using tan-1) for Sector 1 and 2

            float _deltaXs1 = (.75f * _s2X) - (.75f * _s1X);
            float _deltaXs2 = (.75f * _s1X) - (.75f * _s2X);

            float _sector1YPos = 0;
            float _sector2YPos = 0;

            if ((_s1X * _s1X) % 2 == 1)
            {
                _sector1YPos = .9f * _s1Y + .45f;
            }
            else
            {
                _sector1YPos = .9f * _s1Y;
            }


            if ((_s2X * _s2X) % 2 == 1)
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

            Vector3 _p1 = new Vector3(_s1X * .75f + Mathf.Cos(theta1) * .05f, _sector1YPos + Mathf.Sin(theta1) * .05f, -5);
            Vector3 _p2 = new Vector3(_s2X * .75f + Mathf.Cos(theta2) * .05f, _sector2YPos + Mathf.Sin(theta2) * .05f, -5);

            _dMToolObj.GetComponent<LineRenderer>().SetPosition(0, _p1);
            _dMToolObj.GetComponent<LineRenderer>().SetPosition(1, _p2);
            _dMToolObj.GetComponent<IndexScript>()._obj1.transform.position = _p1;
            _dMToolObj.GetComponent<IndexScript>()._obj2.transform.position = _p2;

            // --- NOTICE ---

            int _dist = MapManager.Instance.SectorDistance(_toolS1Int, _toolS2Int);

            Vector2 pC1 = new Vector2(_p1.x, _p1.y);
            Vector2 pC2 = new Vector2(_p2.x, _p2.y);

            _dMDistanceNotice.transform.position = pC1 + ((pC2 - pC1) / 2);
            _dMDistanceNotice.transform.position += new Vector3(0, 0, 5);
            _dMDistanceNotice.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = (_dist != 1) ? _dist + " Hexes" : _dist + " Hex";
        }
    }

    public void CHANGE_OWNER_MENU_FUNCTIONS(int _a)
    {
        if (_a == 0) // CLOSE
        {
            _changeOwnerMenuActive = false;

            for (int i = 0; i < _changeOwnerMenu_objs.Count; i++)
            {
                GameObject _obj = _changeOwnerMenu_objs[i];
                _changeOwnerMenu_objs.Remove(_changeOwnerMenu_objs[i]);
                Destroy(_obj);
                i--;
            }
        }
        else if (_a == 1) // BUILD MENU
        {
            CHANGE_OWNER_MENU_FUNCTIONS(0);

            _changeOwnerMenuActive = true;

            _changeOwnerMenu.transform.localPosition = new Vector3((Input.mousePosition.x - Screen.width / 2), (Input.mousePosition.y - Screen.height / 2), 0);
            float _verticalHeight = 6;
            // FACTIONS
            for (int i = 0; i < MapManager.Instance._map._factions.Count; i++)
            {

                GameObject _fObj = Instantiate(_changeOwnerMenu_ButtonTemplate, _changeOwnerMenu.transform);

                _fObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._factions[i]._shorthand;
                _fObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 3f, 0);

                _verticalHeight += 6;

                _fObj.SetActive(true);
                _changeOwnerMenu_objs.Add(_fObj);
            }


            _changeOwnerMenu.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
        }
    }

    public void CHANGE_OWNER_MENU_PRESS(GameObject _obj)
    {
        int a = -1;

        for (int i = 0; i < _changeOwnerMenu_objs.Count; i++)
        {
            if (_changeOwnerMenu_objs[i] == _obj)
            {
                a = i;
                break;
            }
        }

        if (a < 0)
        {
            return;
        }

        MapManager.Instance._map._sectors[_changeOwnerSec]._controlFaction = a;

        CHANGE_OWNER_MENU_FUNCTIONS(0);
    }

    // Update is called once per frame
    void Update()
    {

        if (!MapManager.Instance._galaxy)
        {
            return;
        }
        
        if (MapManager.Instance._map._lockSelection)
        {
            _menuMain.SetActive(false);
        }
        else
        {
            _menuMain.SetActive(true);
        }

        if (_contextMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            _contextMenu.SetActive(false);
            MapManager.Instance._escapeMenuIncompatTriggered = true;
        }
        else if (_mConnType.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            _mConnType.SetActive(false);
            OBJ3_CONNECTION_FUNCTIONS(9);

            MapManager.Instance._escapeMenuIncompatTriggered = true;
        }
        else if (_mConnType_Context.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            _mConnType_Context.SetActive(false);

            MapManager.Instance._escapeMenuIncompatTriggered = true;
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

            MapManager.Instance._escapeMenuIncompatTriggered = true;
        }
        else if (_menuObjectsL2.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            _currentL2Int = -1;
            _menuObjectsL2.SetActive(false);

            MapManager.Instance._escapeMenuIncompatTriggered = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menuObjects.SetActive(false);
        }


        if (_mSectorActive)
        {
            _mSMenu.SetActive(true);

            if (_mSSInt < 0)
            {
                _mSectorActive = false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                CONTEXT_MENU_G_FUNCTIONS(13);
            }
        }
        else
        {
            _mSMenu.SetActive(false);
        }

        if (_paintToolActive)
        {
            PAINT_TOOL_G_FUNCTIONS(7);

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || (_paintToolMode == 2 && MapManager.Instance._map._playerFactionId > -1))
            {
                PAINT_TOOL_G_FUNCTIONS(0);

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                PAINT_TOOL_G_FUNCTIONS(5);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    PAINT_TOOL_G_FUNCTIONS(9);
                }
                else
                {
                    PAINT_TOOL_G_FUNCTIONS(2);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                PAINT_TOOL_G_FUNCTIONS(3);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PAINT_TOOL_G_FUNCTIONS(8);
            }
        }

        if (_contextMenu.activeSelf)
        {
            if (_contextMenuObjects.Count < 13)
            {
                _scrollbarObjContext.gameObject.SetActive(false);
            }
            else
            {
                _scrollbarObjContext.gameObject.SetActive(true);
            }


            _mObjectsContextOffsetVal = _scrollbarObjContext.value;

            _mObjectsContextOffset = Mathf.RoundToInt(_mObjectsContextOffsetVal * (_contextMenuObjects.Count - 13));
            _mObjectsContextOffset = Mathf.Clamp(_mObjectsContextOffset, 0, 10000);
        }

        if (_mConnType_Context.activeSelf)
        {
            if (_mConnTypeContextObjs.Count < 13)
            {
                _scrollbarObjConnContext.gameObject.SetActive(false);
            }
            else
            {
                _scrollbarObjConnContext.gameObject.SetActive(true);
            }


            _mConnTypeContextOffsetVal = _scrollbarObjConnContext.value;

            _mConnTypeContextOffset[0] = Mathf.RoundToInt(_mConnTypeContextOffsetVal * (_mConnTypeContextObjs.Count - 13));
            _mConnTypeContextOffset[0] = Mathf.Clamp(_mConnTypeContextOffset[0], 0, 10000);

            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || !_menuObjectsL3Jumpgate.activeSelf) && !_mConnType.activeSelf)
            {
                _mConnType_Context.SetActive(false);

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }
        }

        if (_mConnColor.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || !_menuObjectsL3Jumpgate.activeInHierarchy)
            {
                OBJ3_CONNECTION_FUNCTIONS(14);

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }
        }

        if (_mConnType.activeSelf)
        {

            if ((Input.GetKeyDown(KeyCode.Escape) || !_menuObjectsL3Jumpgate.activeInHierarchy) && !_mConnColor.activeSelf)
            {
                _mConnType.SetActive(false);
                OBJ3_CONNECTION_FUNCTIONS(9);

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }
        }
        
        if (Input.GetMouseButtonDown(1) && MapManager.Instance.ContextMenuGCheck)
        {
            CONTEXT_MENU_G_FUNCTIONS(2);

            CONTEXT_MENU_G_FUNCTIONS(0);
        }
        else if ((Input.GetMouseButtonDown(1) && ((!MapManager.Instance.ContextMenuGCheck && _contextMenuG.activeInHierarchy) || _contextMenuG.activeInHierarchy)) || (Input.GetMouseButtonDown(0) && !MapManager.Instance.ContextMenuGCheck && _contextMenuG.activeInHierarchy) || (!MapManager.Instance.ContextMenuGCheck && _contextMenuG.activeInHierarchy))
        {


            CONTEXT_MENU_G_FUNCTIONS(2);
        }

        if (_addConnectionMActive && _s1Int == -1)
        {
            _addConnectionMActive = false;
        }

        if (_addConnectionMActive)
        {
            /* 
            PURPOSE : ALLOW ADDING, EDITING & REMOVING CONNECTIONS VIA CONTEXT MENU
            INPUTS : 
            -   Type (0 = Add, 1 = Remove, 2 = Edit)
            -   Sector 1 (Selected Sector)
            -   Sector 2 (Selected Sector if removing/editing, else -1)
            -   Connection Type (If adding, else irrelevant)

            METHOD :
                (1) Check if ESCAPE pressed (-> exit)
                (2) Display Menu & text based on "type"
                (3) Check if Left clicked (if yes, get mouse sector position & trigger function)
                (4) If adding, check if TAB pressed (if yes, cycle through connection types)
                (5) Modify Connection Type display based on selected Connection
            */
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _addConnectionMActive = false;

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }
            _acmMenu.SetActive(true);

            string[] t0 = { "Left Click to add new Connection", "Left Click to remove existing Connection", "Left Click to edit Connection to Sector" };
            _acmMenu.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = t0[_aCMMode];

            if (Input.GetMouseButtonDown(0))
            {
                _s2Int = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);
                int[] i1 = { 5, 6, 15 };
                CONTEXT_MENU_G_FUNCTIONS(i1[_aCMMode]);
            }
            
            _aCMInt1 = Mathf.Clamp(_aCMInt1, -1, MapManager.Instance._map._connType.Count - 1);

            if (_aCMMode == 0 && Input.GetKeyDown(KeyCode.Tab))
            {
                _aCMInt1++;
                if (_aCMInt1 >= MapManager.Instance._map._connType.Count)
                {
                    _aCMInt1 = -1;
                }
            }

            _acmMenu.GetComponent<IndexScript>()._obj2.SetActive((_aCMMode == 0) ? true : false);
            _acmMenu.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = (_aCMMode == 0) ? ((_aCMInt1 != -1 ) ? "(TAB) TYPE: " + MapManager.Instance._map._connType[_aCMInt1]._name.ToUpper() : "(TAB) TYPE: REGULAR") : "";
        }
        else
        {
            _acmMenu.SetActive(false);
        }

        if (_delMenuActive)
        {
            _delMenu.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DELETE_CONF_MENU_FUNCTIONS(1);

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }
        }
        else
        {
            _delMenu.SetActive(false);
        }

        if (_dActive)
        {
            _dMDistanceNotice.SetActive(true);
            
            _dMToolObj.SetActive(true);

            if (_dMeasurementActive)
            {
                _dMMenu.SetActive(true);
                _dMMenu.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Left Click to measure distance to that sector";
                DISTANCE_M_MENU_FUNCTIONS(0);

                if (Input.GetMouseButtonDown(1))
                {
                    _dMeasurementActive = false;
                    _dActive = false;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    _toolS2Int = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);
                    _dMeasurementActive = false;
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _dMeasurementActive = false;
                    _dActive = false;

                    MapManager.Instance._escapeMenuIncompatTriggered = true;
                }
            }
            else
            {
                _dMMenu.SetActive(false);

                DISTANCE_M_MENU_FUNCTIONS(1);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _dActive = false;

                    MapManager.Instance._escapeMenuIncompatTriggered = true;
                }
                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
                {
                    _dActive = false;
                }
            }
        }
        else
        {
            _dMDistanceNotice.SetActive(false);
            _dMMenu.SetActive(false);
            _dMToolObj.SetActive(false);
        }

        if (_revToMenuActive)
        {
            _revToMenu.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Escape) || _contextMenuG.activeInHierarchy)
            {
                REVEAL_TO_MENU_FUNCTIONS(0);

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                REVEAL_TO_MENU_FUNCTIONS(3);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                REVEAL_TO_MENU_FUNCTIONS(4);
            }
        }
        else
        {
            _revToMenu.SetActive(false);
        }


        if (_changeOwnerMenuActive)
        {
            _changeOwnerMenu.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Escape) || _contextMenuG.activeInHierarchy)
            {
                CHANGE_OWNER_MENU_FUNCTIONS(0);

                MapManager.Instance._escapeMenuIncompatTriggered = true;
            }
        }
        else
        {
            _changeOwnerMenu.SetActive(false);
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

            _playerFaction = Mathf.Clamp(_playerFaction, -1, MapManager.Instance._map._playerFactions.Count);

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

            

            if (!_menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().isFocused)
            {
                _menuExport.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text = XMLWriter.Instance._saveFileName;
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
        
        if (_menuObjectsL2.activeSelf)
        {

            if (_menuObject2Objects.Count < 13)
            {
                _scrollbarObj2.gameObject.SetActive(false);
            }
            else
            {
                _scrollbarObj2.gameObject.SetActive(true);
            }


            _mObjectsm2OffsetVal = _scrollbarObj2.value;

            _mObjectsm2Offset = Mathf.RoundToInt(_mObjectsm2OffsetVal * (_menuObject2Objects.Count - 13));
            _mObjectsm2Offset = Mathf.Clamp(_mObjectsm2Offset, 0, 10000);

            

            if (_currentL2Int == 0)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._sectors.Count)
                {
                    OBJ2_FUNCTIONS(0, 0);
                }
            }
            else if (_currentL2Int == 1)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._jumpGates.Count)
                {
                    OBJ2_FUNCTIONS(0, 1);
                }
            }
            else if (_currentL2Int == 2)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._factions.Count)
                {
                    OBJ2_FUNCTIONS(0, 2);
                }
            }
            else if (_currentL2Int == 3)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._alliances.Count)
                {
                    OBJ2_FUNCTIONS(0, 3);
                }
            }
            else if (_currentL2Int == 4)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._playerFactions.Count)
                {
                    OBJ2_FUNCTIONS(0, 4);
                }
            }
            else if (_currentL2Int == 5)
            {
                if (_menuObject2Objects.Count != MapManager.Instance._map._regCats.Count)
                {
                    OBJ2_FUNCTIONS(0, 5);
                }
            }
            for (int i = 0; i < _menuObject2Objects.Count; i++)
            {
                if (_currentL2Int == 0) // Sectors
                {
                    // Set button text to name
                    _menuObject2Objects[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._sectors[i].GetName(true);

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
                

                if (!_menuObjectsL3Sector.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._posXInt.ToString();
                }

                if (!_menuObjectsL3Sector.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._posYInt.ToString();
                }

                
               
                if (!_menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int].GetName(false);
                }

                if (!_menuObjectsL3Sector.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._hiddenName;
                }

                if (!_menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._description;
                }

                if (!_menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.GetComponent<InputField>().text = MapManager.Instance._map._sectors[_currentL3Int]._lore;
                }


                if (MapManager.Instance._map._sectors[_currentL3Int]._controlFaction != -1)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Controlling Faction: " + MapManager.Instance._map._factions[MapManager.Instance._map._sectors[_currentL3Int]._controlFaction]._shorthand;
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Controlling Faction: Neutral";
                }


                if (_menuObjectsL3SectorPlacementActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape)))
                {
                    _menuObjectsL3SectorPlacementActive = false;

                    MapManager.Instance._escapeMenuIncompatTriggered = true;
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


                if (_menuObjectsL3SectorPlacementActive)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj12.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj12.GetComponent<Image>().color = new Color32(118, 118, 118, 255);
                }

                

                List<string> _t1 = new List<string>() { "Description", "Lore", "Regions" };

                _menuObjectsL3Sector.GetComponent<IndexScript>()._obj13.GetComponent<Text>().text = _t1[_sectorExtraId];

                if (_sectorExtraId == 0)
                {   
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.SetActive(true);
                    
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj14.SetActive(false);
                   
                }

                if (_sectorExtraId == 1)
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.SetActive(true);
                }
                else
                {
                    _menuObjectsL3Sector.GetComponent<IndexScript>()._obj15.SetActive(false);
                    
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


                        int a = MapManager.Instance._map._sectors[_currentL3Int]._regionCats[_sectorRegionInt];

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
                
                

                if (!_menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._name;
                }

                if (!_menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._shorthand;
                }

                if (!_menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._factionColor.r.ToString();
                }

                if (!_menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._factionColor.g.ToString();
                }

                if (!_menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Faction.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text = MapManager.Instance._map._factions[_currentL3Int]._factionColor.b.ToString();
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
                    
                    string _rpN2;
                    if (_repInt != -1)
                    {
                        GetRepName(MapManager.Instance._map._reps[_repInt]._repVal, out _rpN2, MapManager.Instance._map._reps[_repInt]._specialVal);
                        _menuObjectsL3Faction.GetComponent<IndexScript>()._obj18.GetComponent<Text>().text = "Reputation with " + MapManager.Instance._map._factions[_facInt]._shorthand + ": " + MapManager.Instance._map._reps[_repInt]._repVal.ToString() + " (" + _rpN2 + ")";
                    }


                    

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
                if (!_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._name;
                }

                if (!_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj4.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._shorthand;
                }

                if (!_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj5.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.r.ToString();
                }

                if (!_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj6.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.g.ToString();
                }

                if (!_menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Alliance.GetComponent<IndexScript>()._obj7.GetComponent<InputField>().text = MapManager.Instance._map._alliances[_currentL3Int]._allianceColor.b.ToString();
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
                
                if (MapManager.Instance._map._jumpGates[_currentL3Int]._sector1Id != -1)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj4.GetComponent<Text>().text = "Sector 1: " + MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[_currentL3Int]._sector1Id].GetName(true);
                }
                else
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj4.GetComponent<Text>().text = "Sector 1: None";
                }

                if (MapManager.Instance._map._jumpGates[_currentL3Int]._sector2Id != -1)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = "Sector 2: " + MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[_currentL3Int]._sector2Id].GetName(true);
                }
                else
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = "Sector 2: None";
                }

                
                if (!_menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._jumpGates[_currentL3Int]._name;
                }
                
                // Display connection type
                int _tID = MapManager.Instance._map._jumpGates[_currentL3Int]._typeId;
                int _tCount = MapManager.Instance._map._connType.Count;
                _menuObjectsL3Jumpgate.GetComponent<IndexScript>()._obj8.GetComponent<Text>().text = "----------------------\nCONNECTION TYPE: " + ((_tID >= 0 && _tID < _tCount) ? MapManager.Instance._map._connType[_tID]._name : "Regular");


                if (_mOL3JumpGatesS1PlacementActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape)))
                {
                    _mOL3JumpGatesS1PlacementActive = false;

                    MapManager.Instance._escapeMenuIncompatTriggered = true;
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

                    MapManager.Instance._escapeMenuIncompatTriggered = true;
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
                

                if (MapManager.Instance._map._playerFactions[_currentL3Int]._regFactionID != -1)
                {
                    _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Parent Faction: " + MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[_currentL3Int]._regFactionID]._name;
                }
                else
                {
                    _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Parent Faction: None";
                }
                    
                if (_mOL3PlayerFactionAnchorPlacementActive)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        OBJ3_PLAYERFACTION_FUNCTIONS(5);
                    }
                    else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                    {
                        OBJ3_PLAYERFACTION_FUNCTIONS(4);

                        MapManager.Instance._escapeMenuIncompatTriggered = true;
                    }

                    _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Anchor Location: Select";
                }
                else
                {
                    _menuObjectsL3PlayerFaction.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = (MapManager.Instance.ReturnSector(MapManager.Instance._map._playerFactions[_currentL3Int]._spawnPosition) >= 0) ? "Anchor Location: " + MapManager.Instance._map._sectors[MapManager.Instance.ReturnSector(MapManager.Instance._map._playerFactions[_currentL3Int]._spawnPosition)].GetName(true) : "Anchor Location: Empty Space (" + MapManager.Instance._map._playerFactions[_currentL3Int]._spawnPosition.x + "/" + MapManager.Instance._map._playerFactions[_currentL3Int]._spawnPosition.y + ")";
                }
            }
            else
            {
                _menuObjectsL3PlayerFaction.SetActive(false);
                _mOL3PlayerFactionAnchorPlacementActive = false;
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

                



                if (!_menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj1.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._name;
                }

                if (!_menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj20.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._name;
                }

                if (!_menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj21.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.r.ToString();
                }

                if (!_menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj22.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.g.ToString();
                }

                if (!_menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<InputField>().isFocused)
                {
                    _menuObjectsL3Region.GetComponent<IndexScript>()._obj23.GetComponent<InputField>().text = MapManager.Instance._map._regCats[_currentL3Int]._regions[_subRegionInt]._regionColor.b.ToString();
                }

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

                MapManager.Instance._escapeMenuIncompatTriggered = true;
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

        if (_mConnTypeContextObjs.Count > 13)
        {

            _mConnTypeContextOffset[0] = Mathf.Clamp(_mConnTypeContextOffset[0], 0, _mConnTypeContextObjs.Count - 13);
            if (_mConnTypeContextOffset[0] != _mConnTypeContextOffset[1])
            {
                RepositionObjectsConnContext();
                _mConnTypeContextOffset[1] = _mConnTypeContextOffset[0];
            }
        }
        else
        {

            _mConnTypeContextOffset[0] = 0;
            _mConnTypeContextOffsetVal = 0;
            if (_mConnTypeContextOffset[0] != _mConnTypeContextOffset[1])
            {
                RepositionObjectsConnContext();
            }
            _mConnTypeContextOffset[1] = 0;
        }

        if (_settingsMenu.activeSelf)
        {

            _settingsMenu.GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map.yBoundaryMax.ToString();
            _settingsMenu.GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = MapManager.Instance._map.yBoundaryMin.ToString();
            _settingsMenu.GetComponent<IndexScript>()._obj9.GetComponent<Text>().text = MapManager.Instance._map.xBoundaryMax.ToString();
            _settingsMenu.GetComponent<IndexScript>()._obj12.GetComponent<Text>().text = MapManager.Instance._map.xBoundaryMin.ToString();

            _settingsMenu.GetComponent<IndexScript>()._obj13.GetComponent<Text>().text = (MapManager.Instance._map._fleetRevealSectors) ? "Do fleets reveal sectors: Yes" : "Do fleets reveal sectors: No";
        }
        
    }
}
