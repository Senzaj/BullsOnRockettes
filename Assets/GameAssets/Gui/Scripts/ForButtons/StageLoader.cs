using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAssets.Gui.Scripts.ForButtons
{
    public class StageLoader : MonoBehaviour
    {
        private void LoadStartStage()
        {
            SceneManager.LoadScene("StartStage");
        }

        private void LoadGameStage()
        {
            SceneManager.LoadScene("GameStage");
        }
    }
}
