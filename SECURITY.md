# Security Policy — Factory Frenzy VR Level Editor

## Scope

This policy covers the **Factory Frenzy VR Level Editor** Unity project, including its XR interactions, editor UI, level objects, and JSON level export. The editor produces files intended to be imported by the separate PC multiplayer project.

This is a student project. No formal security-support period or public vulnerability-response service has been established.

## Reporting a vulnerability

**Do not publish exploitable details in a public GitHub issue.** Report suspected vulnerabilities privately to the repository maintainers through GitHub's **Report a vulnerability** feature, if enabled, or a private channel agreed upon by the project team.

Include:

- A clear description of the issue and its impact.
- Steps to reproduce it and the affected Unity scene, script, or component.
- Relevant Unity version, XR device, and package versions.
- A minimal proof of concept, logs, or screenshots with sensitive data removed.

Maintainers should acknowledge reports when available, assess impact, and coordinate a fix before public disclosure. **No guaranteed response or remediation time is promised.**

## Security practices

### Level data and JSON export

- Treat level names and filenames as untrusted input; reject path traversal and invalid filename characters.
- Export only to an intended directory. Do not allow user-supplied filenames to select arbitrary paths.
- Warn before overwriting an existing level file.
- Serialize only the fields needed to reconstruct a level; never include credentials, machine-specific paths, or personal information.
- Validate object types, transforms, and configurable values before exporting; reject non-finite numbers and unreasonable values.
- Coordinate changes to the JSON schema with the PC project, which must independently validate imported files.

### Unity and XR

- Keep Unity and project dependencies on supported, team-approved versions; assess security updates before upgrading.
- Use trusted sources for assets and packages, and review third-party scripts before importing them.
- Avoid executing external commands or loading arbitrary code from exported level files.
- Validate object deletion, locking, and placement actions to prevent unintended editor-state changes.
- Avoid logging personal information or sensitive local file paths.

### Repository hygiene

- Never commit API keys, passwords, access tokens, private certificates, or private configuration files.
- Keep Unity-generated folders such as `Library/`, `Temp/`, and `Logs/` out of Git.
- Review pull requests, especially changes to dependencies, serialization, file I/O, and project settings.
- If a secret is committed, revoke or rotate it immediately; deleting it from the latest commit is not sufficient.

## Before release or demonstration

- [ ] Confirm that JSON exports stay within the designated directory.
- [ ] Test invalid level names, overwrites, and malformed or extreme object values.
- [ ] Confirm that exported JSON contains no secrets or personal data.
- [ ] Review dependency and asset changes.
- [ ] Verify the editor works with the agreed PC JSON format.

## Related documentation

See [`CONTRIBUTING.md`](CONTRIBUTING.md) for development and review conventions.
