using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLWriter : MonoBehaviour
{

    public bool _save = false;
    public bool _load = false;
    public string _saveFileName = "save1";

    void Save()
    {

        var serializer = new XmlSerializer(typeof(Map));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, "saves/"+_saveFileName+".xml"), FileMode.Create))
        {
            serializer.Serialize(stream, MapManager.Instance._map);
            Debug.Log(Path.Combine(Application.dataPath, "saves/" + _saveFileName + ".xml"));
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
            Save();
        }

        if (_load)
        {
            _load = false;
            var _map = Map.Load(Path.Combine(Application.dataPath, "saves/"+_saveFileName+".xml"));
            MapManager.Instance._map = _map;

            GalaxyMap.Instance._regen = true;
        }
    }
}
