# Mock AI Engine – State Logic Documentation

## Overview

This document provides a simple overview of the mock AI engine used to simulate basic AI state transitions based on external triggers.  
The engine demonstrates how an AI agent can move between different operational states such as **IDLE**, **ACTIVE**, **TASK_DONE**, and **END_DAY**.

---

## State Definitions

| State       | Description |
|-------------|-------------|
| **IDLE**    | Default resting state. Waiting for a valid command to begin work. |
| **ACTIVE**  | Task is in progress. Engine is currently processing. |
| **TASK_DONE** | Task has been completed. Awaiting either reset or shutdown. |
| **END_DAY** | Final shutdown state. No further triggers are processed. |

---

## Trigger → State Transition Logic

### **1. IDLE State**

| Trigger        | Resulting State | Output |
|----------------|-----------------|--------|
| `start_task`   | ACTIVE          | Begins task processing. |
| `end_shift`    | END_DAY         | Shuts down for the day. |
| Any other input | IDLE          | Unrecognized trigger. |

---

### **2. ACTIVE State**

| Trigger          | Resulting State | Output |
|------------------|-----------------|--------|
| `task_complete`  | TASK_DONE       | Marks task as finished. |
| `force_stop`     | IDLE            | Emergency stop. |
| Any other input   | ACTIVE         | Non-critical triggers ignored. |

---

### **3. TASK_DONE State**

| Trigger       | Resulting State | Output |
|---------------|-----------------|--------|
| `reset`       | IDLE            | Logs results and resets for next task. |
| `end_shift`   | END_DAY         | Ending the workday. |
| Any other input | TASK_DONE     | Task is done—awaiting next instructions. |

---

### **4. END_DAY State**

| Trigger | Resulting State | Output |
|--------|------------------|--------|
| Any trigger | END_DAY | Engine remains powered down. |

---

## Example Trigger Flow

A sample execution sequence:

1. `start_task` → AI becomes **ACTIVE**
2. `status_check` → Ignored while ACTIVE
3. `task_complete` → Moves to **TASK_DONE**
4. `reset` → Returns to **IDLE**
5. `start_task` → Moves to **ACTIVE**
6. `end_shift` → Moves to **END_DAY**
7. Any further triggers → No effect

---

## Notes

- This mock engine is intended for demonstration and early design discussions.
- Future expansions may include:
  - Task queuing
  - Error states
  - Multi-agent interaction
  - Timed state transitions

---

## File Reference

**Source Code:** `mock_ai_engine.py
