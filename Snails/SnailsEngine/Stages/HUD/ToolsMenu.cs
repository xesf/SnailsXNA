using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.ToolObjects;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.Screens;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.Snails.Stages.HUD
{
    public class ToolsMenu
    {

        #region Members
        private int _selected = -1;
        private int _previousSelected = -1;
        public bool HasToolsToSelect
        {
            // There were bugs with this flag, changed this to a always calculated value
            get
            {
                return this.IsThereToolsToSelect();
            }
        }
        private Vector2 _toolsAreaMargins; // Top /left margins

        private List<ToolObject> _tools = new List<ToolObject>();
    //    private ToolEndMission _endMissionTool;

        private Vector2 _toolSelHelpPosition;
        #endregion

        #region Properties
        public List<ToolObject> Tools
        {
            get
            {
                return _tools;
            }
        }

        public int Width { get { return this._sprite.Width; } }
        public BoundingSquare _ToolsArea;

        Sprite _sprite;
        Vector2 _position;
        int _toolsAreaSpriteBBIdx;
        float _toolBottomMargin;
//        Vector2 _endMissionToolOffset;
        #endregion

        public ToolsMenu()
        { }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ControllerEvents()
        {
            if (SnailsGame.GameSettings.WithToolSelectionShortcutKeys)
            {
                for (int i = 0; i < this._tools.Count; i++)
                {
                    if (this._tools[i].IsSelectable &&
                        this._tools[i].IsToolShortcutPressed()) 
                    {
                        this._selected = i;
                        this.SelectTool();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadContent()
        {
            this.InitFromContent();
            this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "Menu");

//            this._endMissionTool = new ToolEndMission();
//            this._endMissionTool.LoadContent();

            //this._tools.Add(this._endMissionTool);
            foreach (ToolObject tool in _tools)
            {
                tool.LoadContent();
            }

            this._position = new Vector2(SnailsGame.ScreenWidth - this._sprite.Width - SnailsGame.ScreenRectangle.X, 0.0f);
            // Update the tools area - is the bounding box of the area where the tools will fit

            // The tools area depends on the presentation mode
            this._ToolsArea = this._sprite.BoundingBoxes[this._toolsAreaSpriteBBIdx].Transform(this._position); 

            // Make room for the xbox controller help
            if (SnailsGame.GameSettings.UseGamepad)
            {
                Rectangle rc = StageHUD.SpriteXBoxHelp.Frames[StageHUD.XBOX_HELP_TOOL_SEL_FRAME_NR].Rect;
                this._toolsAreaMargins += new Vector2(0f, rc.Height);
                this._toolSelHelpPosition = new Vector2(this._ToolsArea.Left + (this._ToolsArea.Width / 2) - (rc.Width / 2),
                                                        this._ToolsArea.Top + this._toolsAreaMargins.Y - rc.Height);
            }
            this.ComputeToolPositions();
            this.UpdateToolsIndex();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitFromContent()
        {
            DataFileRecord record = BrainGame.ResourceManager.Load<DataFileRecord>("screens\\Gameplay", ResourceManager.ResourceManagerCacheType.Temporary);
            DataFileRecord toolsRecord = record.SelectRecordByField("ToolsMenu\\Settings", "presentation", SnailsGame.GameSettings.PresentationModeString);
            this._toolsAreaSpriteBBIdx = toolsRecord.GetFieldValue<int>("toolsAreaSpriteBBIdx");
            this._toolsAreaMargins = toolsRecord.GetFieldValue<Vector2>("toolsAreaMargins");
            this._toolBottomMargin = toolsRecord.GetFieldValue<float>("toolBottomMargin");
//            this._endMissionToolOffset = toolsRecord.GetFieldValue<Vector2>("endMissionToolOffset");
        }

        /// <summary>
        /// Updates the tool index (corresponds to the array position)
        /// This is needed by the tool to set the frame in the shortcut keys sprite
        /// </summary>
        private void UpdateToolsIndex()
        {
            for (int t = 0; t < _tools.Count; t++)
            {
                this._tools[t]._toolboxIndex = t;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeToolPositions()
        {
            float x = this._ToolsArea.Left + this._toolsAreaMargins.X;
            float y = this._ToolsArea.Top + this._toolsAreaMargins.Y;
            for (int t = 0; t < _tools.Count; t++) // -1 because the last tool is always the END MISSION tool and the end mission tool is aligned at the bottom
            {
                this._tools[t].SetPosition(new Vector2(x, y));
                this._tools[t].UpdateBoundingBox();

                y += this._tools[t].Height + this._toolBottomMargin;
            }
            /*
            if (this._endMissionTool != null)
            {
                Vector2 pos = new Vector2(x, this._ToolsArea.Top + this._ToolsArea.Height - this._endMissionTool.Height);
                pos += this._endMissionToolOffset;
                this._endMissionTool.SetPosition(pos);
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        public void IncrementSelection()
        {
            SetIncrementSelection(1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DecrementSelection()
        {
            SetIncrementSelection(-1);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetIncrementSelection(int value)
        {
            if (!HasToolsToSelect)
                return;

            _selected += value;
            if (_selected >= _tools.Count )
            {
                _selected = 0;
            }
            if (_selected < 0)
            {
                _selected = _tools.Count - 1;
            }

            if (_tools[_selected].IsSelectable)
            {
                SelectTool();
            }
            else
            {
                if (HasToolsToSelect)
                    SetIncrementSelection(value); // to unsure we dont call this recursive function if we don't have at least one valid tool
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(BrainGameTime gameTime)
        {
            this.ControllerEvents();
            foreach (ToolObject tool in _tools)
            {
                tool.Update(gameTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Draw()
        {
            // Draw Background
            this._sprite.Draw(this._position, Stage.CurrentStage.SpriteBatch);

            // Xbox help
            if (SnailsGame.GameSettings.UseGamepad)
            {
                StageHUD.SpriteXBoxHelp.Draw(this._toolSelHelpPosition, StageHUD.XBOX_HELP_TOOL_SEL_FRAME_NR, Stage.CurrentStage.SpriteBatch);
            }

            foreach (ToolObject tool in _tools)
            {
                tool.Draw();
            }

            // Draw end mission icon
//            this._endMissionTool.Draw();

#if DEBUG
            if (BrainGame.Settings.ShowBoundingBoxes)
            {
                this._ToolsArea.Draw(Color.Red, new Vector2(0.0f, 0.0f));
            }
#endif
        }

        /// <summary>
        /// Adds a tool to the tool list
        /// If the EndMission is already added, the tool is added before the EndMission tool
        /// The EndMission tool is always at the end of the list
        /// </summary>
        public void AddTool(ToolObject tool)
        {
            int insertPos = this._tools.Count;
            _tools.Insert(insertPos, tool);
            this.ComputeToolPositions();
            this.UpdateToolsIndex();     
       }

        private bool IsThereToolsToSelect()
        {
            for (int i = 0; i < this._tools.Count; i++)
            {
                if (this._tools[i].IsSelectable)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveTool(ToolObject tool)
        {
            tool.Selected = false;
            _selected = -1;
            _previousSelected = -1;

            this.Tools.Remove(tool);

            this.ComputeToolPositions();
            this.UpdateToolsIndex();
            Stage.CurrentStage.Cursor.SetSelectedTool(this.GetSelectedTool());      
        }

        /// <summary>
        /// 
        /// </summary>
        public ToolObject GetSelectedTool()
        {
            if (this._selected < 0 || this._selected >= this.Tools.Count)
            {
                return null;
            }
            return Tools[_selected];
        }

        /// <summary>
        /// 
        /// </summary>
        private void SelectTool()
        {
            if (_selected != _previousSelected)
            {
                _previousSelected = _selected;
                if (_selected != -1)
                {
                    if (Stage.CurrentStage._state == Stage.StageState.Startup)
                    {
                        Stage.CurrentStage.StartupEnded();
                    }
                    Tools[_selected].OnSelect();
                    for (int t = 0; t < Tools.Count; t++)
                    {
                        Tools[t].Selected = (t == _selected);
                        if (Tools[t].Selected)
                        {
                            Stage.CurrentStage.Cursor.SetSelectedTool(Tools[t]);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsOver(Vector2 pos)
        {
            return this._ToolsArea.Contains(pos);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClickOverTool(Vector2 pos)
        {
            for (int t = 0; t < this.Tools.Count; t++)
            {
                if (!this.Tools[t].Selected &&
                    this.Tools[t].IsSelectable &&
                    this.Tools[t].SelectionFrame.Contains(pos))
                {
                    _selected = t;
                    SelectTool();
                }
            }
            /*
            if (this._endMissionTool.SelectionFrame.Contains(pos))
            {
                this._endMissionTool.Action(pos);
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowToolPickUp(ToolObjectType toolType)
        {
            ToolObject tool = _tools.Find(delegate(ToolObject match) { return match.Type == toolType; });
            return (this._tools.Count < SnailsGame.GameSettings.MaxTools) || (tool != null);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddToolQuantity(ToolObjectType type, int quantity)
        {
            ToolObject tool = _tools.Find(delegate(ToolObject match) { return match.Type == type; });

            if (tool == null)
            {
                tool = (ToolObject)Stage.CurrentStage.StageData.GetTool(ToolObject.ToolTypeToString(type)).Clone();
                tool.LoadContent();
                AddTool(tool);
            }
            tool.Quantity += quantity;
        }

        // This code optimize the above functions.
        public bool AllowAddToolQuantity(ToolObjectType type, int quantity)
        {
            bool canAdd = false;
            ToolObject tool = _tools.Find(delegate(ToolObject match) { return match.Type == type; });

            canAdd = (this._tools.Count < SnailsGame.GameSettings.MaxTools) || (tool != null);
            if (canAdd)
            {
                if (tool == null)
                {
                    tool = (ToolObject)Stage.CurrentStage.StageData.GetTool(ToolObject.ToolTypeToString(type)).Clone();
                    tool.LoadContent();
                    AddTool(tool);
                }
                tool.Quantity += quantity;
            }

            return canAdd;
        }

        /// <summary>
        /// Returns the ammount of tools that are available (sum of all quantities)
        /// </summary>
        public int GetTotalTools()
        {
            int quant = 0;
            for (int t = 0; t < _tools.Count; t++)
            {
                quant += _tools[t].Quantity;
            }
            return quant;
        }
       
        /// <summary>
        /// 
        /// </summary>
        public void MissionStateChanged()
        {
         /*    switch (Stage.CurrentStage.MissionState)
            {
                case Stage.MissionStateType.Completed:
                    this._endMissionTool.State = ToolEndMission.EndMissionState.EndMission;
                    break;
                case Stage.MissionStateType.Starting:
                    this._endMissionTool.State = ToolEndMission.EndMissionState.Start;
                    break;
                default:
                    this._endMissionTool.State = ToolEndMission.EndMissionState.Restart;
                    break;
            }*/
        }

        /// <summary>
        /// 
        /// </summary>
        public void SelectEndMission()
        {
            //this._endMissionTool.Select();
        }

    }
}
