using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UI;
using UnityEngine.Rendering;

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

    bool _shift = false;

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

                for (int i = 0; i < FleetUIGalaxy.Instance._fleets.Count; i++)
                {
                    if (FleetUIGalaxy.Instance._fleets[i]._obj == g)
                    {
                        _secondaryVar = FleetUIGalaxy.Instance._fleets[i]._fleetID;
                        return true;
                    }
                }
            }

            return false;
        }
        else if (_a == 3) // Connections
        {
            // GEOMETRIC APPROACH : Compare mouse world angle & distance (2d) to base point of connection with 2nd point. Steps: Get distance of mouse to point (Radius), get closest position on connection line to point (if radius < connection length) and compare to some margin. If within margin, trigger. -> Works from both points.

            for (int i = 0; i < MapManager.Instance._map._jumpGates.Count; i++)
            {
                // Get Connection World position
                Vector2[] _cWorldPos;

                MapManager.Instance.JumpgatePointPos(i, out _cWorldPos);

                // Visibility Verification
                JumpGateConnection _c = MapManager.Instance._map._jumpGates[i];

                bool flag_gm = MapManager.Instance._map._playerFactionId == -1;
                int _pFac = !flag_gm ? MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID : -1;

                bool flag_1 = _c._sector1Id > -1 && _c._sector1Id < MapManager.Instance._map._sectors.Count; // Sector 1 Exists
                bool flag_2 = _c._sector2Id > -1 && _c._sector2Id < MapManager.Instance._map._sectors.Count; // Sector 2 Exists

                bool flag_3 = flag_gm ? true : flag_1 ? MapManager.Instance._map._factions[_pFac].SectorExplored(_c._sector1Id) : false; // Is 1st Sector Explored
                bool flag_4 = flag_gm ? true : flag_2 ? MapManager.Instance._map._factions[_pFac].SectorExplored(_c._sector2Id) : false; // Is 2nd Sector Explored

                if (!flag_3 && !flag_4)
                {
                    continue; // This Jumpgate Entry is entirely invisible.
                }

                int _order = flag_3 ? 0 : 1; // Pick visible point as base point. 

                bool _detectionLimit = !(flag_3 && flag_4); // Impose tooltip trigger limit.

                float _margin = 0.05f;
                
                // Position Stuff
                Vector2 _pPoint = _cWorldPos[_order]; // Position of Base Point
                Vector2 _pPoint2 = _cWorldPos[1 - _order]; // Position of Second Point
                Vector2 _pDelta = _pPoint2 - _pPoint;

                float _maxR = (_pPoint2 - _pPoint).magnitude; // Maximum radius of mouse

                Vector3 _camPos = Camera.main.ScreenToWorldPoint(_startMPos);
                Vector2 _cPos = new Vector2(_camPos.x, _camPos.y);

                Vector2 _diffPos = _cPos - _pPoint;

                float _r = _diffPos.magnitude;

                if (_r <= _maxR)
                {
                    Vector2 _compPoint = _pPoint + _pDelta * (_r / _maxR);

                    if ((_compPoint - _cPos).magnitude < _margin)
                    {
                        _secondaryVar = i;
                        return true;
                    }
                }

                
                
            }


            return false;
        }


        return false; // GENERAL EXIT CASE
    }

    void DetermineHoverObjectMaster() // Determine what the mouse is hovering over - Priority List
    {
        int[] _priorityList = {3, 2, 1, 0};

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

    void MenuConstructor(int _type, List<object[]> _const) // Set up visual of menu
    {
        Color32[] _colTypes = {new Color32(210,210,210,255), new Color32(150,150,150,255), new Color32(175, 0, 0, 255), new Color32(9, 129, 209, 255), new Color32(0,175,0,255)}; 
        // 0: Regular, 1: Darker, 2: Hostile, 3: Allied, 4: Owned

        float[] _maxWidths = {65, 130};
        float _bWidth = 10; // base width var, will be used later
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
        GameObject ogBar = _tooltipMenuObj[2]; // Original Bar obj

        // Construct lines
        for (int i = 0; i < _const.Count; i++)
        {
            if (_const[i][0] == null)
            {
                continue;
            }

            if ((int)_const[i][0] == 0) // REGULAR TEXT
            {
                string _t = _const[i][1] != null ? (string)_const[i][1] : "";
                int _c = _const[i][2] != null ? (int)_const[i][2] : 0;
                FontStyle _f = _const[i][3] != null ? (FontStyle)_const[i][3] : FontStyle.Normal;
            
                GameObject n = Instantiate(og, og.transform.parent);

            
                // Vector2 _p1 = new Vector3();
                n.transform.localPosition = og.transform.localPosition - new Vector3(0, _cHeight, 0);

                n.GetComponent<Text>().text = _t;
                n.GetComponent<Text>().color = _colTypes[_c];
                n.GetComponent<Text>().fontStyle = _f;
                float _a = n.GetComponent<Text>().preferredWidth * _tScaling; // PREFERRED width w/ text scaling
                float _b = _a > _maxWidths[_type] ? _maxWidths[_type] : _a; // ACTUAL width w/ max setting

                int _linesN = Mathf.CeilToInt(_a / _maxWidths[_type]); // Amount of lines needed for text.

                if (_a / _maxWidths[_type] % 1 > 0.92f)
                {
                    _linesN++;
                }

                _cHeight += _linesN * (og.GetComponent<RectTransform>().rect.height * 0.225f);

                _pWidth = (_pWidth < _b) ? _b : _pWidth;

                n.SetActive(true);

                _ttMenuConst.Add(n);
            }
            else if ((int)_const[i][0] == 1) // BARS
            {
                /* 
                BARS - Allow for display of various bar & bar types - i.e relation values, fuel bars, etc.

                ARGUMENTS:
                Type of Line (0 - Aligned Left -> Fills up, 1 - Centered -> Can go both ways (pos & neg))
                Line width value
                Line color
                Line text -> For extra context (i.e. Fuel amount)
                */

                string _t = _const[i][4] != null ? (string)_const[i][4] : "";
                Color32 _c = _const[i][3] != null ? (Color32)_const[i][3] : new Color32(200,200,200,255);
                int _bType = _const[i][1] != null ? (int)_const[i][1] : 0;
                float _val = _const[i][2] != null ? (float)_const[i][2] : 0f;

                float _barWidth = _maxWidths[_type];

                GameObject n = Instantiate(ogBar, ogBar.transform.parent);
                GameObject nBar = n.GetComponent<IndexScript>()._obj1;
                GameObject nText = n.GetComponent<IndexScript>()._obj2;

                if (_bType == 0) // ALIGN LEFT -> PLACE LEFT
                {
                    nBar.GetComponent<RectTransform>().pivot = new Vector2(1f, 1.5f);

                    n.transform.localPosition = ogBar.transform.localPosition - new Vector3(0, _cHeight, 0);
                    nBar.transform.localPosition = new Vector3(0,0,0);
                }

                nBar.GetComponent<Image>().color = _c;

                nText.GetComponent<Text>().text = _t;

                n.transform.localPosition = ogBar.transform.localPosition - new Vector3(0, _cHeight, 0);

                _cHeight += ogBar.GetComponent<RectTransform>().rect.height;

                n.SetActive(true);
                _ttMenuConst.Add(n);

            }
            
        }

        // Set Size of main Menu
        _tooltipMenuObj[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _pWidth + _bWidth);
        _tooltipMenuObj[0].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _cHeight + 6);

        // Resize all to actual width
        for (int i = 0; i < _ttMenuConst.Count; i++)
        {
            GameObject _g = _ttMenuConst[i];

            int _v = (int)_const[i][0];

            if (_v == 0)
            {
                _g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _pWidth / _tScaling + 4);
            }
            else if (_v == 1)
            {
                float _scaleFac = (int)_const[i][1] == 0 ? 1f : 0.5f;
                _g.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _pWidth);
                _g.transform.localPosition = new Vector3((_pWidth + _bWidth) / 2, _g.transform.localPosition.y, 0);
                _g.GetComponent<IndexScript>()._obj1.transform.localPosition = (int)_const[i][1] == 0 ? new Vector3(0,0,0) : new Vector3(_pWidth * _scaleFac / 2,0,0);
                _g.GetComponent<IndexScript>()._obj1.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _pWidth * _scaleFac * Mathf.Abs((float)_const[i][2]));
                _g.GetComponent<IndexScript>()._obj2.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _pWidth / _tScaling);

            }
                
        }

        
    }

    void MenuConfig() // Configure stats for menu
    {
        List<object[]> _arg = new List<object[]>(); // Arguments for tooltip constructor.
        object[] _line = new object[5]; // TEMP for inserting arguments into list.
        /*
        GENERAL ARGUMENTS:
        0 - Type of line (0: Regular Text)

        ARGUMENTS FOR REGULAR TEXT:
        1 - Text string
        2 - Color code
        3 - Font Style (Normal, Bold, Italic, Bold&Italic)

        ARGUMENTS FOR BARS:
        1 - Type of Bar
        2 - Bar Value
        3 - Bar Color
        4 - Bar Text (If any)
        */

        int _tooltipType = 0; // 0: Regular

        string vM = GalaxyMap.Instance._viewMode;

        if (currentHoverType == 0) // Empty Space
        {
            _tooltipType = _shift ? 1 : 0;

            // Line 1 - "Empty Space"
            _line[0] = 0;
            _line[1] = "Empty Space";
            _line[3] = FontStyle.Bold;
            _line[2] = 0;
            _arg.Add(_line.Clone() as object[]);

            // Line 2 - "Empty Space Description" - only appears if shift is pressed
            _line[0] = 0;
            _line[1] = "This part contains no known sector...";
            _line[3] = FontStyle.BoldAndItalic;
            _line[2] = 1;
            _arg.Add(_line.Clone() as object[]);

            // If Debug on: Coordinates of Hex
            if (MapManager.Instance._map._debug)
            {
                Vector2 _pos = MapManager.Instance.GetScreenSectorPos(_startMPos);
                _line[0] = 0;
                _line[1] = "(" + (int)_pos.x + " / " + (int)_pos.y + ")";
                _line[2] = 1;
                _line[3] = 0;
                _arg.Add(_line.Clone() as object[]);
            }
        }
        else if (currentHoverType == 1) // (Named) Sector
        {
            _tooltipType = _shift ? 1 : 0; // WIDEN WHEN SHIFT IS PRESSED

            int i = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);
            Sector _s = MapManager.Instance._map._sectors[i];
            int _cFac = _s._controlFaction;
            bool flag_gm = MapManager.Instance._map._playerFactionId == -1;

            int _pFac = !flag_gm ? MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID : -1;

            
            

            bool flag_1 = _cFac > -1 && _cFac < MapManager.Instance._map._factions.Count; // Is sector in faction

            int _cAll = flag_1 ? MapManager.Instance._map._factions[_cFac]._allianceId : -1;
            bool flag_2 = _cAll > -1 && _cAll < MapManager.Instance._map._alliances.Count; // is faction (if it exists) in alliance.
            bool flag_3 = MapManager.Instance.IsInKnownOwnerList(i, false); // Is this Owner known?

            // Line 1 - Sector Name
            _line[0] = 0;
            _line[1] = _s.GetName(!flag_gm);
            _line[2] = 0;
            _line[3] = FontStyle.Bold;
            _arg.Add(_line.Clone() as object[]);

            // Line 2 - Faction (Default)
            if (vM == "alliances") // Display Alliance affiliation
            {
                _line[0] = 0;
                _line[1] = flag_3 ? (flag_2 ?  MapManager.Instance._map._alliances[_cAll]._name + (_shift ? " (" + MapManager.Instance._map._alliances[_cAll]._shorthand + ")" : "") : "Unaligned") : "Unknown"; // Show Alliance name, + shorthand IF Shift
                _line[2] = 1;
                _line[3] = FontStyle.Bold;
                _arg.Add(_line.Clone() as object[]);
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
                
                _line[0] = 0;
                _line[1] = flag_3 ? (flag_1 ? $"{t0[0]} ({t0[1]})" : "Neutral") : "Unknown"; // Line on Faction & Relationship state
                _line[2] = _repName == "Owned" ? 4 : (_specCon == "War" ? 2 : (_specCon == "Allied" ? 3 : 1));
                _line[3] = FontStyle.Bold;
                _arg.Add(_line.Clone() as object[]);
            }
            else if (vM == "regions") // Encode Region information (No Shift: Only the information for this region, Shift: All Regions)
            {
                if (!_shift)
                {
                    // Only display info on currently displayed region category
                    int _rInt = Mathf.Clamp(GalaxyMap.Instance._selFacInt, 0, MapManager.Instance._map._regCats.Count);

                    RegionCategory _r = MapManager.Instance._map._regCats[_rInt];

                    string[] _tR = {"region", "assigned value"};

                    // Flags assess whether known
                    int _rType = _r._knowledgeType;

                    bool flag_E = (_pFac != -1) ? MapManager.Instance._map._factions[_pFac].SectorExplored(i) : true; // Is this Sector explored?
                    bool flag_S = false; // Does this sector have a special value for the region category?
                    int _sVal = -1;
                    for (int k = 0; k < _s._regionCats.Count; k++)
                    {
                        if (_s._regionCats[k] == _rInt && _s._regionCatsRegionIds[k] != -1)
                        {
                            flag_S = true;
                            _sVal = _s._regionCatsRegionIds[k];
                            break;
                        }
                    }

                    bool flag_MeetsCriteria = (!flag_gm) ? (_rType == 0 ? flag_E : flag_3) : true;

                    _tR[0] = _r._name; // Assign Name
                    _tR[1] = flag_S ? (flag_MeetsCriteria ? _r._regions[_sVal]._name : "Unknown") : "No Data";

                    _line[0] = 0;
                    _line[1] = $"{_tR[0]}: {_tR[1]}"; // Line : Region Name & Region Value
                    _line[2] = 1;
                    _line[3] = FontStyle.Bold;
                    _arg.Add(_line.Clone() as object[]);

                }
                else
                {
                    for (int j = 0; j < MapManager.Instance._map._regCats.Count; j++)
                    {
                        RegionCategory _r = MapManager.Instance._map._regCats[j];

                        string[] _tR = {"region", "assigned value"};

                        // Flags
                        int _rType = _r._knowledgeType;

                        bool flag_E = (_pFac != -1) ? MapManager.Instance._map._factions[_pFac].SectorExplored(i) : true; // Is this Sector explored?
                        bool flag_S = false; // Does this sector have a special value for the region category?
                        int _sVal = -1;
                        for (int k = 0; k < _s._regionCats.Count; k++)
                        {
                            if (_s._regionCats[k] == j && _s._regionCatsRegionIds[k] != -1)
                            {
                                flag_S = true;
                                _sVal = _s._regionCatsRegionIds[k];
                                break;
                            }
                        }

                        bool flag_MeetsCriteria = (!flag_gm) ? (_rType == 0 ? flag_E : flag_3) : true;

                        _tR[0] = _r._name; // Assign Name
                        _tR[1] = (flag_S && _sVal != -1) ? (flag_MeetsCriteria ? _r._regions[_sVal]._name : "Unknown") : "No Data";

                        _line[0] = 0;
                        _line[1] = $"{_tR[0]}: {_tR[1]}"; // Line shows region name & value
                        _line[2] = 1;
                        _line[3] = FontStyle.Bold;
                        _arg.Add(_line.Clone() as object[]);
                    }
                }
            }
            else if (vM == "special_SectorVisibility") // Sector visibility - provide 
            {
                string[] _opt = {"Yes", "No"};
                string[] _opt2 = {"Y", "N"};

                int _fInt = GalaxyMap.Instance._selFacInt;

                // Flags
                bool flag_D = MapManager.Instance._map._factions[_fInt].SectorDiscovered(i);
                bool flag_E = MapManager.Instance._map._factions[_fInt].SectorExplored(i);
                bool flag_K = MapManager.Instance._map._factions[_fInt].SectorKnownOwner(i);

                // Determine Y / N
                int _dOpt = flag_D ? 0 : 1;
                int _eOpt = flag_E ? 0 : 1;
                int _kOpt = flag_K ? 0 : 1;

                if (!_shift)
                {
                    _line[0] = 0;
                    _line[1] = $"D: {_opt2[_dOpt]} / E: {_opt2[_eOpt]} / K: {_opt2[_kOpt]}"; // LINE : Show visibility states
                    _line[3] = FontStyle.Bold;
                    _line[2] = 1;
                    _arg.Add(_line.Clone() as object[]);

                    if (_cFac == _fInt)
                    {
                        _line[0] = 0;
                        _line[1] = $"> Sector is owned by {MapManager.Instance._map._factions[_cFac]._shorthand} so all three are 'Yes'"; // LINE : Explains that sector ownership will auto-yes all three values
                        _line[3] = FontStyle.BoldAndItalic;
                        _line[2] = 1;
                        _arg.Add(_line.Clone() as object[]);
                    }
                }
                else
                {
                    _line[0] = 0;
                    _line[1] = $"Discovered: {_opt[_dOpt]}"; // ONE LINE FOR EACH STATE
                    _line[3] = FontStyle.Bold; // Multi-assignment of FontStyle.Bold not necessary -> only done once for all three lines
                    _line[2] = _dOpt == 0 ? 4 : 2;
                    _arg.Add(_line.Clone() as object[]);

                    _line[1] = $"Explored: {_opt[_eOpt]}";
                    _line[2] = _eOpt == 0 ? 4 : 2;
                    _arg.Add(_line.Clone() as object[]);

                    _line[1] = $"Knows Sector Owner: {_opt[_kOpt]}";
                    _line[2] = _kOpt == 0 ? 4 : 2;
                    _arg.Add(_line.Clone() as object[]);

                    if (_cFac == _fInt)
                    {
                        _line[1] = $"> Sector is owned by {MapManager.Instance._map._factions[_cFac]._shorthand} so all three are 'Yes'";
                        _line[3] = FontStyle.BoldAndItalic;
                        _line[2] = 1;
                        _arg.Add(_line.Clone() as object[]);
                    }
                }
            }
            else // "Faction" && default if no other case provided
            {
                _line[0] = 0;
                _line[1] = flag_3 ? (flag_1 ?  MapManager.Instance._map._factions[_cFac]._name : "Neutral") : "Unknown";
                _line[2] = 1;
                _line[3] = FontStyle.Bold;

                if (flag_2 && flag_3) // ADD ALLIANCE SHORTHAND TO FACTION
                {
                    _line[1] += " (" + MapManager.Instance._map._alliances[_cAll]._shorthand + ")";
                }

                _arg.Add(_line.Clone() as object[]);
            }


            // OPTIONAL : If Special Condition to Player Faction, display additional line
            float _val2 = 0;
            string _specCon2 = "";

            MapManager.Instance.GetRepState(_pFac, _cFac, out _val2, out _specCon2);
            if (_pFac != -1 && (_pFac == _cFac || _specCon2 == "War" || _specCon2 == "Allied") && (vM == "factions" || vM == "alliances"))
            {
                _line[0] = 0;
                _line[1] = _pFac == _cFac ? "OWNED" : (_specCon2 == "War" ? "HOSTILE" : "ALLIED"); // LINE : Show special relationship states
                _line[2] = _pFac == _cFac ? 4 : (_specCon2 == "War" ? 2 : 3);
                _line[3] = FontStyle.Bold;
                _arg.Add(_line.Clone() as object[]);
            }

            // LINE : Sector Description (If Shift is pressed)
            if (_shift)
            {
                _line[0] = 0;
                _line[1] = _s._description != "" ? _s._description : "This sector appears to have no description...";
                _line[3] = FontStyle.BoldAndItalic;
                _line[2] = 1;
                _arg.Add(_line.Clone() as object[]);
            }

            // If Debug on: Coordinates of Hex
            if (MapManager.Instance._map._debug)
            {
                _line[0] = 0;
                _line[1] = "(" + _s._posXInt + " / " + _s._posYInt + ")"; // DEBUG COORDINATES
                _line[2] = 1;
                _line[3] = FontStyle.Normal;
                _arg.Add(_line.Clone() as object[]);
            }
        }
        else if (currentHoverType == 2) // Fleet
        {
            int i = _secondaryVar;

            _tooltipType = 0;

            Fleet _f = MapManager.Instance._map._fleets[i];
            int _fFac = _f._faction;
            int _pFac = (MapManager.Instance._map._playerFactionId >= 0) ? MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID : -1;
            int _fSec = _f._currentSector;

            bool flag_1 = _fFac > -1 && _fFac < MapManager.Instance._map._factions.Count; // Is assigned to Faction?
            bool flag_2 = _fSec > -1 && _fSec < MapManager.Instance._map._sectors.Count; // Is in a sector?
            bool flag_3 = MapManager.Instance.Fleet_IsOwnerKnown(i); // Is the Owner Known

            int _fAll = flag_1 ? MapManager.Instance._map._factions[_fFac]._allianceId : -1;
            bool flag_4 = _fAll > -1 && _fAll < MapManager.Instance._map._alliances.Count; // Is in Alliance?

            bool flag_5 = _f._travelling && MapManager.Instance.ReturnSector(_f._travelEnd) > -1; // Is fleet travelling & endpoint a valid sector
            int _fDest = flag_5 ? MapManager.Instance.ReturnSector(_f._travelEnd) : -1; // Int of destination sector

            bool flag_6 = _fFac == _pFac; // Is same faction as player?
            bool flag_7 = MapManager.Instance.Faction_Allied(_fFac, _pFac); // Is Fleet Allied
            bool flag_8 = MapManager.Instance.Faction_Hostile(_fFac, _pFac); // Is Fleet Hostile
 
            string[] _t0 = {flag_3 ? (flag_1 ? (!(vM == "alliances") ? MapManager.Instance._map._factions[_fFac]._shorthand : (flag_4 ? MapManager.Instance._map._alliances[_fAll]._shorthand : "NEU")) : "NEU") : "UNK", flag_3 ? _f._name : "Fleet"};

            // Line 1 - Fleet Name
            _line[0] = 0;
            _line[1] = $"{_t0[0]} {_t0[1]}";
            _line[2] = 0;
            _line[3] = FontStyle.Bold;
            _arg.Add(_line.Clone() as object[]);

            // Line 2 - Fleet Status
            string[] _t1 = {_f._status, _f._travelling ? "Dest: " + (flag_3 ? (flag_5 ? MapManager.Instance._map._sectors[_fDest].GetName(true) : "Empty Space") : "Unknown") : flag_2 ? MapManager.Instance._map._sectors[_fSec].GetName(true) : "Empty Space"};
            _line[0] = 0;
            _line[1] = $"{_t1[0]} - {_t1[1]}";
            _line[2] = 1;
            _line[3] = FontStyle.Bold;
            _arg.Add(_line.Clone() as object[]);

            // BAR : FUEL
            float _fuelPerc = _f.getMaxFuel > 0 ? _f.getCurrentFuel / _f.getMaxFuel : 0f;
            string[] _tFBar = {Mathf.RoundToInt(_fuelPerc*100).ToString() + "%", _f.getCurrentFuel + "/" + _f.getMaxFuel + " - " + Mathf.RoundToInt(_fuelPerc*100).ToString() + "%"};
            _line[0] = 1; // BAR
            _line[1] = 0; // Type 0 - Align Left
            _line[2] = _fuelPerc; // Value
            _line[3] = new Color32(140, 152, 226, 255); // Same color as in Fleet UI
            _line[4] = "FUEL: " + (_shift ? _tFBar[1] : _tFBar[0]); // Text
            _arg.Add(_line.Clone() as object[]);
            
            // OPTIONAL - Additional Rep stuff
            string _tRep = flag_6 ? "<color=green>Owned</color>" : (flag_7 ? "<color=#007DE1>Allied</color>" : (flag_8 ? "<color=red>Hostile</color>" : ""));
            if (_tRep != "")
            {
                _line[0] = 0;
                _line[1] = _tRep; // Line on REP
                _line[2] = 0;
                _line[3] = FontStyle.Bold;
                _arg.Add(_line.Clone() as object[]);
            }
        }
        else if (currentHoverType == 3) // Connection
        {
            int i = _secondaryVar;

            _tooltipType = _shift ? 1 : 0;


            JumpGateConnection _c = MapManager.Instance._map._jumpGates[i];

            bool flag_1 = _c._typeId > -1 && _c._typeId < MapManager.Instance._map._connType.Count;
            int _cTId = _c._typeId;
            int _s1 = _c._sector1Id;
            int _s2 = _c._sector2Id;

            
            

            bool flag_2 = _s1 > -1 && _s1 < MapManager.Instance._map._sectors.Count; // Does Sector 1 exist?
            bool flag_3 = _s2 > -1 && _s1 < MapManager.Instance._map._sectors.Count; // Does Sector 2 exist?

            bool flag_gm = MapManager.Instance._map._playerFactionId == -1; // IS GM?

            int _pFac = !flag_gm ? MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID : -1;

            bool flag_4 = flag_2 ? (flag_gm ? true : _c.Point1Vis(_pFac)) : false; // Connection 1 Known?
            bool flag_5 = flag_3 ? (flag_gm ? true : _c.Point2Vis(_pFac)) : false; // Connection 2 Known?

            // Line 1 - Connection Type
            _line[0] = 0;
            _line[1] = flag_1 ? MapManager.Instance._map._connType[_cTId]._name : "Lane";
            _line[2] = 0;
            _line[3] = FontStyle.Bold;
            _arg.Add(_line.Clone() as object[]);

            // Sectors
            if (_shift)
            {
                string[] _t = {flag_4 ? MapManager.Instance._map._sectors[_s1].GetName(true) : "Unknown", flag_5 ? MapManager.Instance._map._sectors[_s2].GetName(true) : "Unknown"};

                _line[0] = 0;
                _line[1] = $"{_t[0]} - {_t[1]}"; // Both endpoints in one line
                _line[2] = 1;
                _line[3] = FontStyle.Bold;
                _arg.Add(_line.Clone() as object[]);
            }
            else
            {
                string[] _t = {flag_4 ? MapManager.Instance._map._sectors[_s1].GetName(true) : "Unknown", flag_5 ? MapManager.Instance._map._sectors[_s2].GetName(true) : "Unknown"};

                _line[0] = 0;
                _line[1] = $"{_t[0]}"; // ENDPOINT 1
                _line[2] = 1;
                _line[3] = FontStyle.Bold;
                _arg.Add(_line.Clone() as object[]);

                _line[1] = $"{_t[1]}"; // ENDPOINT 2 - All Params except text identical to 1
                _arg.Add(_line.Clone() as object[]);
            }


        }

        MenuConstructor(_tooltipType, _arg);
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
                
                _shift = Input.GetKey(KeyCode.LeftShift);


                TooltipHandlerMain();
            }
        }

        _currentWaitTime = Mathf.Clamp(_currentWaitTime, 0f, _tooltipWaitTime);

        if (_currentWaitTime >= _tooltipWaitTime && _shift != Input.GetKey(KeyCode.LeftShift))
        {
            _shift = Input.GetKey(KeyCode.LeftShift);
            TooltipHandlerMain();
        }

        // Reset Timer if MousePositionDelta > 0 / some small margin.
        if ((Input.mousePositionDelta.magnitude > _marginSpeed) || ((Input.mousePosition - _startMPos).magnitude > _marginPos) || !MapManager.Instance.TooltipGIncompatCheck)
        {
            _currentWaitTime = 0;
            _startMPos = Input.mousePosition;
        }

    }
}