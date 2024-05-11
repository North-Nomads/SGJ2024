using SGJ.Combat;
using SGJ.Mobs;
using UnityEngine;

public class DestroyableContainer : MonoBehaviour, IHittable
{
    [SerializeField] private float generalDropChance;
    [SerializeField] private Probabilities[] dropChances;
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
        TryDropItem();
        Destroy(gameObject);
    }

    public void TryDropItem()
    {
        if (Random.Range(0f, 1f) > generalDropChance || dropChances is null || dropChances.Length == 0)
            return;

        var random = Random.Range(0f, 1f);
        float bottomBorder = 0f;
        float topBorder;
        for (int i = 0; i < dropChances.Length; i++)
        {
            topBorder = dropChances[i].DropChance;

            if (bottomBorder > random || random > topBorder)
            {
                bottomBorder = dropChances[i].DropChance;
                continue;
            }

            var itemQuantity = Random.Range(dropChances[i].MinQuantity, dropChances[i].MaxQuantity + 1);
            Instantiate(dropChances[i].ItemModel.Prefab, transform.position + Vector3.up * 1.5f, transform.rotation, null)
                .OnObjectCreated(dropChances[i].ItemModel.Item, itemQuantity);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        _timeSinceDamaged += Time.deltaTime;
        if (_timeSinceDamaged < timeToBonk)
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, 0.9f * Vector3.one, 2 * (timeToBonk / 2 - _timeSinceDamaged) / timeToBonk);
    }
}
