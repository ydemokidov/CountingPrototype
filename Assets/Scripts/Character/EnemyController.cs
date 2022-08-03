using UnityEngine;

public class EnemyController : AbstractCharacterController
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject attackPoint;

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        facingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
