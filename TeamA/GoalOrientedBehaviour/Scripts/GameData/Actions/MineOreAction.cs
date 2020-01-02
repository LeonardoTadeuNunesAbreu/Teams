using System.Linq;
using General_Scripts.Enums;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent"/> 2 ores. 
    /// </summary>
    public class MineOreAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _mined;
        /// <summary>
        /// The target of this action
        /// </summary>
        private IronRockComponent _targetRock; // where we get the ore from

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasTool", true); // we need a tool to do this
            AddPrecondition("hasOre", false); // if we have ore we don't want more
            AddEffect("hasOre", true);
            ActionName = General_Scripts.Enums.Actions.MineOre;
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _mined = false;
            _targetRock = null;
            StartTime = 0;
        }

        
        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _mined;
        }

        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near a rock
        }

        /// <summary>
        /// Checks if there is a <see cref="IronRockComponent"/> close to the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // find the nearest rock that we can mine
            if (Utils.GetClosest(ResourcesSpawner.Instance.GetObjectsWithKey(Spawns.Rock).Select(go => go.GetComponent<IronRockComponent>()), agent.transform, out _targetRock) == false)
                return false;

            Target = _targetRock.gameObject;
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is completed, add 2 ores to the <see cref="GoapAgent"/>'s <see cref="BackpackComponent"/>.
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

            // finished mining
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumOre += 2;
            _mined = true;
            var tool = backpack.Tool.GetComponent<ToolComponent>();
            tool.Use(0.5f);
            _targetRock.Durability--;

            AnimManager.GoIdle();
            //if (tool.Destroyed())
            //{
            //    Destroy(backpack.Tool);
            //    backpack.Tool = null;
            //}
            return true;
        }

    }
}


