using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SystemMap
{
    public class SystemMapUI : MonoBehaviour
    {
        #region Singleton
        public static SystemMapUI Instance;
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        [Header("Meta"), SerializeField]
        bool _regen = false;
        public float _power = .5f;

        [Header("Origin Objects")]
        public GameObject _starOrigin;
        public GameObject _cBOrigin;

        public Transform _starParent;
        public Transform _cBParent;

        [Header("Object Lists")]
        public List<GameObject> _stars = new List<GameObject>();
        public List<GameObject> _celestialBodies = new List<GameObject>();

        
        public enum viewMode {Default, Faction, Alliance};

        [Header("UI Options"), SerializeField]
        public viewMode _viewMode = viewMode.Default;

        public void RegenMap()
        {
            // CHECK IF LEGITIMATE SECTORS PRESENT
            if (MapManager.Instance._selectedStarSystem == -1)
            {
                MapManager.Instance.TurnOnGalaxyMap();
                return;
            }

            if (MapManager.Instance._map._sectors.Count == 0)
            {
                MapManager.Instance.TurnOnGalaxyMap();
                MapManager.Instance._selectedStarSystem = -1;
                return;
            }

            // CLAMP STAR SYSTEM VALUE TO ACCEPTABLE RANGE
            MapManager.Instance._selectedStarSystem = Mathf.Clamp(MapManager.Instance._selectedStarSystem, 0, MapManager.Instance._map._sectors.Count - 1);

            // REFERENCE VARIABLES
            int _a = MapManager.Instance._selectedStarSystem;

            // CLEAR OUT OLD OBJECTS
            // - STARS -
            while (_stars.Count > 0)
            {
                GameObject _obj = _stars[_stars.Count - 1];
                Destroy(_obj);

                _stars.Remove(_stars[_stars.Count - 1]);
            }

            // - CELESTIAL BODIES -
            while (_celestialBodies.Count > 0)
            {
                GameObject _obj = _celestialBodies[_celestialBodies.Count - 1];
                Destroy(_obj);

                _celestialBodies.Remove(_celestialBodies[_celestialBodies.Count - 1]);
            }

            // SPAWN NEW OBJECTS
            // - Stars -
            for (int i = 0; i < MapManager.Instance._map._sectors[_a]._system._stars.Count; i++)
            {
                // -- Instantiate object --
                GameObject _obj = Instantiate(_starOrigin, _starParent);

                // -- Turn on --
                _obj.SetActive(true);

                // -- Initialize Orbit Position --
                OrbitParams _orb = MapManager.Instance._map._sectors[_a]._system._stars[i]._orbit;
                if (_orb._radius > 0)
                {
                    float _b = _orb.CurrentPosition;
                    float _c = _orb.CurrentInclination(_b) + 90;
                    float _d = Mathf.Pow(_orb._radius, _power);

                    float _x = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Cos(_b * 360 * Mathf.Deg2Rad);
                    float _y = _d * Mathf.Cos(_c * Mathf.Deg2Rad);
                    float _z = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Sin(_b * 360 * Mathf.Deg2Rad);

                    _obj.transform.position = new Vector3(_x, _y, _z);
                }
                else
                {
                    _obj.transform.position = new Vector3(0, 0, 0);
                }
                    

                // -- Add to list --
                _stars.Add(_obj);

            }

            // - Celestial Bodies -
            for (int i = 0; i < MapManager.Instance._map._sectors[_a]._system._cB.Count; i++)
            {
                // -- Instantiate object --
                GameObject _obj = Instantiate(_cBOrigin, _cBParent);

                // -- Set Active --
                _obj.SetActive(true);




                // -- Get Parent Position --
                OrbitParams _orb = MapManager.Instance._map._sectors[_a]._system._cB[i]._orbit;

                float _modifierB = (!_orb._orbitingStar && _orb._parentInt != -1) ? 2 : 1;
                _obj.transform.localScale = _obj.transform.localScale / _modifierB;

                bool _flag = false;
                int _curInt = i;
                List<int> _alreadyDone = new List<int>() {i};
                Vector3 _parentPos = new Vector3(0, 0, 0);
                while (!_flag)
                {
                    _alreadyDone.Add(_curInt);
                    _orb = MapManager.Instance._map._sectors[_a]._system._cB[_curInt]._orbit;
                    _curInt = _orb._parentInt;

                    // -- If Orbiting Star, then last run --
                    if (_orb._orbitingStar)
                    {

                        if (_curInt > MapManager.Instance._map._sectors[_a]._system._stars.Count)
                        {
                            _curInt = -1;
                            _orb._parentInt = -1;
                        }

                        if (_curInt == -1)
                        {
                            _flag = true;
                            break;
                        }

                        OrbitParams _orb2 = MapManager.Instance._map._sectors[_a]._system._stars[_curInt]._orbit;

                        if (_orb2._radius > 0)
                        {
                            float _b = _orb2.CurrentPosition;
                            float _c = _orb2.CurrentInclination(_b) + 90;
                            float _d = Mathf.Pow(_orb2._radius, _power);

                            float _x = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Cos(_b * 360 * Mathf.Deg2Rad);
                            float _y = _d * Mathf.Cos(_c * Mathf.Deg2Rad);
                            float _z = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Sin(_b * 360 * Mathf.Deg2Rad);

                            _parentPos += new Vector3(_x, _y, _z);
                        }

                        _flag = true;
                        break;
                    }

                    
                    

                    for (int j = 0; j < _alreadyDone.Count; j++)
                    {
                        if (_curInt == _alreadyDone[j])
                        {
                            _curInt = -1;
                        }
                    }

                    if (_curInt > MapManager.Instance._map._sectors[_a]._system._cB.Count - 1)
                    {
                        _curInt = -1;
                        _orb._parentInt = -1;
                    }

                    if (_curInt != -1)
                    {
                        OrbitParams _orb2 = MapManager.Instance._map._sectors[_a]._system._cB[_curInt]._orbit;
                        if (_orb2._radius > 0)
                        {
                            float _b = _orb2.CurrentPosition;
                            float _c = _orb2.CurrentInclination(_b) + 90;
                            float _d = Mathf.Pow(_orb2._radius, _power);

                            float _x = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Cos(_b * 360 * Mathf.Deg2Rad);
                            float _y = _d * Mathf.Cos(_c * Mathf.Deg2Rad);
                            float _z = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Sin(_b * 360 * Mathf.Deg2Rad);

                            _parentPos += new Vector3(_x, _y, _z);
                        }
                    }
                    else
                    {
                        _flag = true;
                        break;
                    }

                }

                _orb = MapManager.Instance._map._sectors[_a]._system._cB[i]._orbit;

                // -- Initialize Orbit Position --
                if (_orb._radius <= 0)
                {
                    _obj.transform.position = new Vector3(0, 0, 0);
                }
                else
                {
                    float _b = _orb.CurrentPosition;
                    float _c = _orb.CurrentInclination(_b) + 90;
                    float _d = Mathf.Pow(_orb._radius, _power);

                    float _x = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Cos(_b * 360 * Mathf.Deg2Rad);
                    float _y = _d * Mathf.Cos(_c * Mathf.Deg2Rad);
                    float _z = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Sin(_b * 360 * Mathf.Deg2Rad);

                    float _modifier = (!_orb._orbitingStar && _orb._parentInt != -1) ? 2 : 1;
                    _obj.transform.position = new Vector3(_x, _y, _z) * _modifier + _parentPos;
                }

                // -- Initialize Orbit LineRenderer --
                if (_orb._radius > 0)
                {
                    LineRenderer _lr = _obj.GetComponent<LineRenderer>();
                    _lr.positionCount = 179;
                    for (int j = 0; j < _lr.positionCount; j++)
                    {
                        float _b = (_orb.CurrentPosition + (((float)j + 1) / 180)) % 1;
                        float _c = _orb.CurrentInclination(_b) + 90;
                        float _d = Mathf.Pow(_orb._radius, _power);

                        float _x = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Cos(_b * 360 * Mathf.Deg2Rad);
                        float _y = _d * Mathf.Cos(_c * Mathf.Deg2Rad);
                        float _z = _d * Mathf.Sin(_c * Mathf.Deg2Rad) * Mathf.Sin(_b * 360 * Mathf.Deg2Rad);

                        float _modifier = (!_orb._orbitingStar && _orb._parentInt != -1) ? 2 : 1;
                        Vector3 _pos1 = new Vector3(_x, _y, _z) * _modifier + _parentPos;
                        _lr.SetPosition(j, _pos1);
                    }
                }
                   
                // -- Obj LookAt Camera --
                _obj.transform.LookAt(Camera.main.transform);


                // -- Add to List --
                _celestialBodies.Add(_obj);
            }
        }

        void LookAtCamera(int _a)
        {
            for (int i = 0; i < _stars.Count; i++)
            {
                _stars[i].transform.LookAt(Camera.main.transform);
            }

            for (int i = 0; i < _celestialBodies.Count; i++)
            {
                _celestialBodies[i].transform.LookAt(Camera.main.transform);
            }
        }

        void UpdateInfo()
        {
            int _s = MapManager.Instance._selectedStarSystem;
            Map _m = MapManager.Instance._map;

            StarSystem _s1 = _m._sectors[_s]._system;

            if (_m._playerFactionId <= -1)
            {
                for (int i = 0; i < _stars.Count; i++)
                {
                    _stars[i].SetActive(true);

                    Text _nameField = _stars[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    Text _typeField = _stars[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>();

                        _nameField.text = _s1._stars[i]._name;

                        switch (_viewMode)
                        {
                            case viewMode.Default:
                            {
                                _typeField.text = _s1._stars[i]._type + _s1._stars[i]._subType + _s1._stars[i]._size;
                                break;
                            }
                            case viewMode.Faction:
                            {
                                _typeField.text = (_m._sectors[_s]._controlFaction != -1) ? _m._factions[_m._sectors[_s]._controlFaction]._shorthand : "Neutral";
                                break;
                            }
                            case viewMode.Alliance:
                            {
                                _typeField.text = (_m._sectors[_s]._controlFaction != -1) ? _m._alliances[_m._factions[_m._sectors[_s]._controlFaction]._allianceId]._shorthand : "Neutral";
                                break;
                            }
                        }
                }

                for (int i = 0; i < _celestialBodies.Count; i++)
                {
                    _celestialBodies[i].SetActive(true);

                    Text _nameField = _celestialBodies[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                    Text _typeField = _celestialBodies[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>();

                    _nameField.text = _s1._cB[i]._name;

                    switch (_viewMode)
                    {
                        case viewMode.Default:
                        {
                            _typeField.text = (_s1._cB[i]._orbit._parentInt == -1 || _s1._cB[i]._orbit._orbitingStar) ? _s1._cB[i]._orbit._radius + "AU" : _s1._cB[i]._orbit._radius * 150000 + "k KM";
                            break;
                        }
                    }
                }
            }
            else
            {
                int _id = _m._playerFactions[_m._playerFactionId]._regFactionID;

                if (!SystemExplored)
                {
                    for (int i = 0; i < _stars.Count; i++)
                    {
                        _stars[i].SetActive(false);
                    }

                    for (int i = 0; i < _celestialBodies.Count; i++)
                    {
                        _celestialBodies[i].SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < _stars.Count; i++)
                    {
                        _stars[i].SetActive(true);

                        Text _nameField = _stars[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        Text _typeField = _stars[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>();

                        _nameField.text = _s1._stars[i]._name;

                        switch (_viewMode)
                        {
                            case viewMode.Default:
                                {
                                    _typeField.text = _s1._stars[i]._type + _s1._stars[i]._subType + _s1._stars[i]._size;
                                    break;
                                }
                            case viewMode.Faction:
                                {
                                    _typeField.text = (_m._sectors[_s]._controlFaction != -1) ? ((!KnownFaction(_m._sectors[_s]._controlFaction)) ? _m._factions[_m._sectors[_s]._controlFaction]._shorthand : "Unknown") : "Neutral";
                                    break;
                                }
                            case viewMode.Alliance:
                                {
                                    _typeField.text = (_m._sectors[_s]._controlFaction != -1) ? ((!KnownFaction(_m._sectors[_s]._controlFaction)) ? _m._alliances[_m._factions[_m._sectors[_s]._controlFaction]._allianceId]._shorthand : "Unknown") : "Neutral";
                                    break;
                                }
                        }
                    }

                    for (int i = 0; i < _celestialBodies.Count; i++)
                    {
                        _celestialBodies[i].SetActive(true);

                        Text _nameField = _celestialBodies[i].GetComponent<IndexScript>()._obj1.GetComponent<Text>();
                        Text _typeField = _celestialBodies[i].GetComponent<IndexScript>()._obj2.GetComponent<Text>();

                        _nameField.text = _s1._cB[i]._name;


                    }


                }
            }
        }

        bool SystemExplored
        {
            get
            {
                Map _m = MapManager.Instance._map;
                int _id = _m._playerFactions[_m._playerFactionId]._regFactionID;
                int _sId = MapManager.Instance._selectedStarSystem;
                for (int i = 0; i < _m._factions[_id]._exploredSectors.Count; i++)
                {
                    if (_m._factions[_id]._exploredSectors[i] == _sId)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        bool KnownFaction(int _id)
        {

            return true;
        }

        void Update()
        {
            if (MapManager.Instance._selectedStarSystem == -1 || MapManager.Instance._map._sectors.Count == 0)
            {
                MapManager.Instance.TurnOnGalaxyMap();
                return;
            }

            MapManager.Instance._selectedStarSystem = Mathf.Clamp(MapManager.Instance._selectedStarSystem, 0, MapManager.Instance._map._sectors.Count - 1);

            LookAtCamera(MapManager.Instance._selectedStarSystem);

            UpdateInfo();

            if (_regen)
            {
                _regen = false;
                RegenMap();
            }


        }
    }
}
