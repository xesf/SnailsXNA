using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using LevelEditor;
using LevelEditor.Forms;
using TwoBrainsGames.Snails.StageObjects;

namespace TwoBrainsGames.Snails.StageEditor.Controls
{
    public partial class BaseComboBox : ComboBox
    {
        public enum ComboBoxType
        {
            SpriteEffect,
            Rotation,
            PathBehaviour,
            Layer,
            LayerType,
            LayerSizeMode,
            Themes,
            StageObjects,
            CrystalColors,
            CrystalSizes,
            LaserBeamColors,
            SnailTypes,
            PumpAttachment,
            PipeTerminatorOrientation,
            TapOpeningDirection,
            TabSignToShow,
            SwithOnAction
        }

        public ComboBoxType ComboType
        {
            get;
            set;
        }
        public BaseComboBox()
        {
            InitializeComponent();
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void SelectByString(string id)
        {
            this.SelectedItem = null;
            foreach (object obj in this.Items)
            {
                if (obj.ToString() == id)
                {
                    this.SelectedItem = obj;
                    return;
                }
            }
        }
        
        public virtual void LoadValues()
        {
            this.LoadValues(null);
        }

        public virtual void LoadValues(object param)
        {
            this.Items.Clear();
            switch (this.ComboType)
            {
                case ComboBoxType.SpriteEffect:
                    this.Items.Add(SpriteEffects.None);
                    this.Items.Add(SpriteEffects.FlipHorizontally);
                    this.Items.Add(SpriteEffects.FlipVertically);
                    break;

                case ComboBoxType.Rotation:
                    this.Items.Add((float)0);
                    this.Items.Add((float)90);
                    this.Items.Add((float)180);
                    this.Items.Add((float)270);
                    break;

                case ComboBoxType.PathBehaviour:
                    this.Items.Add(PathBehaviour.None);
                    this.Items.Add(PathBehaviour.Walk);
                    this.Items.Add(PathBehaviour.Invert);
                    this.Items.Add(PathBehaviour.WalkCW);
                    this.Items.Add(PathBehaviour.WalkCCW);
                    break;

                case ComboBoxType.Layer:
                    foreach (SnailsBackgroundLayer layer in StageEditor.StageData.GetLayers())
                    {
                        this.Items.Add(layer);
                    }
                    break;

                case ComboBoxType.LayerType:
                    this.Items.Add(LayerType.Foreground);
                    this.Items.Add(LayerType.Background);
                    break;

                case ComboBoxType.Themes:
                    this.Items.Add(ThemeType.ThemeA);
                    this.Items.Add(ThemeType.ThemeB);
                    this.Items.Add(ThemeType.ThemeC);
                    this.Items.Add(ThemeType.ThemeD);
                    break;

                case ComboBoxType.LayerSizeMode:
                    break;

                case ComboBoxType.StageObjects:
                    if (param != null)
                    {
                        List<StageObject> list = MainForm.CurrentStageEdited.GetObjectsByType((StageObjectType)param);
                        foreach (StageObject obj in list)
                        {
                            this.Items.Add(obj);
                        }
                    }
                    else
                    {
                        foreach (StageObject obj in MainForm.CurrentStageEdited.Stage.Objects)
                        {
                            this.Items.Add(obj);
                        }
                    }
                    break;

                case ComboBoxType.CrystalColors:
                    this.Items.Add(Crystal.CrystalColorType.Red);
                    this.Items.Add(Crystal.CrystalColorType.Green);
                    this.Items.Add(Crystal.CrystalColorType.Blue);
                    this.Items.Add(Crystal.CrystalColorType.Yellow);
                    this.Items.Add(Crystal.CrystalColorType.Orange);
                   break;

                case ComboBoxType.LaserBeamColors:
                   this.Items.Add(LaserBeam.LaserBeamColor.Cyan);
                   this.Items.Add(LaserBeam.LaserBeamColor.Magenta);
                   this.Items.Add(LaserBeam.LaserBeamColor.Green);
                   break;

                case ComboBoxType.CrystalSizes:
                    this.Items.Add(Crystal.CrystalSizeType.Big);
                    this.Items.Add(Crystal.CrystalSizeType.Medium);
                    this.Items.Add(Crystal.CrystalSizeType.Small);
                    break;

                case ComboBoxType.SnailTypes:
                    this.Items.Add(Snail.ID);
                    this.Items.Add(EvilSnail.ID);
                    break;

                case ComboBoxType.PipeTerminatorOrientation:
                    this.Items.Add(LiquidPipe.PipeLinkType.Left);
                    this.Items.Add(LiquidPipe.PipeLinkType.Right);
                    break;

                case ComboBoxType.PumpAttachment:
                    this.Items.Add(LiquidPipe.PipeLinkType.Left);
                    this.Items.Add(LiquidPipe.PipeLinkType.Right);
                    break;

                case ComboBoxType.TapOpeningDirection:
                    this.Items.Add(LiquidTap.TapOpenDirection.Clockwise);
                    this.Items.Add(LiquidTap.TapOpenDirection.CounterClockwise);
                    break;

                case ComboBoxType.TabSignToShow:
                    this.Items.Add(LiquidTap.TapSignToShow.PumpIn);
                    this.Items.Add(LiquidTap.TapSignToShow.PumpOut);
                    break;

                case ComboBoxType.SwithOnAction:
                    this.Items.Add(Switch.SwitchOnActionType.SwitchOn);
                    this.Items.Add(Switch.SwitchOnActionType.SwitchOff);
                    this.Items.Add(Switch.SwitchOnActionType.Invert);
                   break;
            }
        }

    }
}
