using Assets.Scripts.Mobs;
using SGJ.Combat;
using SGJ.GameItems;
using SGJ.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Mob : MonoBehaviour, IStateSwitcher, IHittable
    {
        private const string PlayerTag = "Player";

        [Header("Drops")]
        [SerializeField, Range(0, 1)] private float generalDropChance;
        [SerializeField] private Probabilities[] dropChances;

        [Header("Common values")]
        [SerializeField] private float maxHealth;
        [SerializeField] private float defaultSpeed;
        [SerializeField] private float damage;
        

        protected List<MobState> AllStates;
        private PlayerUI _playerUI;
        private MobDropper _mobDropper;
        private float _timeSinceDamaged;
        private float timeToBonk = 0.3f;

        protected PlayerController Player { get; private set; }
        protected NavMeshAgent Agent { get; private set; }
        public MobCombat MobCombat { get;  protected set; }
        public MobState CurrentState { get; protected set; }
        public float DefaultSpeed => defaultSpeed;
        public MobDropper MobDropper => _mobDropper;

        public void SetMobParameters(PlayerController player, MobSpawner owner)
        {
            Player = player;
            Agent = GetComponent<NavMeshAgent>();
            MobCombat = new MobCombat(maxHealth, this);
            MobCombat.OnMobDied += owner.HandleMobDeath;
            Agent.speed = defaultSpeed;

            _mobDropper = new MobDropper(this, generalDropChance, dropChances);

            MobCombat.OnMobHit += AnimateBonk;
        }

        private void Update()
        {
            CurrentState.BehaveThisState();

            BonkCounter();
        }

        private void BonkCounter()
        {
            _timeSinceDamaged += Time.deltaTime;
            if (_timeSinceDamaged <= timeToBonk)
                gameObject.transform.GetChild(0).transform.localScale = Vector3.Lerp(Vector3.one, 0.8f * Vector3.one, 2 * (timeToBonk / 2 - _timeSinceDamaged) / timeToBonk);
            Debug.Log(gameObject.transform.GetChild(0));
        }

        private void OnValidate()
        {
            if (dropChances.Length == 0)
                return;

            float total = 0f;
            for (int i = 0; i < dropChances.Length - 1; i++)
            {
                total += dropChances[i].DropChance;
                if (total > 1)
                    throw new Exception("Total chances are >1. Sum must be in range [0, 1]");
            }
            dropChances[^1].DropChance = 1 - total;
        }

        /// <summary>
        /// Changes current mob state
        /// </summary>
        /// <typeparam name="T">New state</typeparam>
        public void SwitchState<T>() where T : MobState
        {
            // Stop and unbind previous state
            CurrentState.OnStateStopped();

            // Set new state
            var state = AllStates.FirstOrDefault(st => st is T);

            //Debug.Log($"{gameObject.name} is switching state: {CurrentState} => {state}");
            CurrentState = state;

            // Start and assign new state
            CurrentState.OnStateStarted();
        }

        private void AnimateBonk(object sender, EventArgs e)
        {
            _timeSinceDamaged = 0;
        }

        public virtual void OnEntityGotHit(float incomeDamage) => MobCombat.HandleIncomeDamage(incomeDamage);

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag))
                Player.OnEntityGotHit(damage);
        }
    }

    [Serializable]
    public class Probabilities
    {
        [SerializeField, Range(0, 1)] private float dropChance;
        [SerializeField] private ItemModel model;
        [SerializeField, Min(1)] private int minQuantity = 1;
        [SerializeField, Min(1)] private int maxQuantity = 1;

        public ItemModel ItemModel => model;
        public float DropChance 
        {
            get => dropChance;
            set => dropChance = value;
        }
        public int MinQuantity => minQuantity;
        public int MaxQuantity => maxQuantity;
    }
}