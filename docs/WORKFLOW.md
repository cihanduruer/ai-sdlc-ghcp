# AI SDLC workflow reference

## Lifecycle labels

| Label | Added by | Meaning |
| --- | --- | --- |
| `status:new` | Issue form | Waiting for triage |
| `status:triaged` | Triage workflow | Classification and complexity are available |
| `ready-for-building` | Human | Scope is approved for implementation |
| `develop-with-ai` | Human | Copilot may begin remote implementation |
| `ready-for-qa` | Post-review workflow | Linked PR received Copilot review |
| `release-to-production` | Human | QA evidence is accepted; run release simulation |
| `released` | Release workflow | Tests passed and artifact was produced |

No automation adds either approval label: `ready-for-building`, `develop-with-ai`, or `release-to-production`.

Workflow **05 - Promote reviewed work to QA** observes every submitted review. Human reviews complete as an intentional no-op. Promotion requires a bot review identified by GitHub as either reviewer `copilot-pull-request-reviewer[bot]` or actor `Copilot`; this accommodates both event identity shapes without weakening the gate.

Workflow **04 - Start automatic Copilot code review** runs when a non-draft pull request opens or a draft becomes ready. It adds Copilot as reviewer automatically; attendees only observe the request and respond to the eventual review. Manual reviewer selection is a recovery path, not a normal lifecycle step.

## Daily routing model

The scheduled workflow runs at 07:00 UTC and can also be started manually. It considers open issues that are both triaged and ready for building, ranks priority before complexity and age, and selects at most one top issue per lane.

| Work profile | Route label | GitHub assignee | Working surface |
| --- | --- | --- | --- |
| Hardest full-stack | `agent:vscode` | `HUMAN_MAINTAINER` | GitHub Copilot in VS Code with `fullstack` agent |
| Simple frontend | `agent:copilot-app` | Copilot after AI approval | Copilot on GitHub with `frontend` agent |
| Documentation-only | `agent:cloud` | Copilot after AI approval | Copilot cloud agent with `documentation` agent |
| Script/workflow | `agent:copilot-cli` | `HUMAN_MAINTAINER` | Copilot CLI |
| Quick logging/backend | `agent:cloud` | Copilot after AI approval | Copilot cloud agent with `clean-code` agent |
| Ambiguous/risky | `agent:human` | `HUMAN_MAINTAINER` | Human decision |
| Pull request maintainability review | n/a | Human starts `clean-code` agent | VS Code |
| Pull request verification | n/a | Human starts `qa` agent | VS Code plus Copilot code review |

VS Code and Copilot CLI are local tools, not GitHub identities. The workflow assigns their issues to the configured human and records the tool in a label. Copilot on GitHub and Copilot cloud agent currently use the same `copilot-swe-agent[bot]` assignment API. For remote work, delegation selects `frontend`, `fullstack`, `documentation`, or `clean-code` from the issue's area labels. The `clean-code` agent implements bounded backend work and can also perform a human-started maintainability pass. The `qa` agent is intentionally started by a person after implementation so it verifies work without becoming the implementer.

The `clean-code` agent contains original repository-specific guidance inspired by general principles popularized by Robert C. Martin. It does not reproduce text from *Clean Code*.

## Explainable triage

The workshop uses deterministic classification so it works without an extra model API or usage cost:

- product keywords choose area and kind,
- urgent and defect language influence priority,
- cross-stack scope, security/data terms, body size, and acceptance-criteria count influence complexity.

The comment displays the outcome and numeric score. This is intentionally inspectable. In production, teams can replace the classifier with an approved model while retaining schema validation, audit comments, token controls, and human approval.

## Project status mapping

| Issue signal | Project Status |
| --- | --- |
| New issue | Backlog |
| `status:triaged` | Triage |
| `ready-for-building` | Ready |
| `develop-with-ai` | In progress |
| `ready-for-qa` | QA |
| `release-to-production` | QA while release validation runs |
| `released` | Done |

## Security boundaries

- Triage and routing use the repository `GITHUB_TOKEN` with Issues write only.
- Copilot assignment uses a user token because agent APIs do not support the repository token.
- Projects v2 uses a separate project token.
- Review requests use `GITHUB_TOKEN` with Pull requests write.
- Privileged workflows never check out or execute untrusted fork code.
- Release checks out `main`, so the reviewed pull request must be merged before a human adds `release-to-production`. It then runs tests, builds static assets, and uploads an artifact only; unmerged PR code is never included.

## Copilot Automation option

Organizations with Copilot Automations can create a daily scheduled automation from `.github/prompts/daily-issue-router.prompt.md`. Keep the included deterministic Action as the event fallback and audit reference. Do not let both automations mutate labels until you have tested conflict behavior.
