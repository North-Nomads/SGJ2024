using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs
{
    [RequireComponent(typeof(NavMeshAgent), typeof(MobCombat))]
    public abstract class Mob : MonoBehaviour, IStateSwitcher
    {
        [SerializeField] private float maxHealth;

        protected List<MobState> AllStates;
        protected Transform Player { get; private set; }
        protected NavMeshAgent Agent { get; private set; }
        public MobCombat MobCombat { get;  protected set; }
        public MobState CurrentState { get; private set; }

        private void SetMobParameters(Transform player)
        {
            Player = player;
            Agent = GetComponent<NavMeshAgent>();
            MobCombat = new MobCombat(maxHealth);
            MobCombat.OnMobDied += HandleMobDeath;
        }

        private void Update()
        {
            CurrentState.BehaveThisState();
        }

        private void HandleMobDeath(object sender, EventArgs e) => Destroy(gameObject);

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

            Debug.Log($"{gameObject.name} is switching state: {CurrentState} => {state}");
            CurrentState = state;

            // Start and assign new state
            CurrentState.OnStateStarted();
        }
    }
}