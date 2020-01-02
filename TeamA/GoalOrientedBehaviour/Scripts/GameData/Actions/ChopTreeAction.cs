using System.Linq;
using General_Scripts.Enums;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent"/> one log.
    /// </summary>
    public class ChopTreeAction : GoapAction
    {
        /// <summary>
        /// The obect used for the effect
        /// </summary>
        private bool _chopped;
        /// <summary>
        /// The target of this action
        /// </summary>
        private TreeComponent _targetTree; // where we get the logs from

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasTool", true); // we need a tool to do this
            AddPrecondition("hasLogs", false); // if we have logs we don't want more
            AddEffect("hasLogs", true);
            ActionName = General_Scripts.Enums.Actions.ChopTree;
        }
        
        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _chopped = false;
            _targetTree = null;
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
            return true; // yes we need to be near a tree
        }

        /// <summary>
        /// Checks if there is a <see cref="TreeComponent"/> close to the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // find the nearest tree that we can chop
            if (Utils.GetClosest(ResourcesSpawner.Instance.GetObjectsWithKey(Spawns.Tree).Select(go => go.GetComponent<TreeComponent>()).Where(tree => tree.CanBeWorked), agent.transform, out _targetTree) == false)
                return false;
            
            Target = _targetTree.gameObject;
            agent.GetComponent<Labourer>().UpdateWorkStation(Target.GetComponent<WorkComponent>());
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is completed add one log to the <see cref="GoapAgent"/>'s <see cref="BackpackComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            // The tree got destroyed before we finished
            if (_targetTree == null)
                return false;

            if (StartTime == 0)
            {
                //start working

                AnimManager.Work();
                StartTime = Time.time;

                if (_targetTree.IsOverloaded)
                {
                    _targetTree.StoptWorking(agent.GetComponent<Labourer>());
                    return false; // we need to plan again to find a new tree
                }
            }

            if (StillWorking())
            {
                // still working
                return true;
            }
            
            // finished working
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumLogs++;
            _chopped = true; // update state -> effect
            var tool = backpack.Tool.GetComponent<ToolComponent>();
            tool.Use(0.34f);
            _targetTree.Durability--;
            
            AnimManager.GoIdle();
            return true;
        }

    }
}