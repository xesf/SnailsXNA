using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace TwoBrains.Common.Controls
{
    [Designer(typeof(CollapsiblePanelDesigner))]
    public partial class Group : UserControl
    {
        bool _allowCollapse;
        bool _collapsed;
        int _saveHeight;

        #region Properties
        [Browsable(false)]
        public bool Collapsed
        {
            get { return this._collapsed; }
            set
            {
                this._collapsed = value;
                if (this._collapsed)
                {
                    this._lblCollapse.Text = "+";
                    this._saveHeight = this.Height;
                    this.Height = this._Caption.Height + 5;
                }
                else
                {
                    this._lblCollapse.Text = "-";
                    this.Height = this._saveHeight;
                }
            }

        }
        [Browsable(true)]
        public bool AllowCollapse
        {
            get { return this._allowCollapse; }
            set 
            { 
                this._allowCollapse = value;
                this._lblCollapse.Visible = this._allowCollapse;
            }

        }
        [Browsable(true)]
        public new HScrollProperties HorizontalScroll
        {
            get { return this._PanelBody.HorizontalScroll; }
        }
        [Browsable(true)]
        public new VScrollProperties VerticalScroll
        {
            get { return this._PanelBody.VerticalScroll; } 
          }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel BodyPanel
        {
            get { return this._PanelBody; }
        }

        
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Bindable(true)]
        public override string Text
        {
            get { return this._Caption.Text; }
            set { this._Caption.Text = value; }
        }

        public Color CaptionBackColor
        {
            get { return this._Caption.BackColor; }
            set { this._Caption.BackColor = value; }
        }

        public Color CaptionForeColor
        {
            get { return this._Caption.ForeColor; }
            set { this._Caption.ForeColor = value; }
        }

        [Browsable(true)]
        public bool CaptionVisible
        {
            get { return this._PanelCaption.Visible; }
            set { this._PanelCaption.Visible = value; }
        }
        #endregion

        public Group()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.Collapsed = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public new void PerformLayout()
        {
            base.PerformLayout();
            this._PanelBody.PerformLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, this.Width - 2, this.Height - 2));
            e.Graphics.DrawRectangle(Pens.White, new Rectangle(1, 1, this.Width - 2, this.Height - 2));
        }

        /// <summary>
        /// 
        /// </summary>
        private void _lblCollapse_Click(object sender, EventArgs e)
        {
            this.Collapsed = !this.Collapsed;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CollapsiblePanelDesigner : ParentControlDesigner
    {

        public override void Initialize(System.ComponentModel.IComponent component)
        {

            base.Initialize(component);

            Group TmpCollapsiblePanel = component as Group;

            EnableDesignMode(TmpCollapsiblePanel.BodyPanel, TmpCollapsiblePanel.BodyPanel.Name);
        }
    }
}
