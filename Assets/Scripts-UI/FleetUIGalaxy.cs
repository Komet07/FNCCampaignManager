using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{

    [System.Serializable]
    public class FleetToken
    {
        public GameObject _obj;
        public int _fleetID;
    }

    public class FleetUIGalaxy : MonoBehaviour
    {

        #region Singleton
        public static FleetUIGalaxy Instance;
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        public List<FleetToken> _fleets = new List<FleetToken>() { };

        public GameObject _fleetBaseObj;

        public void CreateNewFleetToken(int _fleetID)
        {
            FleetToken F = new FleetToken();

            GameObject _obj = Instantiate(_fleetBaseObj, _fleetBaseObj.transform.parent);
            _obj.SetActive(true);
            F._obj = _obj;
            F._fleetID = _fleetID;

            _fleets.Add(F);
        }

        public void RemoveFleet(int _fleetID)
        {
            for (int i = 0; i < _fleets.Count; i++)
            {
                if (_fleets[i]._fleetID == _fleetID)
                {
                    RemoveFleetToken(i);

                    i--;
                }
            }
        }

        public void RemoveFleetToken(int _id)
        {
            Destroy(_fleets[_id]._obj);
            _fleets.Remove(_fleets[_id]);
        }

        public void ProcessRemovedID(int _id)
        {
            for (int i = 0; i < _fleets.Count; i++)
            {
                if (_fleets[i]._fleetID == _id)
                {
                    RemoveFleetToken(i);

                    i--;
                }
                else if (_fleets[i]._fleetID > _id)
                {
                    _fleets[i]._fleetID--;
                }
            }
        }

        public bool isFleetPresent(int _id)
        {
            for (int i = 0; i < _fleets.Count; i++)
            {
                if (_fleets[i]._fleetID == _id)
                {
                    return true;
                }
            }

            return false;
        }

        public int findFleet(int _id)
        {
            for (int i = 0; i < _fleets.Count; i++)
            {
                if (_fleets[i]._fleetID == _id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void UpdateMap()
        {
            // - SPAWN AND REMOVE FLEET TOKENS
            for (int i = 0; i < MapManager.Instance._map._fleets.Count; i++)
            {
                if (isFleetPresent(i)) // TOKEN PRESENT
                {
                    if (!MapManager.Instance.Fleet_IsVisible(i)) // IF FLEET ISN'T VISIBLE
                    {
                        RemoveFleet(i);
                    }
                }
                else // TOKEN NOT PRESENT
                {
                    if (MapManager.Instance.Fleet_IsVisible(i)) // VISIBLE FLEET
                    {
                        CreateNewFleetToken(i);
                    }
                }
            }
            List<int> _sIDs = new List<int>() { };
            List<int> _sFrequency = new List<int>() { };

            // - PROCESS FLEETS -
            for (int i = 0; i < _fleets.Count; i++)
            {
                int _fID = _fleets[i]._fleetID;
                GameObject _token = _fleets[i]._obj;
                
                if (_fID < 0 || _fID >= MapManager.Instance._map._fleets.Count)
                {   
                    
                    RemoveFleet(_fID); // REMOVE FLEET IF IT ISN'T THERE
                    return;
                }

                Fleet F = MapManager.Instance._map._fleets[_fID];

                if (!F._travelling)
                {
                    if (F._currentSector < 0)
                    {
                        _token.transform.position = new Vector3(0, 0, 0);
                        _token.SetActive(false);
                    }
                    else
                    {
                        _token.SetActive(true);
                        Sector S = MapManager.Instance._map._sectors[F._currentSector];
                        _token.transform.position = MapManager.Instance.TurnSectorIntoRealPos(new Vector2(S._posXInt, S._posYInt));
                        _token.transform.position += new Vector3(0, 0, -5);

                        bool isPresent = false;
                        int _count = 0;
                        for (int j = 0; j < _sIDs.Count; j++)
                        {
                            if (_sIDs[j] == F._currentSector)
                            {
                                isPresent = true;
                                _sFrequency[j]++;
                                _count = _sFrequency[j];
                            }
                        }

                        if (!isPresent)
                        {
                            _sIDs.Add(F._currentSector);
                            _sFrequency.Add(1);
                            _count = 1;
                        }

                        if (_count <= 6)
                        {
                            _token.transform.position += new Vector3(Mathf.Cos((360 / 6 * _count-1)*Mathf.Deg2Rad) * .095f, Mathf.Sin((360 / 6 * _count-1) * Mathf.Deg2Rad) * .095f, 0);
                        }
                        else if (_count <= 18)
                        {
                            _token.transform.position += new Vector3(Mathf.Cos((360 / 12 * (_count-7)) * Mathf.Deg2Rad) * .19f, Mathf.Sin((360 / 12 * (_count - 7)) * Mathf.Deg2Rad) * .19f, 0);
                        }
                    }

                    _token.GetComponent<IndexScript>()._obj1.SetActive(false);
                }
                else
                {
                    _token.SetActive(true);

                    Vector2 _p1 = MapManager.Instance.TurnSectorIntoRealPos(F._travelStart);
                    Vector2 _p2 = MapManager.Instance.TurnSectorIntoRealPos(F._travelEnd) - _p1;

                    Vector2 _p3 = _p1 + (_p2 * F._travelCompleted);

                    _token.transform.position = _p3;
                    _token.transform.position += new Vector3(0, 0, -5);

                    // Line Renderer Positioning
                    LineRenderer LR = _token.GetComponent<IndexScript>()._obj1.GetComponent<LineRenderer>();
                    LR.transform.position = new Vector3(0, 0, 5);
                    _token.GetComponent<IndexScript>()._obj1.SetActive(true);

                    LR.SetPosition(0, new Vector3(_p3.x, _p3.y, -3.75f));
                    LR.SetPosition(1, new Vector3(_p1.x + _p2.x, _p1.y + _p2.y, -3.75f));

                    LR.GetComponent<IndexScript>()._obj1.transform.position = _p3;
                    LR.GetComponent<IndexScript>()._obj1.transform.position += new Vector3(0, 0, -4);
                    LR.GetComponent<IndexScript>()._obj2.transform.position = _p1 + _p2;
                    LR.GetComponent<IndexScript>()._obj2.transform.position += new Vector3(0, 0, -4);

                    // Obfuscation purposes
                    if (MapManager.Instance._map._playerFactionId > -1 && (MapManager.Instance._map._playerFactionId < MapManager.Instance._map._playerFactions.Count ? MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId]._regFactionID != F._faction : true))
                    {
                        Gradient gradient = new Gradient();
                        gradient.SetKeys(
                            new GradientColorKey[] { new GradientColorKey(LR.startColor, 0.0f), new GradientColorKey(LR.startColor, 1.0f) },
                            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0f, 0.5f), new GradientAlphaKey(0f, 1.0f) }
                        );
                        LR.colorGradient = gradient;

                        LR.GetComponent<IndexScript>()._obj2.SetActive(false);

                        Vector2 _p2Altered = _p3 + _p2.normalized * 0.5f;
                        LR.SetPosition(1, new Vector3(_p2Altered.x, _p2Altered.y, -3.75f));
                    }
                    else
                    {
                        Gradient gradient = new Gradient();
                        gradient.SetKeys(
                            new GradientColorKey[] { new GradientColorKey(LR.startColor, 0.0f), new GradientColorKey(LR.startColor, 1.0f) },
                            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1f, 1.0f) }
                        );
                        LR.colorGradient = gradient;

                        LR.GetComponent<IndexScript>()._obj2.SetActive(true);
                    }
                }

                string _textPrefix = "";

                // SET COLOR
                if (GalaxyMap.Instance._viewMode == "factions" && MapManager.Instance.Fleet_IsOwnerKnown(_fID))
                {
                    if (F._faction >= 0 && F._faction < MapManager.Instance._map._factions.Count)
                    {
                        _token.GetComponent<Image>().color = MapManager.Instance._map._factions[F._faction]._factionColor;
                        _textPrefix = MapManager.Instance._map._factions[F._faction]._shorthand;
                    }
                    else
                    {
                        _textPrefix = "NEU";
                        _token.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    }
                        
                }
                else if (GalaxyMap.Instance._viewMode == "alliances" && MapManager.Instance.Fleet_IsOwnerKnown(_fID))
                {
                    if (F._faction >= 0 && F._faction < MapManager.Instance._map._factions.Count)
                    {
                        if (MapManager.Instance._map._factions[F._faction]._allianceId >= 0 && MapManager.Instance._map._factions[F._faction]._allianceId < MapManager.Instance._map._alliances.Count)
                        {
                            _token.GetComponent<Image>().color = MapManager.Instance._map._alliances[MapManager.Instance._map._factions[F._faction]._allianceId]._allianceColor;
                            _textPrefix = MapManager.Instance._map._alliances[MapManager.Instance._map._factions[F._faction]._allianceId]._shorthand;
                        }
                        else
                        {
                            _textPrefix = "NEU";
                            _token.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                        }
                    }
                    else
                    {
                        _textPrefix = "NEU";
                        _token.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                    }
                }
                else if (GalaxyMap.Instance._viewMode == "relations" && MapManager.Instance.Fleet_IsOwnerKnown(_fID))
                {
                    if (MapManager.Instance._map._playerFactionId >= 0)
                    {
                        PlayerFaction _pF = MapManager.Instance._map._playerFactions[MapManager.Instance._map._playerFactionId];

                        if (_pF._regFactionID >= 0 && _pF._regFactionID != F._faction && F._faction > -1)
                        {
                            float _val = 0;
                            string _specCon = "";
                            for (int j = 0; j < MapManager.Instance._map._factions[_pF._regFactionID]._repIds.Count; j++)
                            {
                                if (MapManager.Instance._map._reps[MapManager.Instance._map._factions[_pF._regFactionID]._repIds[j]]._faction1 == F._faction || MapManager.Instance._map._reps[MapManager.Instance._map._factions[_pF._regFactionID]._repIds[j]]._faction2 == F._faction)
                                {
                                    _val = MapManager.Instance._map._reps[MapManager.Instance._map._factions[_pF._regFactionID]._repIds[j]]._repVal;
                                    _specCon = MapManager.Instance._map._reps[MapManager.Instance._map._factions[_pF._regFactionID]._repIds[j]]._specialVal;
                                }
                            }
                            Color32 _col = new Color32(0, 0, 0, 255);
                            string _repName = "";
                            GalaxyMap.Instance.DetermineRelationsColor(_val, _specCon, out _col, out _repName);
                            _token.GetComponent<Image>().color = _col;
                            _textPrefix = _repName;

                            Debug.Log(_repName);
                        }
                        else
                        {
                            if (_pF._regFactionID >= 0 && _pF._regFactionID == F._faction && F._faction > -1)
                            {
                                _token.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                                _textPrefix = "OWN";
                            }
                            else
                            {
                                _token.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                                _textPrefix = "NEU";
                            }
                        }
                    }
                    else
                    {
                        if (GalaxyMap.Instance._selFacInt >= 0 && GalaxyMap.Instance._selFacInt < MapManager.Instance._map._factions.Count) 
                        {
                            if (F._faction != -1 && F._faction != GalaxyMap.Instance._selFacInt)
                            {
                                float _val = 0;
                                string _specCon = "";
                                for (int j = 0; j < MapManager.Instance._map._factions[GalaxyMap.Instance._selFacInt]._repIds.Count; j++)
                                {
                                    if (MapManager.Instance._map._reps[MapManager.Instance._map._factions[GalaxyMap.Instance._selFacInt]._repIds[j]]._faction1 == F._faction || MapManager.Instance._map._reps[MapManager.Instance._map._factions[GalaxyMap.Instance._selFacInt]._repIds[j]]._faction2 == F._faction)
                                    {
                                        _val = MapManager.Instance._map._reps[MapManager.Instance._map._factions[GalaxyMap.Instance._selFacInt]._repIds[j]]._repVal;
                                        _specCon = MapManager.Instance._map._reps[MapManager.Instance._map._factions[GalaxyMap.Instance._selFacInt]._repIds[j]]._specialVal;
                                    }
                                }
                                Color32 _col;
                                string _repName;
                                GalaxyMap.Instance.DetermineRelationsColor(_val, _specCon, out _col, out _repName);
                                _token.GetComponent<Image>().color = _col;
                                _textPrefix = _repName;
                            }
                            else
                            {
                                if (F._faction == GalaxyMap.Instance._selFacInt)
                                {
                                    _token.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                                    _textPrefix = "Owned";
                                }
                                else
                                {
                                    _token.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                                    _textPrefix = "NEU";
                                }

                            }
                        }
                        else
                        {
                            _textPrefix = "/";
                            _token.GetComponent<Image>().color = new Color32(175, 175, 175, 255);
                        }
                    }
                }
                if (GalaxyMap.Instance._viewMode == "special_SectorVisibility")
                {
                    _token.GetComponent<Image>().color = new Color32(150, 150, 150, 255);
                }
                else if (!MapManager.Instance.Fleet_IsOwnerKnown(_fID))
                {
                    _token.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
                    _textPrefix = "UNK";
                }
            }
        }



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateMap();
        }
    }
}

