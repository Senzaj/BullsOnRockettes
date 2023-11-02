using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAssets.Gui.Scripts.ForButtons
{
    public class StageLoader : MonoBehaviour
    {
        public void LoadStartStage()
        {
            SceneManager.LoadScene("StartStage");
        }

        public void LoadGameStage()
        {
            SceneManager.LoadScene("GameStage");
        }
    }
}
