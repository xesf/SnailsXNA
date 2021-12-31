using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine;

public class BrainAchievement : Object2D, IDataFileSerializable
{
    public bool ShowOnAppStore = false;
    public bool CanBeDisplayed = true;
    public int EventType = 0;
    public int Difficulty = 0; // 1-Bronze, 2-Silver, 3-Gold
    private Dictionary<LanguageCode, string> _description = null;
    public int Quantity = 0;

    private string _difficultyRes = null;
    private string _ballonRes = null;
    private string _textFontRes = null;
    private string _playSoundRes = null;
    private Sprite _spriteDifficulty = null;
    private Sprite _spriteBallon = null;
    private TextFont _font;
    private Sample _bubbleOut;
    private string _achievementWonText;
    private Rectangle _rcBackground;

    private Vector2 _descPosition;
    private Vector2 _achievWonPosition;

    public string Description
    {
        get { return this._description[BrainGame.CurrentLanguage]; }
    }

    public Sprite Trophy
    {
        get { return this._spriteDifficulty; }
    }

    public BrainAchievement(string difficultyRes, string ballonRes, string textFontRes, string playSoundRes)
    {
        _difficultyRes = difficultyRes;
        _ballonRes = ballonRes;
        _textFontRes = textFontRes;
        _playSoundRes = playSoundRes;
    }

    private string GetDifficultySprite()
    {
        string sprite = string.Empty;
        switch (this.Difficulty)
        { 
            case 1:
                sprite = "AwardBronze";
                break;
            case 2:
                sprite = "AwardSilver";
                break;
            case 3:
                sprite = "AwardGold";
                break;
        }
        return sprite;
    }

    internal void LoadContent()
    {
        string spriteDif = _difficultyRes + "/" + GetDifficultySprite();

        this._bubbleOut = BrainGame.ResourceManager.GetSampleStatic(_playSoundRes, this);
        this._font = BrainGame.ResourceManager.Load<TextFont>(_textFontRes, ResourceManager.ResourceManagerCacheType.Static);
        this._spriteDifficulty = BrainGame.ResourceManager.GetSpriteStatic(spriteDif);
        if (this._ballonRes != null)
        {
            this._spriteBallon = BrainGame.ResourceManager.GetSpriteStatic(this._ballonRes);
        }

    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
        this.EventType = record.GetFieldValue<int>("EventType");
        this.Difficulty = record.GetFieldValue<int>("Difficulty");
        this.Quantity = record.GetFieldValue<int>("Quantity", 0);
        this.ShowOnAppStore = record.GetFieldValue<bool>("ShowOnAppStore", false);
        
        DataFileRecordList descRecords = record.SelectRecords("Description");
        this._description = new Dictionary<LanguageCode, string>();
        foreach (DataFileRecord desc in descRecords)
        {
            LanguageCode language = (LanguageCode)Enum.Parse(typeof(LanguageCode), desc.GetFieldValue<string>("language"), true);
            string descText = desc.GetFieldValue<string>("text");
            descText = descText.Replace("%Quantity%", this.Quantity.ToString());
            this._description.Add(language, descText);
        }
    }

    public override DataFileRecord ToDataFileRecord()
    {
        DataFileRecord record = new DataFileRecord("Achievement");
        record.AddField("EventType", this.EventType);
        record.AddField("Difficulty", this.Difficulty);
        record.AddField("Quantity", this.Quantity);
        record.AddField("Description", this.Description);
        record.AddField("ShowOnAppStore", this.ShowOnAppStore);

        foreach (KeyValuePair<LanguageCode, string> desc in this._description)
        {
            DataFileRecord langRecord = new DataFileRecord("Description");
            langRecord.AddField("language", desc.Key.ToString());
            langRecord.AddField("text", desc.Value.ToString());
            
            record.AddRecord(langRecord);
        }

        return record;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Bring in the hammer. The drawing should be improved but I don't care...
        //        BrainGame.DrawRectangleFilled(spriteBatch, new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this._font.MeasureString(this.Description) + this._spriteDifficulty.Width + 20, this._spriteDifficulty.Height), new Color(0, 0, 0, 180));
        BrainGame.DrawRectangleFilled(spriteBatch, this._rcBackground, new Color(60, 60, 60, 255));
        if (this._spriteBallon != null)
        {
            this._spriteBallon.Draw(this.Position, spriteBatch);
        }
        this._spriteDifficulty.Draw(this.Position + new Vector2(10f, 0f), spriteBatch);
        this._font.DrawString(spriteBatch, this._achievementWonText, this.Position + _achievWonPosition, Vector2.One, Color.LightGreen);
        this._font.DrawString(spriteBatch, this.Description, this.Position + _descPosition , Vector2.One, Color.White);
    }

    internal void Show()
    {
        this._bubbleOut.Play();
        this._achievementWonText = LanguageManager.GetString("MSG_ACHIEVEMENT_WON"); // Could go to config file...
        float width = this._font.MeasureString(this._achievementWonText);
        float descWidth = this._font.MeasureString(this.Description);

        // All this magic numbers should go to the achiev config file...
        this._rcBackground = new Rectangle((int)this.Position.X + 50, (int)this.Position.Y + 5, (int)Math.Max(width, descWidth) + 40, 60);
        this._achievWonPosition = new Vector2(this._spriteDifficulty.Width + 20f, 10f);
        this._descPosition = new Vector2(this._spriteDifficulty.Width + 20f, 30f);
    }

    internal void Hide()
    {
        this.CanBeDisplayed = false;
    }
}
