using System.Linq;
using General_Scripts.Enums;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent"/> one firewood. The effect is true once the agent has 5 ores.
    /// </summary>
    public class GrabFirewoodFromSupply : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _hasFirewood;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile; // where we get the ore from

        private SupplyPileComponent _myTeamSupply;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasFirewood", false); // don't get a firewood if we already have one
            AddEffect("hasFirewood", true); // we now have a ore
            ActionName = General_Scripts.Enums.Actions.GrabFirewoodFromSupply;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _hasFirewood = false;
            _targetSupplyPile = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _hasFirewood;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a supply pile so we can pick up the ore
        }

        /// <summary>
        /// Checks if there is a <see cref="SupplyPileComponent"/> close to the agent with at least 1 firewood.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            if (_myTeamSupply.NumFirewood < 1) return false;

            _targetSupplyPile = _myTeamSupply;
            Target = _myTeamSupply.gameObject;
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is completed, removes 1 firewood from the <see cref="SupplyPileComponent"/> and adds it to the <see cref="GoapAgent"/>'s <see cref="BackpackComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            // we got there but there was no ore available! Someone got there first. Cannot perform action
            if (_targetSupplyPile == null || _targetSupplyPile.NumFirewood < 1)
                return false;

            if (StartTime == 0)
                StartTime = Time.time;

            if (StillWorking())
                return true;

            _targetSupplyPile.RemoveFireWood(1, false);
            //TODO play effect, change actor icon
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumFirewood++;
            _hasFirewood = backpack.NumFirewood == 5;

            return true;
        }
    }
}
