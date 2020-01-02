using System.Collections;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using TMPro.EditorUtilities;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class StealFromLabourer : GoapAction
    {
        private bool _stolen;
        private Labourer _target;
        private Labourer _me;


        protected override void Awake()
        {
            base.Awake();

            _me = GetComponent<Labourer>();
        }

        public override void Reset()
        {
            StartCoroutine(CoolDown());
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(5);
            _target = null;
            StartTime = 0;
            _stolen = false;
        }

        public override bool IsDone()
        {
            return _stolen;
        }

        public override bool RequiresInRange()
        {
            return true;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            //you should look at this method on the other actions and figure out how to get a target. You can steal from any labourer of any team that is carrying resources and you cannot carry more then 10 resources at any time.
            return true;
        }

        public override bool Perform(GameObject agent)
        {
            //once you found a target and got to it, you now have to actually tackle him. to do this just transfer the items from the target backpack to yours. Remember of your total carry of 10!

            _me.IsCarring();

            return true;
        }
    }
}