using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Place all ores in the <see cref="BackpackComponent"/> in the nearest <see cref="SupplyPileComponent"/>
    /// </summary>
    public class DropOffLogsAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _droppedOffLogs;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile; // where we drop off the logs

        private SupplyPileComponent _myTeamSupply;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasLogs", true); // can't drop off logs if we don't already have some
            AddEffect("hasLogs", false); // we now have no logs
            AddEffect("collectLogs", true); // we collected logs
            ActionName = General_Scripts.Enums.Actions.DropOffLogs;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _droppedOffLogs = false;
            _targetSupplyPile = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _droppedOffLogs;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a supply pile so we can drop off the logs
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
        /// Removes all logs form the <see cref="BackpackComponent"/> and adds them to the <see cref="SupplyPileComponent"/>.
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

            _targetSupplyPile.AddLogs(backpack.NumLogs);
            _droppedOffLogs = true;
            backpack.NumLogs = 0;

            return true;
        }
    }
}
