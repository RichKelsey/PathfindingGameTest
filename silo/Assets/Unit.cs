using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour
{
    const float PathUpdateMoveThreshold = 0.5f;
    const float MinPathUpdateTime = 0.2f;
    
    public Transform target;
    public float speed = 1f;
    Vector2[] _path;
    int _targetIndex;
    
    private float _health = 100;
    private SpriteRenderer _spriteRenderer;
    private Color _gizmoColor;

    private Vector2 _currentWaypoint;
    private Vector2 _moveTarget;

    private BoxCollider2D _collider;
    
    void Start()
    {
        StartCoroutine(UpdatePath());
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _gizmoColor = _spriteRenderer.color;
    }
    
    void FixedUpdate()
    {
        HandleCollision();
    }
    
    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            StopCoroutine("FollowPath");
            _path = newPath;
            _targetIndex = 0;
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(MinPathUpdateTime);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        
        
        float sqrMoveThreshold = PathUpdateMoveThreshold * PathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(MinPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
            
        }
    }

    IEnumerator FollowPath()
    {
        _currentWaypoint = _path[0];

        while (true)
        {
            if (transform.position == new Vector3(_currentWaypoint.x, _currentWaypoint.y))
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    yield break;
                }

                _currentWaypoint = _path[_targetIndex];
            }
            
            _moveTarget = Vector2.MoveTowards(transform.position, _currentWaypoint, speed * Time.deltaTime);
            transform.position = _moveTarget;
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (_path != null)
        {
            for (int i = _targetIndex; i < _path.Length; i++)
            {
                Gizmos.color = _gizmoColor;
                Gizmos.DrawCube(_path[i], Vector2.one);

                if (i == _targetIndex)
                {
                    Gizmos.DrawLine(transform.position, _path[i]);
                }
                else
                {
                    Gizmos.DrawLine(_path[i-1], _path[i]);
                }
            }
        }
    }

    private void HandleCollision()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _collider.size, 0f);
        foreach(Collider2D hit in hits)
        {
            if (hit == _collider)
                continue;
            
            ColliderDistance2D colliderDistance = hit.Distance(_collider);
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;
        StartCoroutine(ColorFlash(Color.red, .1f));
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ColorFlash(Color color, float duration)
    {
        _spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);
        _spriteRenderer.color = _gizmoColor;
    }
}
