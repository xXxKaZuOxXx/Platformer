using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float Speed;
    [SerializeField] private bool _invert;

    protected Rigidbody2D Rigidbody;
    protected int Direction;
    protected virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        var mod = _invert ? -1 : 1;
        Direction = mod * transform.lossyScale.x > 0 ? 1 : -1;
        StartCoroutine(TimeOfLife());

    }

    private IEnumerator TimeOfLife()
    {
        yield return new WaitForSeconds(6);
        Destroy(Rigidbody.gameObject);
        yield break;
    }
}
