using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TwoBrainsGames.Snails;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor;

namespace LevelEditor
{
	public class Solution
	{
		#region Constants
		const string DEF_NAME = "noname";
		const string DEF_FILENAME = "noname.seSln";
		#endregion

		#region Variables
		string _Name;
		string _Filename;
		#endregion

		#region Properties
		public Levels Levels { get; set; }
		[Browsable(false)]
		public List<EditorStage> Stages { get; set; }
		[Browsable(false)]
		private bool Changed { get; set; }
		[Browsable(false)]
		public string Name
		{
			get { return this._Name; }
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.Changed = true;
				}
			}
		}

		public string Filename
		{
			get { return this._Filename; }
			set
			{
				if (this._Filename != value)
				{
					this._Filename = value;
					this.Changed = true;
				}
			}
		}
		#endregion

		#region Constructs
		/// <summary>
		/// 
		/// </summary>
		public Solution()
		{
			this.Initialize(new Levels());
		}
		/// <summary>
		/// 
		/// </summary>
		public Solution(Levels levels)
		{
			this.Initialize(levels);
		}

		/// <summary>
		/// 
		/// </summary>
		private void Initialize(Levels levels)
		{
			this.Name = Solution.DEF_NAME;
			this.Filename = Solution.DEF_FILENAME;
			this.Stages = new List<EditorStage>();
			this.Changed = false;
			this.Levels = levels;
			foreach (LevelStage levelStage in this.Levels.Stages)
			{
				this.Stages.Add(new EditorStage(this, levelStage));
			}
		}

		#endregion

		#region Stage methods
		/// <summary>
		/// 
		/// </summary>
		public void AddStage(EditorStage stage)
		{
			this.Stages.Add(stage);
			this.Changed = true;
		}

        /// <summary>
        /// 
        /// </summary>
        public EditorCustomStage LoadCustomStage(string filename)
        {
#if STAGE_EDITOR
            EditorCustomStage customStage = new EditorCustomStage(this);
            customStage.Stage = this.Levels.LoadCustomStage(filename, Stage.StageLoadingContext.StageEditor);
            return customStage;
#else
            throw new SnailsException("Cannot use LoadCustomState(). STAGE_EDITOR not defined.");
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public EditorStage LoadStage(string stageId)
        {
            EditorStage stage = new EditorStage(this);
            stage.Stage = this.Levels.LoadStage(stageId, Stage.StageLoadingContext.StageEditor);
            return stage;
        }
		#endregion

	}
}
