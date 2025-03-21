using Cloudcord.Models;
using static Cloudcord.Models.Grid;

namespace Cloudcord.Models
{
    public class Grid
    {
        const int COLS = 7;
        const int ROWS = 6;
        public PieceEnum?[,] Pieces { get; protected set; }

        public PieceEnum NextTurn { get; protected set; }

        public PieceEnum? Winner { get; protected set; }
        public int Red { get; set; }
        public int Yellow { get; set; }
        public Grid()
        {
            NextTurn = PieceEnum.Red;

            ResetGame();
        }

        public void ResetGame()
        {
            Pieces = new PieceEnum?[COLS, ROWS];

            if (!Winner.HasValue)
            {
                NextTurn = (NextTurn == PieceEnum.Red ? PieceEnum.Yellow : PieceEnum.Red);
            }
            else
            {
                NextTurn = Winner.Value;
            }

                Winner = null;
        }
        public void SetNextttTurn()
        {
            Winner = GetWinner();

            if (!Winner.HasValue)
            {
                if (NextTurn == PieceEnum.Red)
                {
                    NextTurn = PieceEnum.Yellow;
                }
                else
                {
                    NextTurn = PieceEnum.Red;
                }
            }
            else
            {
                switch (Winner.Value)
                {
                    case PieceEnum.Red:
                        Red += 1;
                        break;
                    case PieceEnum.Yellow:
                        Yellow += 1;
                        break;
                }
            }
        }

        public class CheckIndex
        {
            public int Column { get; }
            public int Row { get; }

            public CheckIndex(int column, int row)
            {
                Column = column;
                Row = row;
            }
        }

        public PieceEnum? GetWinner()
        {
            PieceEnum? Winner = null;

            for (var column = 0; column <= Pieces.GetUpperBound(0); column++)
            {
                for (var row = 0; row <= Pieces.GetUpperBound(1); row++)
                {
                    Winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column + checkIndex, row));

                    if (Winner.HasValue)
                    {
                        return Winner;
                    }

                    Winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column, row + checkIndex));

                    if (Winner.HasValue)
                    {
                        return Winner;
                    }

                    Winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column + checkIndex, row + checkIndex));

                    if (Winner.HasValue)
                    {
                        return Winner;
                    }

                    Winner = CheckGroup(column, row, (column, row, checkIndex) => new CheckIndex(column - checkIndex, row + checkIndex));

                    if (Winner.HasValue)
                    {
                        return Winner;
                    }
                }
            }

            return null;
        }

        private PieceEnum? CheckGroup(int column, int row, Func<int, int, int, CheckIndex> check)
        {
            PieceEnum? lastCheck = null;

            for (var checkIndex = 0; checkIndex <= 3; checkIndex++)
            {
                var checkRowColIndex = check?.Invoke(column, row, checkIndex);
                if (checkRowColIndex == null)
                {
                    return null;
                }

                if (checkRowColIndex.Column < Pieces.GetLowerBound(0) || checkRowColIndex.Column > Pieces.GetUpperBound(0)
                    || checkRowColIndex.Row < Pieces.GetLowerBound(1) || checkRowColIndex.Row > Pieces.GetUpperBound(1))
                {
                    return null;
                }

                var thisCheck = Pieces[checkRowColIndex.Column, checkRowColIndex.Row];

                if (thisCheck == null || (checkIndex > 0 && lastCheck != thisCheck))
                {
                    return null;
                }
                lastCheck = thisCheck;
            }
            return lastCheck;
        }
    }
}
