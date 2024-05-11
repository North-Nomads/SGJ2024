using SGJ.Combat;
using UnityEngine;

public class DestroyableContainer : MonoBehaviour, IHittable
{
    [SerializeField] private GameObject[] drop;
    [Header("Animation")]
    [SerializeField] private float timeToBonk;
    [Header("Stats")]
    [SerializeField] private float health;

    private float _timeSinceDamaged;
    public void OnEntityGotHit(float incomeDamage)
    {
        health -= incomeDamage;
        _timeSinceDamaged = 0;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Instantiate(drop[UnityEngine.Random.Range(0, drop.Length)], transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
        _timeSinceDamaged += Time.deltaTime;
        if (_timeSinceDamaged < timeToBonk)
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, 1.1f * Vector3.one, 2 * (timeToBonk / 2 - _timeSinceDamaged) / timeToBonk);
    }
}
