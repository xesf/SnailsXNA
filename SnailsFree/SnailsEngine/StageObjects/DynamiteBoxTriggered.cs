using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.StageObjects
{
  public class DynamiteBoxTriggered : DynamiteBox
  {
    #region Constants
    new public const string ID = "DYNAMITE_BOX_TRIGGERED";
    new public const int EXPLOSION_TIME = 4000;
    #endregion


    public DynamiteBoxTriggered()
      : base(StageObjectType.DynamiteBoxTriggered)
    {
      _counterVisible = false;
      _isActive = false;
    }


    /// <summary>
    /// 
    /// </summary>
    protected override void SnailCollided(Snail snail)
    {
      // Trigger the timer
      this._status = DynamiteBoxStatus.Counting;
      this._counterVisible = true;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
    {
        base.BoxDeployed(addTile, addPaths, checkCollisions);
        this._status = DynamiteBoxStatus.Deployed;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Update(BrainEngine.BrainGameTime gameTime)
    {
        if (this._status == DynamiteBoxStatus.Deployed)
        {
           this.DoQuadtreeCollisions(Stage.QUADTREE_SNAIL_LIST_IDX);
        }

      base.Update(gameTime);
    }

  }
}
