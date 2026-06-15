# TestArchUnit - .NET REST API with ArchUnit License Guardrails

A demonstration project showcasing how to implement architectural safeguards in a .NET application using **ArchUnitNET**, with a specific focus on enforcing license compliance policies (blocking GPL 3 licenses).

## 📋 Project Structure

```
test-archunit/
├── src/
│   ├── TestArchUnit.API/           # Simple REST API component
│   │   ├── Controllers/
│   │   │   ├── ProductsController.cs
│   │   │   └── HealthController.cs
│   │   ├── Program.cs              # API configuration
│   │   └── TestArchUnit.API.csproj
│   │
│   └── TestArchUnit.Architecture/  # Architecture test suite
│       ├── LicenseComplianceTests.cs
│       ├── NuGetLicenseValidator.cs
│       ├── NuGetLicenseValidatorTests.cs
│       └── TestArchUnit.Architecture.csproj
│
├── .github/
│   └── workflows/
│       └── archunit-guardrails.yml # GitHub Actions pipeline
│
├── TestArchUnit.sln               # Solution file
└── README.md                       # This file
```

## 🎯 Key Features

### 1. **REST API Component**
   - **ProductsController**: Full CRUD operations for products (GET, POST, PUT, DELETE)
   - **HealthController**: API health status endpoint
   - Built with ASP.NET Core 10.0
   - Includes Swagger/OpenAPI documentation
   - Structured logging with Serilog

### 2. **ArchUnit License Guardrails**

The project implements several architectural tests:

#### ✅ GPL 3 License Check
- **Test**: `NoGpl3LicensedDependencies()`
- **Purpose**: Ensures no dependencies use GPL 3 license (viral copyleft)
- **Impact**: Prevents legal and compliance issues
- **Implementation**: Custom `NuGetLicenseValidator` parses `project.assets.json`

#### ✅ Approved Licenses Only
- **Test**: `OnlyApprovedLicensesUsed()`
- **Allowed**: MIT, Apache 2.0, BSD, ISC
- **Blocked**: GPL (any version), AGPL, restrictive licenses

#### ✅ Controller Dependency Injection
- **Test**: `ControllersShallDependOnInterfaces()`
- **Purpose**: Ensures proper DI patterns and testability

#### ✅ Naming Conventions
- **Test**: `PublicClassesHaveMeaningfulNames()`
- **Purpose**: Enforces code quality standards

#### ✅ Architecture Layering
- **Test**: `ControllersAreInControllerNamespace()`
- **Purpose**: Maintains proper separation of concerns

## 🚀 Getting Started

### Prerequisites
- .NET 10.0 SDK or later
- Visual Studio, VS Code, or Rider (optional)
- Git for version control

### Build the Solution

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test
```

### Run the API Locally

```bash
# Run the API
dotnet run --project src/TestArchUnit.API

# API will be available at https://localhost:7123
# Swagger UI: https://localhost:7123/swagger
```

### Run Specific Test Suites

```bash
# Run only license compliance tests
dotnet test src/TestArchUnit.Architecture/TestArchUnit.Architecture.csproj --filter "LicenseComplianceTests"

# Run only NuGet validator tests
dotnet test src/TestArchUnit.Architecture/TestArchUnit.Architecture.csproj --filter "NuGetLicenseValidatorTests"

# Verbose output
dotnet test -v detailed
```

## 🏗️ Architecture Rules Explained

### GPL 3 License Detection

The `NuGetLicenseValidator` works by:

1. **Parsing Dependencies**: Reads `project.assets.json` generated during restore
2. **License Lookup**: Checks each package's license metadata
3. **Compliance Check**: Compares against approved/prohibited lists
4. **Reporting**: Generates violations for CI/CD systems

**Prohibited Licenses:**
- GPL (v2, v3)
- AGPL (v3)
- Any viral copyleft license

**Approved Licenses:**
- MIT
- Apache 2.0
- BSD (2-Clause, 3-Clause)
- ISC
- MPL 2.0

### Current Dependencies (License-Compliant)

| Package | License | Status |
|---------|---------|--------|
| Microsoft.AspNetCore.OpenApi | MIT | ✅ Approved |
| Swashbuckle.AspNetCore | MIT | ✅ Approved |
| Newtonsoft.Json | MIT | ✅ Approved |
| Serilog | Apache 2.0 | ✅ Approved |
| Serilog.AspNetCore | Apache 2.0 | ✅ Approved |
| ArchUnitNET | Apache 2.0 | ✅ Approved |

## 🧪 Testing

All tests use **xUnit** and **ArchUnitNET**:

```csharp
// Example: License compliance test
[Fact]
public void NoGpl3LicensedDependencies()
{
    var rule = Types()
        .That()
        .ResideInAssembly(apiAssembly)
        .Should()
        .NotDependOnAny(Types()
            .That()
            .HaveName(x => x.Contains("GPL")));

    var violations = rule.Evaluate(Architecture).ToList();
    Assert.Empty(violations);
}
```

## 🔄 GitHub Actions Pipeline

The project includes an automated CI/CD pipeline (`.github/workflows/archunit-guardrails.yml`):

**Pipeline Steps:**
1. ✅ Checkout code
2. ✅ Setup .NET 10.0
3. ✅ Restore dependencies
4. ✅ Build solution
5. ✅ **Run Architecture Tests (License Compliance)**
6. ✅ Run all tests
7. ✅ Upload test results
8. ✅ Publish test report

**Required Permissions:**
The workflow requires the following GitHub token permissions:
- `contents: read` — Access repository code
- `checks: write` — Create/update check runs
- `issues: write` — Add comments to issues
- `pull-requests: write` — Post test result comments on pull requests

**Triggers:**
- On push to `main` or `develop` branches
- On pull requests to `main` branch

**Status Checks:**
Tests must pass before merging to ensure license compliance. Test results are posted as:
- Check runs (visible in PR checks section)
- Comments on pull requests (when `comment_mode` is enabled)

### Running Tests in GitHub Actions

The pipeline automatically runs on every push/PR. To manually trigger:

```bash
# Push to trigger workflow
git push origin feature/my-feature

# Check workflow status on GitHub
# Actions tab → archunit-guardrails.yml → View run results
```

## 📊 Example Test Output

```
Passed!  - LicenseComplianceTests.NoGpl3LicensedDependencies [0.023 s]
Passed!  - LicenseComplianceTests.OnlyApprovedLicensesUsed [0.015 s]
Passed!  - LicenseComplianceTests.ControllersShallDependOnInterfaces [0.008 s]
Passed!  - NamingConventionTests.PublicClassesHaveMeaningfulNames [0.006 s]
Passed!  - ArchitectureLayeringTests.ControllersAreInControllerNamespace [0.004 s]

Test Run Successful.
Total tests: 5 | Passed: 5 | Failed: 0
```

## ⚙️ Configuration

### Adding New Dependencies

When adding NuGet packages:

1. Add to `.csproj` file:
   ```xml
   <PackageReference Include="PackageName" Version="1.0.0" />
   ```

2. Restore: `dotnet restore`

3. Verify license: Check NuGet.org for GPL 3

4. Run tests to ensure compliance:
   ```bash
   dotnet test
   ```

### Modifying License Rules

Edit `NuGetLicenseValidator.cs`:

```csharp
private static readonly HashSet<string> ProhibitedLicenses = new()
{
    "GPL-3.0",          // Add or remove licenses here
    "AGPL-3.0",
    // "BSD-3-Clause"   // Uncomment to add restrictions
};
```

## 🔍 Troubleshooting

### Test Failures

| Issue | Solution |
|-------|----------|
| Tests not running | Ensure .NET 8.0 SDK is installed: `dotnet --version` |
| GPL license detected | Remove the package or find compliant alternative |
| Project.assets.json not found | Run `dotnet restore` first |

### Build Issues

```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

## 📖 References

- [ArchUnitNET Documentation](https://www.archunit.org/)
- [NuGet License Guide](https://docs.microsoft.com/en-us/nuget/)
- [SPDX License List](https://spdx.org/licenses/)
- [GPL 3.0 License Details](https://www.gnu.org/licenses/gpl-3.0.html)

## 📝 License

This project is licensed under the **MIT License** - see LICENSE file for details.

---

**Created**: June 2026  
**Target Framework**: .NET 8.0  
**Test Framework**: xUnit + ArchUnitNET
