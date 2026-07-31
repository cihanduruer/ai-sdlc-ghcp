# GitHub setup

Complete this once per workshop repository. It is intentionally separate from local application setup.

## 1. Enable repository capabilities

In repository **Settings**:

1. Enable GitHub Actions.
2. Enable Issues.
3. Confirm Copilot cloud agent access for the repository.
4. Confirm the organization policy allows Copilot code review.
5. Optional but recommended: create a branch ruleset for `main` requiring the `validate` check.
6. Optional production-style review: enable **Automatically request Copilot code review** in a branch ruleset. The included workflow also requests Copilot when a non-draft pull request opens.

Copilot code review submits comments; it does not approve a pull request and does not replace required human approval.

![Repository settings with Issues, Rules, Actions, and Copilot highlighted](images/setup/01-repository-capabilities.png)

The red rectangles identify the repository areas used to enable Issues, configure the `main` ruleset, verify Actions, and review Copilot access.

## 2. Create repository labels

Open **Actions > 00 - Set up repository labels > Run workflow**.

![Set up repository labels workflow with Run workflow highlighted](images/setup/02-create-labels.png)

Select **Run workflow** in the highlighted control and run it from `main`.

Expected evidence: the workflow succeeds and Issues shows labels from `.github/labels.json`, including `status:triaged`, `ready-for-building`, `develop-with-ai`, and `ready-for-qa`.

## 3. Create the GitHub Project

Create a Projects v2 project with a Board view. Keep the field name `Status` and use these exact options:

| Order | Status |
| --- | --- |
| 1 | Backlog |
| 2 | Triage |
| 3 | Ready |
| 4 | In progress |
| 5 | QA |
| 6 | Done |

Add optional views grouped by `Area`, `Complexity`, or `Agent` labels. Labels remain on the Issue; Project Status is synchronized by workflow.

![GitHub Projects page with New project highlighted](images/setup/03-create-project.png)

Use the highlighted **New project** button, then choose a Board layout.

## 4. Configure repository variables

Open **Settings > Secrets and variables > Actions > Variables**.

| Variable | Example | Purpose |
| --- | --- | --- |
| `HUMAN_MAINTAINER` | `octocat` | Assignee for VS Code, CLI, or human lanes |
| `PROJECT_OWNER` | `contoso` | User or organization that owns the Project |
| `PROJECT_OWNER_TYPE` | `organization` | `organization` or `user` |
| `PROJECT_NUMBER` | `5` | Number visible in the Project URL |

![Actions variables page with New repository variable highlighted](images/setup/04-repository-variables.png)

Use **New repository variable** once for each row in the table.

## 5. Configure the Copilot assignment token

The workflow `GITHUB_TOKEN` cannot use the Copilot cloud-agent assignment API. Create a short-lived **fine-grained personal access token** owned by a user who:

- has repository write permission,
- has Copilot cloud-agent access, and
- is allowed by organization policy to delegate work.

Grant repository permissions:

- Metadata: read
- Actions: read and write
- Contents: read and write
- Issues: read and write
- Pull requests: read and write

Store it as the repository Actions secret `COPILOT_AGENT_TOKEN`. Never place the token in an issue, prompt, workflow file, or commit. Delete or rotate it after the event.

![Actions secrets page with the Copilot agent secret control highlighted](images/setup/05-copilot-secret.png)

Select **New repository secret** and use the exact name `COPILOT_AGENT_TOKEN`.

The cloud-agent assignment API is a preview capability and may be controlled by enterprise policy. If unavailable, manually assign Copilot from the Issue **Assignees** control.

## 6. Configure the Project token

Projects v2 is outside the repository `GITHUB_TOKEN` boundary. Create a separate fine-grained token with:

- Organization Projects: read and write, or User Projects: read and write
- Repository Issues: read

Store it as `PROJECT_TOKEN`. For a production system, prefer a GitHub App installation token and rotate credentials according to organization policy.

![Actions secrets page with the Project token secret control highlighted](images/setup/06-project-secret.png)

Select **New repository secret** and use the exact name `PROJECT_TOKEN`.

## 7. Smoke test the lifecycle

1. Open a Requirement issue.
2. Wait for **01 - Triage issue**.
3. Confirm the issue receives area, kind, priority, complexity, and `status:triaged`.
4. Add `ready-for-building`.
5. Run **02 - Route top ready issues** manually.
6. Confirm the route comment and `agent:*` label.
7. Do not add `develop-with-ai` until the workshop implementation phase.

![New issue page with Product requirement highlighted](images/setup/07-smoke-test-issue.png)

Start the smoke test with the highlighted **Product requirement** form.

If project sync is skipped, recheck `PROJECT_*` variables, the token scope, and the exact Status option names.

Official references:

- [Use Copilot cloud agent via the API](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/cloud-agent/use-cloud-agent-via-the-api)
- [Configure automatic Copilot review](https://docs.github.com/en/copilot/how-tos/copilot-on-github/set-up-copilot/configure-automatic-review)
- [Automate Projects with Actions](https://docs.github.com/en/issues/planning-and-tracking-with-projects/automating-your-project/automating-projects-using-actions)

## Microsoft NPM feed troubleshooting

The approved feed is mandatory for every attendee. Run:

```powershell
npm config set registry https://packagefeedproxy.microsoft.io/npm/
npm config get registry
```

The second command must return `https://packagefeedproxy.microsoft.io/npm/`. The committed `.npmrc` enforces the same setting inside the repository. Do not bypass NPM Minimum Release controls; use the approved security exception process for an urgent unavailable package version.
