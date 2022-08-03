using System.Collections;
using UnityEngine;

public abstract class AbstractCharacterController : MonoBehaviour
{
    [SerializeField] protected int damage = 20;
    [SerializeField] protected int health = 100;
    [SerializeField] protected GameObject gameManagerObject;
    [SerializeField] protected GameObject attackSound;
    [SerializeField] protected GameObject footStepSound;

    protected bool facingRight;
    // Start is called before the first frame update

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health + " left...");
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " dies...");
        Destroy(gameObject);
    }

    public abstract void Attack();

    protected void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected void ProcessAttack(GameObject attackPoint, float attackRange,LayerMask layerMask)
    {
        GameManager manager = gameManagerObject.GetComponent<GameManager>();
        Collider2D[] attackedObjects = Physics2D.OverlapCircleAll(new Vector2(attackPoint.transform.position.x, attackPoint.transform.position.y), attackRange, layerMask);
        Debug.Log("Found " + attackedObjects.Length + " objects");
        foreach (var attackedObject in attackedObjects)
        {
            manager.DestroyTarget(attackedObject.gameObject);
            manager.AddScore(1);
        }
    }

    protected IEnumerator ProcessAttackWithDelay(GameObject attackPoint, float attackRange, LayerMask layerMask,float delay)
    {
        yield return new WaitForSeconds(delay);
        ProcessAttack(attackPoint, attackRange, layerMask);
    }

}
