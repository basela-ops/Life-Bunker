# Work Room Trigger Flow

This document defines the event-driven flow for the Work Room, including the system states and AI triggers.

## States
- `outside_work_room`
- `in_work_room_active`
- `in_work_room_idle`

## Trigger Events
- `ENTER_WORK_ROOM`
- `IDLE_TIMEOUT`
- `RESUME_ACTIVITY`
- `TASK_COMPLETE`
- `EXIT_WORK_ROOM`

---

## 1. Enter Work Room (`ENTER_WORK_ROOM`)
**Condition:** Player enters the Work Room.

**Actions**
- Set state → `in_work_room_active`
- Log `session_start_time`
- Add session tag: `["work_room", "enter"]`
- Load active goal + tasks
- Trigger AI response group: `work_room.enter`

**Sample AI behavior**
- “Welcome back. Let’s pick one small thing to move forward on.”

---

## 2. Idle (`IDLE_TIMEOUT`)
**Condition:** No input for X minutes.

**Actions**
- Set state → `in_work_room_idle`
- Add tag: `["idle"]`
- Trigger AI response group: `work_room.idle`

**Sample AI behavior**
- “I notice things got quiet. Do you want to pause or keep going?”

---

## 3. Task Complete (`TASK_COMPLETE`)
**Condition:** User marks a task as done.

**Actions**
- Update task → `completed`
- If all tasks for a goal are complete → mark goal as `completed`
- Add tag: `["task_complete"]`
- Trigger AI response group: `work_room.task_complete`

**Sample AI behavior**
- “Nice work—that’s one task off your plate.”

---

## 4. Exit Work Room (`EXIT_WORK_ROOM`)
**Condition:** User leaves room or ends session.

**Actions**
- Compute `session_duration`
- Add tag: `["exit"]`
- Trigger AI response group: `work_room.exit`

**Sample AI behavior**
- “You focused for 42 minutes. That’s real progress.”