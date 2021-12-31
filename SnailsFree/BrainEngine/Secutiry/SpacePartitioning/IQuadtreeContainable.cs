using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.Collision;
using Microsoft.Xna.Framework;
using System.IO;

namespace TwoBrainsGames.BrainEngine.SpacePartitioning
{
    public interface IQuadtreeContainable
    {
        Quadtree Quadtree { get; set; }
        List<QuadtreeNode> QuadtreeNodes { get; set; }
        int ObjectListIdx { get; set; }
        bool ShouldTestCollisions { get; }
        bool Collides(IQuadtreeContainable obj);
        bool Collides(BoundingSquare obj);
        bool Collides(Vector2 p0, Vector2 p1); // Line collision
        bool IsContained(QuadtreeNode node);
        void OnCollide(IQuadtreeContainable obj, int listIdx);
        bool Contains(Vector2 P);
        string FormatStringToDumpFile(QuadtreeNode currentNode);
    }
}
