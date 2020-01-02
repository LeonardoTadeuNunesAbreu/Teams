using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent"/> one log.
    /// </summary>
    public class PickUpLogAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _hasLogs;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile; // where we get the log from

        private SupplyPileComponent _myTeamSupply;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasLogs", false); // don't get a log if we already have one
            AddEffect("hasLogs", true); // we now have a log
            ActionName = General_Scripts.Enums.Actions.PickUpLog;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _hasLogs = false;
            _targetSupplyPile = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _hasLogs;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a supply pile so we can pick up the log
        }

        /// <summary>
        /// Checks if there is a <see cref="SupplyPileComponent"/> close to the agent with at least 1 log.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            if (_myTeamSupply.NumLogs < 1) return false;

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
            //print("1 if");
            //print(_targetSupplyPile);
            // we got there but there was no ore available! Someone got there first. Cannot perform action
            if (_targetSupplyPile == null || _targetSupplyPile.NumLogs < 1)
                return false;

            //print("2 if");
            if (StartTime == 0)
                StartTime = Time.time;

            //print("3 if");
            if (StillWorking())
                return true;

            _targetSupplyPile.RemoveLogs(1, false);
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumLogs++;
            _hasLogs = true;


            return true;
        }
    }
}
