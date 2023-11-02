using GameAssets.Player.Scripts;
using UnityEngine;

namespace GameAssets.Gui.Scripts.Window
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private bool _startWindow;
        [SerializeField] private bool _unpause;
        [SerializeField] private bool _pause;
        [SerializeField] private Animator _windowAnim;
        [SerializeField] private PlayerManager _playerManager;

        private const string AEnter = "Enter";
        private const string AQuit = "Quit";
        private const string AEntered = "Entered";
        private const string AQuited = "Quited";
        
        public void Enter()
        {
            _windowAnim.Play(AEnter);
            
            if (_pause)
            {
                Time.timeScale = 0;
                _playerManager.CurrentMissileIsntReady();
            }
        }

        private void Start()
        {
            if (_startWindow)
                Entered();
        }

        public void Quit()
        {
            _windowAnim.Play(AQuit);
        }

        private void Entered()
        {
            if (_unpause)
            {
                Time.timeScale = 1;
                _playerManager.CurrentMissileReady();
            }

            _windowAnim.Play(AEntered);
        }

        private void Quited()
        {
            _windowAnim.Play(AQuited);
        }
    }
}
