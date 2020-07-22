
using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
/*
using Pathfinding;

namespace VoiceActing
{
    public class EnemyNavmeshAI : EnemyNavmeshController
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== 
        [SerializeField]
        CharacterMovement characterMovement;

        private Seeker seeker;
        private Transform targetPosition;
        private Path path;
        Vector3 destinationVelocity;



        private int currentWaypoint = 0;

        public float repathRate = 0.5f;
        private float lastRepath = float.NegativeInfinity;

        private bool reachedEndOfPath;
        public bool ReachedEndOfPath
        {
            get { return reachedEndOfPath; }
        }

        public float nextWaypointDistance = 3;

        private bool canMove = false;
        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== 

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== 


        public void MoveToTarget(Transform target)
        {
            canMove = true;
            targetPosition = target;
            //seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        }

        public void StopMove()
        {
            canMove = false;
            characterMovement.EndMove();
            //navMeshAgent.Move(Vector3.zero);
            //navMeshAgent.velocity = Vector3.zero;
        }


        public void Start()
        {
            seeker = GetComponent<Seeker>();
        }

        public void OnPathComplete(Path p)
        {
            Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

            // Path pooling. To avoid unnecessary allocations paths are reference counted.
            // Calling Claim will increase the reference count by 1 and Release will reduce
            // it by one, when it reaches zero the path will be pooled and then it may be used
            // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
            // take a path from the pool if possible. See also the documentation page about path pooling.
            p.Claim(this);
            if (!p.error)
            {
                if (path != null) path.Release(this);
                path = p;
                // Reset the waypoint counter so that we start to move towards the first point in the path
                currentWaypoint = 0;
            }
            else
            {
                p.Release(this);
            }
        }

        public void Update()
        {
            if (canMove == false)
                return;

            if (targetPosition == null)
                return;

            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;

                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            }

            if (path == null)
            {
                // We have no path to follow yet, so don't do anything
                return;
            }

            // Check in a loop if we are close enough to the current waypoint to switch to the next one.
            // We do this in a loop because many waypoints might be close to each other and we may reach
            // several of them in the same frame.
            reachedEndOfPath = false;
            // The distance to the next waypoint in the path
            float distanceToWaypoint;
            while (true)
            {
                // If you want maximum performance you can check the squared distance instead to get rid of a
                // square root calculation. But that is outside the scope of this tutorial.
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                // Next way point distance c'est la distance de validation du waypoint (je dois être à nextWaypointDistance pour que je puisse passer au suivant
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            // Slow down smoothly upon approaching the end of the path
            // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.

            // Osef
            //var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            Vector3 velocity = dir;// * characterMovement.Speed;

            Debug.Log(velocity);
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            //characterMovement.MoveCharacterManual(velocity.x, 0, velocity.z, true); //.Move(velocity);
            characterMovement.MoveCharacterWorld(velocity.x, velocity.z, characterMovement.Speed);

        }







        #endregion

    }
}*/