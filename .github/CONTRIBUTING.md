# Contributing to git-cache

Before contributing, please follow the appropriate communication lines
to discuss changes to make sure they fit and/or not being duplicated. And make sure to read the [code of conduct](./CODE_OF_CONDUCT.md).

- [Contributing to git-cache](#contributing-to-git-cache)
  - [Branching Strategy](#branching-strategy)
  - [Branch Naming](#branch-naming)
  - [Pull Requests](#pull-requests)
  - [Code Style](#code-style)


## Branching Strategy

[Git flow](https://nvie.com/posts/a-successful-git-branching-model/)
is used in this project, so please branch off of the
`develop` branch and raise any pull requests to it. Unless you are 
contributing to a release branch.

## Branch Naming

Please use appropriate branch naming schemes:

- Bug fix - `fix/issue#/shortDescription`
- Feature - `feature/feature#/shortDescription`
- Documentation - `documentation/issue#/shortDescription`

## Pull Requests

To keep the noise down in the commit history, please perform an 
interactive rebase to remove commits that do not add anything useful
for knowing what has changed.

Make sure there are no merge conflicts with the destination branch. All 
pull requests are expected to pass all tests, contain new unit tests to 
cover changes and include documentation updates if applicable.

## Code Style

It is expected, any new code will match in style and be commented
appropriately.