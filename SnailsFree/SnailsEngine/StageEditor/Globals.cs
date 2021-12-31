using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LevelEditor.Forms;

namespace LevelEditor
{
  class Globals
  {
        private static MainForm _MainForm;
        public static MainForm MainForm 
        { 
          get 
          {
            if (Globals._MainForm == null)
              Globals._MainForm = new MainForm();

            return Globals._MainForm;
          }
        }




  }
}
