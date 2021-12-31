using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using System.Diagnostics;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.BrainEngine.SpacePartitioning
{
    public class QuadtreeNode
    {
        Quadtree QuadtreeOwner  { get; set; }
        QuadtreeNode ParentNode { get; set; }
        QuadtreeNode[] Nodes { get; set;}
        public BoundingSquare BoundingBox { get; set; }
        // Lists of objects
        public  List<IQuadtreeContainable> [] ObjectLists { get; private set; }
        public bool IsLeafNode
        {
             get { return this.Nodes == null; }
        }

        int ObjectCount { get; set; } // Total objects in this node (sum of all totals in each list)
      
      
        /// <summary>
        /// 
        /// </summary>
        public QuadtreeNode(Vector2 ul, Vector2 lr, QuadtreeNode parentNode, Quadtree quadtreeOwner)
        {
            this.BoundingBox = new BoundingSquare(ul, lr);
            this.ParentNode = parentNode;
            this.QuadtreeOwner = quadtreeOwner;
            this.ObjectLists = new List<IQuadtreeContainable>[this.QuadtreeOwner.ObjectListsPerNode];
            for (int i = 0; i < this.QuadtreeOwner.ObjectListsPerNode; i++)
            {
                this.ObjectLists[i] = new List<IQuadtreeContainable>();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void PutObject(IQuadtreeContainable obj, int listIdx)
        {
#if DEBUG && DEBUG_ASSERTIONS
            // Assert that the object collides with the node BB
            if (!obj.Collides(this.BoundingBox))
              throw new AssertionException("Trying yo assing an object to a node that does not collide with the object.");
#endif
          
            
            this.ObjectLists[listIdx].Add(obj);
            if (obj.QuadtreeNodes == null)
            {
                obj.QuadtreeNodes = new List<QuadtreeNode>();
            }
            obj.QuadtreeNodes.Add(this);
            obj.ObjectListIdx = listIdx;
            this.ObjectCount++;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddObject(IQuadtreeContainable obj, int listIdx)
        {
          if (this.IsLeafNode)
          {
              if (this.ObjectCount < this.QuadtreeOwner.MaxObjectsPerNode ||
                  this.BoundingBox.Width < this.QuadtreeOwner.MinNodeWidth ||
                  this.BoundingBox.Height < this.QuadtreeOwner.MinNodeHeight)
            {
                this.PutObject(obj, listIdx);
                return;
            }
            else
            {
//              this.QuadtreeOwner.DumpToFile("teste1_before.txt", null);
              this.SplitNode();
              this.ReassignObjectsToChildNodes();
  //            this.QuadtreeOwner.DumpToFile("teste1_after.txt", null);
  //            this.QuadtreeOwner.AssertTreeIntegrity();
            }
          }

          this.AssignObjectToChildNode(obj, listIdx);
        }

        /// <summary>
        /// 
        /// </summary>
        void SplitNode()
        {
            this.Nodes = new QuadtreeNode[4];
            float width  = (this.BoundingBox.Width /2);
            float height = (this.BoundingBox.Height /2);
            float x = this.BoundingBox.UpperLeft.X;
            float y = this.BoundingBox.UpperLeft.Y;

            this.Nodes[0] = new QuadtreeNode(new Vector2(x, y), new Vector2(x + width, y + height), this, this.QuadtreeOwner);
            this.Nodes[1] = new QuadtreeNode(new Vector2(x + width, y), new Vector2(x + (width * 2), y + height), this, this.QuadtreeOwner);
            this.Nodes[2] = new QuadtreeNode(new Vector2(x, y + height), new Vector2(x + width, y + (height * 2)), this, this.QuadtreeOwner);
            this.Nodes[3] = new QuadtreeNode(new Vector2(x + width, y + height),new Vector2(x + (width * 2), y + (height * 2)), this, this.QuadtreeOwner);
        }

        /// <summary>
        /// 
        /// </summary>
        void ReassignObjectsToChildNodes()
        {
            for (int j = 0; j < this.ObjectLists.Length; j++)
            {
                for (int i = 0; i < this.ObjectLists[j].Count; i++)
                {
#if DEBUG && DEBUG_ASSERTIONS
                    if (!this.ObjectLists[j][i].Collides(this.BoundingBox))
                    {
                        this.QuadtreeOwner.DumpToFile("obj_out_tree.txt", this.ObjectLists[j][i]);
                        throw new AssertionException("Object is contained in a node that does not collides with it.");
                    }
#endif
                    this.ObjectLists[j][i].QuadtreeNodes.Remove(this);
                    this.AssignObjectToChildNode(this.ObjectLists[j][i], this.ObjectLists[j][i].ObjectListIdx);
                }
                this.ObjectLists[j].Clear();
            }
            this.ObjectCount = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        void AssignObjectToChildNode(IQuadtreeContainable obj, int listIdx)
        {
#if DEBUG && DEBUG_ASSERTIONS
            bool assigned = false;
#endif
          foreach (QuadtreeNode node in Nodes)
          {
            if (obj.Collides(node.BoundingBox))
            {
                node.AddObject(obj, listIdx);
#if DEBUG && DEBUG_ASSERTIONS
              assigned = true;
#endif
            }
          }
#if DEBUG && DEBUG_ASSERTIONS

          if (!assigned)
          {
              this.QuadtreeOwner.DumpToFile("tree_not_assigned.txt", obj);
              throw new AssertionException("Could not assign object to any child.");
          }
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveObject(IQuadtreeContainable obj)
        {
           this.ObjectLists[obj.ObjectListIdx].Remove(obj);
           this.ObjectCount--;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoCollisions(BoundingSquare bs, int listIdx, List<IQuadtreeContainable> objList)
        {
            if (this.ObjectLists[listIdx].Count > 0)
            {
                foreach (IQuadtreeContainable obj in this.ObjectLists[listIdx])
                {
                    if (obj.Collides(bs) && !objList.Contains(obj))
                    {
                        objList.Add(obj);
                    }
                }
            }

            if (this.Nodes != null)
            {
                for (int i = 0; i < this.Nodes.Length; i++)
                {
                    if (this.Nodes[i].BoundingBox.Collides(bs))
                    {
                        this.Nodes[i].DoCollisions(bs, listIdx, objList);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DoCollisions(IQuadtreeContainable obj, int listIdx, List<IQuadtreeContainable> collidingObjects)
        {
            foreach (IQuadtreeContainable objInNode in this.ObjectLists[listIdx])
            {
                if (obj != objInNode && // don't colide with himself
                    obj.ShouldTestCollisions &&
                    objInNode.ShouldTestCollisions &&
                    objInNode.Collides(obj))
                {
                    if (collidingObjects != null)
                    {
                        collidingObjects.Add(objInNode);
                    }
                    else
                    {
                        obj.OnCollide(objInNode, listIdx);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if a object is found (recursive, so it also searches the childs)
        /// </summary>
        public bool Find(IQuadtreeContainable objToFind)
        {
            foreach (List<IQuadtreeContainable> list in this.ObjectLists)
            {
                foreach (IQuadtreeContainable obj in list)
                {
                  if (obj == objToFind)
                  {
                    return true;
                  }
                }
            }

            if (this.IsLeafNode == false)
            {
                foreach (QuadtreeNode node in this.Nodes)
                {
                  if (node.Find(objToFind) == true)
                  {
                    return true;
                  }
                }
            }
            return false;
        }
#if DEBUG

        /// <summary>
        /// Dumps the node to a file (recursive, so it also dumps the childs)
        /// </summary>
        public void DumpToFile(StreamWriter sw, int level, IQuadtreeContainable obj)
        {
          string tab = string.Format("{0, " + (level * 5).ToString() + "}", " ");
          sw.Write(tab);
          sw.WriteLine("".PadRight(30, '-'));
          sw.Write(tab);
          sw.WriteLine(string.Format("Node lvl {0} [x:{1}, y:{2}, xx:{3}, yy:{4}]", level.ToString(), 
                                this.BoundingBox.UpperLeft.X, this.BoundingBox.UpperLeft.Y, 
                                this.BoundingBox.UpperRight.X, this.BoundingBox.LowerRight.Y));
          sw.Write(tab);
          sw.WriteLine(" Leaf:" + this.IsLeafNode.ToString());
          if (obj != null)
          {
            sw.Write(tab);
            sw.Write(string.Format(" Object fits: {0}", obj.Collides(this.BoundingBox)));
            sw.Write(obj.FormatStringToDumpFile(this));
          }
          sw.Write(tab);
          sw.WriteLine(" Objects:" + this.ObjectCount.ToString());
          for (int i = 0; i < this.ObjectLists.Length; i++)
          {
            sw.Write(tab);
            sw.WriteLine(string.Format(" List {0} {1}", i, (this.ObjectLists[i].Count == 0 ? "Empty" : "")));
            for (int j = 0; j < this.ObjectLists[i].Count; j++)
            {
              sw.Write(tab);
              sw.WriteLine(this.ObjectLists[i][j].FormatStringToDumpFile(this));
            }
          }
          if (this.Nodes != null)
          {
            foreach (QuadtreeNode node in this.Nodes)
            {
              node.DumpToFile(sw, level + 1, obj);
            }
          }
          else
          {
            sw.Write(tab);
            sw.WriteLine("No nodes (this.Nodes==null)");
          }
          sw.Write(tab);
          sw.WriteLine("".PadRight(30, '-'));
        }

        /// <summary>
        /// Returns true if the object is contained in any list
        /// </summary>
        private bool Contains(IQuadtreeContainable obj)
        {
          foreach (List<IQuadtreeContainable> objList in this.ObjectLists)
          {
            if (objList.Contains(obj))
              return true;
          }
          return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void AssertNodeIntegrity()
        {
          int objCount = 0;
          foreach (List<IQuadtreeContainable> objList in this.ObjectLists)
          {
            objCount += objList.Count;
            for (int i = 0; i < objList.Count; i++)
            {
              // Assert that no object exists twice in the list?
              if (objList.Count(p => p == objList[i]) > 1)
                throw new AssertionException("Object exists twice in the same list.");
              // Assert that the object stored in fact collides or is contained by the node
              if (!objList[i].Collides(this.BoundingBox))
              {
                  // A change in the BB or position without a correspoding call to StageObject.RepositionObjectInQuatree() may cause this assert
                  throw new AssertionException("Object is in a node, but doesn't collide with it.");
              }
              // Assert that the node is in the object node list
              if (!objList[i].QuadtreeNodes.Contains(this))
                throw new AssertionException("Object is in a list of objects in a node, but the node is not on the object's node list.");
              // Assert that all nodes referenced in the object node list contain the object
              foreach (QuadtreeNode node in objList[i].QuadtreeNodes)
              {
                if (!node.Contains(objList[i]))
                  throw new AssertionException("Node doens't contain an object but the node is in the object node list.");
              }
            }
          }
          // Assert that the total objects in the list is equal to the object counting
          if (objCount != this.ObjectCount)
            throw new AssertionException("Total objects in the list is diferent then the node object counter.");

          // Assert that no objects are stored if we are in a parent node
          if (this.IsLeafNode == false && objCount > 0)
            throw new AssertionException("Parent nodes cannot hold objects.");

          // Assert that if we are in a leaf there are no child nodes
          if (this.IsLeafNode && this.Nodes != null)
            throw new AssertionException("Leaf nodes cannot have childs.");

          if (!this.IsLeafNode)
          {
            // Assert child nodes
            foreach (QuadtreeNode node in this.Nodes)
            {
              node.AssertNodeIntegrity();
            }
          }
        }

        public void GetNodes(IQuadtreeContainable obj, List<QuadtreeNode> nodeList)
        {

            foreach (List<IQuadtreeContainable> objList in this.ObjectLists)
            {
                if (objList.Contains(obj))
                {
                    nodeList.Add(this);
                    break;
                }
            }

            if (!this.IsLeafNode)
            {
                // Assert child nodes
                foreach (QuadtreeNode node in this.Nodes)
                {
                    node.GetNodes(obj, nodeList);
                }
            }
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        public void Draw(Color color, SpriteFont font, SpriteBatch spriteBatch)
        {
            this.DrawNode(color, font, spriteBatch);
          if (!this.IsLeafNode)
          {
            foreach (QuadtreeNode node in Nodes)
            {
                node.Draw(color, font, spriteBatch);
            }
          }

        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawNode(Color color, SpriteFont font, SpriteBatch spriteBatch)
        {
            this.BoundingBox.Draw(color, BrainGame.Instance.ActiveCamera.Position);
            if (this.IsLeafNode)
            {
                spriteBatch.DrawString(font, this.ObjectCount.ToString(), this.BoundingBox.UpperLeft, color, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);
            }
        } 
    }
}
