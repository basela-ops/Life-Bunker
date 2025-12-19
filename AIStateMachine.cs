using UnityEngine;
using System;
using System.Collections; // Required for Coroutines

public class AIStateMachine : MonoBehaviour
{ 
    public enum AIState
    {
        Idle,
        Active,
        TaskComplete,
        EndSession
    }

    [Header("State Configuration")]
    public AIState currentState = AIState.Idle;

    [Header("Metrics & Variables")]
    public int dailyTasksCompleted = 0;
    public float idleTime = 0f;
    public bool externalTaskTrigger = false;

    private const float TIME_TO_ACTIVE_THRESHOLD = 5.0f; 
    private const int TASK_COMPLETION_TARGET = 3;       

    // Events for coordination with other scripts (e.g., UI or Local AI Controllers)
    public static event Action<AIState> OnStateChange;
    public static event Action<int> OnTaskCompleted;

    void Start()
    {
        // Force initial state logic
        TransitionToState(AIState.Idle);
    }

    void Update()
    {
        HandleMockInput();
        HandleStateLogic();
    }

    private void HandleMockInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            externalTaskTrigger = true;
            Debug.Log("<color=orange>System: External Trigger Received.</color>");
        }

        if (Input.GetKeyDown(KeyCode.T) && currentState == AIState.Active)
        {
            SimulateTaskCompletion();
        }
    }

    private void HandleStateLogic()
    {
        switch (currentState)
        {
            case AIState.Idle:
                idleTime += Time.deltaTime; 
                
                if (externalTaskTrigger)
                {
                    externalTaskTrigger = false;
                    TransitionToState(AIState.Active);
                }
                else if (idleTime >= TIME_TO_ACTIVE_THRESHOLD)
                {
                    TransitionToState(AIState.Active);
                }
                break;

            case AIState.Active:
                // Logic: In a real app, this is where your AI would call Ollama or Sentis.
                if (dailyTasksCompleted >= TASK_COMPLETION_TARGET)
                {
                    TransitionToState(AIState.TaskComplete);
                }
                break;

            case AIState.TaskComplete:
                // No logic here; TransitionToState handles the switch to EndSession via Coroutine
                break;

            case AIState.EndSession:
                // Terminal state.
                break;
        }
    }

    public void TransitionToState(AIState newState)
    {
        if (currentState == newState && newState != AIState.TaskComplete) return;

        Debug.Log($"<color=lime>STATE TRANSITION: {currentState} -> {newState}</color>");
        currentState = newState;
        
        // Notify other systems
        OnStateChange?.Invoke(newState);

        // Execute specific entry logic for the new state
        if (newState == AIState.TaskComplete)
        {
            StartCoroutine(HandleTaskCompletionSequence());
        }
        else
        {
            OutputStateResponse(newState);
        }
    }

    private IEnumerator HandleTaskCompletionSequence()
    {
        Debug.Log("<color=green>RESPONSE: Task cycle complete. Finalizing session data...</color>");
        
        // Wait for 2 seconds so the user/developer can see the "Task Complete" state
        yield return new WaitForSeconds(2.0f);
        
        TransitionToState(AIState.EndSession);
    }

    private void OutputStateResponse(AIState state)
    {
        switch (state)
        {
            case AIState.Idle:
                idleTime = 0f;
                Debug.Log("<color=grey>RESPONSE: Awaiting input...</color>");
                break;

            case AIState.Active:
                Debug.Log("<color=yellow>RESPONSE: Processing AI Logic...</color>");
                break;

            case AIState.EndSession:
                Debug.Log($"<color=magenta>RESPONSE: Session concluded. Total Tasks: {dailyTasksCompleted}.</color>");
                break;
        }
    }

    public void SimulateTaskCompletion()
    {
        dailyTasksCompleted++;
        Debug.Log($"<color=cyan>TASK EVENT: {dailyTasksCompleted}/{TASK_COMPLETION_TARGET}</color>");
        OnTaskCompleted?.Invoke(dailyTasksCompleted); 
    }
}