SiriSX.Analyzer.exe: AssemblyInfo.cs Main.cs MessageRetrievalInterface.cs AmazonSQSMessageRetrievalInterface.cs MaildirMessageRetrievalInterface.cs
	gmcs -out:$@ -lib:/usr/lib/mono/AWSSDK -lib:/usr/lib/mono/SharpMimeTools -r:AWSSDK.dll -r:SharpMimeTools.dll $^
