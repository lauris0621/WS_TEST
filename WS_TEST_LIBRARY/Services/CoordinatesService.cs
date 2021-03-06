﻿using System;
using System.Collections.Generic;
using System.Linq;
using WS_TEST_LIBRARY.Interfaces;

namespace WS_TEST_LIBRARY.Services
{
    public class CoordinatesService : ICoordinatesService
    {
        private int[] _currentPosition;
        private char? _currentDirection;

        public CoordinatesService()
        {
            Start = new int[2] { 0, 0 };
            Directions = new List<char> { 'N', 'E', 'S', 'W' };
        }

        public int[] Start { get; }

        public int[] End { get; private set; }

        public List<char> Directions { get; }

        public string Calculate(string directions)
        {
            if (Start == null || End == null)
            {
                throw new Exception("needed to init start and end coordinates");
            }

            if (string.IsNullOrEmpty(directions))
            {
                throw new Exception("directions can't be empty");
            }

            foreach (var chr in directions.ToCharArray())
            {
                // Calculate next position depeing on route
                CalculateNextPosition(chr);
            }

            // return the result
            return string.Format("{0} {1} {2}", _currentPosition[0], _currentPosition[1], _currentDirection);
        }

        public void SetCurrentPosition(string currentPosition)
        {
            if (string.IsNullOrEmpty(currentPosition))
            {
                throw new Exception("current position can't be empty");
            }
            var currentCoordinates = currentPosition.Trim().Split(' ');

            if (currentCoordinates.Length != 3)
            {
                throw new Exception("current position needs x, y and direction");
            }

            int.TryParse(currentCoordinates[0], out var x);
            int.TryParse(currentCoordinates[1], out var y);

            if (x < 0 || y < 0)
            {
                throw new Exception("x and y are missing");
            }

            var currentDirection = currentCoordinates[2];

            if (currentDirection.Length != 1)
            {
                throw new Exception("direction is missing");
            }

            // set current position and direction
            SetCurrentPosition(x, y, currentDirection.ToCharArray()[0]);
        }

        public void SetCurrentPosition(int x, int y, char? direction)
        {
            if (x < 0 || y < 0 || x > End[0] || y > End[1])
            {
                throw new Exception("x and y cannot be lower than 0 or higher than End position");
            }

            if (!Directions.Any(x => x == direction))
            {
                throw new Exception("position can be N, E, S or W");
            }

            // set current position and direction
            _currentPosition = new int[2] { x, y };
            _currentDirection = direction;
        }

        public void SetEnd(string coordinates)
        {
            if (string.IsNullOrEmpty(coordinates))
            {
                throw new Exception("max coordinates can't be empty");
            }

            var maxCoordinates = coordinates.Trim().Split(' ');

            if (maxCoordinates.Length != 2)
            {
                throw new Exception("format of max coordinates is \"x y\", example \"5 5\"");
            }

            int.TryParse(maxCoordinates[0], out var x);
            int.TryParse(maxCoordinates[1], out var y);

            if (x <= 0 && y <= 0)
            {
                throw new Exception("x and y can't be less than or equal to 0 and must be numbers");
            }

            // Set max coordinates
            SetEnd(x, y);
        }

        public void SetEnd(int x, int y)
        {
            if (x <= 0 || y <= 0)
            {
                throw new Exception("x and y coordinates have to be higher than 0");
            }

            // Set max coordinates
            End = new int[2] { x, y };
        }

        private void CalculateNextPosition(char? chr)
        {
            switch (chr)
            {
                case 'L':
                case 'R':
                    // Left or Right calculates next direction
                    CalculateDirection(chr);
                    break;
                case 'M':
                    // Move moves by 1 depending on direction
                    CalculatePosition();
                    break;
            }
        }

        private void CalculatePosition()
        {
            switch (_currentDirection)
            {
                case 'N':
                    // North adds 1 to y
                    if (_currentPosition[1] < End[1])
                    {
                        _currentPosition[1]++;
                    }
                    break;
                case 'E':
                    // East adds 1 to x
                    if (_currentPosition[0] < End[0])
                    {
                        _currentPosition[0]++;
                    }
                    break;
                case 'S':
                    // South subtracts 1 from y
                    if (_currentPosition[1] > Start[1])
                    {
                        _currentPosition[1]--;
                    }
                    break;
                case 'W':
                    // West subtracts 1 from x
                    if (_currentPosition[0] > Start[0])
                    {
                        _currentPosition[0]--;
                    }
                    break;
            }
        }

        private void CalculateDirection(char? chr)
        {
            var directionIndex = Directions.IndexOf(_currentDirection.Value);

            switch (chr)
            {
                case 'L':
                    directionIndex--;
                    break;
                case 'R':
                    directionIndex++;
                    break;
            }

            // if less than 0 returns to last entry in the list
            if (directionIndex < 0)
            {
                directionIndex = Directions.Count - 1;
            }
            // if more than list length returns to first entry in the list
            else if (directionIndex > Directions.Count - 1)
            {
                directionIndex = 0;
            }

            // set current direction
            _currentDirection = Directions[directionIndex];
        }
    }
}
