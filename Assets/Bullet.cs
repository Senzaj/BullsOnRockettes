using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D BirdRigid; 
    [SerializeField] public Rigidbody2D ShootRigid;

    private const float maxDistance = 3f;

    private bool isPressed;

    private void Update()
    {
        if (isPressed == true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.touches[0].position); 

            if (Vector2.Distance(mousePos, ShootRigid.position) > maxDistance) 
            {
                BirdRigid.position = ShootRigid.position + (mousePos - ShootRigid.position).normalized * maxDistance;
            }
            else
            {
                BirdRigid.position = mousePos;
            }
        }
    }

    private void OnMouseDown()
    {
        isPressed = true;
        BirdRigid.isKinematic = true;
    }

    private void OnMouseUp()
    {
        isPressed = false;
        BirdRigid.isKinematic = false;

        StartCoroutine(LetGo());
    }

    IEnumerator LetGo()
    {
        yield return new WaitForSeconds(0.1f);

        gameObject.GetComponent<SpringJoint2D>().enabled = false;
        this.enabled = false; 
        Destroy(gameObject, 5); 

        yield return new WaitForSeconds(2);
    }
}
