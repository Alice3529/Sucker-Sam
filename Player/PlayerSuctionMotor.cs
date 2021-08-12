using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _Scripts.Collectables;
using _Scripts.PowerUpModifiers;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace _Scripts.Player
{
    public interface IPlayerSuctionMotorParameters
    {
        float PullInSpeed { get; }
        bool TurnedOff { get; }
        float radius { get; }
    }

    public class PlayerSuctionMotor : MonoBehaviour, IPlayerSuctionMotorParameters
    {
        
        //Float used to set the speed of the suction of the tornado
        [SerializeField]
        private float _PullInSpeed = 1f;
        public float PullInSpeed => _PullInSpeed;
        public GameObject WallsGameObject;
    
        //public float pullInSpeed = 10f;
        //Float used to set the speed of the rotation of the tornado
        internal float rotateSpeed = 0f;
        //Radius the suction of the tornado reaches
        [SerializeField]
        private float _radius = 2f;
        public float radius => _radius;
        
        [SerializeField]
        private bool _TurnedOff = false;
        public bool TurnedOff => _TurnedOff;


        private IPlayerSuctionMotorParameters parameters => this.modifier != null ? this.modifier.Apply(this) : this; 

        //Holds the objects within the radius
        public List<GameObject> objectsToPullIn;
 
        //Sets whether the objects is being pulled or not
        public Dictionary<GameObject, bool> objectsPulled;

        private IPropertyModifier<IPlayerSuctionMotorParameters> modifier;

        void Start()
        {
            //Instantiate Dictionary and List for use
            objectsToPullIn = new List<GameObject>();
            objectsPulled = new Dictionary<GameObject, bool>();
        }
 
        void RemoveObjectsFarAway()
        {
            //For each of the gameobjects in objectsToPullIn
            foreach (var thing in objectsToPullIn.ToList())
            {
                //Check if the distance between the objects position and the tornados position is greater than the suctions radius
                if (Vector3.Distance(thing.transform.position, transform.position) > parameters.radius)
                {
                    //And if that is true then remove the object from being sucked in
                    objectsToPullIn.Remove(thing);
                }
            }
        }
 
        void GetObjectsToPullIn()
        {
            var objects = Physics2D.OverlapCircleAll(GetComponent<Collider2D>().bounds.center,parameters.radius);//GetComponent<Collider>().bounds.extents.magnitude);
            
            //For each object
            foreach (var t in objects)
            {
                //Ignore if between object & me is some collider.
                if (WallBetweenMeAndObject(t)) continue;
                
                //If that object is not already contained in the objectsToPullIn list
                //the object does not equal the tornado, and if it contains a 
                //rigidbody component
                if (objectsToPullIn.Contains(t.gameObject) 
                    || t.gameObject == gameObject 
                    || t.GetComponent<ICollectable>() == null
                    || t.GetComponent<Rigidbody2D>() == null) continue;
                
                //Then add it to the objects to pull in list
                objectsToPullIn.Add(t.gameObject);
                 
                //And make sure to set that the object has not been pulled all the way in yet
                if (!objectsPulled.ContainsKey(t.gameObject))
                    objectsPulled.Add(t.gameObject, false);
                else
                    objectsPulled[t.gameObject] = false;
            }
        }

        private bool WallBetweenMeAndObject(Collider2D collider2D1)
        {
            if (!WallsGameObject) return false;
            
            var directionFromMe = (transform.position - collider2D1.transform.position).normalized;
            var distanceFromMe = (transform.position - collider2D1.transform.position).magnitude;
            //Handles.DrawLine(transform.position, directionFromMe * distanceFromMe);

            var rayCastHit = Physics2D.RaycastAll(transform.position, directionFromMe, distanceFromMe);
            var isThereWall = rayCastHit.Any(r => r.transform.name == WallsGameObject.transform.name);
            return isThereWall;
        }

        void PullObjectsIn()
        {
            //For each gameobject in objectsToPullIn
            foreach (GameObject thing in objectsToPullIn)
            {
                //If the object has been pulled in
                if (objectsPulled[thing] != true)
                {
                    var forwardSpeed = Mathf.Clamp(1 / thing.GetComponent<Rigidbody2D>().mass, 0.01f, 10f); 
                    
                    //Then move it towards the tornado
                    thing.transform.position = Vector3.MoveTowards(thing.transform.position, transform.position, forwardSpeed * (Time.deltaTime * parameters.PullInSpeed));
                }
            }
        }
 
        void RotateObjects()
        {
            //For each of the gameobjects that have been classified as being pulled in or not
            foreach (GameObject thing in objectsPulled.Keys)
            {
                //If they are pulled in
                if (objectsPulled[thing] == true)
                {
                    //Then rotate it around the tornado
                    var forwardSpeed = Mathf.Clamp(1 / thing.GetComponent<Rigidbody2D>().mass, 0.01f, 10f); 
                    thing.transform.RotateAround(Vector3.zero, transform.up, forwardSpeed * rotateSpeed * Time.deltaTime);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //If the object is contained in the object to pull in list
            if (objectsToPullIn.Contains(other.gameObject))
            {
                //Then set the object as being pulled in to the tornado
                objectsPulled[other.gameObject] = true;
                //Rotate that shit
                RotateObjects();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            //If the object is contained in the object to pull in list
            if (objectsToPullIn.Contains(other.gameObject))
            {
                //Then set the object as being pulled in to the tornado
                objectsPulled[other.gameObject] = true;
                //Rotate that shit
                RotateObjects();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //If the object is contained in the object to pull in list
            if (objectsToPullIn.Contains(other.gameObject))
            {
                //Then set the object as not being pulled in to the tornado
                objectsPulled[other.gameObject] = false;
            }
        }


        void Update()
        {
            //Each update:
 
            //Run these methods
            RemoveDestroyedObjects();
            RemoveObjectsFarAway();
            ExpireModifierIfNeeded();
               
            if (parameters.TurnedOff) return;
            GetObjectsToPullIn();
            PullObjectsIn();
            RotateObjects();
        }

        private void ExpireModifierIfNeeded()
        {
            if (modifier == null) return;
            if (modifier.IsExpired)
            {
                modifier = null;
            }
        }

        private void RemoveDestroyedObjects()
        {
            objectsToPullIn = objectsToPullIn.Where(obj => obj).ToList();

            var destroyedObjecs = objectsPulled.Where(obj => !obj.Key).ToList();
            foreach (var destroyedObject in destroyedObjecs)
                objectsPulled.Remove(destroyedObject.Key);
        }

        public void ApplyModifier(IPropertyModifier<IPlayerSuctionMotorParameters> modifier)
        {
            this.modifier = modifier;
            this.modifier.Initialize(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Handles.color = Color.green;
            var leftBottom = transform.position;
            leftBottom.x *= (float) Math.Sin(transform.rotation.z);
            leftBottom.y *= (float) Math.Cos(transform.rotation.z);
            Gizmos.DrawLine(transform.position,  transform.position* parameters.radius);
        }

#endif
    }

}
