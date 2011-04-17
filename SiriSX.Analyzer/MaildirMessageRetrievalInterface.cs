using System;
using System.IO;
using System.Text;

namespace SiriSX.Analyzer
{
    class MaildirMessageRetrievalInterface : MessageRetrievalInterface
    {
        DirectoryInfo newDir;
        DirectoryInfo curDir;

        public MaildirMessageRetrievalInterface()
        {
            string maildirPath;

            try
            {
                System.Configuration.AppSettingsReader settings = new System.Configuration.AppSettingsReader();
                maildirPath = (string)settings.GetValue("MaildirPath", typeof(string));
            }
            catch (InvalidOperationException)
            {
                throw new ApplicationException(
                    "Could not read MaildirPath from the configuration."
                    );
            }

            if (!Directory.Exists(maildirPath))
            {
                throw new ApplicationException(
                    "MaildirPath does not exist."
                    );
            }

            if (!Directory.Exists(Path.Combine(maildirPath, "new"))
                || !Directory.Exists(Path.Combine(maildirPath, "cur"))
               )
            {
                throw new ApplicationException(
                    "MaildirPath does not refer to a valid Maildir."
                    );
            }

            newDir = new DirectoryInfo(Path.Combine(maildirPath, "new"));
            curDir = new DirectoryInfo(Path.Combine(maildirPath, "cur"));
        }

        public AlertMessage GetNextMessage()
        {
            FileInfo[] files;
            if ((files = newDir.GetFiles()).Length > 0)
            {
                files[0].MoveTo(Path.Combine(curDir.FullName, files[0].Name + ":2,"));
                StreamReader message = new StreamReader(files[0].FullName);

                AlertMessage m = new AlertMessage();

                string s;
                while ((s = message.ReadLine()) != null)
                {
                    if (s.StartsWith("Subject: "))
                    {
                        m.Subject = s.Substring(9);
                    }
                    else if (s.StartsWith("Date: "))
                    {
                        if (s.IndexOf('(') >= 0)
                        {
                            s = s.Substring(6, s.IndexOf('(') - 6);
                        }
                        else
                        {
                            s = s.Substring(6);
                        }
                        m.Date = DateTime.Parse(s);
                    }
                    else if (string.IsNullOrEmpty(s))
                    {
                        break;
                    }
                }

                StringBuilder sb = new StringBuilder();
                while ((s = message.ReadLine()) != null)
                {
                    sb.AppendLine(s);
                }

                m.Body = sb.ToString();

                return m;
            }
            else
            {
                return null;
            }
        }
    }
}
