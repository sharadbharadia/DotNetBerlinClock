using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        private bool _sRowOn = true;
        private int _hFirstRowOn = 0;
        private int _hBottomRowOn = 0;
        private int _mFirstRowOn = 0;
        private int _mBottomRowOn = 0;

        public string ConvertTime(string aTime)
        {
            string[] timeparts = aTime.Split(':');

            Convert(int.Parse(timeparts[0]), out _hFirstRowOn, out _hBottomRowOn);

            Convert(int.Parse(timeparts[1]), out _mFirstRowOn, out _mBottomRowOn);

            _sRowOn = int.Parse(timeparts[2]) % 2 == 0;

            return ToString();
        }

        private static void Convert(int unit, out int topRow, out int bottomRow)
        {
            topRow = unit / 5;
            bottomRow = unit % 5;
        }

        public override string ToString()
        {
            return string.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}", _sRowOn ? 'Y' : 'O',
                FormatLines('R', 'O', _hFirstRowOn, 4 - _hFirstRowOn),
                FormatLines('R', 'O', _hBottomRowOn, 4 - _hBottomRowOn),
                FormatMinutesTopLine(FormatLines('Y', 'O', _mFirstRowOn, 11 - _mFirstRowOn), _mFirstRowOn, 'R'),
                FormatLines('Y', 'O', _mBottomRowOn, 4 - _mBottomRowOn)
                );
        }

        private static string FormatLines(char firstChar, char secondChar, int firstCharCount, int secondCharCount)
        {
            return string.Format("{0}{1}", new String(firstChar, firstCharCount),
                new string(secondChar, secondCharCount));
        }

        private static string FormatMinutesTopLine(string originalFormat, int minutes, char characterToReplace)
        {
            var sbFinalFormat = new StringBuilder(originalFormat);
            for (int i = 2; i < minutes; i += 3)
            {
                sbFinalFormat[i] = characterToReplace;
            }

            return sbFinalFormat.ToString();
        }

    }
}
