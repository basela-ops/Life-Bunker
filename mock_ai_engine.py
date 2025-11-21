import time

# AI State Defintions

AI_STATE = {
    "IDLE": "IDLE",
    "ACTIVE": "ACTIVE",
    "TASK_DONE": "TASK_DONE",
    "END_DAY": "END_DAY"
}

class MockAIEngine:
    """
    A simple engine to demonstrate how the state logic works based on external triggers"""

    def __init__(self):
        # Start in the IDLE State
        self.state = AI_STATE["IDLE"]
        print(f"--- AI Engine Initialized ---")
        print(f"Current State: {self.state}")

    def process_trigger(self, trigger:str):
        """
        Processes a trigger, updates the state of the engine, then returns a response"""


        trigger = trigger.lower().strip()
        response = ""

        print(f"\n[Trigger Received]: '{trigger}'")

        if self.state == AI_STATE["IDLE"]:
            if trigger == "start_task":
                self.state = AI_STATE["ACTIVE"]
                response = "Transitioning to ACTIVE state. Task initiated and processing..."
            elif trigger == "end_shift":
                self.state = AI_STATE["END_DAY"]
                response = "Transitioning to END_DAY state. Shutting down systems."
            else:
                response = "Trigger not recognized. Remaining IDLE."
        elif self.state == AI_STATE["ACTIVE"]:
            if trigger == "task_complete":
                self.state = AI_STATE["TASK_DONE"]
                # FIX: Corrected typo 'Transitiong' to 'Transitioning'
                response = "Task processing finished. Transitioning to TASK_DONE state." 
            elif trigger == "force_stop":
                self.state = AI_STATE["IDLE"]
                # FIX: Corrected typo 'Emergeny' to 'Emergency'
                response = "Emergency stop received. Returning to IDLE state." 
            else:
                response = "Currently processing. Ignoring non-critical trigger"
        
        elif self.state == AI_STATE["TASK_DONE"]:
            if trigger == "reset":
                self.state = AI_STATE["IDLE"]
                response = "Results logged and reported. Returning to IDLE state, awaiting next task."
            elif trigger == "end_shift":
                self.state = AI_STATE["END_DAY"]
                # FIX: Corrected typo 'Transitiong' to 'Transitioning'
                response = "Results logged. Transitioning to END_DAY state."
            else:
                response = "Task completed. Awaiting 'reset' or 'end_shift' trigger."
        
        elif self.state == AI_STATE["END_DAY"]:
            response = "Engine is powered down. No further actions possible until restart."

        # Output the result of the trigger processing
        print(f"[State Updated]: {self.state}")
        return response
    

# Example Execution (Demonstrating trigger -> Output Behavior)

if __name__ == "__main__":
    engine = MockAIEngine()

    # 1. Trigger to start a task
    print("Response:", engine.process_trigger("start_task"))
    time.sleep(0.5)

    # 2. Trigger while active
    print("Response:", engine.process_trigger("status_check"))
    time.sleep(0.5)
    
    # 3. Trigger to complete the task
    print("Response:", engine.process_trigger("task_complete"))
    time.sleep(0.5)

    # 4. Trigger to reset and prepare for next task
    print("Response:", engine.process_trigger("reset"))
    time.sleep(0.5)
    
    # 5. Start a new task and then end the shift
    print("Response:", engine.process_trigger("start_task"))
    time.sleep(0.5)
    
    # 6. Trigger to end the shift (while ACTIVE)
    print("Response:", engine.process_trigger("end_shift"))
    time.sleep(0.5)
    
    # 7. Attempt a trigger after shutdown
    print("Response:", engine.process_trigger("start_task"))