using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    public float playerSpeed = 1f;
    public float currentSpeed;

    public Vector2 playerDirection;

    private bool isWalking;
    private Animator playerAnimator;

    // Player olhando para direita
    private bool playerFacingRight = true;

    //Variavel contadora
    public int punchCount;

    //Tempo de ataque
    private float timeCross = 0.75f;

    private bool comboControl;

    // Indica se o player esta morto
    private bool isDead;

    // Propriedades para a UI
    // Public para ser acessivel em outro script
    public int maxHealth = 10;
    public int currentHealth;
    public Sprite playerImage;

    void Start()
    {
        //Obtem e inicializa as propriedades do RigiBody2D
        playerRigidBody = GetComponent<Rigidbody2D>();

        // Obtem e inicializa as propriedades do animator
        playerAnimator = GetComponent<Animator>();

        currentSpeed = playerSpeed;

        // Iniciar a vida do Player
        currentHealth = maxHealth;

    }


    private void Update()
    {
            PlayerMove();
            UpdateAnimator();



        if (Input.GetKeyDown(KeyCode.J))
        {
            //Iniciar o temporizador
            if (punchCount < 2)
            {
                PlayerJab();
                punchCount++;

                if (!comboControl)
                {
                    StartCoroutine(CrossController());

                }

                else if (punchCount >= 2)
                {
                    PlayerCross();
                    punchCount = 0;
                }
                //Parando o temporizador
                StopCoroutine(CrossController());

            }
            
        }
    }

    // Fixed Update geralmente é utilizada para implementação de física no jogo, por ter uma execução padronizada em diferentes dispositivos
    private void FixedUpdate()
    {
        // Verificar se o Player está em movimento
        if (playerDirection.x != 0 || playerDirection.y != 0)
        {
            isWalking = true;
        }
        else 
        {
            isWalking= false;
        }

        //O que da o movimento para o player
        //playerRigidBody.MovePosition(playerRigidBody.position + playerSpeed * Time.fixedDeltaTime * playerDirection);
        playerRigidBody.MovePosition(playerRigidBody.position + currentSpeed * Time.fixedDeltaTime * playerDirection);

    }

    void PlayerMove()
    {
        //Pega a entrada do jogador e cria um Vector2 para usar no playerDirection
        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Se o Player se movimenta para ESQUERDA e está olhando para a DIREITA
        if (playerDirection.x < 0 && playerFacingRight)
        {
            Flip();
        }

        // Se o Player se movimenta para DIREITA e está olhando para a ESQUERDA
        else if (playerDirection.x > 0 && !playerFacingRight)
        {
            Flip();
        }

    }

    void UpdateAnimator()
    {
        // Definir o valor do parâmetro do animator, igual à propriedade isWalking
        playerAnimator.SetBool("isWalking", isWalking);
    }

    void Flip()
    { 
        // Vai girar o sprite do player em 180 graus

        // Inverter o valor da variável playerfacingRight
        playerFacingRight = !playerFacingRight;

        // Girar o sprite do player em 180 no eixo Y
        // X, Y, Z
        transform.Rotate(0, 180, 0);

    }

    void PlayerJab()
    {
        //Acessa a animação do Jab
        //Ativa o gatilho de ataque jab
        playerAnimator.SetTrigger("isJab");
    }

    void PlayerCross()
    {
        playerAnimator.SetTrigger("isCross");
    }

    IEnumerator CrossController()
    {
        comboControl = true;
        yield return new WaitForSeconds(timeCross);
        punchCount = 0;
        comboControl = false;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = playerSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            playerAnimator.SetTrigger("HitDamage");
            FindFirstObjectByType<UIManager>().UpdatePlayerHealth(currentHealth);
        }
    }

}
