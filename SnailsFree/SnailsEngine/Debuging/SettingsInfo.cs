using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TwoBrainsGames.Snails.Configuration;
using System.Windows.Forms;

namespace TwoBrainsGames.Snails.Debuging
{
    // TODO: also use Engine Settings
    public partial class SettingsInfo : UserControl
    {
        GameSettings _settings;
        public GameSettings Settings
        {
            get { return this._settings; }
            set
            {
                this._settings = value;
                if (this._settings != null)
                {
                    this._cbShowOOBB.Checked = this._settings.ShowBoundingBoxes;
                    this._cbShowFrames.Checked = this._settings.ShowBoardFrames;
                    this._cbShowCoordinates.Checked = this._settings.ShowBoardCoordinates;
                    this._cbShowPaths.Checked = this._settings.ShowPaths;
                    this._txtGravity.Value = (decimal)SnailsGame.GameSettings.Gravity;
                    this._chkShowIds.Checked = this._settings.ShowObjectIds;
                 }
            }
        }

        public SettingsInfo()
        {
            InitializeComponent();
        }

     
        private void _cbShowOOBB_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ShowBoundingBoxes = _cbShowOOBB.Checked;
        }

        private void _cbShowFrames_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ShowBoardFrames = _cbShowFrames.Checked;
        }

        private void _cbShowCoordinates_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ShowBoardCoordinates = _cbShowCoordinates.Checked;
        }

        private void _txtGravity_ValueChanged(object sender, EventArgs e)
        {
            Settings.Gravity = (int)_txtGravity.Value;
        }

        private void _cbShowPaths_CheckedChanged(object sender, EventArgs e)
        {
            Settings.ShowPaths = this._cbShowPaths.Checked;

        }

        private void _chkShowIds_CheckedChanged(object sender, EventArgs e)
        {
            this._settings.ShowObjectIds = this._chkShowIds.Checked;
        }

    }
}
