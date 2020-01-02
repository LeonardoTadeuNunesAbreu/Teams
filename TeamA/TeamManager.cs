using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Labourers;
using General_Scripts.GameData;
using UnityEngine;
using Logger = FinalProject.Teams.TeamA.GoalOrientedBehaviour.Scripts.GameData.Labourers.Logger;

namespace FinalProject.Teams.TeamA
{
    /// <summary>
    /// the strategy of this team is to build 1 worker then 1 upgrade, then 1 work, then 1 upgrade, repeat...
    /// </summary>
    public class TeamManager : MonoBehaviour, ITeamManager
    {
        public TeamsEnum MyTeam { get; set; }
        public bool IsBuilding { get; set; }
        public bool IsUpgrading { get; set; }
        public Labourer WhatsBeingBuild { get; set; }

        [SerializeField]
        private TeamsEnum _myTeam;
        public List<Labourer> Labourers;
        public SupplyPileComponent MySupplies;
        public Transform LabourersParent;
        public List<Labourer> SpawnedLaborers;

        public void Start()
        {
            StartCoroutine(SpawnWoodCutters());
        }

        private IEnumerator SpawnWoodCutters()
        {
            var woodCutterPrefab = Labourers.First(lab => lab.GetType() == typeof(WoodCutter));
            while (true)
            {
                while (MySupplies.NumFirewood < woodCutterPrefab.Costs[(int) Resources.Firewood])
                    yield return null;

                if (MySupplies.NumFirewood >= woodCutterPrefab.Costs[(int)Resources.Firewood])
                    yield return StartCoroutine(SpawnLaborer(woodCutterPrefab)); // if you yield a Coroutine by calling another, it will return here when the second call finishes. In this example, will return when lab.BuildTime has passed
                
                var labourer = SpawnedLaborers.First(lab => lab.GetType() == typeof(WoodCutter));
                var calculateUpgradeCost = labourer.Costs[(int)Resources.Firewood] * MySupplies.WoodCutterUpgrade;
                print("cost" + calculateUpgradeCost);

                while (MySupplies.NumFirewood < calculateUpgradeCost)
                {
                    yield return null; // wait until you can do one upgrade
                }

                if (MySupplies.NumFirewood >= calculateUpgradeCost)
                    yield return StartCoroutine(BuildLaborerUpgrade(labourer));

                yield return null; // wait one frame before checking again if there is enough resources.
            }
        }// intended


        private IEnumerator SpawnLaborer(Labourer lab)
        {
            IsBuilding = true;
            WhatsBeingBuild = lab;
            // wait the build time of the unit
            yield return new WaitForSeconds(lab.BuildTime);

            // spawn the new unit and puts it near your team supply depot. Max distant you are allowed to spawn is a 5 units radius from the supply position.
            var newLab = Instantiate(lab, LabourersParent, true);
            newLab.MyTeam = MyTeam;
            newLab.transform.position = transform.position + Vector3.forward * 3;
            SpawnedLaborers.Add(newLab);

            // pay the cost of the unit
            MySupplies.RemoveFireWood(lab.Costs[(int)Resources.Firewood], false);
            MySupplies.RemoveLogs(lab.Costs[(int)Resources.Log], false);
            MySupplies.RemoveOres(lab.Costs[(int)Resources.Ore], false);
            MySupplies.RemoveTools(lab.Costs[(int)Resources.Tools], false);

            IsBuilding = false;
            WhatsBeingBuild = null;
        }



        public IEnumerator BuildLaborerUpgrade(Labourer lab)
        {
            //print("Upgrading");
            IsUpgrading = true;
            WhatsBeingBuild = lab;

            yield return new WaitForSeconds(lab.BuildTime * 1.5f);
            

            MySupplies.RemoveFireWood(lab.Costs[(int)Resources.Firewood], true);
            MySupplies.RemoveLogs(lab.Costs[(int)Resources.Log], true);
            MySupplies.RemoveOres(lab.Costs[(int)Resources.Ore], true);
            MySupplies.RemoveTools(lab.Costs[(int)Resources.Tools], true);
            
            if(lab.GetType() == typeof(WoodCutter))
                MySupplies.WoodCutterUpgrade *= 1.1f;
            else if (lab.GetType() == typeof(Blacksmith))
                MySupplies.BlackSmithUpgrade *= 1.1f;
            else if (lab.GetType() == typeof(Miner))
                MySupplies.MinerUpgrade *= 1.1f;
            else if (lab.GetType() == typeof(Logger))
                MySupplies.LoggerUpgrade *= 1.1f;


            IsUpgrading = false;
            WhatsBeingBuild = null;

        }


        #region Cant touch this!



        /// <summary>
        /// You are not allowed to change this function. Doing it so will result on failing the course!
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return MySupplies.NumFirewood + MySupplies.NumLogs * 2 + MySupplies.NumOre * 4 + MySupplies.NumTools * 10;
        }
        #endregion

    }
}
