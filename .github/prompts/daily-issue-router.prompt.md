---
description: Review ready issues and recommend the best Copilot or human execution lane.
---

Review open issues labeled `status:triaged` and `ready-for-building`. Rank priority first, then complexity, then age. Select at most one top issue for each lane:

- High-complexity full-stack: `agent:vscode` and a human maintainer using GitHub Copilot in VS Code.
- Low-complexity frontend: `agent:copilot-app` using the frontend custom agent.
- Script or workflow: `agent:copilot-cli` and a human maintainer using Copilot CLI.
- Logging or low-complexity backend fix: `agent:cloud` using Copilot cloud agent.
- Ambiguous or risky work: `agent:human`.

Explain the choice in an issue comment. Never begin Copilot implementation until a human adds `develop-with-ai`. Do not route issues already labeled `ready-for-qa` or `released`. Copilot code review is required for every generated pull request.
