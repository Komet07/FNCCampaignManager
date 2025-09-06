using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEngine.UI;

using TMPro;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        
        public GameObject _loadMenu;
        public Scrollbar _s;

        public int _saveIndexInt = 0;

        [SerializeField]
        private List<string> _saveNames = new List<string>();

        // Save Gameobjects
        public GameObject _save1Obj;
        public GameObject _save2Obj;
        public GameObject _save3Obj;
        public GameObject _save4Obj;

        public TMP_Text _save1Text;
        public TMP_Text _save2Text;
        public TMP_Text _save3Text;
        public TMP_Text _save4Text;

        // Delete Saves
        [Header("Save Deletion")]
        public GameObject _saveConfirmationDelMenu;
        public Text _saveConfirmationDelText;

        public void TurnOnLoad()
        {
            _loadMenu.SetActive(!_loadMenu.activeSelf);
        }

        public void InitLoad(int _a)
        {
            MainMenuManager.Instance._saveName = _saveNames[_saveIndexInt + _a];

            _loadMenu.SetActive(false);

            MainMenuManager.Instance.StartMain(true);
        }

        void ListSaves()
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "saves")))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "saves"));
            }

            DirectoryInfo _d = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "saves"));

            _saveNames = new List<string>();
            foreach (FileInfo file in _d.GetFiles("*.xml"))
            {
                string _name = file.Name;
                string[] _nameStr = _name.Split('.');
                _saveNames.Add(_nameStr[0]);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (_loadMenu.activeSelf)
            {
                ListSaves();

                if (_saveNames.Count < 5)
                {

                    _saveIndexInt = 0;

                    _save1Obj.SetActive((_saveNames.Count > 0) ? true : false);
                    _save2Obj.SetActive((_saveNames.Count > 1) ? true : false);
                    _save3Obj.SetActive((_saveNames.Count > 2) ? true : false);
                    _save4Obj.SetActive((_saveNames.Count > 3) ? true : false);

                    _save1Text.text = (_saveNames.Count > 0) ? _saveNames[0] : "";
                    _save2Text.text = (_saveNames.Count > 1) ? _saveNames[1] : "";
                    _save3Text.text = (_saveNames.Count > 2) ? _saveNames[2] : "";
                    _save4Text.text = (_saveNames.Count > 3) ? _saveNames[3] : "";
                }
                else
                {
                    _saveIndexInt = Mathf.Clamp(Mathf.RoundToInt(_s.value * (_saveNames.Count - 4)), 0, _saveNames.Count - 4);

                    _save1Obj.SetActive(true);
                    _save2Obj.SetActive(true);
                    _save3Obj.SetActive(true);
                    _save4Obj.SetActive(true);

                    _save1Text.text = _saveNames[_saveIndexInt];
                    _save2Text.text = _saveNames[_saveIndexInt + 1];
                    _save3Text.text = _saveNames[_saveIndexInt + 2];
                    _save4Text.text = _saveNames[_saveIndexInt + 3];
                }
            }
        }
    }
}
    
