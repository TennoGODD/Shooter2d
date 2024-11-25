using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    float timer;
    [SerializeField] float timeBtwAttack,attackSpeed;
    [SerializeField] int damage;

    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;

        if(CheackIfCanAttack())
        {
            
            if(timer >= timeBtwAttack)
            {
                timer = 0;
                StartCoroutine(nameof(Attack));
            }
        }
    }
    IEnumerator Attack()
    {
        Player.instance.Damage(damage);
        Vector2 origPos = transform.position;
        Vector2 plPos = Player.instance.transform.position;

        float percent = 0f;
        while(percent <= 1) 
        {
            percent += Time.deltaTime * attackSpeed;

            float intepolation = (-Mathf.Pow(percent,2)+percent)*4;
            transform.position = Vector2.Lerp(origPos,plPos,intepolation);
            yield return null;
        } 
    }
    
}
