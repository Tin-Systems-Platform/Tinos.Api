# Tinos.Api

This document describes the rules and guidelines that coding agents must follow when developing Tinos.Api.

Agents must follow these instructions when inspecting, modifying, or extending the codebase.

## Getting Started

Any common code supported by both VB.NET and C# should be placed in the `Tinos.Api.Common` project.

This project is a .NET 10 library that can be referenced by both VB.NET and C# projects.

The `Tinos.Api.Common` project should contain the core functionality of the API, including:

- Shared data structures and types.
- Utility functions and helpers.
- Common implementations.
- Language-independent abstractions.
- Functionality that is not specific to a particular programming language.

Keep the project structure consistent with the existing codebase.

## Supported Languages

Tinos.Api supports both VB.NET and C#.

Agents may implement code in either language, depending on the requirements of the task.

The `Tinos.Api.Common` project is designed to be language-agnostic. Its public APIs must remain accessible from both VB.NET and C#.

Avoid introducing language-specific features or dependencies into shared code when they would prevent compatibility with either language.

## No Rewriting of Existing Code

**Agents must not rewrite existing code in the `Tinos.Api.Common` project.**

Instead, agents should focus on adding new functionality and improving the existing codebase without unnecessarily replacing existing implementations.

Before implementing new functionality, agents must inspect the existing codebase for reusable implementations.

Do not create duplicate classes, methods, utilities, or abstractions when equivalent functionality already exists.

Any changes to existing code must be made with caution, as they may affect other parts of the project and its consumers.

Agents should coordinate their changes where possible and report potential conflicts or overlapping implementations.

**DO NOT TRY TO CIRCUMVENT THIS RULE.**

If there is a valid reason to rewrite or substantially modify existing code:

1. Explain why the change is necessary.
2. Identify the existing code that would be affected.
3. Explain the expected benefits and potential risks.
4. Obtain approval from the developer before making the changes.

Do not proceed with an unapproved rewrite.

## Documentation

All code in the `Tinos.Api.Common` project should be well-documented using XML documentation comments.

Documentation should provide clear explanations of the purpose and functionality of:

- Classes and structures.
- Public methods and constructors.
- Public properties and fields.
- Public interfaces and enumerations.
- Other relevant public API members.

XML documentation should include appropriate parameter descriptions, return values, exceptions, and usage examples where relevant.

Documentation must be kept up-to-date as the code evolves.

Do not leave outdated, misleading, or incomplete XML documentation when modifying existing code.

## Code Quality and Dependencies

Agents should follow the existing coding conventions and project structure.

Before introducing new dependencies, agents must check whether the required functionality is already available through:

- The .NET SDK.
- Cosmos APIs.
- Existing project implementations.
- Existing dependencies.

Do not introduce unnecessary NuGet packages or external dependencies.

New code should be designed with maintainability, compatibility, and resource efficiency in mind.

Avoid unnecessary complexity and keep implementations focused on their intended responsibilities.

## Cosmos OS

Tinos.Api is designed to work with the Cosmos operating system.

Agents should be familiar with Cosmos OS and its APIs, as these APIs are used to provide operating-system functionality.

The `Tinos.Api.Common` project should provide a layer of abstraction over Cosmos APIs where appropriate, making it easier for applications and operating-system components to interact with the underlying system.

Agents should be aware of Cosmos OS limitations and ensure that implementations are compatible with the target Cosmos environment.

This project is intended to be used within a Cosmos kernel. Agents should therefore consider:

- Kernel architecture and execution environment.
- Available .NET APIs and their compatibility.
- Memory usage and allocation overhead.
- Performance considerations.
- Limitations of the underlying hardware and drivers.

Do not assume that every standard .NET API behaves identically in a Cosmos kernel.

Verify that APIs and dependencies are supported in the target environment before using them.

## Existing Implementations

Before implementing new functionality, agents must inspect the existing codebase for reusable implementations.

Some functionality may already exist in other Tinos projects or external implementations.

Where appropriate, existing implementations should be adapted for Tinos.Api rather than recreated from scratch.

However, agents must ensure that reused code is compatible with the architecture and requirements of Tinos.Api.

Do not blindly copy implementations that depend on unrelated operating-system internals or project-specific assumptions.

Prefer extending existing implementations over introducing parallel implementations.

## Build and Testing

After making changes, agents should build the affected projects and run relevant tests where available.

Agents should verify that:

- The affected projects compile successfully.
- Existing functionality continues to work.
- New functionality behaves as expected.
- VB.NET and C# compatibility is preserved.
- Changes do not introduce unnecessary dependencies or regressions.

Agents must not claim that code works unless it has been verified through an appropriate build, test, or other validation method.

Any build failures, test failures, or warnings introduced by changes should be reported clearly.

Do not silently ignore build or test failures.

## API Compatibility

Public APIs should be designed with long-term compatibility in mind.

Agents must avoid unnecessarily renaming, removing, or changing existing public types, methods, properties, and interfaces.

Breaking changes require approval from the developer before implementation.

When extending existing APIs, prefer backwards-compatible changes whenever possible.

Consider both VB.NET and C# consumers when designing public interfaces.

## Scope of Changes

Agents should keep changes focused on the task they have been assigned.

Do not:

- Modify unrelated files.
- Refactor unrelated code.
- Replace existing implementations without approval.
- Introduce unnecessary architectural changes.
- Change project configuration without a valid reason.

If a task requires changes outside its expected scope, explain why before proceeding.

## General Guidelines

- Follow the existing project architecture.
- Prefer reusable and maintainable implementations.
- Avoid unnecessary duplication.
- Preserve existing functionality.
- Document new public APIs.
- Consider compatibility with all supported languages.
- Consider the Cosmos kernel environment when implementing system functionality.
- Ask the developer for clarification or approval when requirements are unclear.

When in doubt, prioritize preserving the existing codebase over making assumptions or introducing potentially disruptive changes.