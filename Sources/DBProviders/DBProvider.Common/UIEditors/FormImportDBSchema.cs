using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    public partial class FormImportDBSchema : Form, IDBSchemaWizardControl
    {
        private List<IDBSchemaPageControl> pages = null;
        private IDBSchemaPageControl activePage;
        private int activePageIndex = -1;
        private readonly LoadingAction loadingAction = new LoadingAction();
        private NavigationButton buttonPrevious;
        private NavigationButton buttonNext;

        public FormImportDBSchema()
        {
            InitializeComponent();
            Initialize();
        }

        private void Deinitialize()
        {
            ControlSync.Instance.Stop();
        }

        private void Initialize()
        {
            ControlSync.Initialize(this);

            this.pages = new List<IDBSchemaPageControl>(
                new IDBSchemaPageControl[]
                    {
                        pageDatabase,
                        pageTables,
                        pageAuxiliaryTables
                    });

            this.buttonPrevious = new NavigationButton("&Back", NavigationButtonType.Previous, NavigationButtonOnUpdate);
            this.buttonNext = new NavigationButton("&Next", NavigationButtonType.Next, NavigationButtonOnUpdate);

            BeginLoadingAction(true);
            try
            {
                foreach (var page in Pages)
                {
                    page.Initialize(this);
                }

                ChangeActivePage(PageDirection.Next);
            }
            finally
            {
                EndLoadingAction();
            }

            pageDatabase.Select();
        }

        private void NavigationButtonOnUpdate(NavigationButton button)
        {
            Button buttonControl = button.ButtonType == NavigationButtonType.Next ? btnNext : btnBack;
            buttonControl.Text = button.Text;
            buttonControl.Visible = button.Visible;
            buttonControl.Enabled = button.Enabled;
        }

        public static bool DialogShow(out ResultData resultData)
        {
            bool result;
            resultData = null;
            using (FormImportDBSchema form = new FormImportDBSchema())
            {
                try
                {
                    result = form.ShowDialog() == DialogResult.OK;
                    if (result)
                    {
                        resultData = new ResultData(form.SelectedTablesAndColumns);
                    }
                }
                finally
                {
                    form.Deinitialize();
                }
            }

            return result;
        }

        private void CloseDialog(bool operationResult)
        {
            this.DialogResult = operationResult ? DialogResult.OK : DialogResult.Cancel;
            this.Close();
        }

        public ReadOnlyCollection<IDBSchemaPageControl> Pages
        {
            get
            {
                return new ReadOnlyCollection<IDBSchemaPageControl>(this.pages);
            }
        }

        public IDBSchemaPageControl ActivePage
        {
            get { return activePage; }
        }

        public Tuple<IDBProvider, IConnectionInfo> SelectedDB { get; set; }

        public List<Table> SelectedTablesAndColumns { get; set; }

        public NavigationButton ButtonPrevious
        {
            get { return buttonPrevious; }
        }

        public NavigationButton ButtonNext
        {
            get { return buttonNext; }
        }

        private void SetActivePage(IDBSchemaPageControl page)
        {
            this.activePage = page;
            if (page != null)
            {
                activePageIndex = this.pages.FindIndex(item => item == page);
            }
            else
            {
                activePageIndex = -1;
            }
        }

        private void ChangeActivePage(PageDirection direction)
        {
            var lastActivePage = ActivePage;

            switch (direction)
            {
                case PageDirection.Back:
                {
                    if (ActivePage == null)
                    {
                        throw new Exception();
                    }

                    bool leavingLastPage = ActivePage == this.Pages.Last();

                    if (ActivePage.LeavePage(direction))
                    {
                        SetActivePage(this.Pages[activePageIndex - 1]);
                        ActivePage.EnterPage(direction);
                    }

                    if (leavingLastPage)
                    {
                        UpdateButtonNextToFinish(false);
                    }

                    break;
                }
                case PageDirection.Next:
                {
                    if (ActivePage == null)
                    {
                        SetActivePage(this.Pages.First());
                        ActivePage.EnterPage(direction);
                    }
                    else
                    {
                        if (ActivePage == this.Pages.Last())
                        {
                            bool finishResult = ActivePage.LeavePage(PageDirection.Finish);
                            if (finishResult)
                            {
                                CloseDialog(true);
                            }
                        }
                        else
                        {
                            bool leaveValidated = ActivePage.LeavePage(direction);
                            if (leaveValidated)
                            {
                                SetActivePage(this.Pages[activePageIndex + 1]);
                                ActivePage.EnterPage(direction);
                                if (ActivePage == this.Pages.Last())
                                {
                                    UpdateButtonNextToFinish(true);
                                }
                            }
                        }
                    }
                    break;
                }
                default:
                {
                    throw new InvalidOperationException("PageDirection.Finish");
                }
            }

            UpdateButtons(PageDirection.Back, activePageIndex > 0);
            UpdateButtons(PageDirection.Next, true); //ActivePage != this.Pages.Last());

            phPages.Controls.Clear();

            if (lastActivePage != null)
            {
                Control lastActivePageControl = lastActivePage as Control;
                lastActivePageControl.Visible = false;
            }

            Control control = ActivePage as Control;
            phPages.Controls.Add(control);
            control.Dock = DockStyle.Fill;
            control.Visible = true;
            ChangePageTitle(ActivePage.DefaultPageTitle);

            //UpdateControls();
        }

        public void ChangePageTitle(string pageTitle)
        {
            lbPageTitle.Text = pageTitle;
        }

        public void UpdateButtons(PageDirection button, bool enabled)
        {
            switch (button)
            {
                case PageDirection.Back:
                {
                    btnBack.Enabled = enabled;
                    break;
                }
                case PageDirection.Next:
                case PageDirection.Finish:
                {
                    btnNext.Enabled = enabled;
                    break;
                }
            }
        }

        public void UpdateButtonNextToFinish(bool nextToFinish)
        {
            btnNext.Text = nextToFinish ? "&Finish" : "&Next";
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ChangeActivePage(PageDirection.Next);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ChangeActivePage(PageDirection.Back);
        }

        public void BeginLoadingAction(bool showProgressMarquee)
        {
            lock (loadingAction.SyncRoot)
            {
                if (!loadingAction.SetActive())
                {
                    return;
                }
            }

            UpdateControls(false);
            UpdateProgressMarquee(showProgressMarquee);
        }

        private void UpdateControls(bool enabled)
        {
            this.phPages.Enabled = enabled;
            this.btnBack.Enabled = enabled && this.buttonPrevious.Enabled;
            this.btnNext.Enabled = enabled && this.buttonNext.Enabled;

            if (ActivePage != null)
            {
                ActivePage.UpdateControls(enabled, false);
            }
        }

        public void EndLoadingAction()
        {
            lock (loadingAction.SyncRoot)
            {
                if (loadingAction.Deactive())
                {
                    UpdateControls(true);
                    UpdateProgressMarquee(false);
                }
            }
        }

        public void UpdateProgressMarquee(bool visible)
        {
            mainProgress.Visible = visible;
            mainProgress.MarqueeAnimationSpeed = visible ? 30 : 0;
            Application.DoEvents();
        }

        #region class ResultData

        public class ResultData
        {
            public List<Table> tablesColumns { get; private set; }

            public ResultData(List<Table> tablesColumns)
            {
                this.tablesColumns = tablesColumns;
            }
        }

        #endregion class ResultData
    }
}