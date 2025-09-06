using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Singleton
        public static MainMenuManager Instance;
        private void Awake()
        {
            Instance = this;
        }
        #endregion

        public string _saveName = "";

        public void StartMain(bool _load)
        {
            SceneSwitch.Instance._load = _load;
            SceneSwitch.Instance._saveName = _saveName;
            SceneManager.LoadScene("Main");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
