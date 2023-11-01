using System;
using UnityEngine;

namespace GameAssets.Gui.Scripts.Teaching
{
    public class Teaching : MonoBehaviour
    {
        [SerializeField] private Window.Window _thisWindow;
        [SerializeField] private Window.Window[] _windows;
        [SerializeField] private CanvasGroup _gameWindow;
        
        public event Action ChangedStatus;
        
        private const string Status = nameof(Status);

        private int _index;
        private bool _isFinished;

        public void SetStatus()
        {
            if (PlayerPrefs.HasKey(Status))
                _isFinished = true;
            else
                _index = 0;
        }

        public bool GetStatus()
        {
            return _isFinished;
        }

        public void Teach()
        {
            DisableGameWindow();
            
            foreach (Window.Window menu in _windows)
                menu.Quit();

            _thisWindow.Enter();
            Continue();
        }

        private void Continue()
        {
            if (_index < _windows.Length)
                Next();
            else
                Finish();
        }

        private void Next()
        {
            if (_index > 0)
                _windows[_index - 1].Quit();

            _windows[_index++].Enter();
        }
        
        private void Finish()
        {
            _isFinished = true;
            EnableGameWindow();
            _thisWindow.Quit();
            PlayerPrefs.SetInt(Status, 1);
            ChangedStatus?.Invoke();
        }

        private void DisableGameWindow()
        {
            _gameWindow.interactable = false;
            _gameWindow.blocksRaycasts = false;
        }

        private void EnableGameWindow()
        {
            _gameWindow.blocksRaycasts = true;
            _gameWindow.interactable = true;
        }
    }
}
