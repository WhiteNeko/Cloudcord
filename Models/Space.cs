namespace Cloudcord.Models
{
    public class Space
    {
        public PieceEnum Piece { get; protected set; }

        public void SetPiece (PieceEnum piece)
        {
            this.Piece = piece;
        }
    }
}
