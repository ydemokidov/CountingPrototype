using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    //private AudioSource smashSound;
    private GameObject gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        //smashSound = gameObject.GetComponent<AudioSource>();
    }

    public void DeactivateTarget()
    {
        //smashSound.Play();
        Instantiate(explosion, gameObject.transform.position,gameObject.transform.rotation);
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            DeactivateTarget();
            gameManager.GetComponent<GameManager>().TargetGrounded();
        }
    }

}
