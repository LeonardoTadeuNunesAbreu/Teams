using System.Collections;
using General_Scripts.GameData;
using General_Scripts.GoalOrientedBehaviour.AI.GOAP;
using General_Scripts.GUI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class Tackle : GoapAction
    {
        private bool _tackled;
        private Labourer _target;

        public override void Reset()
        {
            StartCoroutine(CoolDown());
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(5);
            _target = null;
            StartTime = 0;
            _tackled = false;
        }

        public override bool IsDone()
        {
            return _tackled;
        }

        public override bool RequiresInRange()
        {
            return true; // you need to be close to tackle
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            //you should look at this method on the other actions and figure out how to get a target. You can tackle any labourer of any team, it's your choice.

            return false;
        }

        public override bool Perform(GameObject agent)
        {
            //once you found a target and got to it, you now have to actually tackle him. we added a function to all labourers called BeenTackled which will imobilize the target for 3 seconds. Check how the other actions are implemented to have an idea on what to do here.
            return true;
        }
    }
}