using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float ShootingTime;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] bullets;
    private Rigidbody2D body;
    private Animator animator;
    private bool grounded = true;
    //public SpriteRenderer spriteRenderer;
    public Sprite aimHorizontal;
    public bool shooting = false;
    private bool boosted = false;
    private float boostTimer = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * Speed, body.velocity.y);

        if (horizontalInput > 0.01f && shooting == false)
        {
            transform.localScale = new Vector3(0.05f, 0.05f, 1);
        }else if(horizontalInput < -0.01f && shooting == false)
        {
            transform.localScale = new Vector3(-0.05f, 0.05f ,1);
        }

        if (Input.GetKey(KeyCode.Space) && grounded == true && shooting == false)
        {
            Jump();
            
        }

       if(boosted)
        {
            boostTimer += Time.deltaTime;
            if(boostTimer >= 10)
            {
                this.Speed = 5;
                boostTimer = 0;
                boosted = false;
            }
        }
        //Seting the animation when running
        animator.SetBool("Running", horizontalInput != 0);
        animator.SetBool("Grounded", grounded);

        if (Input.GetMouseButtonDown(0) && shooting == false && grounded)
        {
            shooting = true;
            StartCoroutine(showAimingAnimation());
            ShootHorizontally();
        }
    }

    public void BoostSpeed(float speed)
    {
        this.Speed = speed;
        this.boosted = true;
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, 15);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }



    //Shooting part of the script:

    //Shoot horizontally:
    public void ShootHorizontally()
    {
        bullets[FindBullet()].transform.position = firePoint.position;
        bullets[FindBullet()].GetComponent<Bullet>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++){
            if (!bullets[i].activeInHierarchy) return i;
        }
        return 0;
    }



    public IEnumerator showAimingAnimation()
    {
        animator.enabled = false;
        body.constraints = RigidbodyConstraints2D.FreezePositionX;
        GetComponent<SpriteRenderer>().sprite = aimHorizontal;
        yield return new WaitForSeconds(ShootingTime);
        animator.enabled = true;
        shooting = false;
        body.constraints = RigidbodyConstraints2D.None;
    }

}
