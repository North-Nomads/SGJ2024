using SGJ.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Mob : MonoBehaviour, IStateSwitcher, IHittable
    {
        private const string PlayerTag = "Player";

        [SerializeField] private float maxHealth;
        [SerializeField] private float defaultSpeed;
        [SerializeField] private float damage;

        protected List<MobState> AllStates;
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
        }

        private void Update()
        {
            CurrentState.BehaveThisState();
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
}