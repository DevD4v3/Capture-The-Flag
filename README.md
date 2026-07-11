<h1 align="center">🎮 Capture the Flag</h1>

<p align="center">
  <strong>A fast-paced C# game mode for open.mp — featuring two teams, two flags, and pure competitive chaos.</strong>
</p>

<p align="center">
  <img src="https://github.com/user-attachments/assets/dd12935e-5897-470b-ab06-a72b492a521c" alt="SA-MP logo" />
</p>

<p align="center">
  <a href="https://github.com/DevD4v3/Capture-The-Flag">
    <img src="https://img.shields.io/badge/Capture%20The%20Flag-SA:MP-red" />
  </a>
  <a href="https://github.com/DevD4v3/Capture-The-Flag">
    <img src="https://img.shields.io/badge/.NET-10.0-blue" />
  </a>
  <a href="https://github.com/DevD4v3/Capture-The-Flag">
    <img src="https://img.shields.io/badge/Framework-SampSharp.net-purple" />
  </a>
  <a href="https://github.com/DevD4v3/Capture-The-Flag">
    <img src="https://img.shields.io/badge/GameMode-CSharp-yellow" />
  </a>
  <a href="https://github.com/DevD4v3/Capture-The-Flag">
    <img src="https://img.shields.io/badge/Mode-Team%20Deathmatch%20+%20Ranks-green" />
  </a>
  <a href="https://github.com/DevD4v3/Capture-The-Flag">
    <img src="https://img.shields.io/badge/License-AGPL%203.0-orange" />
  </a>
</p>

<p align="center">
  <a href="https://github.com/DevD4v3/Capture-The-Flag">
    <img src="https://github.com/user-attachments/assets/2991265d-4626-4da5-839d-58a7ba2042e7" />
  </a>
</p>

**Capture the Flag** is a game mode for [open.mp](https://github.com/openmultiplayer) (Open Multiplayer, a multiplayer mod for GTA San Andreas) created with the [SampSharp](https://github.com/ikkentim/SampSharp) framework.

There are 2 flags on the map, one for each team. Players need to capture the enemy's flag and bring it back to their own base.

## Index
- [Gameplay](#gameplay)
- [Screenshots](#screenshots)
- [Technologies used](#technologies-used)
  - [Programming Languages](#programming-languages)
  - [Softwares](#softwares)
  - [Frameworks and libraries](#frameworks-and-libraries)
  - [Testing](#testing)
- [Software Engineering](#software-engineering)
  - [Programming Paradigms](#programming-paradigms)
  - [Software Patterns](#software-patterns)
  - [Design Principles](#design-principles)
- [Requirements to play](#requirements-to-play)
- [Deployment without Docker](#deployment-without-docker)
- [Deployment with Docker](#deployment-with-docker)
- [Credentials](#credentials)
- [How to become an admin?](#how-to-become-an-admin)
- [Supported RDBMS](#supported-rdbms)
  - [SQLite](#sqlite)
  - [MariaDB](#mariadb)
- [Architectural overview](#architectural-overview)
- [Credits](#credits)
  - [Mappers](#mappers)
- [Contribution](#contribution)
- [License](#license)

## Gameplay

The Beta team plays against the Alpha team. The goal is to steal the enemy team’s flag and bring it back to the spawn of your own flag.

To score, your own flag must be at its base, so teams must attack and defend at the same time. Team coordination and tactical play are essential to win.

The match lasts 15 minutes. The team with the most captures when time runs out wins.  
If both teams have the same number of captures, the match ends in a draw.

Beware! Enemies can see flag carriers on their radar.

In this video, you can watch a gameplay demo: https://youtu.be/rsWCZaT4aBE  
You can also check the full playlist: https://www.youtube.com/playlist?list=PLBM-9TMXSAJjsWn4zmg1ua7eof9Aj83fS

### Gameplay Rules

#### Game Objective
- Steal the enemy team’s flag and bring it back to your base to score a capture.
- The match does not end when a team reaches a specific number of captures.
- The match only ends when the timer runs out.

#### Flag Rules

**Stealing the enemy flag**
- A player can steal the enemy flag by picking up the flag pickup at the enemy base, marked by a square icon on the map.
- A player can carry only one flag at a time.

**Capturing (scoring a capture)**
- To score a capture, all of the following conditions must be met:
  - The player is carrying the enemy flag
  - The player reaches their own base or capture zone
  - The player’s team flag is at its base position
- If the team’s flag has been stolen or is dropped somewhere on the map, a capture cannot be scored.

**Flag carrier death**
- If a player dies while carrying the enemy flag:
  - The flag is dropped on the ground at the player’s current position
  - The player stops being the flag carrier

**Flag carrier disconnection**
- If a player disconnects while carrying the enemy flag:
  - The flag is dropped on the ground at the player’s last known position
  - The player is removed from the match

**Flag carrier inactivity (pause)**
- If a flag carrier remains paused or inactive for 30 seconds:
  - the enemy flag is automatically returned to its base
  - the player stops being the flag carrier

**Flag auto-return**
- If a flag remains dropped on the map and is not recovered:
  - It automatically returns to its base after 120 seconds

#### Death and respawn
- When a player dies:
  - Players respawn at their team’s base
  - A random spawn position is selected for the player (spawn location is not fixed)
  - No additional spawn protection is applied
  - If they were carrying a flag, the flag drop rule applies

#### Match end conditions
- The match ends only when the timer reaches zero.
- The team with the highest number of captures wins.
- If both teams have the same number of captures:
  - The match ends in a draw
  - There is no sudden death

#### Round Transition Rules

- Every 15 minutes, the current map ends and a new map is loaded.
- During map rotation:
  - All connected players are frozen
  - Players are switched to spectator mode
- A 10-second "loading map" countdown is displayed before the new round starts.
- Teams are rebalanced based on player performance from the previous round:
  - Players are reassigned to teams according to their scores
  - Team balance is calculated automatically by the system
- Players are not sent to the class selection screen:
  - Respawn is automatic
  - The system decides the spawn and team assignment


## Screenshots

<details>
<summary>sa-mp-000</summary>

![sa-mp-000](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-000.png)
</details>

<details>
<summary>sa-mp-001</summary>

![sa-mp-001](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-001.png)
</details>

<details>
<summary>sa-mp-002</summary>

![sa-mp-002](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-002.png)
</details>

<details>
<summary>sa-mp-003</summary>

![sa-mp-003](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-003.png)
</details>

<details>
<summary>sa-mp-004</summary>

![sa-mp-004](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-004.png)
</details>

<details>
<summary>sa-mp-005</summary>

![sa-mp-005](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-005.png)
</details>

<details>
<summary>sa-mp-006</summary>

![sa-mp-006](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-006.png)
</details>

<details>
<summary>sa-mp-007</summary>

![sa-mp-007](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-007.png)
</details>

<details>
<summary>sa-mp-008</summary>

![sa-mp-008](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-008.png)
</details>

<details>
<summary>sa-mp-009</summary>

![sa-mp-009](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-009.png)
</details>

<details>
<summary>sa-mp-010</summary>

![sa-mp-010](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/screenshots/sa-mp-010.png)
</details>

## Technologies used

### Programming Languages
- [C Sharp](https://github.com/dotnet/csharplang)
- [Pawn](https://github.com/compuphase/pawn)

### Softwares
- [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools)
- [Open Multiplayer](https://github.com/openmultiplayer)
- [CompileApp-FS](https://github.com/DevD4v3/CompileApp-FS)
- [Visual Studio 2022](https://visualstudio.microsoft.com)
- [vscode](https://github.com/microsoft/vscode)
- [MariaDB](https://github.com/mariadb)
- [SQLite](https://www.sqlite.org)
- [DB Browser for SQLite](https://sqlitebrowser.org)
- [HeidiSQL](https://github.com/HeidiSQL)
- [GitHub Actions](https://github.com/actions)
- [Git](https://github.com/git/git)
- [draw.io](https://app.diagrams.net)
- [Docker](https://github.com/docker)
- [Portainer](https://github.com/portainer/portainer)

### Frameworks and libraries
- [.NET SDK 10.0](https://github.com/dotnet/runtime)
- [SampSharp](https://github.com/ikkentim/SampSharp)
- [SampSharp.OpenMp.Streamer](https://github.com/OpenSamp/SampSharp.OpenMp.Streamer)
- [omp-streamer-component](https://github.com/OpenSamp/omp-streamer-component)
- [GameMode.Common](https://github.com/DevD4v3/GameMode.Common)
- [DotEnv.Core](https://github.com/DevD4v3/dotenv.core)
- [YeSql.Net](https://github.com/ose-net/yesql.net)
- [seztion-parser](https://github.com/DevD4v3/seztion-parser)
- [SmartFormat](https://github.com/axuno/SmartFormat)
- [MySqlConnector](https://github.com/mysql-net/MySqlConnector)
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.SQLite)
- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection)
- [Microsoft.Extensions.Configuration.Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder)
- [Microsoft.Extensions.Configuration.EnvironmentVariables](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.EnvironmentVariables)
- [BCrypt.Net-Next](https://github.com/BcryptNet/bcrypt.net)
- [Serilog.Sinks.Console](https://github.com/serilog/serilog-sinks-console)
- [Serilog.Sinks.File](https://github.com/serilog/serilog-sinks-file)
- [Serilog.Extensions.Logging](https://github.com/serilog/serilog-extensions-logging)

### Testing
- [NUnit](https://github.com/nunit/nunit)
- [Fluent Assertions](https://github.com/fluentassertions/fluentassertions)

## Software Engineering

These concepts have been applied to this project:

### Programming Paradigms
- [Object-oriented programming (OOP)](https://en.wikipedia.org/wiki/Object-oriented_programming)
- [Structured programming](https://en.wikipedia.org/wiki/Structured_programming)

### Software Patterns
- [Hexagonal architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software))
- [Entity–component–system (ECS)](https://en.wikipedia.org/wiki/Entity_component_system)
- [Interface-based programming](https://en.wikipedia.org/wiki/Interface-based_programming)
- [Dependency injection](https://en.wikipedia.org/wiki/Dependency_injection)
- [Repository Pattern](https://deviq.com/design-patterns/repository-pattern)
- [Operation Result Pattern](https://medium.com/@wgyxxbf/result-pattern-a01729f42f8c)

### Design Principles
- [Separation of concerns](https://en.wikipedia.org/wiki/Separation_of_concerns)
- [Open-Closed Principle](https://en.wikipedia.org/wiki/Open%E2%80%93closed_principle)
- [Dependency Inversion Principle](https://deviq.com/principles/dependency-inversion-principle)
- [Explicit dependencies](https://deviq.com/principles/explicit-dependencies-principle)

## Requirements to play

- You must have **DirectX 9** installed on your local machine.
- You must download [Grand Theft Auto: San Andreas](https://www.mediafire.com/file/jhpfq99p28my35m/Gta_San_Andreas.zip/file) on your local machine.
- You must download [open.mp launcher](https://github.com/openmultiplayer/launcher/releases/latest) to connect to the servers.

## Deployment without Docker

* Download the appropriate release package for your operating system:
  * **Windows (x64):** [capture-the-flag-win-x64.zip](https://github.com/DevD4v3/Capture-The-Flag/releases)
  * **Linux (x86_64):** [capture-the-flag-linux-x86_64.tar.xz](https://github.com/DevD4v3/Capture-The-Flag/releases)
* Extract the package to a directory of your choice.
* Modify the `.env` file according to your needs.
* Start the Open Multiplayer server:
  * **Windows:** `omp-server.exe`
  * **Linux:** `./omp-server`


## Deployment with Docker

- Clone the repository:
```sh
git clone --recursive https://github.com/DevD4v3/Capture-The-Flag.git
```
- Change directory:
```sh
cd Capture-The-Flag
```
- Copy the contents of `.env.example` to `.env`:
```sh
cp .env.example .env
```
- Build the image and initiate services:
```sh
docker compose up --build -d
```
- Check the server logs to see if everything is working properly:
```sh
docker compose exec -it app cat log.txt
```
- Add the server IP in your [omp-launcher](https://github.com/openmultiplayer/launcher/releases/latest):
```
localhost:7777
```

## Credentials

The following table shows the default credentials for authentication from the game mode.

| PlayerName              | Password                    |
|-------------------------|-----------------------------|
| Admin_Player            | 123456                      |
| Moderator_Player        | 123456                      |
| VIP_Player              | 123456                      |
| Basic_Player            | 123456                      |

Note that these credentials are only available if your database provider is **in-memory**. In your .env file you must indicate it as follows.
```sh
DatabaseProvider=InMemory
```

## How to become an admin?

You must add your name and secret key from the `.env` file:
```sh
# Your nickname in the game.
ServerOwner__Name=MrDave
# Specify the secret key to give me admin.
ServerOwner__SecretKey=1234._%==?!
```
It is necessary to specify your secret key, which you will use when executing the command "**/givemeadmin**" in the game.

## Supported RDBMS

### SQLite

- Create a file called `.env` in the root directory:
```sh
copy .env.example .env
```
- You must specify the name of the database provider from the .env file:
```sh
DatabaseProvider=SQLite
```
- You must specify the location of the database file:
```sh
SQLite__DataSource=C:\Users\mrdave\OneDrive\Desktop\gamemode.db
```
- The database and its schema will be created automatically during server startup if they do not already exist.

### MariaDB

- Install [MariaDb Server](https://mariadb.org/download) and set up your username and password.
- Create a file called `.env` in the root directory:
```sh
copy .env.example .env
```
- You must specify the name of the database provider from the .env file:
```sh
DatabaseProvider=MariaDB
```
- You must specify the connection string in the .env file:
```sh
MariaDB__Server=localhost
MariaDB__Port=3306
MariaDB__Database=gamemode
MariaDB__UserName=root
MariaDB__Password=123456789
```
- The database and its schema will be created automatically during server startup if they do not already exist.

## Architectural overview

<details>
<summary><b>Show diagram</b></summary>

![overview](https://github.com/DevD4v3/Capture-The-Flag/blob/main/docs/architectural-overview.png)

</details>

### Main components
- **Application Core.** Contains all gameplay logic for the Capture the Flag game mode, including the rules and procedures that define how the game is played. It also defines the outbound ports required to interact with external systems.
- **Persistence Adapters.** Provide concrete implementations of the outbound ports related to persistence. Each adapter encapsulates the data access logic for a specific storage technology, allowing the application core to remain independent of any specific persistence technology.
- **Host Application.** Acts as the application's entry point and contains everything required to bootstrap the game mode.
  - Load application settings from `.env` file.
  - Select the database provider.
  - Register services in the DI container.
  - Register ECS systems.
  - Enable desired ECS system features.

## Credits
This project would not have been possible without the following people and projects:
- [DevD4v3](https://github.com/DevD4v3/Capture-The-Flag) — Creator of the original **Capture the Flag** game mode.
- [Parca_35](https://www.linkedin.com/in/manuel-zurita-057a7a39b) — Helped test the game mode.
- [ikkentim](https://github.com/ikkentim/SampSharp) — Creator of the **SampSharp** framework.
- [Nickk888SAMP](https://github.com/Nickk888SAMP/TextDraw-Editor) — Creator of **NTD (TextDraw Editor)**.
- [samp-incognito](https://github.com/samp-incognito/samp-streamer-plugin) — Creator of the **Streamer Plugin**.
- [Open Multiplayer](https://github.com/openmultiplayer) — Creator of **open.mp**, a multiplayer modification for Grand Theft Auto: San Andreas that is fully compatible with San Andreas Multiplayer (SA-MP).
- [OpenSamp](https://github.com/OpenSamp)
  - Maintainer of **omp-streamer-component**, the x64 port of Incognito's Streamer Plugin for open.mp.
  - Creator of **SampSharp.OpenMp.Streamer**, providing managed (.NET) bindings for the Streamer Plugin on the SampSharp open.mp x64 host.

### Mappers

- Area66 by DragonZafiro.
- d_dust5, SA_Hill, de_aztec and de_dust2_small by Elorreli.
- Compound and cs_rockwar by Amirab. 
- DesertGlory, fy_iceworld2 and de_dust2x3 by TheYoungCapone.
- EntryMap and TheConstruction by B4MB1[MC].
- fy_snow by UnuAlex.
- fy_snow2 by mihaibr.
- de_dust2 by JamesT85.
- Aim_Headshot by haubitze.
- Aim_Headshot2 by Niktia_Ruchkov.
- de_dust2x1 by SpikY_.
- de_dust2x2 by Amads.
- de_dust2x4 textured by excamunicado.
- WarZone by Samarchai.
- WarZone2 by iMaster.
- cs_assault by Ghost-X.
- GateToHell and TheWild by Zniper.
- TheBunker by Dr.Pawno.
- cs_deagle5 by SENiOR.
- mp_jetdoor by saawan.
- Simpson by Risq.
- ZM_Italy - Unknown.
- zone_paintball by Famous.
- mp_island by Leo.
- Baron's Playground (RC Battlefield V2) by Pyraeus.
- cs_train, cs_opposition, fy_iceworld and de_dust2x5 by denis_32.

## Contribution

Any contribution is welcome! Remember that you can contribute not only in the code, but also in the documentation or even improve the tests.

Follow the steps below:

- Fork it
- Create your custom branch (git checkout -b my-new-change)
- Commit your changes (git commit -am 'Add some change')
- Push to the branch (git push origin my-new-change)
- Create new Pull Request

## License

This project is licensed under the [GNU Affero General Public License v3.0](https://github.com/DevD4v3/Capture-The-Flag/blob/main/LICENSE)
