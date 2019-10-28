using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TXSoftware.DataObjectsNetEntityModel.Common.Properties;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms
{
    public partial class FormAddPersistentType : Form
    {
        #region class ResultData

        public class ResultData
        {
            public EntityKind TypeKind { get; private set; }
            public string TypeName { get; private set; }
            public Tuple<EntityKind?, string> BaseTypeInfo { get; private set; }
            // item1 - create, item2 - property name, item3 - property type
            public Tuple<bool, string, Type> KeyPropertyInfo { get; private set; }

            public ResultData(EntityKind typeKind, string typeName, Tuple<EntityKind?, string> baseTypeInfo, 
                Tuple<bool, string, Type> keyPropertyInfo)
            {
                TypeKind = typeKind;
                TypeName = typeName;
                BaseTypeInfo = baseTypeInfo;
                KeyPropertyInfo = keyPropertyInfo;
            }
        }

        #endregion class ResultData

        readonly IEnumerable<Tuple<string, EntityKind>> typeNames;
        //readonly SystemPrimitiveTypesConverter converter = new SystemPrimitiveTypesConverter();

        public FormAddPersistentType()
        {
            InitializeComponent();
        }

        public FormAddPersistentType(IEnumerable<Tuple<string, EntityKind>> existingTypeNames)
            : this()
        {
            this.typeNames = existingTypeNames;
            edEntityName.Text = GenerateTypeName();

            PopulateForm();
        }

        public static bool DialogShow(IEnumerable<Tuple<string, EntityKind>> existingTypeNames, out ResultData resultData)
        {
            resultData = null;
            FormAddPersistentType form = new FormAddPersistentType(existingTypeNames);
            DialogResult dialogResult = form.ShowDialog();
            bool result = dialogResult == DialogResult.OK;
            if (result)
            {
                resultData = form.GetResultData();
            }

            return result;
        }

        private ResultData GetResultData()
        {
            var baseTypeInfo = SelectedBaseType;
            bool createProperty = chCreateKey.Checked && chCreateKey.Enabled;
            var keyPropertyInfo = new Tuple<bool, string, Type>(createProperty, edPropertyName.Text, SelectedPropertyType);
            return new ResultData(SelectedTypeKind, edEntityName.Text, baseTypeInfo, keyPropertyInfo);
        }

        private void PopulateForm()
        {
            PopulateTypes();
            PopulateBaseTypes();
            PopulatePropertyTypes();
            UpdateKeyPropertyGroupBox();
        }

        private void PopulateTypes()
        {
            cmbPersistentTypes.Items.Clear();
            cmbPersistentTypes.Items.Add(new IconListEntry("Entity", EntityKind.Entity, Resources.Entity));
            cmbPersistentTypes.Items.Add(new IconListEntry("Interface", EntityKind.Interface, Resources.Interface));
            cmbPersistentTypes.Items.Add(new IconListEntry("Structure", EntityKind.Structure, Resources.Structure));

            cmbPersistentTypes.SelectedIndex = 0;
        }

        private void PopulateBaseTypes()
        {
            cmbBaseTypes.Items.Clear();
            cmbBaseTypes.Items.Add(new IconListEntry("(None)", null, null));
            EntityKind selectedTypeKind = SelectedTypeKind;
            foreach (var pair in typeNames.Where(tuple => tuple.Item2 == selectedTypeKind))
            {
                string typeName = pair.Item1;
                EntityKind typeKind = pair.Item2;
                cmbBaseTypes.Items.Add(new IconListEntry(typeName, typeKind, GetTypeKindImage(typeKind)));
            }

            cmbBaseTypes.SelectedIndex = 0;
        }

        private void PopulatePropertyTypes()
        {
            cmbPropertyTypes.Items.Clear();
            int idx = -1;

            foreach (var type in SystemPrimitiveTypesConverter.Types)
            {
                cmbPropertyTypes.Items.Add(new IconListEntry(type.Name, type, Resources.bullet));
                if (type == typeof(Int32))
                {
                    idx = cmbPropertyTypes.Items.Count - 1;
                }
            }

            cmbPropertyTypes.SelectedIndex = idx;
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

        private Type SelectedPropertyType
        {
            get
            {
                IconListEntry entry = (IconListEntry) cmbPropertyTypes.SelectedItem;
//                string displayName = SystemPrimitiveTypesConverter.GetDisplayName(entry.Text);
//                return SystemPrimitiveTypesConverter.GetClrType(displayName) ?? typeof (string);
                return (Type) entry.Value;
            }
        }

        private Tuple<EntityKind?, string> SelectedBaseType
        {
            get
            {
                if (cmbBaseTypes.SelectedIndex == 0)
                {
                    return new Tuple<EntityKind?, string>(null, null);
                }

                IconListEntry entry = (IconListEntry) cmbBaseTypes.SelectedItem;
                EntityKind? entityKind = (EntityKind?) entry.Value;
                return new Tuple<EntityKind?, string>(entityKind, entry.Text);
            }
        }

        private EntityKind SelectedTypeKind
        {
            get
            {
                IconListEntry entry = (IconListEntry) cmbPersistentTypes.SelectedItem;
                return (EntityKind) entry.Value;
            }
        }

        private void AddEntityForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(edEntityName.Text))
                {
                    MessageBox.Show("The EntityType name is not valid.");
                    e.Cancel = true;
                }
            }
        }

        private string GenerateTypeName()
        {
            var matchingEntities = this.typeNames.Select(item => item.Item1).Where(e => Regex.Match(e, "Entity[0-9]+").Success);
            var maxIndex = (matchingEntities.Count() > 0) 
                ? matchingEntities.Select(e => int.Parse(e.Substring(6))).Max() 
                : 0;
            
            return string.Format("Entity{0}", maxIndex + 1);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (chCreateKey.Enabled && chCreateKey.Checked && string.IsNullOrEmpty(edPropertyName.Text))
            {
                MessageBox.Show("Property Name cannot be empty!", "Missing property name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrEmpty(edEntityName.Text))
            {
                MessageBox.Show("Type Name cannot be empty!", "Missing type name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.None;
                return;
            }
        }

        private void cmbPersistentTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBaseTypes();
        }

        private void cmbBaseTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateKeyPropertyGroupBox();
        }

        private void UpdateKeyPropertyGroupBox()
        {
            bool noBaseType = SelectedBaseType.Item1 == null;
            bool createKey = chCreateKey.Checked;

            keyPropertyGroupBox.Enabled = noBaseType;
            chCreateKey.Enabled = noBaseType;
            edPropertyName.Enabled = noBaseType && createKey;
            cmbPropertyTypes.Enabled = noBaseType && createKey;
        }

        private void chCreateKey_CheckedChanged(object sender, EventArgs e)
        {
            UpdateKeyPropertyGroupBox();
        }
    }
}
