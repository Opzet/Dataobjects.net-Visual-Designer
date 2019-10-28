using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;
using TXSoftware.DataObjectsNetEntityModel.Dsl;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel
{
    public static class VersionUpgradeManager
    {
        private const string CHECK_VERSION_URL_FORMAT = "http://{0}/versions/latest/{1}/{2}";
        private const string SERVER_FOR_UPDATES = "doemd.aspone.cz";

        private static bool debugMode = false;
        private static bool alreadyExecuted = false;

        public static void CheckForUpdate(bool showInfoWhenNoUpgrade = false, bool asyncCall = true)
        {
            if (alreadyExecuted)
            {
                return;
            }

            VersionNumber currentVersion = ModelApp.ApplicationVersion;

            alreadyExecuted = true;

            if (asyncCall)
            {
                Tuple<VersionNumber, bool> data = new Tuple<VersionNumber, bool>(currentVersion, showInfoWhenNoUpgrade);
                ThreadPool.QueueUserWorkItem(new SynchronizationCallback(
                    state =>
                    {
                        Tuple<VersionNumber, bool> _data = (Tuple<VersionNumber, bool>)state;
                        InternalCheckForUpdate(_data.Item1, _data.Item2);
                    }), data);

//                ThreadPool.QueueUserWorkItem(
//                    state =>
//                    {
//                        Tuple<VersionNumber, bool> _data = (Tuple<VersionNumber, bool>) state;
//                        InternalCheckForUpdate(_data.Item1, _data.Item2);
//                    }, data);
            }
            else
            {
                InternalCheckForUpdate(currentVersion, showInfoWhenNoUpgrade);
            }
        }

        private static SynchronizationContext synchronizationContext;

        private static void InternalCheckForUpdate(VersionNumber currentVersion, bool showInfoWhenNoUpgrade)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Const.PRODUCT_REG_KEY);
            string dontShowUpdateForVersion = null;
            if (registryKey != null)
            {
                dontShowUpdateForVersion =
                    registryKey.GetValue(Const.PRODUCT_REG_KEY_VALUE_DONT_SHOW_UPDATE_VERSION) as string;
                registryKey.Close();
            }

            Action showErrorAction = () => MessageBox.Show("Error getting information about new version, please try again later.", "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

            synchronizationContext = SynchronizationContext.Current;

            WebClient client = new WebClient();
            Uri uri = new Uri(string.Format(CHECK_VERSION_URL_FORMAT, SERVER_FOR_UPDATES, currentVersion, debugMode ? "debug" : "!"));
            client.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs args)
            {
                try
                {
                    if (!args.Cancelled && args.Error == null)
                    {
                        bool hasNewVersionInfo = !string.IsNullOrEmpty(args.Result);

                        try
                        {
                            ProductVersionDto latestProductVersion = hasNewVersionInfo ? ProductVersionDto.FromXml(args.Result) : null;
                            if (latestProductVersion != null)
                            {
                                VersionNumber latestProductVersionId = new VersionNumber(latestProductVersion.Version);
                                if (latestProductVersionId.CompareTo(currentVersion) == 1)
                                {
                                    var versionsToSkip = ParseVersionsToSkip(dontShowUpdateForVersion);

                                    bool skipNewVersion = versionsToSkip.Any(latestProductVersionId.Equals);
                                    bool canShowUpgradeForm = versionsToSkip.Count == 0 || !skipNewVersion;

                                    if (canShowUpgradeForm || showInfoWhenNoUpgrade)
                                    {
                                        synchronizationContext.Post(
                                            state =>
                                            ShowNewUpdateAvailableForm((Tuple<VersionNumber, ProductVersionDto, bool>) state),
                                            new Tuple<VersionNumber, ProductVersionDto, bool>(currentVersion,
                                                latestProductVersion, skipNewVersion));
                                    }
                                }
                            }
                            else
                            {
                                if (showInfoWhenNoUpgrade)
                                {
                                    ProductVersionDto dummpyProductVersion = new ProductVersionDto(currentVersion.ToString());
                                    FormProductUpdate.DialogShow(currentVersion, dummpyProductVersion, false);
                                }
                            }
                        }
                        catch
                        {
                            showErrorAction();
                        }
                    }
                    else
                    {
                        showErrorAction();
                    }
                }
                finally
                {
                    alreadyExecuted = false;
                }
            };
            client.DownloadStringAsync(uri);
        }

        private static List<VersionNumber> ParseVersionsToSkip(string dontShowUpdateForVersions)
        {
            List<VersionNumber> versionsToSkip = new List<VersionNumber>();
            if (!string.IsNullOrEmpty(dontShowUpdateForVersions))
            {
                foreach (var versionToNotShow in dontShowUpdateForVersions.Split(';'))
                {
                    try
                    {
                        versionsToSkip.Add(new VersionNumber(versionToNotShow));
                    }
                    catch
                    {}
                }
            }
            return versionsToSkip;
        }

        private static void ShowNewUpdateAvailableForm(Tuple<VersionNumber, ProductVersionDto, bool> versionInfo)
        {
            VersionNumber currentVersion = versionInfo.Item1;
            ProductVersionDto latestProductVersion = versionInfo.Item2;
            bool skipNewVersion = versionInfo.Item3;

            bool DoNotShowAgain = FormProductUpdate.DialogShow(currentVersion, latestProductVersion, skipNewVersion);
            
            
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Const.PRODUCT_REG_KEY, true);

            if (registryKey == null)
            {
                registryKey = Registry.CurrentUser.CreateSubKey(Const.PRODUCT_REG_KEY);
            }

            if (registryKey != null)
            {
                string currentValue =
                    registryKey.GetValue(Const.PRODUCT_REG_KEY_VALUE_DONT_SHOW_UPDATE_VERSION) as string;
                List<VersionNumber> versionsToSkip = string.IsNullOrEmpty(currentValue)
                                                         ? new List<VersionNumber>()
                                                         : ParseVersionsToSkip(currentValue);

                VersionNumber latestVersion = new VersionNumber(latestProductVersion.Version);

                if (DoNotShowAgain)
                {
                    if (!versionsToSkip.Any(item => item.Equals(latestVersion)))
                    {
                        versionsToSkip.Add(latestVersion);
                    }
                }
                else
                {
                    versionsToSkip.RemoveAll(number => number.Equals(latestVersion));
                }

                currentValue = string.Join(";", versionsToSkip.Select(item => item.ToString()).ToArray());

                registryKey.SetValue(Const.PRODUCT_REG_KEY_VALUE_DONT_SHOW_UPDATE_VERSION, currentValue);
                registryKey.Close();
            }
        }

        public static void OpenProductWebSite()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Const.PRODUCT_REG_KEY, true);
            string productSiteUrl = null;
            if (registryKey != null)
            {
                productSiteUrl = registryKey.GetValue(Const.PRODUCT_REG_KEY_VALUE_WEB_SITE) as string;
            }
            
            if (string.IsNullOrEmpty(productSiteUrl))
            {
                productSiteUrl = "http://doemd.codeplex.com";
            }

            ThreadPool.QueueUserWorkItem(
                url =>
                {
                    ProcessStartInfo nfo = new ProcessStartInfo(url as string);
                    nfo.UseShellExecute = true;
                    Process.Start(nfo);
                }, productSiteUrl);
        }
    }
}