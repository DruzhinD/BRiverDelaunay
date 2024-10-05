using System;
using System.Windows.Forms;

namespace EditLib
{
    [Serializable]
    public class FigureTreeNode : TreeNode
    {
        public Figure Figure { get; set; }

        public FigureTreeNode(string text) : base(text) { }
    }
}
