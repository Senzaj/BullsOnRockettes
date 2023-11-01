using Random = UnityEngine.Random;
using GameAssets.Common.Scripts;
using GameAssets.Player.Scripts;
using GameAssets.Enemy.Scripts;
using System.Collections;
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

        [SerializeField] private TMP_Text[] _currentScoreTMP;
        [SerializeField] private TMP_Text _maxScoreTMP;

        private Coroutine[] _coroutines;
        
        private void OnEnable()
        {
            _enemyPool.InstantiateStartCount();
            _ammoPool.InstantiateStartCount();

            _playerManager.EnemyCollided += IncreaseScore;
            _playerManager.PeacefulCollided += Defeat;
            
            StartLocating();
        }

        private void IncreaseScore()
        {
            //show and set
        }

        private void Defeat()
        {
            //show and set
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
                float randomNumber = Random.Range(1.2f, 3f);
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
                float randomNumber = Random.Range(1.2f, 3f);
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
    }
}
