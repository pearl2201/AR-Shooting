using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Fruit
{
    public class WorldLand : MonoBehaviour
    {
        public List<Island> islands;

        public Vector3[] originalScaleIsland;
        private void Start()
        {
            originalScaleIsland = new Vector3[islands.Count];
            for (int i = 0; i < islands.Count; i++)
            {
                originalScaleIsland[i] = islands[i].transform.localScale;
            }
        }

        public FruitGameManager gameManager;

        public void SetNextRound()
        {
            islands[gameManager.indexRound].gameObject.SetActive(true);
            islands[gameManager.indexRound].transform.localScale = Vector3.zero;
        }

        public void UpdateNextRoundScale(float scl)
        {
            islands[gameManager.indexRound].transform.localScale = originalScaleIsland[gameManager.indexRound] * scl;
        }
    }
}
