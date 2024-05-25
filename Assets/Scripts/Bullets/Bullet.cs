
using SGJ.Combat;
using SGJ.Projectiles;
using SGJ.Weapons;
using System;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] private float lifeSpan;

    private float _actualLifespan;
    private const float Damage = 15;

    public override  event EventHandler OnHitEvent;

    private void OnEnable() => _actualLifespan = lifeSpan;


    public  override float Speed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override Vector3 FlyDirection { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Mob"))
        {
            if (collision.gameObject.TryGetComponent<IHittable>(out IHittable hittable))
                hittable.HandleHit(Damage);
        }
        OnHitEvent.Invoke(this, null);
    }


    private void Update()
    {
        _actualLifespan -= Time.deltaTime;
        if (_actualLifespan <= 0)
        {
            OnHitEvent.Invoke(this, null);
        }
        KeepMoving();
    }

    public override  void KeepMoving()
    {
        transform.position += FlyDirection * Time.deltaTime;
    }

    public override  void OnInstantiated(Vector3 position, float speed, Vector3 normalizedFlyDirection)
    {
        Instantiate(this, position, Quaternion.FromToRotation(Vector3.forward, position));
        Speed = speed;
        FlyDirection = speed * normalizedFlyDirection;
    }
}
