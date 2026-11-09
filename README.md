# SteamX — Xbox UWP gaming client starter

A lightweight, source-only starter repository for **SteamX**, an unofficial Steam-inspired gaming client targeting Xbox UWP.

> **Important:** This repository is a source template, not a complete, ready-to-deploy Xbox package. Create a C# **Blank App (Universal Windows)** project in Visual Studio on Windows, then copy the files from `src/` into that project. UWP/Xbox packaging requires Microsoft's Windows tooling and a compatible Windows SDK.

## Current starter features

- Steam Store search using the publicly accessible Steam Store search endpoint.
- Basic game result model (App ID, name, price text and artwork URL).
- Minimal XAML search screen.
- Basic outbound connectivity test.
- No bundled binaries, SDKs, game files, artwork packs, or NuGet caches.

## Repository layout

```text
SteamX/
├── docs/
│   ├── ARCHITECTURE.md
│   ├── BUILD-XBOX.md
│   └── ROADMAP.md
├── src/
│   ├── MainPage.xaml
│   ├── MainPage.xaml.cs
│   ├── Models/Game.cs
│   └── Services/
│       ├── NetworkService.cs
│       └── SteamStoreService.cs
├── .gitignore
├── CONTRIBUTING.md
├── SECURITY.md
└── README.md
```

## Quick start

1. On GitHub, create a repository named `SteamX`.
2. Upload the contents of this template ZIP, or use **Add file → Upload files**.
3. In the repository, open **Settings → General → Template repository** and enable it if you want others (or your future projects) to create repositories from this template.
4. On a Windows development machine, install Visual Studio with **Universal Windows Platform development** and a compatible Windows SDK.
5. Create a new C# **Blank App (Universal Windows)** project named `SteamX`.
6. Copy the files under `src/` into the matching project locations. If your project's default namespace differs, update the `namespace SteamX` declarations and `x:Class="SteamX.MainPage"` to match.
7. In `Package.appxmanifest`, enable **Internet (Client)**.
8. Build and test on the local machine first, then configure Xbox remote deployment.

## Keep your 32 GB Linux drive free

This repo intentionally contains source code only.

- Edit files in GitHub's web editor or `github.dev` without cloning the whole repo.
- If you do clone it, use a shallow clone: `git clone --depth 1 <your-repo-url>`.
- Avoid committing `bin/`, `obj/`, `.vs/`, `.nuget/`, build packages, SDK installers, game assets, or game downloads.
- UWP/Xbox builds still need a compatible Windows environment. A Linux Codespace is useful for editing documentation and portable backend code, but it cannot replace Visual Studio + the Windows UWP/Xbox toolchain.
- GitHub Codespaces uses remote compute/storage and may have usage limits or charges depending on your plan. Check the current GitHub billing page before using it heavily.

## Current API limitations

The Store search service in this starter uses an **undocumented Steam Store endpoint**. It may change, rate-limit, or return different fields. It is not a guaranteed public developer API. The app-details endpoint is intentionally not wired in yet.

Steam Web API features such as private owned-game data, friends, achievements, and account-linked information require their own supported authentication and authorization design. Do not collect Steam passwords in this app. Do not put private API keys or client secrets in a UWP package or public repository.

SteamX is not the Steam desktop client and cannot make arbitrary Windows Steam games execute natively on Xbox UWP. PC game streaming is a separate future feature.

## License

Choose and add a license before publishing or distributing the project. See `docs/ARCHITECTURE.md` for integration boundaries.
