using System;
using System.IO;
using System.Text;
using anmar.SharpMimeTools;

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
                SharpMessage message;
                using (FileStream stream = new FileStream(files[0].FullName, FileMode.Open))
                {
                    message = new SharpMessage(stream);
                }

                AlertMessage m = new AlertMessage();
                m.Date = message.Date;
                m.Subject = message.Subject;
                m.Body = message.Body;

                return m;
            }
            else
            {
                return null;
            }
        }
    }
}
