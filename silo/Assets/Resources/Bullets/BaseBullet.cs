using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    private CircleCollider2D _bulletCollider;
    
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _speed = 10f;
        _bulletCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.right * (_speed * Time.deltaTime));
        HandleCollision();
    }

    private void HandleCollision()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _bulletCollider.radius);
        foreach(Collider2D hit in hits)
        {
            if (hit == _bulletCollider)
                continue;
            
            ColliderDistance2D colliderDistance = hit.Distance(_bulletCollider);
            if (colliderDistance.isOverlapped)
            {
                Destroy(gameObject);
            }

            if (hit.CompareTag("Enemy"))
            {
                hit.gameObject.GetComponent<Unit>().TakeDamage(25);
                Destroy(gameObject);
            }
        }
    }
}
