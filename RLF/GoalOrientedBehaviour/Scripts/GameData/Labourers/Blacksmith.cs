using System.Collections.Generic;
using General_Scripts.GameData;

namespace FinalProject.Teams.RLF.GoalOrientedBehaviour.Scripts.GameData.Labourers
{
    public class Blacksmith : Labourer
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
                new KeyValuePair<string, object>("collectTools", true)
            };

            return goal;
        }
    }
}

