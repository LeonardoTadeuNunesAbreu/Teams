using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent"/> one ore. The effect is true once the agent has 3 ores.
    /// </summary>
    public class PickUpOreAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _hasOre;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile; // where we get the ore from

        private SupplyPileComponent _myTeamSupply;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasOre", false); // don't get a ore if we already have one
            AddEffect("hasOre", true); // we now have a ore
            ActionName = General_Scripts.Enums.Actions.PickUpOre;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _hasOre = false;
            _targetSupplyPile = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _hasOre;
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
        /// Checks if there is a <see cref="SupplyPileComponent"/> close to the agent with at least 1 ore.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            if (_myTeamSupply.NumOre < 1) return false;

            _targetSupplyPile = _myTeamSupply;
            Target = _myTeamSupply.gameObject;
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is completed, removes 1 ore from the <see cref="SupplyPileComponent"/> and adds it to the <see cref="GoapAgent"/>'s <see cref="BackpackComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            // we got there but there was no ore available! Someone got there first. Cannot perform action
            if (_targetSupplyPile == null || _targetSupplyPile.NumOre < 1)
                return false;

            if (StartTime == 0)
                StartTime = Time.time;

            if (StillWorking())
                return true;

            _targetSupplyPile.RemoveOres(1, false);
            //_hasOre = true;
            //TODO play effect, change actor icon
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumOre++;
            _hasOre = backpack.NumOre == 3;

            return true;
        }
    }
}
