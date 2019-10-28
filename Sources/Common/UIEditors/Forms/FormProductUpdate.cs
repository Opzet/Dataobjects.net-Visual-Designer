using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormProductUpdate : Form
    {
        private ProductVersionDto latestProductVersion;
        private VersionNumber currentVersion;
        
        private const string HTML_CURRENT_VERSION = "<font style=\"font-family:tahoma;font-size:8pt\">Current version is <b>{0}</b>, upgrade is recommended.<font>";
        private const string HTML_CURRENT_VERSION_NO_UPD_NEEDED = "<font style=\"font-family:tahoma;font-size:8pt\">Current version is <b>{0}</b>, no upgrade needed.<font>";
        
        private const string HTML_NEW_VERSION = "<font style=\"font-family:tahoma;font-size:8pt\">A new version <b>{0}</b> is available on the web.<font>";
        private const string HTML_NEW_VERSION_SAME = "<font style=\"font-family:tahoma;font-size:8pt\">A latest version <b>{0}</b> is same as current.<font>";
        
        private const string HTML_RELEASE_DATE = "<font style=\"font-family:tahoma;font-size:8pt\">Released at <b>{0}</b> with those changes:<font>";

        private const string HTML_CHANGES_HEADER =
            "<table border=0 cellspacing=0 cellpadding=2 style=\"font-family:tahoma;font-size:8pt\" width=365>";
        
        private const string HTML_CHANGES_CATEGORY =
            "<tr><td style=\"color:{0};background-color:#F5F5F5\" colspan=2><b>{1} ({2})</b></td></tr>";

        private const string HTML_CHANGES_CHANGE_ROW =
            "<tr><td style=\"color:{0}\" width=50>#{1}</td><td>{2}</td></tr>";

        private const string HTML_CHANGES_FOOTER = "</table>";

        private static readonly Dictionary<ProductVersionChangeType, string> CHANGE_COLORS = new Dictionary<ProductVersionChangeType, string>
            {
                {ProductVersionChangeType.Feature, "blue"},
                {ProductVersionChangeType.Issue, "green"},
                {ProductVersionChangeType.Other, "brown"},
            };

        public FormProductUpdate()
        {
            InitializeComponent();
        }

        public static bool DialogShow(VersionNumber currentVersion, ProductVersionDto latestProductVersion, bool skipNewVersion)
        {
            using (var form = new FormProductUpdate())
            {
                form.Initialize(currentVersion, latestProductVersion, skipNewVersion);
                form.ShowDialog();
                return form.chDoNotShow.Checked;// && isOk;
            }
        }

        private void Initialize(VersionNumber currentVersion, ProductVersionDto latestProductVersion, bool skipNewVersion)
        {
            this.currentVersion = currentVersion;
            this.latestProductVersion = latestProductVersion;
            this.chDoNotShow.Checked = skipNewVersion;

            bool sameVersions = currentVersion.Equals(new VersionNumber(latestProductVersion.Version));

            if (sameVersions)
            {
                htmlCurrentVersion.Text = string.Format(HTML_CURRENT_VERSION_NO_UPD_NEEDED, currentVersion);
                htmlNewVersion.Text = string.Format(HTML_NEW_VERSION_SAME, latestProductVersion.Version);
                htmlReleaseDate.Visible = false;
                htmlChanges.Visible = false;
                btnOk.Visible = false;
                chDoNotShow.Visible = false;
            }
            else
            {
                htmlCurrentVersion.Text = string.Format(HTML_CURRENT_VERSION, currentVersion);
                htmlNewVersion.Text = string.Format(HTML_NEW_VERSION, latestProductVersion.Version);
                DateTimeOffset offset = new DateTimeOffset(latestProductVersion.Published, latestProductVersion.PublishedOffset);
                htmlReleaseDate.Text = string.Format(HTML_RELEASE_DATE, offset);

                StringBuilder changesHtml = new StringBuilder(HTML_CHANGES_HEADER);
                var changes = latestProductVersion.Changes;
                var changesFeatures = changes.Where(change => change.Type == ProductVersionChangeType.Feature).OrderBy(change => change.Order);
                var changesIssues = changes.Where(change => change.Type == ProductVersionChangeType.Issue).OrderBy(change => change.Order);
                var changesOthers = changes.Where(change => change.Type == ProductVersionChangeType.Other).OrderBy(change => change.Order);

                if (changesFeatures.Any())
                {
                    changesHtml.AppendFormat(HTML_CHANGES_CATEGORY, CHANGE_COLORS[ProductVersionChangeType.Feature],
                                             "Features", changesFeatures.Count());

                    foreach (var change in changesFeatures)
                    {
                        changesHtml.AppendFormat(HTML_CHANGES_CHANGE_ROW, CHANGE_COLORS[change.Type], change.WorkItemId,
                                                 change.Title);
                    }
                }

                if (changesIssues.Any())
                {
                    changesHtml.AppendFormat(HTML_CHANGES_CATEGORY, CHANGE_COLORS[ProductVersionChangeType.Issue],
                                             "Issues", changesIssues.Count());

                    foreach (var change in changesIssues)
                    {
                        changesHtml.AppendFormat(HTML_CHANGES_CHANGE_ROW, CHANGE_COLORS[change.Type], change.WorkItemId,
                                                 change.Title);
                    }
                }

                if (changesOthers.Any())
                {
                    changesHtml.AppendFormat(HTML_CHANGES_CATEGORY, CHANGE_COLORS[ProductVersionChangeType.Other],
                                             "Other", changesOthers.Count());

                    foreach (var change in changesOthers)
                    {
                        changesHtml.AppendFormat(HTML_CHANGES_CHANGE_ROW, CHANGE_COLORS[change.Type], change.WorkItemId,
                                                 change.Title);
                    }
                }

                changesHtml.Append(HTML_CHANGES_FOOTER);
                htmlChanges.Text = changesHtml.ToString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ProcessStartInfo nfo = new ProcessStartInfo(this.latestProductVersion.Url);
            nfo.UseShellExecute = true;

            Process.Start(nfo);

            this.DialogResult = DialogResult.OK;
        }

    }
}
