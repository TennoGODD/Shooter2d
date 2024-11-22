using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform shootPos;
    [SerializeField] float timebtwshoot;
    [SerializeField] float health;
    [SerializeField] Sprite[] spritesMuzzleFlash;
    [SerializeField] SpriteRenderer muzzleFlashSpR;
    float _time = 0f;
    Vector2 moveVelocity;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spr;
    public static Player instance;
    bool isDeath = false;

    private void Awake() {
        instance = this;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate() 
    {
        if (isDeath) return;
        Move();
    }

    private void Update()
    {
        if (isDeath) return;
        _time += Time.deltaTime;
        if(Input.GetMouseButtonDown(0) && _time >= timebtwshoot)
        {
            Shoot();
            _time = 0;
        }
    }

    void Move()
    {
        if (isDeath) return;
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

        if(moveInput != Vector2.zero)
            anim.SetBool("run",true);
        else
            anim.SetBool("run",false);
        
        ScalePlayer(moveInput.x);

        moveVelocity = moveInput.normalized * speed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime );
    }
    
    void ScalePlayer(float x)
    {
        if(x == 1)
            spr.flipX = false;
        else if (x == -1)
            spr.flipX = true;
    }

    void Shoot()
    {
        Instantiate(bullet,shootPos.position,shootPos.rotation);
        StartCoroutine(nameof(SetMuzzleFlash));
    }
    IEnumerator SetMuzzleFlash()
    {
        muzzleFlashSpR.enabled = true;
        muzzleFlashSpR.sprite = spritesMuzzleFlash[UnityEngine.Random.Range(0,spritesMuzzleFlash.Length)];

        yield return new WaitForSeconds(0.1f);
        muzzleFlashSpR.enabled = false;
    }

    public void Damage(int damage)
    {
        health -= damage;
        CameraFollow.instance.cameraShake();
        Instantiate(hitEffect,transform.position,Quaternion.identity);
        if(health <= 0) Death();
    }
    void Death()
    {
        isDeath = true;
        anim.SetTrigger("death");
    }
}
