using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.SpacePartitioning;

namespace TwoBrainsGames.BrainEngine.Graphics
{

  public class Object2D : IDataFileSerializable
  {
    public enum AnimtionPlaybackModes
    {
      Loop,
      PlayOnce
    }
    #region Constants
    public const float DEFAULT_LAYER_DEPTH = 0.6f;
    #endregion

    #region Members
    protected Vector2 _position;
    protected float _rotation;
    protected float _rotationInRad;

    Sprite _Sprite;

    public bool SpriteAnimationActive;
    public AnimtionPlaybackModes _SpritePlaybackMode;
    public SpriteEffects SpriteEffect;
    public string Id;
    public string UniqueId { get; set; }
    public string ResourceId;
    public string SpriteId;
    public int CurrentFrame;
    public TransformBlender EffectsBlender;
    public TransformBlender SpriteEffectsBlender;

    // Gets at sets the firs element of the array
    public OOBoundingBox BoundingBox;
    public BoundingSquare AABoundingBox;
    public BoundingCircle BoundingCircle;
    public bool BoundingBoxChanged;
    public bool DrawQuadtree;

    protected float _frameUpdateMultiplier; // Use the change the framerate

    protected float LayerDepth;
    protected int FramesPerTime; // set how many seconds you want fps to be calculated
    protected Vector2 SpriteDrawOffset;
    protected double ElapsedGameTime;
    private Color _color;
    protected bool _colorChanged;
    #endregion

    #region Properties
    public Vector2 Position
    {
      get { return this._position; }
      set { this._position = value; }
    }
    public Color BlendColor
    {
        get { return this._color; }
        set
        {
            if (this._color != value)
            {
                this._colorChanged = true;
                this._color = value;
            }
        }
    }

    public float X
    {
      get { return _position.X; }
      set { _position.X = value; }
    }

    public float Y
    {
      get { return _position.Y; }
      set { _position.Y = value; }
    }


    public virtual float Rotation
    {
      get { return _rotation; }
      set
      {
        if (value != _rotation)
        {
          _rotation = value;
          if (_rotation < 0)
              _rotation += 360;

          if (_rotation >= 360)
              _rotation -= 360;
      
          this._rotationInRad = MathHelper.ToRadians(this._rotation);
        }
      }
    }

    public virtual Sprite Sprite
    {
      get { return this._Sprite; }
      set
      {
        if (this._Sprite != value)  // Because each sprite has it's own BB, recompute the BB if
        {                           // the sprite changes
          this._Sprite = value;
          // Now, this can be confusing
          // And it is!! THis is here because if sprite changes, so does the BB
          if (this.Sprite != null)
          {
              this.UpdateBoundingBox();
          }
        }
      }
    }


    public float CurrentFrameWidth
    {
      get { return this.Sprite.Frames[this.CurrentFrame].Width; }
    }
    public float CurrentFrameHeight
    {
      get { return this.Sprite.Frames[this.CurrentFrame].Height; }
    }

    public int MiddleFrameNumber
    {
        get { return this.Sprite.FrameCount / 2; } 
    }

    public Vector2 Scale { get; set; }
    public bool IsHorizontallyFlipped { get { return ((this.SpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally); } }

    public virtual BoundingSquare SoundEmmiterBoundingBox
    {
        get { return new BoundingSquare(); }
    }

    #endregion

    #region Contructors
    /// <summary>
    /// 
    /// </summary>
    public Object2D()
    {
      this.SpriteEffect = SpriteEffects.None;
      this.LayerDepth = Object2D.DEFAULT_LAYER_DEPTH;
      this.FramesPerTime = 1000;
      this.EffectsBlender = new TransformBlender();
      this.SpriteEffectsBlender = new TransformBlender();
      this.SpriteAnimationActive = true;
      this._frameUpdateMultiplier = 1f;
      this.Scale = new Vector2(1f, 1f);
    }

    /// <summary>
    /// 
    /// </summary>
    public Object2D(Object2D other)
    {
      Copy(other);
    }

    public virtual void Copy(Object2D other)
    {
      this._position = other._position;
      this._rotation = other._rotation;
      this.Id = other.Id;
      this.ResourceId = other.ResourceId;
      this.SpriteId = other.SpriteId;
      this.Sprite = other.Sprite;
      this.CurrentFrame = other.CurrentFrame;
      this.LayerDepth = other.LayerDepth;
      this.SpriteEffect = other.SpriteEffect;
      this.FramesPerTime = other.FramesPerTime;
      this.BoundingBox = new OOBoundingBox();
      this.SpriteAnimationActive = other.SpriteAnimationActive;
      this.Scale = other.Scale;
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    protected void UpdateEffects(BrainGameTime gameTime)
    {
      if (this.EffectsBlender.Count > 0)
      {
        this.EffectsBlender.Update(gameTime);
        this.Position += this.EffectsBlender.PositionV2;
        this.Rotation += this.EffectsBlender.Rotation;
        this.Scale += this.EffectsBlender._scale;
        this._color = this.EffectsBlender.Color;
        // update collision box according with changes in Position
        UpdateBoundingBox();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    protected void UpdateSpriteEffects(BrainGameTime gameTime)
    {
      if (this.SpriteEffectsBlender.Count > 0)
      {
        this.SpriteEffectsBlender.Update(gameTime);
        this.SpriteDrawOffset += this.SpriteEffectsBlender.PositionV2;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public void GoToLastFrame()
    {
      this.CurrentFrame = this.Sprite.FrameCount - 1;
    }

    /// <summary>
    /// 
    /// </summary>
    private int GetCurrentFrameTime()
    {
        if (this.Sprite.Frames[this.CurrentFrame].PlayTime == 0)
        {
            if (this.Sprite.Fps == 0)
            {
                return 0;
            }
            return (this.FramesPerTime / this.Sprite.Fps); // Replace this by pre calculated value in Sprite
        }

        return this.Sprite.Frames[this.CurrentFrame].PlayTime;
    }

    
    /// <summary>
    /// 
    /// </summary>
    public void UpdateCurrentFrame(BrainGameTime gameTime)
    {
        if (!this.SpriteAnimationActive)
        {
            return;
        }

        if (this.Sprite != null && this.Sprite.HasAnimations)
        {
           
            this.ElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds * this._frameUpdateMultiplier;
           
            // Change to a new frame?
            int frameTime = this.GetCurrentFrameTime();
            do
            {
                
                if (this.ElapsedGameTime > frameTime)
                {
                    if (this.CurrentFrame == 0)
                    {
                        OnFirstFrame();
                    }

                    OnMiddleFrames();
                    
                    this.CurrentFrame++;// = ((this.CurrentFrame + 1) % this.Sprite.FrameCount);
                    if (this.CurrentFrame >= this.Sprite.FrameCount)
                    {
                        if (this._SpritePlaybackMode == AnimtionPlaybackModes.Loop)
                        {
                            this.CurrentFrame = 0;
                        }
                        else
                        {
                            this.CurrentFrame = this.Sprite.FrameCount - 1;
                        }
                        this.OnLastFrame();
                    }
                    
                    this.ElapsedGameTime -= frameTime;
                    frameTime = this.GetCurrentFrameTime();
                }
            }
            while (this.ElapsedGameTime > frameTime && frameTime != 0);

            if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
            {
                this.UpdateBoundingBox();
            }
          }
          else
          {
            OnFirstFrame();
            OnMiddleFrames();
            OnLastFrame();
          }
    }

    #region Virtual methods
    /// <summary>
    /// 
    /// </summary>
    public virtual void Update(BrainGameTime gameTime)
    {
      this.UpdateEffects(gameTime);
      this.UpdateSpriteEffects(gameTime);
      this.UpdateCurrentFrame(gameTime);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
  
      // I think this will be enough, sprite draw methods now perform the rounding and the layer depth is not needed anymore
        this.Sprite.Draw(this._position, this.CurrentFrame, this._rotation, this.SpriteEffect, this.LayerDepth, Color.White, 1f, spriteBatch);

    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
        this.Sprite.Draw(position, this.CurrentFrame, this._rotation, this.SpriteEffect, this.LayerDepth, color, this.Scale.X, spriteBatch);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void OnFirstFrame()
    { }

    /// <summary>
    /// 
    /// </summary>
    public virtual void OnMiddleFrames()
    { }

    /// <summary>
    /// 
    /// </summary>
    public virtual void OnLastFrame()
    { }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public Vector2 GetAACenter()
    {
        return new Vector2( this.AABoundingBox.Left + (this.AABoundingBox.Width / 2),
                            this.AABoundingBox.Top + (this.AABoundingBox.Height / 2));

    }

      /// <summary>
    /// 
    /// </summary>
    public Vector2 GetCurrentFrameCenter()
    {
        Rectangle rect = this.Sprite.Frames[this.CurrentFrame].Rect;
        return this.Position + new Vector2((rect.Width / 2),
                                           (rect.Height / 2)) - this.Sprite.Offset;

    }

    /// <summary>  
    ///  
    /// </summary>
    public BoundingSquare TransformToObjectSpace(BoundingSquare bs)
    {
        return bs.Transform(-this.Position);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateBoundingBox()
    {
      this.BoundingBox = this.GetBoundingBoxTransformed(0);
      this.UpdateAABoundingBox();
      this.BoundingBoxChanged = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void UpdateBoundingCircle()
    {
#if DEBUG_ASSERTIONS
            if (this.Sprite._boundingSpheres == null)
            {
                throw new BrainException("Sprite does not have bounding spheres.");
            }
#endif
      this.BoundingCircle = this.Sprite._boundingSpheres[0].Transform(this.Position);
    }
    /// <summary>
    /// 
    /// </summary>
    public OOBoundingBox TransformCurrentFrameBB()
    {
      if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
      {
        return this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBox);
      }
      Rectangle rc = new Rectangle((int)-this.Sprite.Offset.X, (int)-this.Sprite.Offset.Y, this.Sprite.Frames[this.CurrentFrame].Rect.Width, this.Sprite.Frames[this.CurrentFrame].Rect.Height);
      return this.TransformBoundingBox(new BoundingSquare(rc));
    }


    /// <summary>
    /// 
    /// </summary>
    public OOBoundingBox TransformSpriteFrameBB(int idx)
    {
      return this.TransformBoundingBox(this.Sprite.BoundingBoxes[idx]);
    }

    /// <summary>
    /// 
    /// </summary>
    public BoundingCircle TransformBoundingCircle(BoundingCircle bs)
    {
        return new BoundingCircle(this.Position + bs._center, bs._radius);
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector2 TransformVector(Vector2 v)
    {
        return Mathematics.TransformVector(v, this.Rotation, this.Position);
    }

    /// <summary>
    /// 
    /// </summary>
    public OOBoundingBox TransformBoundingBox(BoundingSquare bs)
    {
      if (this.IsHorizontallyFlipped)
      {
        BoundingSquare spriteBB = bs;
        // Flip the collision box (we cannot do this in the sprite, because the flip depends on the current frame size
        BoundingSquare bb = new BoundingSquare(new Vector2(-spriteBB.LowerRight.X,
                                            spriteBB.UpperLeft.Y),
                                            spriteBB.Width, spriteBB.Height);
        return bb.Transform(this.Rotation, this.Position);
      }

      return bs.Transform(this.Rotation, this.Position);
    }

    /// <summary>
    /// 
    /// </summary>
    public OOBoundingBox GetBoundingBoxTransformed(int idx)
    {
      return this.TransformBoundingBox(this.GetSpriteBoundingBox(idx));
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateAABoundingBox()
    {
      this.AABoundingBox = this.BoundingBox.ToBoundingSquare();
    }

    #region IDataFileSerializable Members

    public virtual void InitFromDataFileRecord(DataFileRecord record)
    {
      this.Id = record.GetFieldValue<string>("id");

      this.ResourceId = record.GetFieldValue<string>("res", this.ResourceId);
      this.SpriteId = record.GetFieldValue<string>("sprite", this.SpriteId);
      this.CurrentFrame = record.GetFieldValue<int>("frame", this.CurrentFrame);

      bool flipHorizontal = record.GetFieldValue<bool>("flipHorizontally", false);
      if (flipHorizontal)
      {
        SpriteEffect = SpriteEffects.FlipHorizontally;
      }

      bool flipVertical = record.GetFieldValue<bool>("flipVertically", false);
      if (flipVertical)
      {
        SpriteEffect = SpriteEffects.FlipVertically;
      }

      this.Rotation = record.GetFieldValue<float>("rotation", this.Rotation);
    }


    public virtual DataFileRecord ToDataFileRecord()
    {
      DataFileRecord record = new DataFileRecord("Object");


      // The defaults are not writen. This way the xml file is smaller and less confusing
      record.AddField("id", this.Id);
      record.AddField("res", this.ResourceId);
      record.AddField("sprite", this.SpriteId);
      if (this.CurrentFrame != 0)
      {
        record.AddField("frame", this.CurrentFrame);
      }
      if (this.SpriteEffect == SpriteEffects.FlipHorizontally)
      {
        record.AddField("flipHorizontally", true);
      }

      if (this.SpriteEffect == SpriteEffects.FlipVertically)
      {
        record.AddField("flipVertically", true);
      }

      if (this.Rotation != 0)
      {
        record.AddField("rotation", this.Rotation);
      }
      return record;
    }
    #endregion


    /// <summary>
    /// 
    /// </summary>
    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.UniqueId))
        return this.GetType().Name;
      return this.UniqueId;
    }

    /// <summary>
    /// Returns the BB of the sprite. The bb can be by frame. Better not use a property for this because the
    /// results depends on a if. Callers for performance reasons should call this only once in the same method and store the result
    /// <returns></returns>
    public BoundingSquare GetSpriteBoundingBox(int idx)
    {
      //if (this.Sprite.Frames[CurrentFrame].WithCollisionBox == true)
      //	{
      //       return this.Sprite.Frames[CurrentFrame].BoundingBoxes[idx]; // Return frame BB
      //		}

      return this.Sprite.BoundingBoxes[idx]; // Returns the sprite generic BB
    }


  }
}
