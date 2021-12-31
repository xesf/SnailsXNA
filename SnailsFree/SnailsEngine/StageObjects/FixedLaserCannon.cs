using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails.StageObjects
{
    class FixedLaserCannon : LaserCannonBase
    {
        private const int LASER_BEAM_POSITION_BS_IDX = 0;

        public FixedLaserCannon()
            : base(StageObjectType.FixedLaserCannon)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this._laserBeam.BeamRotation = this.Rotation;
            this._laserBeam.Position = this.TransformSpriteFrameBB(LASER_BEAM_POSITION_BS_IDX).GetCenter();
        }
    }
}
