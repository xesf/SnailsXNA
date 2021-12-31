using System;
using System.Windows.Forms;
using LevelEditor;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using LevelEditor.Forms;
using TwoBrainsGames.Snails.StageEditor.Forms;
using System.IO;
using TwoBrainsGames.Snails.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace TwoBrainsGames.Snails.StageEditor
{
  public class StageEditor 
  {
      private static StageEditor _instance;
      public static StageEditor Instance
      {
          get
          {
              if (StageEditor._instance == null)
              {
                  StageEditor._instance = new StageEditor();
              }
              return StageEditor._instance;
          }
      }
      public static bool IsActive
      {
          get
          {
              if (StageEditor._instance == null)
              {
                  return false;
              }
              return (StageEditor._instance.Visible);
          }
      }
      public static StageData StageData
      {
          get
          {
              return Levels._instance.StageData;
          }
      }

      public static EditorStage CurrentStageEdited
      {
          get { return MainForm.CurrentStageEdited; }
      }

    public const string AppName = "Snails Stage Editor";
    public bool Visible { get; set; }
    public LevelStage CurrentLevelStage { get; set; }

    public StageEditor()
    {
    }

    public void Show()
    {
      this.Show((IWin32Window)null);
    }

    public void Show(IntPtr parentHandle, Levels levels)
    {
        try
        {
            BrainGame.SampleManager.StopAll();
            BrainGame.MusicManager.StopMusic();
            this.SetupEnvironment();
            Settings.Load();
            this.Visible = true;
            LevelStage currentStage = this.CurrentLevelStage;
            Globals.MainForm.ShowDialog(new HwndHandle(parentHandle), levels, ref currentStage);
            this.CurrentLevelStage = currentStage;
            // We have to reload the stage at this point
            // This is needed because Stage.CurrentStage is changed inside the editor
            // The draw would just crash, because LoadContent is not called on added objects
            // Just don't remove this...
            Levels._instance.LoadStage(currentStage.ThemeId, currentStage.StageNr);
        }
        finally
        {
            this.Visible = false;
        }
    }

    public void Show(Levels levels)
    {
      this.Show((IntPtr)null, levels);
    }

    public void Show(IWin32Window owner)
    {
      Globals.MainForm.ShowDialog(owner);
    }

    public void Show(IntPtr parentHandle)
    {
      this.Visible = true;
      this.Show(new HwndHandle(parentHandle));
    }


    public DialogResult LoadCustomLevel()
    {
        LoadCustomStageForm form = new LoadCustomStageForm();
        return form.ShowDialog();
    }


    private void SetupEnvironment()
    {
        Directory.CreateDirectory(SnailsGame.GameSettings.CustomStagesFolder);
    }


    /// <summary>
    /// Returns all custom stages available in disk
    /// This only returns LevelStage for each stage, and not the Stage object
    /// This is because we don't want to load all data
    /// </summary>
    public static List<LevelStage> GetCustomStages()
    {
        List<LevelStage> levelStages = new List<LevelStage>();
        string[] stages = Directory.GetFiles(SnailsGame.GameSettings.CustomStagesFolder, "*." + GameSettings.CustomStagesExtension);
        foreach (string file in stages)
        {
            levelStages.Add(EditorCustomStage.GetLevelStageFromFile(file));
        }

        return levelStages;
    }

    /// <summary>
    /// 
    /// </summary>
    public static System.Drawing.Image GetImageFromResource(string resId)
    {
        string res = "TwoBrainsGames.Snails.StageEditor.Images." + resId.Replace("%THEME%", StageEditor.CurrentStageEdited.LevelStage.ThemeId.ToString());
        res = res.Replace("\\", ".");
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(res);
        if (stream == null)
        {
            throw new SnailsException("Embbeded resource with id [" + res + "] not found.");
        }
        return (System.Drawing.Image)new System.Drawing.Bitmap(stream);

    }
  }

  class HwndHandle : IWin32Window
  {

    public HwndHandle(IntPtr ptr)
    {
      this.Handle = ptr;
    }

    #region IWin32Window Members

    public IntPtr Handle { get; private set; }
    
    #endregion

  }
}
