using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages.HUD;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.Snails.Stages
{
    /// <summary>
    /// Holds logic behaviour for the in-game camera 
    /// </summary>
    public class InGameCamera : Camera2D
    {
        #region Consts
        public const float MAX_ZOOM_IN = 1f;
        public const float MAX_ZOOM_OUT = 0.5f;

        #endregion

        #region Vars
        private Stage _stage;
        private BasicEffect _stageRenderEffect;
        private BasicEffect _backgroundEffect;

        // Camera transform effects
        private FlickEffect _flickEffect;
        private StageStartCameraZoomEffect _stageStartEffect; // This is the zoom in on the stage effect on the stage starts
        private ShakeEffect _shakeEffect;
        private PinchEffect _pinchEffect;

        // This sets a point of interest for the camera to follow
        // If active, the camera position is set to the POI on the update
        private Vector2 _pointOfInterest;
        #endregion

        #region Properties

        // Created this props just to make the code easier to read
        private StageHUD Hud { get { return this._stage.StageHUD; } }
        private Board Board { get { return this._stage.Board; } }
        private BoundingSquare StageArea { get { return this._stage.StageHUD._stageArea; } }
        public BasicEffect StageRenderEffect { get { return this._stageRenderEffect ; } }
        public BasicEffect BackgroundEffect { get { return this._backgroundEffect; } }
        public float MaxZoomOut { get; private set; }
        public Vector2 UpperLeftScreenCorner
        {
            get { return this.Position - this.Origin; }
        }
        #endregion

        /// <summary>
        /// This time, I decided to receive the stage instead of using the global Stage.CurrentStage
        /// </summary>
        /// <param name="stage"></param>
        public InGameCamera(Stage stage)
        {
            this._stage = stage;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            // Setup a specific render effect - BrainGame default effect cannot be used
            // This is needed because the camera moves which asks for a specific World matrix
            this._stageRenderEffect = new BasicEffect(BrainGame.Graphics);
            this._stageRenderEffect.TextureEnabled = true;
            this._stageRenderEffect.VertexColorEnabled = true;
            this._stageRenderEffect.World = Matrix.Identity;
            this._stageRenderEffect.View = Matrix.Identity;
            this._stageRenderEffect.Projection = BrainGame.RenderEffect.Projection;

            // Setup background camera
            // Background has a specific camera to make a zoom, tilling textures and scaling in the same sprite is not possible,
            // so we tile the sprite and zoom using a camera
            this._backgroundEffect = new BasicEffect(BrainGame.Graphics);
            this._backgroundEffect.TextureEnabled = true;
            this._backgroundEffect.VertexColorEnabled = true;
            this._backgroundEffect.World = Matrix.Identity;
            this._backgroundEffect.View = Matrix.Identity;
            this._backgroundEffect.Projection = BrainGame.RenderEffect.Projection;

            // Max zoom out in windows is always equal to min zoom out (there's no zoom under windows)
            this.MaxZoomOut = SnailsGame.GameSettings.MaxZoomOut;
            
            // Pre add effects
            // Flick
            this._flickEffect = new FlickEffect(Vector2.Zero);
            this._flickEffect.Active = false;
            this._flickEffect.AutoDeleteOnEnd = false;
            Stage.CurrentStage.Camera.EffectsBlender.Add(this._flickEffect);

            // Stage start camera effect
            this._stageStartEffect = new StageStartCameraZoomEffect(this);
            this._stageStartEffect.OnEnd = this.StageStartEffectEnded;
            this._stageStartEffect.Active = false;
            this._stageStartEffect.AutoDeleteOnEnd = false;
            Stage.CurrentStage.Camera.EffectsBlender.Add(this._stageStartEffect);

            // Camera shake effect
            this._shakeEffect = new ShakeEffect();
            this._shakeEffect.Active = false;
            this._shakeEffect.AutoDeleteOnEnd = false;
            Stage.CurrentStage.Camera.EffectsBlender.Add(this._shakeEffect);

            // camera pitch end effect - Makes a smooth end pitch
            this._pinchEffect = new PinchEffect(this);
            this._pinchEffect.Active = false;
            this._pinchEffect.AutoDeleteOnEnd = false;
            // Disable for now
//            Stage.CurrentStage.Camera.EffectsBlender.Add(this._pinchEffect);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
 	         base.Update(gameTime);
             
             // This may be optimized to only call check camera bounds if: position or scale change
             this.CheckCameraBounds();

             this._stageRenderEffect.World = this.Transform;

             // Set the listener to the center of the current camera
             SnailsGame.SampleManager.SetAudioListenerPosition(this.Position);
        }
        
        /// <summary>
        /// Place origin of the camera at the center of the stage area
        /// </summary>
        public void SetOriginToHudCenter(StageHUD hud)
        {
            this.Origin = hud.HudCenter;
            this.UpdateTransform();
        }


        /// <summary>
        /// Points the camera to the stage center
        /// </summary>
        public void CenterInStage()
        {
            this.Position = new Vector2(Stage.CurrentStage.Board.Width / 2, 
                                        Stage.CurrentStage.Board.Height / 2);
            this.UpdateTransform();
        }

        /// <summary>
        /// Checks if the camera zoom out is at it's maximum
        /// </summary>
        private bool IsMaximumZoomOut()
        {
            return (this.Board.WidthInCameraWorld <= this.StageArea.Width ||
                    this.Board.HeightInCameraWorld <= this.StageArea.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsMaximumZoomIn()
        {
            return (this.Scale == Vector2.One);
        }

        /// <summary>
        /// Sets the camera to full zoom out
        /// </summary>
        public void FullZoomOut()
        {
            this.Scale = Vector2.One;
            this.Zoom(this.MaxZoomOut);
        }

        /// <summary>
        /// Enables the zoom in effect when the stage starts
        /// The stage starts with the camera zoomed out, then when the stage starts, a zoom in is made and the camera travels from
        /// the current position (the stage center) to the stage startup point
        /// </summary>
        public void StageStartupZoomIn(Vector2 pointOfInterest)
        {
            if (this.IsMaximumZoomIn())
            {
                return;
            }
            this._pointOfInterest = pointOfInterest;
            this._stageStartEffect.Reset(this.Scale, pointOfInterest);
            this._stageStartEffect.Active = true;
        }

        /// <summary>
        /// ScaleFactor should be multiplied by scale, not added
        /// </summary>
        public override void Zoom(float scaleFactor)
        {
            // scaleFactor 1 would do nothing, just exit
            if (scaleFactor == 1f)  
            {
                return;
            }
            // Are we zooming out and we are already at max? Just exit
            if (scaleFactor < 1f && this.IsMaximumZoomOut())
            {
                return;
            }
            // Are we zooming in and we are already at min? Just exit
            if (scaleFactor > 1f && this.IsMaximumZoomIn())
            {
                return;
            }

            // Keep zoom within bounds - we have to compute the max/min zoomFactor possible
            // The formula is simple 
            //        new_scale = camera.Scale * factor
            // Just solve the equation for "factor" where new_scale equals min/max scale
            // Max
            // We have to crop the scaleFactor prior to set it on the camera, because the
            // camera position is changed when the zoom is applied the camera would wrongly move if we get a scale > max
            // It's confusing, just crop the factor here
            if (scaleFactor * this.Scale.X > MAX_ZOOM_IN)
            {
                scaleFactor = (MAX_ZOOM_IN / this.Scale.X);
            }
            // Min
            if (scaleFactor * this.Scale.X < MAX_ZOOM_OUT)
            {
                scaleFactor = (MAX_ZOOM_OUT / this.Scale.X);
            }

            base.Zoom(scaleFactor);
            this.CheckCameraBounds();
        }

        /// <summary>
        /// Places the camera at the specified position
        /// </summary>
        public void MoveTo(Vector2 position)
        {
            this.Position = position;
            this.CheckCameraBounds();
        }

        /// <summary>
        /// Places the camera at the origin
        /// </summary>
        public void MoveToOrigin()
        {
            this.Position = new Vector2(0f, 0f);
            this.CheckCameraBounds();
        }

        /// <summary>
        /// Moves the camera by offset
        /// </summary>
        public void MoveByOffset(Vector2 offset)
        {
            this.Position += offset;
            this.Position = new Vector2((int)this.Position.X, (int)this.Position.Y);
            this.CheckCameraBounds();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckLeftCameraBound()
        {
            if (Origin.X - (this.Position.X * this.Scale.X) > this.StageArea.UpperLeft.X)
            {
                this.Position = new Vector2(this.StageArea.UpperLeft.X + (this.Origin.X / this.Scale.X), this.Position.Y);
                this.EndFlickX(); // Ends the flick effect only on the X Axis (this will make the camera slide)
                this.UpdateTransform();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckTopCameraBound()
        {
            if (Origin.Y - (this.Position.Y * this.Scale.Y) > this.StageArea.UpperLeft.Y)
            {
                this.Position = new Vector2(this.Position.X, this.StageArea.UpperLeft.Y + (this.Origin.Y / this.Scale.Y));
                this.EndFlickY(); // Ends the flick effect only on the Y Axis (this will make the camera slide)
                this.UpdateTransform();
            }
        }

        /// <summary>
        /// Check if the camera and scale are out of the valid bounds and corrects if not
        /// </summary>
        private void CheckCameraBounds()
        {

            // When the scale is less then 1f, we have to check if the zoom out isn't too much
            // The scaled board size must fit the stage area
            if (this.Board.WidthInCameraWorld < this.StageArea.Width)
            {
                this.Scale = new Vector2(this.StageArea.Width / this.Board.Width, this.Scale.Y);
                this.RemovePitchEffect();
            }
            if (this.Board.HeightInCameraWorld < this.StageArea.Height)
            {
                this.Scale = new Vector2(this.Scale.X, this.StageArea.Height / this.Board.Height);
                this.RemovePitchEffect();
            }

            // If scale x/y was corrected previously, we have to make them equal. Scale x and y must always be equal or 
            // else there will be streching
            if (this.Scale.X != this.Scale.Y)
            {
                if (this.Scale.X < this.Scale.Y)
                    this.Scale = new Vector2(this.Scale.Y, this.Scale.Y);
                if (this.Scale.Y < this.Scale.X)
                    this.Scale = new Vector2(this.Scale.X, this.Scale.X);
            }

            // Correct scale 
            // Max
            if (this.Scale.X > MAX_ZOOM_IN ||
                this.Scale.Y > MAX_ZOOM_IN)
            {
                this.Scale = new Vector2(MAX_ZOOM_IN, MAX_ZOOM_IN);
                this.RemovePitchEffect();
            }
            // Min
            if (this.Scale.X < MAX_ZOOM_OUT ||
                this.Scale.Y < MAX_ZOOM_OUT)
            {
                this.Scale = new Vector2(MAX_ZOOM_OUT,  MAX_ZOOM_OUT);
                this.RemovePitchEffect();
            }

            // Left
            this.CheckLeftCameraBound();
            // Right
            if ((this.Position.X * this.Scale.X) - this.Origin.X + this.StageArea.UpperRight.X > (this.Board.Width * this.Scale.X))
            {
                this.Position = new Vector2((this.Origin.X - this.StageArea.UpperRight.X + (this.Board.Width * this.Scale.X)) / this.Scale.X, this.Position.Y);
                this.CheckLeftCameraBound(); // This will avoid a ping pong between left and right 
                this.EndFlickX(); // Ends the flick effect only on the X Axis (this will make the camera slide)
                this.UpdateTransform();
            }

            // Top
            this.CheckTopCameraBound();

            // Bottom
            if ((this.Position.Y * this.Scale.Y) - this.Origin.Y + this.StageArea.LowerRight.Y > (this.Board.Height * this.Scale.Y))
            {
                this.Position = new Vector2(this.Position.X, (this.Origin.Y - this.StageArea.LowerRight.Y + (this.Board.Height * this.Scale.Y)) / this.Scale.Y);
                this.CheckTopCameraBound();
                this.EndFlickY(); // Ends the flick effect only on the Y Axis (this will make the camera slide)
                this.UpdateTransform();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void DoFlick(Vector2 speedVector)
        {
            speedVector *= 500f * this.Scale;
            this._flickEffect.Reset(speedVector);
            this._flickEffect.Active = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndFlick()
        {
            this._flickEffect.Active = false;
        }

        /// <summary>
        /// Ends the flick effect on the X axis
        /// </summary>
        public void EndFlickX()
        {
            this._flickEffect.Speed = new Vector2(0f, this._flickEffect.Speed.Y);
            if (this._flickEffect.Speed == Vector2.Zero)
            {
                this.EndFlick();
            }
        }

        /// <summary>
        /// Ends the flick effect on the Y axis
        /// </summary>
        public void EndFlickY()
        {
            this._flickEffect.Speed = new Vector2(this._flickEffect.Speed.X, 0f);
            if (this._flickEffect.Speed == Vector2.Zero)
            {
                this.EndFlick();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StageStartEffectEnded(object param)
        {
            this.Position = this._pointOfInterest;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveStageStartEffect()
        {
            this._stageStartEffect.Active = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shake(int shakeTime, int shakeStrength)
        {
            this._shakeEffect.Reset(shakeTime, shakeStrength);
            this._shakeEffect.Active = true;
        }

        /// <summary>
        /// Applies the pinch effect when the pinch ends
        /// ths pinch end effect is just a smooth zoom that is applied to the camera, to end the pinch smoothly
        /// </summary>
        public void EndPinch(float previousScale, double pinchTime)
        {
            // Disbable for now
       //     float scaleSpeed = ((this.Scale.X - previousScale) / (float)pinchTime) * 50f;
       //     this._pinchEffect.Reset(this.Scale.X, scaleSpeed); // X or Y doesn't matter they are always the same
       //     this._pinchEffect.Active = true;
        }


        /// <summary>
        /// 
        /// </summary>
        private void RemovePitchEffect()
        {
            this._pinchEffect.Active = false;
        }
    }
}
