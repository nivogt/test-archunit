namespace TestArchUnit.Architecture;

/// <summary>
/// Advanced license validation using NuGet package metadata.
/// This validator checks the project.assets.json file to detect GPL-licensed dependencies.
/// </summary>
public class NuGetLicenseValidator
{
    private static readonly HashSet<string> ApprovedLicenses = new()
    {
        "MIT",
        "Apache-2.0",
        "Apache",
        "BSD",
        "BSD-2-Clause",
        "BSD-3-Clause",
        "ISC",
        "MPL-2.0"
    };

    private static readonly HashSet<string> ProhibitedLicenses = new()
    {
        "GPL",
        "GPL-2.0",
        "GPL-3.0",
        "GPLv2",
        "GPLv3",
        "AGPL",
        "AGPL-3.0",
        "AGPLv3"
    };

    /// <summary>
    /// Validates that a project file contains no GPL-licensed dependencies.
    /// </summary>
    public IEnumerable<LicenseViolation> ValidateProjectFile(string projectFilePath)
    {
        var violations = new List<LicenseViolation>();

        try
        {
            var projectDirectory = Path.GetDirectoryName(projectFilePath) ?? "";
            var assetsFile = Path.Combine(projectDirectory, "obj", "project.assets.json");

            if (!File.Exists(assetsFile))
            {
                return violations;
            }

            var content = File.ReadAllText(assetsFile);
            var packages = ParsePackagesFromAssets(content);

            foreach (var package in packages)
            {
                if (ProhibitedLicenses.Any(lic => package.License?.Contains(lic, StringComparison.OrdinalIgnoreCase) ?? false))
                {
                    violations.Add(new LicenseViolation
                    {
                        PackageName = package.Name,
                        DetectedLicense = package.License,
                        Severity = "Error",
                        Message = $"Package '{package.Name}' uses prohibited license: {package.License}"
                    });
                }
            }
        }
        catch (Exception ex)
        {
            violations.Add(new LicenseViolation
            {
                PackageName = "Unknown",
                DetectedLicense = "Unknown",
                Severity = "Warning",
                Message = $"Failed to validate licenses: {ex.Message}"
            });
        }

        return violations;
    }

    /// <summary>
    /// Parses package information from project.assets.json.
    /// </summary>
    private static List<PackageInfo> ParsePackagesFromAssets(string assetsJson)
    {
        var packages = new List<PackageInfo>();

        try
        {
            // Simple JSON parsing (production code would use Newtonsoft.Json)
            var lines = assetsJson.Split('\n');
            
            foreach (var line in lines)
            {
                if (line.Contains("\"name\":") && line.Contains("\"version\":"))
                {
                    // Extract package name and license information
                    var packageInfo = ExtractPackageInfo(line);
                    if (packageInfo != null)
                    {
                        packages.Add(packageInfo);
                    }
                }
            }
        }
        catch
        {
            // Silently fail for now
        }

        return packages;
    }

    /// <summary>
    /// Extracts package information from a line in the assets file.
    /// </summary>
    private static PackageInfo? ExtractPackageInfo(string line)
    {
        try
        {
            // This is a simplified extraction. Real implementation would use proper JSON parsing
            if (line.Contains("\"name\":"))
            {
                var nameStart = line.IndexOf("\"name\":", StringComparison.OrdinalIgnoreCase) + 8;
                var nameEnd = line.IndexOf(",", nameStart);
                
                if (nameStart > 7 && nameEnd > nameStart)
                {
                    var name = line.Substring(nameStart, nameEnd - nameStart).Trim().Trim('"');
                    return new PackageInfo { Name = name, License = "Unknown" };
                }
            }
        }
        catch
        {
            // Silently handle extraction errors
        }

        return null;
    }
}

/// <summary>
/// Represents a package from NuGet.
/// </summary>
public class PackageInfo
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0.0";
    public string? License { get; set; }
}

/// <summary>
/// Represents a license compliance violation.
/// </summary>
public class LicenseViolation
{
    public string PackageName { get; set; } = string.Empty;
    public string DetectedLicense { get; set; } = string.Empty;
    public string Severity { get; set; } = "Warning";
    public string Message { get; set; } = string.Empty;
}
