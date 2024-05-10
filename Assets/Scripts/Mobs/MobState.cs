using SGJ.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs
{
    public abstract class MobState
    {
        public readonly NavMeshAgent Agent;
        public readonly Mob Self;
        public readonly Transform Transform;
        public readonly Transform Player;

        public MobState(NavMeshAgent agent, Mob self, Transform player)
        {
            Agent = agent;
            Self = self;
            Transform = self.transform;
            Player = player;
        }

        public void MoveTowards(Vector3 targetPoint) => Agent.SetDestination(targetPoint);
        public abstract void BehaveThisState();
        public abstract void OnStateStarted();
        public abstract void OnStateStopped();
    }
}