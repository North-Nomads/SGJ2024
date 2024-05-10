using Assets.Scripts.Mobs;
using SGJ.Combat;
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

        protected PlayerController Player { get; private set; }
        protected NavMeshAgent Agent { get; private set; }
        public MobCombat MobCombat { get;  protected set; }
        public MobState CurrentState { get; protected set; }
        public float DefaultSpeed => defaultSpeed;

        public void SetMobParameters(PlayerController player, MobSpawner owner)
        {
            Player = player;
            Agent = GetComponent<NavMeshAgent>();
            MobCombat = new MobCombat(maxHealth, this);
            MobCombat.OnMobDied += owner.HandleMobDeath;
            Agent.speed = defaultSpeed;

            _mobDropper = new MobDropper(this, generalDropChance, dropChances);
        }

        private void Update()
        {
            CurrentState.BehaveThisState();
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
        [SerializeField] private Items item;
        [SerializeField, Range(0, 1)] private float dropChance;

        public Items Item => item;
        public float DropChance 
        {
            get => dropChance;
            set => dropChance = value;
        }
    }
}