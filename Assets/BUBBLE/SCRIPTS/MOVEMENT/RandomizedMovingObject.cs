using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
    /// <summary>
    /// Add this class to an object and it'll move according to the level's speed,
    /// with its speed randomized between a specified range.
    /// </summary>
    public class RandomizedMovingObject : MonoBehaviour
    {
        [Header("Movement")]
        /// The minimum speed of the object
        [Tooltip("The minimum speed of the object")]
        public float MinSpeed = 0;

        /// The maximum speed of the object
        [Tooltip("The maximum speed of the object")]
        public float MaxSpeed = 10;

        /// The acceleration of the object over time. Starts accelerating on enable.
        [Tooltip("The acceleration of the object over time. Starts accelerating on enable.")]
        public float Acceleration = 0;

        /// The current direction of the object
        [Tooltip("The current direction of the object")]
        public Vector3 Direction = Vector3.left;

        [Header("Behaviour")]
        /// If set to true, the spawner can change the direction of the object. If not the one set in its inspector will be used.
        [Tooltip("If set to true, the spawner can change the direction of the object. If not the one set in its inspector will be used.")]
        public bool DirectionCanBeChangedBySpawner = true;

        /// The space this object moves into, either world or local
        [Tooltip("The space this object moves into, either world or local")]
        public Space MovementSpace = Space.World;

        public Vector3 Movement
        {
            get { return _movement; }
        }

        protected Vector3 _movement;
        protected float _initialSpeed;
        private float _speed; // Add a private field for Speed

        /// <summary>
        /// On awake, we set the initial speed to a randomized value within the specified range.
        /// </summary>
        protected virtual void Awake()
        {
            RandomizeSpeed();
        }

        /// <summary>
        /// On enable, we reset the object's speed to a new randomized value within the range.
        /// </summary>
        protected virtual void OnEnable()
        {
            RandomizeSpeed();
        }

        /// <summary>
        /// Randomizes the speed within the specified range.
        /// </summary>
        protected virtual void RandomizeSpeed()
        {
            _speed = Random.Range(MinSpeed, MaxSpeed);
            _initialSpeed = _speed; // Store it as the initial speed
        }

        /// <summary>
        /// On update(), we move the object based on the level's speed and the object's speed, and apply acceleration.
        /// </summary>
        protected virtual void Update()
        {
            Move();
        }

        public virtual void Move()
        {
            if (LevelManager.Instance == null)
            {
                _movement = Direction * (_speed / 10) * Time.deltaTime;
            }
            else
            {
                _movement = Direction * (_speed / 10) * LevelManager.Instance.Speed * Time.deltaTime;
            }

            transform.Translate(_movement, MovementSpace);

            // Apply the acceleration to increase the speed
            _speed += Acceleration * Time.deltaTime;
        }

        public virtual void SetDirection(Vector3 newDirection)
        {
            if (DirectionCanBeChangedBySpawner)
            {
                Direction = newDirection;
            }
        }
    }
}
