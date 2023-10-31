using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D BirdRigid; 
    [SerializeField] public Rigidbody2D ShootRigid;

    private const float maxDistance = 5f;

    private bool isPressed;

    private void Update()
    {
        if (isPressed)
        {
            Vector2 touch = Camera.main.ScreenToWorldPoint(Input.touches[0].position); 

            if (Vector2.Distance(touch, ShootRigid.position) > maxDistance) 
            {
                BirdRigid.position = ShootRigid.position + (touch - ShootRigid.position).normalized * maxDistance;
            }
            else
            {
                BirdRigid.position = touch;
            }
        }
    }

    private void OnMouseDown()
    {
        BirdRigid.velocity = Vector2.zero;
        isPressed = true;
        BirdRigid.isKinematic = true;
        BirdRigid.useFullKinematicContacts = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        BirdRigid.useFullKinematicContacts = false;
        BirdRigid.isKinematic = false;

        //StartCoroutine(LetGo());
    }

    IEnumerator LetGo()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.GetComponent<SpringJoint2D>().enabled = false;
        enabled = false; 
        Destroy(gameObject, 5); 

        yield return new WaitForSeconds(2);
    }
}
