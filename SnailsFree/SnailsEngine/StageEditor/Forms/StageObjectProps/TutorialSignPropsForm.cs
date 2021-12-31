using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageObjects;

namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    public partial class TutorialSignPropsForm : ObjectPropsBaseForm
    {
        private TutorialSign Sign 
        {
            get { return (TutorialSign)this.GameObject; }
        }

        public TutorialSignPropsForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        protected override void OnLoadValues()
        {
            base.OnLoadValues();
            this.txtTopics.Text = this.Sign.TopicsString;
           
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnSaveValues()
        {
            base.OnSaveValues();
            this.Sign.TopicsString = this.txtTopics.Text;
        }

    }
}
