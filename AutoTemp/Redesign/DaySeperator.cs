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
                pbDays.Image = null;
            }
            else
            {
                pbDays.Image = null; 
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
            if (digit > 9 || digit < 0)
            {
                throw new ArgumentOutOfRangeException("More than one digit in passed number");
            }

            //TODO: Implement this
            return null;
        }
    }
}
