using SGJ.Combat;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeSpan;

    private float _actualLifespan;
    private ObjectPool<Bullet> _pool;
    private const float Damage = 15;

    public Vector3 Speed { get; set; }

    private void OnEnable() => _actualLifespan = lifeSpan;
    public ObjectPool<Bullet> Pool
    {
        set { _pool = value; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Mob"))
        {
            if (collision.gameObject.TryGetComponent<IHittable>(out IHittable hittable))
                hittable.OnEntityGotHit(Damage);
        }
        _pool.Release(this);
    }


    private void Update()
    {
        _actualLifespan -= Time.deltaTime;
        if (_actualLifespan <= 0)
        {
            _pool.Release(this);
        }
        transform.position += Speed * Time.deltaTime;
    }
}
