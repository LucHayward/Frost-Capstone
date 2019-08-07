using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float range = 25f;
    public Transform skull;
    public Transform origin;

    public SpawnManager spawnManager;
    
    public ParticleSystem attackParticles;
    public AudioSource attackClip;

    private void Awake()
    {
        skull.gameObject.GetComponent<ParticleSystem>().Play();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(origin.position, origin.forward, out hit, range)){
            Debug.DrawRay(origin.position, origin.forward, Color.green, 3);
            Debug.Log(hit.transform.name);

            skull.gameObject.GetComponent<ParticleSystem>().Play(true);

            //If the object can be destroyed, play the attached particle system and 
            // destroy it after that completes.
            Debug.Log(hit.transform.tag);
            if (hit.transform.CompareTag("Destructible"))
            {
                //Debug.Log("Destructible");
                var exp = hit.transform.GetComponent<ParticleSystem>();
                bool isBrazier = hit.transform.name.StartsWith("Brazier");
                if(!isBrazier) exp.Play();
                Destroy(hit.transform.gameObject, isBrazier ? 0 : exp.main.duration);

                attackClip.Play();

                spawnManager.SpawnNew();
            }
        }
    }

    
}
