
namespace TwoBrainsGames.Snails.Stages
{
  public struct TileCellCoords
  {
    private int _RowIndex;
    private int _ColIndex;

    public int RowIndex { get { return this._RowIndex; } set { this._RowIndex = value; } }
    public int ColIndex { get { return this._ColIndex; } set { this._ColIndex = value; } }

    public TileCellCoords(int ColIndex, int rowIndex)
    {
      this._RowIndex = rowIndex;
      this._ColIndex = ColIndex;
    }

    public override string ToString()
    {
        return string.Format("{0},{1}", this.RowIndex, this.ColIndex);
    }
  }
}
