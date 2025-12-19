# Technical Documentation: AI State Machine System

**Developer:** Usman Chaudhry  

---

## 1. State Machine Overview

The `AIStateMachine` is a Finite State Machine (FSM) built in C# for Unity. It governs the lifecycle of an AI agent, ensuring logical flow and data integrity between session states.

### State Transition Logic

* **Idle**: The default starting state.
  * *Transitions to Active if:* The `idleTime` exceeds 5.0 seconds (Internal Timer) **OR** the user presses the **'S'** key (External Trigger).

* **Active**: The processing state where the AI handles data tasks.
  * *Transitions to TaskComplete if:* The user presses the **'T'** key to complete a task, reaching the target of 3 total tasks.

* **TaskComplete**: A temporary validation state.
  * *Behavior:* Triggers a 2-second **Coroutine** delay to simulate data saving and finalization.
  * *Transitions to EndSession:* Automatically after the delay.

* **EndSession**: The terminal state.
  * *Behavior:* Shuts down active logic and logs final session metrics to the console.

---

## 2. Research Summary: Local AI Integration

As part of the requirement to explore offline AI options, the following three technologies were researched for potential integration into the **Active** state:

| Technology | Type | Best Use Case |
| :----------- | :----- | :-------------- |
| **Ollama** | Local LLM Server | Running models like Llama 3 locally via JSON API calls. |
| **Unity Sentis** | Inference Engine | Running `.onnx` models directly on the local GPU in Unity. |
| **LM Studio** | Local Host | Testing open-source models (Mistral, Phi-3) with an HTTP endpoint. |

---

## 3. How to Run the Script

1. **Attach** the `AIStateMachine.cs` script to any GameObject in a Unity Scene.
2. **Open** the Console Window (**Ctrl + Shift + C**).
3. **Enter Play Mode** and interact using the following keys:
   * **Press 'S'**: Manually triggers the transition from Idle to Active.
   * **Press 'T'**: Records a completed task (requires 3 to finish).
4. **Observe Logs**: Watch the color-coded console outputs to verify the logic flow.

---

## 4. Technical Logic (Code Stub)

The system utilizes a `switch` statement within `Update()` for O(1) state evaluation:

```csharp
switch (currentState) {
    case AIState.Idle:
        // Standby logic
        break;
    case AIState.Active:
        // Processing logic
        break;
}
``'
