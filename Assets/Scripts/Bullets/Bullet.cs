using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeSpan;

    private float _actualLifespan;
    private ObjectPool<Bullet> _pool;

    public Vector3 Speed { get; set; }

    private void OnEnable()
    {
        _actualLifespan = lifeSpan;
    }
    public ObjectPool<Bullet> Pool
    {
        set { _pool = value; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Check layer and deal damage
        _pool.Release(this);
    }

    private void OnCollisionStay(Collision collision)
    {
        
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
