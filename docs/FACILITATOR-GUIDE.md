# Facilitator guide

## Event shape

- Duration: 90 minutes
- Format: self-led, individual or pairs
- Core tools: GitHub, GitHub Projects, GitHub Copilot, VS Code, GitHub Actions
- Reference product: Northstar Hotel
- Deployment: simulated artifact only

## One week before

1. Publish this repository at the URL used in event communications.
2. Confirm attendee licenses and organization policies for Copilot cloud agent and code review.
3. Confirm every attendee configured `https://packagefeedproxy.microsoft.io/npm/` as the NPM registry.
4. Decide whether attendees create personal repositories or repositories in an event organization.
5. Decide who creates and owns `COPILOT_AGENT_TOKEN` and `PROJECT_TOKEN`. Do not distribute tokens in chat or email.
6. Create a fallback reference Project and pre-run all workflows.

## One day before

1. Run all local validation commands from `README.md`.
2. Run **00 - Set up repository labels**.
3. Create the five issues from `REQUIREMENT-CARDS.md`, add `ready-for-building`, and run the daily router.
4. Delegate Card D and confirm the issue comment names the `clean-code` custom agent.
5. Delegate Card E and confirm the issue comment names the `documentation` custom agent.
6. Open a test pull request and confirm Copilot review can be requested.
7. Run a release simulation on a disposable issue.
8. Confirm Actions minutes, AI premium request budget, and repository write permissions.

## Timing guardrails

| Minute | Expected checkpoint |
| --- | --- |
| 10 | Empty attendee repo contains the automation kit |
| 30 | Local hotel baseline runs |
| 42 | Requirement is triaged |
| 52 | Daily route is visible |
| 68 | Pull request exists |
| 78 | Copilot review evidence and QA label exist |
| 87 | Release artifact exists |
| 90 | Participant can explain human gates |

Call time at each checkpoint. The goal is lifecycle understanding, not feature polish.

## Fallback ladder

1. **Copilot remote agent is slow:** continue with VS Code Agent mode on the same issue.
2. **Assignment API blocked:** assign Copilot manually in the issue UI.
3. **Copilot review API blocked:** request Copilot manually from Reviewers.
4. **Project token blocked:** move the Project card manually while keeping label automation.
5. **Participant baseline incomplete at minute 30:** copy `src`, `tests`, and `HotelBooking.slnx` from the reference repository.
6. **Release fails:** use the failure as the QA lesson; do not manually add `released`.

## Debrief prompts

- Which labels represent machine analysis, and which represent human authority?
- Why are VS Code and CLI routes assigned to a person?
- What could go wrong if `develop-with-ai` were added automatically?
- Which token boundaries would become GitHub Apps in production?
- What evidence is missing for a real production release?

## Production caveats to state aloud

- The triage classifier is a workshop heuristic, not a substitute for product ownership.
- SQLite and the seeded data are local demo choices.
- Copilot review comments do not approve a pull request.
- The cloud-agent API is a policy-controlled preview surface.
- Real deployment needs environments, protection rules, secrets, observability, rollback, and release ownership.
