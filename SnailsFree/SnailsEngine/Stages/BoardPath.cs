using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.SpacePartitioning;

namespace TwoBrainsGames.Snails.Stages
{
    public class BoardPath
    {
        #region Properties
        public List<BoardPathNode> Container { get; private set;}
        Quadtree Quadtree { get; set; }
        #endregion

        public BoardPath()
        {
        }

        public BoardPath(Quadtree quadtree)
        {
            this.Container = new List<BoardPathNode>(100);
            this.Quadtree = quadtree;
        }

        public void Clear()
        {
            this.Container.Clear();
        }

        /// <summary>
        /// New path creation to start a new linked list inside the container
        /// </summary>
        /// <param name="node"></param>
        public BoardPathNode AddPathNode(PathSegment segment)
        {
            BoardPathNode newNode = new BoardPathNode(segment);
            this.Container.Add(newNode);
            this.Quadtree.AddObject(newNode, Stage.QUADTREE_PATH_LIST_IDX);
            return newNode;
        }

        /// <summary>
        /// To add a new node connected to the previous one
        /// </summary>
        /// <param name="node"></param>
        /// <param name="segment"></param>
        public BoardPathNode AddNextToPathNode(BoardPathNode node, PathSegment segment)
        {
            BoardPathNode newNode = new BoardPathNode(segment, node); // create node with previous linkage node
            node.Next = newNode; // set linkage to new created node
            this.Container.Add(newNode);
            return newNode;
        }

        public void Remove(BoardPathNode node)
        {
            if (node != null)
            {
                if (node.Previous != null)
                {
                    node.Previous.Next = null; // reset previous node state
                }

                if (node.Next != null)
                {
                    node.Next.Previous = null; // reset next node state
                }

                this.Container.Remove(node);
                this.Quadtree.RemoveObject(node);
            }
        }

        public void RemoveCoicident(BoardPathNode node)
        {
            this.Container.Remove(node);
            this.Quadtree.RemoveObject(node);

            // reset linked nodes
            node.Next = null;
            node.Previous = null;
            node.Value = null;
            node = null;
        }

        /// <summary>
        /// Find the right node where PathSegment has p
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public BoardPathNode FindNode(Vector2 p)
        {
            BoardPathNode node = this.Container.Find(
                delegate(BoardPathNode match)
                {
                    PathSegment seg = match.Value;
                    return seg.P1.X >= p.X && seg.P0.X <= p.X && seg.P1.Y >= p.Y && seg.P0.Y <= p.Y;
                });

            return node;
        }

        public bool ExistsNode(BoardPathNode n)
        {
            return this.Container.Exists(
                delegate(BoardPathNode match)
                {
                    return match.Value == n.Value;
                });
        }

        public BoardPathNode FindSegmentNode(PathSegment seg)
        {
            BoardPathNode node = this.Container.Find(
                delegate(BoardPathNode match)
                {
                    return seg == match.Value;
                });

            return node;
        }
    }
}
