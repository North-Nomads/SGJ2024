
using SGJ.Mobs;
using UnityEngine;

namespace Assets.Scripts.Mobs
{
    public class MobDropper
    {
        private readonly Probabilities[] _dropChances;
        private readonly float _generalDropChance;
        private readonly Mob _mob;

        public MobDropper(Mob mob, float generalDropChance, Probabilities[] dropChances)
        {
            _generalDropChance = generalDropChance;
            _dropChances = dropChances;
            _mob = mob;
        }

        public void TryDropItem()
        {
            if (Random.Range(0f, 1f) > _generalDropChance || _dropChances is null || _dropChances.Length == 0)
                return;

            var random = Random.Range(0f, 1f);
            float bottomBorder = 0f;
            float topBorder;
            for (int i = 0; i < _dropChances.Length; i++)
            {
                topBorder = _dropChances[i].DropChance;
                
                if (bottomBorder > random || random > topBorder)
                {
                    bottomBorder = _dropChances[i].DropChance;
                    continue;
                }

                var itemQuantity = Random.Range(_dropChances[i].MinQuantity, _dropChances[i].MaxQuantity + 1);
                Object.Instantiate(_dropChances[i].ItemModel.Prefab,
                    _mob.transform.position + Vector3.up * 2, Quaternion.identity, null)
                    .OnObjectCreated(_dropChances[i].ItemModel.Item, itemQuantity);
            }
        }


    }
}