using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Fruit
{
    public class FruitGameOverPopup : MonoBehaviour
    {

        public void Restart()
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
