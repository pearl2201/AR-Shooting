using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using Fruit;
using RavingBots.CartoonExplosion;
public class WorldLandGenerate : MonoBehaviour
{

    public FruitGameManager fruitgameManager;
    public float createHeight;


    // Use this for initialization
    void Start()
    {


    }

    void CreateBall(Vector3 atPosition)
    {
        fruitgameManager.StartGame(atPosition);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                ARPoint point = new ARPoint
                {
                    x = screenPosition.x,
                    y = screenPosition.y
                };

                List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point,
                    ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
                if (hitResults.Count > 0)
                {
                    foreach (var hitResult in hitResults)
                    {
                        Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                        CreateBall(new Vector3(position.x, position.y + createHeight, position.z));
                        break;
                    }
                }

            }
        }

    }

}

