using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;

    //Variavel que indica se o inimigo est� vivo
    public bool isDead;

    // Variaveis para controlar o lado que o inimigo est� virado
    private bool facingRight;
    private bool previousDirectionRight;

    // Variavel para armazenar posi��o do Player
    private Transform target;

    // Variaveis para movimenta��o do inimigo
    private float enemySpeed = 0.5f;
    private float currentSpeed;

    private bool isWalking;

    // 
    private float horizontalForce;
    private float verticalForce;

    // 
    private float walkTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posi��o
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo
        currentSpeed = enemySpeed;
    }

    
    void Update()
    {
        // Verificar se o Player est� para direita ou para a esquerda
        // E determinar o lado que o do inimigo ficar� virado

        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        // Se o FacingRight for TRUE, vira inimigo em 180 no eixo Y
        // Caso ao contrario virar inimigo para esquerda

        // Se o Player est� � direita e a posi��o anterior N�O era direita (inimigo olhando para esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o Player N�O est� � direita e a posi��o anterior ERA direita (inimigo olhando para esquerda)
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Iniciar o timer do caminhar do inimigo
        walkTimer += Time.deltaTime;

        // Gerenciar a anima��o do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        // Atualiza o animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        // MOVIMENTA��O

        // Variavel para armazenar a distancia entre o Inimigo e o Player
        Vector3 targetDistance = target.position - this.transform.position;

        //Determina se a for�a horizontal deve ser negativa ou positiva
        //  5 / 5 = 1
        // -5 / 5 = -1
        horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

        // Entre 1 e 2 segundos, ser� feita uma defini��o de dire��o vertical
        if (walkTimer >= Random.Range(1f, 2f))
        {
            verticalForce = Random.Range(-1, 2);

            // Zera o time de movimenta��o para andar verticalmente novamente daqui a +- 1 seg
            walkTimer = 0;
        }

        // Caso esteja perto do Player, parar a movimenta��o
        if (Mathf.Abs(targetDistance.x) < 0.4f)
        {
            horizontalForce = 0;
        }

        // Aplica a velocidade do inimigo fazendo o movimentar
        rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);
    }

    void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
    }
}
