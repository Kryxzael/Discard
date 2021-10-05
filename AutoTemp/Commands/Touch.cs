using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace Discard.Commands
{
    internal class Touch : Command
    {

        public override string Name => "touch";
        public override string HelpDescription => "creates a new file in the default discard directory";

        public override Syntax GetSyntax(Params args) => Syntax.Begin()
            .Add("file name").Or()
            .Add("file name").Add("days", Range.INFINITY, true).Or()
            .Add("file name").Add("days", Range.INFINITY, true).Add("no-warn", true);

        protected override void Executed(Params args, IConsoleOutput target)
        {
            if (Program.GetDiscardDirectories().Count() == 0)
                ThrowGenericError("No discard directories set up. Do this first", ErrorCode.INVALID_CONTEXT);

            DiscardFile file = new DiscardFile(new FileInfo(Path.Combine(Program.GetDiscardDirectories().First(), args[0])));

            if (file.Source.Exists)
                ThrowGenericError("Cannot create pre-existing file", ErrorCode.FILE_LOCKED);

            try
            {
                (file.Source as FileInfo).Create().Close();
            }
            catch (Exception)
            {
                ThrowGenericError("Unable to touch file", ErrorCode.FILE_ACCESS_DENIED);
            }

            if (args.Count >= 2)
                file.DaysLeft = args.ToInt(1);

            if (args.Count >= 3)
                file.NoWarning = args.ToBoolean(2);
        }
    }
}
