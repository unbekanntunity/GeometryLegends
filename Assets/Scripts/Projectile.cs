using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject user;
    public float bulletSpeed;
    public Vector3 targetPoint;

    private float range;
    private Rigidbody rig;

    private void Start()
    {
        range = user.GetComponent<GetStats>().selectedSkill.range;
        rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 targetDirection = targetPoint - transform.position;
        
        float singleStep =  1f * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
    
        transform.rotation = Quaternion.LookRotation(newDirection);
        rig.AddForce(gameObject.transform.forward * bulletSpeed * Time.deltaTime);


        if (Vector3.Distance(gameObject.transform.position, targetPoint) < 0.4f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == user)
            return;

        Destroy(gameObject);
        if (other.gameObject.GetComponent<GetStats>())
            DamageHandler.DealDamage(user.GetComponent<GetStats>(), other.gameObject.GetComponent<GetStats>());
    }
}
