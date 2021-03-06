﻿namespace EvaSystem.Contracts
{
    public static class ApiRoutes
    {
        public static class Identity
        {
            public const string Login = "api/identity/login";
            public const string RegisterAdmin = "api/identity/admin_register";
            public const string RegisterClient = "api/identity/client_register";
            public const string GetAllUsers = "api/identity/get_all_users";
            public const string GetUserInfo = "api/identity/get_user_info/{username}";
            public const string CheckJWT = "api/identity/check_jwt";


        }

        public static class ClientData
        {
            public const string ChangePassword = "api/ClientData/change_password/{username}";
            public const string ChangeEmail = "api/ClientData/change_email/{username}";
            public const string ChangePosition = "api/ClientData/change_position/{username}";
            public const string ChangeFirstName = "api/ClientData/change_first_name/{username}";
            public const string ChangeMiddleName = "api/ClientData/change_middle_name/{username}";
            public const string ChangeLastName = "api/ClientData/change_last_name/{username}";
            public const string AddUserAvatar = "api/ClientData/add_user_avatar/{username}";
            public const string RemoveUserAvatar = "api/ClientData/remove_user_avatar/{username}";
            public const string DeleteUser = "api/ClientData/delete_user/{username}";
            public const string GetUsersForInteract = "api/ClientData/get_users_for_interact/{username}";
            public const string AddCommunicationBtwUsers = "api/ClientData/add_interected_users/{username}";
            public const string GetInterectedUsers = "api/ClientData/get_interected_users/{username}";
            public const string GetInteractedUsersWithCrits = "api/ClientData/get_interacted_users_with_crits/{username}";
            public const string DeleteСommunicationBtwUsers = "api/ClientData/delete_interected_users/{username}";
            public const string DeleteUserFromInterectedUsersTable = "api/ClientData/delete_all_communications/{username}";
        }


        public static class Evaluation
        {
            public const string AddCriterions = "api/Evaluation/add_criterions";
            public const string GetCriterions = "api/Evaluation/get_criterions/{positionName}";
            public const string GetCriterionsForUser = "api/Evaluation/get_criterions_for_user/{username}";
            public const string DeleteCriterionsForPos = "api/Evaluation/delete_criterions/{positionName}";
            public const string RateUser = "api/Evaluation/rate_user/{username}";
            public const string GetUserRating = "api/Evaluation/get_user_rating/{username}";
            public const string RemoveUserRating = "api/Evaluation/remove_user_rating/{username}";
            public const string GetAllPositions = "api/Evaluation/get_all_positions";
        }
    
    }
}
