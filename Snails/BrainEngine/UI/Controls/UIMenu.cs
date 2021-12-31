using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;

namespace TwoBrainsGames.BrainEngine.UI.Controls
{
    public class UIMenu : UIControl
    {
        private MenuItemPlacement _itemPlacement;

        #region properties
        public UIMenuItemCollection Items { get; private set; }
        public TransformEffectBase ItemOnFocusEffect { get; set; }
        public UIMenuItem SelectedItem { get; private set; }
        public bool ShowDisabledItems { get; set; }
        public float ItemSpacing { get; set; }
        public int Columns { get; set; }

        public MenuItemPlacement ItemPlacement
        {
            get
            {
                return this._itemPlacement;
            }
            set
            {
                this._itemPlacement = value;
                this.RepositionItems();
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UIMenu(UIScreen screenOwner) :
            base(screenOwner)
        {
            this.Columns = 1;
            this.Items = new UIMenuItemCollection(this);
            this.ItemPlacement = MenuItemPlacement.Vertical;
            this.ShowDisabledItems = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            this.RepositionItems();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainGameTime gameTime)
        {
            // Propagate properties to childs
            // This could be optimized (why do this in all loops?)
            foreach (UIMenuItem item in this.Items)
            {
                item.DropShadow = this.DropShadow;
                item.OnFocusEffect = this.ItemOnFocusEffect;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void OnItemEnter(IUIControl sender)
        {
            this.SelectedItem = (UIMenuItem)sender;
            this.SelectedItem.BringToFront();
        }

        /// <summary>
        /// 
        /// </summary>
        internal void OnItemLeave(IUIControl sender)
        {
            this.SelectedItem = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            this.RepositionItems();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RepositionItems()
        {
            if (this.ItemPlacement == MenuItemPlacement.Free) // Item placement is controled by the items
            {
                return;
            }

            float x = 0.0f;
            float y = 0.0f;
            float width = 0.0f;
            float height = 0.0f;
            // Beware! Not taking into account neither orientation or hidden items
            int itemsPerPage = (this.Items.Count / this.Columns);
            int itemsCountCurrentPage = 0;
            int currentColumn = 1;

            
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Enabled == false &&
                    this.ShowDisabledItems == false)
                {
                    continue;
                }


                itemsCountCurrentPage++;
                this.Items[i].Position = new Vector2(x, y);
                if (this.Items[i].Position.X + this.Items[i].Size.Width > width)
                {
                    width = this.Items[i].Position.X + this.Items[i].Size.Width;
                }
                if (this.Items[i].Position.Y + this.Items[i].Size.Height > height)
                {
                    height = this.Items[i].Position.Y + this.Items[i].Size.Height;
                }

                if (this.Columns > 1) // Hammer, tired of this shit
                {
                    if (currentColumn == 1)
                    {
                        this.Items[i].ParentAlignmentOffset = new Vector2(-this.Items[i].Size.Width / 2, 0f);
                    }
                    else
                    {
                        this.Items[i].ParentAlignmentOffset = new Vector2(this.Items[i].Size.Width / 2, 0f);
                    }
                }
                bool pageBreak = (itemsCountCurrentPage == itemsPerPage);
                switch (this.ItemPlacement)
                {
                    case MenuItemPlacement.Horizontal:
                        x += this.Items[i].Size.Width;
                        break;

                    case MenuItemPlacement.Vertical:
                        y += this.Items[i].Size.Height + this.ItemSpacing;
                        if (pageBreak)
                        {
                            y = 0;
                            x += this.Items[i].Size.Width;
                            itemsCountCurrentPage = 0;
                            currentColumn++;
                        }
                        break;
                }
            }

            // Adjust the menu size to fit the items
            this.AdjustSizeToFitItems();
         }

        /// <summary>
        /// 
        /// </summary>
        public void AdjustSizeToFitItems()
        {
            if (this.ItemPlacement == MenuItemPlacement.Free) // Item placement is controled by the items
            {
                return;
            }

            float width = 0.0f;
            float height = 0.0f;

            if (this.Items.Count == 0)
                return;
            float x = 999999;
            float y = 999999;
            float xx = 0;
            float yy = 0;
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Position.X < x)
                {
                    x = this.Items[i].Position.X;
                }
                if (this.Items[i].Position.Y < y)
                {
                    y = this.Items[i].Position.Y;
                }
                if (this.Items[i].Position.X + this.Items[i].Size.Width > xx)
                {
                    xx = this.Items[i].Position.X + this.Items[i].Size.Width;
                }
                if (this.Items[i].Position.Y + this.Items[i].Size.Height > yy)
                {
                    yy = this.Items[i].Position.Y + this.Items[i].Size.Height;
                }

                /*if (this.Items[i].Position.X + this.Items[i].Size.Width > width)
                {
                    width = this.Items[i].Position.X + this.Items[i].Size.Width;
                }
                if (this.Items[i].Position.Y + this.Items[i].Size.Height > height)
                {
                    height = this.Items[i].Position.Y + this.Items[i].Size.Height;
                }*/

            }

            // Adjust the menu size to fit the items
            this.Size = new Size(xx - x, yy - y);
        }
    }
}
