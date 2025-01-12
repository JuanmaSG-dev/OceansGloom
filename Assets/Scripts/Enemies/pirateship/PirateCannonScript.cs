using UnityEngine;

public class PirateCannonScript : MonoBehaviour
{
    public GameObject cannonBallPrefab;
    public Transform cannonLeft;
    public Transform cannonRight;
    public float fireRate = 3f;
    private float nextFireTime;

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireCannons();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireCannons()
    {
        Instantiate(cannonBallPrefab, cannonLeft.position, cannonLeft.rotation);
        Instantiate(cannonBallPrefab, cannonRight.position, cannonRight.rotation);
    }
}
