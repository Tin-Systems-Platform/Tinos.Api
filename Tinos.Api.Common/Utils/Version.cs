using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos.Api.Common.Utils
{
    /// <summary>
    /// Represents a version number.
    /// </summary>
    public class Version
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class with the specified major, minor, and patch version numbers.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="patch">The patch version number.</param>
        public Version(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        /// <summary>
        /// Major version number.
        /// </summary>
        public int Major { get; }

        /// <summary>
        /// Minor version number.
        /// </summary>
        public int Minor { get; }

        /// <summary>
        /// Patch version number.
        /// </summary>
        public int Patch { get; }

        /// <summary>
        /// If the version is unstable, this property can hold additional information about the unstable version (e.g., "alpha", "beta", "rc1").
        /// </summary>
        public string UnstableVersion { get; set; }

        /// <summary>
        /// Returns a string representation of the version in the format "Major.Minor.Patch".
        /// </summary>
        /// <returns>the string representation of the version</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(UnstableVersion))
                return $"{Major}.{Minor}.{Patch}-{UnstableVersion}";

            return $"{Major}.{Minor}.{Patch}";
        }

        /// <summary>
        /// Parses a version string into a Version object.
        /// </summary>
        /// <param name="versionString">the version string to parse</param>
        /// <returns>the parsed Version object</returns>
        public static Version Parse(string versionString)
        {
            if (!TryParse(versionString, out Version version))
                throw new FormatException("Invalid version format.");

            return version;
        }

        /// <summary>
        /// Tries to parse a version string into a Version object.
        /// </summary>
        /// <param name="versionString">the version string to parse</param>
        /// <param name="version">version object if parsing is successful</param>
        /// <returns>true if parsing is successful; otherwise, false</returns>
        public static bool TryParse(string versionString, out Version version)
        {
            version = null;

            if (string.IsNullOrWhiteSpace(versionString))
                return false;

            string[] versionParts = versionString.Split('-', 2);
            string[] numbers = versionParts[0].Split('.');

            if (numbers.Length != 3 ||
                !int.TryParse(numbers[0], out int major) ||
                !int.TryParse(numbers[1], out int minor) ||
                !int.TryParse(numbers[2], out int patch))
                return false;

            if (major < 0 || minor < 0 || patch < 0)
                return false;

            if (versionParts.Length == 2 &&
                string.IsNullOrWhiteSpace(versionParts[1]))
                return false;

            version = new Version(major, minor, patch)
            {
                UnstableVersion = versionParts.Length == 2
                    ? versionParts[1]
                    : null
            };

            return true;
        }
    }
}
