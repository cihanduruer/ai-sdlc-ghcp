# Attendee guide - Phases 1 to 7

Complete [Setup and Phase 0](ATTENDEE-GUIDE-PHASE-0.md) before starting this guide.

## Phase 1 - Let VS Code Copilot create the baseline (10-30 minutes)

**Outcome:** the local hotel application runs with a seeded SQLite database.

1. Open VS Code Copilot Chat, open the agent picker, and select the repository's **fullstack** custom agent.
2. Paste:

   ```text
   Create the Northstar Hotel baseline described by the repository instructions.

   Use the frontend-design skill. Before coding, propose a compact visual plan for a sleek,
   modern boutique-hotel experience whose single job is helping a guest find and book a room.
   Preserve the Northstar identity: deep harbour green, warm coral accents, generous off-white
   space, restrained serif display headings, crisp sans-serif body copy, and compact room cards.
   Make the signature element a refined availability search card that overlaps the hero and
   clearly summarizes the selected stay. Avoid a generic admin dashboard, stock travel-template
   styling, excessive gradients, glassmorphism, and decorative animation.

   Build a responsive mobile-first layout with:
   - a concise hotel hero and clear primary action,
   - labelled check-in and check-out controls with an obvious search action,
   - scannable room cards showing room name, capacity, nightly price, and availability,
   - a focused booking form and a readable current-bookings section,
   - purposeful loading, empty, validation, success, and error states.

   Use semantic HTML, visible keyboard focus, readable contrast, status announcements, and
   reduced-motion support. Keep motion limited to one subtle, useful interaction. Do not add
   a UI framework, state library, or unnecessary design dependency.

   Use a .NET 8 minimal API with EF Core SQLite and an idempotent seeder for six rooms.
   Add a React TypeScript Vite single-page UI that lists rooms, searches by check-in/check-out,
   creates bookings, shows bookings, and cancels bookings.
   Enforce non-overlapping stays and all booking rules at the API boundary. Add focused xUnit
   integration tests. Review the rendered interface at desktop and mobile widths, remove one
   unnecessary decorative element, then run tests, lint, and production builds.
   ```

3. Confirm the proposed plan preserves the Northstar Hotel palette, makes availability search the visual signature, covers all interaction states, and remains accessible on mobile before allowing edits.
4. Accept changes in small groups. Ask Copilot to resolve any failed build rather than bypassing validation.
5. Run the application using **Terminal > Run Task > app: run**, or the commands in `README.md`.
6. Book a room, then search the same dates and confirm it becomes unavailable.
7. Commit and push the baseline.

**Checkpoint:** `http://localhost:5173` displays rooms; booking creates data; tests pass; a local `.db` file is ignored by Git.

**Recovery:** compare with the [reference implementation](https://github.com/cihanduruer/ai-sdlc-ghcp). Do not copy the whole solution unless ten minutes remain.

## Phase 2 - Create workload and observe triage (30-42 minutes)

**Outcome:** GitHub turns a requirement into scored, visible work.

1. Open **Issues > New issue > Product requirement**.
2. Choose one copy-ready example from the [Phase 3 sample issue library](SAMPLE-ISSUE-LIBRARY.md) and complete the matching issue form.
3. Submit the issue.
4. Watch **Actions > 01 - Triage issue**.
5. Inspect the labels and triage comment. Ask yourself whether the area and complexity are reasonable.
6. Wait for **Sync issue to GitHub Project** to finish.
7. Open the Project's **Full board** view, clear any filters, and find the issue in the **Triage** column. Opening the issue from the card should also show Project Status **Triage** in its sidebar.

**Checkpoint:** the issue has one `area:*`, `kind:*`, `priority:*`, `complexity:*`, and `status:triaged` label, and its Project card is in **Triage**.

**Recovery:** if labels are missing, edit the issue with a harmless space and inspect **01 - Triage issue**. If labels exist but the card is absent or still in **Backlog**, inspect **Sync issue to GitHub Project**, clear board filters, and confirm the Project has a Status option named exactly `Triage`.

## Phase 3 - Exercise human governance and routing (42-52 minutes)

**Outcome:** a person approves scope before automation chooses an execution lane.

1. Read the acceptance criteria again. Improve anything ambiguous.
2. Add `ready-for-building`.
3. Open the repository **Actions** tab.
4. In the left workflow list, select **02 - Route top ready issues**.
5. Select **Run workflow**, keep the branch set to `main`, then select the green **Run workflow** button.
6. Wait for the new run to complete successfully.
7. Return to the issue and inspect the `agent:*` label and routing comment.
8. Compare the result with the routing table in [Workflow reference](WORKFLOW.md).

### Why run workflow 02 manually?

In normal operation, **02 - Route top ready issues** runs every day at **07:00 UTC**. The workshop starts it manually so you can see the routing decision immediately rather than waiting for the next schedule.

The separation is intentional:

- triage describes the issue when it is opened or edited,
- a human approves the scope with `ready-for-building`,
- workflow 02 then chooses the most suitable implementation lane.

This keeps approval and execution separate. Adding `ready-for-building` does not silently start development, and running workflow 02 still does not start remote Copilot work; `develop-with-ai` remains the final human gate.

### How selection works

The router considers open issues that have both `status:triaged` and `ready-for-building`. It ignores work already in QA or released, recommends an `agent:*` lane, and selects at most one top issue for each lane.

The summary's **Score** is only a simple ordering value:

- priority contributes the most: P1 before P2 before P3,
- complexity breaks close ties: high before medium before low,
- older issues receive a small boost.

For example, a P2 medium issue scores higher than a P3 high issue because priority has more weight. A higher score means “route this first,” not “better quality” or “higher AI confidence.”

For an event group, create the four core examples from the [sample issue library](SAMPLE-ISSUE-LIBRARY.md). The daily router selects the highest-ranked ready issue in each lane:

- hardest full-stack -> VS Code Copilot with `fullstack`,
- simple frontend -> Copilot on GitHub with `frontend`,
- documentation-only -> Copilot cloud agent with `documentation`,
- script -> Copilot CLI,
- quick logging -> Copilot cloud agent with `clean-code`.

**Checkpoint:** the workflow summary shows the selected issue, route, and score; the Project shows **Ready**; and the issue has exactly one route label.

**Recovery:** if no issue appears in the summary, confirm it has both `status:triaged` and `ready-for-building`, then rerun workflow 02. If necessary, add the expected `agent:*` label manually and record the mismatch for discussion.

## Phase 4 - Build with the selected Copilot surface (52-68 minutes)

**Outcome:** the issue becomes a focused branch and pull request.

Before implementation, open the Project's **Full board** view and confirm the approved issue is in **Ready**. Local lanes (`agent:vscode` and `agent:copilot-cli`) are human-led, so move their card to **In progress** when you begin. Remote lanes move automatically when you add `develop-with-ai`.

### `agent:vscode`

Assign the issue to yourself and move its Project card to **In progress**. In VS Code Copilot Agent mode, run `/implement-approved-issue` or paste the prompt file. Review the plan, keep the branch focused, validate, push, and open a pull request.

### `agent:copilot-cli`

Assign the issue to yourself and move its Project card to **In progress**. From the repository root run Copilot CLI, reference the issue, and ask it to implement and validate the requirement using repository instructions. Push the branch and open a pull request.

### Guided GitHub Copilot App exercise - `agent:copilot-app`

Use the sample issue **[Requirement] Show an occupancy badge on each room** for this exercise. It keeps the asynchronous implementation small enough to fit inside Phase 4; it is not an additional phase.

1. Open the issue and confirm it has `area:frontend`, `complexity:low`, `ready-for-building`, and `agent:copilot-app`.
2. Read the acceptance criteria once more. Do not approve the issue if its scope has expanded beyond the React presentation change.
3. Add `develop-with-ai`. This is the human decision that allows remote implementation.
4. Open **Actions** and wait for **03 - Delegate approved issue to Copilot** and **Sync issue to GitHub Project** to pass.
5. Return to the issue. Confirm the delegation comment names the `frontend` custom agent and the assignee is `copilot-swe-agent[bot]`.
6. Open the Project's **Full board** view, clear any filters, and confirm the issue moved from **Ready** to **In progress**.
7. Follow the Copilot agent session from the issue. Let it finish before making competing edits to its branch.
8. Open the resulting pull request and confirm:
   - it is ready for review rather than a draft,
   - its body contains `Closes #<issue>`,
   - the change stays within the frontend requirement,
   - **Build and test** and **04 - Start automatic Copilot code review** begin without manual reviewer selection.
9. Do not merge yet. Continue to Phase 5 so the VS Code `clean-code` and `qa` agents independently verify the Copilot App pull request.

**Checkpoint:** Copilot App produced a focused pull request, the Project card is **In progress**, CI started, and automatic Copilot review was requested.

**Recovery:** if workflow 03 does not assign Copilot, confirm the four labels listed in step 1 and that `COPILOT_AGENT_TOKEN` is configured. Correct the problem, then remove and re-add `develop-with-ai` to create a new delegation event.

### Other remote work - `agent:cloud`

1. Add `develop-with-ai`. The delegation workflow enforces `ready-for-building`, then assigns `copilot-swe-agent[bot]`.
2. Wait for **Sync issue to GitHub Project** and **03 - Delegate approved issue to Copilot** to finish.
3. Confirm the delegation comment names the custom agent selected from the issue labels.
4. Confirm the issue moved from **Ready** to **In progress** in the Project.
5. Watch the agent session and resulting pull request, then continue to Phase 5 without merging.

For remote delegation, the workflow derives the custom agent from triage labels:

| Issue area | Custom agent |
| --- | --- |
| `area:frontend` | `frontend` |
| `area:full-stack` | `fullstack` |
| `area:documentation` or `kind:docs` | `documentation` |
| `area:backend` | `clean-code` |

The issue comment names the selected custom agent. All remote lanes use `copilot-swe-agent[bot]`; the custom agent changes the role and instructions, not the GitHub assignee.

The `clean-code` agent uses original Northstar-specific guidance inspired by general principles popularized by Robert C. Martin: clear names, cohesive functions, explicit errors, restrained abstraction, and focused tests. It does not reproduce the book.

**Checkpoint:** the Project card is in **In progress**, a non-draft pull request exists, its body contains `Closes #<issue>`, CI starts, and **04 - Start automatic Copilot code review** begins without attendee action.

**Recovery:** if `develop-with-ai` exists but the card remains **Ready**, inspect **Sync issue to GitHub Project**, clear board filters, and confirm the Project has a Status option named exactly `In progress`. If workflow 03 fails, open its **Assign Copilot cloud agent** log first. After correcting configuration or updating the workflow, remove and re-add `develop-with-ai` to create a new delegation event. If remote agent access or tokens remain blocked, manually assign Copilot in the issue UI; if that is also unavailable, use VS Code Agent mode with the same issue.

## Phase 5 - Review and promote to QA (68-78 minutes)

**Outcome:** code is validated and reviewed before the work item changes state.

1. Wait for **Build and test** to pass.
2. In VS Code, check out the pull-request branch, open the agent picker, and select the repository's **clean-code** custom agent.
3. Ask: `Review this pull request for clear names, cohesive responsibilities, explicit errors, unnecessary complexity, and focused tests. Report required fixes separately from optional improvements; do not change product scope.`
4. Apply required findings, rerun the affected validation, and record a short maintainability note in the pull request.
5. Select the repository's **qa** custom agent. For the guided Copilot App exercise, verify the Copilot-created occupancy-badge pull request rather than asking QA to implement more changes.
6. Ask: `Verify this pull request against its linked issue. Produce a compact test matrix, run the required validation, and report blockers without changing product scope.`
7. Record the QA agent's pass/fail evidence in the pull request.
8. Do not request a reviewer manually. Confirm **04 - Start automatic Copilot code review** completed and the PR shows Copilot under **Reviewers**.
9. If a human submits a review first, workflow 05 records it as an intentional no-op; it does not promote the issue or show a misleading skipped job.
10. Read every Copilot review comment. Apply or explicitly dismiss suggestions; Copilot comments are advice, not approval.
11. Ensure the PR remains linked to the issue with `Closes #<issue>`.
12. After Copilot submits its review, inspect **05 - Promote reviewed work to QA**.
13. Merge the pull request into `main` after event branch policy requirements are satisfied.
14. Open **Code**, switch to `main`, and confirm the merged commit and implementation files are present before approving release.

**Checkpoint:** the clean-code agent produced a maintainability note, the QA agent produced test evidence, the linked issue has `ready-for-qa`, the Project shows **QA**, and the approved code is merged into `main`.

**Recovery:** if workflow 05 reports that it recorded a non-Copilot review, wait for Copilot; rerunning the human-review event cannot promote the issue. If workflow 04 comments that automatic review could not start, confirm repository policy first, then use the **Reviewers** control only as the workshop fallback. If time is short, combine the clean-code and QA requests into one QA pass. An event organizer may add `ready-for-qa` only after showing evidence of a completed Copilot review.

## Phase 6 - Approve and simulate release (78-87 minutes)

**Outcome:** a final human decision tests the merged `main` branch and produces release evidence but no deployment.

1. Confirm the pull request is merged and the approved implementation exists on `main`. Workflow 06 deliberately ignores unmerged pull-request code.
2. Review the acceptance criteria and CI evidence.
3. Add `release-to-production` to the issue.
4. Watch **06 - Simulate production release** check out `main`, rerun tests, and build the web assets.
5. Open the workflow artifact `simulated-production-release`.
6. Inspect `manifest.json` and the built `web` directory.

**Checkpoint:** the manifest identifies the released `main` commit, all release validation passes, an artifact exists, the issue has `released`, and the Project shows **Done**.

**Recovery:** if the PR is still open, remove `release-to-production`, merge the approved PR, and re-add the label. After any failed release test, fix and merge the correction to `main`, then remove and re-add `release-to-production`. Never add `released` manually.

## Phase 7 - Reflect (87-90 minutes)

Write one sentence for each:

1. Which decision stayed human, and why?
2. Which Copilot surface best matched its task?
3. What evidence would you add before using this lifecycle in production?

You have completed the hack when you can show the Project moving from Triage to Done and explain every human gate.

**Previous:** [Setup and Phase 0](ATTENDEE-GUIDE-PHASE-0.md).
