using System;
using System.Collections.Generic;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade
{
    public class ModelUpgrader
    {
        private readonly List<IModelVersionUpgrader> Upgraders = new List<IModelVersionUpgrader>();
        private bool upgradersMakeChanges;
        private static ModelUpgrader instance;

        public static readonly VersionNumber Version_1_0_0_0 = new VersionNumber("1.0.0.0");
        public static readonly VersionNumber Version_1_0_1_0 = new VersionNumber("1.0.1.0");
        public static readonly VersionNumber Version_1_0_2_0 = new VersionNumber("1.0.2.0");
        public static readonly VersionNumber Version_1_0_3_0 = new VersionNumber("1.0.3.0");
        public static readonly VersionNumber Version_1_0_4_0 = new VersionNumber("1.0.4.0");
        public static readonly VersionNumber Version_1_0_5_0 = new VersionNumber("1.0.5.0");

        public static ModelUpgrader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModelUpgrader();
                }

                return instance;
            }
        }

        public bool UpgradersMakeChanges
        {
            get { return upgradersMakeChanges; }
        }

        private ModelUpgrader()
        {
            Upgraders.Add(new ModelVersionUpgrader_1_0_5_0());
            CleanUp();
        }

        internal VersionNumber DeserializingModelVersion { get; set; }

        public void CleanUp()
        {
            DeserializingModelVersion = VersionNumber.Empty;
        }

        public void UpdateMakeChangesFlag()
        {
            this.upgradersMakeChanges = true;
        }

        /// <summary>
        /// Requests the serialization upgrade.
        /// </summary>
        /// <param name="upgraderFunc">The upgrader func, 1st param - upgrader, 2nd must return true if upgrader make changes or false if not.</param>
        public void RequestSerializationUpgrade(Func<IModelVersionUpgrader, bool> upgraderFunc)
        {
            upgradersMakeChanges = false;

            foreach (IModelVersionUpgrader versionUpgrader in Upgraders)
            {
                bool upgraderMakeChanges = upgraderFunc(versionUpgrader);
                if (upgraderMakeChanges)
                {
                    upgradersMakeChanges = true;
                }
            }
        }
    }
}