using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.ToolObjects;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails
{
    public static class BrainEngineExtensions
    {
        public static Sprite GetSpriteTemporary(this ResourceManager res, Tile tile)
        {
            if (!string.IsNullOrEmpty(tile.ResourceId) && !string.IsNullOrEmpty(tile.SpriteId))
            {
                return BrainGame.ResourceManager.GetSpriteTemporary(tile.ResourceId, tile.SpriteId);
            }

            return null;
        }

        public static Sprite GetSpriteStatic(this ResourceManager res, Tile tile)
        {
            if (!string.IsNullOrEmpty(tile.ResourceId) && !string.IsNullOrEmpty(tile.SpriteId))
            {
                return BrainGame.ResourceManager.GetSpriteStatic(tile.ResourceId, tile.SpriteId);
            }

            return null;
        }

        public static Sprite GetSpriteTemporary(this ResourceManager res, StageObject obj)
        {
            if (!string.IsNullOrEmpty(obj.ResourceId) && !string.IsNullOrEmpty(obj.SpriteId))
            {
                return BrainGame.ResourceManager.GetSpriteTemporary(obj.ResourceId, obj.SpriteId);
            }

            return null;
        }

        public static Sprite GetSpriteStatic(this ResourceManager res, StageObject obj)
        {
            if (!string.IsNullOrEmpty(obj.ResourceId) && !string.IsNullOrEmpty(obj.SpriteId))
            {
                return BrainGame.ResourceManager.GetSpriteStatic(obj.ResourceId, obj.SpriteId);
            }

            return null;
        }

        public static Sprite GetSpriteTemporary(this ResourceManager res, ToolObject tool)
        {
            if (!string.IsNullOrEmpty(tool.ResourceId) && !string.IsNullOrEmpty(tool.SpriteId))
            {
                return BrainGame.ResourceManager.GetSpriteTemporary(tool.ResourceId, tool.SpriteId);
            }

            return null;
        }

        public static Sprite GetSpriteStatic(this ResourceManager res, ToolObject tool)
        {
            if (!string.IsNullOrEmpty(tool.ResourceId) && !string.IsNullOrEmpty(tool.SpriteId))
            {
                return BrainGame.ResourceManager.GetSpriteStatic(tool.ResourceId, tool.SpriteId);
            }

            return null;
        }
    }

#if XBOX360 || WP7
    public static class ListExtensions
    {
        /// <summary> 
        /// Removes all elements from the List that match the conditions defined by the specified predicate. 
        /// </summary> 
        /// <typeparam name="T">The type of elements held by the List.</typeparam> 
        /// <param name="list">The List to remove the elements from.</param> 
        /// <param name="match">The Predicate delegate that defines the conditions of the elements to remove.</param> 
        public static int RemoveAll<T>(this System.Collections.Generic.List<T> list, Func<T, bool> match)
        {
            int numberRemoved = 0;

            // Loop through every element in the List, in reverse order since we are removing items. 
            for (int i = (list.Count - 1); i >= 0; i--)
            {
                // If the predicate function returns true for this item, remove it from the List. 
                if (match(list[i]))
                {
                    list.RemoveAt(i);
                    numberRemoved++;
                }
            }

            // Return how many items were removed from the List. 
            return numberRemoved;
        }

        /// <summary> 
        /// Returns true if the List contains elements that match the conditions defined by the specified predicate. 
        /// </summary> 
        /// <typeparam name="T">The type of elements held by the List.</typeparam> 
        /// <param name="list">The List to search for a match in.</param> 
        /// <param name="match">The Predicate delegate that defines the conditions of the elements to match against.</param> 
        public static bool Exists<T>(this System.Collections.Generic.List<T> list, Func<T, bool> match)
        {
            // Loop through every element in the List, until a match is found. 
            for (int i = 0; i < list.Count; i++)
            {
                // If the predicate function returns true for this item, return that at least one match was found. 
                if (match(list[i]))
                    return true;
            }

            // Return that no matching elements were found in the List. 
            return false;
        }

        public static T Find<T>(this List<T> list, Func<T, bool> match)
        {
            if (list == null)
                return default(T);

            foreach (T item in list)
            {
                if (match(item))
                {
                    return item;
                }
            }
            return default(T);
        }
    }
#endif    
}
