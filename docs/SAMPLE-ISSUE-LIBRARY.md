# Phase 3 sample issue library

Use these copy-ready issues to demonstrate the four core routing levels. Create each one through **Issues > New issue > Product requirement**, unless the example says to use **Bug report**.

Choose one issue during an individual run. Facilitators can create all four to show that workflow **02 - Route top ready issues** selects at most one top issue for each lane.

## Level 1 - Hardest full-stack -> VS Code Copilot

**Why this lane:** the change crosses the UI, API, booking rules, database behavior, and tests. A person should guide architecture decisions interactively in VS Code.

**Issue form:** Product requirement

**Title:** `[Requirement] Let guests amend booking dates`

**Problem or opportunity:** Guests currently need to cancel and recreate a booking when their travel dates change. This is slow and can cause them to lose the room.

**Desired outcome:** A guest can change the dates of an existing booking. The API prevents overlap with another booking for the same room and the UI shows the recalculated total before saving.

**Acceptance criteria:**

- [ ] Existing booking dates can be edited from the bookings list.
- [ ] Check-out remains after check-in and check-in is not in the past.
- [ ] Conflicting dates return a clear message and do not change the database.
- [ ] The new total uses the room's nightly price and updated night count.
- [ ] API integration tests cover successful and conflicting amendments.

**Context and constraints:** Preserve inclusive check-in and exclusive check-out behavior. Do not add authentication or a new UI framework.

**Business impact:** Important

**Expected evidence:** `area:full-stack`, `complexity:high`, and then `agent:vscode` after workflow 02 runs.

## Level 2 - Simple frontend -> Copilot on GitHub

**Why this lane:** the requirement is small, visual, and isolated to the React client, making it suitable for asynchronous implementation.

**Issue form:** Product requirement

**Title:** `[Requirement] Show an occupancy badge on each room`

**Problem or opportunity:** Guests must read the numeric capacity and interpret whether a room suits their party.

**Desired outcome:** Each room card uses its existing capacity value to show whether it is for one guest, two guests, or a family.

**Acceptance criteria:**

- [ ] Capacity 1 shows `Solo`.
- [ ] Capacity 2 shows `Couples`.
- [ ] Capacity 3 or more shows `Family`.
- [ ] The badge is readable on mobile and by a screen reader.

**Context and constraints:** Reuse the existing room-card styles and capacity data. This is a presentation-only change.

**Business impact:** Normal

**Expected evidence:** `area:frontend`, `complexity:low`, and then `agent:copilot-app` after workflow 02 runs. Remote delegation selects the `frontend` custom agent.

## Level 3 - Script feature -> Copilot CLI

**Why this lane:** the work is command-line focused and benefits from local path checks and direct script execution.

**Issue form:** Product requirement

**Title:** `[Requirement] Add a safe local database reset script`

**Problem or opportunity:** Workshop attendees need a repeatable way to reset sample data without accidentally deleting files outside the API project.

**Desired outcome:** A developer can reset only the local hotel database and restart the API so the idempotent seeder recreates sample rooms.

**Acceptance criteria:**

- [ ] A PowerShell script resolves the database path from the repository root.
- [ ] The script refuses paths outside the API project.
- [ ] The script removes only known SQLite database and sidecar files.
- [ ] Documentation includes the exact command and a data-loss warning.

**Context and constraints:** Support Windows PowerShell used in the workshop. Do not delete directories or use wildcard deletion.

**Business impact:** Normal

**Expected evidence:** `area:automation`, `kind:script`, and then `agent:copilot-cli` after workflow 02 runs.

## Level 4 - Quick logging fix -> Copilot cloud agent

**Why this lane:** the backend change is bounded, has a short test loop, and does not require interactive product decisions.

**Issue form:** Bug report

**Title:** `[Bug] Booking cancellation outcomes are not logged`

**What happened?:** Cancelling a booking, or requesting a booking that does not exist, produces no structured diagnostic event.

**What should happen?:** A successful cancellation writes an information log, while a missing booking writes a warning. Logs must not contain guest personal data.

**Steps to reproduce:**

1. Start the API and cancel an existing booking.
2. Attempt to cancel a booking ID that does not exist.
3. Inspect the API logs and observe that neither outcome is recorded.

**Evidence:** Capture only sanitized local log output; do not include guest names or contact details.

**Impact:** Minor

**Acceptance criteria to add in the Evidence field after any sanitized log excerpt:**

- [ ] Successful cancellation logs booking ID and room ID without guest personal data.
- [ ] Missing booking logs the requested ID.
- [ ] HTTP behavior remains 204 for success and 404 when missing.
- [ ] Tests prove both HTTP outcomes.

**Expected evidence:** `area:backend`, `kind:logging`, `complexity:low`, and then `agent:cloud` after workflow 02 runs. Remote delegation selects the `clean-code` custom agent.

## Bonus - Documentation-only -> Copilot cloud agent

Use this optional example to demonstrate the `documentation` custom agent.

**Issue form:** Product requirement

**Title:** `[Requirement] Add a guest cancellation troubleshooting section`

**Problem or opportunity:** Guests and attendees cannot easily distinguish a successful cancellation from a missing booking.

**Desired outcome:** Documentation explains both outcomes and the next action without requiring readers to inspect implementation code.

**Acceptance criteria:**

- [ ] The attendee guide explains both cancellation outcomes in guest-facing language.
- [ ] Commands and HTTP status names match the implementation.
- [ ] The section includes expected evidence and one recovery action.
- [ ] No token, personal identifier, or environment-specific URL appears.

**Context and constraints:** Documentation only; do not change product behavior.

**Business impact:** Normal

**Expected evidence:** `area:documentation`, `kind:docs`, and then `agent:cloud` after workflow 02 runs. Remote delegation selects the `documentation` custom agent.

## If the expected route does not appear

Confirm the issue text contains the supplied area signals, wait for **01 - Triage issue**, add `ready-for-building`, and rerun **02 - Route top ready issues**. The router reads labels produced by triage; it does not route from this page directly.
