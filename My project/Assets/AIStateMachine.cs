using UnityEngine;
using System; 
// *** ADD THIS NAMESPACE ***
using UnityEngine.InputSystem; 

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
    // Tracks the number of tasks completed in the current session.
    public int dailyTasksCompleted = 0;

     // Tracks how long the AI has been in the Idle state.
    public float idleTime = 0f;

     // A flag to simulate a 'trigger' from Usman's external system.
    public bool externalTaskTrigger = false;

    // --- Thresholds for State Changes ---
    // 5 seconds of idle time triggers Active (Internal Trigger).
    private const float TIME_TO_ACTIVE_THRESHOLD = 5.0f; 
    // 3 tasks must be completed to trigger EndSession (Internal Goal).
    private const int TASK_COMPLETION_TARGET = 3;       

    // --- Event Delegates (Mocking coordination with other systems) ---
    // Declaring events for communication with other C# scripts.
    public static event Action OnStateChange;
    public static event Action<int> OnTaskCompleted;

    // Start is called once before the first frame update
    void Start()
    {
        // Set the initial state and log the beginning of the session.
        TransitionToState(AIState.Idle);
    }

    // Update is called once per frame (Unity's main game loop)
    void Update()
    {
        // 1. Handle external manual triggers (for testing/mocking)
        HandleMockInput();

        // 2. Run the logic specific to the current state.
        HandleStateLogic();
    }

    /// <summary>
    /// Handles user input for simulating external triggers using the new Input System.
    /// *** UPDATED METHOD ***
    /// </summary>
    private void HandleMockInput()
    {
        // Check if the keyboard is available
        if (Keyboard.current == null)
        {
            return; 
        }

        // Check for 'S' key press (to simulate an external system triggering a task)
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            externalTaskTrigger = true;
            Debug.Log("<color=orange>External System Triggered Task via 'S' key!</color>");
        }

        // Check for 'T' key press (to simulate task completion)
        if (Keyboard.current.tKey.wasPressedThisFrame && currentState == AIState.Active)
        {
            SimulateTaskCompletion();
        }
    }

    /// <summary>
    /// Executes the specific actions and transition checks for the current AI state.
    /// </summary>
    private void HandleStateLogic()
    {
        // The 'switch' statement manages the state logic efficiently.
        switch (currentState)
        {
            case AIState.Idle:
                // Check for time or an external trigger to go Active.
                idleTime += Time.deltaTime; 
                
                if (externalTaskTrigger)
                {
                    TransitionToState(AIState.Active);
                    externalTaskTrigger = false; // Reset the trigger after use
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
                idleTime = 0f; 
                break;
        }
    }

    /// <summary>
    /// Handles the actual state change, logs the transition, and outputs the response.
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

        // Output the correct response/action based on the new state (DELIVERABLE REQUIREMENT)
        OutputStateResponse(newState);
    }

    /// <summary>
    /// Outputs the correct, human-readable response based on the new state.
    /// </summary>
    private void OutputStateResponse(AIState state)
    {
        switch (state)
        {
            case AIState.Idle:
                Debug.Log("<color=grey>RESPONSE: Entering sleep mode. Awaiting external task trigger or time threshold.</color>");
                break;

            case AIState.Active:
                // Determine if the transition was from the time threshold or an external trigger.
                if (idleTime > 0f) 
                {
                    Debug.Log("<color=yellow>RESPONSE: Time threshold reached. Auto-starting task execution.</color>");
                }
                else
                {
                    Debug.Log("<color=yellow>RESPONSE: External task received. Entering Active mode for processing.</color>");
                }
                break;

            case AIState.TaskComplete:
                // This state is immediately followed by EndSession in current logic.
                Debug.Log($"<color=green>RESPONSE: Task cycle complete. Finalizing session data.</color>");
                break;

            case AIState.EndSession:
                Debug.Log($"<color=magenta>RESPONSE: Session concluded! Successfully completed {dailyTasksCompleted} tasks. Saving metrics and entering deep sleep.</color>");
                break;
        }
    }

    /// <summary>
    /// Mock function to simulate a task completion event, only callable during the Active state.
    /// </summary>
    public void SimulateTaskCompletion()
    {
        if (currentState == AIState.Active)
        {
            dailyTasksCompleted++;
            Debug.Log($"<color=cyan>TASK EVENT: Task Completed! Total: {dailyTasksCompleted}/{TASK_COMPLETION_TARGET}</color>");
            // Fire the OnTaskCompleted event (coordinating with Usman's system)
            OnTaskCompleted?.Invoke(dailyTasksCompleted); 

            // If a task is completed, check the completion target immediately.
            if (dailyTasksCompleted >= TASK_COMPLETION_TARGET)
            {
                TransitionToState(AIState.TaskComplete);
            }
        }
    }
}