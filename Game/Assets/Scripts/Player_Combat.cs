using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Reference to the animator for player animations
    public Animator animator;

    // Reference to the transform that represents the attack point
    public Transform attackPoint;
    public Transform attackPoint1;
    public BarHandler barHandler;

    // Layer mask for detecting enemies
    public LayerMask enemyLayers;
    
    private float range;
    private float _attackRate = 1.8f; // Attacks per sec
    private int _specialAttackCounter; // Define a counter variable at the class level
    public int _resolveBuildUp; // For powerup
    float _nextAttack;
    private Rigidbody2D _rb2D;
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Attack2 = Animator.StringToHash("Attack_Sp");

    void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool isMoving = _rb2D.velocity.magnitude > 0.1f;

        // check if attack is alowed after past time
        if (Input.GetKeyDown(KeyCode.S) && _resolveBuildUp >= 100)
        {
            if (_resolveBuildUp < 100) return;
            animator.SetTrigger(Attack2);
            //reset bar to 0
            _resolveBuildUp = 0;
            barHandler.GetPower(_resolveBuildUp);
        }
        else if (Time.time >= _nextAttack && !isMoving)
        {
            // Check for the attack input (e.g., pressing Space key)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                animator.SetTrigger(Attack1);
                _nextAttack = Time.time + 1f / _attackRate;
            }
        }

    }
    
   
    
    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 1f, enemyLayers);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            // Check if the collider is not a trigger collider
            if (!enemyCollider.isTrigger)
            {
                // Check if the collider has a component and take damage
                Goblin enemy = enemyCollider.GetComponent<Goblin>();
                if (enemy != null)
                {
                    _resolveBuildUp+=10;
                    barHandler.GetPower(_resolveBuildUp);
                    enemy.TakeDamage(20);
                }

                FlyingEye enemy2 = enemyCollider.GetComponent<FlyingEye>();
                if (enemy2 != null)
                {
                    _resolveBuildUp+=10;
                    barHandler.GetPower(_resolveBuildUp);
                    enemy2.TakeDamage(20);
                }
                /*
                Skeleton enemy3 = enemyCollider.GetComponent<Skeleton>();
                if (enemy3 != null)
                {
                    barHandler.GetPower(_resolveBuildUp);
                    // check if skeleton is shielding
                    if (enemy3.TakeDamage(20))
                    {
                        _resolveBuildUp+=10;
                    }
                }
                */

            }
        }
    }
    
    

    // Called when animation is played
    private void SpecialAttack()
    {
        _specialAttackCounter++;

        if (_specialAttackCounter == 1)
        {
            range = 1.0f; // Change the radius for the 1st call
        }
        else if (_specialAttackCounter == 2)
        {
            range = 1.2f; // Change the radius for the 2nd call
        }
        else
        {
            _specialAttackCounter = 0;
            range = 1.5f;
        }
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint1.position, range, enemyLayers);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            // Check if the collider is not a trigger collider
            if (!enemyCollider.isTrigger)
            {
                // Check if the collider has a component and take damage
                Goblin enemy = enemyCollider.GetComponent<Goblin>();
                if (enemy != null)
                {
                    enemy.TakeDamage(20);
                }

                FlyingEye enemy2 = enemyCollider.GetComponent<FlyingEye>();
                if (enemy2 != null)
                {
                    enemy2.TakeDamage(20);
                }
                /*
                Skeleton enemy3 = enemyCollider.GetComponent<Skeleton>();
                if (enemy3 != null)
                {
                    _resolveBuildUp+=10;
                    barHandler.GetPower(_resolveBuildUp);
                    enemy3.TakeDamage(20);
                }
                */
            }
        }
    }
}
