using UnityEngine;

namespace GameAssets.Gui.Scripts.OrientationManager
{
    public class OrientationManager : MonoBehaviour
    {
        [SerializeField] private bool _game;

        private void Awake()
        {
            if (_game)
            {
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
            }
            else
            {
                Screen.orientation = ScreenOrientation.Portrait;
                Screen.orientation = ScreenOrientation.AutoRotation;
                Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = false;
            }
        }
    }
}
