# Audience prerequisites email

**Subject:** Action required before iCSU MiniHack: GitHub Copilot AI SDLC

Hello,

You are joining the **iCSU MiniHack: Human-governed AI SDLC**. In 90 minutes you will take a hotel booking requirement from a GitHub Issue through AI-assisted development, Copilot code review, QA approval, and a simulated release.

Please complete every item below **before the event**. We will start with an empty GitHub repository and will not spend hack time installing software.

## Accounts and access

- Confirm you can sign in to the GitHub account you will use at the event.
- Confirm that account has an active GitHub Copilot entitlement.
- Confirm you can create a repository and a GitHub Project in `<EVENT_ORGANIZATION>`.
- Open `<WORKSHOP_REPOSITORY_URL>` and confirm you can read it.
- Confirm **Copilot cloud agent** and **Copilot code review** are enabled for your account/organization. If either feature is unavailable, tell `<SUPPORT_CONTACT>` before the event.
- Enable two-factor authentication if your organization requires it.

## Install and sign in

1. Install [Git](https://git-scm.com/downloads).
2. Install [Visual Studio Code](https://code.visualstudio.com/).
3. Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
4. Install [Node.js 22 LTS](https://nodejs.org/).
5. Install [GitHub CLI](https://cli.github.com/).
6. In VS Code, install:
   - GitHub Copilot
   - GitHub Copilot Chat
   - GitHub Actions
   - C# Dev Kit
7. Sign in to GitHub and GitHub Copilot inside VS Code.
8. In a terminal, run `gh auth login` and select GitHub.com, HTTPS, and browser authentication.
9. Configure the mandatory approved NPM package feed:

   ```powershell
   npm config set registry https://packagefeedproxy.microsoft.io/npm/
   ```

## Five-minute machine check

Run:

```powershell
git --version
gh auth status
dotnet --version
node --version
npm --version
npm config get registry
code --version
```

Expected minimums:

- .NET SDK: `8.x` or newer
- Node.js: `22.x`
- GitHub CLI: authenticated to the account you will use
- NPM registry: exactly `https://packagefeedproxy.microsoft.io/npm/`

Then open VS Code Copilot Chat and ask:

```text
Reply with the current workspace name and do not change any files.
```

If Copilot answers, your editor connection is ready.

## Mandatory NPM security control

Every attendee must use `https://packagefeedproxy.microsoft.io/npm/` as the upstream NPM feed. The repository also commits this setting in `.npmrc`, but the user-level command above is required so supporting tools use the same approved feed.

Do not bypass the NPM Minimum Release control to obtain a newly published package. For an urgent business need, follow the approved security exception process.

## Event details

- Date: `<EVENT_DATE>`
- Start time: `<EVENT_TIME>`
- Location or meeting link: `<EVENT_LOCATION>`
- Workshop repository: `<WORKSHOP_REPOSITORY_URL>`
- Support contact: `<SUPPORT_CONTACT>`

Bring your laptop and charger. Do not email access tokens or place them in source files. We will create any short-lived repository secrets during the guided setup.

Thank you,
`<FACILITATOR_NAME>`
