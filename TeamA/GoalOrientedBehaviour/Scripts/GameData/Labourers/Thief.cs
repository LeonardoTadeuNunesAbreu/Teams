using System.Collections.Generic;
using General_Scripts.GameData;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Labourers
{
    public class Thief : Labourer
    {
        /// <summary>
        /// Our only goal will ever be to make tools.
        /// The ForgeTooldAction will be able to fulfill this goal.
        /// </summary>
        /// <returns></returns>
        public override HashSet<KeyValuePair<string, object>> CreateGoalState()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                // TODO you should decide what is the thief goal
                new KeyValuePair<string, object>("TODO", true)
            };

            return goal;
        }
    }
}