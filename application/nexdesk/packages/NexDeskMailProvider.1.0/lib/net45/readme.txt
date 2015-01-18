NexDeskMailProvider mailProvider = new NexDeskMailProvider();
            Message message = new Message();
            message.Body = "<h1>hello there</h1> please visit our network <a href=\"http://nexdeskdev.azurewebsites.net/\">NexDesk</a>";
            message.IsBodyHtml = true;
            message.Subject = "Hi Library test 2 is here";
            message.Recipients.Add("johny.cheerful@gmail.com");
            mailProvider.SendMail(message);