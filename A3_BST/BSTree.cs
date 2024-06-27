using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace A3_BST
{
    internal class BSTree
    {
        public Node Root { get; set; }
        public BSTree()
        {
            Root = null;
        }
        public void Clear()
        {
            Root = null;
        }

        #region Read words from a file and store 
        public void ReadWordsFromFile(string filePath)
        {
            try
            {
                List<string> words = new List<string>(File.ReadAllLines(filePath));
                Clear();

                foreach (string word in words)
                {
                    if (!word.StartsWith("#") && !string.IsNullOrWhiteSpace(word))
                    {
                        Add(word); 
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found: " + ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }
        #endregion

        #region Add Operation
        public void Add(string word)
        {//UI Call
            int alength = word.Length; 
            Node node = new Node(word, alength);

            if (Root == null)
            {
                Root = node;
            }
            else
            {
                InsertNode(Root, node);
            }
        }

        public void InsertNode(Node tree, Node node)
        {
            //1. Compare node for less than node in tree
            if (string.Compare(node.Word, tree.Word) < 0)
            {
                if (tree.Left == null)
                {// 2. left is empty, insert node
                    tree.Left = node;
                }
                else
                {//3. left not empty traverse the tree using recursive call
                    InsertNode(tree.Left, node);
                }
            }
            // 4. Compare node for greater than node in tree
            else if (string.Compare(node.Word, tree.Word) > 0)
            {
                if (tree.Right == null)
                {
                    //5. Right is empty, insert node
                    tree.Right = node;
                }
                else
                {
                    //6. Right not empty, traverse the tree using recursive call
                    InsertNode(tree.Right, node);
                }
            }
        }
        #endregion

        #region Print Order
        //Pre Order
        private string TraversePreOrder(Node node)
        {
            StringBuilder sb = new StringBuilder();
            if (node != null)
            {
                sb.AppendLine(NodeToString(node)); 
                sb.Append(TraversePreOrder(node.Left));
                sb.Append(TraversePreOrder(node.Right));
            }
            return sb.ToString();
        }

        public string PreOrder()
        {
            StringBuilder sb = new StringBuilder();
            if (Root == null)
            {
                sb.Append("TREE is EMPTY");
            }
            else
            {
                sb.Append(TraversePreOrder(Root));
            }
            return sb.ToString();
        }

        // In Order
        private string TraverseInOrder(Node node)
        {
            StringBuilder sb = new StringBuilder();
            if (node != null)
            {
                sb.Append(TraverseInOrder(node.Left));
                sb.AppendLine(NodeToString(node));
                sb.Append(TraverseInOrder(node.Right));
            }
            return sb.ToString();
        }

        public string InOrder()
        {
            StringBuilder sb = new StringBuilder();
            if (Root == null)
            {
                sb.Append("TREE is EMPTY");
            }
            else
            {
                sb.Append(TraverseInOrder(Root));
            }
            return sb.ToString();
        }

        //Post Order
        private string TraversePostOrder(Node node)
        {
            StringBuilder sb = new StringBuilder();
            if (node != null)
            {
                sb.Append(TraversePostOrder(node.Left));
                sb.Append(TraversePostOrder(node.Right));
                sb.AppendLine(NodeToString(node));
            }
            return sb.ToString();
        }

        public string PostOrder()
        {
            StringBuilder sb = new StringBuilder();
            if (Root == null)
            {
                sb.Append("TREE is EMPTY");
            }
            else
            {
                sb.Append(TraversePostOrder(Root));
            }
            return sb.ToString();
        }
        #endregion

        #region Search Operation
        private Node Search(Node tree, Node node)
        {
            if (tree != null)
            {//1. have not reached the end of a brancj
                if (node.Word == tree.Word)
                {//2. found node
                    return tree;
                }
                if (string.Compare(node.Word, tree.Word) < 0)
                {//3. traverse left side
                    return Search(tree.Left, node);
                }
                else
                {//4. traverse right side
                    return Search(tree.Right, node);
                }

            }
            //5. Not found
            return null;
        }

        public string Find(string word)
        {//UI Method call
            Node node = new Node(word, word.Length);
            node = Search(Root, node);
            if (node != null)
            {
                return "Target: " + word.ToString() + ", \n NODE found => " + NodeToString(node);

            }
            else
            {
                return "Target: " + word.ToString() + ", \n NODE NOT found or Tree is empty.";
            }
        }
        #endregion

        #region Delete Operation
        private Node Delete(Node tree, Node node)
        {
            if (tree == null)
            {//1. Reached null side of the tree, return to unload stack
                return tree;
            }
            if (string.Compare(node.Word, tree.Word) < 0)
            {// 2. Traverse left side to find node
                tree.Left = Delete(tree.Left, node);
            }
            else if (string.Compare(node.Word, tree.Word) > 0)
            {// 3. Traverse right side to find node
                tree.Right = Delete(tree.Right, node);
            }
            else
            {//4. Found node to delete
                //check if node has only one child or no child
                if (tree.Left == null)
                {//5. Pull right side of tree up
                    return tree.Right;
                }
                else if (tree.Right == null)
                {//6. pull left side of tree up
                    return tree.Left;
                }
                else
                {
                    //7. node has two leaf nodes, get the InOrder successor node
                    //(the smallest), therefore travers right side and replace
                    //the node found with the current node  / copy로 생각 그리고 다음스텝에 지움
                    tree.Word = MinValue(tree.Right);

                    //8. Traverse the right sode of the tree to delete the InOrder Successor
                    //tree.Right = Delete(tree.Right, tree);
                    //tree.Right = Delete(tree.Right, new Node(tree.Word, tree.Word.Length));
                    tree.Right = Delete(tree.Right, new Node(tree.Word, tree.ALength));
                }

            }
            return tree;
        }

        private string MinValue(Node node)
        {//Finds the minimum node in the rightside of the tree // use int here use node in assignment
            string minval = node.Word;
            while (node.Left != null)
            {//traverse the tree replacing the minval with the 
                //node on the left side of the tree
                //furthest left
                minval = node.Left.Word;
                node = node.Left;
            }
            return minval;

        }

        public string Remove(string word)
        {//UI Call
            Node node = new Node(word, word.Length);
            node = Search(Root, node); // was extra

            if (node != null)
            {
                Root = Delete(Root, node);
                return "Target: " + word.ToString() + ", NODE removed.";

            }
            else
            {
                return "Target: " + word.ToString() + ", NODE NOT found.";
            }
        }
        #endregion


        #region TreeDetails

        private int MaxTreeDepth(Node tree)
        {
            if (tree == null) return 0;
            int left = MaxTreeDepth(tree.Left);
            int right = MaxTreeDepth(tree.Right);

            return Math.Max(left, right) + 1;
        }
        public string TreeDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("******* Tree Details *******");
            sb.AppendLine($"- Root node:   " + Root.ToString());
            sb.AppendLine($"- Max Tree Depth: " + MaxTreeDepth(Root));

            return sb.ToString();
        }


        #endregion

        #region Node Height and Depth Operations
        public int NodeDepth(string word)
        {
            Node node = new Node(word, word.Length);
            return GetNodeDepth(Root, node);
        }

        private int GetNodeDepth(Node tree, Node node)
        {
            if (tree == null)
                return -1; // Node not found

            if (node.Word == tree.Word)
                return GetDepth(tree);

            if (string.Compare(node.Word, tree.Word) < 0)
                return GetNodeDepth(tree.Left, node);
            else
                return GetNodeDepth(tree.Right, node);
        }

        private int GetDepth(Node node)
        {
            if (node == null)
                return -1;

            int leftHeight = GetDepth(node.Left);
            int rightHeight = GetDepth(node.Right);

            return Math.Max(leftHeight, rightHeight) + 1;
        }

        public int NodeHeight(string word)
        {
            Node node = new Node(word, word.Length);
            return GetNodeHeight(Root, node, 0);
        }

        private int GetNodeHeight(Node tree, Node node, int depth)
        {
            if (tree == null)
                return -1; // Node not found

            if (node.Word == tree.Word)
                return depth;

            if (string.Compare(node.Word, tree.Word) < 0)
                return GetNodeHeight(tree.Left, node, depth + 1);
            else
                return GetNodeHeight(tree.Right, node, depth + 1);
        }
        #endregion

        #region Node Count
        public int GetNodeCount()
        {//UI Call
            return CountNodes(Root);
        }

        private int CountNodes(Node node)
        {
            if (node == null)
                return 0;

            return 1 + CountNodes(node.Left) + CountNodes(node.Right);
        }
        #endregion

        private string NodeToString(Node node)
        {
            int height = GetNodeHeight(Root, node, 0);
            int depth = GetNodeDepth(Root, node);
            return $"Word: {node.Word,-10}, Length: {node.ALength,-10}, Height: {height,-10}, Depth: {depth,-10}";
        }

    }
}
