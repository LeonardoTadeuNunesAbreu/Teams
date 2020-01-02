using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent" /> an <see cref="GoapAction" /> to pick up a tool. Has the effect &lt;"hasTool", true&gt;
    /// </summary>
    public class PickUpToolAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _hasTool;
        /// <summary>
        /// The target of this action
        /// </summary>
        private SupplyPileComponent _targetSupplyPile; // where we get the tool from

        private SupplyPileComponent _myTeamSupply;


        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasTool", false); // don't get a tool if we already have one
            AddEffect("hasTool", true); // we now have a tool
            ActionName = General_Scripts.Enums.Actions.PickUpTool;
            _myTeamSupply = GameObject.Find("SupplyPile team A").GetComponent<SupplyPileComponent>();
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            StartTime = 0;
            _hasTool = false;
            _targetSupplyPile = null;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _hasTool;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a supply pile so we can pick up the tool
        }

        /// <summary>
        /// Checks if there is a <see cref="SupplyPileComponent"/> close to the agent with at least 1 tool.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            if (_myTeamSupply.NumTools < 1) return false;

            _targetSupplyPile = _myTeamSupply;
            Target = _targetSupplyPile.gameObject;
            return true;
        }

        /// <summary>
        /// Removes 1 tool from the <see cref="SupplyPileComponent"/> and add it to the <see cref="GoapAgent"/>'s <see cref="BackpackComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            // we got there but there was no tool available! Someone got there first. Cannot perform action
            if (_targetSupplyPile == null || _targetSupplyPile.NumTools <= 0)
                return false;

            if (StartTime == 0)
                StartTime = Time.time;

            if (StillWorking())
                return true;

            _targetSupplyPile.RemoveTools(1, false);
            _hasTool = true;

            // create the tool and add it to the agent
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.Tool.Strength = 1; // reset tool strenght

            return true;
        }
    }
}


