﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGP.Control
{
    public abstract class State
    {
        protected Dictionary<Transition, StateID> stateMap = new Dictionary<Transition, StateID>();

        protected StateID stateID;

        public StateID ID { get { return stateID; } }

        protected Vector3 destinationPos;
        protected bool aggrevated; 

        public void AddTransition(Transition transition, StateID id)
        {
            //Check if the map already has this transition added
            if (stateMap.ContainsKey(transition))
            {
                Debug.LogError("[Error: State.cs]: Transition is already inside stateMap");
                return;
            }

            stateMap.Add(transition, id);
        }

        public void DeleteTransition(Transition transition)
        {
            if (stateMap.ContainsKey(transition))
            {
                stateMap.Remove(transition);
                return;
            }
            Debug.LogError("[Error: State.cs]: Transition passed in was not in this states map");
        }

        public StateID GetOutputState(Transition transition)
        {
            if (stateMap.ContainsKey(transition))
            {
                return stateMap[transition];
            }

            Debug.LogError("[Error: StateMachine.cs]: " + transition + " Transition passed to the state was not found");
            return StateID.None;
        }

        protected bool InFOV(Transform player, Transform npc)
        {
            Vector3 rayDirection = player.position - npc.position;

            if (Vector3.Angle(rayDirection, npc.forward) < 45)
            {
                return true;
            }
            return false;
        }
        public State(StateID id)
        {
            stateID = id;
        }

        public virtual void OnEntry() { }

        public virtual void OnExit() { }

        //This method checks if we should transition out of this state
        public abstract void Reason(Transform player, Transform npc);

        //This method acts on the state
        public abstract void Act(Transform player, Transform npc);


    }
}

