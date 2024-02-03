using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenuUI : MonoBehaviour
{

    // Buttons
    public GameObject _saveButton;
    public GameObject _loadButton;
    public GameObject _newButton;
    public GameObject _quitProgram;

    // Menu Canvas
    public GameObject _menuCanvas;

    // Save Instance Canvas & Objects & vars
    public GameObject _saveCanvas;
    public GameObject _saveMenuButton;
    public GameObject _saveMenuInput;

    public bool _saveMenuInputActive = false;

    // Load Instance Canvas & Objects & vars
    public GameObject _loadCanvas;
    public GameObject _loadMenuButton;
    public GameObject _loadMenuInput;

    public bool _loadMenuInputActive = false;

    // Confirmation menu
    int _confirmationResponse = -1;

    // Enter Text Function
    void EnterText(InputField _inputField, bool _lettersAllowed, bool _numbersAllowed, bool _spaceAllowed, int _maxLength, out bool _active)
    {
        _active = true;
        if (_inputField.text.Length < _maxLength)
        {
            if (_lettersAllowed)
            {
                if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "A";
                }
                else if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "B";
                }
                else if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "C";
                }
                else if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "D";
                }
                else if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "E";
                }
                else if (Input.GetKeyDown(KeyCode.F) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "F";
                }
                else if (Input.GetKeyDown(KeyCode.G) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "G";
                }
                else if (Input.GetKeyDown(KeyCode.H) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "H";
                }
                else if (Input.GetKeyDown(KeyCode.I) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "I";
                }
                else if (Input.GetKeyDown(KeyCode.J) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "J";
                }
                else if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "K";
                }
                else if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "L";
                }
                else if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "M";
                }
                else if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "N";
                }
                else if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "O";
                }
                else if (Input.GetKeyDown(KeyCode.P) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "P";
                }
                else if (Input.GetKeyDown(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "Q";
                }
                else if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "R";
                }
                else if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "S";
                }
                else if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "T";
                }
                else if (Input.GetKeyDown(KeyCode.U) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "U";
                }
                else if (Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "V";
                }
                else if (Input.GetKeyDown(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "W";
                }
                else if (Input.GetKeyDown(KeyCode.X) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "X";
                }
                else if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "Y";
                }
                else if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift))
                {
                    _inputField.text = _inputField.text + "Z";
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    _inputField.text = _inputField.text + "a";
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    _inputField.text = _inputField.text + "b";
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    _inputField.text = _inputField.text + "c";
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    _inputField.text = _inputField.text + "d";
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    _inputField.text = _inputField.text + "e";
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    _inputField.text = _inputField.text + "f";
                }
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    _inputField.text = _inputField.text + "g";
                }
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    _inputField.text = _inputField.text + "h";
                }
                else if (Input.GetKeyDown(KeyCode.I))
                {
                    _inputField.text = _inputField.text + "i";
                }
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    _inputField.text = _inputField.text + "j";
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    _inputField.text = _inputField.text + "k";
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    _inputField.text = _inputField.text + "l";
                }
                else if (Input.GetKeyDown(KeyCode.M))
                {
                    _inputField.text = _inputField.text + "m";
                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    _inputField.text = _inputField.text + "n";
                }
                else if (Input.GetKeyDown(KeyCode.O))
                {
                    _inputField.text = _inputField.text + "o";
                }
                else if (Input.GetKeyDown(KeyCode.P))
                {
                    _inputField.text = _inputField.text + "p";
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    _inputField.text = _inputField.text + "q";
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    _inputField.text = _inputField.text + "r";
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    _inputField.text = _inputField.text + "s";
                }
                else if (Input.GetKeyDown(KeyCode.T))
                {
                    _inputField.text = _inputField.text + "t";
                }
                else if (Input.GetKeyDown(KeyCode.U))
                {
                    _inputField.text = _inputField.text + "u";
                }
                else if (Input.GetKeyDown(KeyCode.V))
                {
                    _inputField.text = _inputField.text + "v";
                }
                else if (Input.GetKeyDown(KeyCode.W))
                {
                    _inputField.text = _inputField.text + "w";
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    _inputField.text = _inputField.text + "x";
                }
                else if (Input.GetKeyDown(KeyCode.Y))
                {
                    _inputField.text = _inputField.text + "y";
                }
                else if (Input.GetKeyDown(KeyCode.Z))
                {
                    _inputField.text = _inputField.text + "z";
                }
            }
            if (_numbersAllowed)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _inputField.text = _inputField.text + "1";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _inputField.text = _inputField.text + "2";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    _inputField.text = _inputField.text + "3";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    _inputField.text = _inputField.text + "4";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    _inputField.text = _inputField.text + "5";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    _inputField.text = _inputField.text + "6";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    _inputField.text = _inputField.text + "7";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    _inputField.text = _inputField.text + "8";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    _inputField.text = _inputField.text + "9";
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    _inputField.text = _inputField.text + "0";
                }
                else if (Input.GetKeyDown(KeyCode.Minus) && Input.GetKey(KeyCode.Space))
                {
                    _inputField.text = _inputField.text + "_";
                }
            }
            if (_spaceAllowed && Input.GetKeyDown(KeyCode.Space))
            {
                _inputField.text += " ";
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (_inputField.text.Length > 0)
                {
                    _inputField.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
                }

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                _active = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {

                if (_inputField.text.Length > 0)
                {
                    _inputField.text = _inputField.text.Substring(0, _inputField.text.Length - 1);
                }

            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                _active = false;
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
        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _newButton && Input.GetMouseButtonDown(0))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _quitProgram && Input.GetMouseButtonDown(0) && !_saveCanvas.activeSelf && !_loadCanvas.activeSelf)
        {
            Application.Quit();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_saveCanvas.activeSelf || _loadCanvas.activeSelf)
            {
                _loadCanvas.SetActive(false);
                _saveCanvas.SetActive(false);
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
        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _saveMenuInput && Input.GetMouseButtonDown(0) && _saveCanvas.activeSelf)
        {
            _saveMenuInputActive = true;
        }
        else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _saveButton && Input.GetMouseButtonDown(0))
        {
            _saveCanvas.SetActive(true);
        }
        else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _saveMenuButton && Input.GetMouseButtonDown(0))
        {
            XMLWriter.Instance._save = true;
        }

        if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _loadMenuInput && Input.GetMouseButtonDown(0) && _loadCanvas.activeSelf)
        {
            _loadMenuInputActive = true;
        }
        else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _loadButton && Input.GetMouseButtonDown(0))
        {
            _loadCanvas.SetActive(true);
        }
        else if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == _loadMenuButton && Input.GetMouseButtonDown(0))
        {
            XMLWriter.Instance._load = true;
            _loadCanvas.SetActive(true);
            _loadMenuInputActive = false;
            
        }

        if (_saveMenuInputActive)
        {
            _saveMenuInput.GetComponent<Image>().color = new Color32(220, 220, 220, 255);

            _saveMenuInput.GetComponent<InputField>().text = XMLWriter.Instance._saveFileName;

            EnterText(_saveMenuInput.GetComponent<InputField>(), true, true, false, 20, out _saveMenuInputActive);

            XMLWriter.Instance._saveFileName = _saveMenuInput.GetComponent<InputField>().text;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject != _saveMenuInput && Input.GetMouseButtonDown(0))
            {

                _saveMenuInputActive = false;
                _saveMenuInput.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
            }
        }
        else
        {
            _saveMenuInputActive = false;
            _saveMenuInput.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
        }

        if (_loadMenuInputActive)
        {
            _loadMenuInput.GetComponent<Image>().color = new Color32(220, 220, 220, 255);

            _loadMenuInput.GetComponent<InputField>().text = XMLWriter.Instance._saveFileName;

            EnterText(_loadMenuInput.GetComponent<InputField>(), true, true, false, 20, out _loadMenuInputActive);

            XMLWriter.Instance._saveFileName = _loadMenuInput.GetComponent<InputField>().text;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject != _loadMenuInput && Input.GetMouseButtonDown(0))
            {

                _loadMenuInputActive = false;
                _loadMenuInput.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
            }
        }
        else
        {
            _loadMenuInputActive = false;
            _loadMenuInput.GetComponent<Image>().color = new Color32(180, 180, 180, 255);
        }
    }
}
