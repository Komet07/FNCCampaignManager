using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public List<GameObject> _jgConnections = new List<GameObject>() { };

    public GameObject _originHexagon;
    public GameObject _originJGC;

    public GameObject UI;
    public GameObject UIb;

    // _viewmode Text

    public Text _viewModeText;
    public GameObject _viewModeButton;

    public bool _regen = false;
    public bool _enable = false;
    public bool _disable = false;

    public string _viewMode = "factions";

    public void SwitchViewMode()
    {
        Debug.Log("A");
        if (_viewMode == "factions")
        {
            _viewMode = "alliances";
            _viewModeText.text = "Mode: Alliances";
        }
        else
        {
            _viewMode = "factions";
            _viewModeText.text = "Mode: Factions";
        }
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
            _hexClone.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = _sector._name;



            // Add hex to list
            _hexagons.Add(_hexClone);

        }

        // JG Connection
        for (int i = 0; i < MapManager.Instance._map._jumpGates.Count; i++)
        {
            // Instantiate
            GameObject _jgClone = Instantiate(_originJGC, UIb.transform);
            _jgClone.SetActive(true);

            // Set sector 1 & 2
            Sector _sector1 = MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector1Id];
            Sector _sector2 = MapManager.Instance._map._sectors[MapManager.Instance._map._jumpGates[i]._sector2Id];

            // Calculate deltaX, deltaY and theta (using tan-1) for Sector 1 and 2

            float _deltaXs1 = (.75f * _sector2._posXInt) - (.75f * _sector1._posXInt);
            float _deltaXs2 = (.75f * _sector1._posXInt) - (.75f * _sector2._posXInt);

            float _sector1YPos = 0;
            float _sector2YPos = 0;

            if ((_sector1._posXInt * _sector1._posXInt)%2 == 1)
            {
                _sector1YPos = .9f * _sector1._posYInt + .45f;
            }
            else
            {
                _sector1YPos = .9f * _sector1._posYInt;
            }

            if ((_sector2._posXInt*_sector2._posXInt) % 2 == 1)
            {
                _sector2YPos = .9f * _sector2._posYInt + .45f;
            }
            else
            {
                _sector2YPos = .9f * _sector2._posYInt;
            }

            float _deltaYs1 = _sector2YPos - _sector1YPos;
            float _deltaYs2 = _sector1YPos - _sector2YPos;

            float theta1 = Mathf.Atan2(_deltaYs1, _deltaXs1);
            float theta2 = Mathf.Atan2(_deltaYs2, _deltaXs2);

            GameObject _hex1 = _hexagons[MapManager.Instance._map._jumpGates[i]._sector1Id];
            GameObject _hex2 = _hexagons[MapManager.Instance._map._jumpGates[i]._sector2Id];

            Vector3 _p1 = new Vector3(_hex1.transform.position.x + Mathf.Cos(theta1) * .35f, _hex1.transform.position.y + Mathf.Sin(theta1) * .35f, -5);
            Vector3 _p2 = new Vector3(_hex2.transform.position.x + Mathf.Cos(theta2) * .35f, _hex2.transform.position.y + Mathf.Sin(theta2) * .35f, -5);

            _jgClone.GetComponent<LineRenderer>().SetPosition(0, _p1);
            _jgClone.GetComponent<LineRenderer>().SetPosition(1, _p2);
            _jgClone.GetComponent<IndexScript>()._obj1.transform.position = _p1;
            _jgClone.GetComponent<IndexScript>()._obj2.transform.position = _p2;

            _jgConnections.Add(_jgClone);
        }


    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_regen)
        {
            _regen = false;
            RegenMap();
        }

        if (_enable)
        {
            _enable = false;
        }

        if (_disable)
        {
            _disable = false;
        }

        // Update Hexagons
        for (int i = 0; i < _hexagons.Count; i++)
        {
            if (_viewMode == "factions")
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
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
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
            else if (_viewMode == "alliances")
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
                if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _hexagons[i].GetComponent<IndexScript>()._obj3)
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

        }

        // Check if pressed on ViewMode Button
        Ray rayB;
        RaycastHit hitB;

        rayB = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayB, out hitB) && hitB.transform.gameObject == _viewModeButton && Input.GetMouseButtonDown(0))
        {
            SwitchViewMode();

        }
    }
}
