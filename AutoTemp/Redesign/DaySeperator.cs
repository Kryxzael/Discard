using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discard.Redesign
{
    public partial class DaySeperator : UserControl
    {
        private int _daysLeft;
        public int DaysLeft
        {
            get => _daysLeft;
            set
            {
                _daysLeft = value;
                Refresh();
            }
        }

        public DaySeperator()
        {
            InitializeComponent();
        }

        public DaySeperator(int daysLeft) : this()
        {
            DaysLeft = daysLeft;
        }

        public override void Refresh()
        {
            base.Refresh();
            (pbTens.Image, pbUnits.Image) = GetDigits(DaysLeft);

            //TODO: Get an actual dynamic texture going here
            if (DaysLeft == 1)
            {
                pbDays.Image = Properties.Resources.Day;
            }
            else
            {
                pbDays.Image = Properties.Resources.Days; 
            }
        }

        private static (Bitmap tens, Bitmap units) GetDigits(int value)
        {
            //Cap value to range 0..99
            value = Math.Max(Math.Min(99, value), 0);

            //Get the digits
            return (
                tens: GetBitmapForDigit((int)Math.Floor(value / 10f), true), 
                units: GetBitmapForDigit(value % 10, false)
            );
        }

        private static Bitmap GetBitmapForDigit(int digit, bool zeroIsNull)
        {
            switch (digit)
            {
                case 0: return zeroIsNull ? null : Properties.Resources.N0;
                case 1: return Properties.Resources.N1;
                case 2: return Properties.Resources.N2;
                case 3: return Properties.Resources.N3;
                case 4: return Properties.Resources.N4;
                case 5: return Properties.Resources.N5;
                case 6: return Properties.Resources.N6;
                case 7: return Properties.Resources.N7;
                case 8: return Properties.Resources.N8;
                case 9: return Properties.Resources.N9;
                default: throw new ArgumentOutOfRangeException("More than one digit in passed number");
            }
        }
    }
}
