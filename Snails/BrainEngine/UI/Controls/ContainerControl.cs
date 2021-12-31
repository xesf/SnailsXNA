using System;
using TwoBrainsGames.BrainEngine.UI.Screens;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    /// <summary>
    /// Control containers should implement this interface
    /// </summary>
    interface IControlContainer
    {
        Screen ParentScreen { get; }
        void DrawControls();
    }

    /// <summary>
    /// This is a helper class for controls that implement IControlContainer interface
    /// </summary>
    class ContainerControl : IControlContainer
    {
        private ControlCollection _controls;

        public ContainerControl(ControlCollection controls)
        {
            this._controls = controls;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DrawControls()
        {
            foreach (UIControl ctl in this._controls)
            {
                if (ctl as IControlContainer != null)
                {
                    ((IControlContainer)ctl).DrawControls();
                }
                ctl.Draw();
            }
        }

        public Screen ParentScreen
        {
            get { throw new NotImplementedException(); }
        }
    }
}
