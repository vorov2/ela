using System;
using System.Collections.Generic;
using Ela.Runtime;

namespace Ela.Library.Collections
{
	internal class AvlTree
	{
		#region Construction
		internal static readonly AvlTree Empty = new EmptyAvlTree();
		
		protected AvlTree()
		{

		}


		internal AvlTree(ElaValue key, ElaValue value, AvlTree left, AvlTree right)
		{
			Key = key;
			Value = value;
			Left = left;
			Right = right;
			TreeHeight = 1 + Math.Max(Height(left), Height(right));
		}
		#endregion


		#region Nested Classes
		private sealed class EmptyAvlTree : AvlTree
		{
			#region Methods
			internal override AvlTree Add(ElaValue key, ElaValue value)
			{
				return new AvlTree(key, value, this, this);
			}


			internal override AvlTree Remove(ElaValue key)
			{
				throw Exception();
			}


			internal override AvlTree Search(ElaValue key)
			{
				return this;
			}


			internal override IEnumerable<AvlTree> Enumerate()
			{
				yield break;
			}


			private Exception Exception()
			{
				return new Exception("AVL tree is empty.");
			}
			#endregion


			#region Properties
			internal override bool IsEmpty
			{
				get { return true; }
			}


			internal override ElaValue Key
			{
				get { throw Exception(); }
			}


			internal override ElaValue Value
			{
				get { throw Exception(); }
			}


			internal override AvlTree Left
			{
				get { throw Exception(); }
			}


			internal override AvlTree Right
			{
				get { throw Exception(); }
			}
			#endregion
		}
		#endregion
		

		#region Methods
		internal virtual AvlTree Search(ElaValue key)
		{
			var compare = key.CompareTo(Key);

			if (compare == 0)
				return this;
			else if (compare > 0)
				return Right.Search(key);
			else
				return Left.Search(key);
		}


		internal virtual AvlTree Add(ElaValue key, ElaValue value)
		{
			var result = default(AvlTree);

			if (key.CompareTo(Key) > 0)
				result = new AvlTree(Key, Value, Left, Right.Add(key, value));
			else
				result = new AvlTree(Key, Value, Left.Add(key, value), Right);

			return MakeBalanced(result);
		}


		internal virtual AvlTree Remove(ElaValue key)
		{
			var result = default(AvlTree);
			var compare = key.CompareTo(Key);

			if (compare == 0)
			{
				if (Right.IsEmpty && Left.IsEmpty)
					result = Empty;
				else if (Right.IsEmpty && !Left.IsEmpty)
					result = Left;
				else if (!Right.IsEmpty && Left.IsEmpty)
					result = Right;
				else
				{
					var successor = Right;

					while (!successor.Left.IsEmpty)
						successor = successor.Left;

					result = new AvlTree(successor.Key, successor.Value, Left, Right.Remove(successor.Key));
				}
			}
			else if (compare < 0)
				result = new AvlTree(Key, Value, Left.Remove(key), Right);
			else
				result = new AvlTree(Key, Value, Left, Right.Remove(key));

			return MakeBalanced(result);
		}


		internal virtual IEnumerable<AvlTree> Enumerate()
		{
			var stack = new Stack<AvlTree>();

			for (var current = this; !current.IsEmpty || stack.Count > 0; current = current.Right)
			{
				while (!current.IsEmpty)
				{
					stack.Push(current);
					current = current.Left;
				}

				current = stack.Peek();
				stack.Pop();
				yield return current;
			}
		}


		private static int Height(AvlTree tree)
		{
			return tree.IsEmpty ? 0 : tree.TreeHeight;
		}


		private static AvlTree RotateLeft(AvlTree tree)
		{
			if (tree.Right.IsEmpty)
				return tree;

			return new AvlTree(tree.Right.Key, tree.Right.Value,
				new AvlTree(tree.Key, tree.Value, tree.Left, tree.Right.Left),
				tree.Right.Right);
		}


		private static AvlTree RotateRight(AvlTree tree)
		{
			if (tree.Left.IsEmpty)
				return tree;

			return new AvlTree(tree.Left.Key, tree.Left.Value, tree.Left.Left,
				new AvlTree(tree.Key, tree.Value, tree.Left.Right, tree.Right));
		}


		private static AvlTree DoubleLeft(AvlTree tree)
		{
			if (tree.Right.IsEmpty)
				return tree;

			var rotatedRightChild = new AvlTree(tree.Key, tree.Value, tree.Left, RotateRight(tree.Right));
			return RotateLeft(rotatedRightChild);
		}


		private static AvlTree DoubleRight(AvlTree tree)
		{
			if (tree.Left.IsEmpty)
				return tree;

			var rotatedLeftChild = new AvlTree(tree.Key, tree.Value, RotateLeft(tree.Left), tree.Right);
			return RotateRight(rotatedLeftChild);
		}


		private static int Balance(AvlTree tree)
		{
			if (tree.IsEmpty)
				return 0;

			return Height(tree.Right) - Height(tree.Left);
		}


		private static bool IsRightHeavy(AvlTree tree)
		{
			return Balance(tree) >= 2;
		}


		private static bool IsLeftHeavy(AvlTree tree)
		{
			return Balance(tree) <= -2;
		}


		private static AvlTree MakeBalanced(AvlTree tree)
		{
			var result = default(AvlTree);

			if (IsRightHeavy(tree))
			{
				if (IsLeftHeavy(tree.Right))
					result = DoubleLeft(tree);
				else
					result = RotateLeft(tree);
			}
			else if (IsLeftHeavy(tree))
			{
				if (IsRightHeavy(tree.Left))
					result = DoubleRight(tree);
				else
					result = RotateRight(tree);
			}
			else
				result = tree;

			return result;
		}
		#endregion


		#region Properties
		internal virtual ElaValue Key { get; private set; }

		internal virtual ElaValue Value { get; private set; }

		internal virtual AvlTree Left { get; private set; }

		internal virtual AvlTree Right { get; private set; }

		internal int TreeHeight { get; private set; }
		
		internal virtual bool IsEmpty 
		{ 
			get { return false; } 
		}		
		#endregion
	}
}
