using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Place one tool in the nearest <see cref="SupplyPileComponent"/>
    /// </summary>
    public class DropOffToolsAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _droppedOffTools;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile; // where we drop off the  tools

        private SupplyPileComponent _myTeamSupply;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasNewTools", true); // can't drop off tools if we don't already have some
            AddEffect("hasNewTools", false); // we now have no tools
            AddEffect("collectTools", true); // we collected tools
            ActionName = General_Scripts.Enums.Actions.DropOffTools;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _droppedOffTools = false;
            _targetSupplyPile = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _droppedOffTools;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a supply pile so we can drop off the tools
        }

        /// <summary>
        /// Checks if there is a <see cref="SupplyPileComponent"/> close to the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            _targetSupplyPile = _myTeamSupply;
            Target = _myTeamSupply.gameObject;
            return true;
        }

        /// <summary>
        /// Adds 2 tools to the <see cref="SupplyPileComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            if (StartTime == 0)
                StartTime = Time.time;

            if (StillWorking())
                return true;

            var backpack = GetComponent<BackpackComponent>();
            _targetSupplyPile.AddTools(backpack.NumOfForgedTools);
            backpack.NumOfForgedTools = 0;
            _droppedOffTools = true;

            return true;
        }
    }
}