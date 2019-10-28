using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    public partial class ControlDBSchemaAuxiliaryTables : ControlDBSchemaPage
    {
        private const string DEFAULT_PAGE_TITLE = "Please provide information which of those tables are auxiliary tables, if any.";

        private List<Tuple<Table, bool, bool>> auxiliaryTables = new List<Tuple<Table, bool, bool>>();

        public ControlDBSchemaAuxiliaryTables()
        {
            InitializeComponent();
        }

        public override string DefaultPageTitle
        {
            get { return DEFAULT_PAGE_TITLE; }
        }

        protected override void InternalInitialize() {}

        protected internal override void CheckNextButton()
        {}

        public override bool LeavePage(PageDirection leaveDirection)
        {
            return true;
        }

        public override void EnterPage(PageDirection enterDirection)
        {
            if (enterDirection == PageDirection.Next)
            {
                PopulateForm();
            }
        }

        protected internal override void InternalUpdateControls(bool enabled, bool waitCursor)
        {
            lvTables.Enabled = enabled;
        }

        private void PopulateForm()
        {
            lvTables.Groups.Clear();
            lvTables.Items.Clear();

            List<Table> selectedTablesAndColumns = this.wizardOwner.SelectedTablesAndColumns;

            var tableSchemas =
                from table in selectedTablesAndColumns
                group table by table.Owner
                    into g
                    select new { Schema = g.Key};

            foreach (var tableSchema in tableSchemas.OrderBy(item => item.Schema.Name))
            {
                Schema schema = tableSchema.Schema;
                ListViewGroup schemaViewGroup = lvTables.Groups.Add(schema.Name, schema.Name);
                schemaViewGroup.Tag = schema;
            }


            auxiliaryTables = (from table in selectedTablesAndColumns
                        let isAux = AssociationsBuilder.DetectTableIsAuxiliary(table)
                        select new Tuple<Table, bool, bool>(table, isAux, !isAux)).ToList();

            foreach (var pair in auxiliaryTables.OrderBy(item => item.Item1.Name))
            {
                Table table = pair.Item1;
                bool tableIsAux = pair.Item2;
                bool genPersistType = pair.Item3;
                Schema schema = table.Owner;

                ListViewGroup schemaViewGroup = lvTables.Groups[schema.Name];

//                if (schemaViewGroup == null)
//                {
//                    schemaViewGroup = lvTables.Groups.Add(schema.Name, schema.Name);
//                    schemaViewGroup.Tag = schema;
//                }

                ListViewItem tableViewItem = lvTables.Items.Add(table.FormatName());
                tableViewItem.SubItems.AddRange(new string[] {tableIsAux.ToString(), genPersistType.ToString()});
                
                tableViewItem.Tag = pair;
                tableViewItem.Group = schemaViewGroup;

                int row = lvTables.Items.Count - 1;

                //Label labelTableName = new Label{Text = table.Name};
                //lvTables.AddEmbeddedControl(labelTableName, 0, row);

                CheckBox checkBoxIsAux = new CheckBox
                                         {
                                             Checked = tableIsAux,
                                             CheckAlign = ContentAlignment.MiddleCenter,
                                             FlatStyle = FlatStyle.Flat
                                         };
                lvTables.AddEmbeddedControl(checkBoxIsAux, 1, row);

                CheckBox checkBoxGenPT = new CheckBox
                                         {
                                             Checked = genPersistType,
                                             CheckAlign = ContentAlignment.MiddleCenter,
                                             FlatStyle = FlatStyle.Flat
                                         };
                lvTables.AddEmbeddedControl(checkBoxGenPT, 2, row);
            }
        }
    }
}
