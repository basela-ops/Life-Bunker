# Trigger Mapping Reference

This document defines which in-game events should call which AI JSON entries.

## How to call
Unity should call:
- ShowResponse(room, trigger)
or the wrapper methods:
- BtnStartDay(), BtnIdle(), BtnTaskComplete(), BtnEndDay()

## Mapping

### Global triggers
- Start of session / start of day UI opens
  - room: global
  - trigger: start_day

- Player ends a session / end-of-day summary screen
  - room: global
  - trigger: end_day

- Energy meter drops below threshold (example: <= 20%)
  - room: global
  - trigger: low_energy

### Work Room triggers
- Player enters the Work Room scene/panel
  - room: work_room
  - trigger: enter_room

- Player idle detected in Work Room (example: no input for 30–60 seconds)
  - room: work_room
  - trigger: idle

- Task completion event (task marked done)
  - room: work_room
  - trigger: task_complete

## Notes
- Trigger names must match JSON exactly (case + underscores).
- If a trigger has no matching JSON entry, the manager should log a warning and show nothing (or show a fallback if added later).