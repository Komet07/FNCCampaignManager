using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.IO;

public class EscapeMenuUI : MonoBehaviour
{

    // Buttons
    public GameObject _saveButton;
    public GameObject _loadButton;
    public GameObject _newButton;
    public GameObject _settingsButton;
    public GameObject _quitProgram;

    // Menu Canvas
    public GameObject _menuCanvas;

    public List<string> _saveNames = new List<string>();

    // Save Instance Canvas & Objects & vars
    public GameObject _saveCanvas;
    public GameObject _saveMenuButton;
    public GameObject _saveMenuInput;

    public bool _saveMenuInputActive = false;

    public List<GameObject> _saveSaveButtons = new List<GameObject>();
    public List<Text> _saveSaveText = new List<Text>();

    public Scrollbar _saveSaveScrollbar;

    public int _saveOffset = 0;

    // Load Instance Canvas & Objects & vars
    public GameObject _loadCanvas;
    public GameObject _loadMenuButton;
    public GameObject _loadMenuInput;

    public bool _loadMenuInputActive = false;

    public Scrollbar _loadSaveScrollbar;

    public List<GameObject> _loadSaveButtons = new List<GameObject>();
    public List<Text> _loadSaveText = new List<Text>();

    // Settings menu
    public GameObject _settingsMenu;

    // Save Conf Menu
    public GameObject _sCMenu;
    public bool _sCActive;

    bool _dontLoad = false;

    // Confirmation menu
    //int _confirmationResponse = -1;

    public void LoadTurnOn()
    {
        _loadCanvas.SetActive(true);
        _loadSaveScrollbar.value = 0;
    }

    public void SaveTurnOn()
    {
        _saveCanvas.SetActive(true);
        _saveSaveScrollbar.value = 0;
    }

    public void SettingTurnOn()
    {
        _settingsMenu.SetActive(true);
    }

    public void New()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Save()
    {
        XMLWriter.Instance._save = true;
        _sCActive = false;
    }

    public void Load()
    {
        XMLWriter.Instance._load = true;
        _loadCanvas.SetActive(false);
        _loadMenuInputActive = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SaveNameProcess(GameObject _obj)
    {
        InputField _a = _obj.GetComponent<InputField>();

        XMLWriter.Instance._saveFileName = _a.text;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveConfTurnOn()
    {
        _sCActive = true;

        _saveCanvas.SetActive(false);
    }

    public void SaveConfCancel()
    {
        _sCActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

    bool savePresent
    {
        get
        {
            foreach (string _str in _saveNames)
            {
                if (_str == XMLWriter.Instance._saveFileName)
                {
                    return true;
                }
            }

            return false;
        }


    }

    public void _replaceSaveFileName(int _a)
    {
        XMLWriter.Instance._saveFileName = _saveNames[_a + _saveOffset];
    }

    // Update is called once per frame
    void Update()
    {
        ListSaves();

        _settingsMenu.GetComponent<IndexScript>()._obj1.GetComponent<Text>().text = "Fullscreen: " + Screen.fullScreen;

        

        if (_sCActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _sCActive = false;
            }

            _sCMenu.SetActive(true);
        }
        else
        {
            _sCMenu.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !_dontLoad)
        {
            // CHECK IF INCOMPATABILITIES
            if (MapManager.Instance._escapeMenuIncompatTriggered)
            {
                return;
            }

            for (int i = 0; i < MapManager.Instance._escapeMenuIncompat.Count; i++)
            {
                if (MapManager.Instance._escapeMenuIncompat[i].activeInHierarchy)
                {
                    return;
                }
            }

            if (_saveCanvas.activeSelf || _loadCanvas.activeSelf || _settingsMenu.activeSelf)
            {
                _loadCanvas.SetActive(false);
                _saveCanvas.SetActive(false);
                _settingsMenu.SetActive(false);
            }
            else {
                if (_menuCanvas.activeSelf)
                {
                    _menuCanvas.SetActive(false);
                }
                else
                {
                    _menuCanvas.SetActive(true);
                }
            }
        }
        

        if (_saveCanvas.activeSelf)
        {
            if (_saveNames.Count < 10)
            {
                _saveOffset = 0;
                _saveSaveScrollbar.value = 0;
                if (_saveSaveScrollbar.gameObject.activeSelf)
                {
                    _saveSaveScrollbar.gameObject.SetActive(false);
                }
                for (int i = 0; i < _saveSaveButtons.Count; i++)
                {
                    _saveSaveButtons[i].SetActive(_saveNames.Count > i ? true : false);
                    _saveSaveText[i].text = (_saveNames.Count > i) ? _saveNames[i] : "";
                }
            }
            else
            {
                if (!_saveSaveScrollbar.gameObject.activeSelf)
                {
                    _saveSaveScrollbar.gameObject.SetActive(true);
                }
                    
                _saveOffset = Mathf.RoundToInt(_saveSaveScrollbar.value * (_saveNames.Count - 10));

                if (!_saveSaveButtons[9].activeSelf)
                {
                    for (int i = 0; i < _saveSaveButtons.Count; i++)
                    {
                        _saveSaveButtons[i].SetActive(true);
                    }
                }

                for (int i = 0; i < _saveSaveText.Count; i++)
                {
                    _saveSaveText[i].text = _saveNames[i + _saveOffset];
                }
            }
                
        }

        if (_loadCanvas.activeSelf)
        {
            if (_saveNames.Count < 10)
            {
                _saveOffset = 0;
                _loadSaveScrollbar.value = 0;
                if (_loadSaveScrollbar.gameObject.activeSelf)
                {
                    _loadSaveScrollbar.gameObject.SetActive(false);
                }

                for (int i = 0; i < _loadSaveButtons.Count; i++)
                {
                    _loadSaveButtons[i].SetActive(_saveNames.Count > i ? true : false);
                    _loadSaveText[i].text = (_saveNames.Count > i) ? _saveNames[i] : "";
                }
            }
            else
            {

                if (!_loadSaveScrollbar.gameObject.activeSelf)
                {
                    _loadSaveScrollbar.gameObject.SetActive(true);
                }

                _saveOffset = Mathf.RoundToInt(_loadSaveScrollbar.value * (_saveNames.Count - 10));

                if (!_loadSaveButtons[9].activeSelf)
                {
                    for (int i = 0; i < _loadSaveButtons.Count; i++)
                    {
                        _loadSaveButtons[i].SetActive(true);
                    }
                }

                for (int i = 0; i < _loadSaveText.Count; i++)
                {
                    _loadSaveText[i].text = _saveNames[i + _saveOffset];
                }
            }

        }


        if (!_loadMenuInput.GetComponent<InputField>().isFocused)
        {
            _loadMenuInput.GetComponent<InputField>().text = XMLWriter.Instance._saveFileName;
        }

        if (!_saveMenuInput.GetComponent<InputField>().isFocused)
        {
            _saveMenuInput.GetComponent<InputField>().text = XMLWriter.Instance._saveFileName;
        }


        _loadMenuButton.SetActive(savePresent);

        

        if (GalaxyMap.Instance._InfoDisplay.activeSelf || GMMenu.Instance._menuObjects.activeSelf)
        {
            _dontLoad = true;
        }
        else
        {
            _dontLoad = false;
        }
    }
}
