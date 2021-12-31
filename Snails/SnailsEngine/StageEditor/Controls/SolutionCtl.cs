using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoBrainsGames.Snails;
using TwoBrainsGames.Snails.StageEditor;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.StageEditor.Controls;
using System.IO;
using TwoBrainsGames.Snails.Configuration;

namespace LevelEditor.Controls
{
	public partial class SolutionCtl : UserControl
	{
        internal const int IMG_IDX_CUSTOM_STAGES_GROUP = 5;
        internal const int IMG_IDX_FIRST_THEME = 1;

		public enum NodeType
		{
			None,
			Solution,
			Stage,
            Theme,
            CustomStageGroup,
            CustomStage
		}
		#region Events
		public delegate void CancelEventHandler(out bool cancel);
        public delegate void SolutionNodeSelectedHandler(NodeType nodeType);

		public event EventHandler CurrentStageChanged;
        public event CancelEventHandler BeforeCurrentStageChanged;
        public event SolutionNodeSelectedHandler SolutionNodeSelected;

        public event EventHandler StageSelected;
        public event EventHandler CustomStageSelected;
        #endregion

		#region Variables
		Solution _Solution;
		#endregion

		#region Properties
		[Browsable(false)]
		public Solution Solution
		{
			get { return this._Solution; }
			set
			{
				if (this._Solution != value)
				{
					this._Solution = value;
					this.Refresh();
				}
			}
		}

        private CustomStageGroupNode CustomStagesNode
        {
            get;
            set;
        }
        [Browsable(false)]
        public string SelectedStageResourceId
        {
            get
            {
                switch (this.SelectedNodeType)
                {
                    case NodeType.Stage:
                        return ((StageNode)this._tviewSolution.SelectedNode).LevelStage.StageId;
                    case NodeType.CustomStage:
                        return ((CustomStageNode)this._tviewSolution.SelectedNode).LevelStage.CustomStageFilename;
                }
                return null;
            }
        }


		[Browsable(true)]
		public PropertiesCtl PropGrid { get; set; }

		[Browsable(false)]
		TreeNode SolutionNode
		{
			get
			{
				if (this._tviewSolution.Nodes.Count == 0)
					return null;

				return this._tviewSolution.Nodes[0];
			}
		}

		[Browsable(false)]
		NodeType SelectedNodeType
		{
			get
			{
				if (this._tviewSolution.SelectedNode == null)
					return NodeType.None;

				if (this._tviewSolution.SelectedNode.Tag is Solution)
					return NodeType.Solution;

                if (this._tviewSolution.SelectedNode is ThemeNode)
                    return NodeType.Theme; 
                
                if (this._tviewSolution.SelectedNode is CustomStageGroupNode)
                    return NodeType.CustomStageGroup;

                if (this._tviewSolution.SelectedNode is StageNode)
                    return NodeType.Stage;

                if (this._tviewSolution.SelectedNode is CustomStageNode)
                    return NodeType.CustomStage;

				return NodeType.None;
			}
		}
       
		#endregion

		#region Constructs
		/// <summary>
		/// 
		/// </summary>
		public SolutionCtl()
		{
			InitializeComponent();
		}
		#endregion

		#region Solution methods

		/// <summary>
		/// 
		/// </summary>
		public override void Refresh()
		{
			this._tviewSolution.Nodes.Clear();
			if (this.Solution == null)
			{
				return;
			}
			this.CreateSolutionNode();
            this.SelectFirstStage();
            this.SolutionNodeSelected(this.SelectedNodeType);
        }

       

		/// <summary>
		/// 
		/// </summary>
		private TreeNode CreateSolutionNode()
		{
			if (this.Solution == null)
				return null;
			TreeNode node = new TreeNode(this.Solution.Name);
			node.Tag = this.Solution;
			this._tviewSolution.Nodes.Add(node);
			foreach (EditorStage stage in this._Solution.Stages)
			{
				this.AddStageNode(stage.LevelStage);
			}

            // Create custom stage node
            this.AddCustomStagesNode();

			return node;
		}
		#endregion

		#region State methods
		/// <summary>
		/// 
		/// </summary>
		public void AddCustomStage(LevelStage newStage)
		{
            if (newStage.IsCustomStage == false)
            {
                throw new ApplicationException("Stage to add is not a custom stage.");
            }

            CustomStageNode node = new CustomStageNode(newStage);
            this.CustomStagesNode.Nodes.Add(node);
		}

        /// <summary>
        /// 
        /// </summary>
        private void AddCustomStagesNode()
        {
            if (this.SolutionNode == null)
                return;

            this.CustomStagesNode = new CustomStageGroupNode();
            this.SolutionNode.Nodes.Add(this.CustomStagesNode);
            List<LevelStage> levelStages = StageEditor.GetCustomStages();

            foreach (LevelStage levelStage in levelStages)
            {
                CustomStageNode node = new CustomStageNode(levelStage);
                this.CustomStagesNode.Nodes.Add(node);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		private void AddStageNode(LevelStage levelStage)
		{
			if (this.SolutionNode == null)
				return;

            StageNode stageNode = new StageNode(levelStage);

            ThemeNode themeNode = this.GetThemeNode(levelStage.ThemeId, true);
            themeNode.Nodes.Add(stageNode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="themeId"></param>
		/// <returns></returns>
		private ThemeNode GetThemeNode(ThemeType theme, bool createNode)
		{
			if (this.SolutionNode.Nodes == null)
				return null;

			foreach (TreeNode node in this.SolutionNode.Nodes)
			{
				if (node as ThemeNode != null)
				{
                    if (((ThemeNode)node).Theme == theme)
					{
						return (ThemeNode)node;
					}
				}
			}
			if (createNode)
			{
				ThemeNode themeNode = new ThemeNode(theme);
                this.SolutionNode.Nodes.Add(themeNode);
                return themeNode;
			}
			return null;
		}
      

		/// <summary>
		/// Traverses the key looking for the TreeNode that has the stage
        /// It may be a StageNode or a CustomStageNode
		/// </summary>
		private TreeNode FindStageNode(TreeNode parentNode, LevelStage levelStage)
		{
			if (parentNode == null || levelStage.StageKey == null)
				return null;

			foreach (TreeNode node in parentNode.Nodes)
			{
                if (node as StageNode != null)
                {
                    if (((StageNode)node).LevelStage.StageKey == levelStage.StageKey)
					{
                        return node;
					}
				}
                if (node as CustomStageNode != null)
                {
                    if (((CustomStageNode)node).LevelStage.StageKey == levelStage.StageKey)
                    {
                        return node;
                    }
                }

                TreeNode findNode = this.FindStageNode(node, levelStage);
                if (findNode != null)
                {
                    return findNode;
                }
			}
			return null;
		}

        /// <summary>
        /// 
        /// </summary>
        public void SelectStage(LevelStage levelStage)
        {
            this._tviewSolution.SelectedNode = this.FindStageNode(this.SolutionNode, levelStage);
        }

		/// <summary>
		/// 
		/// </summary>
		private void SelectFirstStage()
		{
            this._tviewSolution.SelectedNode = this.SelectFirstStage(this.SolutionNode);
		}

        /// <summary>
        /// 
        /// </summary>
        private StageNode SelectFirstStage(TreeNode node)
        {
            if (node == null)
                return null;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode is StageNode)
                {
                    return (StageNode)childNode;
                }
                StageNode stageNode = this.SelectFirstStage(childNode);
                if (stageNode != null)
                    return stageNode;
            }
            return null;
        }

		#endregion

		#region Treeview events
		/// <summary>
		/// 
		/// </summary>
		private void _tviewSolution_AfterSelect(object sender, TreeViewEventArgs e)
		{
			try
			{
                this.PropGrid.SelectedObject = null;
                switch (this.SelectedNodeType)
                {
                    case NodeType.Stage:
                        this.OnStageSelected();
                        break;

                    case NodeType.CustomStage:
                        this.OnCustomStageSelected();
                        break;
                }

                this.OnSolutionNodeSelected(this.SelectedNodeType);
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this.ParentForm, ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void _tviewSolution_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			try
			{
				bool cancel = false;
				this.OnBeforeCurrentStageChanged(out cancel);
				e.Cancel = cancel;
			}
			catch (System.Exception ex)
			{
				Diag.ShowException(this.ParentForm, ex);
			}
		}
		#endregion

		#region events

        /// <summary>
        /// 
        /// </summary>
        void OnStageSelected()
        {
            if (this.StageSelected != null)
            {
                this.StageSelected(this, new EventArgs());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void OnCustomStageSelected()
        {
            if (this.CustomStageSelected != null)
            {
                this.CustomStageSelected(this, new EventArgs());
            }
        }	
        
        /// <summary>
        /// 
        /// </summary>
        void OnSolutionNodeSelected(NodeType nodeType)
        {
            if (this.SolutionNodeSelected != null)
            {
                this.SolutionNodeSelected(nodeType);
            }
        }	
       

		/// <summary>
		/// 
		/// </summary>
		void OnCurrentStageChanged()
		{
			if (this.CurrentStageChanged != null)
			{
				this.CurrentStageChanged(this, new EventArgs());
			}
		}

        
		/// <summary>
		/// 
		/// </summary>
		void OnBeforeCurrentStageChanged(out bool cancel)
		{
			cancel = false;
			if (this.BeforeCurrentStageChanged != null)
			{
				this.BeforeCurrentStageChanged(out cancel);
			}
		}

		#endregion

        /// <summary>
        /// 
        /// </summary>
        public void RemoveStage(LevelStage levelStage)
        {
            TreeNode node = this.FindStageNode(this.SolutionNode, levelStage);
            if (node == null)
            {
                return;
            }
            node.Parent.Nodes.Remove(node);
        }

    }
}
