using System;
using WS_TEST_LIBRARY.Interfaces;
using WS_TEST_LIBRARY.Services;
using Xunit;

namespace WS_TEST_LIBRARY_TESTS
{
    public class CoordinatesServiceTests
    {
        private readonly ICoordinatesService _coordinatesService;

        public CoordinatesServiceTests()
        {
            _coordinatesService = new CoordinatesService();
        }

        [Fact]
        public void CheckSetup()
        {
            Assert.Equal(2, _coordinatesService.Start.Length);
            Assert.Equal(0, _coordinatesService.Start[0]);
            Assert.Equal(0, _coordinatesService.Start[1]);

            Assert.Equal(4, _coordinatesService.Directions.Count);
            Assert.Equal('N', _coordinatesService.Directions[0]);
            Assert.Equal('E', _coordinatesService.Directions[1]);
            Assert.Equal('S', _coordinatesService.Directions[2]);
            Assert.Equal('W', _coordinatesService.Directions[3]);
        }

        [Fact]
        public void SetEnd_String()
        {
            _coordinatesService.SetEnd("1 5");
            Assert.Equal(2, _coordinatesService.End.Length);
            Assert.Equal(1, _coordinatesService.End[0]);
            Assert.Equal(5, _coordinatesService.End[1]);
        }

        [Fact]
        public void SetEnd_String_Incorrect()
        {
            var ex = Assert.Throws<Exception>(() => _coordinatesService.SetEnd(""));
            Assert.Equal("max coordinates can't be empty", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetEnd("1"));
            Assert.Equal("format of max coordinates is \"x y\", example \"5 5\"", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetEnd("1 5 9"));
            Assert.Equal("format of max coordinates is \"x y\", example \"5 5\"", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetEnd("-1 -5"));
            Assert.Equal("x and y can't be less than or equal to 0 and must be numbers", ex.Message);
        }

        [Fact]
        public void SetEnd_XY()
        {
            _coordinatesService.SetEnd(1, 5);
            Assert.Equal(2, _coordinatesService.End.Length);
            Assert.Equal(1, _coordinatesService.End[0]);
            Assert.Equal(5, _coordinatesService.End[1]);
        }

        [Fact]
        public void SetEnd_Incorrect()
        {
            var ex = Assert.Throws<Exception>(() => _coordinatesService.SetEnd(-1, 5));
            Assert.Equal("x and y coordinates have to be higher than 0", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetEnd(1, -5));
            Assert.Equal("x and y coordinates have to be higher than 0", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetEnd(-1, -5));
            Assert.Equal("x and y coordinates have to be higher than 0", ex.Message);
        }

        [Fact]
        public void SetCurrentPosition_String()
        {
            _coordinatesService.SetEnd("5 5");
            _coordinatesService.SetCurrentPosition("1 5 N");
        }

        [Fact]
        public void SetCurrentPosition_String_Incorrect()
        {
            _coordinatesService.SetEnd("5 5");

            var ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition(""));
            Assert.Equal("current position can't be empty", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition("1 5"));
            Assert.Equal("current position needs x, y and direction", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition("1 -5 N"));
            Assert.Equal("x and y are missing", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition("1 5 NE"));
            Assert.Equal("direction is missing", ex.Message);
        }

        [Fact]
        public void SetCurrentPosition_XYD()
        {
            _coordinatesService.SetEnd("5 5");
            _coordinatesService.SetCurrentPosition(1, 5, 'N');
        }

        [Fact]
        public void SetCurrentPosition_XYD_Incorrect()
        {
            _coordinatesService.SetEnd("5 5");

            var ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition(-1, 5, 'N'));
            Assert.Equal("x and y cannot be lower than 0 or higher than End position", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition(1, -5, 'N'));
            Assert.Equal("x and y cannot be lower than 0 or higher than End position", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition(6, 5, 'N'));
            Assert.Equal("x and y cannot be lower than 0 or higher than End position", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition(1, 6, 'N'));
            Assert.Equal("x and y cannot be lower than 0 or higher than End position", ex.Message);

            ex = Assert.Throws<Exception>(() => _coordinatesService.SetCurrentPosition(1, 5, 'R'));
            Assert.Equal("position can be N, E, S or W", ex.Message);
        }

        [Fact]
        public void Calculate_Option1()
        {
            _coordinatesService.SetEnd(5, 5);
            _coordinatesService.SetCurrentPosition(1, 2, 'N');
            var result = _coordinatesService.Calculate("LMLMLMLMM");

            Assert.Equal("1 3 N", result);
        }

        [Fact]
        public void Calculate_Option2()
        {
            _coordinatesService.SetEnd(5, 5);
            _coordinatesService.SetCurrentPosition(3, 3, 'E');
            var result = _coordinatesService.Calculate("MMRMMRMRRM");

            Assert.Equal("5 1 E", result);
        }
    }
}
