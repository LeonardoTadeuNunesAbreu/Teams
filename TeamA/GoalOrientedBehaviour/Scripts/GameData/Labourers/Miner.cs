using System.Collections.Generic;
using General_Scripts.GameData;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Labourers
{
    public class Miner : Labourer
    {
        /// <summary>
        /// Our only goal will ever be to mine ore. The MineOreAction will be able to fulfill this goal.
        /// </summary>
        /// <returns></returns>
        public override HashSet<KeyValuePair<string,object>> CreateGoalState ()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("collectOre", true)
            };

            return goal;
        }
    }
}

