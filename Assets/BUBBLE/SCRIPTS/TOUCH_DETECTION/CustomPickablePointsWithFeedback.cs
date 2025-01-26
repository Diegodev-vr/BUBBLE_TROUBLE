using UnityEngine;
using MoreMountains.Feedbacks; // For feedback components
using MoreMountains.Tools; // For MMPoolableObject
using System.Collections; // Required for IEnumerator and coroutines

namespace MoreMountains.InfiniteRunnerEngine
{
    public class CustomPickablePointsWithFeedback : MMPoolableObject
    {
        public enum PointType { Red, Blue } // Enum to differentiate point types

        [Tooltip("Specify the type of point this object represents (Red or Blue).")]
        [SerializeField] private PointType pointType;

        [Tooltip("A serialized feedback object to activate when the point is picked up.")]
        [SerializeField] private MonoBehaviour feedbackComponent;

        [Tooltip("Delay before destroying the object, in seconds.")]
        [SerializeField] private float destroyDelay = 0.5f;

        private Camera mainCamera;
        private BoxCollider2D _collider;

        private void Awake()
        {
            mainCamera = Camera.main; // Cache the main camera
            _collider = GetComponent<BoxCollider2D>(); // Cache the BoxCollider2D
        }

        protected override void Update() // Override the inherited Update() method
        {
            base.Update(); // Call the base class Update() method if needed
            DetectTouchOrClick();
        }

        /// <summary>
        /// Ensures the collider is re-enabled when the object is reused.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable(); // Call the base class OnEnable() method

            if (_collider != null)
            {
                _collider.enabled = true; // Re-enable the collider
            }
        }

        private void DetectTouchOrClick()
        {
            // Handle all touch inputs (mobile)
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    HandleTouch(mainCamera.ScreenToWorldPoint(touch.position));
                }
            }

            // Handle mouse clicks (desktop testing)
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                HandleTouch(mousePosition);
            }
        }

        private void HandleTouch(Vector3 position)
        {
            Vector2 position2D = new Vector2(position.x, position.y);

            RaycastHit2D hit = Physics2D.Raycast(position2D, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Disable the BoxCollider2D to prevent further interactions
                if (_collider != null)
                {
                    _collider.enabled = false;
                }

                // Add point to the respective type via PointsManager
                var pointsManager = Object.FindFirstObjectByType<PointsManager>();
                if (pointsManager != null)
                {
                    pointsManager.AddPoint(pointType);
                }

                // Trigger the feedback component
                ActivateFeedback();

                // Handle any custom pickup behavior
                OnPickedUp();

                // Recycle or deactivate the object after delay
                StartCoroutine(DestroyAfterDelay());
            }
        }

        protected virtual void OnPickedUp() { }

        private void ActivateFeedback()
        {
            if (feedbackComponent != null)
            {
                if (feedbackComponent is MMF_Player mmfPlayer)
                {
                    mmfPlayer.PlayFeedbacks(); // Trigger feedbacks in MMF_Player
                }
                else
                {
                    Debug.LogWarning("The feedback component is not an MMF_Player component.");
                }
            }
            else
            {
                Debug.LogWarning("No feedback component assigned.");
            }
        }

        private IEnumerator DestroyAfterDelay()
        {
            yield return new WaitForSeconds(destroyDelay); // Wait for the specified delay
            Destroy(); // Call inherited Destroy() method from MMPoolableObject
        }
    }
}
