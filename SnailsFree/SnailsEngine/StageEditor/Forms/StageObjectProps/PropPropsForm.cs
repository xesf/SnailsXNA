using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.StageObjects;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.StageEditor.Forms
{
    public partial class PropPropsForm :  ObjectPropsBaseForm
    {
        Prop PropObject { get { return (Prop)base.GameObject; } }

        public PropPropsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PropPropsForm_Load(object sender, EventArgs e)
        {
            try
            {
                this._cbPropType.Items.Clear();
             /*   foreach (Prop.PropType propType in Enum.GetValues(typeof(Prop.PropType)))
                {
                    this._cbPropType.Items.Add(propType);
                }
                this._cbPropType.SelectedItem = this.PropObject._propType;*/
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PropPropsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
           //     this.PropObject.SetPropType((Prop.PropType)this._cbPropType.SelectedItem);
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this, ex);
            }
        }

        private void _btnOk_Click(object sender, EventArgs e)
        {

        }
    }
}
