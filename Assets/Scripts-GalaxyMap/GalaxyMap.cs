using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GalaxyMap : MonoBehaviour
{
    #region Singleton
    public static GalaxyMap Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<GameObject> _hexagons = new List<GameObject>() { };
    public List<GameObject> _backgroundTiles = new List<GameObject>() { }; 
    public List<GameObject> _jgConnections = new List<GameObject>() { };

    public GameObject _originHexagon;
    public GameObject _originBackgroundTile;
    public GameObject _originJGC;

    public GameObject UI;
    public GameObject UIb;
    public GameObject UIc;

    // _viewmode Text

    public Text _viewModeText;
    public GameObject _viewModeButton;

    public GameObject _viewMode2Button;

    // Info Display

    public GameObject _InfoDisplay;
    public Text _InfoDisplayName;
    public Text _InfoDisplayControl;
    public Text _InfoDisplayDesc;

    public bool _regen = false;
    public bool _regen2 = false;
    public bool _backgroundTilesVis = false;
    public bool _enable = false;
    public bool _disable = false;

    public int _selectedInt = -1;
    public int _selFacInt = 0;
    public string _selectedType = "sector";

    public string _viewMode = "factions";

    // SPRITE COMPENDIUM
    [Header("Textures")]
    public Texture _spriteLineDash;
    public Texture _spriteLineDotted;
    public Texture _spriteLineSlanted;

    public void GalaxyMapInfoDisplay()
    {

        if (_selectedInt >= 0)
        {
            _InfoDisplay.SetActive(true);
            if (_selectedType == "sector" && _selectedInt < MapManager.Instance._map._sectors.Count)
            {
                bool _discoveredSector = true;
                if (MapManager.Instance._map._playerFactionId >= 0)
                {
                    _discoveredSector = false;
                    for (int i = 0; i < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._discoveredSectors.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._discoveredSectors[i] == _selectedInt)
                        {
                            _discoveredSector = true;
                        }
                    }
                }
                if (!_discoveredSector)
                {
                    _selectedInt = -1;
                    _InfoDisplay.SetActive(false);
                    return;
                }

                _InfoDisplayName.text = MapManager.Instance._map._sectors[_selectedInt].GetName(true);

                _InfoDisplayDesc.text = MapManager.Instance._map._sectors[_selectedInt]._description + "\n \n" + MapManager.Instance._map._sectors[_selectedInt]._lore;

                bool _knowsOwner = true;

                if (MapManager.Instance._map._playerFactionId >= 0)
                {
                    _knowsOwner = false;
                    for (int i = 0; i < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership[i] == _selectedInt)
                        {
                            _knowsOwner = true;
                        }
                    }
                }

                if (_knowsOwner)
                {

                    if (MapManager.Instance._map._sectors[_selectedInt]._controlFaction != -1)
                    {
                        if (_viewMode != "alliances")
                        {

                            _InfoDisplayControl.text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[_selectedInt]._controlFaction]._name;
                        }
                        else
                        {
                            if (MapManager.Instance._map._factions[MapManager.Instance._map._sectors[_selectedInt]._controlFaction]._allianceId != -1)
                            {
                                _InfoDisplayControl.text = MapManager.Instance._map._alliances[MapManager.Instance._map._factions[MapManager.Instance._map._sectors[_selectedInt]._controlFaction]._allianceId]._name;
                            }
                            else
                            {
                                _InfoDisplayControl.text = "Unaligned";
                            }
                        }

                    }
                    else
                    {
                        _InfoDisplayControl.text = "Unaligned";
                    }
                }
                else
                {
                    _InfoDisplayControl.text = "Unknown";
                }


            }
        }
        else
        {
            _InfoDisplay.SetActive(false);
        }
    }

    public void GalaxyMapLegend()
    {
        /*
        DESCRIPTION : UI ELEMENT ON MAIN SCREEN THAT DISPLAYS INFORMATION ON CONNECTION TYPES, AIDING VISUAL IDENTIFICATION
        */
    }

    public void SwitchViewMode()
    {
        string[] _viewModes = {"factions", "alliances", "relations", "regions"}; // IN ORDER!
        string[] _specialViewModes = {"special_SectorVisibility"};

        string[] _viewModesText = {"Mode: Factions", "Mode: Alliances", "Mode: Relations", "Mode: Regions"};
        string[] _specialViewModesText = {"Mode: Sector Vis."};

        // Find current 'viewmode'
        int viewmode = -1;

        for (int i = 0; i < _viewModes.Length; i++)
        {
            if (_viewModes[i] == _viewMode)
            {
                viewmode = i;
            }
        }

        if (viewmode == -1)
        {
            for (int i = 0; i < _specialViewModes.Length; i++)
            {
                if (_specialViewModes[i] == _viewMode)
                {
                    viewmode = i + _viewModes.Length;
                }
            }
        }

        if (viewmode == -1)
        {
            viewmode = 0;
        }

        // Handle switch in GM & non-GM mode
        if (MapManager.Instance._map._playerFactionId == -1) // GM CASE
        {
            viewmode++;
            if (viewmode >= _viewModes.Length + _specialViewModes.Length)
            {
                viewmode = 0;
            }
        }
        else
        {
            viewmode++;
            if (viewmode >= _viewModes.Length)
            {
                viewmode = 0;
            }
        }

        if (viewmode < _viewModes.Length)
        {
            _viewMode = _viewModes[viewmode];
            _viewModeText.text = _viewModesText[viewmode];
        }
        else
        {
            viewmode -= _viewModes.Length;
            _viewMode = _specialViewModes[viewmode];
            _viewModeText.text = _specialViewModesText[viewmode];
        }

        _viewMode2Button.SetActive(false);
    }

    public void RegenMap()
    {

        // delete old components

        // Destroy sector hexagons
        List<GameObject> _hexes = _hexagons;
        _hexagons = new List<GameObject>();
        for (int i = 0; i < _hexes.Count; i++)
        {
            Destroy(_hexes[i]);
        }

        // Destroy JG Connections
        List<GameObject> _jgConn = _jgConnections;
        _jgConnections = new List<GameObject>();
        for (int i = 0; i < _jgConn.Count; i++)
        {
            Destroy(_jgConn[i]);
            
        }

        // spawn new components

        // Sectors
        for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
        {
            // Instantiate
            GameObject _hexClone = Instantiate(_originHexagon, UI.transform);
            _hexClone.SetActive(true);

            // Set ref Sector
            Sector _sector = MapManager.Instance._map._sectors[i];

            // Set Color
            if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
            {
                _hexClone.GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = MapManager.Instance._map._factions[_sector._controlFaction]._factionColor;
            }
            else
            {
                _hexClone.GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
            }

            // Set Position on screen
            if (_sector._posXInt%2 == 0)
            {
                _hexClone.transform.position = new Vector3(.75f * _sector._posXInt, .9f * _sector._posYInt, 0);
            }
            else
            {
                _hexClone.transform.position = new Vector3(.75f * _sector._posXInt, (.9f * _sector._posYInt) + .45f, 0);
            }

            // Modify Name
            _hexClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = _sector.GetName(true);



            // Add hex to list
            _hexagons.Add(_hexClone);

        }

        // JG Connection
        for (int i = 0; i < MapManager.Instance._map._jumpGates.Count; i++)
        {
            // Instantiate
            GameObject _jgClone = Instantiate(_originJGC, UIb.transform);
            

            // Set sector 1 & 2 positions

            float _s1X = 0;
            float _s1Y = 0;

            float _s2X = 0;
            float _s2Y = 0;

            if (MapManager.Instance._map._jumpGates[i]._sector1Id != -1)
            {
                Sector _sector1 = MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector1Id];
                _s1X = _sector1._posXInt;
                _s1Y = _sector1._posYInt;

                MapManager.Instance._map._jumpGates[i]._s1p = new Vector2(_sector1._posXInt, _sector1._posYInt);
            }
            else
            {
                _s1X = MapManager.Instance._map._jumpGates[i]._s1p.x;
                _s1Y = MapManager.Instance._map._jumpGates[i]._s1p.y;
            }

            if (MapManager.Instance._map._jumpGates[i]._sector2Id != -1)
            {
                Sector _sector2 = MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector2Id];
                _s2X = _sector2._posXInt;
                _s2Y = _sector2._posYInt;

                MapManager.Instance._map._jumpGates[i]._s2p = new Vector2(_sector2._posXInt, _sector2._posYInt);
            }
            else
            {
                _s2X = MapManager.Instance._map._jumpGates[i]._s2p.x;
                _s2Y = MapManager.Instance._map._jumpGates[i]._s2p.y;
            }


            
            

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

            if ((_s2X*_s2X) % 2 == 1)
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

            Vector3 _p1 = new Vector3(_s1X*.75f + Mathf.Cos(theta1) * .3f, _sector1YPos + Mathf.Sin(theta1) * .3f, -5);
            Vector3 _p2 = new Vector3(_s2X*.75f + Mathf.Cos(theta2) * .3f, _sector2YPos + Mathf.Sin(theta2) * .3f, -5);

            _jgClone.GetComponent<LineRenderer>().SetPosition(0, _p1);
            _jgClone.GetComponent<LineRenderer>().SetPosition(1, _p2);
            _jgClone.GetComponent<IndexScript>()._obj1.transform.position = _p1;
            _jgClone.GetComponent<IndexScript>()._obj2.transform.position = _p2;

            // Type-specific visual adjustments
            int _tID = MapManager.Instance._map._jumpGates[i]._typeId;
            if (_tID != -1 && _tID < MapManager.Instance._map._connType.Count)
            {
                Debug.Log("Connection - Type ID:" + _tID);
                ConnectionType _ct = MapManager.Instance._map._connType[_tID];

                LineRenderer _lr = _jgClone.GetComponent<LineRenderer>();

                // SET LINE COLOR
                _lr.startColor = _ct._lineColor;
                _lr.endColor = _ct._lineColor;

                // SET COLOR OF DOTS
                _jgClone.GetComponent<IndexScript>()._obj1.GetComponent<Image>().color = _ct._lineColor;
                _jgClone.GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = _ct._lineColor;

                // LINE MATERIAL
                Material _m = _lr.material;

                if (_ct._lineType == 0) // REGULAR
                {
                    _lr.textureScale = new Vector2(1, 1);
                    _m.SetTexture("_MainTex", null);
                }
                else if (_ct._lineType == 1) // DASHED
                {
                    // Use distance for offset
                    _m.SetTexture("_MainTex", _spriteLineDash);
                    _lr.textureScale = new Vector2((10f / _ct._lineWidth), 1);
                    Debug.Log(_lr.textureScale);
                }
                else if (_ct._lineType == 2) // DOTTED
                {
                    _m.SetTexture("_MainTex", _spriteLineDotted);
                    _lr.textureScale = new Vector2((25f / _ct._lineWidth), 1);
                }
                else if (_ct._lineType == 3) // SLANTED
                {
                    _m.SetTexture("_MainTex", _spriteLineSlanted);
                    _lr.textureScale = new Vector2((15f / _ct._lineWidth), 1);
                }
                else if (_ct._lineType == 4) // NONE (JUST POINTS)
                {
                    _lr.textureScale = new Vector2(1, 1);
                    _m.SetTexture("_MainTex", null);

                }


                // SET LINE WIDTH
                float _w = 0.02f * _ct._lineWidth;
                _lr.startWidth = _w;
                _lr.endWidth = _w;

                Debug.Log("Connection - Line Width:" + _ct._lineWidth);

            }
            // Activate object
            _jgClone.SetActive(true);

            _jgConnections.Add(_jgClone);
        }


    }

    public void RegenBackgroundTiles()
    {
        // Check Tile amount

        int _deltaBoundsX = (int)MapManager.Instance._map.xBoundaryMax - (int)MapManager.Instance._map.xBoundaryMin;
        int _deltaBoundsY = (int)MapManager.Instance._map.yBoundaryMax - (int)MapManager.Instance._map.yBoundaryMin;

        int _xMin = (int)MapManager.Instance._map.xBoundaryMin;
        int _yMin = (int)MapManager.Instance._map.yBoundaryMin;

        int _totalAmount = _deltaBoundsX * _deltaBoundsY;


        // Add more if too little
        if (_backgroundTiles.Count < _totalAmount)
        {
            int _a = _backgroundTiles.Count;
            for (int i = 0; i < (_totalAmount - _a); i++)
            {
                GameObject _obj = Instantiate(_originBackgroundTile, UIc.transform);

                _obj.SetActive(true);

                _backgroundTiles.Add(_obj);
            }
        }
        else if (_backgroundTiles.Count > _totalAmount) // Remove if too many
        {
            int _a = _backgroundTiles.Count;
            for (int i = 0; i < (_a - _totalAmount); i++)
            {
                GameObject _obj = _backgroundTiles[_backgroundTiles.Count - 1];
                Destroy(_obj);
                _backgroundTiles.Remove(_backgroundTiles[_backgroundTiles.Count - 1]);
            }
        }

        // Initialise Position
        for (int i = 0; i < _deltaBoundsX; i++)
        {
            for (int j = 0; j < _deltaBoundsY; j++)
            {
                // Set Position on screen
                if ((_xMin + i) % 2 == 0)
                {
                    _backgroundTiles[(i * _deltaBoundsY) + j].transform.position = new Vector3(.75f * (i + _xMin), .9f * (j + _yMin), 0);
                }
                else
                {
                    _backgroundTiles[(i * _deltaBoundsY) + j].transform.position = new Vector3(.75f * (i + _xMin), (.9f * (j + _yMin)) + .45f, 0);
                }
            }
        }
    }


    public void BackgroundTilesTurnOn()
    {
        _backgroundTilesVis = !_backgroundTilesVis;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DetermineRelationsColor(float _repVal, string _specCon, out Color32 _repCol, out string _repName)
    {
        _repCol = new Color32(0, 0, 0, 255);
        _repName = "";

        if (_specCon == "War")
        {
            _repCol = new Color32(150, 0, 0, 255);
            _repName = "Hostile";
        }
        else if (_specCon == "Allied")
        {
            _repCol = new Color32(9, 129, 209, 255);
            _repName = "Allied";
        }
        else
        {
            if (_repVal >= 3.875f)
            {
                _repCol = new Color32(40, 190, 190, 255);
                _repName = "Excellent";
            }
            else if (_repVal >= 2.75f)
            {
                _repCol = new Color32(66, 200, 0, 255);
                _repName = "Very Good";
            }
            else if (_repVal >= 1.625f)
            {
                _repCol = new Color32(133, 200, 0, 255);
                _repName = "Good";
            }
            else if (_repVal >= 0.5f)
            {
                _repCol = new Color32(175, 200, 0, 255);
                _repName = "Mediocre";
            }
            else if (_repVal >= -0.5f)
            {
                _repCol = new Color32(200, 175, 0, 255);
                _repName = "Neutral / Unknown";
            }
            else if (_repVal >= -1.625f)
            {
                _repCol = new Color32(210, 135, 0, 255);
                _repName = "Poor";
            }
            else if (_repVal >= -2.75f)
            {
                _repCol = new Color32(220, 100, 0, 255);
                _repName = "Bad";
            }
            else if (_repVal >= -3.875f)
            {
                _repCol = new Color32(230, 66, 0, 255);
                _repName = "Very Bad";
            }
            else
            {
                _repCol = new Color32(240, 0, 0, 255);
                _repName = "Horrible";
            }
        }


        return;
    }


    void SwitchRelationsFaction()
    {
        if (_selFacInt >= MapManager.Instance._map._factions.Count -1)
        {
            _selFacInt = 0;
        }
        else
        {
            _selFacInt++;
        }
    }

    void SwitchRegionCategory()
    {
        if (_selFacInt >= MapManager.Instance._map._regCats.Count - 1)
        {
            _selFacInt = 0;
        }
        else
        {
            _selFacInt++;
        }
    }

    public void SwitchM()
    {
        if (_viewMode == "relations" || _viewMode == "special_SectorVisibility")
        {
            SwitchRelationsFaction();
        }
        else if (_viewMode == "regions")
        {
            SwitchRegionCategory();
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!MapManager.Instance._galaxy)
        {
            return;
        }

        if (_regen)
        {
            _regen = false;
            RegenMap();
        }

        if (_regen2)
        {
            _regen2 = false;
            RegenBackgroundTiles();
        }

        if (_enable)
        {
            _enable = false;
        }

        if (_disable)
        {
            _disable = false;
        }


        if (_backgroundTilesVis && !UIc.activeSelf)
        {
            UIc.SetActive(true);
        }

        if (!_backgroundTilesVis && UIc.activeSelf)
        {
            UIc.SetActive(false);
        }


        // Clamp PlayerFaction value to amount of _playerFactions
        MapManager.Instance._map._playerFactionId = Mathf.Clamp(MapManager.Instance._map._playerFactionId, -1, MapManager.Instance._map._playerFactions.Count - 1);

        // Update Hexagons
        for (int i = 0; i < _hexagons.Count; i++)
        {
            _hexagons[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = MapManager.Instance._map._sectors[i].GetName(true);
            if (_viewMode == "relations")
            {
                _hexagons[i].GetComponent<IndexScript>()._obj6.SetActive(true);
                // _hexagons[i].GetComponent<IndexScript>()._obj7.SetActive(false);
            }
            else if (_viewMode == "regions" || _viewMode == "special_SectorVisibility")
            {
                _hexagons[i].GetComponent<IndexScript>()._obj6.SetActive(true);
                // _hexagons[i].GetComponent<IndexScript>()._obj7.SetActive(false);
            }
            else
            {
                _hexagons[i].GetComponent<IndexScript>()._obj6.SetActive(false);
                
            }
            bool _discoveredSector = MapManager.Instance.IsInDiscoveredList(i, false);
            

            if (_discoveredSector)
            {
                _hexagons[i].SetActive(true);
            }
            else
            {
                _hexagons[i].SetActive(false);
            }
            if (MapManager.Instance._map._debug)
            {
                _hexagons[i].GetComponent<IndexScript>()._obj4.SetActive(true);
                _hexagons[i].GetComponent<IndexScript>()._obj5.SetActive(true);
                _hexagons[i].GetComponent<IndexScript>()._obj4.GetComponent<Text>().text = i.ToString();
            }
            else
            {
                _hexagons[i].GetComponent<IndexScript>()._obj4.SetActive(false);
                _hexagons[i].GetComponent<IndexScript>()._obj5.SetActive(false);
            }
            if (_viewMode == "factions")
            {
                /* if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                {
                    if (MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._factionImg == null)
                    {
                        _hexagons[i].GetComponent<IndexScript>()._obj7.SetActive(false);
                    }
                    else
                    {
                        _hexagons[i].GetComponent<IndexScript>()._obj7.GetComponent<RawImage>().texture = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._factionImg;
                        _hexagons[i].GetComponent<IndexScript>()._obj7.SetActive(true);
                    }
                }
                else
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj7.SetActive(false);
                } */ 
                if (MapManager.Instance._map._debug)
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = MapManager.Instance._map._sectors[i]._controlFaction.ToString();
                }
                bool _knowsSectorOwner = true;
                if (MapManager.Instance._map._playerFactionId >= 0 && MapManager.Instance._map._sectors[i]._controlFaction != MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                {
                    _knowsSectorOwner = MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID].SectorKnownOwner(i);

                    if (MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                    {
                        _knowsSectorOwner = true;
                    }
                }
                if (_knowsSectorOwner)
                {
                    if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                    { 
                        _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._factionColor;
                    }
                    else
                    {
                        _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    }
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                    {

                        if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._name;
                        }
                        else
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Neutral";
                        }

                    }
                    else
                    {
                        if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._shorthand;
                        }
                        else
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "NEU";
                        }
                    }
                }
                else
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                    {

                        _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Unknown";
                        

                    }
                    else
                    {

                        _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "UNK";
                     
                    }
                }
            }
            else if (_viewMode == "alliances")
            {
                if (MapManager.Instance._map._debug)
                {
                    if (MapManager.Instance._map._sectors[i]._controlFaction >= 0)
                    {
                        _hexagons[i].GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId.ToString();
                    }
                    else
                    {
                        _hexagons[i].GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = "-1";
                    }
                }
                bool _knowsSectorOwner = true;
                if (MapManager.Instance._map._playerFactionId >= 0)
                {
                    _knowsSectorOwner = false;
                    for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership.Count; j++)
                    {
                        if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership[j] == i)
                        {
                            _knowsSectorOwner = true;
                        }
                    }
                    if (MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                    {
                        _knowsSectorOwner = true;
                    }
                }
                if (_knowsSectorOwner)
                {
                    if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                    {
                        if (MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId != -1)
                        { 
                            _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = MapManager.Instance._map._alliances[MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId]._allianceColor;
                        }
                        else
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                        }
                    }
                    else
                    {
                        _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    }
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                    {

                        if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                        {
                            if (MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId != -1)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._alliances[MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId]._name;
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Neutral";
                            }
                        }
                        else
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Neutral";
                        }

                    }
                    else
                    {
                        if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                        {
                            if (MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId != -1)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._alliances[MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._allianceId]._shorthand;
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "NEU";
                            }
                        }
                        else
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "NEU";
                        }
                    }

                }
                else
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                    {

                        _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Unknown";


                    }
                    else
                    {

                        _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "UNK";

                    }
                }
            }
            else if (_viewMode == "relations")
            {

                _viewModeText.text = "Mode: Relations";
                bool _knowsSectorOwner = true;
                if (MapManager.Instance._map._playerFactionId >= 0)
                {
                    _knowsSectorOwner = false;
                    for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership.Count; j++)
                    {
                        if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership[j] == i)
                        {
                            _knowsSectorOwner = true;
                        }

                        if (MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                        {
                            _knowsSectorOwner = true;
                        }
                    }
                }

                if (MapManager.Instance._map._debug)
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = MapManager.Instance._map._sectors[i]._controlFaction.ToString();
                }
                if (_knowsSectorOwner)
                {
                    if (MapManager.Instance._map._playerFactionId >= 0)
                    {
                        
                        _viewMode2Button.SetActive(false);
                        if (MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID != -1 && MapManager.Instance._map._sectors[i]._controlFaction != -1 && MapManager.Instance._map._sectors[i]._controlFaction != MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                        {
                            float _val = 0;
                            string _specCon = "";

                            MapManager.Instance.GetRepState(MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID, MapManager.Instance._map._sectors[i]._controlFaction, out _val, out _specCon);

                            Color32 _col = new Color32(0, 0, 0, 255);
                            string _repName = "";
                            DetermineRelationsColor(_val, _specCon, out _col, out _repName);
                            _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = _col;
                            _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = _repName;
                        }
                        else
                        {
                            if (MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID != -1 && MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                                _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "Owned";
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                                _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "/";
                            }
                            
                        }
                        Ray ray;
                        RaycastHit hit;

                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                        {

                            if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._name;
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Neutral";
                            }

                        }
                        else
                        {
                            if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._shorthand;
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "NEU";
                            }
                        }
                    }
                    else
                    {
                        if (MapManager.Instance._map._factions.Count == 0)
                        {
                            _viewMode = "factions";
                            _viewModeText.text = "Mode: Factions";

                            _viewMode2Button.SetActive(false);
                            return;
                        }
                        _selFacInt = Mathf.Clamp(_selFacInt, 0, MapManager.Instance._map._factions.Count - 1);
                        _viewModeText.text = "Mode: Relations (" + MapManager.Instance._map._factions[_selFacInt]._shorthand + ")";

                        _viewMode2Button.SetActive(true);
                        _viewMode2Button.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Switch Factions";
                        if (MapManager.Instance._map._sectors[i]._controlFaction != -1 && MapManager.Instance._map._sectors[i]._controlFaction != _selFacInt)
                        {
                            float _val = 0;
                            string _specCon = "";

                            MapManager.Instance.GetRepState(_selFacInt, MapManager.Instance._map._sectors[i]._controlFaction, out _val, out _specCon);

                            Color32 _col;
                            string _repName;
                            DetermineRelationsColor(_val, _specCon, out _col, out _repName);
                            _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = _col;
                            _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = _repName;
                        }
                        else
                        {
                            if (MapManager.Instance._map._sectors[i]._controlFaction == _selFacInt)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                                _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "Owned";
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                                _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "/";
                            }

                        }
                        Ray ray;
                        RaycastHit hit;

                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                        {

                            if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._name;
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Neutral";
                            }

                        }
                        else
                        {
                            if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._shorthand;
                            }
                            else
                            {
                                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "NEU";
                            }
                        }
                    }
                }
                else
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                    _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "Unknown";
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                    {

                        _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Unknown";


                    }
                    else
                    {

                        _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "UNK";

                    }
                }
            }
            else if (_viewMode == "regions" && MapManager.Instance._map._regCats.Count > 0)
            {
                if (_selFacInt >= MapManager.Instance._map._regCats.Count)
                {
                    _selFacInt = 0;
                }
                
                bool _knowsSectorOwner = true;
                bool _knownCategory = true;
                if (MapManager.Instance._map._playerFactionId >= 0)
                {
                    _knowsSectorOwner = false;
                    _knownCategory = false;
                    if (MapManager.Instance._map._regCats[_selFacInt]._knowledgeType == 0) // Explored Sector
                    {
                        for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._exploredSectors.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._exploredSectors[j] == i)
                            {
                                _knowsSectorOwner = true;
                            }

                            if (MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                            {
                                _knowsSectorOwner = true;
                            }
                        }

                        for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._exploredSectors.Count; j++)
                        {
                            int a = MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._exploredSectors[j];
                            for (int k = 0; k < MapManager.Instance._map._sectors[a]._regionCats.Count; k++)
                            {
                                if (MapManager.Instance._map._sectors[a]._regionCats[k] == _selFacInt)
                                {
                                    _knownCategory = true;
                                }
                            }
                        }
                    }
                    else if (MapManager.Instance._map._regCats[_selFacInt]._knowledgeType == 1) // Known Sector owner
                    {
                        for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership.Count; j++)
                        {
                            if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership[j] == i)
                            {
                                _knowsSectorOwner = true;
                            }

                            if (MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                            {
                                _knowsSectorOwner = true;
                            }
                        }

                        for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership.Count; j++)
                        {
                            int a = MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership[j];
                            for (int k = 0; k < MapManager.Instance._map._sectors[a]._regionCats.Count; k++)
                            {
                                if (MapManager.Instance._map._sectors[a]._regionCats[k] == _selFacInt)
                                {
                                    _knownCategory = true;
                                }
                            }
                        }
                    }


                    
                }

                if (MapManager.Instance._map._debug)
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj5.GetComponent<Text>().text = MapManager.Instance._map._sectors[i]._controlFaction.ToString();
                }
                if (_knowsSectorOwner && _knownCategory)
                {
                    
                    if (MapManager.Instance._map._regCats.Count == 0)
                    {
                        _viewMode = "factions";
                        _viewModeText.text = "Mode: Factions";

                        _viewMode2Button.SetActive(false);
                        return;
                    }
                    if (_selFacInt >= MapManager.Instance._map._regCats.Count)
                    {
                        _selFacInt = 0;
                    }
                    

                    int _secRegInt = -1;

                    for (int j = 0; j < MapManager.Instance._map._sectors[i]._regionCats.Count; j++)
                    {
                        if (MapManager.Instance._map._sectors[i]._regionCats[j] == _selFacInt && MapManager.Instance._map._sectors[i]._regionCatsRegionIds.Count - 1 >= j)
                        {
                            _secRegInt = MapManager.Instance._map._sectors[i]._regionCatsRegionIds[j];
                        }
                    }

                    _secRegInt = Mathf.Clamp(_secRegInt, -1, MapManager.Instance._map._regCats[_selFacInt]._regions.Count - 1);

                    if (_secRegInt != -1)
                    {
                            
                        
                        _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = MapManager.Instance._map._regCats[_selFacInt]._regions[_secRegInt]._regionColor;
                        _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = MapManager.Instance._map._regCats[_selFacInt]._regions[_secRegInt]._name;
                    }
                    else
                    {
                            
                        _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                        _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "No Data";
                            

                    }
                    Ray ray;
                    RaycastHit hit;

                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.transform != null && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
                    {
                        bool _kso = true;
                        if (MapManager.Instance._map._playerFactionId >= 0)
                        {
                            _kso = false;
                            for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership.Count; j++)
                            {
                                if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership[j] == i)
                                {
                                    _kso = true;
                                }

                                if (MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                                {
                                    _kso = true;
                                }
                            }
                        }
                            

                        if (MapManager.Instance._map._sectors[i]._controlFaction != -1 && _kso)
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._name;
                        }
                        else if (!_kso)
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Unknown";
                        }
                        else
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "Neutral";
                        }

                    }
                    else
                    {
                        bool _kso = true;
                        if (MapManager.Instance._map._playerFactionId >= 0)
                        {
                            _kso = false;
                            for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership.Count; j++)
                            {
                                if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._knownSectorOwnership[j] == i)
                                {
                                    _kso = true;
                                }

                                if (MapManager.Instance._map._sectors[i]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                                {
                                    _kso = true;
                                }
                            }
                        }

                        if (MapManager.Instance._map._sectors[i]._controlFaction != -1 && _kso)
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._shorthand;
                        }
                        else if (!_kso)
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "UNK";
                        }
                        else
                        {
                            _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "NEU";
                        }
                    }
                }
                else
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "Unknown";
                    
                }
            }
            else if (_viewMode == "special_SectorVisibility")
            {
                if (MapManager.Instance._map._playerFactionId != -1)
                {
                    SwitchViewMode(); // CHANGE OUT IF VIEW MODE IS GM-ONLY & NOT GM
                    return;
                }
                
                _viewModeText.text = "Mode: (GM) Sector Vis. (" + MapManager.Instance._map._factions[_selFacInt]._shorthand + ")";

                if (MapManager.Instance._map._factions.Count == 0)
                {
                    SwitchViewMode();
                    return;
                }
                _selFacInt = Mathf.Clamp(_selFacInt, 0, MapManager.Instance._map._factions.Count - 1);
                _viewModeText.text = "Mode: Sector Vis. (" + MapManager.Instance._map._factions[_selFacInt]._shorthand + ")";

                _viewMode2Button.SetActive(true);
                _viewMode2Button.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Switch Factions";

                // HEXAGON COLORS - BASED ON DISCOVERED Y/N, EXPLORED Y/N, KNOWN SECTOR OWNER Y/N
                byte[] colorCase = {75, 175, 230};
                string[] textCase1 = {"N", "Y"};

                int _cFac = MapManager.Instance._map._sectors[i]._controlFaction;
                bool _flag1 = _cFac == _selFacInt;

                byte rCol = !_flag1 ? (MapManager.Instance._map._factions[_selFacInt].SectorDiscovered(i) ? colorCase[1] : colorCase[0]) : colorCase[2];
                byte gCol = !_flag1 ? (MapManager.Instance._map._factions[_selFacInt].SectorExplored(i) ? colorCase[1] : colorCase[0]) : colorCase[2];
                byte bCol = !_flag1 ? (MapManager.Instance._map._factions[_selFacInt].SectorKnownOwner(i) ? colorCase[1] : colorCase[0]) : colorCase[2];

                string dT = MapManager.Instance._map._factions[_selFacInt].SectorDiscovered(i) ? textCase1[1] : textCase1[0];
                string eT = MapManager.Instance._map._factions[_selFacInt].SectorExplored(i) ? textCase1[1] : textCase1[0];
                string kT = MapManager.Instance._map._factions[_selFacInt].SectorKnownOwner(i) ? textCase1[1] : textCase1[0];

                _hexagons[i].GetComponent<IndexScript>()._obj2.GetComponent<Image>().color = new Color32(rCol, gCol, bCol, 255); 
                _hexagons[i].GetComponent<IndexScript>()._obj3.GetComponent<Text>().text = "D: " + dT + " / E: " + eT + " / O: " + kT;
                if (MapManager.Instance._map._sectors[i]._controlFaction != -1)
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = MapManager.Instance._map._factions[MapManager.Instance._map._sectors[i]._controlFaction]._shorthand + (_flag1 ? " (Own)" : "");
                }
                else
                {
                    _hexagons[i].GetComponent<IndexScript>()._obj6.GetComponent<Text>().text = "NEU";
                }

            }
            
            //Check if selecting a sector hexagon
            Ray rayC;
            RaycastHit hitC;

            rayC = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayC, out hitC) && hitC.transform != null && hitC.transform.gameObject == _hexagons[i] && Input.GetMouseButtonDown(0) && MapManager.Instance.InfoMenuGCheck)
            {
                _selectedInt = i;
                _selectedType = "sector";
            }

        }

        // Region stuff
        if (_viewMode == "regions")
        {
            if (MapManager.Instance._map._regCats.Count > 0)
            {
                _selFacInt = Mathf.Clamp(_selFacInt, 0, MapManager.Instance._map._regCats.Count - 1);
                _viewModeText.text = "Mode: Regions (" + MapManager.Instance._map._regCats[_selFacInt]._name + ")";

                _viewMode2Button.SetActive(true);
                _viewMode2Button.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Switch Regions";
            }
            else
            {
                SwitchViewMode();
            }
            
        }

        // Update Jump Gate Connections
        for (int i = 0; i < _jgConnections.Count; i++)
        {
            bool _visConn1 = true;
            bool _visConn2 = true;

            if (MapManager.Instance._map._playerFactionId >= 0)
            {
                _visConn1 = false;
                _visConn2 = false;

                if (MapManager.Instance._map._jumpGates[i]._sector1Id != -1 && MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector1Id]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                {
                    _visConn1 = true;
                }

                if (MapManager.Instance._map._jumpGates[i]._sector2Id != -1 && MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector2Id]._controlFaction == MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID)
                {
                    _visConn2 = true;
                }

                for (int j = 0; j < MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._exploredSectors.Count; j++)
                {
                    if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._exploredSectors[j] == MapManager.Instance._map._jumpGates[i]._sector1Id && MapManager.Instance._map._jumpGates[i]._discoverable1 == true)
                    {
                        _visConn1 = true;
                        
                    }

                    if (MapManager.Instance._map._factions[MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID]._exploredSectors[j] == MapManager.Instance._map._jumpGates[i]._sector2Id && MapManager.Instance._map._jumpGates[i]._discoverable2 == true)
                    {
                        _visConn2 = true;
                    }
                }
            }

            if (_visConn1)
            {
                _jgConnections[i].GetComponent<IndexScript>()._obj1.SetActive(true);
                Color32 _col = _jgConnections[i].GetComponent<LineRenderer>().startColor;
                _jgConnections[i].GetComponent<LineRenderer>().startColor = new Color32(_col.r, _col.g, _col.b, 200);
            }
            else
            {
                _jgConnections[i].GetComponent<IndexScript>()._obj1.SetActive(false);
                Color32 _col = _jgConnections[i].GetComponent<LineRenderer>().startColor;
                _jgConnections[i].GetComponent<LineRenderer>().startColor = new Color32(_col.r, _col.g, _col.b, 0);
            }

            if (_visConn2)
            {
                _jgConnections[i].GetComponent<IndexScript>()._obj2.SetActive(true);
                Color32 _col = _jgConnections[i].GetComponent<LineRenderer>().endColor;
                _jgConnections[i].GetComponent<LineRenderer>().endColor = new Color32(_col.r, _col.g, _col.b, 200);
            }
            else
            {
                _jgConnections[i].GetComponent<IndexScript>()._obj2.SetActive(false);
                Color32 _col = _jgConnections[i].GetComponent<LineRenderer>().endColor;
                _jgConnections[i].GetComponent<LineRenderer>().endColor = new Color32(_col.r, _col.g, _col.b, 0);
            }

        }




        if (Input.GetKeyDown(KeyCode.Escape) && _selectedInt >= 0)
        {
            _selectedInt = -1;
        }

        // Info Display

        GalaxyMapInfoDisplay();
    }
}
