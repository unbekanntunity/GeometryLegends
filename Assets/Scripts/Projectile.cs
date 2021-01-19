using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject user;
    public float bulletSpeed;
    public Vector3 targetPoint;

    private Vector3 startPoint;
    private float range;
    private Rigidbody rig;

    private void Start()
    {
        startPoint = user.transform.position;
        range = user.GetComponent<GetStats>().selectedSkill.range;
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        while(Vector3.Distance(startPoint, gameObject.transform.position) <= range)
        gameObject.transform.position = Vector3.RotateTowards(gameObject.transform.position, targetPoint, 360f, 360f);
        rig.AddForce(gameObject.transform.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if(collision.collider.gameObject.GetComponent<GetStats>())
            DamageHandler.DealDamage(user.GetComponent<GetStats>(), collision.collider.gameObject.GetComponent<GetStats>());
    }
}
