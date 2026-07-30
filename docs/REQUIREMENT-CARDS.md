# Requirement cards

Use these pre-sized issues to demonstrate every routing lane. Create them through the Product requirement issue form.

## Card A - hardest full-stack -> VS Code Copilot

**Title:** `[Requirement] Let guests amend booking dates`

**Desired outcome:** A guest can change the dates of an existing booking. The API prevents overlap with another booking for the same room and the UI shows the recalculated total before saving.

**Acceptance criteria:**

- [ ] Existing booking dates can be edited from the bookings list.
- [ ] Check-out remains after check-in and check-in is not in the past.
- [ ] Conflicting dates return a clear message and do not change the database.
- [ ] The new total uses the room's nightly price and updated night count.
- [ ] API integration tests cover successful and conflicting amendments.

Expected route: `area:full-stack`, `complexity:high`, `agent:vscode`.

## Card B - simple frontend -> Copilot on GitHub

**Title:** `[Requirement] Show an occupancy badge on each room`

**Desired outcome:** Each room card clearly shows whether it is for one guest, two guests, or a family, without changing the API.

**Acceptance criteria:**

- [ ] Capacity 1 shows `Solo`.
- [ ] Capacity 2 shows `Couples`.
- [ ] Capacity 3 or more shows `Family`.
- [ ] The badge is readable on mobile and by a screen reader.

Expected route: `area:frontend`, `complexity:low`, `agent:copilot-app`.

## Card C - script feature -> Copilot CLI

**Title:** `[Requirement] Add a safe local database reset script`

**Desired outcome:** A developer can reset only the local hotel database and restart the API so the idempotent seeder recreates sample rooms.

**Acceptance criteria:**

- [ ] A PowerShell script resolves the database path from the repository root.
- [ ] The script refuses paths outside the API project.
- [ ] The script removes only known SQLite database and sidecar files.
- [ ] Documentation includes the exact command and a data-loss warning.

Expected route: `area:automation`, `kind:script`, `agent:copilot-cli`.

## Card D - quick logging fix -> Copilot cloud agent

**Title:** `[Bug] Log booking cancellation outcomes`

**Desired outcome:** The API writes a structured information log when a booking is cancelled and a structured warning when a cancellation references a missing booking.

**Acceptance criteria:**

- [ ] Successful cancellation logs booking ID and room ID without guest personal data.
- [ ] Missing booking logs the requested ID.
- [ ] HTTP behavior remains 204 for success and 404 when missing.
- [ ] Tests prove both HTTP outcomes.

Expected route: `kind:logging`, `complexity:low`, `agent:cloud`.
