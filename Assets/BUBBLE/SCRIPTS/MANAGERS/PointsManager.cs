using MoreMountains.Feedbacks; // Make sure to import the MMFeedbacks namespace
using MoreMountains.InfiniteRunnerEngine; // Ensure you have the correct namespace
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    [Tooltip("The Text UI element to display red points.")]
    [SerializeField] private Text redPointsText;

    [Tooltip("The Text UI element to display blue points.")]
    [SerializeField] private Text bluePointsText;

    // Serialized MMFeedbacks for both players
    [Tooltip("The feedback to play when the red player wins.")]
    [SerializeField] private MMFeedbacks redWinFeedback;

    [Tooltip("The feedback to play when the blue player wins.")]
    [SerializeField] private MMFeedbacks blueWinFeedback;

    // Serialized number of points required to win
    [Tooltip("The number of points required to win.")]
    [SerializeField] private int winPointsThreshold = 15; // Default set to 15

    private int redPoints = 0;
    private int bluePoints = 0;

    /// <summary>
    /// Adds a point to the specified type (Red or Blue).
    /// </summary>
    /// <param name="pointType">The type of point to add (Red or Blue).</param>
    public void AddPoint(CustomPickablePointsWithFeedback.PointType pointType)
    {
        if (pointType == CustomPickablePointsWithFeedback.PointType.Red)
        {
            redPoints++;
            UpdateRedPointsUI();

            // Check if red player reached the winning points threshold
            if (redPoints >= winPointsThreshold)
            {
                Debug.Log("Red player wins with " + winPointsThreshold + " points!");
                redWinFeedback?.PlayFeedbacks(); // Play feedback for red player win
            }
        }
        else if (pointType == CustomPickablePointsWithFeedback.PointType.Blue)
        {
            bluePoints++;
            UpdateBluePointsUI();

            // Check if blue player reached the winning points threshold
            if (bluePoints >= winPointsThreshold)
            {
                Debug.Log("Blue player wins with " + winPointsThreshold + " points!");
                blueWinFeedback?.PlayFeedbacks(); // Play feedback for blue player win
            }
        }
    }

    private void UpdateRedPointsUI()
    {
        if (redPointsText != null)
        {
            redPointsText.text = $"{redPoints}"; // Only show the number of red points
        }
        else
        {
            Debug.LogWarning("Red Points Text is not assigned!");
        }
    }

    private void UpdateBluePointsUI()
    {
        if (bluePointsText != null)
        {
            bluePointsText.text = $"{bluePoints}"; // Only show the number of blue points
        }
        else
        {
            Debug.LogWarning("Blue Points Text is not assigned!");
        }
    }

    public int GetRedPoints() => redPoints;
    public int GetBluePoints() => bluePoints;
}
