# Getting started with Tinos.Api

This guide will help you get started with Tinos.Api. It provides step-by-step instructions for setting up your development environment, installing the necessary dependencies, and creating your first application using Tinos.Api.

## Prerequisites

Before you begin, ensure that you have the following installed:

- **.NET SDK 10.0 or later**
- **A code editor**, such as Visual Studio or Visual Studio Code
- **Basic C# knowledge** and familiarity with .NET development

### Additional requirements for OS development

If you intend to build an operating system using Tinos.Api, you will also need:

- **Cosmos User Kit** for developing Cosmos-based operating systems.


## Adding Tinos.Api to your project

The following sections explain how to add Tinos.Api to your project. Several methods are available, but using the NuGet package is the recommended approach.

### Using the NuGet package (Recommended)

Tinos.Api is available through NuGet. To install the common package, run the following command in your project directory:

```bash
dotnet add package Tinos.Api.Common
```

This package contains the common APIs shared across supported languages and projects.

### Using a Git submodule (Not recommended)

You can add the Tinos.Api repository as a Git submodule by running:

```bash
git submodule add https://github.com/Tin-Systems-Platform/Tinos.Api
```

### Cloning outside your project (Not recommended)

Alternatively, you can clone the Tinos.Api repository separately:

```bash
git clone https://github.com/Tin-Systems-Platform/Tinos.Api
```

### Adding a project reference (Not recommended)

After obtaining the repository, you can reference the relevant Tinos.Api project from your own project.

Open your project's `.csproj` file and add the following inside an `<ItemGroup>`:

```xml
<ProjectReference Include="path/to/Tinos.Api.Common.csproj" />
```
