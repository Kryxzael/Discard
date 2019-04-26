using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Discard
{
    public partial class DiscardManager : Form
    {
        public DiscardManager()
        {
            InitializeComponent();
            FetchIcons();
        }

        private void FetchIcons()
        {
            IEnumerable<IGrouping<int, DiscardFile>> files = Program.GetDiscardDirectories()
                .SelectMany(i => new DirectoryInfo(i).GetFileSystemInfos()
                    .Select(o => new DiscardFile(o)))
                .GroupBy(i => i.DaysLeft)
                .OrderBy(i => i.Key)
                .ToArray();

            foreach (IGrouping<int, DiscardFile> i in files)
            {
                flw.Controls.Add(new Redesign.DaySeperator(i.Key));

                foreach (DiscardFile o in i)
                {
                    flw.Controls.Add(new Redesign.FileIconControl(o));
                }
            }
        }
    }
}