using System.Collections.Generic;

namespace Minefield
{
    public interface IPlayer
    {
        List<BoardLocation> HistoricPositions { get; }

        int NumberOfLives { get; }

        BoardLocation CurrentBoardLocation { get; set; }

        bool CanPlay { get; }
    }
}
