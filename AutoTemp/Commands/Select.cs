using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserConsoleLib;

namespace Discard.Commands
{
    internal class Select : Command
    {
        public static IEnumerable<DiscardFile> Selected { get; private set; } = Enumerable.Empty<DiscardFile>();

        public override string Name => "select";
        public override string HelpDescription => "Selects a set of files";

        public override Syntax GetSyntax(Params args) =>
            Syntax.Begin()
            .AddTrailing("quoted file name").Or()
            .Add("days left", Range.INFINITY, true).Or()
            .Add("min days left", Range.INFINITY, true).Add("max days left", Range.INFINITY, true).Or()
            .Add("special name", "all", "untracked", "tracked", "expired", "not-expired", "no-warn", "warn", "external-tracker", "baked-tracker", "clean", "dirty");

        protected override void Executed(Params args, IConsoleOutput target)
        {
            try
            {
                DiscardCycle discard = DiscardCycle.DryRun(Program.GetDiscardDirectories()
                .Select(i => new DirectoryInfo(i)), 0);

                if (args[0].StartsWith("\""))
                {
                    foreach (DiscardFile i in discard.DiscardFiles)
                    {
                        DiscardFile.DeconstructFileName(i.Source.Name, out _, out _, out string name, out _);

                        if (name.ToUpper() == args.JoinEnd(0).Trim('"').ToUpper())
                        {
                            Selected = Enumerable.Repeat(i, 1);
                            return;
                        }
                    }
                }
                else if (args.Count == 1)
                {
                    switch (args[0])
                    {
                        case "all":
                            Selected = discard.DiscardFiles;
                            return;
                        case "untracked":
                            Selected = discard.DiscardFiles.Where(i => i.Untracked);
                            return;
                        case "tracked":
                            Selected = discard.DiscardFiles.Where(i => !i.Untracked);
                            return;
                        case "expired":
                            Selected = discard.DiscardFiles.Where(i => i.Expired);
                            return;
                        case "not-expired":
                            Selected = discard.DiscardFiles.Where(i => !i.Expired);
                            return;
                        case "no-warn":
                            Selected = discard.DiscardFiles.Where(i => i.NoWarning);
                            return;
                        case "warn":
                            Selected = discard.DiscardFiles.Where(i => !i.NoWarning);
                            return;
                        case "external-tracker":
                            Selected = discard.DiscardFiles.Where(i => i.HasExternalCounter);
                            return;
                        case "baked-tracker":
                            Selected = discard.DiscardFiles.Where(i => !i.HasExternalCounter);
                            return;
                        case "clean":
                            Selected = discard.DiscardFiles.Where(i => (i.Source is DirectoryInfo d && !d.GetFileSystemInfos().Any()) || (i.Source is FileInfo f && f.Length == 0));
                            return;
                        case "dirty":
                            Selected = discard.DiscardFiles.Where(i => !((i.Source is DirectoryInfo d && !d.GetFileSystemInfos().Any()) || (i.Source is FileInfo f && f.Length == 0)));
                            return;
                        default:
                            if (args.IsInteger(0))
                            {
                                Selected = discard.DiscardFiles.Where(i => i.DaysLeft == args.ToInt(0));
                                return;
                            }
                            break;
                    }
                }
                else
                {
                    if (args.IsInteger(0) && args.IsInteger(1))
                    {
                        Selected = discard.DiscardFiles.Where(i => i.DaysLeft >= args.ToInt(0) && i.DaysLeft <= args.ToInt(1));
                        return;
                    }
                }

                ThrowSyntaxError(this, args, ErrorCode.ARGUMENT_INVALID);
            }
            finally
            {
                Command.ParseLine("list", target);
            }
        }
    }
}
