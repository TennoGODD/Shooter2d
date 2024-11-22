using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float deathTime;
    [SerializeField] int damage;



    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        Invoke(nameof(Death),deathTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Wall")
            Death();
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Damage(damage);
            Death();
        }
    }
    void Death()
    {
        Destroy(gameObject);
    }
}
