using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TwoBrainsGames.Snails.StageEditor;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using System.Drawing.Imaging;
using LevelEditor.Forms;
using TwoBrainsGames.Snails.Stages.Hints;

namespace LevelEditor.Controls
{
    public partial class BoardCtl : UserControl
    {

        public enum SelectType
        {
            Tiles,
            Objects,
            Any
        }

        #region Events
        public delegate void CellClickedHandler(int x, int y, int rowx, int rowy, MouseEventArgs mouseArgs);
        public delegate void StageObjectHandler(StageObject obj);

        public event CellClickedHandler CellClicked;
        public event StageObjectHandler StageObjectPropertiesClicked;
        public event EventHandler DeleteItemsClicked;
        public event StageObjectHandler MoveToFrontClicked;
        public event StageObjectHandler SendToBackClicked;

        #endregion

        #region Variables
        private EditorStage _Stage;
        ToolType _SelectedToolType;
        StageObject _CursorObject;
        ObjectBehaviour _ObjBehaviour;
        bool _selectionStarted;
        Point _mouseStartPoint;
        Point _mouseEndPoint;
        List<Object> _selectedItems;
        bool _dragStarted;
        Point _mouseDragOffset;
        Font _gridFont;
        bool DisableDraw;
        #endregion

        #region Properties
        [Browsable(false)]
        public List<Object> SelectedItems
        {
            get
            {
                if (this._selectedItems == null)
                {
                    this._selectedItems = new List<object>();
                }
                return this._selectedItems;
            }
            set
            {
                this._selectedItems = value;
            }
        }

        public List<StageObject> SelectedObjects
        {
            get
            {
                List<StageObject> list = new List<StageObject>();

                if (this.SelectedItems.Count > 0)
                {
                    foreach (Object obj in this.SelectedItems)
                    {
                        if (obj is StageObject)
                        {
                            list.Add((StageObject)obj);
                        }

                    }
                }
                return list;
            }
        }

        public bool SelectedObjectsAllowRotation
        {
            get
            {
                foreach (StageObject obj in this.SelectedObjects)
                {
                    if (obj.EditorBehaviour.AllowRotation)
                    {
                        return true;
                    }
                }
                return false;
            }

        }
        public SelectionType SelectionMode
        {
            get; set;
        }

        [Browsable(false)]
        public ToolType SelectedToolType
        {
            get
            {
                return this._SelectedToolType;
            }
            set
            {
                if (this._SelectedToolType != value)
                {
                    this._SelectedToolType = value;
                    this.Cursor.Visible = (this._SelectedToolType != ToolType.Select);

                    this.Refresh();
                }
            }
        }

        [Browsable(false)]
        public EditorStage Stage
        {
            get { return this._Stage; }
            set
            {
                if (this._Stage != value)
                {
                    this._Stage = value;
                    this.CurrentStageChanged();
                }

                this.Visible = (this._Stage != null);
            }
        }

        Pen TileSelectionPen { get; set; }
        Pen LinkPen { get; set; }
        Pen LinkPenSelObject { get; set; }
        Pen LinkPenBack { get; set; }

        public StageObject CursorObject 
        { 
            get { return this._CursorObject; } 
            set
            {
                if (this._CursorObject != value)
                {
                    this._CursorObject = value;
                    this._ObjBehaviour = null;
                    if (this._CursorObject != null)
                    {
                        this._ObjBehaviour = Settings.GetObjectBehaviour(this._CursorObject.Id);
                    }
                }
                if (this._CursorObject != null)
                {
                    this.Cursor.Set(this._CursorObject);
                }
            }
        }

        Point CursorObjectPos { get; set; }
        bool ShowCursorObject { get; set; }
        Pen HiLightedCellPen { get; set; }
        Pen HiLightedCellPenBack { get; set; }

        public new BoardCursor Cursor { get; private set; }
        Color GridColor { get; set; }
        Pen GridPen { get; set; }
        Brush GridBrush { get; set; }

        bool ShouldHightLightGridCell
        {
            get
            {
                if (this.SelectedToolType == ToolType.Tile ||
                    this.SelectedToolType == ToolType.Prop)
                    return true;
                if (this.SelectedToolType == ToolType.GameObject)
                {
                    if (this._ObjBehaviour == null)
                        return true;

                    return (this._ObjBehaviour.TilePlacementX != ObjectBehaviour.TilePlacement.Arbitrary &&
                            this._ObjBehaviour.TilePlacementY != ObjectBehaviour.TilePlacement.Arbitrary);
                }

                return false;
            }

        }

        Rectangle SelectionRect
        {
            get
            {
                Point pt = this._mouseStartPoint;
                if (this._mouseEndPoint.X < this._mouseStartPoint.X)
                    pt.X = this._mouseEndPoint.X;
                if (this._mouseEndPoint.Y < this._mouseStartPoint.Y)
                    pt.Y = this._mouseEndPoint.Y;

                return new Rectangle(pt.X, pt.Y,
                                     Math.Abs(this._mouseStartPoint.X - this._mouseEndPoint.X),
                                     Math.Abs(this._mouseStartPoint.Y - this._mouseEndPoint.Y));
            }
        }

        StageObject SelectedObject
        {
            get 

            {
                if (this.SelectedItems.Count == 0)
                    return null;
                return (this.SelectedItems[0] as StageObject);
            }
        }
        #endregion

        #region Constructs
        /// <summary>
        /// 
        /// </summary>
        public BoardCtl()
        {
            InitializeComponent();
            this.TileSelectionPen = new Pen(Color.White);
            this.TileSelectionPen.DashPattern = new float[] { 3.0F, 3.0F };
            this.TileSelectionPen.Width = 2.0F;
            this.TileSelectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;

            

            this.HiLightedCellPen = new Pen(Brushes.Green);
            this.HiLightedCellPen.Width = 3.0F;
            this.HiLightedCellPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            this.HiLightedCellPenBack = new Pen(Brushes.White);
            this.HiLightedCellPenBack.Width = 3.0F;

            this.Cursor = new BoardCursor(this);

            this._gridFont = new Font("arial", 8, FontStyle.Regular);

            this.LinkPen = new Pen(Brushes.Yellow);
            this.LinkPen.Width = 2.0F;
            this.LinkPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.LinkPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            this.LinkPenSelObject = new Pen(Brushes.Green);
            this.LinkPenSelObject.Width = this.LinkPen.Width;
            this.LinkPenSelObject.DashStyle = this.LinkPen.DashStyle;
            this.LinkPenSelObject.EndCap = this.LinkPen.EndCap;

            this.LinkPenBack = new Pen(Brushes.Black);
            this.LinkPenBack.Width = this.LinkPen.Width;
            
        }
        #endregion


        #region Control events
        /// <summary>
        /// 
        /// </summary>
        private void DrawGrid(Graphics graphics)
        {
            for (int i = 1; i < this.Stage.Size.Width; i++)
            {
                graphics.DrawLine(this.GridPen, i * this.Stage.TileSize.Width, 0,
                                              i * this.Stage.TileSize.Width, this.Stage.SizeInPixels.Height);

                if ((i + 1) % 5 == 0)
                {
                    graphics.DrawString((i + 1).ToString(), this._gridFont, this.GridBrush, new PointF((i * this.Stage.TileSize.Width) + 3, 3));
                }

            }

            for (int i = 1; i < this.Stage.Size.Height; i++)
            {
                graphics.DrawLine(this.GridPen, 0, i * this.Stage.TileSize.Height,
                                                this.Stage.SizeInPixels.Width, i * this.Stage.TileSize.Height);
                if ((i + 1) % 5 == 0)
                {
                    graphics.DrawString((i + 1).ToString(), this._gridFont, this.GridBrush, new PointF(3, (i * this.Stage.TileSize.Height) + 3));
                }
            }
        }

        private Rectangle GetCameraRectangle(int sX, int sY)
        {
            int centerX = (int)this.Stage.StartupCameraPosition.X;
            int centerY = (int)this.Stage.StartupCameraPosition.Y;
            int x = (centerX - 1) * this.Stage.TileSize.Width + (this.Stage.TileSize.Width / 2);
            int y = (centerY - 1) * this.Stage.TileSize.Height + (this.Stage.TileSize.Height / 2);
            int originX = sX/2;
            int originY = sY/2;
            int rX = x - originX;
            int rY = y - originY;
            if (rX < 0)
                rX = 0;
            if (rY < 0)
                rY = 0;
            if (rX > this.Stage.SizeInPixels.Width)
                rX = this.Stage.SizeInPixels.Width;
            if (rY > this.Stage.SizeInPixels.Height)
                rY = this.Stage.SizeInPixels.Height;
            return new Rectangle(rX, rY, sX, sY);
        }

        private void DrawCameraFrame(Graphics graphics)
        {
            graphics.DrawRectangle(new Pen(Color.Red, 1), GetCameraRectangle(800-140,480)); // wp7
            graphics.DrawRectangle(new Pen(Color.Red, 1), GetCameraRectangle(1280-140,720)); // native
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawObjectLinks(Graphics graphics)
        {
            // Objects
            foreach (StageObject obj in this.Stage.Stage.Objects)
            {
                foreach (StageObject objLink in obj.LinkedObjects)
                {
                    Point pt1 = new Point((int)obj.GetCurrentFrameCenter().X, (int)obj.GetCurrentFrameCenter().Y);
                    Point pt2 = new Point((int)objLink.GetCurrentFrameCenter().X, (int)objLink.GetCurrentFrameCenter().Y);

                    Pen pen = this.LinkPen;
                    if (obj.Selected)
                    {
                        pen = this.LinkPenSelObject;
                        pt1 = new Point(pt1.X + _mouseDragOffset.X, pt1.Y + _mouseDragOffset.Y);
                    }
                    if (objLink.Selected)
                    {
                        pt2 = new Point(pt2.X + _mouseDragOffset.X, pt2.Y + _mouseDragOffset.Y);
                    }
                    graphics.DrawLine(this.LinkPenBack, pt1, pt2);

                    graphics.DrawLine(pen, pt1, pt2);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawObjects(Graphics graphics, bool foreground)
        {
            // Objects
            foreach (StageObject obj in this.Stage.Stage.Objects)
            {
                bool draw = true;
                if ((obj.DrawInForeground && foreground == false) ||
                    (obj.DrawInForeground == false && foreground == true) ||
                    (obj.Selected && foreground == false))
                {
                    draw = false;
                }
                if (obj.Selected && foreground) // Selected object are always display in foreground
                {
                    draw = true;
                }
             
                int offsetX = 0;
                int offsetY = 0;
                if (this._dragStarted && obj.Selected)
                {
                    offsetX = this._mouseDragOffset.X;
                    offsetY = this._mouseDragOffset.Y;
                }
                if (draw)
                {
                    if (obj is Liquid)
                    {
                        Liquid w = obj as Liquid;
                        Color color = Color.FromArgb(180, Color.Blue);
                        if (w is Acid)
                        {
                            color = Color.FromArgb(180, w.BlendColor.R, w.BlendColor.G, w.BlendColor.B);
                        }
                        else if (w is Lava)
                        {
                            color = Color.FromArgb(180, Color.DarkOrange);
                        }
                        obj.DrawWater(graphics, offsetX, offsetY, this._dragStarted, (int)w.Size.X, (int)w.Size.Y, color);
                    }
                    else
                    {
                        obj.Draw(graphics, offsetX, offsetY, this._dragStarted);
                    }
                }

                if (foreground)
                {
                    obj.DrawSelectionRect(graphics, offsetX, offsetY, this._dragStarted);
                    int px = (int)obj.Position.X - (int)obj.Sprite.OffsetX + offsetX;
                    int py = (int)obj.Position.Y - (int)obj.Sprite.OffsetY + offsetY;
                    obj.DrawObjectProps(graphics, px, py);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void DrawHints(Graphics graphics)
        {
            if (this.Stage.Stage.HintManager.CurrentHint == null)
            {
                return;
            }

            // 
            foreach (IHintItem hintItem in this.Stage.Stage.HintManager.CurrentHint.HintItems)
            {
                if (hintItem.ItemObject is StageObject)
                {
                    StageObject obj = (StageObject)hintItem.ItemObject;
                    int offsetX = 0;
                    int offsetY = 0;
                    if (this._dragStarted && obj.Selected)
                    {
                        offsetX = this._mouseDragOffset.X;
                        offsetY = this._mouseDragOffset.Y;
                    }
                    obj.Draw(graphics, offsetX, offsetY, this._dragStarted);
                
                    obj.DrawSelectionRect(graphics, offsetX, offsetY, this._dragStarted);
                    graphics.DrawImage(this._imgList.Images[0], new Point(offsetX + (int)obj.Position.X, offsetY + (int)obj.Position.Y));
                }
                else
                if (hintItem.ItemObject is TileCell)
                {
                    TileCell tc = (TileCell)hintItem.ItemObject;
                    int dx = 0, dy = 0;
                    if (this._dragStarted && tc.Selected)
                    {
                        dx += this._mouseDragOffset.X;
                        dy += this._mouseDragOffset.Y;
                    }
                    tc.Draw(graphics, dx, dy, this.TileSelectionPen, Settings.ObjectSelectionPenBack);
                    graphics.DrawImage(this._imgList.Images[0], new Point(dx + (tc.BoardX * TwoBrainsGames.Snails.Stages.Stage.TILE_WIDTH),
                                                                          dy + (tc.BoardY * TwoBrainsGames.Snails.Stages.Stage.TILE_HEIGHT)));
                }
            }
        }

        public System.Drawing.Point GetPositionInBoardFromPoint(System.Drawing.Point position)
        {
            int rowx = (position.X / TwoBrainsGames.Snails.Stages.Stage.TILE_WIDTH);
            int rowy = (position.Y / TwoBrainsGames.Snails.Stages.Stage.TILE_HEIGHT);
            int x = 0, y = 0;

            y = (rowy * TwoBrainsGames.Snails.Stages.Stage.TILE_HEIGHT);
            x = (rowx * TwoBrainsGames.Snails.Stages.Stage.TILE_WIDTH);

            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawTiles(Graphics graphics)
        {
            // Tiles
            for (int i = 0; i < this.Stage.Size.Width; i++)
            {
                for (int j = 0; j < this.Stage.Size.Height; j++)
                {
                    if (this.Stage.InBoardBounds(i, j) && this.Stage.Board.Tiles[j, i] != null)
                    {

                        int dx = 0, dy = 0;
                        if (this._dragStarted && this.Stage.Board.Tiles[j, i].Selected)
                        {
                            dx += this._mouseDragOffset.X;
                            dy += this._mouseDragOffset.Y;
                        }
                        this.Stage.Board.Tiles[j, i].Draw(graphics, dx, dy, this.TileSelectionPen, Settings.ObjectSelectionPenBack);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlBoard_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (this.DisableDraw)
                {
                    return;
                }
                if (UserSettings.ShowImages == false)
                {
                    return;
                }

                if (this.Stage == null)
                {
                    return;
                }

                this.DrawBackground(e.Graphics);
                // Grid
                if (UserSettings.GridVisible && !UserSettings.GridOnTop)
                {
                    this.DrawGrid(e.Graphics);
                }

                if (!MainForm._instance.ShowOnlyHints)
                {
                    this.DrawObjects(e.Graphics, false);
                    if (UserSettings.ShowTiles)
                    {
                        this.DrawTiles(e.Graphics);
                    }
                    this.DrawObjects(e.Graphics, true);
                    this.DrawObjectLinks(e.Graphics);
                }

                if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Hints)
                {
                    this.DrawHints(e.Graphics);
                }
                // Grid
                if (UserSettings.GridVisible && UserSettings.GridOnTop)
                {
                    this.DrawGrid(e.Graphics);
                }
                if (UserSettings.ShowCameraFrame)
                {
                    this.DrawCameraFrame(e.Graphics);
                }

                if (this.Cursor.Visible)
                {
                    // This is all too strange...
                    // If _CursorObject not null, then the selected object is a StageObject
                    // Use the GetPositionInBoardFromPoint() to find the position
                    if (this._CursorObject != null)
                    {
                        Point pt = this._CursorObject.GetPositionInBoardFromPoint(this.CursorObjectPos);
                        this.Cursor.Draw(e.Graphics, pt.X, pt.Y);
                    }
                    else // If not, then it's a Tile object.Just draw it like normaly..lol
                    {
                        Point pt = this.GetPositionInBoardFromPoint(new Point((int)this.CursorObjectPos.X, (int)this.CursorObjectPos.Y));
                        this.Cursor.Draw(e.Graphics, pt.X, pt.Y);
                    }
                }
                // Selection
                if (this._selectionStarted)
                {
                    e.Graphics.DrawRectangle(this.HiLightedCellPenBack, this.SelectionRect);
                    e.Graphics.DrawRectangle(this.HiLightedCellPen, this.SelectionRect);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
                this.DisableDraw = true;
            }
        }

        #endregion


        private void DrawBackground(Graphics g)
        {
#if STAGE_EDITOR
            foreach (BackgroundLayer bg in this.Stage.Layers)
            {

                int nx = (int)(Math.Ceiling((decimal)((double)this.Stage.Board.Width / (double)bg.Sprite.Image.Width)));
                int ny = (int)(Math.Ceiling((decimal)((double)this.Stage.Board.Height / (double)bg.Sprite.Image.Height)));
                for (int i = 0; i < nx; i++)
                {

                    for (int j = 0; j < ny; j++)
                    {

                        g.DrawImage(bg.Sprite.Image, new Point(i * bg.Sprite.Image.Width, j * bg.Sprite.Image.Height));
                    }
                }
                break; // for now ignore second layer
            }
#endif
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnCellClicked(int x, int y, int row, int col, MouseEventArgs mouseArgs)
        {
            if (this.CellClicked != null)
            {
                this.CellClicked(x, y, row, col, mouseArgs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnStageObjectPropertiesClicked(StageObject obj)
        {

            if (this.StageObjectPropertiesClicked != null)
            {
                this.StageObjectPropertiesClicked(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnMoveToFrontClicked(StageObject obj)
        {

            if (this.MoveToFrontClicked != null)
            {
                this.MoveToFrontClicked(obj);
            }
        }  
        
        /// <summary>
        /// 
        /// </summary>
        private void OnSendToBackClicked(StageObject obj)
        {

            if (this.SendToBackClicked != null)
            {
                this.SendToBackClicked(obj);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnDeleteItemsClicked()
        {
            if (this.SelectedItems.Count == 0)
                return;

            if (this.DeleteItemsClicked != null)
            {
                this.DeleteItemsClicked(this, new EventArgs());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void DeselectObject(Object obj)
        {
            if (!this.SelectedItems.Contains(obj))
                return;
            this.SelectedItems.Remove(obj);
            if (obj as StageObject != null)
            {
                ((StageObject)obj).Selected = false;
            }
            else
            if (obj as TileCell != null)
            {
                ((TileCell)obj).Selected = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SelectObject(Object obj)
        {
            if (this.SelectedItems.Contains(obj))
                return;

            if (this.SelectionMode == SelectionType.Objects &&
                !(obj is StageObject))
            {
                return;
            }

            if (this.SelectionMode == SelectionType.Tiles &&
                !(obj is TileCell))
            {
                return;
            }

            this.SelectedItems.Add(obj);
            if (obj as StageObject != null)
            {
                ((StageObject)obj).Selected = true;
            }
            else
            if (obj as TileCell != null)
            {
                ((TileCell)obj).Selected = true;
            }
            this._SelectionTimer.Enabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SelectAll()
        {
            this.ClearSelection();
            foreach (StageObject obj in this.Stage.Objects)
            {
                this.SelectObject(obj);
            }
            for (int i = 0; i < this.Stage.Board.Rows; i++)
            {
                for (int j = 0; j < this.Stage.Board.Columns; j++)
                {
                    if (this.Stage.Board.Tiles[i, j] != null)
                    {
                        this.SelectObject(this.Stage.Board.Tiles[i, j]);
                    }
                }
            }
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearSelection()
        {

            foreach (Object obj in this.SelectedItems)
            {
                if (obj as StageObject != null)
                {
                    ((StageObject)obj).Selected = false;
                }
                if (obj as TileCell != null)
                {
                    ((TileCell)obj).Selected = false;
                }
            }
            this.SelectedItems.Clear();
            this._SelectionTimer.Enabled = false;
            this.Refresh();
        }

        /// <summary>
        /// if multiple false, only 1 object is selected, otherwise all objects in the BB will be selected
        /// </summary>
        private void SelectObjects(BoundingSquare bsSelection, bool multiple)
        {
            if (
                 ((Control.ModifierKeys & Keys.Shift) != Keys.Shift) &&
                 ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                ||
                !multiple)

            {
                this.ClearSelection();
            }

            for (int i = this.Stage.CurrentEditedObjects.Count - 1; i >= 0; i--)
            {
                StageObject obj = this.Stage.CurrentEditedObjects[i];
                Rectangle selRc = obj.SelectionRect;
                BoundingSquare bs = new BoundingSquare(new Microsoft.Xna.Framework.Rectangle(selRc.Left, selRc.Top, selRc.Width, selRc.Height));
                if (bsSelection.Collides(bs))
                {
                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        this.SelectObject(obj);
                    }
                    else
                    {
                        if (obj.Selected)
                        {
                            this.DeselectObject(obj);
                        }
                        else
                        {
                            this.SelectObject(obj);
                            if (!multiple)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if (MainForm._instance.EditingMode == MainForm.StageEditingMode.Hints)
            {
                foreach (TileCell tc in this.Stage.Stage.HintManager.CurrentHint.TileItems)
                {
                    Microsoft.Xna.Framework.Rectangle rc =
                                    new Microsoft.Xna.Framework.Rectangle
                                                (tc.BoardX * this.Stage.Board.TileWidth,
                                                 tc.BoardY * this.Stage.Board.TileHeight,
                                                 this.Stage.Board.TileWidth,
                                                 this.Stage.Board.TileHeight);
                    BoundingSquare bs = new BoundingSquare(rc);
                    if (bs.Collides(bsSelection))
                    {
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            this.SelectObject(tc);
                        }
                        else
                        {
                            if (tc.Selected)
                                this.DeselectObject(tc);
                            else
                                this.SelectObject(tc);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.Stage.Board.Columns; i++)
                {
                    for (int j = 0; j < this.Stage.Board.Rows; j++)
                    {
                        if (this.Stage.Board.Tiles[j, i] == null)
                        {
                            continue;
                        }
                        Microsoft.Xna.Framework.Rectangle rc =
                                    new Microsoft.Xna.Framework.Rectangle
                                                (i * this.Stage.Board.TileWidth,
                                                 j * this.Stage.Board.TileHeight,
                                                 this.Stage.Board.TileWidth,
                                                 this.Stage.Board.TileHeight);
                        BoundingSquare bs = new BoundingSquare(rc);
                        if (bs.Collides(bsSelection))
                        {
                            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                            {
                                this.SelectObject(this.Stage.Board.Tiles[j, i]);
                            }
                            else
                            {
                                if (this.Stage.Board.Tiles[j, i].Selected)
                                    this.DeselectObject(this.Stage.Board.Tiles[j, i]);
                                else
                                    this.SelectObject(this.Stage.Board.Tiles[j, i]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _SelectionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                this.TileSelectionPen.DashOffset -= 1.0F;
                if (this.TileSelectionPen.DashOffset <= -this.TileSelectionPen.DashPattern[0] * 2)
                    this.TileSelectionPen.DashOffset = 0;

                Settings.ObjectSelectionPen.DashOffset -= 1.0F;
                if (Settings.ObjectSelectionPen.DashOffset <= -Settings.ObjectSelectionPen.DashPattern[0] * 2)
                    Settings.ObjectSelectionPen.DashOffset = 0;

                this.Refresh();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void DragEnded()
        {
            if (this.SelectedItems.Count == 0)
                return;

            // Snap to tiles the new position. This has to be reviewed because there are objects
            // that don't snap. The selection must be analysed to check what's the case

            Point startColRow = new Point(this._mouseStartPoint.X / Stage.TileSize.Width,
                                                  this._mouseStartPoint.Y / Stage.TileSize.Height);
            Point endColRow = new Point(this._mouseEndPoint.X / Stage.TileSize.Width,
                                                  this._mouseEndPoint.Y / Stage.TileSize.Height);

            Point offsetColRow = new Point(endColRow.X - startColRow.X,
                                           endColRow.Y - startColRow.Y);
            Point moveOffsetInPixels = new Point(this._mouseEndPoint.X - this._mouseStartPoint.X, 
                                                 this._mouseEndPoint.Y - this._mouseStartPoint.Y);
            int offsetX = offsetColRow.X * Stage.TileSize.Width;
            int offsetY = offsetColRow.Y * Stage.TileSize.Height;
            
            offsetX = moveOffsetInPixels.X;
            offsetY = moveOffsetInPixels.Y;

            if (this.Stage.CheckAnyObjectOutOfBoard(this.SelectedItems, offsetX, offsetY))
            {
                if (MessageBox.Show(this.ParentForm, "There are objects out of the board bounds. This objects will be deleted.\nProceed?", StageEditor.AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == 
                    DialogResult.Cancel)
                   return;
            }

            List<Object> movedObjects = this.Stage.MoveObjectsAndTiles(this.SelectedItems, offsetX, offsetY, offsetColRow.X, offsetColRow.Y);
            foreach (Object obj in movedObjects)
            {
                this.SelectObject(obj);
            }
            this._mouseDragOffset = new Point(0, 0);
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlBoard_MouseClick(object sender, MouseEventArgs e)
        {
         

        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlBoard_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._mnuMultipleSelected.Visible)
                {
                    return;
                }

                this._pnlBoard.Capture = false;

                this._mouseEndPoint = e.Location;

                if (this.SelectedToolType == ToolType.Tile ||
                    this.SelectedToolType == ToolType.GameObject ||
                    this.SelectedToolType == ToolType.Prop)
                {
                    this.ClearSelection();
                    this.OnCellClicked(e.X, e.Y,
                                       (e.X / Stage.TileSize.Width), (e.Y / Stage.TileSize.Height),
                                       e);
                    return;
                }

                if (this._dragStarted && this._mouseStartPoint != this._mouseEndPoint)
                {
                    this.DragEnded();
                    return;
                }

                // Select tool
                if (this._selectionStarted || this._mouseStartPoint == this._mouseEndPoint)
                {
                    // Mouse click occurs on the button is depressed
                    // We will try to select and object on mouse click, only if the user
                    // didn't started a drag selection

                    Microsoft.Xna.Framework.Rectangle xnaRect = new Microsoft.Xna.Framework.Rectangle(this.SelectionRect.Left, this.SelectionRect.Top, this.SelectionRect.Width, this.SelectionRect.Height);
                    BoundingSquare bsSelection = new BoundingSquare(xnaRect);
                    this.SelectObjects(bsSelection, (this._mouseStartPoint != this._mouseEndPoint));

                    if (this.SelectedItems.Count == 0)
                        return;
                    this._SelectionTimer.Enabled = true;
                }

                if (e.Button == MouseButtons.Right)
                {
                    if (!this._mnuStageObject.Visible &&
                        !this._mnuTileSelected.Visible)
                    {
                        if (this.SelectedItems.Count == 1)
                        {
                            if (this.SelectedItems[0] as StageObject != null)
                            {
                                this._mnuStageObject.Show(this, this.ToClientAbsolutePosition(e.Location));
                            }
                            if (this.SelectedItems[0] as TileCell != null)
                            {
                                this._mnuTileSelected.Show(this, this.ToClientAbsolutePosition(e.Location));
                            }

                        }
                    }
                }
   
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
            finally
            {
                this._selectionStarted = false;
                this._dragStarted = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlBoard_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                this._pnlBoard.Capture = true;
                // If the user has already objects selected and the user right clicked an already selected
                // object, the multiple object context menu pops up
                this._mouseStartPoint = this._mouseEndPoint = e.Location;

                if (this._selectedItems.Count > 0 && this.AnySelectedItemContains(e.Location))
                {
                    if (e.Button == MouseButtons.Right && this._selectedItems.Count > 1)
                    {
                       this._mnuMultipleSelected.Show(this, this.ToClientAbsolutePosition(e.Location));
                    }
                    else
                    if (e.Button == MouseButtons.Left) // Object dragging
                    {
                        this._dragStarted = true;
                        this._mouseDragOffset = new Point(0, 0);
                    }
                    return;
                }
                else
                if (this._SelectedToolType == ToolType.Select)
                {
                    this._selectionStarted = true;
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// This controls the autoscroll ammount when dragging. If the mouse goes behound 
        /// a specific margin, the map will autoscroll. This may return negative values vor
        /// scroll left and top
        /// </summary>
        public Point ShouldAutoHScroll(Point mousePos)
        {
            Point scroll = new Point();
        
            // Right scroll
            if (mousePos.X - this._grpBoard.HorizontalScroll.Value > this.Width - 20.0f &&
                this._grpBoard.HorizontalScroll.Value < this._grpBoard.HorizontalScroll.Maximum)
            {
                scroll.X = (mousePos.X - this._grpBoard.HorizontalScroll.Value) - this.Width + 20;
                if (scroll.X + this._grpBoard.HorizontalScroll.Value > this._grpBoard.HorizontalScroll.Maximum)
                    scroll.X = this._grpBoard.HorizontalScroll.Maximum - this._grpBoard.HorizontalScroll.Value;
            }
            else
            // Left scroll
            if (mousePos.X - this._grpBoard.HorizontalScroll.Value < 20.0f &&
                 mousePos.X - this._grpBoard.HorizontalScroll.Value > 0 &&
                this._grpBoard.HorizontalScroll.Value > this._grpBoard.HorizontalScroll.Minimum)
            {
                scroll.X = (mousePos.X - this._grpBoard.HorizontalScroll.Value) - 20;
                if (scroll.X + this._grpBoard.HorizontalScroll.Value < this._grpBoard.HorizontalScroll.Minimum)
                    scroll.X = this._grpBoard.HorizontalScroll.Minimum - this._grpBoard.HorizontalScroll.Value;
            }

            // Bottom scroll
            if (mousePos.Y - this._grpBoard.VerticalScroll.Value > this.Height - 20.0f &&
                this._grpBoard.VerticalScroll.Value < this._grpBoard.VerticalScroll.Maximum)
            {
                scroll.Y = (mousePos.Y - this._grpBoard.VerticalScroll.Value) - this.Height + 20;
                if (scroll.Y + this._grpBoard.VerticalScroll.Value > this._grpBoard.VerticalScroll.Maximum)
                    scroll.Y = this._grpBoard.VerticalScroll.Maximum - this._grpBoard.VerticalScroll.Value;
            }
            else
            // Top scroll
                if (mousePos.Y - this._grpBoard.VerticalScroll.Value < 20.0f &&
                this._grpBoard.VerticalScroll.Value > this._grpBoard.VerticalScroll.Minimum)
            {
                scroll.Y = (mousePos.Y - this._grpBoard.VerticalScroll.Value) - 20;
                if (scroll.Y + this._grpBoard.VerticalScroll.Value < this._grpBoard.VerticalScroll.Minimum)
                    scroll.Y = this._grpBoard.VerticalScroll.Minimum - this._grpBoard.VerticalScroll.Value;
            }
            if (scroll.X > 20)
                scroll.X = 20;
            if (scroll.Y > 20)
                scroll.Y = 20;
            if (scroll.X < -20)
                scroll.X = -20;
            if (scroll.Y < -20)
                scroll.Y = -20;
            return scroll;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlBoard_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this._SelectedToolType != ToolType.None)
                {
                    this.CursorObjectPos = e.Location;
                }

                if (this._selectionStarted)
                    this._mouseEndPoint = e.Location;

                if (this._dragStarted)
                {
                    this._mouseDragOffset = new Point(e.X - this._mouseStartPoint.X, e.Y - this._mouseStartPoint.Y);
                }

                if (this._selectionStarted || this._dragStarted)
                {
                    this._mouseEndPoint = e.Location;
                    // What follows is code to allow autoscroll when the mouse pointer reaches a specific position
                    Point pt = this._pnlBoard.PointToClient(System.Windows.Forms.Control.MousePosition);
                    Point pt1 = ShouldAutoHScroll(pt);
                    while (!pt1.IsEmpty)
                    {
                       this._grpBoard.HorizontalScroll.Value += pt1.X;
                       this._grpBoard.VerticalScroll.Value += pt1.Y;
                       this._mouseDragOffset = new Point(pt.X - this._mouseStartPoint.X + this._grpBoard.HorizontalScroll.Value,
                                                         pt.Y - this._mouseStartPoint.Y + this._grpBoard.VerticalScroll.Value);
                       this.Refresh();
                       Application.DoEvents();
                       pt = this._pnlBoard.PointToClient(System.Windows.Forms.Control.MousePosition);
                       pt1 = ShouldAutoHScroll(pt);
                       // It's strange to put this here to break the loop, but it something strange was happening here
                       // The loop wasn't breaking, but this._selectionStarted and this._dragStarted are being set
                       // Don't know why, because this happens in MouseUp.finally
                       if (!this._selectionStarted && !this._dragStarted)
                       {
                           break;
                       }
                    }
               }

              
                this.Refresh();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlBoard_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                this.ShowCursorObject = false;
                this.Refresh();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _pnlBoard_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                this.ShowCursorObject = true;
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }


        /// <summary>
        /// Converts a point in the client area to a point in the client area subtracting
        /// the scroll bars positions
        /// <param name="pt"></param>
        /// <returns></returns>
        public Point ToClientAbsolutePosition(Point pt)
        {
            return new Point(pt.X - this._grpBoard.HorizontalScroll.Value, pt.Y - this._grpBoard.VerticalScroll.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optStageObjProps_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SelectedObject != null)
                {
                    this.OnStageObjectPropertiesClicked((StageObject)this.SelectedItems[0]);
                }
                
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optDelStageObj_Click(object sender, EventArgs e)
        {
            try
            {
                this.OnDeleteItemsClicked();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optMultipleDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.OnDeleteItemsClicked();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optTileDelete_Click(object sender, EventArgs e)
        {
            try
            {
             
                this.OnDeleteItemsClicked();
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool AnySelectedItemContains(Point pt)
        {
            foreach (Object obj in this.SelectedItems)
            {
                if (obj as StageObject != null)
                {
                    StageObject stageObj = (StageObject)obj;
                    if (stageObj.SelectionRect.Contains(pt.X, pt.Y))
                        return true;
                }
                else
                if (obj as TileCell != null)
                {
                    TileCell cell = (TileCell)obj;
                    if (cell.Contains(pt))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private void _mnuStageObject_Opening(object sender, CancelEventArgs e)
        {
            this._optMultipleRotate.Enabled = this._optStageObjRotate.Enabled = this.SelectedObjectsAllowRotation;
        }

        private void _optSendToBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SelectedObject != null)
                {
                    this.OnSendToBackClicked((StageObject)this.SelectedItems[0]);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        private void _optBringToFront_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SelectedObject != null)
                {
                    this.OnMoveToFrontClicked((StageObject)this.SelectedItems[0]);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        private void _grpBoard_Load(object sender, EventArgs e)
        {
            this._pnlBoard.Location = new Point(0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optStageObjRotate_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditorStage.CurrentStage != null)
                {
                    EditorStage.CurrentStage.RotateObject(this.SelectedObject);
                }
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void _optMultipleRotate_Click(object sender, EventArgs e)
        {
            try
            {
                if (EditorStage.CurrentStage != null)
                {
                    EditorStage.CurrentStage.RotateObjects(this.SelectedObjects);
                }    
            }
            catch (System.Exception ex)
            {
                Diag.ShowException(this.ParentForm, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void _Stage_SizeChanged(object sender, EventArgs e)
        {
            this.RefreshBoardSize();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CurrentStageChanged()
        {
            this.GridColor = Settings.Theme[(int)this._Stage.Stage.LevelStage.ThemeId].GridColor;
            this.GridBrush = new SolidBrush(this.GridColor);
            this.GridPen = new Pen(this.GridBrush);
            this._Stage.SizeChanged += new EventHandler(_Stage_SizeChanged);
            this.RefreshBoardSize();
            this.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        void RefreshBoardSize()
        {
            this._pnlBoard.Width = this.Stage.SizeInPixels.Width;
            this._pnlBoard.Height = this.Stage.SizeInPixels.Height;
            this._pnlBoard.HorizontalScroll.Value = 0;
            this._grpBoard.VerticalScroll.Value = 0;
            this._grpBoard.PerformLayout();
        }

    }
}
