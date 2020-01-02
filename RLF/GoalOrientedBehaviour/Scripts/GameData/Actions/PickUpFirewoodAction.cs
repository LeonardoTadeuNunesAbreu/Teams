using System.Linq;
using General_Scripts.Enums;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using UnityEngine;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Gives the <see cref="GoapAgent"/> 5 firewood.
    /// </summary>
    public class PickUpFirewoodAction : GoapAction
    {
        /// <summary>
        /// The object used for the effect
        /// </summary>
        private bool _hasWood;
        /// <summary>
        /// The target of this action
        /// </summary>
        private TreeComponent _targetTree;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasFirewood", false); // if we have firewood we don't want more
            AddEffect("hasFirewood", true);
            ActionName = General_Scripts.Enums.Actions.PickUpFirewood;
        }

        /// <summary>
        /// Resets the action to its default values, so it can be used again.
        /// </summary>
        public override void Reset()
        {
            _hasWood = false;
            _targetTree = null;
            StartTime = 0;
        }

        /// <summary>
        /// Check if the action has been completed
        /// </summary>
        /// <returns></returns>
        public override bool IsDone()
        {
            return _hasWood;
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
        /// Checks if there is a <see cref="TreeComponent"/> close to the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // find the nearest tree where we can pick up firewood
            if (Utils.GetClosest(ResourcesSpawner.Instance.GetObjectsWithKey(Spawns.Tree).Select(
                    go => go.GetComponent<TreeComponent>())
                    .Where(tree => tree.CanBeWorked), agent.transform, out _targetTree) == false)
                return false;

            Target = _targetTree.gameObject;
            agent.GetComponent<Labourer>().UpdateWorkStation(Target.GetComponent<WorkComponent>());
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is completed, adds 5 firewood to the <see cref="GoapAgent"/>'s <see cref="BackpackComponent"/>.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            // still working
            if (_targetTree == null)
                return false; // action failed

            if (StartTime == 0)
            {
                StartTime = Time.time;

                if (_targetTree.IsOverloaded)
                {
                    _targetTree.StoptWorking(agent.GetComponent<Labourer>());
                    return false;
                }
            }

            if (StillWorking())
                return true;

            // finished chopping
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.NumFirewood += 5;
            _hasWood = true;
            _targetTree.Durability--;
            return true;
        }
    }
}
