# Contributing to Factory Frenzy — VR Level Editor

Thank you for contributing to the **Factory Frenzy VR Level Editor**. This Unity application lets players build levels in virtual reality and export them as JSON files for the PC multiplayer game.

## Getting started

1. Clone the VR Editor repository.
2. Open the project with the Unity version specified in the repository's `README.md` or Unity `ProjectSettings/ProjectVersion.txt`.
3. Install any required packages through Unity Package Manager.
4. Open the editor scene and verify that it runs before making changes.
5. Use a compatible VR headset and controllers when testing XR interactions.

Do not commit local machine settings, generated Unity caches, or secrets.

## Branches and commits

- Keep `main` stable and ready for demonstration.
- Create a dedicated branch for each change: `feature/vr-object-snap`, `fix/vr-menu-raycast`, or `docs/vr-controls`.
- Make small, focused commits with descriptive messages, such as `feat(vr): add object locking` or `fix(vr): prevent grabbing locked objects`.
- Open a pull request before merging into `main` and request review from another team member when possible.
- Resolve merge conflicts and test the final changes before merging.

## Scope of contributions

The VR Editor includes these features from the project specifications:

- VR avatar teleportation (`VR-FP1-1`).
- Selecting and creating level elements through a UI (`VR-FP1-2`).
- Grabbing, placing, and repositioning objects (`VR-FP1-3`).
- Deleting placed elements (`VR-FP1-4`).
- Exporting a named level as JSON (`VR-FP1-5`).
- Audio and haptic feedback (`VR-FP1-6`).
- Optional advanced features: snapping and rotation steps (`VR-FP2-1`), object locking (`VR-FP2-2`), and moving-platform configuration (`VR-FP2-3`).

## Code and Unity conventions

- Use clear C# names: `PascalCase` for classes, methods, and public members; `camelCase` for local variables and private fields.
- Keep fields private and use `[SerializeField]` when they must be configured in the Inspector.
- Separate UI, XR interaction, object manipulation, and JSON serialization responsibilities where practical.
- Prefer reusable prefabs over duplicated scene objects.
- Avoid hard-coded scene references and unnecessary work in `Update()`.
- Preserve Unity `.meta` files and their GUIDs. Commit asset changes together with their corresponding `.meta` files.
- Do not commit `Library/`, `Temp/`, `Obj/`, `Build/`, `Builds/`, or `Logs/`.
- Coordinate edits to shared scenes, prefabs, and project settings to reduce merge conflicts.

## Shared level JSON contract

The VR Editor produces files consumed by the PC multiplayer project. Treat the JSON structure as a shared interface.

- Agree on element identifiers, position and rotation formats, moving-platform parameters, and other required fields with the PC team.
- Keep serialization models compatible between both repositories.
- Document any schema change and provide a sample exported level for testing.
- Validate that an exported level can be imported and played in the PC application before merging a breaking format change.
- Handle existing filenames and overwrite confirmation in the export UI.

## VR usability and feedback

- Keep controller mappings consistent throughout the application.
- Ensure text and buttons remain legible in the headset.
- Make object selection, placement, deletion, and menu navigation understandable and efficient.
- Use sound or haptic feedback intentionally; avoid excessive effects.
- Test both the tutorial/start menu and the active editing workflow.

## Testing checklist

Before opening a pull request, confirm the relevant items:

- [ ] The project opens without new Console errors.
- [ ] VR controllers and UI interactions work as intended.
- [ ] Object creation, movement, and deletion do not break existing features.
- [ ] Locked objects and snap behavior work where applicable.
- [ ] JSON export produces a valid file and respects the shared format.
- [ ] Any changed exported level imports correctly in the PC project.
- [ ] Modified scenes and prefabs are saved, and required `.meta` files are committed.
- [ ] The change has been tested in the headset when XR behavior is affected.

## Pull request template

Include the following in each pull request:

- **Summary:** What changed and why?
- **Related feature:** Specification ID or Trello card.
- **Testing:** What was tested, and on which hardware?
- **Compatibility:** Does this change affect JSON exports or the PC project?
- **Evidence:** Screenshots or a short video when UI or VR interaction changes.

## Team collaboration

Track tasks and progress in the team's Trello board. Assign an owner to each feature, flag blockers early, and keep Git history understandable so the team can present its contributions during the final evaluation.
