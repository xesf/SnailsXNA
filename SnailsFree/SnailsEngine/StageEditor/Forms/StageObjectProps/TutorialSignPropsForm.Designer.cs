namespace TwoBrainsGames.Snails.StageEditor.Forms.StageObjectProps
{
    partial class TutorialSignPropsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.txtTopics = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._gbProps.SuspendLayout();
            this.SuspendLayout();
            // 
            // _gbProps
            // 
            this._gbProps.Controls.Add(this.label3);
            this._gbProps.Controls.Add(this.txtTopics);
            this._gbProps.Controls.Add(this.label2);
            this._gbProps.Size = new System.Drawing.Size(303, 63);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tutorial topic to show";
            // 
            // txtTopics
            // 
            this.txtTopics.Location = new System.Drawing.Point(126, 20);
            this.txtTopics.Name = "txtTopics";
            this.txtTopics.Size = new System.Drawing.Size(84, 20);
            this.txtTopics.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(284, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "(Separate multiple topics with comma. Ex: \"100, 102, 103\")";
            // 
            // TutorialSignPropsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 333);
            this.Name = "TutorialSignPropsForm";
            this.Text = "TutorialSignPropsForm";
            this._gbProps.ResumeLayout(false);
            this._gbProps.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTopics;
    }
}