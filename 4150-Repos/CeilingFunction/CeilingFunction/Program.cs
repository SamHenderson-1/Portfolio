using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CeilingFunction
{
    class BinaryTree
    {
        private Node root;

        public BinaryTree()
        {
            root = null;
        }

        public void addNode(int num)
        {
            if (root == null)
            {
                root = new Node(num);
            }
            else
            {
                Node newNode = new Node(num);
                if (root == null) 
                {
                    root = newNode;
                }
                else
                {
                    addRecursive(num, root);
                }
            }
        }

        private Node addRecursive(int val, Node node)
        {
            if (node == null)
            {
                node = new Node(val);
            }
            else if (val < node.getValue())
            {
                node.setLHS(addRecursive(val, node.getLHS()));
            }
            else if (val > node.getValue())
            {
                node.setRHS(addRecursive(val, node.getRHS()));
            }
            else if (val == node.getValue())
            {
                node.setRHS(addRecursive(val, node.getRHS()));
            }
            return node;
        }

        static void Main(string[] args)
        {
            string line; 
            bool firstInts = false;
            int numNodes = 0;

            List<BinaryTree> treeHash = new List<BinaryTree>();

            while ((line = Console.ReadLine()) != null && line != "")
            {
                
                if (!firstInts)
                {
                    string[] temp = line.Split();
                    numNodes = int.Parse(temp[1]);
                    firstInts = true;
                }
                else
                {
                    int[] nodes = parseLine(line, numNodes);
                    BinaryTree tree = new BinaryTree();
                    foreach (int i in nodes)
                    {
                        tree.addNode(i);
                    }
                    treeHash.Add(tree);
                }
            }

            
            for (int i = 0, j; i < treeHash.Count - 1; ++i)
            {
                j = i + 1;

                while (j < treeHash.Count)
                {
                    if (checkTreeShapeEquality(treeHash[i].root, treeHash[j].root))
                    {
                        treeHash.RemoveAt(j);
                    }
                    else
                    {
                        ++j;
                    }
                }
            }
            Console.Out.WriteLine(treeHash.Count);
            Console.ReadLine();
        }

       
        private static int[] parseLine(string s, int length)
        {
            string[] temp = s.Split();
            int[] arr = new int[length];
            for(int i = 0; i < length; i++) {
                arr[i] = int.Parse(temp[i]);
            }

            return arr;
        }

        
        private static bool checkTreeShapeEquality(Node lhs, Node rhs)
        {
            
            if (lhs == null && rhs == null) 
            {
                return true;
            }

            
            if ((lhs == null && rhs != null) || (lhs != null && rhs == null)) 
            {
                return false;
            }
                
            
            if (   checkTreeShapeEquality(lhs.getLHS(), rhs.getLHS()) 
                && checkTreeShapeEquality(lhs.getRHS(), rhs.getRHS()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

      
        private class Node
        {
            private int value;
            private Node rhs;
            private Node lhs;

            public Node(int nodeVal)
            {
                value = nodeVal;
                lhs = null;
                rhs = null;
            }

           
            public Node getLHS()
            {
                return lhs;
            }

           
            public void setLHS(Node n)
            {
                lhs = n;
            }

           
            public Node getRHS()
            {
                return rhs;
            }

            
            public void setRHS(Node n)
            {
                rhs = n;
            }

           
            public int getValue()
            {
                return value;
            }
        }
    }
}