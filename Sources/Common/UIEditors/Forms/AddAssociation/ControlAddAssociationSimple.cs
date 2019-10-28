using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class ControlAddAssociationSimple : UserControl
    {
        private const string VARIABLE_PERSISTENT_TYPE = "{PersistentType}";
        private const string VARIABLE_PROPERTY_TYPE = "{PropertyType}";
        private const string VARIABLE_PROPERTY_NAME = "{PropertyName}";
        private const string VARIABLE_ENTITYSET = "EntitySet&lt;{0}&gt;";

        private FormAddAssociation ownerForm;
        private readonly Dictionary<IconComboBox, bool> disableComboChangeNotification = new Dictionary<IconComboBox, bool>();
        private readonly string originalPreviewHtml;
        private bool disablePreviewUpdate = false;

        public ControlAddAssociationSimple()
        {
            InitializeComponent();
            htmlPreview.AutoScroll = false;
            originalPreviewHtml = htmlPreview.Text;
        }

        internal void Initialize(FormAddAssociation ownerForm, PersistentTypeItem persistentTypeToSelect)
        {
            this.ownerForm = ownerForm;
            PopulatePersistentTypesCombos(persistentTypeToSelect);
        }

        private void PopulatePersistentTypesCombos(PersistentTypeItem persistentTypeToSelect)
        {
            disablePreviewUpdate = true;
            try
            {
                disableComboChangeNotification.Add(cmbPersistentTypes, false);
                disableComboChangeNotification.Add(cmbPropertyTypes, false);

                PersistentTypeItem selectedPersistentType = SelectedPersistentType;
                PersistentTypeItem selectedPropertyType = SelectedPropertyType;

                PopulatePersistentType(cmbPersistentTypes, /*selectedPropertyType, */selectedPersistentType);
                PopulatePersistentType(cmbPropertyTypes, /*selectedPersistentType, */selectedPropertyType);
                cmbPropertyTypes.SelectedIndex = 1;

                if (persistentTypeToSelect != null)
                {
                    SelectPersistentType(cmbPersistentTypes, persistentTypeToSelect);
                }

                GenerateAssociationName();
                GeneratePropertyNames();

                PopulateMultiplicity();

                UpdateControlNavigationToItSelf();
            }
            finally
            {
                disablePreviewUpdate = false;
                UpdatePreview();
            }
        }

        public PersistentTypeItem SelectedPersistentType
        {
            get
            {
                IconListEntry selectedItem = cmbPersistentTypes.SelectedItem as IconListEntry;
                return (selectedItem != null ? selectedItem.Value : null) as PersistentTypeItem;
            }
        }

        public PersistentTypeItem SelectedPropertyType
        {
            get
            {
                IconListEntry selectedItem = cmbPropertyTypes.SelectedItem as IconListEntry;
                return (selectedItem != null ? selectedItem.Value : null) as PersistentTypeItem;
            }
        }

        private MultiplicityKind SelectedMultiplicity
        {
            get
            {
                IconListEntry entry = (IconListEntry)cmbMultiplicity.SelectedItem;
                return (MultiplicityKind)entry.Value;
            }
        }

        private void GenerateAssociationName()
        {
            var selectedPersistentType = SelectedPersistentType;
            var selectedPropertyType = SelectedPropertyType;
            string associationName = string.Format("{0}{1}", selectedPersistentType.Name, selectedPropertyType.Name);

            ownerForm.SetAssociationName(true,
                Util.GenerateUniqueName(ownerForm.existingAssociations, associationName));
        }

        private void GeneratePropertyNames()
        {
            edPropertyName.Text = GeneratePropertyName(SelectedPropertyType, SelectedPersistentType);
        }

        private string GeneratePropertyName(PersistentTypeItem source, PersistentTypeItem target)
        {
            return Util.GenerateUniqueName(target.Properties, source.Name);
        }

        private void PopulateMultiplicity()
        {
            cmbMultiplicity.Items.Clear();

            foreach (var multiplicityKind in FormAddAssociation.orderedMultiplicities)
            {
                cmbMultiplicity.Items.Add(new IconListEntry(FormAddAssociation.GetMultiplicityDisplayName(multiplicityKind), multiplicityKind,
                    null));
            }

            cmbMultiplicity.SelectedIndex = 0;
        }

        private void PopulatePersistentType(IconComboBox comboBox, /*PersistentTypeItem exceptItem, */PersistentTypeItem itemToSelect = null)
        {
            comboBox.Items.Clear();
            foreach (var entry in ownerForm.BuildPersistentTypesItems())
            {
                comboBox.Items.Add(entry);
            }

            disableComboChangeNotification[comboBox] = true;
            try
            {
                if (itemToSelect != null)
                {
                    SelectPersistentType(comboBox, itemToSelect);
                }

                if (comboBox.SelectedIndex == -1)
                {
                    comboBox.SelectedIndex = 0;
                }
            }
            finally
            {
                disableComboChangeNotification[comboBox] = false;
            }
        }

        private void SelectPersistentType(IconComboBox comboBox, PersistentTypeItem itemToSelect)
        {
            int idx = 0;
            foreach (var entry in comboBox.Items.OfType<IconListEntry>())
            {
                PersistentTypeItem item = entry.Value as PersistentTypeItem;
                if (item.EqualsTo(itemToSelect))
                {
                    comboBox.SelectedIndex = idx;
                    break;
                }

                idx++;
            }
        }

        private void htmlNavigationToItSelf_Click(object sender, EventArgs e)
        {
            chNavigationToItSelf.Checked = !chNavigationToItSelf.Checked;
        }

        private void PersistentTypeComboSelectionChanged(object sender, EventArgs e)
        {
            IconComboBox comboBox = (sender as IconComboBox);
            if (disableComboChangeNotification[comboBox])
            {
                return;
            }

            bool isPersistentCombo = (sender as Control).Tag == "0";
            if (!isPersistentCombo)
            {
                PersistentTypeItem selectedPersistentType = SelectedPersistentType;
                PersistentTypeItem selectedPropertyType = SelectedPropertyType;
                bool isItself = false;
                if (selectedPersistentType != null && selectedPropertyType != null)
                {
                    isItself = selectedPersistentType.EqualsTo(selectedPropertyType);
                }

                chNavigationToItSelf.Checked = isItself;
            }
            UpdateControlNavigationToItSelf();
            UpdatePreview();
            GenerateAssociationName();
            GeneratePropertyNames();
        }

        private void chNavigationToItSelf_CheckedChanged(object sender, EventArgs e)
        {
            PersistentTypeItem selectedPersistentType = SelectedPersistentType;
            PersistentTypeItem selectedPropertyType = SelectedPropertyType;

            bool isItself = false;
            if (selectedPersistentType != null && selectedPropertyType != null)
            {
                isItself = selectedPersistentType.EqualsTo(selectedPropertyType);
            }

            if (chNavigationToItSelf.Checked && !isItself)
            {
                SelectPersistentType(cmbPropertyTypes, selectedPersistentType);
            }
            
            UpdateControlNavigationToItSelf();
            UpdatePreview();
        }

        private void UpdateControlNavigationToItSelf()
        {
            cmbPropertyTypes.Enabled = !chNavigationToItSelf.Checked;
        }

        private void UpdatePreview()
        {
            if (disablePreviewUpdate)
            {
                return;
            }

            PersistentTypeItem selectedPersistentType = SelectedPersistentType;
            string persistentType = selectedPersistentType == null ? VARIABLE_PERSISTENT_TYPE : selectedPersistentType.Name;
            
            PersistentTypeItem selectedPropertyType = SelectedPropertyType;
            string propertyType = selectedPropertyType == null ? VARIABLE_PROPERTY_TYPE : selectedPropertyType.Name;

            string propertyName = string.IsNullOrEmpty(edPropertyName.Text)
                                      ? VARIABLE_PROPERTY_NAME
                                      : edPropertyName.Text;

            MultiplicityKind selectedMultiplicity = SelectedMultiplicity;
            propertyType = selectedMultiplicity == MultiplicityKind.Many
                                          ? string.Format(VARIABLE_ENTITYSET, propertyType)
                                          : propertyType;

            string html = originalPreviewHtml.Replace(VARIABLE_PERSISTENT_TYPE, persistentType);
            html = html.Replace(VARIABLE_PROPERTY_TYPE, propertyType);
            html = html.Replace(VARIABLE_PROPERTY_NAME, propertyName);

            htmlPreview.Text = html;
        }

        private void edPropertyName_TextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void cmbMultiplicity_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        public string GetPreviewHtml()
        {
            return htmlPreview.Text;
        }

        public FormAddAssociation.ResultData.PersistentItem GetResultPersistentItem(bool firstItem)
        {
            PersistentTypeItem typeItem = firstItem ? SelectedPersistentType : SelectedPropertyType;
            return new FormAddAssociation.ResultData.PersistentItem(typeItem, 
                edPropertyName.Text, SelectedMultiplicity, false);
        }

        public bool CanShow()
        {
            return true;
        }

        public string ValidateForm()
        {
            if (SelectedPersistentType == null)
            {
                return "Please select persistent type.";
            }

            if (string.IsNullOrEmpty(edPropertyName.Text))
            {
                return "Please set navigation property name.";
            }

            if (SelectedPropertyType == null)
            {
                return "Please select type of property type.";
            }

            if (SelectedPersistentType.Kind == EntityKind.Structure &&
                SelectedPropertyType.Kind == EntityKind.Structure)
            {
                return
                    "Source persistent type and type of property are both of kind Structure which is not allowed for navigation property.\n\rInstead use 'Add structure property' feature.";
            }

            return null;
        }
    }
}
