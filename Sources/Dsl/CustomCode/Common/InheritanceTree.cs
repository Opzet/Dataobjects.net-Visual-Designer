using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    #region InheritanceNode

    public class InheritanceNode : IEqualityComparer
    {
        private readonly List<InheritanceNode> childList = new List<InheritanceNode>();

        #region properties

        public IInterface Interface { get; private set; }

        public InheritanceNodeType NodeType { get; private set; }

        public bool IsSuperRoot
        {
            get { return this.NodeType == InheritanceNodeType.SuperRoot; }
        }

        public bool IsTree
        {
            get { return this.NodeType == InheritanceNodeType.Tree; }
        }

        public bool IsNode
        {
            get { return this.NodeType == InheritanceNodeType.Node; }
        }

        public ReadOnlyCollection<InheritanceNode> Childs
        {
            get
            {
                return new ReadOnlyCollection<InheritanceNode>(childList);
            }
        }

        public InheritanceNode Parent { get; private set; }

        #endregion properties

        #region constructors

        protected InheritanceNode(InheritanceNode parent, IInterface @interface, InheritanceNodeType nodeType, IEnumerable<InheritanceNode> childs)
        {
            this.Parent = parent;
            this.Interface = @interface;
            this.NodeType = nodeType;
            if (childs != null)
            {
                childList.AddRange(childs);
            }
        }

        #endregion constructors

        internal void AddChild(InheritanceNode child)
        {
            if (!this.childList.Contains(child))
            {
                this.childList.Add(child);
            }
        }

        public override string ToString()
        {
            return string.Format("'{0}'({1}:{2})", Interface.Name, (int)Interface.TypeKind, NodeType);
        }

        #region static methods

        internal static InheritanceNode CreateNode(InheritanceNodeType nodeType, InheritanceNode parent, IInterface @interface, IEnumerable<InheritanceNode> childs)
        {
            return new InheritanceNode(parent, @interface, nodeType, childs);
        }

        #endregion static methods

        #region Implementation of IEqualityComparer

        bool IEqualityComparer.Equals(object x, object y)
        {
            InheritanceNode nodeX = (InheritanceNode) x;
            InheritanceNode nodeY = (InheritanceNode) y;

            return nodeX.Interface == nodeY.Interface;
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            InheritanceNode node = (InheritanceNode) obj;
            return node.Interface.GetHashCode();
        }

        #endregion
    }

    #endregion InheritanceNode

    #region enum InheritanceNodeType

    public enum InheritanceNodeType
    {
        Node,
        SuperRoot,
        Tree
    }

    #endregion enum InheritanceNodeType

    #region class InheritanceTreeIterator

    public sealed class InheritanceTreeIterator
    {
        #region enum Stage

        public enum Stage
        {
            Enter,
            Leave
        }

        #endregion enum Stage

        public int Level { get; internal set; }
        public InheritanceNode Parent { get; internal set; }
        public InheritanceNode Current { get; internal set; }
        public bool Cancel { get; set; }
        public Stage CurrentStage { get; internal set; }

        public InheritanceTreeIterator(int level, InheritanceNode parent, InheritanceNode current)
        {
            Level = level;
            Parent = parent;
            Current = current;
            this.CurrentStage = Stage.Enter;
        }
    }

    #endregion class InheritanceTreeIterator

    #region class InheritanceTree

    public sealed class InheritanceTree : InheritanceNode
    {
        private readonly List<InheritancePath> inheritancePaths = new List<InheritancePath>();
        private bool treeRebuilded = false;
        private bool pathsRebuilded = false;

        public bool PathsRebuilded
        {
            get { return pathsRebuilded; }
        }

        public bool TreeRebuilded
        {
            get { return treeRebuilded; }
        }

        internal InheritanceTree(IInterface @interface, IEnumerable<InheritanceNode> childs)
            : base(null, @interface, InheritanceNodeType.Tree, childs)
        { }

        internal static InheritanceTree Create(IInterface treeRoot)
        {
            List<InheritanceNode> treeRootChilds = new List<InheritanceNode>();
            InheritanceTree inheritanceTree = new InheritanceTree(treeRoot, treeRootChilds);

            return inheritanceTree;
        }

        internal static List<InheritanceTree> Create(params IInterface[] treeRoots)
        {
            return (from treeRoot in treeRoots
                   select Create(treeRoot)).ToList();
        }

        internal static void RebuildTree(bool rebuildPaths, params InheritanceTree[] trees)
        {
            foreach (var tree in trees)
            {
                tree.RebuildTree(rebuildPaths);
            }
        }

        public void RebuildTree()
        {
            RebuildTree(false);
        }

        public void RebuildTree(bool rebuildPaths)
        {
            if (!treeRebuilded)
            {
                treeRebuilded = true;
            }

            var interfacesIterator = new GenericTreeIterator<IInterface>(@interface => @interface.GetCurrentLevelInheritedInterfaces());
            interfacesIterator.IterateTree(true, this.Interface, delegate(GenericTreeIterationArgs<IInterface, InheritanceNode> iterationArgs)
            {
                if (iterationArgs.CurrentStage == GenericTreeIterationStage.Leave)
                {
                    iterationArgs.Data = null;
                }
                else
                {
                    var childs = iterationArgs.Current.GetCurrentLevelInheritedInterfaces();

                    InheritanceNodeType nodeType = childs.Count() == 0
                                                       ? InheritanceNodeType.SuperRoot
                                                       : InheritanceNodeType.Node;

                    InheritanceNode parentNode = iterationArgs.Data ?? this;

                    InheritanceNode newNode = CreateNode(nodeType, parentNode, iterationArgs.Current, null);
                    parentNode.AddChild(newNode);

                    iterationArgs.Data = newNode;
                }
            });

            pathsRebuilded = false;

            if (rebuildPaths)
            {
                RebuildPaths();
            }
        }

        public void RebuildPaths()
        {
            CheckTreeRebuilded();

            if (!pathsRebuilded)
            {
                pathsRebuilded = true;
            }

            this.inheritancePaths.Clear();

            List<InheritanceNode> pathNodes = new List<InheritanceNode>();
            InternalIterateTree(true, 0, this,
                                delegate(InheritanceTreeIterator iterator)
                                {
                                    if (iterator.CurrentStage == InheritanceTreeIterator.Stage.Leave)
                                    {
                                        pathNodes.Remove(iterator.Current);
                                    }
                                    else
                                    {
                                        if (/*pathNodes.Count > 0 &&*/ iterator.Current.Childs.Count() == 0)
                                        {
                                            pathNodes.Add(iterator.Current);
                                            CreatePath(Enumerable.Reverse(pathNodes));
                                        }
                                        else
                                        {
                                            pathNodes.Add(iterator.Current);
                                        }
                                    }
                                });
        }

        private void CheckTreeRebuilded()
        {
            if (!treeRebuilded)
            {
                throw new InvalidOperationException("InheritanceTree is not initialized, call RebuildTree to do so.");
            }
        }

        private void CheckPathsRebuilded()
        {
            if (!pathsRebuilded)
            {
                throw new InvalidOperationException("InheritanceTree paths was not rebuilded, call RebuildPaths to do so.");
            }
        }

        public ReadOnlyCollection<InheritanceNode> GetFlatList(InheritanceListMode listMode)
        {
            CheckTreeRebuilded();
            List<InheritanceNode> result = new List<InheritanceNode>();
            var requestsCurrentLevel = listMode == InheritanceListMode.CurrentLevel;
            IterateTree(true, args =>
            {
                if (args.CurrentStage == GenericTreeIterationStage.Enter)
                {
                    if (!result.Any(node => node.Interface == args.Current.Interface) &&
                        (!requestsCurrentLevel || args.Level == 0))
                    {
                        result.Add(args.Current);
                    }
                }

                if (requestsCurrentLevel && args.Level > 0)
                {
                    args.Cancel = true;
                    args.CancelBreaksChildLoop = true;
                }
            });

            return new ReadOnlyCollection<InheritanceNode>(result);
        }

        public IEnumerable<InheritancePath> FindInheritancePath(IInterface hierarchyTopInterface, IInterface superRootInterface, int requiredDistance)
        {
            var query = from path in inheritancePaths
                        let hierarchyTopInterfacePos = path.GetNodePosition(hierarchyTopInterface)
                        let superRootInterfacePos = path.GetNodePosition(superRootInterface)
                        let distance = Math.Abs(hierarchyTopInterfacePos - superRootInterfacePos)
                        where path.HierarchyTopNode.Interface == hierarchyTopInterface &&
                                path.SuperRootNode.Interface == superRootInterface &&
                              distance == requiredDistance
                        select path;

            return query;
        }

        public ReadOnlyCollection<InheritanceDetail> FoundDuplicatedDirectInheritances()
        {
            CheckTreeRebuilded();
            CheckPathsRebuilded();

            List<InheritanceDetail> result = new List<InheritanceDetail>();

            var parentInterface = this.Interface;

            foreach (InheritanceNode childNode in this.Childs)
            {
                var childInterface = childNode.Interface;
                var hierarchyToSuperRootPath = FindInheritancePath(parentInterface, childInterface, 1).SingleOrDefault();

                IEnumerable<InheritancePath> query;

                if (hierarchyToSuperRootPath != null)
                {
                    query = from path in inheritancePaths
                            /*let childInterfacePos = path.GetNodePosition(childInterface)
                                let parentInterfacePos = path.GetNodePosition(parentInterface)*/
                            where path.ContainsNode(childInterface) &&
                                  path.SuperRootNode.Interface == childInterface &&
                                  !path.Id.Equals(hierarchyToSuperRootPath.Id)
                            select path;
                }
                else
                {
                    query = from path in inheritancePaths
                            let childInterfaceNode = path.GetNode(childInterface)
                            let parentInterfaceNode = path.GetNode(parentInterface)
                                //let childInterfacePos = path.GetNodePosition(childInterface)
                                //let parentInterfacePos = path.GetNodePosition(parentInterface)
                            where path.ContainsNode(childInterface) &&
                                childInterfaceNode.Parent != null && childInterfaceNode.Parent != parentInterfaceNode
                                  //path.SuperRootNode.Interface == childInterface &&
                                  //!path.Id.Equals(hierarchyToSuperRootPath.Id)
                            select path;
                }

                var paths = query.ToArray();
                if (paths.Length > 0)
                {
                    InheritanceDetail detail = new InheritanceDetail(childInterface);
                    foreach (InheritancePath path in paths)
                    {
                        InheritanceNode node = path.GetNode(path.PathLength - 2);
                        int distance = path.GetNodesDistance(node, path.SuperRootNode);
                        if (!detail.ContainsItem(node.Interface))
                        {
                            detail.Add(new InheritanceDetailItem(distance == 1 ? InheritanceType.Direct : InheritanceType.Indirect, node.Interface));
                        }
                    }

                    result.Add(detail);
                }
            }

            return new ReadOnlyCollection<InheritanceDetail>(result);
        }

        public void IterateTree(bool twoStagePass, Action<GenericTreeIterationArgs<InheritanceNode>> iterator)
        {
            CheckTreeRebuilded();
            var treeIterator = new GenericTreeIterator<InheritanceNode>(node => node.Childs);
            treeIterator.IterateTree(true, this, iterator);
        }

        public void IterateTree(Action<InheritanceTreeIterator> iterateAction)
        {
            IterateTree(true, iterateAction);
        }

        public void IterateTree(bool twoStagePass, Action<InheritanceTreeIterator> iterateAction)
        {
            CheckTreeRebuilded();
            InternalIterateTree(twoStagePass, 0, this, iterateAction);
        }

        // Returns true if iteration was cancelled
        private bool InternalIterateTree(bool twoStagePass, int level, InheritanceNode fromNode, Action<InheritanceTreeIterator> iterateAction)
        {
            foreach (InheritanceNode child in fromNode.Childs)
            {
                var treeIterator = new InheritanceTreeIterator(level, fromNode, child);
                iterateAction(treeIterator);
                if (treeIterator.Cancel)
                {
                    return true;
                }

                if (child.Childs.Count() > 0)
                {
                    bool cancel = InternalIterateTree(twoStagePass, level + 1, child, iterateAction);
                    if (cancel)
                    {
                        return true;
                    }
                }

                if (twoStagePass)
                {
                    treeIterator.CurrentStage = InheritanceTreeIterator.Stage.Leave;
                    iterateAction(treeIterator);
                    if (treeIterator.Cancel)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private InheritancePath CreatePath(IEnumerable<InheritanceNode> nodes)
        {
            InheritancePath path = new InheritancePath(this, nodes);
            this.inheritancePaths.Add(path);
            return path;
        }

        public ReadOnlyCollection<InheritancePath> InheritancePaths
        {
            get
            {
                return new ReadOnlyCollection<InheritancePath>(inheritancePaths);
            }
        }

        public IEnumerable<InheritanceNode> GetUniquePathsSuperRoots()
        {
            CheckTreeRebuilded();
            var uniqueSuperRoots = (from path in inheritancePaths
                                    group path by path.SuperRootNode
                                        into g
                                        select g.Key).Distinct(new InheritanceNodeSuperRootEqualityComparer());
            return uniqueSuperRoots;
        }

        public static IEnumerable<IInterface> GetDependencyImplementationFlatList(IEnumerable<InheritancePath> paths)
        {
            List<IInterface> result = new List<IInterface>();
            int maxPathLength = paths.Count() == 0 ? 0 : paths.Max(path => path.PathLength);

            for (int i = 0; i < maxPathLength; i++)
            {
                foreach (InheritancePath path in paths)
                {
                    InheritanceNode inheritanceNode = path.GetNode(i);
                    if (inheritanceNode != null && !result.Contains(inheritanceNode.Interface))
                    {
                        result.Add(inheritanceNode.Interface);
                    }
                }
            }

            return result;
        }

        public static IEnumerable<InheritancePath> MergePaths(IEnumerable<InheritanceTree> inheritanceTrees)
        {
            return MergePaths(inheritanceTrees.ToArray());
        }

        public static IEnumerable<InheritancePath> MergePaths(params InheritanceTree[] inheritanceTrees)
        {
            foreach (var tree in inheritanceTrees)
            {
                tree.CheckPathsRebuilded();
            }

            var allInheritancePaths = inheritanceTrees.SelectMany(list => list.inheritancePaths)
                .OrderBy(path => path, new PathComparer());

            var comparer = new InheritancePathHierarchyTopNodeEqualityComparer();
            /*var comparer = new IdentityProjectionEqualityComparer<InheritancePath, string>(
                path => path.GetPathAsString(InheritancePathDirection.TreeToSuperRoot, false, node => node != path.HierarchyTopNode));*/

            return allInheritancePaths.Distinct(comparer).ToList();
        }

        public class PathComparer : IComparer<InheritancePath>
        {
            public int Compare(InheritancePath x, InheritancePath y)
            {
                int xLength = x.GetPathAsString(InheritancePathDirection.TreeToSuperRoot, false).Length;
                int yLength = y.GetPathAsString(InheritancePathDirection.TreeToSuperRoot, false).Length;
                return yLength - xLength;
            }
        }

        public static IEnumerable<InheritanceNode> GetUniquePathsSuperRoots(IEnumerable<InheritanceTree> inheritanceTrees)
        {
            return GetUniquePathsSuperRoots(inheritanceTrees.ToArray());
        }

        public static IEnumerable<InheritanceNode> GetUniquePathsSuperRoots(params InheritanceTree[] inheritanceTrees)
        {
            var uniquePaths = from tree in inheritanceTrees
                    select tree.GetUniquePathsSuperRoots();

            IEnumerable<InheritanceNode> inheritanceNodes = uniquePaths.SelectMany(list => list);
            var uniqueSuperRoots = inheritanceNodes.Distinct(new InheritanceNodeSuperRootEqualityComparer());
            return uniqueSuperRoots;
        }
    }

    #endregion class InheritanceTree

    #region enum InheritancePathDirection

    public enum InheritancePathDirection
    {
        SuperRootToTree,
        TreeToSuperRoot
    }

    #endregion enum InheritancePathDirection

    #region class InheritancePath

    public sealed class InheritancePath
    {
        private readonly List<InheritanceNode> pathNodes;
        private readonly Guid id;

        public const int PATH_POSITION_SUPER_ROOT = 0;

        public InheritanceNode SuperRootNode
        {
            get { return GetNode(PATH_POSITION_SUPER_ROOT); }
        }

        public InheritanceNode HierarchyTopNode
        {
            get { return PathLength > 0 ? GetNode(PathLength - 1) : null; }
        }

        public int PathLength
        {
            get { return this.pathNodes.Count; }
        }

        public Guid Id
        {
            get { return id; }
        }

        internal InheritancePath(InheritanceTree treeNode, IEnumerable<InheritanceNode> nodes)
        {
            this.pathNodes = new List<InheritanceNode>(nodes)
                {
                    treeNode
                };

            this.id = Guid.NewGuid();
        }

        public bool ContainsNode(IInterface @interface)
        {
            return pathNodes.Any(item => item.Interface == @interface);
        }

        public bool ContainsNode(InheritanceNode node)
        {
            return ContainsNode(node.Interface);
        }

        public InheritanceNode GetNode(IInterface @interface)
        {
            int nodePosition = GetNodePosition(@interface);
            return GetNode(nodePosition);
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="pathPosition">The path position, value 0 (zero) returns <see cref="SuperRootNode"/>, where <see cref="PathLength"/>-1 returns <see cref="HierarchyTopNode"/>.</param>
        /// <returns></returns>
        public InheritanceNode GetNode(int pathPosition)
        {
            if (pathPosition > -1 && PathLength > 0 && pathPosition < PathLength)
            {
                return pathNodes[pathPosition];
            }

            return null;
        }

        public string GetPathAsString(InheritancePathDirection direction, bool includeId)
        {
            return GetPathAsString(direction, includeId, node => true);
        }

        public string GetPathAsString(InheritancePathDirection direction, bool includeId, Func<InheritanceNode, bool> predicate)
        {
            var pathItems = GetPath(direction).Where(predicate).Select(node => node.ToString());
            return includeId
                       ? string.Format("Id:{0}> {1}", this.id, string.Join(" -> ", pathItems))
                       : string.Join(" -> ", pathItems);
        }

        public override string ToString()
        {
            return ToString(InheritancePathDirection.TreeToSuperRoot);
        }

        public string ToString(InheritancePathDirection direction, bool includeId = true)
        {
            return GetPathAsString(direction, includeId);
            /*var pathItems = GetPath(direction).Select(node => node.ToString());
            return includeId
                       ? string.Format("Id:{0}> {1}", this.id, string.Join(" -> ", pathItems))
                       : string.Join(" -> ", pathItems);*/
        }

        public IEnumerable<InheritanceNode> GetPath(InheritancePathDirection direction)
        {
            if (direction == InheritancePathDirection.SuperRootToTree)
            {
                return this.pathNodes.ToArray();
            }

            return this.pathNodes.ToArray().Reverse();
        }

        public int GetNodePosition(InheritanceNode node)
        {
            return GetNodePosition(node.Interface);
        }

        public int GetNodePosition(IInterface @interface)
        {
            //return pathNodes.Where(item => item.Interface == @interface).Select((item, i) => i).SingleOrDefault();
            return pathNodes.FindIndex(node => node.Interface == @interface);
        }

        public int GetNodesDistance(InheritanceNode nodeFrom, InheritanceNode nodeTo)
        {
            return GetNodesDistance(nodeFrom.Interface, nodeTo.Interface);
        }

        public int GetNodesDistance(IInterface interfaceFrom, IInterface interfaceTo)
        {
            int fromPosition = GetNodePosition(interfaceFrom);
            int toPosition = GetNodePosition(interfaceTo);

            return Math.Abs(fromPosition - toPosition);
        }
    }

    #endregion class InheritancePath

    #region class InheritanceNodeSuperRootEqualityComparer

    public class InheritanceNodeSuperRootEqualityComparer : IEqualityComparer<InheritanceNode>
    {
        public bool Equals(InheritanceNode x, InheritanceNode y)
        {
            return x.Interface.Name == y.Interface.Name && x.NodeType == y.NodeType;
        }

        public int GetHashCode(InheritanceNode obj)
        {
            return string.Format("{0}{1}", obj.Interface.Name, obj.NodeType).GetHashCode();
        }
    }

    #endregion class InheritanceNodeSuperRootEqualityComparer

    #region class InheritancePathHierarchyTopNodeEqualityComparer

    public class InheritancePathHierarchyTopNodeEqualityComparer : IEqualityComparer<InheritancePath>
    {
        public bool Equals(InheritancePath x, InheritancePath y)
        {
            string xPath = x.GetPathAsString(InheritancePathDirection.TreeToSuperRoot, false, node => node != x.HierarchyTopNode);
            string yPath = y.GetPathAsString(InheritancePathDirection.TreeToSuperRoot, false, node => node != y.HierarchyTopNode);
            return Util.StringEqual(xPath, yPath, true) || xPath.Contains(yPath) || yPath.Contains(xPath);
        }

        public int GetHashCode(InheritancePath path)
        {
            /*string sPath = path.GetPathAsString(InheritancePathDirection.TreeToSuperRoot, false, node => node != path.HierarchyTopNode);
            return sPath.GetHashCode();*/
            //return path.Id.GetHashCode();
            return 0;
        }
    }

    #endregion class InheritancePathHierarchyTopNodeEqualityComparer

    #region enum InheritanceType

    public enum InheritanceType
    {
        Direct,
        Indirect
    }

    #endregion enum InheritanceType

    #region class InheritanceItem

    public sealed class InheritanceDetail
    {
        private readonly List<InheritanceDetailItem> list;

        public IInterface Owner { get; private set; }

        public ReadOnlyCollection<InheritanceDetailItem> Items
        {
            get { return new ReadOnlyCollection<InheritanceDetailItem>(list); }
        }

        public InheritanceDetail(IInterface owner)
        {
            Owner = owner;
            list = new List<InheritanceDetailItem>();
        }

        internal bool ContainsItem(IInterface @interface)
        {
            return this.list.Any(item => item.Interface == @interface);
        }

        internal void Add(InheritanceDetailItem item)
        {
            this.list.Add(item);
        }
    }

    #endregion class InheritanceItem

    #region class InheritanceDetailItem

    public sealed class InheritanceDetailItem
    {
        public InheritanceType Type { get; private set; }

        public IInterface Interface { get; private set; }

        public InheritanceDetailItem(InheritanceType type, IInterface @interface)
        {
            Type = type;
            Interface = @interface;
        }
    }

    #endregion class InheritanceDetailItem

    #region enum InheritanceListMode

    public enum InheritanceListMode
    {
        CurrentLevel,
        WholeTree
    }

    #endregion enum InheritanceListMode
}