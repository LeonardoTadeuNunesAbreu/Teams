using System.Linq;
using General_Scripts.Enums;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Creates firewood at the Chopping block
    /// </summary>
    public class ChopFirewoodAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _chopped;
        /// <summary>
        /// The target of this action
        /// </summary>
        private ChoppingBlockComponent _targetChoppingBlock; // where we chop the firewood

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasTool", true); // we need a tool to do this
            AddPrecondition("hasFirewood", false); // if we have firewood we don't want more
            AddPrecondition("hasLogs", true); // we need logs to chop into firewood
            AddEffect("hasFirewood", true);
            ActionName = General_Scripts.Enums.Actions.ChopFirewood;
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _chopped = false;
            _targetChoppingBlock = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _chopped;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a chopping block
        }

        /// <summary>
        /// Checks if there is a <see cref="ChoppingBlockComponent"/> close to the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // find the nearest chopping block that we can chop our wood at
            if (Utils.GetClosest(ResourcesSpawner.Instance.GetObjectsWithKey(Spawns.ChoppingBlock).Select(go => go.GetComponent<ChoppingBlockComponent>()), agent.transform, out _targetChoppingBlock) == false)
                return false;

            Target = _targetChoppingBlock.gameObject;
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is compelted, adds 5 FireWood to the agent's backpack.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            if (StartTime == 0)
            {
                AnimManager.Work();
                StartTime = Time.time;
            }

            // still working
            if (StillWorking())
                return true;

            if (Target == null)
                return false;

            // finished chopping
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumFirewood += 3;
            backpack.NumLogs--;
            _chopped = true;
            var tool = backpack.Tool.GetComponent<ToolComponent>();
            tool.Use(0.34f);
            Target.GetComponent<WorkComponent>().Durability--;

            AnimManager.GoIdle();

            return true;
        }
    }
}

