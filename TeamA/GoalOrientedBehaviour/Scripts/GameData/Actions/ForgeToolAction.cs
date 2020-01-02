using System.Linq;
using General_Scripts.Enums;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent"/> one tool. 
    /// </summary>
    public class ForgeToolAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _forged;
        /// <summary>
        /// The target of this action
        /// </summary>
        private ForgeComponent _targetForge; // where we forge tools

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasOre", true);
            AddPrecondition("hasFirewood", true);
            AddEffect("hasNewTools", true);
            ActionName = General_Scripts.Enums.Actions.ForgeTool;
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _forged = false;
            _targetForge = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _forged;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a forge
        }

        /// <summary>
        /// Checks if there is a <see cref="ForgeComponent"/> close to the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // find the nearest forge
            if (Utils.GetClosest(ResourcesSpawner.Instance.GetObjectsWithKey(Spawns.Forge).Select(go => go.GetComponent<ForgeComponent>()), agent.transform, out _targetForge) == false)
                return false;

            Target = _targetForge.gameObject;
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is completed, adds 1 tool to the <see cref="GoapAgent"/>'s <see cref="BackpackComponent"/>.
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
            if (Target == null)
                return false;

            // still working
            if (StillWorking())
                return true;

            // finished forging a tool
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumOre = 0;
            backpack.NumFirewood = 0;
            backpack.NumOfForgedTools += 2;
            _forged = true;
            Target.GetComponent<WorkComponent>().Durability--;

            AnimManager.GoIdle();
            return true;
        }
    }
}
