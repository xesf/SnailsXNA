using System;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;

namespace TwoBrainsGames.Snails.Debuging
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        public void AddDebugLine(string text)
        {
            this._List.Items.Add(text);
            this._List.TopIndex = this._List.Items.Count - 1;
        }

        public void Refresh(bool refreshList)
        {
            this._SettingsInfo.Settings = SnailsGame.GameSettings;

            if (refreshList)
            {
                this._List.Refresh();
                this._List.TopIndex = this._List.Items.Count - 1;
            }
        }

        private void _btnClear_Click(object sender, EventArgs e)
        {
            this._List.Items.Clear();
        }


        private void RefreshList()
        {
            this._lstSnails.Items.Clear();
            foreach (Snail snail in Stage.CurrentStage.Snails)
            {
                if (snail.IsDead == false)
                  this._lstSnails.Items.Add(snail);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.RefreshList(); 
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _btnKill_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._lstSnails.SelectedItem == null)
                    return;

                foreach (Snail snail in this._lstSnails.SelectedItems)
                {
                    snail.Kill();
                }
                this.RefreshList();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        private void _btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this._lstSnails.Items.Count; i++)
                {
                    if (!this._lstSnails.SelectedItems.Contains(this._lstSnails.Items[i]))
                        ((Snail)(this._lstSnails.Items[i])).Kill();
                }
                this.RefreshList();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        private void _btnKillUnselected_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this._lstSnails.Items.Count; i++)
                {
                    if (!this._lstSnails.SelectedItems.Contains(this._lstSnails.Items[i]))
                        ((Snail)(this._lstSnails.Items[i])).Kill();
                }
                this.RefreshList();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }

        private void _lstSnails_SelectedIndexChanged(object sender, EventArgs e)
        {
#if DEBUG
            try
            {
                foreach(Snail snail in this._lstSnails.Items)
                {
                    snail.Selected = false;
                }
                foreach (Snail snail in this._lstSnails.SelectedItems)
                {
                    snail.Selected = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
#endif
        }

        private void btnSetSnailProps_Click(object sender, EventArgs e)
        {
            try
            {
               
                foreach (Snail snail in this._lstSnails.SelectedItems)
                {
                   snail.DrawQuadtree = this._chkDrawQuadtree.Checked;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
        }
    }
}
