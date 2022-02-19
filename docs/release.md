# Releasing updates
1. Update version numbers
    - Follow [semantic versioning](https://semver.org/) guidelines (`MAJOR.MINOR.PATCH`)
        - Breaking changes incur a major version increment
        - Added functionality (non-breaking) incurs a minor version increment
        - Fixing bugs (non-breaking) incurs a patch version increment
        - Optionally include a pre-release suffix (e.g., `1.4.2-alpha`)
    - Version locations (as of 2021-04-20):
        - _package.json_
        - _Runtime/BehaviorTree.csproj_
        - _README.md_
            - Only update for non-pre-release versions
1. [Create a new release](https://github.com/asasine/BehaviorTree.NET/releases/new)
    - Tag version: `vMAJOR.MINOR.PATCH` targeting `main` (whatever version was decided above, with `v` prefix)
    - Title: `vMAJOR.MINOR.PATCH`
    - Description: use the `Auto-generate release notes` button. 
        - Add a single H1: `vMAJOR.MINOR.PATCH (YYYY-MM-DD)`
        - Can have a small paragraph after the H1 (e.g., a summary, link to a blog post, etc.)
        - Can have categorized H2 (omit an H2 if it would be empty), or a general `What's Changed` H2. Examples of categories:
            - New Features
            - Bug Fixes
            - Internal
        - Use an unordered list under each H2
            - Each list item should have the format `{PR or Issue title} by @{user} {link}`
            - `{PR or Issue title}` is the title from the PR or Issue that was merged or closed.
            - `{user}` is the GitHub username of the person who made the change.
            - `{link}` is the URL of the PR or Issue.
    - Check `This is a pre-release` if `version < 1.0.0` or there's a pre-release suffix on the version (e.g., `0.1.1` or `1.4.2-alpha`)
