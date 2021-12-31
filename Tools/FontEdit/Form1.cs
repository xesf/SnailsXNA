using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace FontEdit
{
  public partial class Form1 : Form
  {
    private int selectedCharIdx = -1;
    public CFont _Font;
    Image _fontImage;

    private string _CurrentFilename;
    int ZoomFactor { get; set; }
    Image FontImage 
    {
      get { return this._fontImage; }
      set
      {
        this._fontImage = value;
        if (this._fontImage != null)
        {
          this._pnlFontImage.Size = new Size(this._fontImage.Width * this.ZoomFactor,
                                              this._fontImage.Height * this.ZoomFactor);
        }
        this._pnlFontImage.Visible = (this._fontImage != null);
      }
    }

    private List<CFont.FontData> SelectedChars
    {
      get
      {
        List<CFont.FontData> sel = new List<CFont.FontData>();
        if (this._Font != null)
        {
          foreach (int idx in this._ListboxChars.SelectedIndices)
          {
            foreach (CFont.FontData ch in this._Font.Characters)
            {
              if (ch.Character != null && ch.Character.Value == Convert.ToChar(idx))
              {
                sel.Add(ch);
              }
            }
          }
        }
        return sel;
      }
    }
    public Form1()
    {
      try
      {
        InitializeComponent();
        _Font = new CFont();
        this.RefreshCharsList();
        this._ListboxChars.SelectedIndex = 53;
        _pgFontProps.SelectedObject = _Font;
        this.ZoomFactor = 1;
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptSelectImage_Click(object sender, EventArgs e)
    {
      try
      {
        this._OpenFileDlg.Filter = "PNG|*.png";
        this._OpenFileDlg.FileName = "fontimage";

        this._OpenFileDlg.InitialDirectory = CSettings.LastOpenImageFolder;
        if (this._OpenFileDlg.ShowDialog(this) != DialogResult.OK)
        {
          return;
        }
        CSettings.LastOpenImageFolder = Path.GetDirectoryName(this._OpenFileDlg.FileName);
      //  this._FontPicture.Image = Image.FromFile(this._OpenFileDlg.FileName);
        this.FontImage = Image.FromFile(this._OpenFileDlg.FileName);
        int imgWidth = this.FontImage.Width;
        int imgHeight = this.FontImage.Height;

        _Font.ImageWidth = imgWidth;
        _Font.ImageHeight = imgHeight;

        _Font.GenerateDefaultFrames(imgWidth, imgHeight, _Font.NumCharsWidth, _Font.NumCharsHeight);
        AutoAdjustFrames();

        if (this._Font.ImageFilename == null)
        {
          this._Font.ImageFilename = Path.GetFileName(this._OpenFileDlg.FileName);
        }
        this._Font.FontEditImageFilename = this._OpenFileDlg.FileName;

        this._pnlFontImage.Refresh();
        _pgFontProps.Refresh();
        btGenerateFrames.Enabled = true;
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void UpdateStatusBar()
    {
      if (selectedCharIdx != -1)
      {
        this.toolStripStatusLabel1.Text = String.Format("Selected character: {4}     X: {0}  Y: {1}  Width: {2}  Height: {3}",
                this._Font.Characters[selectedCharIdx].Rect.Left,
                this._Font.Characters[selectedCharIdx].Rect.Top,
                this._Font.Characters[selectedCharIdx].Rect.Width,
                this._Font.Characters[selectedCharIdx].Rect.Height,
                this._Font.Characters[selectedCharIdx].Character);
      }
    }

    /// <summary>
    /// 
    /// </summary>
   
    private void _pnlFontImage_Paint(object sender, PaintEventArgs e)
    {
      if (this._Font == null || this.FontImage == null)
        return;

      try
      {
       // this._FontPicture.SizeMode = PictureBoxSizeMode.StretchImage;
        e.Graphics.DrawImage(this.FontImage, new Rectangle(0, 0,
                                                            this.FontImage.Width * this.ZoomFactor,
                                                            this.FontImage.Height * this.ZoomFactor));
        for (int i = 0; i < _Font.NumCharsHeight; i++)
        {
          for (int j = 0; j < _Font.NumCharsWidth; j++)
          {
            Pen pen = Pens.Yellow;
            if (this._Font.Characters[j + i * _Font.NumCharsWidth].Character == null)
            {
              pen = Pens.Red;
            }

            e.Graphics.DrawRectangle(pen, this.GetRectWithZoom(this._Font.Characters[j + i * _Font.NumCharsWidth].Rect));
          }
        }

        foreach (CFont.FontData ch in this.SelectedChars)
        {
          e.Graphics.DrawRectangle(Pens.Black, this.GetRectWithZoom(ch.Rect));
        }
        UpdateStatusBar();
        /*
        if (selectedCharIdx != -1)
        {
          e.Graphics.DrawRectangle(Pens.Black, this._Font.Characters[selectedCharIdx].Rect);
          UpdateStatusBar();
        }*/
      }
      catch (System.Exception)
      {
      }

      picExemplo.Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptExit_Click(object sender, EventArgs e)
    {
      try
      {
        this.Close();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RefreshCharsList()
    {
      this._ListboxChars.Items.Clear();
      char ch;
      string s;
      for (int i = 0; i < 256; i++)
      {
        s = Convert.ToChar(i).ToString();
        ch = Convert.ToChar(i);

        if (ch == ' ')
        {
          s = "SPACE";
        }
        else
          if (i < 32)
          {
            s = "CODE CHAR";
          }

        this._ListboxChars.Items.Add(s);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _ListboxChars_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        if (_Font != null)
        {
          for (int i = 0; i < _Font.Characters.Length; i++)
          {
            if (_ListboxChars.SelectedIndex != -1)
            {
              if (_Font.Characters[i].Character == Convert.ToChar(_ListboxChars.SelectedIndex))
              {
                selectedCharIdx = i;
                this.RefreshFontImage();
                break;
              }
            }
          }
        }
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void RefreshFontImage()
    {
      this._pnlFontImage.Refresh();
    }

    private void RefreshFontImage(Rectangle rect)
    {
      Rectangle rc = new Rectangle(rect.X * ZoomFactor,
                           rect.Y * ZoomFactor,
                           (rect.Width * ZoomFactor) + 1,
                           (rect.Height * ZoomFactor) + 1);
        this._pnlFontImage.Invalidate(rc);
    }

    private void SetCharRect()
    {
      if (selectedCharIdx != -1)
      {
        _Font.Characters[selectedCharIdx].Character = Convert.ToChar(_ListboxChars.SelectedIndex);
        this.RefreshFontImage();
        UpdateStatusBar();
      }
    }

    private void _ListboxChars_DoubleClick(object sender, EventArgs e)
    {
      try
      {
        this.SetCharRect();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxLeft_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(-1, 0);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxRight_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(1, 0);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxUp_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(0, -1);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxDown_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(0, 1);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptDecBoxWidth_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResizeChar(-1, 0);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptIncBoxWidth_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResizeChar(1, 0);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptDecBoxHeight_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResizeChar(0, -1);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptIncBoxHeight_Click(object sender, EventArgs e)
    {
      try
      {
        this.ResizeChar(0, 1);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void ResizeChar(int x, int y)
    {
      if (this.SelectedChars.Count >= 1)
      {
        // Font data unfortunately is a struct, so references cannot be used
        // SelectedChars and _Font.Characters don't have the same object...
        foreach (CFont.FontData ch1 in this.SelectedChars)
        {
          for (int i = 0; i < this._Font.Characters.Length; i++)
          {
            if (this._Font.Characters[i].Character == ch1.Character)
            {
              Rectangle rc = this._Font.Characters[i].Rect;
              this._Font.Characters[i].Rect.Width += x;
              this._Font.Characters[i].Rect.Height += y;
              this.RefreshFontImage(rc);
              this.RefreshFontImage(this._Font.Characters[i].Rect);
              break;
            }

          }
        }
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveChar(int x, int y)
    {
      if (this.SelectedChars.Count >= 1)
      {
        // Font data unfortunately is a struct, so references cannot be used
        // SelectedChars and _Font.Characters don't have the same object...
        foreach (CFont.FontData ch1 in this.SelectedChars)
        {
          for (int i = 0; i < this._Font.Characters.Length; i++)
          {
            if (this._Font.Characters[i].Character == ch1.Character)
            {
              Rectangle rc = this._Font.Characters[i].Rect;
              this._Font.Characters[i].Rect.X += x;
              this._Font.Characters[i].Rect.Y += y;
              this.RefreshFontImage(rc);
              this.RefreshFontImage(this._Font.Characters[i].Rect);
              break;
            }
          }
        }
      }
    }
    /// <summary>
    /// 
    /// </summary>
    private void _OptSave_Click(object sender, EventArgs e)
    {
      try
      {
        if (this._CurrentFilename == null)
        {
          this._SaveFileDlg.Filter = "TryE Font Project|*.fntProj";
          this._SaveFileDlg.FileName = Path.GetFileNameWithoutExtension(_Font.ImageFilename);

          this._SaveFileDlg.InitialDirectory = Directory.GetCurrentDirectory();
          if (this._SaveFileDlg.ShowDialog(this) != DialogResult.OK)
          {
            return;
          }
          this._CurrentFilename = this._SaveFileDlg.FileName;
        }
        FontSaverLoader.Save(this._CurrentFilename, this._Font);
        CSettings.LastEditedFile = this._CurrentFilename;
        MessageBox.Show(this, "TryE Font Project saved.");
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptProps_Click(object sender, EventArgs e)
    {
      try
      {
        FontPropsForm frm = new FontPropsForm();
        frm.ShowDialog(this, this._Font);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadFile(string filename)
    {
      string saveDir = Directory.GetCurrentDirectory();

      Directory.SetCurrentDirectory(Path.GetDirectoryName(filename));
      CSettings.LastEditedFile = filename;
      CSettings.LastOpenFileFolder = Path.GetDirectoryName(filename);
      this._Font = FontSaverLoader.Load(filename);
      _pgFontProps.Refresh();
      try
      {
       // this._FontPicture.Image = Image.FromFile(this._Font.FontEditImageFilename);
        this.FontImage = Image.FromFile(this._Font.FontEditImageFilename);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, "Error loading font proxy file!\n" + ex.ToString());
        this.FontImage = null;
      }
      this._CurrentFilename = filename;
      this._pgFontProps.SelectedObject = this._Font;
      btGenerateFrames.Enabled = true;
      this.Text = "FontEdit - " + this._CurrentFilename;
      Directory.SetCurrentDirectory(saveDir);
      this.Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptLoad_Click(object sender, EventArgs e)
    {
      try
      {
        this._OpenFileDlg.Filter = "TryE Font Project|*.fntProj";
        this._OpenFileDlg.FileName = string.Empty;
        this._OpenFileDlg.InitialDirectory = CSettings.LastOpenFileFolder;
        if (this._OpenFileDlg.ShowDialog(this) != DialogResult.OK)
        {
          return;
        }
        this.LoadFile(this._OpenFileDlg.FileName);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void SetFontImage(string imageFile)
    {
    }
    /// <summary>
    /// 
    /// </summary>
    private void _OptPreferences_Click(object sender, EventArgs e)
    {
      try
      {
        PreferencesForm frm = new PreferencesForm();
        frm.ShowDialog(this);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void setDefaultSizeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        this._Font.Characters[selectedCharIdx].Rect.Width = CSettings.DefaultCharWidth;
        this._Font.Characters[selectedCharIdx].Rect.Height = CSettings.DefaultCharHeight;
        this.RefreshFontImage();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxLeftSnap_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(-CSettings.Snap, 0);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxRightSnap_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(CSettings.Snap, 0);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxUpSnap_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(0, -CSettings.Snap);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void _OptMoveBoxDowntSnap_Click(object sender, EventArgs e)
    {
      try
      {
        this.MoveChar(0, CSettings.Snap);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }

    }

    private void btGenerateFrames_Click(object sender, EventArgs e)
    {
      int imgWidth = this.FontImage.Width;
      int imgHeight = this.FontImage.Height;

      _Font.GenerateDefaultFrames(imgWidth, imgHeight, _Font.NumCharsWidth, _Font.NumCharsHeight);
      AutoAdjustFrames();

      this.RefreshFontImage();
      picExemplo.Refresh();
      _pgFontProps.Refresh();
    }

    private void _pnlFontImage_MouseClick(object sender, MouseEventArgs e)
    {
      try
      {
        int x = e.X;
        int y = e.Y;
        if ( (Control.ModifierKeys & Keys.Shift ) != Keys.Shift &&
               (Control.ModifierKeys & Keys.Control ) != Keys.Control)
        {
          this._ListboxChars.SelectedIndices.Clear();
        }


        for (int i = 0; i < _Font.NumCharsHeight; i++)
        {
          for (int j = 0; j < _Font.NumCharsWidth; j++)
          {
           
            Rectangle rect = this.GetRectWithZoom(this._Font.Characters[j + i * _Font.NumCharsWidth].Rect);
            if (rect.X <= x && x <= rect.X + rect.Width &&
              rect.Y <= y && y <= rect.Y + rect.Height)
            {
              selectedCharIdx = j + i * _Font.NumCharsWidth;

              char? ch = this._Font.Characters[selectedCharIdx].Character;
              if (ch != null)
              {
                _ListboxChars.SelectedIndex = Convert.ToInt32(ch);
              }

              UpdateStatusBar();
            }
          }
        }
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    internal void AutoAdjustFrames()
    {
      if (_Font != null)
      {
        char character = '!'; // always start with this character
        Image img = this.FontImage;
        Bitmap b = new Bitmap(img);

        int pixelWidth = img.Width / _Font.NumCharsWidth;
        int pixelHeight = img.Height / _Font.NumCharsHeight;

        for (int i = 0; i < _Font.NumCharsHeight; i++)
        {
          for (int j = 0; j < _Font.NumCharsWidth; j++)
          {
            if (this._Font.Characters.Length <= (j + i * _Font.NumCharsWidth))
            {
              continue;
            }

            int max, min;
            Rectangle rect = this._Font.Characters[j + i * _Font.NumCharsWidth].Rect;

            min = max = (int)(rect.X + (pixelWidth / 2));

            for (int h = rect.Y; h < rect.Y + pixelHeight; h++)
            {
              for (int w = rect.X; w < rect.X + pixelWidth; w++)
              {
                Color c = b.GetPixel(w, h);

                if (c.ToArgb() != Color.Empty.ToArgb())
                {
                  if (w < min)
                  {
                    min = w;
                  }
                  if (w > max)
                  {
                    max = w;
                  }
                }
              }
            }

            rect.X = min;
            rect.Width = max - min + 1;

            if (max == min)
            {
              character++;
              continue;
            }

            this._Font.Characters[j + i * _Font.NumCharsWidth].Character = character;
            this._Font.Characters[j + i * _Font.NumCharsWidth].Rect = rect;
            character++;
          }
        }
        this.RefreshFontImage();
      }
    }

    private void generateNewFramesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      btGenerateFrames_Click(sender, e);
    }

    private void exportGameFontToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string curDir = Directory.GetCurrentDirectory();
      try
      {
        if (string.IsNullOrEmpty(this._Font.IngameExportFile))
        {
          this._SaveFileDlg.Filter = "TryE Game Font|*.fnt";
          this._SaveFileDlg.FileName = Path.GetFileNameWithoutExtension(_Font.ImageFilename);

          this._SaveFileDlg.InitialDirectory = CSettings.LastExportFolder;
          if (this._SaveFileDlg.ShowDialog(this) != DialogResult.OK)
          {
            return;
          }
          this._Font.IngameExportFile = Goodies.RelativePath(Path.GetDirectoryName(CSettings.LastEditedFile), this._SaveFileDlg.FileName);
        }
        CSettings.LastExportFolder = Path.GetDirectoryName(this._Font.IngameExportFile);
        Directory.SetCurrentDirectory(Path.GetDirectoryName(CSettings.LastEditedFile));
        FontSaverLoader.ExportGameFont(this._Font.IngameExportFile, this._Font, this._CurrentFilename);

        MessageBox.Show(this, "TryE Game Font '" + this._Font.IngameExportFile + "'Exported.");
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
      finally
      {
        Directory.SetCurrentDirectory(curDir);
      }
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this._CurrentFilename = null;
      _OptSave_Click(sender, e);
    }

    private void picExemplo_Paint(object sender, PaintEventArgs e)
    {
      if (_Font.Characters.Length > 0)
      {
        int x = 0, y = 0;
        char[] arrayChars = txtExemplo.Text.ToCharArray();

        for (int i = 0; i < arrayChars.Length; i++)
        {
          FontEdit.CFont.FontData data = new CFont.FontData();

          if (arrayChars[i] != ' ' && arrayChars[i] != '\r' && arrayChars[i] != '\n')
          {
            for (int c = 0; c < _Font.Characters.Length; c++)
            {
              if (_Font.Characters[c].Character == arrayChars[i])
              {
                data = _Font.Characters[c];
                break;
              }
            }

            if (data.Character != null)
            {
              Rectangle r = new Rectangle(x + data.Spacing, y, data.Rect.Width, data.Rect.Height);

              e.Graphics.DrawImage(this.FontImage, r, data.Rect, GraphicsUnit.Pixel);

              x += data.Rect.Width + _Font.BetweenCharsWidth + data.Spacing + data.SpacingAfter;
            }
          }
          else
          {
            if (arrayChars[i] == ' ')
            {
              x += _Font.SpaceWidth;
            }

            if (arrayChars[i] == '\r')
            {
              x = 0;
              y += _Font.LineHeight;
            }
          }
        }
      }
    }

    private void txtExemplo_TextChanged(object sender, EventArgs e)
    {
      picExemplo.Refresh();
    }

    private void _optOpenLast_Click(object sender, EventArgs e)
    {
      try
      {
        this.LoadFile(CSettings.LastEditedFile);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      try
      {
        CSettings.Load();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
      try
      {
        CSettings.Save();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
      try
      {
        if (!String.IsNullOrEmpty(CSettings.LastEditedFile))
        {
          this._optOpenLast.Enabled = true;
          this._optOpenLast.Text = "Open " + CSettings.LastEditedFile;
        }
        else
          this._optOpenLast.Enabled = false;

      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void _optSetSelChar_Click(object sender, EventArgs e)
    {
      try
      {
        this.SetCharRect();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void _optRemoveChar_Click(object sender, EventArgs e)
    {
      try
      {
        _Font.Characters[selectedCharIdx].Character = null;
        this.RefreshFontImage();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void _optAutoSet_Click(object sender, EventArgs e)
    {
      try
      {
        if (selectedCharIdx < 0 || selectedCharIdx >= _Font.Characters.Length)
        {
          return; 
        }

        Rectangle rc = _Font.Characters[selectedCharIdx].Rect;
        _Font.Characters[selectedCharIdx + 1].Rect = new Rectangle(rc.Left + 1 + rc.Width, rc.Top, rc.Width, rc.Height);
        this._ListboxChars.SelectedIndices.Clear();
        this._ListboxChars.SelectedIndex = (int)_Font.Characters[selectedCharIdx + 1].Character;
        this.RefreshFontImage();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
      try
      {

        this.picExemplo.Refresh();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void _optZoom2x_Click(object sender, EventArgs e)
    {
      try
      {

        this.SetZoom(2);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void _optZoom4x_Click(object sender, EventArgs e)
    {
      try
      {

        this.SetZoom(4);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void SetZoom(int factor)
    {
      if (this.FontImage != null)
      {
        if (factor < 0) factor = 0;
        if (factor > 4) factor = 4;
        this.ZoomFactor = factor;
        this._pnlFontImage.Size = new Size(this.FontImage.Width * this.ZoomFactor, 
                                           this.FontImage.Height * this.ZoomFactor);
        this.RefreshFontImage();
      }
    }

    private void _optNormalZoom_Click(object sender, EventArgs e)
    {
      try
      {
        this.SetZoom(1);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void _optZoomIn_Click(object sender, EventArgs e)
    {
      try
      {
        this.SetZoom(++this.ZoomFactor);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private void _optZoomOut_Click(object sender, EventArgs e)
    {
      try
      {
        this.SetZoom(--this.ZoomFactor);
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(this, ex.ToString());
      }
    }

    private Rectangle GetRectWithZoom(Rectangle rc)
    {
      return new Rectangle(rc.Left * this.ZoomFactor,
                           rc.Top * this.ZoomFactor,
                           rc.Width * this.ZoomFactor,
                           rc.Height * this.ZoomFactor);
    }
   

  }
}
