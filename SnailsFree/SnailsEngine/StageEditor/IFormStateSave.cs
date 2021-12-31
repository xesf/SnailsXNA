using System;
using TwoBrainsGames.Snails.StageEditor;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace LevelEditor
{
  interface IFormStateSave
  {
    DataFileRecord CreateSaveSate();
  }
}
