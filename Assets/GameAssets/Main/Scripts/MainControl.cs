using Random = UnityEngine.Random;
using GameAssets.Common.Scripts;
using GameAssets.Player.Scripts;
using GameAssets.Enemy.Scripts;
using System.Collections;
using GameAssets.Gui.Scripts.Teaching;
using GameAssets.Gui.Scripts.Window;
using UnityEngine;
using TMPro;

namespace GameAssets.Main.Scripts
{
    public class MainControl : MonoBehaviour
    {
        [SerializeField] private ObjectPool _enemyPool;
        [SerializeField] private ObjectPool _ammoPool;
        [SerializeField] private Vector2[] _enemyRightPositions;
        [SerializeField] private Vector2[] _ammoRightPositions;
        [SerializeField] private PeacefulInstance _leftTarget;
        [SerializeField] private PeacefulInstance _rightTarget;
        [SerializeField] private PlayerManager _playerManager;
        [SerializeField] private Teaching _teaching;

        [SerializeField] private TMP_Text[] _currentScoreTMP;
        [SerializeField] private TMP_Text _maxScoreTMP;
        [SerializeField] private Window _gameWindow;
        [SerializeField] private Window _defeatWindow;

        private const string Teaching = nameof(Teaching);
        private const string MaxScore = nameof(MaxScore);
        private static readonly WaitForSeconds Wait = new WaitForSeconds(1f);

        private bool _isDefeat;
        private int _currentScore;
        private Coroutine[] _coroutines;
        
        private void OnEnable()
        {
            _currentScore = 0;
            _isDefeat = false;
            _teaching.SetStatus();
            _enemyPool.InstantiateStartCount();
            _ammoPool.InstantiateStartCount();

            _playerManager.EnemyCollided += IncreaseScore;
            _playerManager.PeacefulCollided += Defeat;

            if (_teaching.GetStatus())
            {
                StartLocating();
            }
            else
            {
                _teaching.ChangedStatus += TeachingFinished;
                _teaching.Teach();
            }

        }

        private void IncreaseScore()
        {
            _currentScore++;

            foreach (TMP_Text tmp in _currentScoreTMP)
                tmp.text = _currentScore.ToString();
        }

        private void Defeat()
        {
            if (_isDefeat == false)
            {
                _isDefeat = !_isDefeat;
                _gameWindow.Quit();

                int maxScore = PlayerPrefs.HasKey(MaxScore) ? PlayerPrefs.GetInt(MaxScore) : 0;

                if (_currentScore > maxScore)
                {
                    maxScore = _currentScore;
                    PlayerPrefs.SetInt(MaxScore, maxScore);
                }

                _maxScoreTMP.text = maxScore.ToString();
                StartCoroutine(OnDefeat());
            }
        }

        private IEnumerator OnDefeat()
        {
            yield return Wait;
            _defeatWindow.Enter();
        }
        
        private void StartLocating()
        {
            if (_coroutines != null)
                foreach (Coroutine coroutine in _coroutines)
                    StopCoroutine(coroutine);

            _coroutines = new[] { StartCoroutine(LocateAmmo()), StartCoroutine(LocateEnemy()) };
        }

        private void OnDisable()
        {
            if (_coroutines != null)
                foreach (Coroutine coroutine in _coroutines)
                    StopCoroutine(coroutine);
        }

        private IEnumerator LocateAmmo()
        {
            while (gameObject.activeSelf)
            {
                float randomNumber = Random.Range(1.4f, 3.1f);
                int sideIndex = Random.Range(0, 2);
                RocketAmmo ammo = _ammoPool.Get().GetComponent<RocketAmmo>();
                Vector2 rightPos = _ammoRightPositions[Random.Range(0, _ammoRightPositions.Length)];
                
                yield return new WaitForSeconds(randomNumber);

                if (sideIndex == 0)
                {
                    ammo.transform.position = new Vector2(-rightPos.x, rightPos.y);
                    ammo.gameObject.SetActive(true);
                    ammo.StartFlyTo(Vector2.right);
                }
                else
                {
                    ammo.transform.position = rightPos;
                    ammo.gameObject.SetActive(true);
                    ammo.StartFlyTo(Vector2.left);
                }
            }
        }
        
        private IEnumerator LocateEnemy()
        {
            while (gameObject.activeSelf)
            {
                float randomNumber = Random.Range(1.9f, 4f);
                int sideIndex = Random.Range(0, 2);
                RocketEnemy enemy = _enemyPool.Get().GetComponent<RocketEnemy>();
                Vector2 rightPos = _enemyRightPositions[Random.Range(0, _enemyRightPositions.Length)];
                
                yield return new WaitForSeconds(randomNumber);

                if (sideIndex == 0)
                {
                    enemy.transform.position = new Vector2(-rightPos.x, rightPos.y);
                    enemy.gameObject.SetActive(true);
                    enemy.StartFlyTo(Vector2.right, _leftTarget.transform.position);
                }
                else
                {
                    enemy.transform.position = rightPos;
                    enemy.gameObject.SetActive(true);
                    enemy.StartFlyTo(Vector2.left, _rightTarget.transform.position);
                }
            }
        }
        
        private void TeachingFinished()
        {
            StartLocating();
            _teaching.ChangedStatus -= TeachingFinished;
        }
    }
}
