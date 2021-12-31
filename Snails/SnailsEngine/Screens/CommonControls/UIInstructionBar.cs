using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    class UIInstructionBar : UIControl
    {
        #region Consts
        private const float LABEL_SPACE = 200.0f; // Space between the labels
        #endregion

        #region Vars
        List<UIInstructionLabel> _labels;
        #endregion

        #region Properties

        public override bool Visible
        {
            get 
            {
                return (base.Visible && this.AnyLabelVisible()); 
            }
            set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;
                    if (base.Visible == true) // If the bar was hidden and now becomes visible, hide all labels. It's some kind of reset
                    {
                        this.HideAllLabels();
                    }
                }
            }
        }
        #endregion

        public UIInstructionBar(UIScreen ownerScreen) :
            base(ownerScreen)
        {
            this._labels = new List<UIInstructionLabel>();

            this.ParentAlignment = BrainEngine.UI.AlignModes.Bottom | BrainEngine.UI.AlignModes.Horizontaly;
            // There's a rounding issue with the bottom aligment. The control is 1 pixel off
            // Just add a negative bottom margin to push the control a little bit down
            this.Margins.Bottom = -50.0f;
            this.BackgroundColor = Colors.InstructionBarBackground;
            this.Size = new BrainEngine.UI.Size(ownerScreen.Size.Width, 600.0f);

            this.AcceptControllerInput = false;
            this.RepositionLabels();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddLabel(UIInstructionLabel.LabelActionTypes labelType)
        {
            UIInstructionLabel label = new UIInstructionLabel(this.ScreenOwner, labelType);
            this.Controls.Add(label);
            this._labels.Add(label);
            this.RepositionLabels();
        }

        /// <summary>
        /// The method centers the labels on the screen
        /// </summary>
        private void RepositionLabels()
        {
            float labelsTotalWidth = 0.0f;
            // First step: calculate the total width that visible labels occupy
            foreach (UIInstructionLabel label in this._labels)
            {
                if (label.Visible)
                {
                    labelsTotalWidth += label.Size.Width;
                    labelsTotalWidth += LABEL_SPACE;
                }
            }


            // Calculate the starting position of the first label
            float startPos = (this.Size.Width / 2) - (labelsTotalWidth / 2);

            // Second step: position the labels
            foreach (UIInstructionLabel label in this._labels)
            {
                if (label.Visible)
                {
                    label.Position = new Vector2(startPos, 0.0f);
                    startPos += label.Size.Width + LABEL_SPACE;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool AnyLabelVisible()
        {
            foreach (UIInstructionLabel label in this._labels)
            {
                if (label.Visible)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Instruction labels are only available on the XBox 
        /// </summary>
        public void ShowLabel(UIInstructionLabel.LabelActionTypes labelType)
        {
            foreach (UIInstructionLabel label in this._labels)
            {
                if (label.LabelType == labelType)
                {
                    label.Visible = true;
                    return;
                }
            }
            this.AddLabel(labelType);
            this.OrderLabels();
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideLabel(UIInstructionLabel.LabelActionTypes labelType)
        {
            foreach (UIInstructionLabel label in this._labels)
            {
                if (label.LabelType == labelType)
                {
                    label.Visible = false;
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideAllLabels()
        {
            if (this._labels != null)
            {
                foreach (UIInstructionLabel label in this._labels)
                {
                    label.Visible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            this.RepositionLabels();
        }

        public override void Draw()
        {
            base.Draw();
        }


        /// <summary>
        /// Places the labels in the correct order
        /// The Back button shoudl always be on the right
        /// </summary>
        public void OrderLabels()
        {
            for(int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] as UIInstructionLabel != null)
                {
                    UIInstructionLabel label = (UIInstructionLabel)this.Controls[i];
                    if (label.ControllerKey == UIInstructionLabel.ControllerKeys.B)
                    {
                        this._labels.RemoveAt(i);
                        this._labels.Insert(this._labels.Count, label);

                        this.Controls.RemoveAt(i);
                        this.Controls.Insert(this.Controls.Count, (UIControl)label);
                    }
                }
            }
        }
    }
}
