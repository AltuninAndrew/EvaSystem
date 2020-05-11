
namespace EvaSystem.Services.AuxiliaryHandlers
{
    public static class EmailMessageWrapper
    {
        public static string RegClientMessageWrapp(string firstUserName, string lastUserName, string email,string userPass)
        {
            string message = $"<h3>Здравствуйте, {firstUserName} {lastUserName}</h3>" +
                "<div>Вы были зарегистированы в системе оценки сотрудников EvaSystem</div>" +
                $"<div>Адресс email для входа: {email}</div>" +
                $"<div>Пароль: {userPass}</div>" +
                $"<h5>Пожалуйста, не передавайте никому эти данные. По всем вопросам обращайтесь к администрации</h5>";
               
            return message;
        }

    }
}
