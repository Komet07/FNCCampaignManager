using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UI;

public class TooltipHandlerGalaxy : MonoBehaviour
{
    #region Singleton
    public static TooltipHandlerGalaxy I;
    private void Awake()
    {
        I = this;
    }
    #endregion

    public float _tooltipWaitTime = 3f; // Waiting time for cursor to be still before tooltip appears.
    [SerializeField]
    float _currentWaitTime = 0f; // Current Time

    float _marginSpeed = 2; // 2 pixels of movement margin / frame.
    float _marginPos = 20; // 20 pixels of position margin.

    int currentHoverType = 0; // What is the current hover type?

    [SerializeField]
    Vector3 _startMPos = new Vector3();

    [SerializeField]
    List<GameObject> _tooltipMenuObj = new List<GameObject>(); // List of all relevant game objects

    [SerializeField]
    List<GameObject> _ttMenuConst = new List<GameObject>(); // List of constructed obj

    int _secondaryVar = 0;

    public float _waitTimeRemaining // How much time is left?
    {
        get
        {
            return _tooltipWaitTime - _currentWaitTime;
        }
    }

    // -- OBJECT DETERMINATION SYSTEM --

    bool DetermineHover(int _a)
    {
        if (_a == 0) // Empty Space - generally *should* be last.
        {
            return true;
        }
        else if (_a == 1) // Sectors
        {
            int i = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);
            return (i != -1) ? (MapManager.Instance.IsInDiscoveredList(i, false) ? true : false) : false;
        }
        else if (_a == 2) // Fleets
        {
            // Raycast
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_startMPos);
            if (Physics.Raycast(ray, out hit, 1000))
            {
                GameObject g = hit.collider.gameObject;

                Debug.Log(hit.collider.gameObject.name);

                for (int i = 0; i < FleetUIGalaxy.Instance._fleets.Count; i++)
                {
                    if (FleetUIGalaxy.Instance._fleets[i]._obj == g)
                    {
                        _secondaryVar = FleetUIGalaxy.Instance._fleets[i]._fleetID;
                        return true;
                    }
                }

                return false;
            }
        }


        return false; // GENERAL EXIT CASE
    }

    void DetermineHoverObjectMaster() // Determine what the mouse is hovering over - Priority List
    {
        int[] _priorityList = {2, 1, 0};

        for (int i = 0; i < _priorityList.Length; i++)
        {
            if (DetermineHover(_priorityList[i]))
            {
                currentHoverType = _priorityList[i];
                return;
            }
        }

        currentHoverType = 0;
    }

    // -- VIS. CONFIG --

    void MenuConstructor(int _type, int _lineCount, string[] _t, int[] _c, FontStyle[] _tType) // Set up visual of menu
    {
        Color32[] _colTypes = {new Color32(210,210,210,255), new Color32(150,150,150,255), new Color32(175, 0, 0, 255), new Color32(9, 129, 209, 255), new Color32(0,175,0,255)}; 
        // 0: Regular, 1: Darker, 2: Hostile, 3: Allied, 4: Owned

        float[] _maxWidths = {65, 130};
        float _bWidth = 8; // base width var, will be used later
        float _pWidth = 0; // Preferred menu width.
        float _cHeight = 0;
        float _tScaling = 0.2f;
        

        // Destruct old lines
        for (int i = 0; i < _ttMenuConst.Count; i++)
        {
            Destroy(_ttMenuConst[i]);
        }

        _ttMenuConst = new List<GameObject>();

        GameObject og = _tooltipMenuObj[1]; // Original Text bar obj

        // Construct lines
        for (int i = 0; i < _lineCount; i++)
        {
            

            GameObject n = Instantiate(og, og.transform.parent);

           
            // Vector2 _p1 = new Vector3();
            n.transform.localPosition = og.transform.localPosition - new Vector3(0, _cHeight, 0);

            n.GetComponent<Text>().text = _t[i];
            n.GetComponent<Text>().color = _colTypes[_c[i]];
            n.GetComponent<Text>().fontStyle = _tType[i];
            float _a = n.GetComponent<Text>().preferredWidth * _tScaling; // PREFERRED width w/ text scaling
            float _b = _a > _maxWidths[_type] ? _maxWidths[_type] : _a; // ACTUAL width w/ max setting

            int _linesN = Mathf.CeilToInt(_a / _maxWidths[_type]); // Amount of lines needed for text.

            _cHeight += _linesN * (og.GetComponent<RectTransform>().rect.height * 0.225f);

            _pWidth = (_pWidth < _b) ? _b : _pWidth;

            n.SetActive(true);

            _ttMenuConst.Add(n);
        }

        // Resize all to actual width
        foreach(GameObject _g in _ttMenuConst)
        {
            _g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _pWidth / _tScaling);
        }

        // Set Size of main Menu
        _tooltipMenuObj[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _pWidth + _bWidth);
        _tooltipMenuObj[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _cHeight + 6);
    }

    void MenuConfig() // Configure stats for menu
    {
        

        string[] _text = new string[5]; // Text strings
        int[] _tColor = new int[5]; // Color Codes for text lines
        FontStyle[] _tType = new FontStyle[5]; // Text types (regular, bold, italic, italic & Bold)
        int _tooltipType = 0; // 0: Regular
        int _lineCount = 1; // Amount of lines

        string vM = GalaxyMap.Instance._viewMode;

        if (currentHoverType == 0) // Empty Space
        {
            _tooltipType = 0;
            _lineCount = 1;

            // Line 1 - "Empty Space"
            _text[0] = "Empty Space";
            _tType[0] = FontStyle.Bold;
            _tColor[0] = 0;

            // If Debug on: Coordinates of Hex
            if (MapManager.Instance._map._debug)
            {
                Vector2 _pos = MapManager.Instance.GetScreenSectorPos(_startMPos);
                _text[_lineCount] = "(" + (int)_pos.x + " / " + (int)_pos.y + ")";
                _tColor[_lineCount] = 1;

                _lineCount++;
            }
        }
        else if (currentHoverType == 1) // (Named) Sector
        {
            _tooltipType = 0;
            _lineCount = 2;

            int i = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);
            Sector _s = MapManager.Instance._map._sectors[i];
            int _cFac = _s._controlFaction;
            bool flag_gm = MapManager.Instance._map._playerFactionId == -1;

            int _pFac = !flag_gm ? MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID : -1;

            
            

            bool flag_1 = _cFac > -1 && _cFac < MapManager.Instance._map._factions.Count; // Is sector in faction

            int _cAll = flag_1 ? MapManager.Instance._map._factions[_cFac]._allianceId : -1;
            bool flag_2 = _cAll > -1 && _cAll < MapManager.Instance._map._alliances.Count; // is faction (if it exists) in alliance.
            bool flag_3 = MapManager.Instance.IsInKnownOwnerList(i, false) ? true : false; // Is this Owner known?
            

            // Line 1 - Sector Name
            _text[0] = _s.GetName(!flag_gm);
            _tColor[0] = 0;
            _tType[0] = FontStyle.Bold;

            // Line 2 - Faction (Default)
            if (vM == "alliances") // Display Alliance affiliation
            {
                _text[1] = flag_3 ? (flag_2 ?  MapManager.Instance._map._alliances[_cAll]._name : "Unaligned") : "Unknown";
                _tColor[1] = 1;
                _tType[1] = FontStyle.Bold;
            }
            else if (vM == "relations") // Display Faction shorthand and relationship status
            {
                _pFac = flag_gm ? GalaxyMap.Instance._selFacInt : _pFac;

                float _val = 0;
                string _specCon = "";

                MapManager.Instance.GetRepState(_pFac, _cFac, out _val, out _specCon);

                Color32 _col;
                string _repName;
                GalaxyMap.Instance.DetermineRelationsColor(_val, _specCon, out _col, out _repName);

                _repName = _pFac == _cFac ? "Owned" : _repName;

                string[] t0 = {MapManager.Instance._map._factions[_cFac]._shorthand, _repName};
                
                _text[1] = flag_3 ? (flag_1 ? $"{t0[0]} ({t0[1]})" : "Neutral") : "Unknown";
                _tColor[1] = _repName == "Owned" ? 4 : (_specCon == "War" ? 2 : (_specCon == "Allied" ? 3 : 1));
                _tType[1] = FontStyle.Bold;
            }
            else // "Faction" && default
            {
                _text[1] = flag_3 ? (flag_1 ?  MapManager.Instance._map._factions[_cFac]._name : "Neutral") : "Unknown";
                _tColor[1] = 1;
                _tType[1] = FontStyle.Bold;

                if (flag_2 && flag_3) // ADD ALLIANCE SHORTHAND TO FACTION
                {
                    _text[1] += " (" + MapManager.Instance._map._alliances[_cAll]._shorthand + ")";
                }
            }


            // OPTIONAL : If Special Condition to Player Faction, display additional line
            float _val2 = 0;
            string _specCon2 = "";

            MapManager.Instance.GetRepState(_pFac, _cFac, out _val2, out _specCon2);
            if (_pFac != -1 && (_pFac == _cFac || _specCon2 == "War" || _specCon2 == "Allied") && (vM == "factions" || vM == "alliances"))
            {
                
                _text[_lineCount] = _pFac == _cFac ? "OWNED" : (_specCon2 == "War" ? "HOSTILE" : "ALLIED");
                _tColor[_lineCount] = _pFac == _cFac ? 4 : (_specCon2 == "War" ? 2 : 3);
                _tType[_lineCount] = FontStyle.Bold;

                _lineCount++;
            }

            // If Debug on: Coordinates of Hex
            if (MapManager.Instance._map._debug)
            {
                _text[_lineCount] = "(" + _s._posXInt + " / " + _s._posYInt + ")";
                _tColor[_lineCount] = 1;
                _tType[_lineCount] = FontStyle.Normal;

                _lineCount++;
            }
        }
        else if (currentHoverType == 2) // Fleet
        {
            int i = _secondaryVar;

            _tooltipType = 0;
            _lineCount = 1;

            Fleet _f = MapManager.Instance._map._fleets[i];
            int _fFac = _f._faction;

            bool flag_1 = _fFac > -1 && _fFac < MapManager.Instance._map._factions.Count;
 
            string[] _t0 = {flag_1 ? MapManager.Instance._map._factions[_fFac]._shorthand : "NEU", _f._name};

            // Line 1 - Fleet Name
            _text[0] = $"{_t0[0]} {_t0[1]}";
            _tColor[0] = 0;
            _tType[0] = FontStyle.Bold;

        }

        MenuConstructor(_tooltipType, _lineCount, _text, _tColor, _tType);
    }

    void MenuVisibilityMaster()
    {
        // Active
        _tooltipMenuObj[0].SetActive(true);

        // Position Menu
        Vector2 mPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_tooltipMenuObj[0].transform.parent as RectTransform, Input.mousePosition + new Vector3(20,-20,0), Camera.main, out mPos);

        _tooltipMenuObj[0].transform.position = _tooltipMenuObj[0].transform.parent.TransformPoint(mPos);

        // Configure Text
        MenuConfig();
    }

    // -- MASTER --

    void TooltipHandlerMain() // Master function of galaxy tooltips.
    {
        DetermineHoverObjectMaster();

        // Set visibility
        
        MenuVisibilityMaster();
    }

    void Update() // Runs each frame - only used to determine wait time
    {
        if (_currentWaitTime < _tooltipWaitTime)
        {
            _currentWaitTime += Time.deltaTime;
            _tooltipMenuObj[0].SetActive(false);

            if (_currentWaitTime >= _tooltipWaitTime)
            {
                TooltipHandlerMain();
            }
        }

        _currentWaitTime = Mathf.Clamp(_currentWaitTime, 0f, _tooltipWaitTime);

        // Reset Timer if MousePositionDelta > 0 / some small margin.
        if ((Input.mousePositionDelta.magnitude > _marginSpeed) || ((Input.mousePosition - _startMPos).magnitude > _marginPos) || !MapManager.Instance.TooltipGIncompatCheck)
        {
            _currentWaitTime = 0;
            _startMPos = Input.mousePosition;
        }

    }
}