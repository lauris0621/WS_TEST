using System.Collections.Generic;

namespace WS_TEST_LIBRARY.Interfaces
{
    public interface ICoordinatesService
    {
        int[] Start { get; }
        int[] End { get; }
        List<char> Directions { get; }

        void SetEnd(string coordinates);
        void SetEnd(int x, int y);
        void SetCurrentPosition(string currentPosition);
        void SetCurrentPosition(int x, int y, char? direction);
        string Calculate(string directions);
    }
}
