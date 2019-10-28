using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common.Properties;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormAddAssociation : Form
    {
        #region class ResultData

        public class ResultData
        {
            #region class PersistentItem

            public class PersistentItem
            {
                public PersistentTypeItem TypeItem { get; private set; }
                //public PersistentTypeItem PropertyTypeItem { get; private set; }
                public string PropertyName { get; private set; }
                public MultiplicityKind Multiplicity { get; private set; }
                public bool UseAssociationAttribute { get; private set; }

                public PersistentItem(PersistentTypeItem typeItem, 
                    /*PersistentTypeItem propertyTypeItem, */string propertyName, 
                    MultiplicityKind multiplicity, bool useAssociationAttribute)
                {
                    TypeItem = typeItem;
                    //PropertyTypeItem = propertyTypeItem;
                    PropertyName = propertyName;
                    Multiplicity = multiplicity;
                    UseAssociationAttribute = useAssociationAttribute;
                }
            }

            #endregion class PersistentItem

            public bool SimpleMode { get; private set; }
            public string AssociationName { get; private set; }
            public PersistentItem PersistentItem1 { get; private set; }
            public PersistentItem PersistentItem2 { get; private set; }

            public ResultData(bool simpleMode, string associationName, PersistentItem persistentItem1, 
                PersistentItem persistentItem2)
            {
                SimpleMode = simpleMode;
                AssociationName = associationName;
                PersistentItem1 = persistentItem1;
                PersistentItem2 = persistentItem2;
            }
        }

        #endregion class ResultData

        internal IEnumerable<PersistentTypeItem> existingTypeNames;
        internal IEnumerable<string> existingAssociations;
        private readonly Dictionary<bool, string> associationNames = new Dictionary<bool, string>();

        private bool disableTypeChangeNotification = false;

        internal static readonly MultiplicityKind[] orderedMultiplicities = new[]
                                                                            {
                                                                                MultiplicityKind.One,
                                                                                MultiplicityKind.ZeroOrOne,
                                                                                MultiplicityKind.Many
                                                                            };


        public FormAddAssociation()
        {
            InitializeComponent();
        }

        public static bool DialogShow(IEnumerable<PersistentTypeItem> existingTypeNames,
            IEnumerable<string> existingAssociations, PersistentTypeItem persistentTypeToSelect, 
            out ResultData resultData)
        {
            resultData = null;
            DialogResult dialogResult;
            using (FormAddAssociation form = new FormAddAssociation())
            {
                form.Initialize(existingTypeNames, existingAssociations, persistentTypeToSelect);
                dialogResult = form.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    resultData = form.GetResultData();
                }
            }

            return dialogResult == DialogResult.OK;
        }

        private ResultData GetResultData()
        {
            ResultData.PersistentItem persistentItem1;
            ResultData.PersistentItem persistentItem2;

            if (SelectedIsSimple)
            {
                persistentItem1 = controlSimple.GetResultPersistentItem(true);
                persistentItem2 = controlSimple.GetResultPersistentItem(false);
            }
            else
            {
                persistentItem1 = controlAdvanced.GetResultPersistentItem(true);
                persistentItem2 = controlAdvanced.GetResultPersistentItem(false);
            }

            return new ResultData(this.SelectedIsSimple, this.edAssociationName.Text, persistentItem1, persistentItem2);
        }

        private void OnTypeChanged(object sender, EventArgs e)
        {
            if (disableTypeChangeNotification)
            {
                return;
            }

            string action = (string) (sender as Control).Tag;
            bool isSimple = action == "0";

            if (UpdateControls(isSimple))
            {
                string associationName = GetAssociationName(isSimple);
                SetAssociationName(isSimple, associationName);
            }
        }

        private bool SelectedIsSimple
        {
            get { return rbSimple.Checked; }
        }

        private bool UpdateControls(bool isSimple)
        {
            bool canShow;
            if (isSimple)
            {
                canShow = controlSimple.CanShow();
            }
            else
            {
                canShow = controlAdvanced.CanShow();
            }

            if (canShow)
            {
                controlSimple.Visible = isSimple;
                controlAdvanced.Visible = !isSimple;
            }
            else
            {
                disableTypeChangeNotification = true;
                try
                {
                    rbSimple.Checked = true;
                }
                finally
                {
                    disableTypeChangeNotification = false;
                }
            }

            return canShow;
        }

        private void Initialize(IEnumerable<PersistentTypeItem> existingTypeNames, 
            IEnumerable<string> existingAssociations, PersistentTypeItem persistentTypeToSelect)
        {
            associationNames.Add(true, string.Empty);
            associationNames.Add(false, string.Empty);

            this.existingTypeNames = existingTypeNames;
            this.existingAssociations = existingAssociations;

            this.controlSimple.Initialize(this, persistentTypeToSelect);
            this.controlAdvanced.Initialize(this, persistentTypeToSelect);

            UpdateControls(true);
        }

        internal IconListEntry[] BuildPersistentTypesItems()
        {
            return (from item in existingTypeNames
                    select new IconListEntry(item.Name, item, GetTypeKindImage(item.Kind)))
                .ToArray();
        }

        internal static string GetMultiplicityDisplayName(MultiplicityKind multiplicityKind)
        {
            switch (multiplicityKind)
            {
                case MultiplicityKind.ZeroOrOne:
                {
                    return "0..1 (Zero or One)";
                }
                case MultiplicityKind.Many:
                {
                    return "* (Many)";
                }
                default:
                {
                    return "1 (One)";
                }
            }
        }

        public void SetAssociationName(bool simpleAssociation, string name, bool updateTextBox = true)
        {
            associationNames[simpleAssociation] = name;
            if (SelectedIsSimple == simpleAssociation && updateTextBox)
            {
                edAssociationName.Text = name;
            }
        }

        private string GetAssociationName(bool simpleAssociation)
        {
            return associationNames[simpleAssociation];
        }

        private Bitmap GetTypeKindImage(EntityKind entityKind)
        {
            switch (entityKind)
            {
                case EntityKind.Entity:
                {
                    return Resources.Entity;
                }
                case EntityKind.Structure:
                {
                    return Resources.Structure;
                }
                default:
                {
                    return Resources.Interface;
                }
            }
        }

        private void edAssociationName_TextChanged(object sender, EventArgs e)
        {
            SetAssociationName(SelectedIsSimple, edAssociationName.Text, false);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            string error = string.Empty;
            if (!string.IsNullOrEmpty(edAssociationName.Text))
            {
                if (SelectedIsSimple)
                {
                    error = controlSimple.ValidateForm();
                }
                else
                {
                    error = controlAdvanced.ValidateForm();
                }
            }
            else
            {
                error = "Please set association link name.";
            }

            bool isValid = string.IsNullOrEmpty(error);
            if (!isValid)
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;
        }
    }
}
