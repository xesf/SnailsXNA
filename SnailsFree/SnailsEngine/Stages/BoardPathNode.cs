using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Stages
{
    public class BoardPathNode : IQuadtreeContainable
    {
        #region Properties
        public BoardPathNode Next  {get; set;}
        public BoardPathNode Previous {get; set;}
        public PathSegment Value {get; set;}    
        #endregion


        public BoardPathNode(PathSegment segment) :
            this(segment, null)
        {
        }

        public BoardPathNode(PathSegment segment, BoardPathNode previousNode)
        {
            this.Value = segment;
            this.Previous = previousNode;
        }

        #region IQuadtreeContainable Members
        public Quadtree Quadtree { get; set; }
        public List<QuadtreeNode> QuadtreeNodes { get; set; }
        public int ObjectListIdx { get; set; }
        public bool ShouldTestCollisions { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        public bool Collides(IQuadtreeContainable obj)
        {
            if (obj.Contains(this.Value.P0) || obj.Contains(this.Value.P1))
            {
                return true;
            }

            return (obj.Collides(this.Value.P0, this.Value.P1));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Collides(Vector2 p0, Vector2 p1)
        {
            Vector2 v;
            return Mathematics.LineLineIntersection(p0, p1, this.Value.P0, this.Value.P1, out v);
        }
        /// <summary>
        /// 
        /// </summary>
        public void OnCollide(IQuadtreeContainable obj, int listIdx)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsContained(QuadtreeNode node)
        {
            return (node.BoundingBox.Contains(this.Value.P0) &&
                    node.BoundingBox.Contains(this.Value.P1));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Collides(BoundingSquare bs)
        {
            // Important!! Changed in 2012/01/04
            // Small bs was failing the test because P0 and P1 could be outside the BS but the vector could still collide
            // A new PathSegment.Intersecs test was added to take that into account
            // Beware of bugs related with path colisions
           /* return (obj.Contains(this.Value.P0) ||
                    obj.Contains(this.Value.P1));
            */
            
            if (this.Value != null) // to fix null value references
                return this.Value.Intersects(bs);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Contains(Vector2 P)
        {
            return false;
        }

        #endregion

        #region IQuadtreeContainable Members


        public string FormatStringToDumpFile(QuadtreeNode currentNode)
        {
            return this.GetType().ToString();
        }

        #endregion
    }
}
