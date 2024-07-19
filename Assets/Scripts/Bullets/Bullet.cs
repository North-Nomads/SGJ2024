
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

    public override event EventHandler OnHitEvent =  delegate { };

    private void OnEnable() => _actualLifespan = lifeSpan;


    public  override float Speed { get; set; }
    public override Vector3 FlyDirection { get; set ; }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Mob"))
        {
            if (collision.gameObject.TryGetComponent(out IHittable hittable))
                hittable.HandleHit(Damage);
        }
        OnHitEvent.Invoke(this, null);
    }


    private void Update()
    {
        _actualLifespan -= Time.deltaTime;
        if (_actualLifespan <= 0)
        {
            //OnHitEvent.Invoke(this, null);
        }
        KeepMoving();
    }

    public override  void KeepMoving()
    {
        Debug.Log($"[{this.gameObject.name}] Flying: {FlyDirection.magnitude}");
        transform.position += FlyDirection * Time.deltaTime;
    }

    public override IProjectile OnInstantiated(Vector3 position, float speed, Vector3 normalizedFlyDirection)
    {
        var bullet = Instantiate(this, position, Quaternion.FromToRotation(Vector3.forward, position));
        bullet.Speed = speed;
        bullet.FlyDirection = speed * normalizedFlyDirection;
        Debug.Log($"Instatntiating [{this.gameObject.name}] with FlyDirection:{FlyDirection.magnitude}");
        return bullet;
    }

    public override void OnReactivated(Vector3 position, float speed, Vector3 normalizedFlyDirection)
    {
        transform.position = position;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, position);
        Speed = speed;
        FlyDirection = speed * normalizedFlyDirection;
        Debug.Log($"FlyDirection of a [{this.gameObject.name}]:{FlyDirection.magnitude}");
    }
}
