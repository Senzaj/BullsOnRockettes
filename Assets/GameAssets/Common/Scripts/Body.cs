using UnityEngine;

namespace GameAssets.Common.Scripts
{
    public class Body : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;

        public void makeDynamic()
        {
            _rigidbody2D.isKinematic = false;
        }
    }
}
