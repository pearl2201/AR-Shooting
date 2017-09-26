using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Fruit
{
    public class FruitGunControl : MonoBehaviour
    {
        public int gunDamage = 1;
        // Set the number of hitpoints that this gun will take away from shot objects with a health script
        public float fireRate = 0.25f;
        // Number in seconds which controls how often the player can fire
        public float weaponRange = 50f;
        // Distance in Unity units over which the player can fire
        public float hitForce = 100f;
        // Amount of force which will be added to objects with a rigidbody shot by the player
        public Transform gunEnd;
        [SerializeField]
        private Camera fpsCam;
        private float nextFire;

        public GameObject Shot1;
        public GameObject Wave;
        //public PlaceUIElementAtWorldPosition shootPoint;

        public RaycastHit rayhit;

        public GameManager gamemanager;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Shooting()
        {

            if (Time.time > nextFire)
            {
                Debug.Log("Click shooting");
                SoundManager.Instance.Play("shoot");
                // Create a vector at the center of our camera's viewport
                Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

                // Declare a raycast hit to store information about what our raycast has hit
                RaycastHit hit;

                // Set the start position for our visual effect for our laser to the position of gunEnd
                //laserLine.SetPosition (0, gunEnd.position);
                Vector3 pointShooting = new Vector3(0, 0, 0);
                bool hitGo = false;
                // Check if our raycast has hit anything
                if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit))
                {
                    // Set the end position for our laser line 
                    //laserLine.SetPosition (1, hit.point);
                    Debug.Log("hit");
                    // Get a reference to a health script attached to the collider we hit
                    hitGo = true;

                    pointShooting = hit.point;
                    Vector3 dir = (pointShooting - rayOrigin).normalized;


                    var rotation = Quaternion.LookRotation(dir);
                    Debug.Log("hit length: " + hit.distance);

                    /*
                    DragonControl health = hit.collider.GetComponent<DragonControl> ();

                    // If there was a health script attached
                    if (health != null) {
                        // Call the damage function of that script, passing in our gunDamage variable
                        health.OnHit (gunDamage);
                    }
                    */

                    /*
                    // Check if the object we hit has a rigidbody attached
                    if (hit.rigidbody != null) {
                        // Add force to the rigidbody we hit, in the direction from which it was hit
                        hit.rigidbody.AddForce (-hit.normal * hitForce);
                    }
                    */
                }
                else
                {
                    // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                    //laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
                    pointShooting = rayOrigin + (fpsCam.transform.forward * weaponRange);
                    Debug.Log("No hit");
                }

                {
                    Vector3 dir = (pointShooting - rayOrigin).normalized;


                    var rotation = Quaternion.LookRotation(dir);
                    GameObject Bullet = Shot1;
                    //Fire
                    GameObject s1 = (GameObject)Instantiate(Bullet, gunEnd.transform.position, rotation);
                    s1.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());

                    GameObject wav = (GameObject)Instantiate(Wave, gunEnd.transform.position, rotation);
                    wav.transform.localScale *= 0.25f;
                    wav.transform.Rotate(Vector3.left, 90.0f);
                    wav.GetComponent<BeamWave>().col = this.GetComponent<BeamParam>().BeamColor;
                    if (hitGo)
                    {
                        EffectHit(hit.collider.gameObject, hit.point, rotation, Bullet);
                    }
                    else
                    {
                        gamemanager.AddCountHitMiss();
                    }

                }


                gamemanager.ShakeCam();
            }
        }
        public GameObject HitEffect;
        public void EffectHit(GameObject objHit, Vector3 pos, Quaternion rotation, GameObject bullet)
        {
            var Angle = Quaternion.AngleAxis(180.0f, transform.up) * rotation;
            GameObject obj = (GameObject)Instantiate(HitEffect, pos, bullet.transform.rotation);
            //	obj.GetComponent<BeamParam>().SetBeamParam(this.GetComponent<BeamParam>());
            obj.transform.localScale = bullet.transform.localScale;

            DragonControl health = objHit.GetComponent<DragonControl>();

            // If there was a health script attached
            if (health != null)
            {
                // Call the damage function of that script, passing in our gunDamage variable
                health.OnHit(1);
            }
        }
    }
}
