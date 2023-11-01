using UnityEngine;

namespace GameAssets.Gui.Scripts.Window
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private bool _unpause;
        [SerializeField] private bool _pause;
        [SerializeField] private Animator _windowAnim;
        
        
        
        public void Enter()
        {
            _windowAnim.Play("Revealing");
            
            if (_pause)
            {
                Time.timeScale = 0;

                //stop touching
            }
        }

        public void Quit()
        {
            _windowAnim.Play("Hidin");
        }

        private void Entered()
        {
            if (_unpause)
            {
                Time.timeScale = 1;

                //stop touching
            }

            _windowAnim.Play("Revealed");
        }

        private void Quited()
        {
            _windowAnim.Play("Hidden");
        }
    }
}
