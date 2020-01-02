using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Place all firewood in the <see cref="BackpackComponent"/> in the nearest <see cref="SupplyPileComponent"/>
    /// </summary>
    public class DropOffFirewoodAction : GoapAction
    {
        /// <summary>
        /// The object used for the effect
        /// </summary>
        private bool _droppedOffFirewood;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile;

        private SupplyPileComponent _myTeamSupply;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasFirewood", true); // can't drop off firewood if we don't already have some
            AddEffect("hasFirewood", false); // we now have no firewood
            AddEffect("collectFirewood", true); // we collected firewood
            ActionName = General_Scripts.Enums.Actions.DropOffFirewood;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _droppedOffFirewood = false;
            _targetSupplyPile = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _droppedOffFirewood;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a supply pile so we can drop off the firewood
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
        /// Removes all firewood form the <see cref="BackpackComponent"/> and adds them to the <see cref="SupplyPileComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            if (StartTime == 0)
                StartTime = Time.time;

            if (StillWorking())
                return true;

            if (Target == null)
                return false;

            var backpack = agent.GetComponent<BackpackComponent>();

            print("dropping this amount of firewood: " + backpack.NumFirewood);

            _targetSupplyPile.AddFireWood(backpack.NumFirewood);
            _droppedOffFirewood = true;
            backpack.NumFirewood = 0;

            return true;
        }
    }
}
