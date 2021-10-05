using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace Discard.Commands
{
    internal class SetDays : Command    
    {
        public override string Name => "setdays";
        public override string HelpDescription => "Sets the amount of days left one or more files have";

        public override Syntax GetSyntax(Params args) => Syntax.Begin().Add("Days left", Range.INFINITY, true);

        protected override void Executed(Params args, IConsoleOutput target)
        {
            foreach (DiscardFile i in Select.Selected)
                i.DaysLeft = args.ToInt(0);
        }
    }
}
