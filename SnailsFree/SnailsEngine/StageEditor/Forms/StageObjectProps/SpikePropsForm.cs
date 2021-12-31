using TwoBrainsGames.Snails.StageObjects;
using System.Windows.Forms;
using LevelEditor;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class SpikePropsForm : ObjectPropsBaseForm
    {
        private Spikes Spikes
        {
            get { return (Spikes)this.GameObject; }
        }

        public SpikePropsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this._numActivationTime.Value = this.Spikes.ActivationTime;
            this._numReleaseDelay.Value = this.Spikes.StartupDelay;
            this._rbOn.Checked = this.Spikes.TurnedOn;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Spikes.ActivationTime = (int)this._numActivationTime.Value;
            this.Spikes.StartupDelay = (int)this._numReleaseDelay.Value;
            this.Spikes.TurnedOn = (this._rbOn.Checked);
        }
      
        private void _btnOk_Click(object sender, System.EventArgs e)
        {

        }
    }
}
