using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class SectorFleetContents : IComparable
    {
        public int _sectorID = -1;
        public List<int> _fleetIDs = new List<int>();

        public int _compareMode = 0;

        public int CompareTo(object obj)
        {
            if (_compareMode == 0) // SECTOR NAME
            {
                

                SectorFleetContents _SFC = obj as SectorFleetContents;
                if (_SFC != null)
                {
                    string _n1 = (_sectorID >= 0 && _sectorID < MapManager.Instance._map._sectors.Count) ? MapManager.Instance._map._sectors[_sectorID].GetName(true) : "Empty Space";
                    string _n2 = (_SFC._sectorID >= 0 && _SFC._sectorID < MapManager.Instance._map._sectors.Count) ? MapManager.Instance._map._sectors[_SFC._sectorID].GetName(true) : "Empty Space";
                   


                    if (_n1 == _n2)
                    {
                        return 0;
                    }

                    List<string> _n = new List<string>() { _n1, _n2 };
                    _n.Sort();
                    

                    if (_n[0] == _n1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    throw new ArgumentException("NOT A COMPARABLE OF CLASS : SectorFleetContents");
                }
                
            }
            else if (_compareMode == 1) // SECTOR NAME, BUT EMPTY SPACE AT TOP
            {


                SectorFleetContents _SFC = obj as SectorFleetContents;
                if (_SFC != null)
                {
                    string _n1 = (_sectorID >= 0 && _sectorID < MapManager.Instance._map._sectors.Count) ? MapManager.Instance._map._sectors[_sectorID].GetName(true) : "Empty Space";
                    string _n2 = (_SFC._sectorID >= 0 && _SFC._sectorID < MapManager.Instance._map._sectors.Count) ? MapManager.Instance._map._sectors[_SFC._sectorID].GetName(true) : "Empty Space";

                    if (_n1 == _n2)
                    {
                        return 0;
                    }

                    List<string> _n = new List<string>() { _n1, _n2 };
                    _n.Sort();

                    if (_n1 == "Empty Space")
                    {
                        return -1;
                    }
                    else if (_n2 == "Empty Space")
                    {
                        return 1;
                    }


                    if (_n[0] == _n1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    throw new ArgumentException("NOT A COMPARABLE OF CLASS : SectorFleetContents");
                }

            }

            return 0;
        }
    }

    public class OutlinerUIGalaxy : MonoBehaviour
    {

        #region Singleton
        public static OutlinerUIGalaxy Instance;
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        // MAIN PART
        [Header("Main Menu Objects")]
        public GameObject _title;
        public GameObject _fleetButton;

        [Header("Fleet Menu")]
        public bool _fleetIsOn = false;
        public GameObject _fleetMenu;
        public GameObject _fleetScrollbar;
        public GameObject _AddButton;

        public List<GameObject> _fleetMObjs1 = new List<GameObject>() { };
        public List<GameObject> _fleetMObjsSH = new List<GameObject>() { };
        public List<SectorFleetContents> _fleetList = new List<SectorFleetContents>() { };

        public int _comparisonMethod = 0;

        public GameObject _fMSubHeader;
        public GameObject _fMFleetCard;

        [Header("Individual Fleet Menu")]
        public bool _indivFleetIsOn = false;
        public int _currentFleetID = -1;
        public bool _iFMTravelEdit = false;
        public bool _iFMTravelEditS = false;
        public bool _iFMTravelEditE = false;
        public bool _iFMLocationSelect = false;
        public Vector2 _iFMStartLoc = new Vector2(0, 0);
        public Vector2 _iFMEndLoc = new Vector2(0, 0);
        public float _iFMProgress = 0f;

        public GameObject _indivFleetMenu;
        public List<GameObject> _gmObjs_IndivFM = new List<GameObject>() { };
        public GameObject _iFMTitleText;
        public GameObject _iFMNameText;
        public GameObject _iFMTransponderText;
        public GameObject _iFMOwnerText;
        public GameObject _iFMLocationText;
        public GameObject _iFMStatusText;
        public GameObject _iFMFuelbarM;
        public GameObject _iFMFuelbarA;
        public GameObject _iFMFuelbarT;
        public GameObject _iFMFuelHeader;
        public GameObject _iFMTravelHeader;
        public GameObject _iFMTravelText;
        public GameObject _iFMShipHeader;

        // Context Menu - Outliner
        [Header("Context Menu - Outliner")]
        public GameObject _contextMenuO;
        public GameObject _contextMenuO_TextTemplate;
        public GameObject _contextMenuO_ButtonTemplate;
        public Scrollbar _contextMenuO_Scrollbar;
        public List<GameObject> _contextMenuO_objs = new List<GameObject>() { };
        protected int _contextMenuOIndex = -1;
        protected int _contextMenuOInt2 = 0;


        public void RebuildFleetMenu()
        {
            for (int i = 0; i < _fleetMObjs1.Count; i++)
            {
                GameObject _obj = _fleetMObjs1[i];
                _fleetMObjs1.Remove(_fleetMObjs1[i]);
                Destroy(_obj);
                i--;
            }

            for (int i = 0; i < _fleetMObjsSH.Count; i++)
            {
                GameObject _obj = _fleetMObjsSH[i];
                _fleetMObjsSH.Remove(_fleetMObjsSH[i]);
                Destroy(_obj);
                i--;
            }

            for (int i = 0; i < _fleetList.Count; i++)
            {
                _fleetList.Remove(_fleetList[i]);
                i--;
            }

            if (!_fleetIsOn)
            {
                _fleetMenu.SetActive(false);

                return;
            }

            _fleetMenu.SetActive(true);

            float _counter2 = 0;
            for (int i = 0; i < MapManager.Instance._map._fleets.Count; i++)
            {
                if (MapManager.Instance.Fleet_IsVisible(i))
                {
                    _counter2++;

                    Fleet F = MapManager.Instance._map._fleets[i];
                    bool _placed = false;
                    for (int j = 0; j < _fleetList.Count; j++)
                    {
                        if (_fleetList[j]._sectorID == F._currentSector)
                        {
                            _placed = true;
                            _fleetList[j]._fleetIDs.Add(i);
                        }
                    }

                    if (!_placed)
                    {
                        SectorFleetContents _SFC = new SectorFleetContents();
                        _SFC._sectorID = F._currentSector;
                        _SFC._fleetIDs.Add(i);
                        _SFC._compareMode = _comparisonMethod;

                        _fleetList.Add(_SFC);
                    }
                }
            }
            

            float _maxFleetAmount = 9;
            float _verticalHeight = 55;

            float _startPlace = 0;

            if (_counter2 > _maxFleetAmount)
            {
                _startPlace = Mathf.Clamp(Mathf.RoundToInt(_fleetScrollbar.GetComponent<Scrollbar>().value * (_counter2 - _maxFleetAmount)), 0, _counter2 - _maxFleetAmount);
                _fleetScrollbar.SetActive(true);
            }
            else
            {
                _fleetScrollbar.SetActive(false);
            }
                
            float _counter = 0;
            float _counter3 = 0;

            _fleetList.Sort();
            

             for (int i = 0; i < _fleetList.Count; i++)
            {
                if (_counter >= _startPlace && _fleetMObjs1.Count < _maxFleetAmount)
                {
                    GameObject _obj = Instantiate(_fMSubHeader, _fMSubHeader.transform.parent);
                    _obj.SetActive(true);
                    if (_fleetList[i]._sectorID >= 0 && _fleetList[i]._sectorID < MapManager.Instance._map._sectors.Count)
                    {
                        _obj.GetComponent<Text>().text = "Sector - " + MapManager.Instance._map._sectors[_fleetList[i]._sectorID].GetName(true);
                    }
                    else
                    {
                        _obj.GetComponent<Text>().text = "Empty Space";
                    }

                    int _a = _fleetList[i]._sectorID;

                    _obj.GetComponent<Button>().onClick.AddListener(() => CENTER_SECTOR_FLEET(_a));

                    _obj.transform.localPosition = new Vector3(10, _verticalHeight * -1, -5);

                    _fleetMObjsSH.Add(_obj);

                    _verticalHeight += 55;
                }

                for (int j = 0; j < _fleetList[i]._fleetIDs.Count && _fleetMObjs1.Count < _maxFleetAmount; j++)
                {
                    if (!MapManager.Instance.Fleet_IsOwnerKnown(_fleetList[i]._fleetIDs[j]))
                    {
                        _counter3++;
                    }
                    if (_counter == _startPlace && j != 0 && _fleetMObjs1.Count <= _maxFleetAmount)
                    {
                        GameObject _obj = Instantiate(_fMSubHeader, _fMSubHeader.transform.parent);
                        _obj.SetActive(true);
                        if (_fleetList[i]._sectorID >= 0 && _fleetList[i]._sectorID < MapManager.Instance._map._sectors.Count)
                        {
                            _obj.GetComponent<Text>().text = "Sector - " + MapManager.Instance._map._sectors[_fleetList[i]._sectorID].GetName(true);
                        }
                        else
                        {
                            _obj.GetComponent<Text>().text = "Empty Space";
                        }

                        _obj.transform.localPosition = new Vector3(10, _verticalHeight * -1, -5);

                        int _a = _fleetList[i]._sectorID;
                        
                        _obj.GetComponent<Button>().onClick.AddListener(() => CENTER_SECTOR_FLEET(_a));

                        _fleetMObjsSH.Add(_obj);

                        _verticalHeight += 55;
                    }

                    if (_counter >= _startPlace)
                    {
                        GameObject _objF = Instantiate(_fMFleetCard, _fMFleetCard.transform.parent);
                        _objF.SetActive(true);
                        string _tPrefix = "NEU";
                        string _tName = "";
                        string _tStatus = "Idle";
                        string _tAdditional = "";

                        Fleet F = MapManager.Instance._map._fleets[_fleetList[i]._fleetIDs[j]];
                        if (F._faction >= 0 && F._faction < MapManager.Instance._map._factions.Count)
                        {
                            _tPrefix = MapManager.Instance.Fleet_IsOwnerKnown(_fleetList[i]._fleetIDs[j]) ? MapManager.Instance._map._factions[F._faction]._shorthand : "UNK";

                            int _pFInt = (MapManager.Instance._map._playerFactionId >= 0) ? MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID : -1;

                            if (F._faction == _pFInt)
                            {
                                _tAdditional = " - <color=green>Owned</color>";
                            }
                            else if (MapManager.Instance.Faction_Allied(F._faction, _pFInt) && MapManager.Instance.Fleet_IsOwnerKnown(_fleetList[i]._fleetIDs[j]))
                            {
                                _tAdditional = " - <color=#007DE1>Allied</color>";
                            }
                            else if (MapManager.Instance.Faction_Hostile(F._faction, _pFInt) && MapManager.Instance.Fleet_IsOwnerKnown(_fleetList[i]._fleetIDs[j]))
                            {
                                _tAdditional = " - <color=red>Hostile</color>";
                            }
                        }

                        _tName = MapManager.Instance.Fleet_IsOwnerKnown(_fleetList[i]._fleetIDs[j]) ? F._name : "Fleet " + _counter3;

                        if (F._travelling)
                        {
                            _tStatus = "Travelling";
                        }

                        

                        _objF.GetComponent<Text>().text = _tPrefix + " Fleet - " + _tName + "\n" + _tStatus + _tAdditional;

                        int _a = _fleetList[i]._fleetIDs[j];
                        _objF.GetComponent<Button>().onClick.AddListener(() => INITIALIZE_INDIV_FLEET_MENU(_a));

                        _objF.transform.localPosition = new Vector3(10, _verticalHeight * -1, -5);

                        _fleetMObjs1.Add(_objF);

                        _verticalHeight += 85;
                    }

                    _counter++;
                }
            }

            _fleetMenu.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
            _fleetScrollbar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (_verticalHeight-50) / 5);
            _fleetScrollbar.transform.SetAsLastSibling();
        }

        public void OUTLINER_MAIN_FUNCTIONS(int _a)
        {
            if (_a == 0) // OPEN / CLOSE FLEET TAB
            {
                _fleetIsOn = !_fleetIsOn;

                RebuildFleetMenu();
                _indivFleetIsOn = false;
            }
            if (_a == 1) // ADD FLEET BUTTON
            {
                if (MapManager.Instance._map._playerFactionId >= 0 && MapManager.Instance._map._playerFactionId < MapManager.Instance._map._playerFactions.Count)
                {
                    MapManager.Instance.AddFleet(MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID);
                }
                else
                {
                    MapManager.Instance.AddFleet();
                }
            }
            else if (_a == 2) // CENTER SECTOR IN VIEWPORT
            {
                
            }
        }

        public void FleetMenuScrollbar_Switch()
        {
            if (_fleetMObjs1.Count > 0)
            {
                RebuildFleetMenu();
            }
        }

        public void INITIALIZE_INDIV_FLEET_MENU(int _fleetID)
        {
            if (_fleetID < 0)
            {
                _indivFleetIsOn = false;
            }

            if (!_indivFleetIsOn)
            {
                _currentFleetID = _fleetID;
                _indivFleetIsOn = true;
            }
            else if (_indivFleetIsOn && _currentFleetID == _fleetID)
            {
                _currentFleetID = -1;
                _indivFleetIsOn = false;
            }
            else
            {
                _currentFleetID = _fleetID;
                _indivFleetIsOn = true;
            }
        }

        public void CENTER_SECTOR_FLEET(int _sectorID)
        {
            if (_sectorID < 0 || _sectorID >= MapManager.Instance._map._sectors.Count)
            {
                return;
            }

            Vector2 _p = MapManager.Instance.TurnSectorIntoRealPos(new Vector2(MapManager.Instance._map._sectors[_sectorID]._posXInt, MapManager.Instance._map._sectors[_sectorID]._posYInt));
            CamMovement.Instance.transform.position = new Vector3(_p.x, _p.y, -10);
        }

        public void INDIV_FLEET_FUNCTIONS(int _a)
        {
            
            if (_a == 0) // DISPLAY ELEMENTS
            {
                /* 
                GOAL : Structure Fleet Outliner modularly and reposition / adjust UI elements based on which sections are shown / not shown
                */
                if (_currentFleetID < 0 || _currentFleetID >= MapManager.Instance._map._fleets.Count)
                {
                    _currentFleetID = -1;
                    _indivFleetIsOn = false;
                    return;
                }

                float _vHeight = 240;

                Fleet F = MapManager.Instance._map._fleets[_currentFleetID];

                float[] _fuelVal = {F.getCurrentFuel, F.getMaxFuel};

                if (MapManager.Instance._map._lockSelection || (MapManager.Instance._map._playerFactionId >= 0 && F._faction != MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID))
                {
                    for (int i = 0; i < _gmObjs_IndivFM.Count; i++)
                    {
                        _gmObjs_IndivFM[i].SetActive(false);
                    }

                    if (MapManager.Instance.Fleet_IsOwnerKnown(_currentFleetID))
                    {
                        _iFMNameText.GetComponent<Text>().text = "Name: " + F._name;

                        if (F._faction >= 0 && F._faction < MapManager.Instance._map._factions.Count)
                        {
                            _iFMOwnerText.GetComponent<Text>().text = "Faction: " + MapManager.Instance._map._factions[F._faction]._shorthand;
                            _iFMTitleText.GetComponent<Text>().text = "Fleet - " + MapManager.Instance._map._factions[F._faction]._shorthand + " " + F._name;
                        }
                        else
                        {
                            _iFMOwnerText.GetComponent<Text>().text = "Faction: Neutral";
                            _iFMTitleText.GetComponent<Text>().text = "Fleet - NEU " + F._name;
                        }
                    }
                    else
                    {
                        _iFMNameText.GetComponent<Text>().text = "Name: Unknown";
                        _iFMOwnerText.GetComponent<Text>().text = "Faction: Unknown";
                        _iFMTitleText.GetComponent<Text>().text = "Fleet - Unknown";
                    }

                    if (F._currentSector >= 0 && F._currentSector < MapManager.Instance._map._sectors.Count)
                    {
                        _iFMLocationText.GetComponent<Text>().text = "Location: " + MapManager.Instance._map._sectors[F._currentSector].GetName(true);
                    }
                    else
                    {
                        _iFMLocationText.GetComponent<Text>().text = "Location: Empty Space";
                    }

                    

                    if (MapManager.Instance.IsFaction(F._faction))
                    {
                        _iFMStatusText.SetActive(true);
                        _iFMStatusText.GetComponent<Text>().text = "Status: " + F._status;

                        _iFMTransponderText.SetActive(true);
                        _iFMTransponderText.GetComponent<Text>().text = (F._transponder) ? "Transponder: On" : "Transponder: Off";
                    }
                    else
                    {
                        _iFMStatusText.SetActive(false);
                        _iFMTransponderText.SetActive(false);
                        _vHeight -= 40;
                    }

                    // -- FUEL SECTION --
                    if (F.getMaxFuel > 0 && MapManager.Instance.IsFaction(F._faction))
                    {
                        _iFMFuelbarM.SetActive(true);
                        _iFMFuelHeader.SetActive(true);

                        _vHeight += 170;

                        _iFMFuelHeader.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 160, -5);
                        _iFMFuelbarM.GetComponent<RectTransform>().localPosition = new Vector3(50, (_vHeight * -1) + 95, -5);
                        _iFMFuelbarT.GetComponent<Text>().text = $"{_fuelVal[0]} / {_fuelVal[1]}";
                        _iFMFuelbarA.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(900 * (_fuelVal[0] / _fuelVal[1]), 0, 900));
                    }
                    else
                    {
                        _iFMFuelbarM.SetActive(false);
                        _iFMFuelHeader.SetActive(false);
                    }


                    // -- TRAVELLING SECTION --
                    if (F._status == "Travelling")
                    {
                        _iFMTravelHeader.SetActive(true);
                        _iFMTravelText.SetActive(true);

                        _vHeight += 105;

                        _iFMTravelHeader.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 105, -5);
                        _iFMTravelText.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 45, -5);

                        if (MapManager.Instance.IsFaction(F._faction))
                        {
                            string _t1 = (MapManager.Instance.ReturnSector(F._travelStart) < 0 || !MapManager.Instance.IsInDiscoveredList(MapManager.Instance.ReturnSector(F._travelStart), false)) ? ("Empty Space (" + F._travelStart.x + "/" + F._travelStart.y + ")") : MapManager.Instance._map._sectors[MapManager.Instance.ReturnSector(F._travelStart)].GetName(true);
                            string _t2 = (MapManager.Instance.ReturnSector(F._travelEnd) < 0 || !MapManager.Instance.IsInDiscoveredList(MapManager.Instance.ReturnSector(F._travelEnd), false)) ? ("Empty Space (" + F._travelEnd.x + "/" + F._travelEnd.y + ")") : MapManager.Instance._map._sectors[MapManager.Instance.ReturnSector(F._travelEnd)].GetName(true);

                            _iFMTravelText.GetComponent<Text>().text = "Current Travel: " + _t1 + " to " + _t2 + " (" + (F._travelCompleted * 100) + "%)";
                        }
                        else
                        {
                            _iFMTravelText.GetComponent<Text>().text = "Currently travelling";
                        }

                    }
                    else
                    {
                        _iFMTravelHeader.SetActive(false);
                        _iFMTravelText.SetActive(false);
                    }

                    // -- SHIP SECTION --
                    _vHeight += 50;
                    _iFMShipHeader.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 50, -5);

                }
                else
                {
                    for (int i = 0; i < _gmObjs_IndivFM.Count; i++)
                    {
                        if (_gmObjs_IndivFM[i] != null)
                        {
                            _gmObjs_IndivFM[i].SetActive(true);
                        }
                    }

                    _iFMStatusText.SetActive(true);

                    _iFMNameText.GetComponent<Text>().text = "Name:";
                    _iFMOwnerText.GetComponent<Text>().text = "Faction:";
                    string _fPrefix = (F._faction >= 0) ? MapManager.Instance._map._factions[F._faction]._shorthand : "NEU";
                    _iFMTitleText.GetComponent<Text>().text = "Fleet - " + _fPrefix + " " + F._name;
                    _iFMLocationText.GetComponent<Text>().text = "Location:";
                    _iFMStatusText.GetComponent<Text>().text = "Status:";
                    _iFMTransponderText.GetComponent<Text>().text = "Transponder:              ";
                    _gmObjs_IndivFM[0].GetComponent<InputField>().text = F._name;
                    _gmObjs_IndivFM[1].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = (F._faction >= 0) ? MapManager.Instance._map._factions[F._faction]._shorthand : "Neutral";
                    if (!_iFMLocationSelect)
                    {
                        _gmObjs_IndivFM[2].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = (F._currentSector >= 0 && F._currentSector < MapManager.Instance._map._sectors.Count) ? MapManager.Instance._map._sectors[F._currentSector].GetName(true) : "Empty Space";
                    }
                    else
                    {
                        _gmObjs_IndivFM[2].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Select";
                    }

                    _gmObjs_IndivFM[3].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = F._status;
                    _gmObjs_IndivFM[23].GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = (F._transponder) ? "On" : "Off";

                    _iFMFuelHeader.SetActive(true);
                    _vHeight += 70;
                    _iFMFuelHeader.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 60, -5);

                    // FUEL SECTION
                    if (F.getMaxFuel > 0)
                    {
                        _iFMFuelbarM.SetActive(true);

                        _vHeight += 100;

                        _iFMFuelbarM.GetComponent<RectTransform>().localPosition = new Vector3(50, (_vHeight * -1) + 95, -5);
                        
                        _iFMFuelbarT.GetComponent<Text>().text = $"{_fuelVal[0]} / {_fuelVal[1]}";
                        _iFMFuelbarA.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Clamp(900 * (_fuelVal[0] / _fuelVal[1]), 0, 900));
                    }
                    else
                    {
                        _iFMFuelbarM.SetActive(false);
                    }

                    // FUEL - GM SECTION
                    _vHeight += 35;


                    if (_fuelVal[1] > 0)
                    {
                        _gmObjs_IndivFM[6].SetActive(true);
                        _gmObjs_IndivFM[7].SetActive(true);

                        _vHeight += 45;

                        _gmObjs_IndivFM[6].GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 45, -5);
                        _gmObjs_IndivFM[7].GetComponent<RectTransform>().localPosition = new Vector3(325, (_vHeight * -1) + 45, -5);

                        if (!_gmObjs_IndivFM[7].GetComponent<InputField>().isFocused)
                        {
                            _gmObjs_IndivFM[7].GetComponent<InputField>().text = _fuelVal[0].ToString();
                        }
                    }
                    else
                    {
                        _gmObjs_IndivFM[6].SetActive(false);
                        _gmObjs_IndivFM[7].SetActive(false);
                    }

                    // -- TRAVELLING SECTION --
                    if (F._status == "Travelling")
                    {
                        _iFMTravelHeader.SetActive(true);
                        _iFMTravelText.SetActive(true);

                        _gmObjs_IndivFM[9].SetActive(true);
                        _gmObjs_IndivFM[10].SetActive(true);
                        _gmObjs_IndivFM[11].SetActive(true);

                        _vHeight += 155;

                        _iFMTravelHeader.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 155, -5);
                        _iFMTravelText.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 95, -5);
                        _gmObjs_IndivFM[9].GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 45, -5);
                        _gmObjs_IndivFM[10].GetComponent<RectTransform>().localPosition = new Vector3(95, (_vHeight * -1) + 45, -5);
                        _gmObjs_IndivFM[11].GetComponent<RectTransform>().localPosition = new Vector3(220, (_vHeight * -1) + 45, -5);

                        string _t1 = (MapManager.Instance.ReturnSector(F._travelStart) < 0) ? ("Empty Space (" + F._travelStart.x + "/" + F._travelStart.y + ")") : MapManager.Instance._map._sectors[MapManager.Instance.ReturnSector(F._travelStart)].GetName(true);
                        string _t2 = (MapManager.Instance.ReturnSector(F._travelEnd) < 0) ? ("Empty Space (" + F._travelEnd.x + "/" + F._travelEnd.y + ")") : MapManager.Instance._map._sectors[MapManager.Instance.ReturnSector(F._travelEnd)].GetName(true);

                        _iFMTravelText.GetComponent<Text>().text = "Current Travel: " + _t1 + " to " + _t2 + " (" + (F._travelCompleted * MapManager.Instance.SectorDistance(F._travelStart, F._travelEnd)) + "/" + MapManager.Instance.SectorDistance(F._travelStart, F._travelEnd) + " Hexes)";
                    }
                    else
                    {
                        _iFMTravelHeader.SetActive(false);
                        _iFMTravelText.SetActive(false);
                        _iFMTravelEdit = false;
                        _gmObjs_IndivFM[9].SetActive(false);
                        _gmObjs_IndivFM[10].SetActive(false);
                        _gmObjs_IndivFM[11].SetActive(false);
                    }

                    // -- TRAVEL EDIT SECTION --
                    if (F._status == "Travelling" && _iFMTravelEdit)
                    {

                        // Deactivate the following Objects (9 - 11)
                        for (int i = 9; i <= 11; i++)
                        {
                            _gmObjs_IndivFM[i].SetActive(false);
                        }

                        // Activate the following Objects (12 - 20)
                        for (int i = 12; i <= 20; i++)
                        {
                            _gmObjs_IndivFM[i].SetActive(true);
                        }

                        // Adjust vHeight by 135
                        _vHeight += 135;

                        // 
                        float[] _pX = new float[] { 10, 200, 285, 10, 250, 10, 230, 10, 230 };
                        float[] _pY = new float[] { 180, 175, 175, 130, 130, 85, 82.5f, 40, 37.5f };

                        // Set Positions of UI elements relative to needed height
                        for (int i = 0; i <= 8; i++)
                        {
                            _gmObjs_IndivFM[i + 12].GetComponent<RectTransform>().localPosition = new Vector3(_pX[i], (_vHeight * -1) + _pY[i], -5);
                        }

                        if (_iFMTravelEditS)
                        {
                            _gmObjs_IndivFM[21].GetComponent<Text>().text = "Select Position";
                        }
                        else
                        {
                            string _t1 = (MapManager.Instance.ReturnSector(_iFMStartLoc) < 0) ? ("Empty Space (" + _iFMStartLoc.x + "/" + _iFMStartLoc.y + ")") : MapManager.Instance._map._sectors[MapManager.Instance.ReturnSector(_iFMStartLoc)].GetName(true);
                            _gmObjs_IndivFM[21].GetComponent<Text>().text = _t1;
                        }

                        if (_iFMTravelEditE)
                        {
                            _gmObjs_IndivFM[22].GetComponent<Text>().text = "Select Position";
                        }
                        else
                        {
                            string _t1 = (MapManager.Instance.ReturnSector(_iFMEndLoc) < 0) ? ("Empty Space (" + _iFMEndLoc.x + "/" + _iFMEndLoc.y + ")") : MapManager.Instance._map._sectors[MapManager.Instance.ReturnSector(_iFMEndLoc)].GetName(true);
                            _gmObjs_IndivFM[22].GetComponent<Text>().text = _t1;
                        }

                        _gmObjs_IndivFM[15].GetComponent<Text>().text = "Travel Progress:               / " + MapManager.Instance.SectorDistance(_iFMStartLoc, _iFMEndLoc) + " Hexes";

                        if (!_gmObjs_IndivFM[16].GetComponent<InputField>().isFocused)
                        {
                            int _b = MapManager.Instance.SectorDistance(_iFMStartLoc, _iFMEndLoc);

                            if (_b > 0)
                            {
                                _iFMProgress *= _b;
                                _iFMProgress = Mathf.RoundToInt(Mathf.Clamp(_iFMProgress, 0, _b));
                                _iFMProgress /= _b;
                            }
                            else
                            {
                                _iFMProgress = 0;
                            }
                            _gmObjs_IndivFM[16].GetComponent<InputField>().text = (_iFMProgress * MapManager.Instance.SectorDistance(_iFMStartLoc, _iFMEndLoc)).ToString();
                        }


                    }
                    else
                    {
                        _gmObjs_IndivFM[12].SetActive(false);
                        _gmObjs_IndivFM[13].SetActive(false);
                        _gmObjs_IndivFM[14].SetActive(false);
                        _gmObjs_IndivFM[15].SetActive(false);
                        _gmObjs_IndivFM[16].SetActive(false);
                        _gmObjs_IndivFM[17].SetActive(false);
                        _gmObjs_IndivFM[18].SetActive(false);
                        _gmObjs_IndivFM[19].SetActive(false);
                        _gmObjs_IndivFM[20].SetActive(false);
                    }

                    // -- SHIP SECTION --
                    _vHeight += 50;
                    _iFMShipHeader.GetComponent<RectTransform>().localPosition = new Vector3(10, (_vHeight * -1) + 50, -5);
                }

                _indivFleetMenu.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _vHeight);
            }
            else if (_a == 1) // Change Name
            {
                MapManager.Instance._map._fleets[_currentFleetID]._name = _gmObjs_IndivFM[0].GetComponent<InputField>().text;
                RebuildFleetMenu();
            }
            else if (_a == 2) // Change Faction - Context Menu
            {
                _contextMenuOIndex = 0;
                CONTEXT_MENU_O_INIT();
            }
            else if (_a == 3) // Change Sector - Context Menu
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    _contextMenuOIndex = 1;
                    CONTEXT_MENU_O_INIT();
                }
                else
                {
                    _iFMLocationSelect = true;
                }

            }
            else if (_a == 4) // DEPRECATED - DO NOT USE ANYMORE -> LEGACY CHANGE MAX FUEL VALUE
            {
                /* Fleet F = MapManager.Instance._map._fleets[_currentFleetID];
                F._maxFuel = float.Parse(_gmObjs_IndivFM[5].GetComponent<InputField>().text); */
            }
            else if (_a == 5) // CHANGE CURRENT FUEL VALUE
            {
                Fleet F = MapManager.Instance._map._fleets[_currentFleetID];
                F.SetFuelValue(float.Parse(_gmObjs_IndivFM[7].GetComponent<InputField>().text));
            }
            else if (_a == 6) // Change Status - Context Menu
            {
                _contextMenuOIndex = 2;
                CONTEXT_MENU_O_INIT();
            }
            else if (_a == 7) // Delete Fleet
            {
                MapManager.Instance.RemoveFleet(_currentFleetID);
                _currentFleetID = -1;
                _indivFleetIsOn = false;
                if (_contextMenuO.activeSelf)
                {
                    CONTEXT_MENU_O_INIT();
                }
                RebuildFleetMenu();
            }
            else if (_a == 8) // Remove Travel
            {
                MapManager.Instance._map._fleets[_currentFleetID]._travelling = false;
                MapManager.Instance._map._fleets[_currentFleetID]._status = "Idle";
            }
            else if (_a == 9) // Complete Travel
            {
                MapManager.Instance._map._fleets[_currentFleetID]._travelling = false;
                MapManager.Instance._map._fleets[_currentFleetID]._status = "Idle";
                MapManager.Instance._map._fleets[_currentFleetID]._currentSector = MapManager.Instance.ReturnSector(MapManager.Instance._map._fleets[_currentFleetID]._travelEnd);
                MapManager.Instance._map._fleets[_currentFleetID]._travelStart = MapManager.Instance._map._fleets[_currentFleetID]._travelEnd;
            }
            else if (_a == 10) // Close Travel Editing / Clear Edits
            {
                _iFMTravelEdit = false;
                _iFMTravelEditE = false;
                _iFMTravelEditS = false;
            }
            else if (_a == 11) // Confirm Edits
            {
                INDIV_FLEET_FUNCTIONS(10);

                Fleet F = MapManager.Instance._map._fleets[_currentFleetID];
                F._travelStart = _iFMStartLoc;
                F._travelEnd = _iFMEndLoc;
                F._travelCompleted = _iFMProgress;
            }
            else if (_a == 12) // EXIT TRAVEL LOCATION EDITING
            {
                _iFMTravelEditE = false;
                _iFMTravelEditS = false;
            }
            else if (_a == 13) // CONFIRM START LOCATION
            {
                INDIV_FLEET_FUNCTIONS(12);

                _iFMStartLoc = MapManager.Instance.GetMouseSectorPos;
            }
            else if (_a == 14) // CONFIRM END LOCATION
            {
                INDIV_FLEET_FUNCTIONS(12);

                _iFMEndLoc = MapManager.Instance.GetMouseSectorPos;
            }
            else if (_a == 15) // OPEN TRAVEL EDITING
            {
                _iFMTravelEdit = true;

                Fleet F = MapManager.Instance._map._fleets[_currentFleetID];

                _iFMStartLoc = F._travelStart;
                _iFMEndLoc = F._travelEnd;
                _iFMProgress = F._travelCompleted;
            }
            else if (_a == 16) // START START LOCATION EDIT
            {
                _iFMTravelEditS = true;
            }
            else if (_a == 17) // START END LOCATION EDIT
            {
                _iFMTravelEditE = true;
            }
            else if (_a == 18) // ENTER IN ORBIT PROGRESS
            {
                _iFMProgress = (MapManager.Instance.SectorDistance(_iFMStartLoc, _iFMEndLoc) > 0) ? float.Parse(_gmObjs_IndivFM[16].GetComponent<InputField>().text) / MapManager.Instance.SectorDistance(_iFMStartLoc, _iFMEndLoc) : 0;


                int _b = MapManager.Instance.SectorDistance(_iFMStartLoc, _iFMEndLoc);

                if (_b > 0)
                {
                    _iFMProgress *= _b;
                    _iFMProgress = Mathf.RoundToInt(Mathf.Clamp(_iFMProgress, 0, _b));
                    _iFMProgress /= _b;
                    Debug.Log(Mathf.RoundToInt(Mathf.Clamp(_iFMProgress * _b, 0, _b)));
                }
                else
                {
                    _iFMProgress = 0;
                }

            }
            else if (_a == 19) // SWITCH TRANSPONDER
            {
                MapManager.Instance._map._fleets[_currentFleetID]._transponder = !MapManager.Instance._map._fleets[_currentFleetID]._transponder;
            }
            else if (_a == 20) // Change Known To Factions - Context Menu
            {
                _contextMenuOIndex = 3;
                CONTEXT_MENU_O_INIT();
            }
            else if (_a == 21) // Change Known Owner To Factions - Context Menu
            {
                _contextMenuOIndex = 4;
                CONTEXT_MENU_O_INIT();
            }
            else if (_a == 22)
            {
                _iFMLocationSelect = false;

                MapManager.Instance._map._fleets[_currentFleetID]._currentSector = MapManager.Instance.ReturnSector(MapManager.Instance.GetMouseSectorPos);
                RebuildFleetMenu();
            }
            else if (_a == 23)
            {
                _iFMLocationSelect = false;
            }
        }

        public void CONTEXT_MENU_FUNCTIONS(int _a)
        {
            for (int i = 0; i < _contextMenuO_objs.Count; i++)
            {
                GameObject _obj = _contextMenuO_objs[i];
                _contextMenuO_objs.Remove(_contextMenuO_objs[i]);
                Destroy(_obj);
                i--;
            }

            if (_a == 0) // INDIVIDUAL FLEET MENU : FACTIONS ( OWNER )
            {
                _contextMenuO.SetActive(true);

                _contextMenuOIndex = _a;

                // - Start Int for Faction Count -
                int _mObjs = 9;
                if (MapManager.Instance._map._factions.Count <= _mObjs)
                {
                    _contextMenuOInt2 = 0;
                }
                else
                {
                    _contextMenuOInt2 = Mathf.Clamp(Mathf.RoundToInt(_contextMenuO_Scrollbar.value * (MapManager.Instance._map._factions.Count - _mObjs)), 0, MapManager.Instance._map._factions.Count - 1);
                }

                float _verticalHeight = 0;
                // - Title -
                _verticalHeight += 15;

                GameObject _titleObj = Instantiate(_contextMenuO_TextTemplate, _contextMenuO.transform);

                
                Text _t = _titleObj.GetComponent<Text>();
                _t.text = "Factions";
                

                _titleObj.SetActive(true);
                _contextMenuO_objs.Add(_titleObj);

                if (_contextMenuOInt2 == 0)
                {
                    // - ASSIGN -
                    GameObject _b2Obj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                    _b2Obj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t1 = _b2Obj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t1.text = "Neutral";

                    _b2Obj.SetActive(true);
                    _contextMenuO_objs.Add(_b2Obj);
                    _b2Obj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_EVALUATION(_b2Obj));
                }

                for (int i = 0; i < MapManager.Instance._map._factions.Count; i++)
                {
                    if (i >= _contextMenuOInt2 - 1 && i < _contextMenuOInt2 + _mObjs)
                    {
                        // - ASSIGN -
                        GameObject _bObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                        _bObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                        _verticalHeight += 6;

                        Text _t2 = _bObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        _t2.text = MapManager.Instance._map._factions[i]._shorthand;

                        _bObj.SetActive(true);
                        _contextMenuO_objs.Add(_bObj);
                        _bObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_EVALUATION(_bObj));
                    }
                        
                }
                    

                // - CLOSE -
                GameObject _cObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                _cObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t7 = _cObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                _t7.text = "Close";

                _cObj.SetActive(true);
                _contextMenuO_objs.Add(_cObj);
                _cObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_INIT());

                _contextMenuO_Scrollbar.transform.SetAsLastSibling();
                _contextMenuO_Scrollbar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);


                _contextMenuO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
            }
            if (_a == 1) // INDIVIDUAL FLEET MENU : LOCATIONS ( SECTOR )
            {
                _contextMenuO.SetActive(true);

                _contextMenuOIndex = _a;

                // - Start Int for Faction Count -
                int _mObjs = 9;
                if (MapManager.Instance._map._sectors.Count <= _mObjs)
                {
                    _contextMenuOInt2 = 0;
                }
                else
                {
                    _contextMenuOInt2 = Mathf.Clamp(Mathf.RoundToInt(_contextMenuO_Scrollbar.value * (MapManager.Instance._map._sectors.Count - _mObjs)), 0, MapManager.Instance._map._sectors.Count - 1);
                }

                float _verticalHeight = 0;
                // - Title -
                _verticalHeight += 15;

                GameObject _titleObj = Instantiate(_contextMenuO_TextTemplate, _contextMenuO.transform);


                Text _t = _titleObj.GetComponent<Text>();
                _t.text = "Sectors";


                _titleObj.SetActive(true);
                _contextMenuO_objs.Add(_titleObj);

                if (_contextMenuOInt2 == 0)
                {
                    // - ASSIGN -
                    GameObject _b2Obj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                    _b2Obj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                    _verticalHeight += 6;

                    Text _t1 = _b2Obj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    _t1.text = "Empty Space";

                    _b2Obj.SetActive(true);
                    _contextMenuO_objs.Add(_b2Obj);
                    _b2Obj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_EVALUATION(_b2Obj));
                }

                for (int i = 0; i < MapManager.Instance._map._sectors.Count; i++)
                {
                    if (i >= _contextMenuOInt2 - 1 && i < _contextMenuOInt2 + _mObjs)
                    {
                        // - ASSIGN -
                        GameObject _bObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                        _bObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                        _verticalHeight += 6;

                        Text _t2 = _bObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        _t2.text = MapManager.Instance._map._sectors[i].GetName(true);

                        _bObj.SetActive(true);
                        _contextMenuO_objs.Add(_bObj);
                        _bObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_EVALUATION(_bObj));
                    }

                }


                // - CLOSE -
                GameObject _cObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                _cObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t7 = _cObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                _t7.text = "Close";

                _cObj.SetActive(true);
                _contextMenuO_objs.Add(_cObj);
                _cObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_INIT());

                _contextMenuO_Scrollbar.transform.SetAsLastSibling();
                _contextMenuO_Scrollbar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);


                _contextMenuO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
            }
            if (_a == 2) // INDIVIDUAL FLEET MENU : STATUS
            {
                _contextMenuO.SetActive(true);

                _contextMenuOIndex = _a;

                // - Start Int for Faction Count -
                int _mObjs = 9;
                List<string> _statusList = new List<string>() { "Idle", "Travelling", "Moving", "Combat", "Repairing", "Refueling" };

                if (_statusList.Count <= _mObjs)
                {
                    _contextMenuOInt2 = 0;
                }
                else
                {
                    _contextMenuOInt2 = Mathf.Clamp(Mathf.RoundToInt(_contextMenuO_Scrollbar.value * (_statusList.Count - _mObjs)), 0, _statusList.Count - 1);
                }

                float _verticalHeight = 0;
                // - Title -
                _verticalHeight += 15;

                GameObject _titleObj = Instantiate(_contextMenuO_TextTemplate, _contextMenuO.transform);


                Text _t = _titleObj.GetComponent<Text>();
                _t.text = "Status";


                _titleObj.SetActive(true);
                _contextMenuO_objs.Add(_titleObj);

                

                for (int i = 0; i < _statusList.Count; i++)
                {
                    if (i >= _contextMenuOInt2 && i < _contextMenuOInt2 + _mObjs)
                    {
                        // - ASSIGN -
                        GameObject _bObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                        _bObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                        _verticalHeight += 6;

                        Text _t2 = _bObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        _t2.text = _statusList[i];

                        _bObj.SetActive(true);
                        _contextMenuO_objs.Add(_bObj);
                        _bObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_EVALUATION(_bObj));
                    }

                }


                // - CLOSE -
                GameObject _cObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                _cObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t7 = _cObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                _t7.text = "Close";

                _cObj.SetActive(true);
                _contextMenuO_objs.Add(_cObj);
                _cObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_INIT());

                _contextMenuO_Scrollbar.transform.SetAsLastSibling();
                _contextMenuO_Scrollbar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);


                _contextMenuO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
            }
            if (_a == 3) // INDIVIDUAL FLEET MENU : KNOWN TO FACTIONS
            {
                _contextMenuO.SetActive(true);

                _contextMenuOIndex = _a;

                // - Start Int for Faction Count -
                int _mObjs = 9;
                if (MapManager.Instance._map._factions.Count <= _mObjs)
                {
                    _contextMenuOInt2 = 0;
                }
                else
                {
                    _contextMenuOInt2 = Mathf.Clamp(Mathf.RoundToInt(_contextMenuO_Scrollbar.value * (MapManager.Instance._map._factions.Count - _mObjs)), 0, MapManager.Instance._map._factions.Count - 1);
                }

                float _verticalHeight = 0;
                // - Title -
                _verticalHeight += 15;

                GameObject _titleObj = Instantiate(_contextMenuO_TextTemplate, _contextMenuO.transform);


                Text _t = _titleObj.GetComponent<Text>();
                _t.text = "Factions";


                _titleObj.SetActive(true);
                _contextMenuO_objs.Add(_titleObj);

                for (int i = 0; i < MapManager.Instance._map._factions.Count; i++)
                {
                    if (i >= _contextMenuOInt2 && i < _contextMenuOInt2 + _mObjs)
                    {
                        // - ASSIGN -
                        GameObject _bObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                        _bObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                        _verticalHeight += 6;

                        Text _t2 = _bObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        _t2.text = MapManager.Instance._map._factions[i]._shorthand;

                        _bObj.SetActive(true);
                        _contextMenuO_objs.Add(_bObj);
                        _bObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_EVALUATION(_bObj));

                        if (MapManager.Instance.Fleet_IsInKnownList(_currentFleetID, i))
                        {
                            _bObj.GetComponent<Image>().color = new Color32(118, 118, 118, 255);
                        }
                        else
                        {
                            _bObj.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                        }
                    }

                }


                // - CLOSE -
                GameObject _cObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                _cObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t7 = _cObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                _t7.text = "Close";

                _cObj.SetActive(true);
                _contextMenuO_objs.Add(_cObj);
                _cObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_INIT());

                _contextMenuO_Scrollbar.transform.SetAsLastSibling();
                _contextMenuO_Scrollbar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);


                _contextMenuO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
            }
            if (_a == 4) // INDIVIDUAL FLEET MENU : KNOWN OWNER TO FACTIONS
            {
                _contextMenuO.SetActive(true);

                _contextMenuOIndex = _a;

                // - Start Int for Faction Count -
                int _mObjs = 9;
                if (MapManager.Instance._map._factions.Count <= _mObjs)
                {
                    _contextMenuOInt2 = 0;
                }
                else
                {
                    _contextMenuOInt2 = Mathf.Clamp(Mathf.RoundToInt(_contextMenuO_Scrollbar.value * (MapManager.Instance._map._factions.Count - _mObjs)), 0, MapManager.Instance._map._factions.Count - 1);
                }

                float _verticalHeight = 0;
                // - Title -
                _verticalHeight += 15;

                GameObject _titleObj = Instantiate(_contextMenuO_TextTemplate, _contextMenuO.transform);


                Text _t = _titleObj.GetComponent<Text>();
                _t.text = "Factions";


                _titleObj.SetActive(true);
                _contextMenuO_objs.Add(_titleObj);

                for (int i = 0; i < MapManager.Instance._map._factions.Count; i++)
                {
                    if (i >= _contextMenuOInt2 && i < _contextMenuOInt2 + _mObjs)
                    {
                        // - ASSIGN -
                        GameObject _bObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                        _bObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                        _verticalHeight += 6;

                        Text _t2 = _bObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        _t2.text = MapManager.Instance._map._factions[i]._shorthand;

                        _bObj.SetActive(true);
                        _contextMenuO_objs.Add(_bObj);
                        _bObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_EVALUATION(_bObj));

                        if (MapManager.Instance.Fleet_IsInKnownOwnerList(_currentFleetID, i))
                        {
                            _bObj.GetComponent<Image>().color = new Color32(118, 118, 118, 255);
                        }
                        else
                        {
                            _bObj.GetComponent<Image>().color = new Color32(90, 90, 90, 255);
                        }
                    }

                }


                // - CLOSE -
                GameObject _cObj = Instantiate(_contextMenuO_ButtonTemplate, _contextMenuO.transform);
                _cObj.transform.localPosition = new Vector3(26.5f, (_verticalHeight * -1) + 6, 0);
                _verticalHeight += 6;

                Text _t7 = _cObj.GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                _t7.text = "Close";

                _cObj.SetActive(true);
                _contextMenuO_objs.Add(_cObj);
                _cObj.GetComponent<Button>().onClick.AddListener(() => CONTEXT_MENU_O_INIT());

                _contextMenuO_Scrollbar.transform.SetAsLastSibling();
                _contextMenuO_Scrollbar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);


                _contextMenuO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _verticalHeight);
            }

        }

        public void CONTEXT_MENU_O_EVALUATION(GameObject _obj)
        {

            if (_contextMenuOIndex < 0)
            {
                CONTEXT_MENU_O_INIT();
                return;
            }

            if (_contextMenuOIndex == 0) // ASSIGN FACTION TO FLEET IN INDIV FLEET MENU
            {
                int _a = -1;
                for (int i = 0; i < _contextMenuO_objs.Count; i++)
                {
                    if (_obj == _contextMenuO_objs[i])
                    {
                        _a = (i - 2) + _contextMenuOInt2; // -2 because Title is also in list.
                    } 
                }
                MapManager.Instance._map._fleets[_currentFleetID]._faction = _a;
            }
            else if (_contextMenuOIndex == 1) // ASSIGN LOCATION TO FLEET IN INDIV FLEET MENU
            {
                int _a = -1;
                for (int i = 0; i < _contextMenuO_objs.Count; i++)
                {
                    if (_obj == _contextMenuO_objs[i])
                    {
                        _a = (i - 2) + _contextMenuOInt2; // -2 because Title is also in list.
                    }
                }
                MapManager.Instance._map._fleets[_currentFleetID]._currentSector = _a;
            }
            else if (_contextMenuOIndex == 2) // ASSIGN STATUS TO FLEET IN INDIV FLEET MENU
            {
                int _a = -1;
                List<string> _statusList = new List<string>() { "Idle", "Travelling", "Moving", "Combat", "Repairing", "Refueling" };
                for (int i = 0; i < _contextMenuO_objs.Count; i++)
                {
                    if (_obj == _contextMenuO_objs[i])
                    {
                        _a = (i - 1) + _contextMenuOInt2; 
                    }
                }
                MapManager.Instance._map._fleets[_currentFleetID]._status = _statusList[_a];
                if (_statusList[_a] == "Travelling")
                {
                    MapManager.Instance._map._fleets[_currentFleetID]._travelling = true;
                }
            }
            else if (_contextMenuOIndex == 3) // FLEET IN KNOWN LIST
            {
                int _a = -1;
                
                for (int i = 0; i < _contextMenuO_objs.Count; i++)
                {
                    if (_obj == _contextMenuO_objs[i])
                    {
                        _a = (i - 1) + _contextMenuOInt2;
                    }
                }
                if (MapManager.Instance.Fleet_IsInKnownList(_currentFleetID, _a))
                {
                    for (int i = 0; i < MapManager.Instance._map._factions[_a]._knownFleets.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_a]._knownFleets[i] == _currentFleetID)
                        {
                            MapManager.Instance._map._factions[_a]._knownFleets.Remove(MapManager.Instance._map._factions[_a]._knownFleets[i]);
                            i--;
                        }
                    }
                }
                else
                {
                    MapManager.Instance._map._factions[_a]._knownFleets.Add(_currentFleetID);
                }

                RebuildFleetMenu();
                CONTEXT_MENU_FUNCTIONS(_contextMenuOIndex);
                return;
            }
            else if (_contextMenuOIndex == 4) // FLEET IN KNOWN OWNER LIST
            {
                int _a = -1;

                for (int i = 0; i < _contextMenuO_objs.Count; i++)
                {
                    if (_obj == _contextMenuO_objs[i])
                    {
                        _a = (i - 1) + _contextMenuOInt2;
                    }
                }
                if (MapManager.Instance.Fleet_IsInKnownOwnerList(_currentFleetID, _a))
                {
                    for (int i = 0; i < MapManager.Instance._map._factions[_a]._knownFleetOwners.Count; i++)
                    {
                        if (MapManager.Instance._map._factions[_a]._knownFleetOwners[i] == _currentFleetID)
                        {
                            MapManager.Instance._map._factions[_a]._knownFleetOwners.Remove(MapManager.Instance._map._factions[_a]._knownFleetOwners[i]);
                            i--;
                        }
                    }
                }
                else
                {
                    MapManager.Instance._map._factions[_a]._knownFleetOwners.Add(_currentFleetID);
                }

                RebuildFleetMenu();
                CONTEXT_MENU_FUNCTIONS(_contextMenuOIndex);
                return;
            }



            CONTEXT_MENU_O_INIT();
            RebuildFleetMenu();
        }

        public void CONTEXT_MENU_O_INIT()
        {
            if (_contextMenuO.activeSelf)
            {
                _contextMenuO.SetActive(false);
                for (int i = 0; i < _contextMenuO_objs.Count; i++)
                {
                    GameObject _obj = _contextMenuO_objs[i];
                    _contextMenuO_objs.Remove(_contextMenuO_objs[i]);
                    Destroy(_obj);
                    i--;
                }
                _contextMenuOIndex = -1;
                return;
            }
            else
            {
                for (int i = 0; i < _contextMenuO_objs.Count; i++)
                {
                    GameObject _obj = _contextMenuO_objs[i];
                    _contextMenuO_objs.Remove(_contextMenuO_objs[i]);
                    Destroy(_obj);
                    i--;
                }

                // - Place Context Menu -
                _contextMenuO.transform.localPosition = new Vector3((Input.mousePosition.x - Screen.width / 2), (Input.mousePosition.y - Screen.height / 2), 0);

                

                _contextMenuO.SetActive(true);
                CONTEXT_MENU_FUNCTIONS(_contextMenuOIndex);
            }
        }

        public void CONTEXT_MENU_O_SCROLLBAR()
        {
            if (_contextMenuO_objs.Count > 0)
            {
                CONTEXT_MENU_FUNCTIONS(_contextMenuOIndex);
            }
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
            float _sFactorWidth = 2560f / Screen.width;
            float _sFactorHeight = 1440f / Screen.height;

            // MOVE OUTLINER MAIN MENU IF LOCK SELECTION IS ON
            if (MapManager.Instance._map._lockSelection)
            {
                _title.GetComponent<RectTransform>().localPosition = new Vector3(10, -170, -5) - new Vector3(Screen.width/2 * _sFactorWidth, -Screen.height/2 * _sFactorHeight);
                _fleetButton.GetComponent<RectTransform>().localPosition = new Vector3(10, -200, -5) - new Vector3(Screen.width / 2 * _sFactorWidth, -Screen.height / 2 * _sFactorHeight);
                _AddButton.SetActive(false);
            }
            else
            {
                
                _title.GetComponent<RectTransform>().localPosition = new Vector3(10, -550, -5) - new Vector3(Screen.width / 2 * _sFactorWidth, -Screen.height / 2 * _sFactorHeight);
                _fleetButton.GetComponent<RectTransform>().localPosition = new Vector3(10, -580, -5) - new Vector3(Screen.width / 2 * _sFactorWidth, -Screen.height / 2 * _sFactorHeight);

                _AddButton.SetActive(true);
            }

            if (_indivFleetIsOn)
            {
                _indivFleetMenu.SetActive(true);

                INDIV_FLEET_FUNCTIONS(0);

                if (_iFMTravelEdit)
                {
                    if (_iFMTravelEditS && Input.GetMouseButtonDown(0))
                    {
                        INDIV_FLEET_FUNCTIONS(13);
                    }
                    else if (_iFMTravelEditS && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
                    {
                        INDIV_FLEET_FUNCTIONS(12);
                    }

                    if (_iFMTravelEditE && Input.GetMouseButtonDown(0))
                    {
                        INDIV_FLEET_FUNCTIONS(14);
                    }
                    else if (_iFMTravelEditE && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
                    {
                        INDIV_FLEET_FUNCTIONS(12);
                    }
                }

                if (_iFMLocationSelect && Input.GetMouseButtonDown(0))
                {
                    INDIV_FLEET_FUNCTIONS(22);
                }
                else if (_iFMLocationSelect && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)))
                {
                    INDIV_FLEET_FUNCTIONS(23);
                }
            }
            else
            {
                _indivFleetMenu.SetActive(false);
            }

            

            if (_contextMenuO.activeInHierarchy)
            {
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape) || !_indivFleetIsOn)
                {
                    CONTEXT_MENU_O_INIT();
                }
            }
        }
    }
}

