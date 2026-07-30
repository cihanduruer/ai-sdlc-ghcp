# 90-minute self-led attendee guide

## Mission

Start in an empty repository and deliver one Northstar Hotel requirement through this lifecycle:

`Issue -> triage -> human approval -> route -> implementation -> Copilot review -> QA -> simulated release`

Keep moving when a checkpoint passes. Use the recovery path only when it does not.

## Phase 0 - Ready the empty repository (0-10 minutes)

**Outcome:** an empty application repository contains only the workshop's AI SDLC kit.

1. Create an empty GitHub repository named `northstar-hotel-<your-handle>`. Do not initialize it with a README.
2. Clone it and open it in VS Code:

   ```powershell
   gh repo clone <OWNER>/northstar-hotel-<your-handle>
   Set-Location northstar-hotel-<your-handle>
   code .
   ```

3. Copy the workshop kit without copying the reference application:

   ```powershell
   git remote add workshop <WORKSHOP_REPOSITORY_GIT_URL>
   git fetch workshop main
   git checkout workshop/main -- .github .vscode .editorconfig .gitignore .npmrc
   git add .
   git commit -m "Add iCSU MiniHack AI SDLC kit"
   git push -u origin main
   ```

4. In GitHub Actions, run **00 - Set up repository labels**.
5. Create a Project and variables using [GitHub setup](SETUP.md). Pair with the facilitator for tokens if event policy centralizes secrets.

**Checkpoint:** the repository has no `src` folder, labels exist, `.npmrc` points to the approved Microsoft package feed, and VS Code detects the recommended extensions.

**Recovery:** if the `git checkout workshop/main` command fails, download the workshop repository ZIP and copy only `.github`, `.vscode`, `.editorconfig`, `.gitignore`, and `.npmrc`.

## Phase 1 - Let VS Code Copilot create the baseline (10-30 minutes)

**Outcome:** the local hotel application runs with a seeded SQLite database.

1. Open VS Code Copilot Chat in **Agent** mode.
2. Paste:

   ```text
   Create the Northstar Hotel baseline described by the repository instructions.
   Use a .NET 8 minimal API with EF Core SQLite and an idempotent seeder for six rooms.
   Add a React TypeScript Vite single-page UI that lists rooms, searches by check-in/check-out,
   creates bookings, shows bookings, and cancels bookings.
   Enforce non-overlapping stays in the API and add xUnit integration tests.
   Keep the design polished but dependency-light. Run tests, lint, and builds.
   ```

3. Review the proposed plan before allowing edits.
4. Accept changes in small groups. Ask Copilot to resolve any failed build rather than bypassing validation.
5. Run the application using **Terminal > Run Task > app: run**, or the commands in `README.md`.
6. Book a room, then search the same dates and confirm it becomes unavailable.
7. Commit and push the baseline.

**Checkpoint:** `http://localhost:5173` displays rooms; booking creates data; tests pass; a local `.db` file is ignored by Git.

**Recovery:** compare with the reference implementation in `<WORKSHOP_REPOSITORY_URL>`. Do not copy the whole solution unless ten minutes remain.

## Phase 2 - Create workload and observe triage (30-42 minutes)

**Outcome:** GitHub turns a requirement into scored, visible work.

1. Open **Issues > New issue > Product requirement**.
2. Choose one card from [Requirement cards](REQUIREMENT-CARDS.md) and copy its title, outcome, and acceptance criteria.
3. Submit the issue.
4. Watch **Actions > 01 - Triage issue**.
5. Inspect the labels and triage comment. Ask yourself whether the area and complexity are reasonable.
6. Open the Project and confirm Status moved to **Triage**.

**Checkpoint:** the issue has one `area:*`, `kind:*`, `priority:*`, `complexity:*`, and `status:triaged` label.

**Recovery:** rerun the setup-label workflow, edit the issue with a harmless space, and inspect the triage run logs.

## Phase 3 - Exercise human governance and routing (42-52 minutes)

**Outcome:** a person approves scope before automation chooses an execution lane.

1. Read the acceptance criteria again. Improve anything ambiguous.
2. Add `ready-for-building`.
3. Run **Actions > 02 - Route top ready issues > Run workflow**.
4. Inspect the `agent:*` label and routing comment.
5. Compare the result with the routing table in [Workflow reference](WORKFLOW.md).

For an event group, create all four requirement cards. The daily router selects the highest-ranked ready issue in each lane:

- hardest full-stack -> VS Code Copilot,
- simple frontend -> Copilot on GitHub,
- script -> Copilot CLI,
- quick logging -> Copilot cloud agent.

**Checkpoint:** the Project shows **Ready** and the issue has exactly one route label.

**Recovery:** route manually by adding the expected `agent:*` label, then continue. Record the automation mismatch for discussion.

## Phase 4 - Build with the selected Copilot surface (52-68 minutes)

**Outcome:** the issue becomes a focused branch and pull request.

### `agent:vscode`

Assign the issue to yourself. In VS Code Copilot Agent mode, run `/implement-approved-issue` or paste the prompt file. Review the plan, keep the branch focused, validate, push, and open a pull request.

### `agent:copilot-cli`

Assign the issue to yourself. From the repository root run Copilot CLI, reference the issue, and ask it to implement and validate the requirement using repository instructions. Push the branch and open a pull request.

### `agent:copilot-app` or `agent:cloud`

Add `develop-with-ai`. The delegation workflow enforces `ready-for-building`, then assigns `copilot-swe-agent[bot]`. Watch the agent session and resulting pull request.

For the event, the App lane uses the `frontend` custom agent for a small UI feature. The Cloud lane demonstrates a bounded backend fix. Both execute remotely; their distinction is the task profile and custom instructions, not a different GitHub assignee.

**Checkpoint:** a non-draft pull request exists, its body contains `Closes #<issue>`, and CI starts.

**Recovery:** if remote agent access or tokens are blocked, manually assign Copilot in the issue UI. If that is also unavailable, use VS Code Agent mode with the same issue and continue.

## Phase 5 - Review and promote to QA (68-78 minutes)

**Outcome:** code is validated and reviewed before the work item changes state.

1. Wait for **Build and test** to pass.
2. Confirm **04 - Request Copilot code review** requested Copilot.
3. Read every Copilot review comment. Apply or explicitly dismiss suggestions; Copilot comments are advice, not approval.
4. Ensure the PR remains linked to the issue with `Closes #<issue>`.
5. After Copilot submits its review, inspect **05 - Promote reviewed work to QA**.
6. Merge the pull request after event branch policy requirements are satisfied.

**Checkpoint:** the linked issue has `ready-for-qa` and the Project shows **QA**.

**Recovery:** if organization policy prevents automatic review, request Copilot from the Reviewers control. The facilitator may add `ready-for-qa` after showing evidence of a completed Copilot review.

## Phase 6 - Approve and simulate release (78-87 minutes)

**Outcome:** a final human decision produces tested release evidence but no deployment.

1. Review the acceptance criteria and CI evidence.
2. Add `release-to-production` to the issue.
3. Watch **06 - Simulate production release**.
4. Open the workflow artifact `simulated-production-release`.
5. Inspect `manifest.json` and the built `web` directory.

**Checkpoint:** all release validation passes, an artifact exists, the issue has `released`, and the Project shows **Done**.

**Recovery:** remove and re-add `release-to-production` after fixing a failed test on `main`. Never add `released` manually.

## Phase 7 - Reflect (87-90 minutes)

Write one sentence for each:

1. Which decision stayed human, and why?
2. Which Copilot surface best matched its task?
3. What evidence would you add before using this lifecycle in production?

You have completed the hack when you can show the Project moving from Triage to Done and explain every human gate.
