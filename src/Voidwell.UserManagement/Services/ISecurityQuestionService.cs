using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Voidwell.UserManagement.Models;

namespace Voidwell.UserManagement.Services
{
    public interface ISecurityQuestionService
    {
        IEnumerable<string> GetSecurityQuestionsList();
        Task<IEnumerable<SecurityQuestion>> GetSecurityQuestions(Guid userId);
        Task<IEnumerable<SecurityQuestion>> CreateSecurityQuestions(Guid userId, IEnumerable<SecurityQuestion> securityQuestions);
        Task RemoveSecurityQuestions(Guid userId);
        void ValidateSecurityQuestions(IEnumerable<SecurityQuestion> questions);
        Task<IEnumerable<SecurityQuestion>> GetSecurityQuestionsByEmail(string email);
    }
}
