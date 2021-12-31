using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using LevelEditor.Forms;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.Configuration;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
	public partial class StageDataEditorForm : BaseForm
	{
		StageData _StageData;
		int _SelTileIdx;
		int _SelObjIdx;

        Tile SelectedTile
        {
            get
            {
                if (this._lstTiles.SelectedItem == null)
                    return null;
                return (Tile)this._lstTiles.SelectedItem;
            }
        }

		public StageDataEditorForm()
		{
			InitializeComponent();
			this._SelTileIdx = -1;
			this._SelObjIdx = -1;
		}

		/// <summary>
		/// 
		/// </summary>
		private void StageDataEditorForm_Load(object sender, EventArgs e)
		{
			try
			{
                this._StageData = BrainGame.ResourceManager.Load<StageData>("stages/stagedata", ResourceManager.ResourceManagerCacheType.Static);
                this._StageData.LoadContent(ThemeType.ThemeA);
                this.FillObjectTypeCombo();
				this.RefreshData();
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this, ex);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void FillObjectTypeCombo()
		{
			this._cmbType.Items.Clear();
			foreach (StageObjectType objType in Enum.GetValues(typeof(StageObjectType)))
			{
				this._cmbType.Items.Add(objType);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void RefreshData()
		{
			this._lstTiles.Items.Clear();
			this._pnlTile.Enabled = false;
			foreach (Tile tile in this._StageData.Tiles.Values)
			{
				this._lstTiles.Items.Add(tile);
			}

			this._lstObjects.Items.Clear();
			this._pnlObjects.Enabled = false;
			foreach (StageObject obj in this._StageData.Objects.Values)
			{
				this._lstObjects.Items.Add(obj);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void _lstTiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._SelTileIdx != -1)
				{
					this.SaveTile((Tile)this._lstTiles.Items[this._SelTileIdx]);
				}
				this.SelectTile((Tile) this._lstTiles.Items[this._lstTiles.SelectedIndex]);
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void SelectTile(Tile tile)
		{
			this._SelTileIdx = this._lstTiles.SelectedIndex;
		/*	this._chkWalkTop.Checked = ((tile.WalkFlag & WalkTypes.Top) == WalkTypes.Top);
			this._chkWalkBottom.Checked = ((tile.WalkFlag & WalkTypes.Bottom) == WalkTypes.Bottom);
			this._chkWalkLeft.Checked = ((tile.WalkFlag & WalkTypes.Left) == WalkTypes.Left);
			this._chkWalkRight.Checked = ((tile.WalkFlag & WalkTypes.Right) == WalkTypes.Right);

			this._chkInvertTop.Checked = ((tile.WalkFlag & WalkTypes.InvertTop) == WalkTypes.InvertTop);
			this._chkInvertBottom.Checked = ((tile.WalkFlag & WalkTypes.InvertBottom) == WalkTypes.InvertBottom);
			this._chkInvertLeft.Checked = ((tile.WalkFlag & WalkTypes.InvertLeft) == WalkTypes.InvertLeft);
			this._chkInvertRight.Checked = ((tile.WalkFlag & WalkTypes.InvertRight) == WalkTypes.InvertRight);
*/
      //      tile.InitPathBehavioursFromWalk();
            this._cmbLeftPath.SelectedItem = tile._leftPath;
            this._cmbTopPath.SelectedItem = tile._topPath;
            this._cmbRightPath.SelectedItem = tile._rightPath;
            this._cmbBottomPath.SelectedItem = tile._bottomPath;

			this._txtTileId.Text = tile.Id;
			this._udFrameNr.Value = tile.CurrentFrame;
			this._udStyleGroupId.Value = tile.StyleGroupId;

            this._pnlTile.Enabled = true;
            this._pnlTile.Refresh();

		}

		/// <summary>
		/// 
		/// </summary>
		private void SaveTile(Tile tile)
		{
            tile._leftPath = (PathBehaviour)this._cmbLeftPath.SelectedItem;
            tile._topPath = (PathBehaviour)this._cmbTopPath.SelectedItem;
            tile._rightPath = (PathBehaviour)this._cmbRightPath.SelectedItem;
            tile._bottomPath = (PathBehaviour)this._cmbBottomPath.SelectedItem;
            // Compatible reasons. remove later
            
            /*	if (this._chkWalkTop.Checked)
                                            tile.WalkFlag |= WalkTypes.Top;
                                        if (this._chkWalkBottom.Checked)
                                            tile.WalkFlag |= WalkTypes.Bottom;
                                        if (this._chkWalkLeft.Checked)
                                            tile.WalkFlag |= WalkTypes.Left;
                                        if (this._chkWalkRight.Checked)
                                            tile.WalkFlag |= WalkTypes.Right;
            
                                        if (this._chkInvertTop.Checked)
                                            tile.WalkFlag |= WalkTypes.InvertTop;
                                        if (this._chkInvertBottom.Checked)
                                            tile.WalkFlag |= WalkTypes.InvertBottom;
                                        if (this._chkInvertLeft.Checked)
                                            tile.WalkFlag |= WalkTypes.InvertLeft;
                                        if (this._chkInvertRight.Checked)
                                            tile.WalkFlag |= WalkTypes.InvertRight;
                            */
			tile.Id = this._txtTileId.Text;
			tile.CurrentFrame = (int)this._udFrameNr.Value;
			tile.StyleGroupId = (int)this._udStyleGroupId.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		private void _lstObjects_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._SelObjIdx != -1)
				{
					this.SaveObject((StageObject)this._lstObjects.Items[this._SelObjIdx]);
				}
				this.SelectObject((StageObject)this._lstObjects.Items[this._lstObjects.SelectedIndex]);
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void SelectObject(StageObject obj)
		{
			this._SelObjIdx = this._lstObjects.SelectedIndex;

			this._cmbType.SelectedItem = obj.Type;
			this._txtId.Text = obj.Id;
			this._txtResource.Text = obj.ResourceId;
//			this._txtSprite.Text = obj.Sprite.Id;
			this._chkCanFall.Checked = obj.CanFall;
			this._chkCanCollide.Checked = obj.CanCollide;
			this._chkCanDie.Checked = obj.CanDie;
			this._chkCanDieWithExplosions.Checked = obj.CanDieWithExplosions;
			this._chkCanHoover.Checked = obj.CanHoover;
			this._chkCanWalk.Checked = obj.CanWalk;
			this._chkCanWalkOnWalls.Checked = obj.CanWalkOnWalls;
            this._chkCanDieWithAnyTypeOfExplosion.Checked = obj.CanDieWithAnyTypeOfExplosion;
            this._chkCanDieWithCrates.Checked = obj.CanDieWithCrates;
            this._pnlObjects.Enabled = true;
		}

		/// <summary>
		/// 
		/// </summary>
		private void SaveObject(StageObject obj)
		{
			obj.Type = (StageObjectType)this._cmbType.SelectedItem;
			obj.Id = this._txtId.Text;
			obj.ResourceId = this._txtResource.Text;

			obj.StaticFlags = StageObjectStaticFlags.None;
			if (this._chkCanFall.Checked)
				obj.StaticFlags |= StageObjectStaticFlags.CanFall;
			if (this._chkCanCollide.Checked)
				obj.StaticFlags |= StageObjectStaticFlags.CanCollide;
			if (this._chkCanDie.Checked)
				obj.StaticFlags |= StageObjectStaticFlags.CanDie;
			if (this._chkCanDieWithExplosions.Checked)
				obj.StaticFlags |= StageObjectStaticFlags.CanDieWithExplosions;
			if (this._chkCanHoover.Checked)
				obj.StaticFlags |= StageObjectStaticFlags.CanHoover;
			if (this._chkCanWalk.Checked)
				obj.StaticFlags |= StageObjectStaticFlags.CanWalk;
			if (this._chkCanWalkOnWalls.Checked)
				obj.StaticFlags |= StageObjectStaticFlags.CanWalkOnWalls;
			if (this._chkCanDieWithAnyTypeOfExplosion.Checked)
  				obj.StaticFlags |= StageObjectStaticFlags.CanDieWithAnyTypeOfExplosion;
		}

		/// <summary>
		/// 
		/// </summary>
		private void _btnUndoTileChanges_Click(object sender, EventArgs e)
		{
			try
			{
				this.SelectTile((Tile)this._lstTiles.Items[this._SelTileIdx]);
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void _btnUndoObjChanges_Click(object sender, EventArgs e)
		{
			try
			{
				this.SelectObject((StageObject)this._lstObjects.Items[this._SelObjIdx]);
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void _btnOk_Click(object sender, EventArgs e)
		{
			try
			{
                // Don't save stage data, or else %THEME_ID% will be incorrect!
                /*
                if (this._SelObjIdx != -1)
                {
                    this.SaveObject((StageObject)this._lstObjects.Items[this._SelObjIdx]);
                }
                if (this._SelTileIdx != -1)
                {
                    this.SaveTile((Tile)this._lstTiles.Items[this._SelTileIdx]);
                }
				this.Save(); */
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Save()
		{
		  DataFile dataFile = new DataFile();
		  dataFile.RootRecord = this._StageData.ToDataFileRecord();
		  XmlDataFileWriter writer = new XmlDataFileWriter();
          writer.Write(System.IO.Path.Combine(GameSettings.StageDataOutputFolder, Settings.StageDataFilename), dataFile);
		}

        /// <summary>
        /// 
        /// </summary>
        private void _pnlImage_Paint(object sender, PaintEventArgs e)
        {
#if STAGE_EDITOR
            try
            {
                if (this.SelectedTile != null)
                {
                    System.Drawing.Image bmp = this.SelectedTile.Sprite.Image;
                    int frame = this.SelectedTile.CurrentFrame;
                    Microsoft.Xna.Framework.Rectangle frameRect = this.SelectedTile.Sprite.Frames[frame].Rect;
                    Rectangle rc = new Rectangle(frameRect.Left, frameRect.Top, frameRect.Width, frameRect.Height);
                    e.Graphics.DrawImage(bmp, 1, 1, rc, GraphicsUnit.Pixel);
                }

            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
#endif
        }

        private void _chkCanDieWithExplosions_CheckedChanged(object sender, EventArgs e)
        {

        }

	}
}
