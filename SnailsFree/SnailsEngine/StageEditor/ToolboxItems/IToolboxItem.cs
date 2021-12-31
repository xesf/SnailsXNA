using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.Snails.StageEditor.ToolboxItems
{
    public interface IToolboxItem
    {
        Sprite Sprite { get; }
        int SpriteFrameNr { get; }
        System.Drawing.Image Thumbnail { get; }
        ToolboxItem.ThumbnailSourceType ThumbnailType { get; }
        string ThumbnailResource { get; }
        void OnBoardPlacement(int col, int row);
        void OnBoardRemove(int col, int row);
    }
}
