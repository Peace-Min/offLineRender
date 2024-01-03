using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tree
{
    public class LazyTreeNode
    {
        public ObservableCollection<LazyTreeNode> _children;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public object Tag { get; set; }

        public LazyTreeNode Parent { get; set; }

        public event Action<LazyTreeNode> OnExpanded;

        /// <summary>
        /// 자식노드의 부모가 누구인지할당
        /// </summary>
        public ObservableCollection<LazyTreeNode> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new ObservableCollection<LazyTreeNode>();
                    _children.CollectionChanged += _children_CollectionChanged;
                }
                return _children;
            }
            set => _children = value;
        }

        private void _children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (LazyTreeNode child in e.NewItems)
                {
                    child.Parent = this;
                }
            }
        }


        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded; set
            {
                _isExpanded = value;

                if (_isExpanded)
                {
                    OnExpanded.Invoke(this);
                }
            }
        }

    }
}
