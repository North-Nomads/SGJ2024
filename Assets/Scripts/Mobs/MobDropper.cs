
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
            if (Random.Range(0, 1) > _generalDropChance)
                return;

            
        }


    }
}