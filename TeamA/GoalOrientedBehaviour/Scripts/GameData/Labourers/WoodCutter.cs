using System.Collections.Generic;
using General_Scripts.GameData;
using UnityEngine;

namespace FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Labourers
{
    [RequireComponent(typeof(BackpackComponent))]
    public class WoodCutter : Labourer
    {
        /// <summary>
        /// Our only goal will ever be to chop logs. The ChopFirewoodAction will be able to fulfill this goal.
        /// </summary>
        /// <returns></returns>
        public override HashSet<KeyValuePair<string,object>> CreateGoalState ()
        {
            var goal = new HashSet<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("collectFirewood", true)
            };

            return goal;
        }
    }
}

