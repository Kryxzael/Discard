using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace Discard.Commands
{
    internal class List : Command
    {
        public override string Name => "list";
        public override string HelpDescription => "Lists all selected files";

        public override Syntax GetSyntax(Params args) => Syntax.Begin();

        protected override void Executed(Params args, IConsoleOutput target)
        {
            foreach (DiscardFile i in Select.Selected)
            {
                string w;
                DiscardFile.DeconstructFileName(i.Source.Name, out _, out _, out w, out _);

                w += " - " + i.DaysLeft + " day(s)";

                if (i.NoWarning)
                    w += " - No Warning";

                if (i.HasExternalCounter)
                    w += " - External Tracker";

                if (i.Expired)
                    target.WriteError(w);
                else if (i.DaysLeft == 1)
                    target.WriteWarning(w);
                else
                    target.WriteLine(w);
            }
        }
    }

}

