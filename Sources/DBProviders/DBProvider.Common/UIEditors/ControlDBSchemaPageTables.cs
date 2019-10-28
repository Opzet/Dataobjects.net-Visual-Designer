using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TXSoftware.DataObjectsNetEntityModel.Common;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider.UIEditors
{
    public partial class ControlDBSchemaPageTables : ControlDBSchemaPage
    {
        private const string DEFAULT_PAGE_TITLE = "Select tables and columns for which persistent entities and their properties will be generated into model.";

        private bool nodeCheckedChangeDisabled = false;

        //private readonly Dictionary<Table, List<TreeNode>> tableColumns = new Dictionary<Table, List<TreeNode>>();
        //private readonly Dictionary<Table, List<Column>> selectedTableColumns = new Dictionary<Table, List<Column>>();
        private readonly List<Column> selectedTableColumns = new List<Column>();

        private const int IMAGE_INDEX_DB = 0;
        private const int IMAGE_INDEX_SCHEMA = 1;
        private const int IMAGE_INDEX_TABLE = 2;
        private const int IMAGE_INDEX_TABLE_COLUMN = 3;

        public ControlDBSchemaPageTables()
        {
            InitializeComponent();
        }

        public override string DefaultPageTitle
        {
            get { return DEFAULT_PAGE_TITLE; }
        }

        protected override void InternalInitialize()
        {
            treeTables.CheckBoxes = true;
            treeColumns.CheckBoxes = true;
        }

        protected internal override void CheckNextButton()
        {
            //wizardOwner.ButtonNext.Enabled = HasAnyCheckedNodes(treeTables);
            var selectedTablesAndColumns = GetSelectedTablesAndColumns();
            wizardOwner.ButtonNext.Enabled = selectedTablesAndColumns.SelectMany(list => list.Value).Count() > 0;
            UpdateSelectedCounts();
        }

        private bool HasAnyCheckedNodes(TreeViewEx treeView)
        {
            int checkedCount = 0;

            IterateTreeNodes(treeView, (o, node) =>
                             {
                                 if (node.Checked)
                                 {
                                     checkedCount++;
                                 }
                             });

            return checkedCount > 0;
        }

        private List<Table> GetSelectedTables()
        {
            List<Table> result = new List<Table>();

            IterateTreeNodes(treeTables, (dbObject, node) =>
            {
                if (node.Checked && dbObject.ItemType == DbItemType.Table)
                {
                    result.Add((Table) dbObject);
                }
            });

            return result;
        }

        private Dictionary<Table, IEnumerable<Column>> GetSelectedTablesAndColumns()
        {
            Dictionary<Table, IEnumerable<Column>> result = new Dictionary<Table, IEnumerable<Column>>();
            foreach (Table table in GetSelectedTables())
            {
                var columns = GetSelectedColumns(table);
                result.Add(table, columns);
            }

            return result;
        }

        private IEnumerable<Column> GetSelectedColumns(Table table)
        {
            return selectedTableColumns.Where(column => column.Owner == table);
//            List<Column> columns = new List<Column>();
//            List<TreeNode> columnNodes = tableColumns.ContainsKey(table) ? tableColumns[table] : null;
//
//            if (columnNodes != null)
//            {
//                IterateTreeNodes(columnNodes, delegate(DBObject dbObject, TreeNode node)
//                {
//                    if (node.Checked)
//                    {
//                        columns.Add((Column)dbObject);
//                    }
//                });
//            }
//
//            return columns;
        }

        private bool AutoSelectDependencyTables
        {
            get { return chSelectDependTables.Checked; }
        }

        private void RecursiveCheckNodes(IEnumerable treeNodes, bool isChecked)
        {
            foreach (TreeNode node in treeNodes)
            {
                node.Checked = isChecked;

                if (node.Nodes.Count > 0)
                {
                    RecursiveCheckNodes(node.Nodes, isChecked);
                }

                //AutoCheckDependencyTables(node);
            }
        }

        private void IterateTreeNodes(TreeViewEx treeControl, Action<DBObject, TreeNode> action)
        {
            IterateTreeNodes(treeControl.Nodes, action);
        }

        private void IterateTreeNodes(IEnumerable nodes, Action<DBObject, TreeNode> action)
        {
            foreach (TreeNode node in nodes)
            {
                var nodeItem = node.Tag as DBObject;
                action(nodeItem, node);

                if (node.Nodes.Count > 0)
                {
                    IterateTreeNodes(node.Nodes, action);
                }
            }
        }

        protected internal override void InternalUpdateControls(bool enabled, bool waitCursor)
        {
            
        }

        private void RefreshTables()
        {
            ControlSync.Instance.Post(CallbackRefreshTables,
                                     this.wizardOwner.SelectedDB,
                                     delegate
                                     {
                                         //CheckLastErrors();
                                         CheckNextButton();
                                     });
        }

        private void CallbackRefreshTables(AsyncCallbackState callbackState)
        {
            var data = (Tuple<IDBProvider, IConnectionInfo>)callbackState.State;
            IDBProvider dbProvider = data.Item1;
            IConnectionInfo connectionInfo = data.Item2;

            if (dbProvider.IsConnected)
            {
                dbProvider.Disconnect();
            }

            bool connectResult = dbProvider.Connect(connectionInfo);

            if (connectResult)
            {
                Server server = new Server(connectionInfo.ServerName);
                Database database = new Database(connectionInfo.DatabaseName, server);
                bool refresResult = dbProvider.Refresh(database, LoadingMode.RecursiveAllLevels);

                if (refresResult)
                {
                    callbackState.InvokeOnUI(delegate(object state)
                    {
                        Database db = (Database)state;

                        PopulateTables(db);
                    }, database);

                }
            }
        }

        private void PopulateTables(Database database)
        {
            TreeNodeCollection tableRootNodes = treeTables.Nodes;
            tableRootNodes.Clear();

            TreeNode nodeDatabase = AddTreeNode(tableRootNodes, database);
            var schemas = database.Schemas.OrderByDescending(item => item.Tables.Count).ThenBy(item => item.Name);

            if (schemas.Count() > 0)
            {
                foreach (var schema in schemas)
                {
                    TreeNode schemaNode = AddTreeNode(nodeDatabase.Nodes, schema);
                    if (schema.Tables.Count > 0)
                    {
                        foreach (var table in schema.Tables.OrderBy(item => item.Name))
                        {
                            AddTreeNode(schemaNode.Nodes, table);
                        }
                    }
                    else
                    {
                        HideCheckBoxForTreeNode(schemaNode);
                    }
                }
            }
            else
            {
                HideCheckBoxForTreeNode(nodeDatabase);
            }

            nodeDatabase.Expand();
        }

        private void PopulateColumns(Table table)
        {
            treeColumns.Nodes.Clear();
            if (table == null)
            {
                return;
            }
            //bool requirePopulate;
            //IEnumerable<TreeNode> columnsNodes = GetColumnsNodes(table, out requirePopulate);
            //if (requirePopulate)
//            {
//                foreach (TreeNode node in columnsNodes)
//                {
//                    treeColumns.Nodes.Add(node);
//                }
//            }

            foreach (var column in table.Columns)
            {
                TreeNode columnNode = AddTreeNode(treeColumns.Nodes, column);
                if (IsColumnSelected(column))
                {
                    columnNode.Checked = true;
                }
            }
        }

//        private IEnumerable<TreeNode> GetColumnsNodes(Table table, out bool requirePopulate)
//        {
//            requirePopulate = true;
//
//            if (!tableColumns.ContainsKey(table))
//            {
//                List<TreeNode> list = new List<TreeNode>();
                //var tableNode = AddTreeNode(treeColumns.Nodes, table);
                //list.Add(tableNode);
//
//                foreach (var column in table.Columns)
//                {
//                    TreeNode columnNode = AddTreeNode(treeColumns.Nodes, column);
//                    list.Add(columnNode);
//                }
//
//                tableColumns.Add(table, list);
//                requirePopulate = false;
//            }
//            
//            return tableColumns[table];
//        }

        private TreeNode AddTreeNode(TreeNodeCollection parentNodes, DBObject dbObject)
        {
            int imageIndex = GetNodeImageIndex(dbObject);

            string text = dbObject.ItemType != DbItemType.Column ? dbObject.Name : string.Format("{0}: {1}", dbObject.Name, (dbObject as Column).ClrDataType.Name);
            TreeNode newNode = new TreeNode(text)
            {
                ImageIndex = imageIndex,
                SelectedImageIndex = imageIndex
            };

            bool showCheckbox = true;

            newNode.Tag = dbObject; //new TreeNodeItem(newNode, dbObject, nodeItemType, showCheckbox);
            newNode.Checked = false;

            parentNodes.Add(newNode);

//            if (!showCheckbox)
//            {
//                WinAPI.HideCheckBoxForTreeNode(newNode);
//            }

            return newNode;
        }

        private Table SelectedTable
        {
            get
            {
                if (treeTables.SelectedNode != null)
                {
                    DBObject dbObject = (DBObject) treeTables.SelectedNode.Tag;
                    if (dbObject.ItemType == DbItemType.Table)
                    {
                        return (Table) dbObject;
                    }
                }

                return null;
            }
        }

        private void HideCheckBoxForTreeNode(TreeNode node)
        {
            WinAPI.HideCheckBoxForTreeNode(node);
        }

        private int GetNodeImageIndex(DBObject dbObject)
        {
            switch (dbObject.ItemType)
            {
                case DbItemType.Database:
                    return IMAGE_INDEX_DB;
                case DbItemType.Schema:
                    return IMAGE_INDEX_SCHEMA;
                case DbItemType.Table:
                    return IMAGE_INDEX_TABLE;
                case DbItemType.Column:
                    return IMAGE_INDEX_TABLE_COLUMN;
                default:
                    throw new NotSupportedException();
            }
        }


        public override bool LeavePage(PageDirection leaveDirection)
        {
            bool isValid = leaveDirection == PageDirection.Back;

            if (!isValid)
            {
                CheckNextButton();
                isValid = wizardOwner.ButtonNext.Enabled;
                if (isValid)
                {
                    isValid = CheckSelectedColumns();

                    if (isValid)
                    {
                        SaveSelectedData();
                    }
                }
            }

            return isValid;
        }

        private bool CheckSelectedColumns()
        {
            var selectedTablesAndColumns = GetSelectedTablesAndColumns();
            var tablesWithNoColumnsSelected = from item in selectedTablesAndColumns
                    let table = item.Key
                    let columns = item.Value
                    where columns.Count() == 0
                    select table.FormatName();

            bool isValid = tablesWithNoColumnsSelected.Count() == 0;

            if (!isValid)
            {
                string error = string.Join(Environment.NewLine, tablesWithNoColumnsSelected);
                MessageBox.Show(
                    "Some selected tables does not have selected any columns (at least one column must be selected).\n\rList of tables:\n\r" +
                    error,
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isValid;
        }

        internal void SaveSelectedData()
        {
            var selectedTablesAndColumns = GetSelectedTablesAndColumns();

            foreach (var pair in selectedTablesAndColumns)
            {
                Table table = pair.Key;
                var columns = pair.Value;

                table.Columns.RemoveAll(tableCol => !columns.Contains(tableCol));
            }

            wizardOwner.SelectedTablesAndColumns = selectedTablesAndColumns.Keys.ToList();
        }

        public override void EnterPage(PageDirection enterDirection)
        {
            if (enterDirection == PageDirection.Next)
            {
                Tuple<IDBProvider, IConnectionInfo> selectedDatabase = this.wizardOwner.SelectedDB;

                IDBProvider dbProvider = selectedDatabase.Item1;
                if (dbProvider.IsConnected)
                {
                    dbProvider.Disconnect();
                }

                bool connectResult = dbProvider.Connect(selectedDatabase.Item2);
                if (connectResult)
                {
                    RefreshTables();
                }
            }
        }

        private void treeTables_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TableNodeChecked(e.Node);
        }

        private void AddOrRemoveSelectedTableColumn(TreeNode columnNode, bool add)
        {
            Column column = (Column)columnNode.Tag;
            AddOrRemoveSelectedTableColumn(column, add);
        }

        private void AddOrRemoveSelectedTableColumn(Column column, bool add)
        {
            if (add)
            {
                if (!selectedTableColumns.Contains(column))
                {
                    selectedTableColumns.Add(column);
                }
            }
            else
            {
                if (selectedTableColumns.Contains(column))
                {
                    selectedTableColumns.Remove(column);
                }
            }
        }

        private bool IsColumnSelected(Column column)
        {
            return selectedTableColumns.Contains(column);
        }

        private void TableNodeChecked(TreeNode checkedNode)
        {
            if (nodeCheckedChangeDisabled)
            {
                return;
            }

            bool isChecked = checkedNode.Checked;
            DBObject checkedDbObject = (DBObject) checkedNode.Tag;
            DbItemType checkedNodeItemType = checkedDbObject.ItemType;

            nodeCheckedChangeDisabled = true;

            try
            {
                RecursiveCheckNodes(checkedNode.Nodes, isChecked);

                if (checkedNode.Parent != null)
                {
                    if (isChecked)
                    {
                        checkedNode.Parent.Checked = true;
                    }
                    else
                    {
                        int childNodesChecked = 0;
                        IterateTreeNodes(checkedNode.Parent.Nodes, (o, node) =>
                        {
                            if (node.Checked)
                            {
                                childNodesChecked++;
                            }
                        });

                        if (childNodesChecked == 0)
                        {
                            checkedNode.Parent.Checked = false;
                        }
                    }
                }

                if (isChecked)
                {
                    List<TreeNode> nodesToAutoCheck = new List<TreeNode>();
                    switch (checkedNodeItemType)
                    {
                        case DbItemType.Database:
                        {
                            selectedTableColumns.Clear();
                            Database checkedDB = (Database)checkedDbObject;
                            var allDatabaseColumns = checkedDB.Schemas.Select(schema => schema.Tables).SelectMany(list => list).Select(
                                table => table.Columns).SelectMany(list => list);
                            selectedTableColumns.AddRange(allDatabaseColumns);
                            
                            break;
                        }
                        case DbItemType.Table:
                        {
                            nodesToAutoCheck.Add(checkedNode);

                            Table checkedTable = (Table)checkedDbObject;
                            foreach (var column in checkedTable.Columns)
                            {
                                AddOrRemoveSelectedTableColumn(column, true);
                            }
                            
                            break;
                        }
                        case DbItemType.Schema:
                        {
                            nodesToAutoCheck.AddRange(checkedNode.Nodes.OfType<TreeNode>().ToArray());

                            Schema chckedSchema = (Schema)checkedDbObject;
                            var schemaTablesColumns = chckedSchema.Tables.Select(table => table.Columns).SelectMany(list => list);
                            foreach (var column in schemaTablesColumns)
                            {
                                AddOrRemoveSelectedTableColumn(column, true);
                            }

                            break;
                        }
                    }

                    if (nodesToAutoCheck.Count > 0)
                    {
                        AutoCheckDependencyTables(nodesToAutoCheck.ToArray());
                    }
                }
                else
                {
                    switch (checkedNodeItemType)
                    {
                        case DbItemType.Database:
                        {
                            selectedTableColumns.Clear();
                            break;
                        }
                        case DbItemType.Table:
                        {
                            Table checkedTable = (Table) checkedDbObject;
                            selectedTableColumns.RemoveAll(column => checkedTable.Columns.Contains(column));
                            break;
                        }
                        case DbItemType.Schema:
                        {
                            Schema chckedSchema = (Schema) checkedDbObject;
                            var schemaTablesColumns = chckedSchema.Tables.Select(table => table.Columns).SelectMany(list => list);
                            selectedTableColumns.RemoveAll(column => schemaTablesColumns.Contains(column));
                            break;
                        }
                    }
                }

                ReflectColumnsCheckedState();
            }
            finally
            {
                nodeCheckedChangeDisabled = false;
            }

            CheckNextButton();
        }

        private void ReflectColumnsCheckedState()
        {
            foreach (TreeNode node in treeColumns.Nodes)
            {
                Column column = (Column) node.Tag;
                node.Checked = IsColumnSelected(column);
            }
        }

        private void AutoCheckDependencyTables(params TreeNode[] sourceNodes)
        {
            //bool isChecked = sourceNode.Checked;
            //DBObject sourceDbObject = (DBObject)sourceNode.Tag;

            if (!AutoSelectDependencyTables)
            {
                return;
            }


            UpdateControls(false, true);
            wizardOwner.BeginLoadingAction(true);

            try
            {
                nodeCheckedChangeDisabled = true;

                List<TreeNode> dependTableNodes = new List<TreeNode>();

                var sourceDbTables = sourceNodes.Select(node => (DBObject) node.Tag).Where(dbObj => dbObj.ItemType == DbItemType.Table).Cast<Table>();

                var allDependendTables = sourceDbTables.Select(sourceTable => sourceTable.GetDependencyTables())
                    .SelectMany(list => list)
                    .Distinct(Table.TableEqualityComparer);


                var flatList = GetFlatList(treeTables);

                foreach (var treeNode in flatList)
                {
                    Table nodeTable = treeNode.Tag as Table;
                    if (nodeTable != null)
                    {
                        if (allDependendTables.Contains(nodeTable))
                        {
                            if (!dependTableNodes.Contains(treeNode))
                            {
                                dependTableNodes.Add(treeNode);
                            }
                        }
                    }
                }

                /*var treeDbTables = flatList.Select(node => (DBObject)node.Tag).Where(dbObj => dbObj.ItemType == DbItemType.Table).Cast<Table>();

                Database database = sourceDbTables.First().Owner.Owner;
                IEnumerable<Table> allSchemaTables = database.Schemas.SelectMany(schema => schema.Tables);


                foreach (var sourceTable in sourceDbTables)
                {
                    var allTables = allSchemaTables.Except(new[] { sourceTable });
                    var allDependTables = allTables.Where(nodeTable.DependsOn);
                }*/



/*
                Table nodeTable = (Table) sourceDbObject;
                Database database = nodeTable.Owner.Owner;
                var allTables = database.Schemas.SelectMany(schema => schema.Tables)
                    .Where(table => table != nodeTable);

                var allDependTables = allTables.Where(nodeTable.DependsOn);

                List<TreeNode> dependTableNodes = new List<TreeNode>();
                IterateTreeNodes(treeTables, delegate(DBObject dbObject, TreeNode node)
                                             {
                                                 if (dbObject.ItemType == DbItemType.Table)
                                                 {
                                                     Table dbTable = (Table) dbObject;
                                                     if (allDependTables.Contains(dbTable))
                                                     {
                                                         if (!dependTableNodes.Contains(node))
                                                         {
                                                             dependTableNodes.Add(node);
                                                         }
                                                     }
                                                 }
                                             });
*/

                foreach (TreeNode dependTableNode in dependTableNodes)
                {
                    dependTableNode.Checked = true;

                    if (!dependTableNode.Parent.Checked)
                    {
                        dependTableNode.Parent.Checked = true;
                    }
                }
            }
            finally
            {
                UpdateControls(true, false);
                wizardOwner.EndLoadingAction();
                nodeCheckedChangeDisabled = false;
            }
        }

        private List<TreeNode> GetFlatList(TreeViewEx treeView)
        {
            List<TreeNode> flatList = new List<TreeNode>();
            var treeIterator = new GenericTreeIterator<TreeNode>(
                treeNode => treeNode.Nodes.OfType<TreeNode>(),
                () => treeView.Nodes.OfType<TreeNode>());

            treeIterator.IterateTree(false, null,
                                     delegate(GenericTreeIterationArgs<TreeNode> args)
                                     {
                                         if (!flatList.Contains(args.Current))
                                         {
                                             flatList.Add(args.Current);
                                         }
                                     });

            return flatList;
        }

        private void treeTables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                CheckAllNodes(true);
            }
        }

        private void treeColumns_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                CheckAllNodes(false);
            }
        }

        private void UpdateSelectedCounts()
        {
            var selectedTablesAndColumns = GetSelectedTables();
            lbSelectedTables.Text = string.Format("Selected Tables: {0}", selectedTablesAndColumns.Count);

            UpdateSelectedColumnsCounts();
        }

        private void UpdateSelectedColumnsCounts() {
            Table selectedTable = SelectedTable;
            int selectedColumns = 0;
            string advText = string.Empty;
            if (selectedTable != null)
            {
                selectedColumns = GetSelectedColumns(selectedTable).Count();
                advText = string.Format(" in Table: {0}", selectedTable.Name);
            }
            lbSelectedColumns.Text = string.Format("Selected Columns: {0} {1}", selectedColumns, advText);
        }

        private void CheckAllNodes(bool tablesTree)
        {
            TreeViewEx treeControl = tablesTree ? treeTables : treeColumns;

            bool tablesHasAnyCheckedNodes = HasAnyCheckedNodes(treeControl);

            nodeCheckedChangeDisabled = true;

            try
            {
                IEnumerable<TreeNode> nodes = treeControl.Nodes.OfType<TreeNode>();
                IEnumerable<TreeNode> treeNodes = tablesTree
                                                      ? nodes.Where(
                                                          node => node.Nodes.Count > 0)
                                                      : nodes;

                RecursiveCheckNodes(treeNodes, !tablesHasAnyCheckedNodes);
            }
            finally
            {
                nodeCheckedChangeDisabled = false;
            }

            CheckNextButton();
        }

        private void treeTables_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PopulateColumns(SelectedTable);
            UpdateSelectedColumnsCounts();
        }

        private void treeColumns_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode treeNode = e.Node;

            AddOrRemoveSelectedTableColumn(treeNode, treeNode.Checked);

            CheckNextButton();
        }

        private void treeTables_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            DBObject dbObject = (DBObject) e.Node.Tag;
            e.Cancel = dbObject.ItemType == DbItemType.Schema && e.Node.Nodes.Count == 0;
        }
    }
}