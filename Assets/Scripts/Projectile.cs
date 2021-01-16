using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject user;
    public float bulletSpeed;

    private Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rig.AddForce(gameObject.transform.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        DamageHandler.DealDamage(user.GetComponent<GetStats>(), collision.collider.gameObject.GetComponent<GetStats>());
    }
}
