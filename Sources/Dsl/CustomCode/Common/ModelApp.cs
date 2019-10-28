using System;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public static class ModelApp
    {
        public static string FirstVersion = "1.0.0.0";
        private static VersionNumber currentVersion;

        public static VersionNumber ApplicationVersion
        {
            get
            {
                if (currentVersion == null)
                {
                    currentVersion = Util.CurrentVersion;
                }

                return currentVersion;
            }
        }

        public const string ProductName = @"DataObjects.Net Entity Model Designer";

        public static bool IsCompatibleWithTemplate(string templateVersion)
        {
            //NOTE: For now it is just simple compare of current version of model with version from template,
            // later can add more advanced compatibility check (e.g. template version A is compatible with more than 1 version od model, etc...)

            VersionNumber templVersion = new VersionNumber(templateVersion);

            return templVersion.Equals(ApplicationVersion);
            //return Util.StringEqual(templateVersion, ApplicationVersion, true);
        }

        /*private static readonly Dictionary<string, List<string>> safeUpgradeVersion =
            new Dictionary<string, List<string>>
            {
                {
                    "1.0.0.0", new List<string>
                               {
                                   "1.0.1.0"
                               }
                }
            };

        public static bool CanUpgrade(Version oldVersion, Version newVersion)
        {
            return CanUpgrade(oldVersion.ToString(), newVersion.ToString());
        }

        public static bool CanUpgrade(string oldVersion, string newVersion)
        {
            if (oldVersion == newVersion)
            {
                return true;
            }

            if (safeUpgradeVersion.ContainsKey(oldVersion))
            {
                return safeUpgradeVersion[oldVersion].Contains(newVersion);
            }

            return false;
        }*/
    }
}