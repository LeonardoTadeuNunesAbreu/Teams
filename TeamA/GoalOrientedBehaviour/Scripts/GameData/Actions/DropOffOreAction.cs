using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Place all ores in the <see cref="BackpackComponent"/> in the nearest <see cref="SupplyPileComponent"/>
    /// </summary>
    public class DropOffOreAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _droppedOffOre;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile; // where we drop off the ore

        private SupplyPileComponent _myTeamSupply;


        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasOre", true); // can't drop off ore if we don't already have some
            AddEffect("hasOre", false); // we now have no ore
            AddEffect("collectOre", true); // we collected ore
            ActionName = General_Scripts.Enums.Actions.DropOffOre;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _droppedOffOre = false;
            _targetSupplyPile = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _droppedOffOre;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a supply pile so we can drop off the ore
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
        /// Removes all ores form the <see cref="BackpackComponent"/> and adds them to the <see cref="SupplyPileComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            var backpack = agent.GetComponent<BackpackComponent>();

            if (StartTime == 0)
                StartTime = Time.time;

            if (StillWorking())
                return true;
            
            _targetSupplyPile.AddOres(backpack.NumOre);
            _droppedOffOre = true;
            backpack.NumOre = 0;

            return true;
        }
    }
}
