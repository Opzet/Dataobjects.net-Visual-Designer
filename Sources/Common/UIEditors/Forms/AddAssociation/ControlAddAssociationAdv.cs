using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class ControlAddAssociationAdv : UserControl
    {
        private const string VARIABLE_PERSISTENT_TYPE_END1 = "{PersistentType1}";
        private const string VARIABLE_PERSISTENT_TYPE_END2 = "{PersistentType2}";
        private const string VARIABLE_PROPERTY_NAME_END1 = "{PropertyName1}";
        private const string VARIABLE_PROPERTY_NAME_END2 = "{PropertyName2}";
        private const string VARIABLE_PROPERTY_TYPE_END1 = "{PropertyType1}";
        private const string VARIABLE_PROPERTY_TYPE_END2 = "{PropertyType2}";
        private const string VARIABLE_ASSOCIATION_ATTRIBUTE_END1 = "{AssociationAttribute1}";
        private const string VARIABLE_ASSOCIATION_ATTRIBUTE_END2 = "{AssociationAttribute2}";
        private const string VARIABLE_ENTITYSET = "EntitySet&lt;{0}&gt;";

        private const string HTML_ASSOCIATION_ATTRIBUTE_END1 = "[Association( PairTo = <span color=Purple>\"{PropertyName2}\"</span> )]";
        private const string HTML_ASSOCIATION_ATTRIBUTE_END2 = "[Association( PairTo = <span color=Purple>\"{PropertyName1}\"</span> )]";

        private FormAddAssociation ownerForm;
        private readonly string originalPreviewHtml;



        private readonly Dictionary<IconComboBox, bool> disableComboChangeNotification = new Dictionary<IconComboBox, bool>();
        private bool disablePreviewUpdate = false;
        private IEnumerable<IconListEntry> persistentTypesEntries;
        private bool isValidToShow;
        private string showValidationError = string.Empty;

        public ControlAddAssociationAdv()
        {
            InitializeComponent();
            htmlHint.AutoScroll = false;
            htmlPreview.AutoScroll = false;
            originalPreviewHtml = htmlPreview.Text;
        }

        internal void Initialize(FormAddAssociation ownerForm, PersistentTypeItem persistentTypeToSelect)
        {
            this.ownerForm = ownerForm;
            PopulateForm(persistentTypeToSelect);
        }

        private void PopulateForm(PersistentTypeItem persistentTypeToSelect)
        {
            disablePreviewUpdate = true;
            try
            {
                persistentTypesEntries = ownerForm.BuildPersistentTypesItems().Where(
                    entry => ((PersistentTypeItem) entry.Value).Kind != EntityKind.Structure);

                isValidToShow = persistentTypesEntries.Count() >= 2;
                if (!isValidToShow)
                {
                    showValidationError = "There must be at least 2 persistent types (exclude structures) to make advanced association.";
                    return;
                }

                disableComboChangeNotification.Add(cmbPersistentTypesEnd1, false);
                disableComboChangeNotification.Add(cmbPersistentTypesEnd2, false);

                PersistentTypeItem selectedPersistentType = GetSelectedPersistentType(true);
                PersistentTypeItem selectedPropertyType = GetSelectedPersistentType(false);

                PopulatePersistentType(true, selectedPersistentType);
                PopulatePersistentType(false, selectedPropertyType);
                GetPersistentTypeCombo(false).SelectedIndex = 1;

                PopulateMultiplicity(true);
                PopulateMultiplicity(false);

                GetUseAssociationAttributeCheckbox(true).Checked = true;
                GetUseAssociationAttributeCheckbox(false).Checked = true;

                if (persistentTypeToSelect != null)
                {
                    SelectPersistentType(GetPersistentTypeCombo(true), persistentTypeToSelect);
                }

                GenerateAssociationName();
                GeneratePropertyNames();
            }
            finally
            {
                disablePreviewUpdate = false;
            }

            UpdatePreview();
        }

        private void CheckLabelDecorateWithAssocation(object sender, EventArgs e)
        {
            bool end1 = (sender as Control).Tag == "0";
            CheckBox checkBox = GetUseAssociationAttributeCheckbox(end1);
            checkBox.Checked = !checkBox.Checked;
        }

        private void GenerateAssociationName()
        {
            var selectedPersistentTypeEnd1 = GetSelectedPersistentType(true);
            var selectedPersistentTypeEnd2 = GetSelectedPersistentType(false);
            string associationName = string.Format("{0}{1}", selectedPersistentTypeEnd1.Name, selectedPersistentTypeEnd2.Name);

            ownerForm.SetAssociationName(false,
                Util.GenerateUniqueName(ownerForm.existingAssociations, associationName));
        }

        private void GeneratePropertyNames()
        {
            PersistentTypeItem source = GetSelectedPersistentType(true);
            PersistentTypeItem target = GetSelectedPersistentType(false);
            string generatePropertyNameA = GeneratePropertyName(target, source);
            string generatePropertyNameB = GeneratePropertyName(source, target);
            if (generatePropertyNameB == generatePropertyNameA)
            {
                generatePropertyNameB = generatePropertyNameB + "_1";
            }

            edPropertyNameEnd1.Text = generatePropertyNameA;
            edPropertyNameEnd2.Text = generatePropertyNameB;
        }

        private string GeneratePropertyName(PersistentTypeItem source, PersistentTypeItem target)
        {
            return Util.GenerateUniqueName(target.Properties, source.Name);
        }

        private CheckBox GetUseAssociationAttributeCheckbox(bool end1)
        {
            return end1 ? chUseAssocAttrEnd1 : chUseAssocAttrEnd2;
        }

        private IconComboBox GetPersistentTypeCombo(bool end1)
        {
            return end1 ? cmbPersistentTypesEnd1 : cmbPersistentTypesEnd2;
        }

        private IconComboBox GetMultiplicityCombo(bool end1)
        {
            return end1 ? cmbMultiplicityEnd1 : cmbMultiplicityEnd2;
        }

        private PersistentTypeItem GetSelectedPersistentType(bool end1)
        {
            IconComboBox comboBox = GetPersistentTypeCombo(end1);
            IconListEntry selectedItem = comboBox.SelectedItem as IconListEntry;
            return (selectedItem != null ? selectedItem.Value : null) as PersistentTypeItem;
        }

        private MultiplicityKind GetSelectedMultiplicity(bool end1)
        {
            var combo = GetMultiplicityCombo(end1);
            IconListEntry entry = (IconListEntry)combo.SelectedItem;
            return (MultiplicityKind)entry.Value;
        }


        private void PopulateMultiplicity(bool end1)
        {
            var combo = GetMultiplicityCombo(end1);
            combo.Items.Clear();

            foreach (var multiplicityKind in FormAddAssociation.orderedMultiplicities)
            {
                combo.Items.Add(new IconListEntry(FormAddAssociation.GetMultiplicityDisplayName(multiplicityKind), multiplicityKind,
                    null));
            }

            combo.SelectedIndex = 0; // end1 ? 0 : 2;
        }

        private void PopulatePersistentType(bool end1, PersistentTypeItem itemToSelect = null)
        {
            IconComboBox comboBox = GetPersistentTypeCombo(end1);

            comboBox.Items.Clear();
            foreach (var entry in ownerForm.BuildPersistentTypesItems().Where(entry => ((PersistentTypeItem)entry.Value).Kind!= EntityKind.Structure))
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

        private void UpdatePreview()
        {
            if (disablePreviewUpdate)
            {
                return;
            }

            PersistentTypeItem selectedPersistentTypeEnd1 = GetSelectedPersistentType(true);
            string persistentTypeEnd1 = selectedPersistentTypeEnd1 == null ? VARIABLE_PERSISTENT_TYPE_END1 : selectedPersistentTypeEnd1.Name;


            PersistentTypeItem selectedPersistentTypeEnd2 = GetSelectedPersistentType(false);
            string persistentTypeEnd2 = selectedPersistentTypeEnd2 == null ? VARIABLE_PERSISTENT_TYPE_END2 : selectedPersistentTypeEnd2.Name;

            string propertyNameEnd1 = string.IsNullOrEmpty(edPropertyNameEnd1.Text)
                                      ? VARIABLE_PROPERTY_NAME_END1
                                      : edPropertyNameEnd1.Text;

            string propertyNameEnd2 = string.IsNullOrEmpty(edPropertyNameEnd2.Text)
                                      ? VARIABLE_PROPERTY_NAME_END2
                                      : edPropertyNameEnd2.Text;

            MultiplicityKind selectedMultiplicityEnd1 = GetSelectedMultiplicity(true);
            MultiplicityKind selectedMultiplicityEnd2 = GetSelectedMultiplicity(false);

            string propertyTypeEnd1 = selectedMultiplicityEnd1 == MultiplicityKind.Many
                                          ? string.Format(VARIABLE_ENTITYSET, persistentTypeEnd2)
                                          : persistentTypeEnd2;

            string propertyTypeEnd2 = selectedMultiplicityEnd2 == MultiplicityKind.Many
                                          ? string.Format(VARIABLE_ENTITYSET, persistentTypeEnd1)
                                          : persistentTypeEnd1;

            bool useAssocEnd1 = GetUseAssociationAttributeCheckbox(true).Checked;
            bool useAssocEnd2 = GetUseAssociationAttributeCheckbox(false).Checked;
            string useAssociationEnd1 = useAssocEnd1 ? HTML_ASSOCIATION_ATTRIBUTE_END1.Replace(VARIABLE_PROPERTY_NAME_END2, propertyNameEnd2) : "<br>";
            string useAssociationEnd2 = useAssocEnd2 ? HTML_ASSOCIATION_ATTRIBUTE_END2.Replace(VARIABLE_PROPERTY_NAME_END1, propertyNameEnd1) : "<br>";

            string html = originalPreviewHtml.Replace(VARIABLE_PERSISTENT_TYPE_END1, persistentTypeEnd1);
            html = html.Replace(VARIABLE_PERSISTENT_TYPE_END2, persistentTypeEnd2);
            
            html = html.Replace(VARIABLE_PROPERTY_NAME_END1, propertyNameEnd1);
            html = html.Replace(VARIABLE_PROPERTY_NAME_END2, propertyNameEnd2);

            html = html.Replace(VARIABLE_PROPERTY_TYPE_END1, propertyTypeEnd1);
            html = html.Replace(VARIABLE_PROPERTY_TYPE_END2, propertyTypeEnd2);
            
            html = html.Replace(VARIABLE_ASSOCIATION_ATTRIBUTE_END1, useAssociationEnd1);
            html = html.Replace(VARIABLE_ASSOCIATION_ATTRIBUTE_END2, useAssociationEnd2);


            htmlPreview.Text = html;
        }

        private void OnPersistentTypeSelectionChanged(object sender, EventArgs e)
        {
            IconComboBox comboBox = sender as IconComboBox;
            if (disableComboChangeNotification[comboBox])
            {
                return;
            }

            UpdatePreview();
            GenerateAssociationName();
            GeneratePropertyNames();
        }

        private void OnPropertyNameTextChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void OnMultiplicitySelectionChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void OnUseAssociationCheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        public string GetPreviewHtml()
        {
            return htmlPreview.Text;
        }

        public FormAddAssociation.ResultData.PersistentItem GetResultPersistentItem(bool firstItem)
        {
            string propertyName = firstItem ? edPropertyNameEnd1.Text : edPropertyNameEnd2.Text;
            return new FormAddAssociation.ResultData.PersistentItem(GetSelectedPersistentType(firstItem),
                propertyName, GetSelectedMultiplicity(firstItem), GetUseAssociationAttributeCheckbox(firstItem).Checked);
        }

        public bool CanShow()
        {
            if (!isValidToShow)
            {
                MessageBox.Show(showValidationError, "Advanced Association", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValidToShow;
        }

        public string ValidateForm()
        {
            if (GetSelectedPersistentType(true) == null)
            {
                return "Please select 1st persistent type.";
            }

            if (GetSelectedPersistentType(false) == null)
            {
                return "Please select 2nd persistent type.";
            }

            if (string.IsNullOrEmpty(edPropertyNameEnd1.Text))
            {
                return "Please set navigation property name for 1st persistent type.";
            }

            if (string.IsNullOrEmpty(edPropertyNameEnd2.Text))
            {
                return "Please set navigation property name for 2nd persistent type.";
            }

            if (GetSelectedPersistentType(true).EqualsTo(GetSelectedPersistentType(false)) &&
                Util.StringEqual(edPropertyNameEnd1.Text, edPropertyNameEnd2.Text, true))
            {
                return "1st persistent type and 2nd persistent type points to same type with same property names.";
            }

            return null;
        }
    }
}
