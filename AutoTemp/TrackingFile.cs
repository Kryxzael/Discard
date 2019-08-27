using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Discard
{
    //Represents a file used to track a discard file
    public class TrackingFile
    {
        //Culture info and date format used globaly by the tracking files
        private static readonly CultureInfo CULTURE = CultureInfo.GetCultureInfo("en-US");
        private static readonly string DATE_FORMAT = "yyyy'.'MM'.'dd";

        /// <summary>
        /// The FileInfo instance of the tracking file
        /// </summary>
        public FileInfo Source { get; }

        /// <summary>
        /// Has this tracker's file expired?
        /// </summary>
        public bool Expired => DaysLeft <= 0;

        /// <summary>
        /// Gets or sets the amount of days are left until this tracker's discard file expires
        /// </summary>
        public int DaysLeft
        {
            get => (ExpiryDate.Date - DateTime.Now.Date).Days;
            set => RewriteFile(DateTime.Now.Date.AddDays(value), NoWarning, TrackingFormat);
        }

        /// <summary>
        /// Will this file be deleted without warning?
        /// </summary>
        public bool NoWarning
        {
            get => SearchForLine("nowarn");
            set => RewriteFile(ExpiryDate, value, TrackingFormat);
        }

        /// <summary>
        /// Gets or sets the type of format this tracker uses
        /// </summary>
        public TrackingFormat TrackingFormat
        {
            get
            {
                string line = ReadLineOfTracker(0);

                if (line != null)
                {
                    line = line.ToLower().Trim();

                    if (line == "infinite" || line == "endless" || line == "eternal" || line == "ignored")
                    {
                        return TrackingFormat.Infinite;
                    }
                    else if (line.EndsWith("d"))
                    {
                        return TrackingFormat.DaysLeft;
                    }
                    else if (DateTime.TryParseExact(line, DATE_FORMAT, CULTURE, DateTimeStyles.AssumeLocal, out _))
                    {
                        return TrackingFormat.Date;
                    }
                }

                return TrackingFormat.Unspecified;

            }
            set => RewriteFile(ExpiryDate, NoWarning, value);
        }

        /// <summary>
        /// Gets or sets the date this tracker's discard file is set to expire
        /// </summary>
        public DateTime ExpiryDate
        {
            get
            {
                string line = ReadLineOfTracker(0);

                if (line != null)
                {
                    line = line.Trim();

                    switch (TrackingFormat)
                    {
                        case TrackingFormat.DaysLeft:
                            if (int.TryParse(line.Substring(0, line.Length - 1), NumberStyles.Integer, CULTURE, out int i))
                            {
                                return DateTime.Now.Date.AddDays(i);
                            }

                            break;
                        case TrackingFormat.Date:
                            if (DateTime.TryParseExact(line, DATE_FORMAT, CULTURE, DateTimeStyles.AssumeLocal, out DateTime dt))
                            {
                                return dt;
                            }

                            break;
                        case TrackingFormat.Infinite:
                            return DateTime.MaxValue;
                    }
                }

                return DateTime.Now.AddDays(Properties.Settings.Default.DefaultDays);
            }
            set => RewriteFile(value, NoWarning, TrackingFormat);
        }

        /// <summary>
        /// Rewrites the tracking file
        /// </summary>
        /// <param name="expiryDate"></param>
        private void RewriteFile(DateTime expiryDate, bool noWarn, TrackingFormat format)
        {
            StringBuilder builder = new StringBuilder();

            //Counter
            switch (format)
            {
                case TrackingFormat.DaysLeft:
                    builder.AppendLine((expiryDate.Date - DateTime.Now.Date).Days.ToString(CULTURE) + "d");
                    break;
                case TrackingFormat.Date:
                    builder.AppendLine(expiryDate.ToString(DATE_FORMAT, CULTURE));
                    break;
                case TrackingFormat.Infinite:
                    builder.AppendLine(ReadLineOfTracker(0) ?? "infinite");
                    break;
            }

            //No warning file
            if (noWarn)
            {
                builder.AppendLine("nowarn");
            }

            try
            {
                File.WriteAllText(Source.FullName, builder.ToString());
                //File.SetAttributes(Source.FullName, File.GetAttributes(Source.FullName) | FileAttributes.Hidden);
                
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to write tracking file '" + Source.Name + "'");
            }

        }

        /// <summary>
        /// Creates a new tracking file
        /// </summary>
        /// <param name="source"></param>
        public TrackingFile(FileInfo source)
        {
            Source = source;
        }

        /*
         * UTIL
         */

        /// <summary>
        /// Reads all the lines of the tracking file
        /// </summary>
        /// <returns></returns>
        private string[] ReadAllLinesOfTracker()
        {
            AsureCreationOfTrackingFile();

            try
            {
                return File.ReadAllLines(Source.FullName);
            }
            catch (Exception)
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Returns the text at a specific line of the tracker file (or null if out of bounds)
        /// </summary>
        /// <param name="lineNr"></param>
        /// <returns></returns>
        private string ReadLineOfTracker(int lineNr)
        {
            AsureCreationOfTrackingFile();
            return ReadAllLinesOfTracker().ElementAtOrDefault(lineNr);
        }

        /// <summary>
        /// Searches for a specific line of text in the tracker file
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool SearchForLine(string line)
        {
            foreach (string i in ReadAllLinesOfTracker())
            {
                if (i.ToLower() == line.ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Asures that the tracker exists, creating it if it doesn't
        /// </summary>
        private void AsureCreationOfTrackingFile()
        {
            if (!Source.Exists)
            {
                //TODO: Create tracking file
            }
        }

    }

    /// <summary>
    /// Represents a way for trackers to format their countdown
    /// </summary>
    public enum TrackingFormat
    {
        /// <summary>
        /// No format is specified, will default to days left
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// The tracker counter is formated as the amount of days remaining
        /// </summary>
        DaysLeft = 0,

        /// <summary>
        /// The tracker counter is formated as a specific date
        /// </summary>
        Date = 1,

        /// <summary>
        /// The tracker counter has been set to be infinite
        /// </summary>
        Infinite = 2
    }
}
