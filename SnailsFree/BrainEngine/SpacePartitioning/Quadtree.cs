using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.BrainEngine.SpacePartitioning
{
    public class Quadtree
    {
        QuadtreeNode RootNode { get; set; }
        // Max number of objects that a node can hold. 
        // Node is split and objects are reassigned to the new childs if the object count surpasses this
        public int MaxObjectsPerNode { get; set; } 
        // Minimum width of a node. Node is not split if it's size is smaller
        public int MinNodeWidth { get; set; }
        // Minimum height of a node. Node is not split if it's size is smaller
        public int MinNodeHeight { get; set; }
        // Number of object list in a node. Objects lists are stored in an array on the nodes, 
        // so this value has to be set when the tree is created
        // and it cannot be changed
        public int ObjectListsPerNode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Quadtree(BoundingSquare bb, int maxObjectsPerNode, int minNodeWidth, int minNodeHeight, int objectListsPerNode)
        {
            this.MaxObjectsPerNode = maxObjectsPerNode;  
            this.MinNodeWidth = minNodeWidth;  
            this.MinNodeHeight = minNodeHeight;  
            this.ObjectListsPerNode = objectListsPerNode;
            this.RootNode = new QuadtreeNode(bb.UpperLeft, bb.LowerRight, null, this);
        }


        /// <summary>
        /// 
        /// </summary>
        public void AddObject(IQuadtreeContainable obj, int listIdx)
        {

#if (DEBUG && DEBUG_ASSERTIONS)
          // Assert that the object is not already contained in a tree
          if (obj.Quadtree != null || obj.QuadtreeNodes != null)
            throw new AssertionException("Quadtree assertion failed! Cannot add an object that is already contained in the tree.");
#endif

#if (DEBUG && DEBUG_ASSERTIONS)
          if (this.IsObjectInBounds(obj) == false)
            throw new AssertionException("Object to add to the Quadtree is out of the quadtree bounding box.");
#endif

            this.RootNode.AddObject(obj, listIdx);
          obj.Quadtree = this;
#if (DEBUG && DEBUG_ASSERTIONS)
          // Assert that the object was in fact added to the tree
          if (obj.QuadtreeNodes == null || obj.QuadtreeNodes.Count == 0)
            throw new AssertionException("Quadtree assertion failed! Object wasn't added to the tree.");
#endif
        }

        /// <summary>
        /// Object position changed, so the nodes were it is stored might have changed
        /// </summary>
        public void ObjectMoved(IQuadtreeContainable obj)
        {
            if (obj.QuadtreeNodes != null)
            {
                // Object is fully contained in the first node and there's only one node? Nothing changed then
                if (obj.QuadtreeNodes.Count == 1 &&
                    obj.IsContained(obj.QuadtreeNodes[0]))
                {
                  return;
                }
                // Object is intersecting nodes. We don't know what happened then. Remove the object and re-add it
                this.RemoveObject(obj);
                this.AddObject(obj, obj.ObjectListIdx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveObject(IQuadtreeContainable obj)
        {
#if (DEBUG && DEBUG_ASSERTIONS)
            // Assert that the object is contained in the tree
          if (obj.Quadtree != this || obj.QuadtreeNodes == null || obj.QuadtreeNodes.Count == 0)
            throw new AssertionException("Quadtree assertion failed! Object to remove is not on the tree.");
#endif
          if (obj.Quadtree != null && obj.QuadtreeNodes != null)
          {
              foreach (QuadtreeNode node in obj.QuadtreeNodes)
              {
                  node.RemoveObject(obj);
              }
          }
          obj.Quadtree = null;
          obj.QuadtreeNodes = null;
#if (DEBUG && DEBUG_ASSERTIONS)
          // Assert that the object was removed successfully
          if (this.Find(obj))
             throw new AssertionException("Quadtree assertion failed! Object wasn't removed successfully.");
#endif
        }

        /// <summary>
        /// Returns a list of objects that colide with a bounding box
        /// </summary>
        public List<IQuadtreeContainable> GetCollidingObjects(BoundingSquare bs, int listIdx)
        {
            List<IQuadtreeContainable> colObjs = new List<IQuadtreeContainable>();

            this.RootNode.DoCollisions(bs, listIdx, colObjs);

            return colObjs;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IQuadtreeContainable> GetCollidingObjects(IQuadtreeContainable obj, int listIdx)
        {
            List<IQuadtreeContainable> colObjs = new List<IQuadtreeContainable>();
            foreach (QuadtreeNode node in obj.QuadtreeNodes)
            {
                node.DoCollisions(obj, listIdx, colObjs);
            }
            return colObjs;
        }

        /// <summary>
        /// Test collisions with te object in the list with index "listIdx"
        /// </summary>
        public void DoCollisions(IQuadtreeContainable obj, int listIdx)
        {
            if (obj.ShouldTestCollisions == false)
            {
                return;
            }

            foreach (QuadtreeNode node in obj.QuadtreeNodes)
            {
                node.DoCollisions(obj, listIdx, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Find(IQuadtreeContainable obj)
        {
            return this.RootNode.Find(obj);
        }

#if DEBUG
        /// <summary>
        /// Asserts the Quadtree integrity. If something is wrong an exception is throuned
        /// Asserts made:
        ///       -There are no objects in a parent node
        ///       -Objects counts in the nodes are equal to the total objects in all the lists in the node
        ///      -The object exists in the nodes that are in the object's node list
        ///       -There's no object in a node that's not in the objects node list
        ///       -There's no object stored in a wrong node (i.e. the object must collide with the nodes it is stored in)
        ///       -There's no object stored twice in the same list in the same node
        /// Note: This processing is heavy, so there is certainly a performance hit. When the tree is stable, this can be removed
        /// Should be reactivated from time to time
        /// </summary>
        public void AssertTreeIntegrity()
        {
          this.RootNode.AssertNodeIntegrity();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AssertObjectIntegrity(IQuadtreeContainable obj)
        {
            // Asserts that the object is in the tree
            if (obj.Quadtree != this)
                throw new AssertionException("Assertion failed! Object is not on the tree.");
            // Asserts that the object is contained in at least one node
            if (obj.QuadtreeNodes != null && obj.QuadtreeNodes.Count == 0)
                throw new AssertionException("Assertion failed! Object is not on the tree.");
            
            // Get all nodes that has the object in the object lists
            List<QuadtreeNode> nodeList = new List<QuadtreeNode>();
            this.RootNode.GetNodes(obj, nodeList);
            foreach (QuadtreeNode node in obj.QuadtreeNodes)
            {
                // Assert that the node collides with the object
                if (!obj.Collides(node.BoundingBox))
                    throw new AssertionException("Assertion failed! Object is in a node, but the node BB does not intersects the object.");

                // Assert that all nodes that contain the object are in the objects node list
                if (nodeList.Contains(node) == false)
                    throw new AssertionException("Assertion failed! Object is in a node, but the node is not on the object lists.");

                // Asserts that the object is contained in all nodes that are in the node list
                bool found = false;
                foreach (List<IQuadtreeContainable> objList in node.ObjectLists)
                {
                    found = objList.Contains(obj);
                    if (found)
                        break;
                }
                if (!found)
                    throw new AssertionException("Assertion failed! There's a node in the object list nodes that does not has the object in his objecte lists.");
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public void DumpToFile(string filename, IQuadtreeContainable obj)
        {
#if !WIN8
            StreamWriter sw = new StreamWriter(filename);
            this.RootNode.DumpToFile(sw, 0, obj);
            sw.Close();
#endif
#warning TODO Corrigir isto para o Windows 8
        }


#endif

        /// <summary>
        /// 
        /// </summary>
        public void Draw(Color color, SpriteFont font, SpriteBatch spriteBatch)
        {
            this.RootNode.Draw(color, font, spriteBatch);
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsObjectInBounds(IQuadtreeContainable obj)
        {
            return (obj.Collides(this.RootNode.BoundingBox));
        }
    }
}
