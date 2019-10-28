using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common.Properties;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormAddAssociationOld : Form
    {
        private readonly IEnumerable<PersistentTypeItem> existingTypeNames;
        private readonly IEnumerable<string> existingAssociations;
        private IconListEntry[] persistentTypesItems;
        private bool DisabledOnPersistentTypeChanged = false;

        public FormAddAssociationOld()
        {
            InitializeComponent();
        }

        internal FormAddAssociationOld(IEnumerable<PersistentTypeItem> existingTypeNames, IEnumerable<string> existingAssociations,
            PersistentTypeItem persistentTypeEnd1)
            : this()
        {
            //this.existingTypeNames = existingTypeNames.Where(type => type.Kind != EntityKind.Structure).ToArray();
            this.existingTypeNames = existingTypeNames.ToArray();
            this.existingAssociations = existingAssociations;
            PopulateForm(persistentTypeEnd1);
        }

        public static bool DialogShow(IEnumerable<PersistentTypeItem> existingTypeNames,
            IEnumerable<string> existingAssociations, PersistentTypeItem persistentTypeEnd1, out ResultData resultData)
        {
            bool result;
            resultData = null;

            using (FormAddAssociationOld form = new FormAddAssociationOld(existingTypeNames, existingAssociations, persistentTypeEnd1))
            {
                result = form.ShowDialog() == DialogResult.OK;
                if (result)
                {
                    resultData = form.GetResultData();
                }
            }

            return result;
        }

        private void PopulateForm(PersistentTypeItem persistentTypeEnd1)
        {
            chCreateEnd1.Checked = true;
            chCreateEnd2.Checked = true;

            BuildPersistentTypesItems();

            PopulateMultiplicity(true);
            PopulateMultiplicity(false);

            PopulatePeristentTypes(true, persistentTypeEnd1);
            PopulatePeristentTypes(false);

            GenerateAssociationName();
            GeneratePropertyNames();

            PopulateOnRemoveActions(true);
            PopulateOnRemoveActions(false);

            chUseAssocEnd1.Checked = true;
            chUseAssocEnd2.Checked = true;
        }

        readonly MultiplicityKind[] orderedMultiplicities = new []
            {
                MultiplicityKind.One, MultiplicityKind.ZeroOrOne, MultiplicityKind.Many
            };

        private void PopulateMultiplicity(bool end1)
        {
            var combo = GetMultiplicityCombo(end1);
            combo.Items.Clear();

            foreach (var multiplicityKind in orderedMultiplicities)
            {
                combo.Items.Add(new IconListEntry(GetMultiplicityDisplayName(multiplicityKind), multiplicityKind,
                    null));
            }

            combo.SelectedIndex = end1 ? 0 : 2;
        }

        private string GetMultiplicityDisplayName(MultiplicityKind multiplicityKind)
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

        private IconComboBox GetMultiplicityCombo(bool end1)
        {
            return end1 ? cmbMultiplicityEnd1 : cmbMultiplicityEnd2;
        }

        private IconComboBox GetPersistentTypeCombo(bool end1)
        {
            return end1 ? cmbPersistentTypesEnd1 : cmbPersistentTypesEnd2;
        }

        private PersistentTypeItem GetSelectedPersistentType(bool end1)
        {
            var combo = GetPersistentTypeCombo(end1);
            if (combo.SelectedItem != null)
            {
                IconListEntry entry = (IconListEntry) combo.SelectedItem;
                return entry.Value as PersistentTypeItem;
            }

            return null;
        }

        private void SelectPersistentType(bool end1, PersistentTypeItem typeToSelect)
        {
            var combo = GetPersistentTypeCombo(end1);
            int idx = 0;
            foreach (var entry in combo.Items.OfType<IconListEntry>())
            {
                PersistentTypeItem item = entry.Value as PersistentTypeItem;
                if (item.EqualsTo(typeToSelect))
                {
                    combo.SelectedIndex = idx;
                    break;
                }

                idx++;
            }
        }

        private int FindPersistentTypeIndex(IconComboBox combo, PersistentTypeItem persistentTypeItem)
        {
            int idx = -1;
            foreach (var entry in combo.Items.OfType<IconListEntry>())
            {
                idx++;
                PersistentTypeItem item = entry.Value as PersistentTypeItem;
                if (item.EqualsTo(persistentTypeItem))
                {
                    break;
                }
            }

            return idx;
        }

        private MultiplicityKind GetSelectedMultiplicity(bool end1)
        {
            var combo = GetMultiplicityCombo(end1);
            IconListEntry entry = (IconListEntry) combo.SelectedItem;
            return (MultiplicityKind) entry.Value;
        }

        private void PopulateOnRemoveActionCombo(IconComboBox comboBox)
        {
            comboBox.Items.Clear();
            int idx = -1;
            int selIdx = 0;
            foreach (var value in EnumType<AssociationOnRemoveAction>.Values)
            {
                idx++;
                comboBox.Items.Add(new IconListEntry(value.ToString(), value, Resources.bullet));
                if (value == AssociationOnRemoveAction.Default)
                {
                    selIdx = idx;
                }
            }

            comboBox.SelectedIndex = selIdx;
        }

        private AssociationOnRemoveAction GetSelectedOnRemoveAction(bool end1, bool onOwner)
        {
            IconComboBox onOwnerCombo = end1 ? cmbOnOwnerRemoveEnd1 : cmbOnOwnerRemoveEnd2;
            IconComboBox onTargetCombo = end1 ? cmbOnTargetRemoveEnd1 : cmbOnTargetRemoveEnd2;
            IconComboBox combo = onOwner ? onOwnerCombo : onTargetCombo;
            return (AssociationOnRemoveAction) (combo.SelectedItem as IconListEntry).Value;
        }

        private void PopulateOnRemoveActions(bool end1)
        {
            IconComboBox onOwnerCombo = end1 ? cmbOnOwnerRemoveEnd1 : cmbOnOwnerRemoveEnd2;
            IconComboBox onTargetCombo = end1 ? cmbOnTargetRemoveEnd1 : cmbOnTargetRemoveEnd2;
            PopulateOnRemoveActionCombo(onOwnerCombo);
            PopulateOnRemoveActionCombo(onTargetCombo);
        }

        private void PopulatePeristentTypes(bool end1)
        {
            PopulatePeristentTypes(end1, null);
        }

        private void PopulatePeristentTypes(bool end1, PersistentTypeItem persistentTypeToSelect)
        {
            PersistentTypeItem currentSelectedPersistentType = GetSelectedPersistentType(end1);
            var combo = GetPersistentTypeCombo(end1);
            var otherCombo = GetPersistentTypeCombo(!end1);
            combo.Items.Clear();
            foreach (var entry in persistentTypesItems)
            {
                combo.Items.Add(entry);
            }

            DisabledOnPersistentTypeChanged = true;

            try
            {
                if (persistentTypeToSelect != null)
                {
                    currentSelectedPersistentType = persistentTypeToSelect;
                }

                if (currentSelectedPersistentType != null)
                {
                    SelectPersistentType(end1, currentSelectedPersistentType);
                }
                else
                {
                    combo.SelectedIndex = end1 ? 0 : 1;
                    if (combo.SelectedIndex == otherCombo.SelectedIndex)
                    {
                        combo.SelectedIndex = combo.SelectedIndex == 0 ? 1 : 0;
                    }
                }
            }
            finally
            {
                DisabledOnPersistentTypeChanged = false;
            }
        }

        private void BuildPersistentTypesItems()
        {
            persistentTypesItems = (from item in existingTypeNames
                                    select new IconListEntry(item.Name, item, GetTypeKindImage(item.Kind)))
                                    .ToArray();
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateForm()
        {
            ResultData resultData = GetResultData();
            string error = null;

            if (string.IsNullOrEmpty(resultData.AssociationName))
            {
                error = "Association Name must not be empty.";
            }
            else if (existingAssociations.Any(item => Util.StringEqual(item, resultData.AssociationName, true)))
            {
                error = string.Format("Entity Model already contains association with name '{0}'.", resultData.AssociationName);
            }
            else if (resultData.PersistentTypeEnd1 == null || resultData.PersistentTypeEnd2 == null)
            {
                string end = resultData.PersistentTypeEnd1 == null ? "End1" : "End2";
                error = string.Format("{0} Persistent Type must be selected.", end);
            }
            else if (string.IsNullOrEmpty(resultData.NavigationPropertyEnd1) || string.IsNullOrEmpty(resultData.NavigationPropertyEnd2))
            {
                string end = string.IsNullOrEmpty(resultData.NavigationPropertyEnd1) ? "End1" : "End2";
                error = string.Format("{0} Navigation Property must not be empty.", end);
            }
            else
            {
                string typeName = null;
                string propName = null;

                if (resultData.PersistentTypeEnd1.Properties.Any(
                    prop => Util.StringEqual(prop, resultData.NavigationPropertyEnd1, true)))
                {
                    typeName = resultData.PersistentTypeEnd1.Name;
                    propName = resultData.NavigationPropertyEnd1;
                }
                else if (resultData.PersistentTypeEnd2.Properties.Any(
                    prop => Util.StringEqual(prop, resultData.NavigationPropertyEnd2, true)))
                {
                    typeName = resultData.PersistentTypeEnd2.Name;
                    propName = resultData.NavigationPropertyEnd2;
                }

                if (!string.IsNullOrEmpty(typeName))
                {
                    error = string.Format("Persistent Type '{0}' already contains property with name '{1}'.",
                        typeName, propName);
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private ResultData GetResultData()
        {
            PersistentTypeItem persistentTypeEnd1 = GetSelectedPersistentType(true);
            PersistentTypeItem persistentTypeEnd2 = GetSelectedPersistentType(false);

            AssociationOnRemoveAction onOwnerRemoveActionEnd1 = GetSelectedOnRemoveAction(true, true);
            AssociationOnRemoveAction onOwnerRemoveActionEnd2 = GetSelectedOnRemoveAction(false, true);

            AssociationOnRemoveAction onTargetRemoveActionEnd1 = GetSelectedOnRemoveAction(true, false);
            AssociationOnRemoveAction onTargetRemoveActionEnd2 = GetSelectedOnRemoveAction(false, false);

            MultiplicityKind multiplicityEnd1 = GetSelectedMultiplicity(true);
            MultiplicityKind multiplicityEnd2 = GetSelectedMultiplicity(false);

            string navigationPropertyEnd1 = edNavPropertyEnd1.Text;
            string navigationPropertyEnd2 = edNavPropertyEnd2.Text;

            string associationName = edAssociationName.Text;
            return new ResultData(persistentTypeEnd1, persistentTypeEnd2, multiplicityEnd1, multiplicityEnd2,
                navigationPropertyEnd1, navigationPropertyEnd2, associationName,
                onOwnerRemoveActionEnd1, onOwnerRemoveActionEnd2, onTargetRemoveActionEnd1, onTargetRemoveActionEnd2,
                chCreateEnd1.Checked, chCreateEnd2.Checked, chUseAssocEnd1.Checked, chUseAssocEnd2.Checked);
        }

        #region class ResultData

        public class ResultData
        {
            public PersistentTypeItem PersistentTypeEnd1 { get; private set; }
            public PersistentTypeItem PersistentTypeEnd2 { get; private set; }
            
            public AssociationOnRemoveAction OnOwnerRemoveEnd1 { get; private set; }
            public AssociationOnRemoveAction OnOwnerRemoveEnd2 { get; private set; }

            public AssociationOnRemoveAction OnTargetRemoveEnd1 { get; private set; }
            public AssociationOnRemoveAction OnTargetRemoveEnd2 { get; private set; }

            public MultiplicityKind MultiplicityEnd1 { get; private set; }
            public MultiplicityKind MultiplicityEnd2 { get; private set; }

            public string NavigationPropertyEnd1 { get; private set; }
            public string NavigationPropertyEnd2 { get; private set; }

            public string AssociationName { get; private set; }

            //public bool LookupToItself { get; set; }

            public bool CreatePropertyEnd1 { get; private set; }
            public bool CreatePropertyEnd2 { get; private set; }

            public bool UseAssociationAttributeEnd1 { get; private set; }
            public bool UseAssociationAttributeEnd2 { get; private set; }

            public ResultData(PersistentTypeItem persistentTypeEnd1, PersistentTypeItem persistentTypeEnd2,
                MultiplicityKind multiplicityEnd1, MultiplicityKind multiplicityEnd2, string navigationPropertyEnd1,
                string navigationPropertyEnd2, string associationName,
                AssociationOnRemoveAction onOwnerRemoveEnd1, AssociationOnRemoveAction onOwnerRemoveEnd2,
                AssociationOnRemoveAction onTargetRemoveEnd1, AssociationOnRemoveAction onTargetRemoveEnd2,
                bool createPropertyEnd1, bool createPropertyEnd2,
                bool useAssociationAttributeEnd1, bool useAssociationAttributeEnd2)
            {
                PersistentTypeEnd1 = persistentTypeEnd1;
                PersistentTypeEnd2 = persistentTypeEnd2;
                MultiplicityEnd1 = multiplicityEnd1;
                MultiplicityEnd2 = multiplicityEnd2;
                NavigationPropertyEnd1 = navigationPropertyEnd1;
                NavigationPropertyEnd2 = navigationPropertyEnd2;
                AssociationName = associationName;
                this.OnOwnerRemoveEnd1 = onOwnerRemoveEnd1;
                this.OnOwnerRemoveEnd2 = onOwnerRemoveEnd2;
                this.OnTargetRemoveEnd1 = onTargetRemoveEnd1;
                this.OnTargetRemoveEnd2 = onTargetRemoveEnd2;
                this.CreatePropertyEnd1 = createPropertyEnd1;
                this.CreatePropertyEnd2 = createPropertyEnd2;
                this.UseAssociationAttributeEnd1 = useAssociationAttributeEnd1;
                this.UseAssociationAttributeEnd2 = useAssociationAttributeEnd2;
            }
        }

        #endregion class ResultData

        private void OnPersistentTypeChanged(object sender, EventArgs e)
        {
            if (DisabledOnPersistentTypeChanged)
            {
                return;
            }

            bool isEnd1Combo = sender == cmbPersistentTypesEnd1;
            var comboA = isEnd1Combo ? cmbPersistentTypesEnd1 : cmbPersistentTypesEnd2;
            var comboB = isEnd1Combo ? cmbPersistentTypesEnd2 : cmbPersistentTypesEnd1;
            var editA = isEnd1Combo ? edNavPropertyEnd1 : edNavPropertyEnd2;
            var editB = isEnd1Combo ? edNavPropertyEnd2 : edNavPropertyEnd1;
            var persistentTypeA = GetSelectedPersistentType(isEnd1Combo);
            var persistentTypeB = GetSelectedPersistentType(!isEnd1Combo);

            string generatedPropertyName = GeneratePropertyName(persistentTypeA, persistentTypeB);
            if (editA.Text == generatedPropertyName)
            {
                generatedPropertyName = generatedPropertyName + "_1";
            }
            editB.Text = generatedPropertyName;

            PopulatePeristentTypes(!isEnd1Combo);
            GenerateAssociationName();
        }

        private void GenerateAssociationName()
        {
            var type1 = GetSelectedPersistentType(true);
            var type2 = GetSelectedPersistentType(false);

            string assocName = string.Format("{0}{1}", type1.Name, type2.Name);
            edAssociationName.Text = Util.GenerateUniqueName(existingAssociations, assocName);
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

            edNavPropertyEnd1.Text = generatePropertyNameA;
            edNavPropertyEnd2.Text = generatePropertyNameB;
        }

        private string GeneratePropertyName(PersistentTypeItem source, PersistentTypeItem target)
        {
            return Util.GenerateUniqueName(target.Properties, source.Name);
        }

//        private void chLookup_CheckedChanged(object sender, EventArgs e)
//        {
//            UpdateEnd2Controls();
//        }

//        private void UpdateEnd2Controls()
//        {
//            bool end2Enabled = !chLookup.Checked;
////            cmbPersistentTypesEnd2.Enabled = end2Enabled;
////            cmbMultiplicityEnd2.Enabled = end2Enabled;
////            edNavPropertyEnd2.Enabled = end2Enabled;
////            cmbOnOwnerRemoveEnd2.Enabled = end2Enabled;
////            cmbOnTargetRemoveEnd2.Enabled = end2Enabled;
////            grEnd2.Enabled = end2Enabled;
//            UpdateEndControls(false, true, end2Enabled);
//        }

        private void UpdateEndControls(bool end1, bool updateCreateEndCheckbox, bool enabled)
        {
            if (end1)
            {
                cmbPersistentTypesEnd1.Enabled = enabled;
                cmbMultiplicityEnd1.Enabled = enabled;
                edNavPropertyEnd1.Enabled = enabled;
                cmbOnOwnerRemoveEnd1.Enabled = enabled;
                cmbOnTargetRemoveEnd1.Enabled = enabled;
                grEnd1.Enabled = enabled;

//                if (updateCreateEndCheckbox)
//                {
//                    chCreateEnd1.Enabled = enabled;
//                }
            }
            else
            {
                cmbPersistentTypesEnd2.Enabled = enabled;
                cmbMultiplicityEnd2.Enabled = enabled;
                edNavPropertyEnd2.Enabled = enabled;
                cmbOnOwnerRemoveEnd2.Enabled = enabled;
                cmbOnTargetRemoveEnd2.Enabled = enabled;
                grEnd2.Enabled = enabled;

//                if (updateCreateEndCheckbox)
//                {
//                    chCreateEnd2.Enabled = enabled;
//                }
            }
        }

        private void CreateEndCheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (sender as CheckBox);
            bool end1 = checkBox == chCreateEnd1;
            //CheckBox otherCheckBox = end1 ? chCreateEnd2 : chCreateEnd1;
            if (!chCreateEnd1.Checked && !chCreateEnd2.Checked)
            {
                checkBox.Checked = true;
            }

            //chLookup.Enabled = chCreateEnd1.Enabled && chCreateEnd1.Checked;

            UpdateEndControls(end1, false, checkBox.Checked);
        }
    }

    #region class PersistentTypeItem

    public class PersistentTypeItem
    {
        public string Name { get; set; }
        public EntityKind Kind { get; set; }
        public string[] Properties { get; set; }

        public PersistentTypeItem(string name, EntityKind kind, string[] properties)
        {
            Name = name;
            Kind = kind;
            this.Properties = properties;
        }

        public bool EqualsTo(PersistentTypeItem other)
        {
            if (other == null)
            {
                return false;
            }

            return Util.StringEqual(this.Name, other.Name, true) &&
                   this.Kind == other.Kind;
        }
    }

    #endregion class PersistentTypeItem
}
