using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float speed, stopDistance, distanceToRunOut;
    [SerializeField] GameObject hitEffect;
    Rigidbody2D rb;
    Animator anim;
    Player player;
    SpriteRenderer spr;
    bool isDeath = false;
    bool canAttack = false;

    Vector3 addRandPosToGo;

    public virtual void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = Player.instance;
        spr = GetComponent<SpriteRenderer>();

        StartCoroutine(nameof(SetRandomPos));
    }

    public virtual void Update() {
        if (isDeath) return;
        Scale(player.transform.position);
    }

    private void FixedUpdate() {
        if (isDeath) return;
        if(Vector2.Distance(transform.position,player.transform.position)> stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, speed * Time.fixedDeltaTime);
            anim.SetBool("run",true);
            canAttack = false;
        }
        else if(Vector2.Distance(transform.position,player.transform.position)< distanceToRunOut)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGo, -speed * Time.fixedDeltaTime);
            canAttack = false;
        }
        else 
        {
            anim.SetBool("run",false);
            canAttack = true;    
        }
    }

    IEnumerator SetRandomPos()
    {
        addRandPosToGo = new Vector3(Random.Range(-2,2),Random.Range(-2,2));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(nameof(SetRandomPos));
    }

    void Scale(Vector3 pos){
        if (pos.x >= transform.position.x) spr.flipX = false;
        else spr.flipX = true;
    }

    public void Damage(int damage){
        Instantiate(hitEffect,transform.position,Quaternion.identity);
        if (isDeath) return;
        health -= damage;
        if(health <= 0) Death();
    }

    void Death()
    {
        isDeath = true;
        anim.SetTrigger("death");
    }
    public IEnumerator DestroyObj()
    {
        while(spr.color.a != 0)
        {
            float p = spr.color.a;
            spr.color = new Color(255f,255f,255f,p - 0.1f);
            yield return new WaitForSeconds(0.1f);

        }
            Destroy(gameObject);
    }
    public virtual bool CheackIfCanAttack()
    {
        return canAttack;
    }
}
