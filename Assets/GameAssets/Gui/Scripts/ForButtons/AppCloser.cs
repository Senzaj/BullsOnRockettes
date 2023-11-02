using UnityEngine;

namespace GameAssets.Gui.Scripts.ForButtons
{
    public class AppCloser : MonoBehaviour
    {
        public void CloseApp()
        {
            Application.Quit();
        }
    }
}
