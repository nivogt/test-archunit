# Quick Start Guide - TestArchUnit

## 🚀 5-Minute Setup

### Step 1: Clone the Repository
```bash
cd /home/nico/Documents/Projects/test-archunit
```

### Step 2: Verify .NET Installation
```bash
dotnet --version   # Should be 8.0 or higher
```

### Step 3: Restore Dependencies
```bash
dotnet restore
```

### Step 4: Build Solution
```bash
dotnet build
```

### Step 5: Run Architecture Tests
```bash
dotnet test src/TestArchUnit.Architecture/TestArchUnit.Architecture.csproj --verbosity detailed
```

### Step 6: Run the API (Optional)
```bash
dotnet run --project src/TestArchUnit.API
```
Then visit: `https://localhost:7123/swagger`

---

## 📋 What Gets Tested

### ✅ License Compliance Tests
- **NoGpl3LicensedDependencies**: Ensures no GPL 3 licensed packages
- **OnlyApprovedLicensesUsed**: Validates MIT, Apache 2.0, BSD, ISC licenses only
- **Current Packages**: All compliant ✅

### ✅ Architecture Tests
- **ControllersShallDependOnInterfaces**: Proper dependency injection
- **AllControllersHaveApiControllerAttribute**: REST conventions
- **ControllersAreInControllerNamespace**: Layering enforcement

### ✅ Naming Convention Tests
- **PublicClassesHaveMeaningfulNames**: No generic Class1, Class2 names

---

## 🔍 Test Execution Examples

### Run All Tests
```bash
dotnet test
```

### Run Only License Tests
```bash
dotnet test --filter "LicenseComplianceTests"
```

### Run with Detailed Output
```bash
dotnet test --verbosity detailed
```

### Run Specific Test
```bash
dotnet test --filter "NoGpl3LicensedDependencies"
```

---

## 🐛 Troubleshooting

### Issue: "dotnet: command not found"
**Solution**: Install .NET 8.0 SDK from https://dotnet.microsoft.com/download

### Issue: "The project file does not contain a TargetFramework"
**Solution**: Ensure all `.csproj` files have `<TargetFramework>net8.0</TargetFramework>`

### Issue: "Could not find project.assets.json"
**Solution**: Run `dotnet restore` to generate the file

### Issue: Tests fail with "ArchUnitNET" errors
**Solution**: Ensure ArchUnitNET package is installed: `dotnet restore`

---

## 📊 Expected Test Results

When all tests pass:
```
Passed!  - LicenseComplianceTests.NoGpl3LicensedDependencies
Passed!  - LicenseComplianceTests.OnlyApprovedLicensesUsed
Passed!  - LicenseComplianceTests.ControllersShallDependOnInterfaces
Passed!  - NamingConventionTests.PublicClassesHaveMeaningfulNames
Passed!  - ArchitectureLayeringTests.ControllersAreInControllerNamespace

Test Run Successful.
Total tests: 5 | Passed: 5 | Failed: 0
```

---

## 🔄 GitHub Actions Workflow

The repository includes automated CI/CD:

1. **Push to main/develop** → Workflow triggers automatically
2. **Tests run** in GitHub Actions (Linux, .NET 8.0)
3. **Results published** with detailed logs

**To view results:**
1. Go to your GitHub repository
2. Click **Actions** tab
3. Select **Test ArchUnit License Guardrails**
4. View detailed logs and test results

---

## 📝 Adding New Dependencies

To add a new NuGet package (e.g., `Newtonsoft.Json`):

1. **Edit** `src/TestArchUnit.API/TestArchUnit.API.csproj`:
   ```xml
   <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
   ```

2. **Restore** dependencies:
   ```bash
   dotnet restore
   ```

3. **Verify** license on NuGet.org (must not be GPL 3)

4. **Test** to ensure no license violations:
   ```bash
   dotnet test
   ```

**If GPL 3 package is added:**
- Test `NoGpl3LicensedDependencies` will FAIL ❌
- Must remove package or find GPL 3-compliant alternative

---

## 🎯 Key Concepts

### Why GPL 3 License Matters
- **Viral Copyleft**: Forces your code to be GPL 3 if you use it
- **Legal Risk**: Can affect commercial deployment
- **Compliance**: Industry best practice to block GPL 3

### Approved Licenses (Safe to Use)
- **MIT**: Permissive, allows commercial use
- **Apache 2.0**: Permissive with patent protection
- **BSD**: Permissive, old and well-tested
- **ISC**: Permissive, essentially MIT equivalent

### Blocked Licenses (Will Fail Tests)
- **GPL-3.0**: Viral copyleft (blocked by design)
- **AGPL-3.0**: Even more restrictive than GPL-3
- **GPL-2.0**: GPL v2 (can block if policy requires)

---

## 📚 Further Reading

- [ArchUnitNET Guide](https://github.com/BenediktEberhardsen/ArchUnitNET)
- [SPDX License List](https://spdx.org/licenses/)
- [GPL 3.0 vs Other Licenses](https://www.synopsys.com/blogs/software-security/gpl-license-copyleft/)
- [NuGet License Compliance](https://docs.microsoft.com/en-us/nuget/)

---

## ✨ What's Next?

1. **Customize License Rules**: Modify `NuGetLicenseValidator.cs` to add/remove licenses
2. **Add API Endpoints**: Extend `ProductsController.cs` with more operations
3. **Expand Tests**: Add more architecture rules in `LicenseComplianceTests.cs`
4. **Deploy**: Configure GitHub Actions to deploy to cloud (Azure, AWS, etc.)

---

**Last Updated**: June 2026  
**Framework**: .NET 8.0  
**Test Framework**: xUnit + ArchUnitNET
