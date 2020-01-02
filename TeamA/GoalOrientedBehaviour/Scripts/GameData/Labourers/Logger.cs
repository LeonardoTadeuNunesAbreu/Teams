using System.Collections.Generic;
using General_Scripts.GameData;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Labourers
{
    public class Logger : Labourer
    {
        /// <summary>
        /// Our only goal will ever be to chop trees. The ChopTreeAction will be able to fulfill this goal.
        /// </summary>
        /// <returns></returns>
        public override HashSet<KeyValuePair<string,object>> CreateGoalState ()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("collectLogs", true)
            };

            return goal;
        }
    }
}

