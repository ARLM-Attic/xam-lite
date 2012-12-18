using System.Collections.Generic;

namespace XAMLite
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Children : List<XAMLiteBaseControl>
    {
        /// <summary>
        /// parent of the children.
        /// </summary>
        private XAMLiteGridNew _parent;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Children(XAMLiteGridNew parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Adds the child to the list of children.
        /// </summary>
        /// <param name="child"></param>
        public new void Add(XAMLiteBaseControl child)
        {
            child.IsAttachedToGrid = true;
            
            if (child.Parent == null)
            {
                child.Index = Count;
                child.Parent = _parent;
                //Console.WriteLine("Child Parent: " + child.Index);
            }

            child.Window = _parent.Window;
            base.Add(child);
        }

        /// <summary>
        /// Removes child from the list of children at the specific index.
        /// </summary>
        /// <param name="index"></param>
        public new void RemoveAt(int index)
        {
            _parent.DecreaseChildrenLists(index);
            base.RemoveAt(index);
        }
    }
}
