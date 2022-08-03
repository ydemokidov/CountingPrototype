using System.Collections;
using UnityEngine;

public class PlayerController : AbstractCharacterController
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float attack1Range = 0.5f;
    [SerializeField] private float attack2Range = 0.3f;
    [SerializeField] private GameObject attack1Point;
    [SerializeField] private GameObject attack2Point;

    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private float horizontalInput;
    private bool isOnGround;
    private AudioSource footStepsAudio;

    private bool comboPossible = false;
    private int comboStep = 0;

    private bool canDashing = true;
    private bool isDashing;
    [SerializeField] private float dashingPower;
    [SerializeField] private float dashingTime;
    [SerializeField] private float dashingCooldown;
    [SerializeField] private TrailRenderer trailRenderer;
    
    void Start()
    {
        facingRight = true;
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        playerAnimator = gameObject.GetComponent<Animator>();
        playerAnimator.SetBool("isOnGroundb", true);
        footStepsAudio = footStepSound.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerObject.GetComponent<GameManager>().IsGameReady())
        {
            if (isDashing)
            {
                return;
            }

            bool attacking = playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
            if (!attacking || !isOnGround)
            {
                HandleMovement();
            }

            if (isOnGround)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    playerAnimator.SetBool("isOnGroundb", false);
                    playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    isOnGround = false;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDashing)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            playerAnimator.SetBool("isOnGroundb", true);
        }
    }

    private void HandleMovement()
    {
         horizontalInput = Input.GetAxis("Horizontal");
         float horizontalMove = speed * horizontalInput;

        if (horizontalMove != 0)
        {
            if (horizontalMove > 0 && !facingRight)
            {
                Flip();
            }
            else if (horizontalMove < 0 && facingRight)
            {
                Flip();
            }

            playerAnimator.SetFloat("runningSpeedf", 1f);
            transform.Translate(Vector3.right * horizontalMove * Time.deltaTime);
            if (!footStepsAudio.isPlaying && isOnGround)
            {
                footStepsAudio.Play();
            }
        }
        else
        {
            playerAnimator.SetFloat("runningSpeedf", 0.25f);
        }
    }

    override
    public void Attack()
    {
        if (comboStep == 0)
        {
            playerAnimator.Play("Attack1");
            StartCoroutine(ProcessAttackWithDelay(attack1Point, attack1Range, LayerMask.GetMask("Enemies"),0.2f));
            attackSound.GetComponent<AudioSource>().Play();
            comboStep = 1;
            return;
        }
        if (comboPossible)
        {
            if (comboStep == 1)
            {
                playerAnimator.Play("Attack2");
                StartCoroutine(ProcessAttackWithDelay(attack2Point, attack2Range, LayerMask.GetMask("Enemies"), 0.2f));
                attackSound.GetComponent<AudioSource>().Play();
            }
        }
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack1Point.transform.position, attack1Range);
        Gizmos.DrawWireSphere(attack2Point.transform.position, attack2Range);
    }

    private IEnumerator Dash()
    {
        canDashing = false;
        isDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0;
        playerRb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        playerRb.gravityScale = originalGravity;
        playerRb.velocity = Vector3.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDashing = true;
    }
}
