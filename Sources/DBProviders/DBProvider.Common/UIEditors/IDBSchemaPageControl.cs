using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    public interface IDBSchemaWizardControl: IControlSyncUIContext
    {
        ReadOnlyCollection<IDBSchemaPageControl> Pages { get; }

        IDBSchemaPageControl ActivePage { get; }
        
        Tuple<IDBProvider, IConnectionInfo> SelectedDB { get; set; }
        NavigationButton ButtonPrevious { get; }
        NavigationButton ButtonNext { get; }
        List<Table> SelectedTablesAndColumns { get; set; }

        void ChangePageTitle(string pageTitle);

        //void UpdateButtons(PageDirection button, bool enabled);

        void UpdateButtonNextToFinish(bool nextToFinish);
    }

    public interface IDBSchemaPageControl
    {
        /// <summary>
        /// Gets the default page title.
        /// </summary>
        /// <value>The default page title.</value>
        string DefaultPageTitle { get; }

        /// <summary>
        /// Initializes the page control
        /// </summary>
        /// <param name="wizardOwner">The wizard owner.</param>
        void Initialize(IDBSchemaWizardControl wizardOwner);

        /// <summary>
        /// Leaves the page.
        /// </summary>
        /// <param name="leaveDirection">The leave direction.</param>
        /// <returns>
        /// Returns <c>true</c> if page can be leaved (all validation passed out),
        /// or <c>false</c> if page cannot be leaved (some/all validation does not pass)
        /// </returns>
        bool LeavePage(PageDirection leaveDirection);

        /// <summary>
        /// Enters the page.
        /// </summary>
        /// <param name="enterDirection">The enter direction tells from which direction was page entered.</param>
        void EnterPage(PageDirection enterDirection);

        void UpdateControls(bool enabled, bool waitCursor);
    }

    #region enum PageLeaveDirection

    public enum PageDirection
    {
        Back,
        Next,
        Finish
    }

    #endregion enum PageLeaveDirection
}