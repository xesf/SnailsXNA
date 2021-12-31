using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using LevelEditor;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class StageEntrancePropsForm : ObjectPropsBaseForm
    {

        StageEntrance Door
        { get { return (StageEntrance)this.GameObject; } }

        public StageEntrancePropsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._cmbDirection.Items.Add(StageEntrance.EntranceReleaseDirection.Clockwise);
            this._cmbDirection.Items.Add(StageEntrance.EntranceReleaseDirection.CounterClockwise);
            this._cmbDirection.Items.Add(StageEntrance.EntranceReleaseDirection.Both);

            this._numSnailsToRelease.Value = this.Door.SnailsToRelease;
            this._numReleaseInterval.Value = this.Door.IntervalToRelease;
            this._cmbDirection.SelectedItem = this.Door.ReleaseDirection;
            this._chkReleasesKing.Checked = this.Door.ReleasesSnailKing;
            this._numInitialDelay.Value = this.Door.InitialReleaseDelay;
            this._numBeforeKing.Value = this.Door.SnailsToReleaseBeforeKing;

            this._cmbSnailCounters.LoadValues(StageObjectType.SnailCounter);
            if (this.Door.SnailCounter != null)
            {
                this._cmbSnailCounters.SelectedItem = this.Door.SnailCounter;
            }
            this._cmbSnailsType.SelectedItem = this.Door.SnailsToReleaseId;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Door.SnailsToRelease = (int)this._numSnailsToRelease.Value;
            this.Door.InitialReleaseDelay = (int)this._numInitialDelay.Value;
            this.Door.TotalSnailsToRelease = this.Door.SnailsToRelease;
            this.Door.IntervalToRelease = (int)this._numReleaseInterval.Value;
            this.Door.ReleaseDirection = (StageEntrance.EntranceReleaseDirection)this._cmbDirection.SelectedItem;
            this.Door.ReleasesSnailKing = _chkReleasesKing.Checked;
            this.Door.SetSnailCounter(this._cmbSnailCounters.SelectedItem as SnailCounter);
            this.Door.SnailsToReleaseId = (string)this._cmbSnailsType.SelectedItem;
            this.Door.SnailsToReleaseBeforeKing = (int)this._numBeforeKing.Value;
        }
    }
}
