using UnityEngine;
using System; 

public class AIStateMachine : MonoBehaviour
{ 
    // C# uses 'enum' (enumeration) to define a set of named integer constants.
    public enum AIState
    {
        Idle,
        Active,
        TaskComplete,
        EndSession
    }
     [Header("State Configuration")]
    // The current state of the AI, visible in the Unity Inspector.
    public AIState currentState = AIState.Idle;

    [Header("Metrics & Variables")]
    // C# requires explicit type declaration: 'public int' instead of just 'daily_tasks = 0'.
    public int dailyTasksCompleted = 0;

     // 'float' is used for decimal numbers, crucial for time tracking in Unity.
    public float idleTime = 0f;

     // A flag to simulate a 'trigger' from Usman's system.
    public bool externalTaskTrigger = false;

    // --- Thresholds for State Changes ---
    private const float TIME_TO_ACTIVE_THRESHOLD = 5.0f; // 5 seconds of idle time triggers Active.
    private const int TASK_COMPLETION_TARGET = 3;       // 3 tasks must be completed to trigger EndSession.

      // --- Event Delegates (Mocking coordination with other systems) ---
    // C# delegates are similar to function pointers/callbacks in Python.
    // 'event Action' is the standard way to declare a broadcast event.
    public static event Action OnStateChange;
    public static event Action<int> OnTaskCompleted;

    // Start is called once before the first frame update (like a '__init__' or setup function)
    void Start()
    {
        // Set the initial state and log the beginning of the session.
        TransitionToState(AIState.Idle);
    }

    // Update is called once per frame (this is Unity's main game loop, similar to a 'while True' loop)
    void Update()
    {
        // 1. Handle external manual triggers (for testing/mocking)
        // Check for 'S' key press to simulate an external system setting the task trigger
        if (Input.GetKeyDown(KeyCode.S))
        {
            externalTaskTrigger = true;
            Debug.Log("<color=orange>External System Triggered Task via 'S' key!</color>");
        }

        // Check for 'T' key press to simulate an external system completing a task.
        if (Input.GetKeyDown(KeyCode.T) && currentState == AIState.Active)
        {
            SimulateTaskCompletion();
        }

        // 2. Run the logic for the current state.
        HandleStateLogic();
    }

    /// <summary>
    /// Executes the specific actions and transition checks for the current AI state.
    /// </summary>
    private void HandleStateLogic()
    {
        // The 'switch' statement is C#'s clean way to handle multiple conditional branches
        // based on a single variable (like a multi-level 'if/elif/else' chain in Python).
        switch (currentState)
        {
            case AIState.Idle:
                // Check for time or an external trigger to go Active.
                idleTime += Time.deltaTime; // Time.deltaTime is the time elapsed since the last frame.
                
                if (externalTaskTrigger)
                {
                    TransitionToState(AIState.Active);
                    externalTaskTrigger = false; // Reset the trigger
                        idleTime = 0f;
                }
                else if (idleTime >= TIME_TO_ACTIVE_THRESHOLD)
                {
                    TransitionToState(AIState.Active);
                }
                break;

            case AIState.Active:
                // Actively working on tasks. Check if completion target is met.
                if (dailyTasksCompleted >= TASK_COMPLETION_TARGET)
                {
                    TransitionToState(AIState.TaskComplete);
                }
                break;

            case AIState.TaskComplete:
                // Task is done. Report and move to EndSession immediately.
                TransitionToState(AIState.EndSession);
                break;

            case AIState.EndSession:
                // The session has ended. Stop processing.
                // In a real app, you might wait for a user to start a new session.
                idleTime = 0f; // Prevent Idle logic from immediately re-triggering.
                break;

    }

    /// <summary>
    /// Handles the actual state change and logs the transition.
    /// </summary>
    /// <param name="newState">The state to transition to.</param>
    private void TransitionToState(AIState newState)
    {
        // Only transition if the state is actually changing.
        if (currentState != newState)
        {
            Debug.Log($"<color=lime>STATE TRANSITION: {currentState} -> {newState}</color>");
            currentState = newState;

            // Fire the OnStateChange event (coordinating with Usman's system).
            OnStateChange?.Invoke();
        }

        // Output the correct response/action based on the new state
        OutputStateResponse(newState);
    }

    /// <summary>
    /// Mock function to simulate a task completion event.
    /// </summary>
    public void SimulateTaskCompletion()
    {
        if (currentState == AIState.Active)
        {
            dailyTasksCompleted++;
            Debug.Log($"<color=cyan>TASK EVENT: Task Completed! Total: {dailyTasksCompleted}/{TASK_COMPLETION_TARGET}</color>");
              // Fire the OnTaskCompleted event (coordinating with Usman's system)
            OnTaskCompleted?.Invoke(dailyTasksCompleted); 
        }
    }

}