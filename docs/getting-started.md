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

Currently, Tinos.Api is not available on NuGet. To use Tinos.Api in your project, you will need to clone the repository and reference the project directly or use the repo as a submodule in your own repository.

### Submodule
To add Tinos.Api as a submodule, run the following command in your project directory:

```bash
git submodule add #TODO: Add the repository URL here
```

### Cloning outside of your project
Alternatively, you can clone the Tinos.Api repository outside of your project and reference it in your project. To do this, run the following command:

```bash
git clone #TODO: Add the repository URL here
```

### Adding a project reference
Once you have the Tinos.Api project in your solution, you can add a reference to it in your projects `.csproj` file. Open the `.csproj` file of your project and add the following line inside the `<ItemGroup>` section:

```xml
<ProjectReference Include="path/to/Tinos.Api.csproj" />
```