# Factory Frenzy — VR Level Editor

## Overview

Factory Frenzy is a Unity proof-of-concept inspired by obstacle-course games such as *Fall Guys*. It consists of two separate applications: a **VR level editor** and a **PC multiplayer racing game**.

This repository contains the **VR Level Editor**. Players build obstacle courses in virtual reality, then export their levels as JSON files for use by the PC application.

> Project status: Development / academic proof of concept. Features below are project requirements and may not all be implemented yet.

## Technology

- Unity **2022.3.8** (version used by the provided starter project)
- C#
- Unity XR Interaction Toolkit
- Compatible VR headset and controllers (depending on available hardware)
- JSON level serialization

## Planned Features

### Priority 1

| ID | Feature |
| --- | --- |
| VR-FP1-1 | VR avatar teleportation |
| VR-FP1-2 | Select and create level elements through a VR UI |
| VR-FP1-3 | Grab, move, rotate and place objects |
| VR-FP1-4 | Delete level elements |
| VR-FP1-5 | Export the level to a JSON file |
| VR-FP1-6 | Audio and haptic feedback |

### Priority 2

| ID | Feature |
| --- | --- |
| VR-FP2-1 | Smart placement, snapping and incremental rotation |
| VR-FP2-2 | Lock and unlock placed objects |
| VR-FP2-3 | Configure moving platforms (start/end positions and travel time) |

### Available Level Elements

- Start and finish platforms
- Standard platforms
- Moving platforms
- Bumpers
- Launchers
- Trampolines
- Fans
- Checkpoints

## Getting Started

1. Install **Unity Hub** and **Unity 2022.3.8**.
2. Clone this repository.
3. Open the project folder in Unity Hub.
4. Import or configure any required XR packages and device settings if they are not already included.
5. Connect a compatible VR headset and open the editor scene.
6. Enter Play Mode or build the application for the configured VR platform.

> The exact scene name, headset configuration and build target should be documented once finalized.

## Level Export and Integration

The editor is designed to export a level as a `LevelName.json` file. The JSON representation is shared with the [Factory Frenzy PC Multiplayer](../) application conceptually; replace this placeholder with the actual PC repository URL.

The shared data model should represent object types, transforms and any object-specific configuration (such as moving-platform parameters). The exact JSON schema is defined jointly by both application teams.

## Development Workflow

- Track features and bugs in the team Trello board.
- Develop changes in dedicated Git branches and review them before merging.
- Keep Unity-generated folders (such as `Library/`, `Temp/`, `Obj/` and `Builds/`) out of version control.
- Commit Unity `.meta` files alongside their corresponding assets.

## Academic Context

Developed as part of the **Factory Frenzy** final Unity project at **CESI**. The project brief specifies a ten-day proof of concept and a separate PC multiplayer application connected through JSON level files.

## Credits

Project assets and initial scene are provided through the course's `Factory-Frenzy-2.0.0-Platform-Trap.zip` starter package. Additional third-party assets remain subject to their respective licenses.
