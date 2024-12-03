using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;

    //Variavel que indica se o inimigo está vivo
    public bool isDead;

    // Variaveis para controlar o lado que o inimigo está virado
    private bool facingRight;
    private bool previousDirectionRight;

    // Variavel para armazenar posição do Player
    private Transform target;

    // Variaveis para movimentação do inimigo
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

        // Buscar o Player e armazenar sua posição
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo
        currentSpeed = enemySpeed;
    }

    
    void Update()
    {
        // Verificar se o Player está para direita ou para a esquerda
        // E determinar o lado que o do inimigo ficará virado

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

        // Se o Player está à direita e a posição anterior NÃO era direita (inimigo olhando para esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o Player NÃO está à direita e a posição anterior ERA direita (inimigo olhando para esquerda)
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Iniciar o timer do caminhar do inimigo
        walkTimer += Time.deltaTime;

        // Gerenciar a animação do inimigo
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
        // MOVIMENTAÇÃO

        // Variavel para armazenar a distancia entre o Inimigo e o Player
        Vector3 targetDistance = target.position - this.transform.position;

        //Determina se a força horizontal deve ser negativa ou positiva
        //  5 / 5 = 1
        // -5 / 5 = -1
        horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

        // Entre 1 e 2 segundos, será feita uma definição de direção vertical
        if (walkTimer >= Random.Range(1f, 2f))
        {
            verticalForce = Random.Range(-1, 2);

            // Zera o time de movimentação para andar verticalmente novamente daqui a +- 1 seg
            walkTimer = 0;
        }

        // Caso esteja perto do Player, parar a movimentação
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
